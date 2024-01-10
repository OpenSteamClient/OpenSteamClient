import fs from "fs"
import { CreateDifferenceObject, Difference, compareArrays } from "./util";

export interface ClientFunction {
    name: string;
    argc: string;
    addr?: string;
    interfaceid: string;
    functionid: string;
    fencepost: string;
    cannotcallincrossprocess: string;
    serializedargs: string[];
    serializedreturns: string[];
}

export interface ClientInterface {
    name: string;
    functions: ClientFunction[]; 
    found_at?: string;
}

export interface ClientCallback {
    id: number;
    name: string;
    size: number;
    posted_at?: string[];
}

export interface ClientEMsg {
    emsg: number;
    flags: number;
    server_type: number;
    name: string;
}

export interface ClientDifference {
    interfaces: Difference<ClientInterface>;
    methods: Difference<ClientFunction>;
    callbacks: Difference<ClientCallback>;
    eMsgs: Difference<ClientEMsg>;
}

export class ClientDump {
    public interfaces: ClientInterface[] = [];
    public callbacks: ClientCallback[] = [];
    public eMsgList: ClientEMsg[] = [];

    static async ReadFromDirectory(dir: string): Promise<ClientDump> {
        var clientDump: ClientDump = new ClientDump();
        //clientDump.eMsgList = JSON.parse(fs.readFileSync(dir + "/emsg_list.json").toString())
        clientDump.eMsgList = [];
        clientDump.callbacks = JSON.parse(fs.readFileSync(dir + "/callbacks.json").toString())
        for (var file of fs.readdirSync(dir)) {
            if (file.endsWith("Map.json") && file.startsWith("IClient") || file.startsWith("IRegistry")) {
                var asJson = JSON.parse(fs.readFileSync(dir+file).toString());

                var iface: ClientInterface = {
                    name: asJson.name,
                    functions: [],
                    found_at: asJson.found_at,    
                }
                
                iface.name = iface.name.substring(0, iface.name.indexOf("Map"));
                for (var funcAsJson of asJson.functions) {
                    // There's some functions with 0 args. These are unknown, but let's keep them since the headers fuck up without them. 
                    funcAsJson.argc = funcAsJson.argc - 1;
                    if (funcAsJson.argc < 0) {
                        funcAsJson.name += "_DONTUSE"
                    }

                    iface.functions.push({
                        name: funcAsJson.name,
                        argc: funcAsJson.argc,
                        addr: funcAsJson.addr,
                        interfaceid: funcAsJson.interfaceid,
                        functionid: funcAsJson.functionid,
                        fencepost: funcAsJson.fencepost,
                        cannotcallincrossprocess: funcAsJson.cannotcallincrossprocess,
                        serializedargs: funcAsJson.serializedargs,
                        serializedreturns: funcAsJson.serializedreturns
                    });
                }

                clientDump.interfaces.push(iface);
            }
        }
        return clientDump;
    }

    private methodComparer(searchItem: ClientFunction, current: ClientFunction): boolean {
        if (searchItem.name == current.name) {
            return true;
        }
        return false;
    }

    CompareTo(other: ClientDump): ClientDifference {
        var diff: ClientDifference = {
            interfaces: CreateDifferenceObject(),
            methods: CreateDifferenceObject(),
            callbacks: CreateDifferenceObject(),
            eMsgs: CreateDifferenceObject()
        };

        // Compare interfaces
        {
            diff.interfaces = compareArrays(this.interfaces, other.interfaces, (searchItem, current) => {
                return searchItem.name == current.name
            }, (oldInterface, newInterface) => {
                if (oldInterface.functions.length != newInterface.functions.length) {
                    return true;
                }
                if (compareArrays(oldInterface.functions, newInterface.functions, this.methodComparer).isDifferent) {
                    return true;
                }
                return false;
            });
        }
    
        // Compare functions
        {
            var thisfuncs: ClientFunction[] = [];
            var otherfuncs: ClientFunction[] = [];
            
            for (const iface of this.interfaces) {
                for (const func of iface.functions) {
                    thisfuncs.push({ ...func, name: `${iface.name}::${func.name}` });
                }
            }

            for (const iface of other.interfaces) {
                for (const func of iface.functions) {
                    otherfuncs.push({ ...func, name: `${iface.name}::${func.name}` });
                }
            }

            diff.methods = compareArrays(thisfuncs, otherfuncs, this.methodComparer, (oldFunc, newFunc) => {
                return !(oldFunc.argc == newFunc.argc);
            });
        }
        
        // Compare callbacks
        {
            diff.callbacks = compareArrays(this.callbacks, other.callbacks, (searchItem, currentItem) => {
                return searchItem.id == currentItem.id;
            }, (oldCallback, newCallback) => {
                if (oldCallback.name != newCallback.name) {
                    return true;
                }
                if (oldCallback.size != newCallback.size) {
                    return true;
                }
                return false;
            });
        }
        
        // Compare eMsgs
        {
            diff.eMsgs = compareArrays(this.eMsgList, other.eMsgList, (searchItem, currentItem) => {
                return searchItem.emsg == currentItem.emsg;
            }, (oldEMsg, newEMsg) => {
                if (oldEMsg.name != newEMsg.name) {
                    return true;
                }
                return false;
            });
        }
        

        return diff;
    }
}