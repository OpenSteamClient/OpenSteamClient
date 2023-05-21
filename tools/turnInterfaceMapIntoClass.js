// Turns an interfacemap (in dumped_data) to a class
// Use this to update interfaces and shit
const fs = require('fs');

const interfaceName = process.argv[2];

if (!interfaceName) {
    console.log("Needs an interface name to use");
    return;
}

const ininterfaces = JSON.parse(fs.readFileSync('../dumped_data/'+interfaceName+"Map.json",
    { encoding: 'utf8', flag: 'r' }
));

const interfaceOut = [];

interfaceOut.push("#ifndef " + interfaceName.toUpperCase()+"_H");
interfaceOut.push("#define " + interfaceName.toUpperCase()+"_H");
interfaceOut.push("#ifndef _WIN32");
interfaceOut.push("#pragma once");
interfaceOut.push("#endif");

interfaceOut.push("");

interfaceOut.push('#include "SteamTypes.h"');

if (fs.existsSync("../include/opensteamworks/" + interfaceName.substring("IClient".length) + 'Common.h')) {
    interfaceOut.push('#include "' + interfaceName.substring("IClient".length) + 'Common.h"');
}


interfaceOut.push("");

interfaceOut.push("abstract_class " + interfaceName);
interfaceOut.push("{");
interfaceOut.push("public:");

ininterfaces.functions.forEach((func) => {
	interfaceOut.push("     virtual void " + func.name + "() = 0;" + " //args: " + (func.argc-1) + ", index: " + ininterfaces.functions.indexOf(func));
})

interfaceOut.push("};");
interfaceOut.push("#endif // " + interfaceName.toUpperCase() + "_H")

const filename = "./generated_interfaces/" + interfaceName + ".h";

if (!fs.existsSync("./generated_interfaces")) {
    fs.mkdirSync("./generated_interfaces");
}

var writter = fs.createWriteStream(filename, {
    flags: 'w'
})

interfaceOut.forEach(element => {
    writter.write(element+"\n");
});

writter.close();
