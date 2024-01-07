# THIS FOLDER IS CURRENTLY EXCLUDED FROM THE BUILD
I don't know whether we should hand-write all the IPC interfaces (a ton of work) or somehow JIT generate it (how to do returns and ensure args get taken from the correct places?)
JIT approach would be the best since we wouldn't need to manually write 3000+ functions and update them as we discover how the interfaces are typed.
But I don't know if that's possible with the data that's being dumped now. We'd need a big update to steamworks_dumper to dump all arguments and connect them to their respective serializer functions.
Also deserializing would be a nightmare.