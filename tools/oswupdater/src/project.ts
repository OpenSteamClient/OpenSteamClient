import fs from "fs"
import path from "path"

export namespace Project {
    var cachedProjectDir: string = "";
    export function GetProjectDirectory(): string {
        if (cachedProjectDir != "")
            return cachedProjectDir;

        // Go up recursively until we find CMakeLists.txt
        var found: boolean = false;
        var currentPath: string = __dirname;
        while (!found) {
            found = fs.existsSync(currentPath + "/CMakeLists.txt");
            if (!found) {
                currentPath = path.resolve(currentPath + "/../");
                if (currentPath == "/") {
                    throw "Couldn't find CMakeLists.txt in parent directories of this script. "
                }
            }
        }
        cachedProjectDir = currentPath;
        return currentPath;
    }
}