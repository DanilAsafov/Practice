using System;
using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand {
    private readonly string _directoryPath;
    public long Size {get; private set;}

    public DirectorySizeCommand(string directoryPath) {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Путь к каталогу не может быть пустым");

        _directoryPath = directoryPath;
    }
    
    public void Execute() {
        if (!Directory.Exists(_directoryPath))
            throw new DirectoryNotFoundException($"Каталог не найден: {_directoryPath}");
            
        Size = CalculateDirectorySize(_directoryPath);
    }

    private long CalculateDirectorySize(string directoryPath) {
        long size = 0;
        
        foreach (var file in Directory.GetFiles(directoryPath))
            size += new FileInfo(file).Length;
        
        foreach (var dir in Directory.GetDirectories(directoryPath))
            size += CalculateDirectorySize(dir);
        
        return size;
    }
}


public class FindFilesCommand : ICommand {
    private readonly string _directoryPath;
    private readonly string _pattern;
    public string[] Files {get; private set;} = [];

    public FindFilesCommand(string directoryPath, string pattern) {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Путь к каталогу не может быть пустым");
            
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Маска поиска не может быть пустой");
            
        _directoryPath = directoryPath;
        _pattern = pattern;
    }

    public void Execute() {
        if (!Directory.Exists(_directoryPath))
            throw new DirectoryNotFoundException($"Каталог не найден: {_directoryPath}");
            
        Files = Directory.GetFiles(_directoryPath, _pattern);
    }
}
