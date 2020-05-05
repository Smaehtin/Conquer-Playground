# Introduction
**This is currently for learning purposes only, it is in no way intended to be some kind of "playable" thing.**

The project consists of a DLL that is injected into the Conquer Online process which hooks functions in the client and "tricks" the client into thinking that it's connecting and talking to an actual server.

The included `MinHookManaged` project is a thick .NET binding library for the MinHook library, which is the library that is currently used for the hooking part.

A launcher is also provided for convenience, but any launcher that is capable of injecting the DLL and calling the exported "Initialize" function should work fine.

The project currently targets the patch 4356 Conquer.exe. If you are interested in trying out the project, I'd suggest that you download the 4351 client and apply patch 4352-4358. It is of course possible to use this for other patches, but that will require you to update the addresses for the ReceiveMsg and SendMsg functions and quite possibly also some packets.

# Files
Inside the `bin/Debug` or `bin/Release` folder, the following files will need to be copied to your Conquer Online installation directory:

`ConquerPlayground.dll` - the C# DLL that contains all the "game logic" (not much so far).  
`Launcher.exe` - responsible for launching Conquer.exe and injecting the DLL.  
`MinHook.x86.dll` - the native MinHook library.  
`MinHookManaged.dll` - the thick .NET binding for the MinHook library.

# Features
Close to none. Right now, there's almost nothing implemented. There's only a couple of packet handlers implemented which is just enough to allow you to "log in" (username/password and server doesn't matter), get items from the shopping mall, equip items, and use a couple of commands.