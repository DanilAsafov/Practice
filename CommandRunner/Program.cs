using System;
using CommandLib;
using System.Reflection;
using FileSystemCommands;

namespace CommandRunner;

class Program {
    static void Main() {
        string commandDllPath = "FileSystemCommands.dll";
        
        if (!File.Exists(commandDllPath)) {
            Console.WriteLine($"Файл библиотеки команд не найден: {commandDllPath}");
            return;
        }

        var assembly = Assembly.LoadFrom(commandDllPath);
        var commands = new ICommand?[]
        {
            CreateCommand(assembly, "DirectorySizeCommand", [Directory.GetCurrentDirectory()]),
            CreateCommand(assembly, "FindFilesCommand", [Directory.GetCurrentDirectory(), "*.*"])
        }.Where(c => c != null).ToArray()!;

        Console.WriteLine($"Найдено команд: {commands.Length}\n");
    
        foreach (var command in commands) {
            Console.WriteLine($"Выполнение команды: {command.GetType().Name}");
            Console.WriteLine(new string('-', 40));
            
            try {
                command.Execute();
                PrintCommandResult(command);
            } catch (Exception ex) {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
            
            Console.WriteLine();
        }
    }

    private static ICommand? CreateCommand(Assembly assembly, string className, object[]parameters) {
        try {
            var type = assembly.GetType($"FileSystemCommands.{className}");
            return (ICommand?)Activator.CreateInstance(type!, parameters);
        } catch {
            Console.WriteLine($"Не удалось создать команду: {className}");
            return null;
        }
    }

    private static void PrintCommandResult(ICommand command) {
        switch (command) {
            case DirectorySizeCommand sizeCommand:
                Console.WriteLine($"Размер каталога: {sizeCommand.Size} байт");
                break;
            
            case FindFilesCommand findCommand:
                Console.WriteLine($"Найдено файлов: {findCommand.Files.Length}");
                foreach (var file in findCommand.Files)
                    Console.WriteLine($"- {Path.GetFileName(file)}");
                break;
        }
    }
}
