import fs from 'fs';
import clone from 'git-clone/promise';
import { execWrap, mkdir } from '../util';

export async function ProcessProtobuf(workdir: string, targetdir: string) {
    console.info("Downloading SteamDatabase/Protobufs git repo")
    try {
        await clone("https://github.com/SteamDatabase/Protobufs.git", workdir);
    } catch (e) {
        throw "Failed to download SteamDatabase/Protobufs " + e;
    } finally {
        console.log("Downloaded SteamDatabase/Protobufs successfully");
    }
    
    console.info("Compiling protobuf")
    
    var wantedProtobufs = `${workdir}/steam`
    var allFilesRelative = fs.readdirSync(wantedProtobufs, {
        recursive: false,
    });

    var allFiles: string[] = [];
    allFilesRelative.forEach(file => {
        if (fs.lstatSync(`${wantedProtobufs}/${file}`).isFile()) {
            allFiles.push(`${wantedProtobufs}/${file}`)
        }
    });

    var allFilesStr = "";
    allFiles.forEach(fullpath => {
        allFilesStr += '"' + fullpath + '"' + " ";
    });

    mkdir(targetdir);

    try {
        await execWrap(`protoc -I="${wantedProtobufs}" --csharp_out="${targetdir}" ${allFilesStr}`, {});
    } catch (error) {
        throw "Failed to run protoc for C# protobuf file generation: " + error;
    }
}