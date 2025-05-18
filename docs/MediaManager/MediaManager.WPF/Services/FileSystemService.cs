using CliWrap;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaManager.WPF.Services;

public interface IFileSystemService
{
    bool DirectoryExists(string dirPath);
    Task<string?> ListFilesAsync(string dirPath);
    Task<List<FileInfo>?> GetFileInfosAsync(string dirPath);
    Task OpenFileAsync(string filePath);
    Task OpenDirectoryAsync(string directoryPath);
}

public class FileSystemService : IFileSystemService
{
    private readonly ILogger<FileSystemService> logger;
    private readonly ICommandLineService commandLineService;

    public FileSystemService(ILogger<FileSystemService> logger, ICommandLineService commandLineService)
    {
        this.logger = logger;
        this.commandLineService = commandLineService;
    }

    public async Task<List<FileInfo>?> GetFileInfosAsync(string dirPath)
    {
        if (!DirectoryExists(dirPath))
        {
            logger.LogError("Directory not found: '{dirPath}'", dirPath);
            return null;
        }
        List<FileInfo>? fileInfos = null;
        try
        {
            // var files = Directory.GetFiles(dirPath, SearchOption.AllDirectories);
            var files = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            if (files.Count() > 0)
            {
                fileInfos = new List<FileInfo>();
                foreach (var file in files)
                {
                    fileInfos.Add(new FileInfo(file));
                }
            }
            else
            {
                logger.LogInformation("No files found in: '{dirPath}'", dirPath);
            }
        }
        catch(Exception ex)
        {
            logger.LogError("Error in GetFileInfosAsync: {Message}", ex.Message);
        }
        return fileInfos;
    }

    public async Task<string?> ListFilesAsync(string dirPath)
    {
        // await Task.Delay(2000); // Simulate a bit of work
        if (!DirectoryExists(dirPath))
        {
            logger.LogError("Directory not found: '{dirPath}'", dirPath);
            return null;
        }
        var commandConstruct = new CommandConstruct
        {
            Exectuable = "C:\\Program Files\\Git\\usr\\bin\\ls.exe",
            Parameters = "-ABmQ",
            WorkingDirectory = dirPath
        };
        var listing = await commandLineService.RunCommandReturnStringAsync(commandConstruct);

        return listing;
    }

    public async Task OpenFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            logger.LogError("File not found: '{filePath}'", filePath);
            return;
        }
        var notepadPlusPlusExePath = string.Empty;
        try
        {
            notepadPlusPlusExePath = Environment.GetEnvironmentVariable("NP");
        }
        catch { }
        if (String.IsNullOrWhiteSpace(notepadPlusPlusExePath))
        {
            notepadPlusPlusExePath = @"C:\Program Files\Notepad++\notepad++.exe";
        }
        // notepadPlusPlusExePath = notepadPlusPlusExePath.MassageFilePath();
        await RunCommand(notepadPlusPlusExePath, filePath);
    }

    public async Task OpenDirectoryAsync(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            logger.LogError("Directory not found: '{directoryPath}'", directoryPath);
            return;
        }
        directoryPath = $"\"{directoryPath}\"";
        var explorerExePath = @"C:\Windows\explorer.exe";

        logger.LogInformation("{explorerExePath} {directoryPath}", explorerExePath, directoryPath);
        await RunCommand(explorerExePath, directoryPath);
    }

    private async Task RunCommand(string command, string parameters)
    {
        try
        {
            await Cli.Wrap(command)
                .WithArguments(parameters)
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            logger.LogError("ERROR: '{Message}'", ex.Message);
        }
    }

    public bool DirectoryExists(string dirPath)
    {
        return Directory.Exists(dirPath);
    }
}
