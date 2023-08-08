import fs from "fs"
import clone from "git-clone/promise"
import { execWrap, mkdir } from "./util";

export class SteamworksDumper {
    constructor() {
       
    }
    isSetup(): boolean {
        return fs.existsSync("steamworks_dumper") && fs.existsSync("steamworks_dumper/build/steamworks_dumper");
    }
    async dump(clientpath: string, outpath: string): Promise<void> {
        if (!this.isSetup()) {
            throw "SteamworksDumper not setup";
        }
        
        if (outpath.length == 0) {
            throw "outpath cannot be empty";
        }
        if (clientpath.length == 0) {
            throw "clientpath cannot be empty"
        }

        mkdir(clientpath, true);
        try {
            await execWrap(`steamworks_dumper/build/steamworks_dumper --dump-offsets "${clientpath}" "${outpath}"`, {});
        } catch (error) {
            throw "Failed to execute compiled binary: " + error;
        } finally {
            return;
        }
    }
    async setup(): Promise<void> {
        if (fs.existsSync("steamworks_dumper")) {
            if (fs.existsSync("steamworks_dumper/build/steamworks_dumper")) {
                throw "SteamworksDumper already setup.";
            }  
        } else {
            console.info("Downloading m4dEngi/steamworks_dumper git repo")
            try {
                await clone("https://github.com/m4dEngi/steamworks_dumper.git", "steamworks_dumper");
            } catch (e) {
                throw "Failed to download m4dEngi/steamworks_dumper " + e;
            } finally {
                console.log("Downloaded m4dEngi/steamworks_dumper successfully");
            }
        }

        console.info("Compiling steamworks_dumper")

        try {
            fs.mkdirSync("steamworks_dumper/build")
        } catch (e) {}
        
        try {
            await execWrap('cmake ..', {
                cwd: 'steamworks_dumper/build',
            });
        } catch (error) {
            throw "Failed to run cmake for build file generation: " + error;
        } finally {
            try {
                await execWrap('cmake --build .', {
                    cwd: 'steamworks_dumper/build',
                });
            } catch (error) {
                throw "Failed to run cmake for compilation: " + error;
            } finally {
                console.info("Compilation finished")
            }
        }
    }
}