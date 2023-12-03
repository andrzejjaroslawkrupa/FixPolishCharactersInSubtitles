# FixPolishCharactersInSubtitles

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Using the tool](#using-the-tool)

## General info
For some reason when dowloaded from NapiProjekt subtitles are encoded in a way that is not supported by Kodi, Jellyfin or in some cases by VLC. This console app converts subtitle text files to have proper Polish characters (encoding 1252) and saves it in SubRip format (srt). 
	
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
Run executable file with --path argument and path to a folder:
```
$ .\FixPolishCharactersInSubtitles.exe --path "path to your folder"
```
Alternatively you can paste your subtitle file to a folder where executable "FixPolishCharactersInSubtitles.exe" resides and run that executable.
After that file should be converted.

# FixPolishCharactersInSubtitles

## Spis treści
* [Informacje ogólne](#informacje-ogólne)
* [Technologie](#technologie)
* [Konfiguracja](#konfiguracja)
* [Korzystanie z narzędzia](#korzystanie-z-narzędzia)

## Informacje ogólne
Z jakiegoś powodu, gdy pobierane są napisy ze strony NapiProjekt, są one zakodowane w sposób niewspierany przez Kodi, Jellyfin lub w niektórych przypadkach przez VLC. Ta aplikacja konsolowa konwertuje pliki tekstowe z napisami, aby zawierały poprawne polskie znaki (kodowanie 1252) oraz zapisuje w formacie SubRip (srt).

## Technologie
Projekt został stworzony przy użyciu:
* .NET 6
* xUnit
* Moq

## Konfiguracja
Aby uruchomić ten projekt, możesz albo pobrać kod źródłowy i skompilować go za pomocą 

```
$ dotnet build .\FixPolishCharactersInSubtitles.sln
```

lub pobrać wersję releasową.

## Korzystanie z narzędzia
Uruchom plik wykonywalny z argumentem --path i ścieżką do folderu:
```
$ .\FixPolishCharactersInSubtitles.exe --path "ścieżka do twojego folderu"
```
Alternatywnie wklej plik z napisami do folderu, w którym znajduje się wykonywalny plik "FixPolishCharactersInSubtitles.exe" i go uruchom.
Po wykonaniu operacji plik powinien zostać skonwertowany.
