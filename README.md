# Solitaire
A simple implementation of the classic game "Klondike", or "Patience"

Updated to version 1.0.1

Configuration: Any CPU; Target Framework: .NET3.5; C# v7.0, Visual Studio 2017

This was written using features of C# version 7 (Visual Studio 2017). It will NOT compile in Visual Studio 2012.

Make sure to copy the "data" folder from the "Debug' folder to the "Release" folder, or the graphics won't work.

There is an excluded class called "Images.cs" in "Classes/Helpers" that builds the graphics data files from the "Images" folder. To rebuild the graphics files call Images.Build() from somewhere at start up. A good place is void Main in "Program.cs" before the main form is even loaded.

These binary data files are included in this git.

Bin/Debug/data/sound/music folder is included for putting mp3 files in, as the application is set up to play music; however, no music files have been included, as it would make the repository bigger than it needs to be. You can put whatever music you like in that folder, or none at all.
