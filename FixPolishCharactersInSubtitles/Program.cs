using FixPolishCharactersInSubtitles;
using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.CharacterTranslation;
using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using FixPolishCharactersInSubtitles.FileManagement;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

var serviceProvider = new ServiceCollection()
    .AddSingleton<ITranslateCharactersService, TranslateAnsiCharactersToPolishService>()
    .AddSingleton<IConvertCharactersInFiles, FixPolishCharactersInFilesI>()
    .AddSingleton<IConvertToSubRipService, ConvertToSubRipService>()
    .AddSingleton<IGetFiles, GetFilesService>()
    .AddSingleton<IFileSystem, FileSystem>()
    .AddSingleton<ICommandLineInterface, CommandLineInterface>()
    .AddSingleton<IConverterFactory, ConverterFactory>()
    .BuildServiceProvider();

serviceProvider?.GetService<IConvertCharactersInFiles>()?.Convert();
