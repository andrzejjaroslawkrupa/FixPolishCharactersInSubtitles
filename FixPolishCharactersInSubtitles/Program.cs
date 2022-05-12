using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.CharacterTranslation;
using FixPolishCharactersInSubtitles.FileManagement;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

var serviceProvider = new ServiceCollection()
    .AddSingleton<ITranslateCharactersService, TranslateAnsiCharactersToPolishService>()
    .AddSingleton<IConvertCharactersInAllFiles, FixPolishCharactersInAllFilesInCurrentDirectory>()
    .AddSingleton<IGetLocalFiles, GetLocalFilesService>()
    .AddSingleton<IFileSystem, FileSystem>()
    .BuildServiceProvider();

serviceProvider?.GetService<IConvertCharactersInAllFiles>()?.ConvertAllFiles();
