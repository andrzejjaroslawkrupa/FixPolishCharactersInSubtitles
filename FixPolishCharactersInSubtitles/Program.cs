using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.CharacterTranslation;
using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using FixPolishCharactersInSubtitles.FileManagement;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

var serviceProvider = new ServiceCollection()
    .AddSingleton<ITranslateCharactersService, TranslateAnsiCharactersToPolishService>()
    .AddSingleton<IConvertCharactersInAllFiles, FixPolishCharactersInAllFilesInCurrentDirectory>()
    .AddSingleton<IConvertToSubRipService, ConvertToSubRipService>()
    .AddSingleton<IGetLocalFiles, GetLocalFilesService>()
    .AddSingleton<IFileSystem, FileSystem>()
    .AddSingleton<IConverterFactory, ConverterFactory>()
    .BuildServiceProvider();

serviceProvider?.GetService<IConvertCharactersInAllFiles>()?.ConvertAllFiles();
