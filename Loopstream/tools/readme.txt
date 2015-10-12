When launching Loopstream, it will generate a LoopstreamTools folder
in its working directory. The folder you just opened is the template.

In order to embed this folder into Loopstream.exe, it must be
compressed into an archive which then becomes an "Embedded Resource".

When Loopstream is launched, it looks for "../../tools". If found,
it will create "../../res/tools.dfc" with the folder's contents.
This file is flagged as an Embedded Resource in Visual Studio,
so the embedded tools.dfc in Loopstream.exe will be replaced
the next time you launch Loopstream from Visual Studio.

If you just grabbed the source from GitHub then this folder won't
contain lame and oggenc, so you'll be asked to select them
when creating your dfc.

So basically, modify stuff in this folder, launch Loopstream from
Visual Studio, and whenever someone runs the new version of your
Loopstream.exe in a folder that doesn't have LoopstreamTools
it'll be created with the junk thats in this directory!
