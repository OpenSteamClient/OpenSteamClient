import path from 'path';
import fs from 'fs';
import clone from 'git-clone/promise';
import { execWrap, mkdir } from '../util';

export async function ProcessProtobuf(projdir: string, workdir: string, targetdircsharp: string, targetdircpp: string) {
    console.info("Downloading SteamDatabase/Protobufs git repo")
    try {
        await clone("https://github.com/SteamDatabase/Protobufs.git", workdir);
    } catch (e) {
        throw "Failed to download SteamDatabase/Protobufs " + e;
    } finally {
        console.log("Downloaded SteamDatabase/Protobufs successfully");
    }
    
    console.info("Compiling protobuf")
    
    var wantedProtobufsSteam = `${workdir}/steam`
    var wantedProtobufsWebui = `${workdir}/webui`
    var steamFilesRelative: string[] = fs.readdirSync(wantedProtobufsSteam, {
        recursive: false,
        encoding: "utf-8"
    });

    var webuiFilesRelative: string[] = fs.readdirSync(wantedProtobufsWebui, {
        recursive: false,
        encoding: "utf-8"
    });

    var allFilesRelative = steamFilesRelative.concat(webuiFilesRelative);

    var allFiles: string[] = [];
    var webuiFiles: string[] = [];
    var steamFiles: string[] = [];
    steamFilesRelative.forEach(file => {
        if (fs.lstatSync(`${wantedProtobufsSteam}/${file}`).isFile()) {
            allFiles.push(`${wantedProtobufsSteam}/${file}`);
            steamFiles.push(`${wantedProtobufsSteam}/${file}`);
        }
    });

    // The growing list of erroring files is worrying... Seriously.
    const blacklistedFiles: string[] = ["service_steamvrvoicechat.proto", "service_steamvrwebrtc.proto", "service_cloud.proto", "service_transportvalidation.proto", "service_accountcart.proto", "service_checkout.proto"];

    webuiFilesRelative.forEach(file => {
        // These files cause compile errors, they're blacklisted
        if (!blacklistedFiles.includes(file)) {
            if (fs.lstatSync(`${wantedProtobufsWebui}/${file}`).isFile()) {
                allFiles.push(`${wantedProtobufsWebui}/${file}`);
                webuiFiles.push(`${wantedProtobufsWebui}/${file}`);
            }
        }
    });

    var steamFilesStr = "";
    steamFiles.forEach(fullpath => {
        steamFilesStr += '"' + fullpath + '"' + " ";
    });

    var webuiFilesStr = "";
    webuiFiles.forEach(fullpath => {
        webuiFilesStr += '"' + fullpath + '"' + " ";
    });

    mkdir(targetdircsharp);
    mkdir(targetdircpp);

    // Process all files before compiling
    // Very simple steps:
    allFiles.forEach(file => {
        // Read the file, replacing all newlines with linux style ones, then splitting by them
        var lines: string[] = fs.readFileSync(file).toString().replace(/\r\n/g, '\n').split("\n");
        var namespace: string = "OpenSteamworks.Protobuf";
        var isWebui: boolean = false;
        if (path.basename(path.dirname(file)).includes("webui")) {
            namespace += ".WebUI";
            isWebui = true;
        }
        
        var existingOpenSteamworksReference = lines.findIndex(line => line.includes("OpenSteamworks.Protobuf"));
        if (existingOpenSteamworksReference == -1) {
            // 1. Add a 'syntax = "proto2";' as the first line
            lines.unshift('syntax = "proto2";');
            
            // 2. Define a 'option csharp_namespace = "OpenSteamworks.Protobufs";' after the line 'import "google/protobuf/descriptor.proto";'
            var descriptorLine = lines.findIndex(s => s.includes('import "google/protobuf/descriptor.proto";'));
            console.log("DescriptorLine = " + descriptorLine);
            if (descriptorLine == -1) {
                // Add a descriptor import if it doesn't exist
                lines.splice(1, 0, 'import "google/protobuf/descriptor.proto";');
                descriptorLine = 1;
            }
            lines.splice(descriptorLine + 1, 0, `option csharp_namespace = "${namespace}";`);
            
            // 3. Remove all instances of 'k_'. This should help enums be named a little better, however the enum names are all over the place so some names will still look dumb
            lines.forEach(function (part, index, theArray) {
                if (part.includes("k_")) {
                    console.log("Replacing '" + part + "' with '" + part.replaceAll("k_", '') + "'");
                    theArray[index] = part.replaceAll("k_", '');
                }
            });

            // Save and override old file
            fs.writeFileSync(file, Buffer.from(lines.join("\n")));
        } else {
            console.warn("File " + file + " skipped processing, contained text OpenSteamworks.Protobuf")
        }
    });

    try {
        await execWrap(`protoc -I="${wantedProtobufsSteam}" --csharp_out="${targetdircsharp}" ${steamFilesStr}`, {});
        await execWrap(`protoc -I="${wantedProtobufsWebui}" --csharp_out="${targetdircsharp}" ${webuiFilesStr}`, {});
    } catch (error) {
        throw "Failed to run protoc for C# protobuf file generation: " + error;
    }

    try {
        // DO NOT REMOVE, protobufhack builds need this very specific protoc instead of the system one
        await execWrap(projdir+`/OpenSteamworks.Native/oldprotoc/build/protoc -I="${projdir+'/OpenSteamworks.Native/oldprotoc/build/external.protobuf/include/'}" -I="${wantedProtobufsSteam}" --cpp_out="${targetdircpp}" ${steamFilesStr}`, {});
    } catch (error) {
        throw "Failed to run protoc for C++ protobuf file generation: " + error;
    }
    
}