import { exec, SpawnOptionsWithoutStdio } from 'child_process';
import fs from "fs";
import pathlib from "path";
import util from "util";
import { spawn } from "child_process";

export function mkdir(path: string, recursive: boolean = false): void {
    if (!fs.existsSync(path)){
        fs.mkdirSync(path, recursive ? {} : {recursive: true});
    }
}

export function rm(path: string): void {
    if (fs.existsSync(path)){
        fs.rmSync(path);
    }
}


//TODO: this doesn't wait for the process to finish executing, even though it should
export function execWrap(command: string, options: SpawnOptionsWithoutStdio): Promise<string> {
    return new Promise<string>((resolve, reject) => {
        var cmd = spawn(command, { ...options, shell: true });
        var alldata: string = "";
        cmd.stdout.on('data', function (data) {
            process.stdout.write(data.toString());
            alldata += data.toString();
        });
        
        cmd.stderr.on('data', function (data) {
            process.stdout.write(data.toString());
            alldata += data.toString();
        });
        
        cmd.on('exit', function (code) {
            if (code == null) {
                reject("code is null???");
            } else {
                if (code != 0) {
                    reject("command exited with non-zero exit code " + code);
                } else {
                    resolve(alldata);
                }
                console.log('child process exited with code ' + code.toString());
            }
        });

        // var exechandle = exec(command, options, ((error, stdout, stderr) => {
        //     if (error) {
        //         reject(error);
        //         return;
        //     }
        //     resolve(stdout);
        // }));
        // exechandle.stdout?.pipe(process.stdout);
        // exechandle.stderr?.pipe(process.stderr);
        // if (exechandle.stdin) 
        //     process.stdin.pipe(exechandle.stdin);
    })
}

export function readdirRecursiveSync(path: string): string[] {
    var dirs: string[] = [];
    const dirents = fs.readdirSync(path, { withFileTypes: true });
    for (const dirent of dirents) {
        const resolved = pathlib.resolve(path, dirent.name);
        dirs.push(resolved);
        if (dirent.isDirectory()) {
            dirs = dirs.concat(readdirRecursiveSync(resolved));
        }
    }
    return dirs;
}

export function find32BitELFBinaryRecursive(path: string, filename: string, bitness: "32" | "64" = "32"): Promise<string> {
    return new Promise<string>(async (resolve, reject) => {
        for (var file of readdirRecursiveSync(path)) {
            if (file.endsWith(filename)) {
                if ((await execWrap("file " + file, {})).includes("ELF "+bitness+"-bit")) {
                    resolve(file);
                }
            }
        }     
        reject(`Didn't find ${filename}`)
    })
}

export function CreateDifferenceObject<T>(): Difference<T> {
    return { additions: [], removals: [], changes: [], isDifferent: false }
}

export interface Difference<T> {
    isDifferent: boolean;
    additions: T[];
    removals: T[];
    changes: T[];
}

export function replaceRange(s: string, start: number, end: number, substitute: string) {
    return s.substring(0, start) + substitute + s.substring(end);
}

// If you pass objects to this function, use comparer or it will give false results
export function compareArrays<T>(_originalArr: T[], _newArr: T[], comparer?: (searchItem: T, currentItem: T) => boolean, changeComparer?: (origItem: T, newItem: T) => boolean): Difference<T> {
    
    // Clones the arrays. This is dumb. Why not just have pointers.
    var originalArr: any[] = JSON.parse(JSON.stringify(_originalArr));
    var newArr: any[] = JSON.parse(JSON.stringify(_newArr));

    // Store the differences here
    var diff: Difference<T> = CreateDifferenceObject();

    // Loop over the original array and see:
    for (let orig_i = 0; orig_i < originalArr.length; orig_i++) {
        const originalItem = originalArr[orig_i];

        var new_i: number = -1;
        if (comparer) {
            for (var i = 0, len = newArr.length; i < len; i++) {
                if (comparer(originalItem, newArr[i])) {
                    new_i = i;
                    break;
                }
            }
        } else {
            new_i = newArr.indexOf(originalItem);
        }
        
        var newItem: T; 

        // - if the same item exists in the new array
        if (new_i != -1) {
            // Exists, remove from new array but store for change comparing
            newItem = newArr[new_i];
            newArr = newArr.splice(new_i+1);
        } else {
            // Doesn't exist, it's a removal
            diff.isDifferent = true;
            diff.removals.push(originalItem);
            continue;
        }

        // - if the item exists, but has changed
        if (changeComparer && changeComparer(originalItem, newItem)) {
            // item has changed
            diff.isDifferent = true;
            diff.changes.push(originalItem);
        }
    }

    // All non-removed items are additions
    for (const newItem of newArr) {
        diff.isDifferent = true;
        diff.additions.push(newItem);
    }

    return diff;
}