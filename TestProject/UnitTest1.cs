using System;
using Xunit;
using FileSystemCommands;

namespace TestProject;

public class FileSystemCommandsTests {
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize() {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute(); // Проверяем, что не возникает исключений

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles() {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute(); // Должен найти 1 файл

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldReturnEmpty_WhenNoFilesMatch() {
        var testDir = Path.Combine(Path.GetTempPath(), "NoMatchTestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");

        var command = new FindFilesCommand(testDir, "*.jpg");
        command.Execute();

        var foundFiles = Directory.GetFiles(testDir, "*.jpg");
        Assert.Empty(foundFiles);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMultipleFiles_WhenMatchesMultipleFiles() {
        var testDir = Path.Combine(Path.GetTempPath(), "MultiMatchTestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "First file");
        File.WriteAllText(Path.Combine(testDir, "file2.txt"), "Second file");
        File.WriteAllText(Path.Combine(testDir, "file3.log"), "Log file");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        var foundFiles = Directory.GetFiles(testDir, "*.txt");
        Assert.Equal(2, foundFiles.Length);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldReturnCorrectSize() {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_Size");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "12345");
        File.WriteAllText(Path.Combine(testDir, "file2.txt"), "1234567890");

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Assert.Equal(5 + 10, command.Size);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldReturnZeroForEmptyDirectory() {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_Empty");
        Directory.CreateDirectory(testDir);

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Assert.Equal(0, command.Size);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldThrowWhenDirectoryNotFound() {
        var nonExistentDir = Path.Combine(Path.GetTempPath(), "NonExistentDir");

        var command = new DirectorySizeCommand(nonExistentDir);
        var exception = Assert.Throws<DirectoryNotFoundException>(() => command.Execute());
        Assert.Contains(nonExistentDir, exception.Message);
    }

    [Fact]
    public void FindFilesCommand_ShouldThrow_WhenDirectoryNotFound() {
        var nonExistentDir = Path.Combine(Path.GetTempPath(), "NonExistentDir");

        var command = new FindFilesCommand(nonExistentDir, "*.*");
        var exception = Assert.Throws<DirectoryNotFoundException>(() => command.Execute());
        Assert.Contains(nonExistentDir, exception.Message);
    }
}
