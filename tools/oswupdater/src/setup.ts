import { Project } from "./project";
import { SteamworksDumper } from "./steamworksdumper";
import { execWrap, mkdir } from "./util";

async function main(): Promise<void> {
    var projdir = Project.GetProjectDirectory();

    let dumper = new SteamworksDumper();
    dumper.setup();

    mkdir(projdir + "/OpenSteamworks.Native/oldprotoc/build");

    execWrap("cmake ..", {
        cwd: projdir + "/OpenSteamworks.Native/oldprotoc/build"
    })

    execWrap("cmake --build . --config MinSizeRel --parallel 24 --", {
        cwd: projdir + "/OpenSteamworks.Native/oldprotoc/build"
    })
}

main()