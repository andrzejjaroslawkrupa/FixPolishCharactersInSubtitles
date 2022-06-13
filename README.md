# FixPolishCharactersInSubtitles

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Using the tool](#using-the-tool)

## General info
For some reason when dowloaded from NapiProjekt subtitles are encoded in a way that is not supported by Kodi or in some cases by VLC. This console app converts subtitle text files to have proper Polish characters (encoding 1252). 
	
## Technologies
Project is created with:
* .NET 6
* xUnit
* Moq
	
## Setup
To run this project, either download source code, compile with 

```
$ dotnet build .\FixPolishCharactersInSubtitles.sln
```

Or download release version.

## Using the tool
Paste subtitle file to a folder where executable "FixPolishCharactersInSubtitles.exe" resides and run that executable.
After that file should be converted.
