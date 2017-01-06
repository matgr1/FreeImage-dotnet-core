This is a clone of the FreeImage .NET project (http://freeimage.sourceforge.net/) modified to work with dotnet core.  Note that all functions that use System.Drawing.Bitmap have been removed from the netstandard framework versions as there is no implementation for this class.

FreeImage native binaries are included in the nuget package for Windows, other platforms they will have to be installed separately (eg. on OSX `brew install freeimage` should work)

nuget package: https://www.nuget.org/packages/FreeImage-dotnet-core

`Install-Package FreeImage-dotnet-core`

#### FreeImage Version

This is for FreeImage version 3.17.0 

##### The version number of this package no longer matches the FreeImage native library version!

#### License

The license is the same as the FreeImage license available at the link above
