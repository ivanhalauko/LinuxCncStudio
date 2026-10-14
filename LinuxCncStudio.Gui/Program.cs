using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Avalonia;

namespace LinuxCncStudio.Gui;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            GenerateProjectBundle();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Предупреждение бандла]: {ex.Message}");
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    private static void GenerateProjectBundle()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        DirectoryInfo? solutionDir = new DirectoryInfo(baseDir).Parent?.Parent?.Parent?.Parent;

        if (solutionDir == null || !solutionDir.Exists)
        {
            solutionDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        string outputFile = Path.Combine(solutionDir.FullName, "LinuxCncStudio_Bundle.txt");
        var allowedExtensions = new HashSet<string> { ".cs", ".csproj", ".sln", ".axaml", ".manifest" };
        var bundleBuilder = new StringBuilder();
        int fileCount = 0;

        foreach (string filePath in Directory.EnumerateFiles(solutionDir.FullName, "*.*", SearchOption.AllDirectories))
        {
            string ext = Path.GetExtension(filePath).ToLower();
            if (!allowedExtensions.Contains(ext)) continue;

            if (filePath.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") ||
                filePath.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") ||
                filePath.Contains($"{Path.DirectorySeparatorChar}.vs{Path.DirectorySeparatorChar}") ||
                filePath.Equals(outputFile, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string relativePath = Path.GetRelativePath(solutionDir.FullName, filePath);

            bundleBuilder.AppendLine("================================================================================");
            bundleBuilder.AppendLine($"FILE: {relativePath}");
            bundleBuilder.AppendLine("================================================================================");
            bundleBuilder.AppendLine(File.ReadAllText(filePath));
            bundleBuilder.AppendLine();
            bundleBuilder.AppendLine();

            fileCount++;
        }

        File.WriteAllText(outputFile, bundleBuilder.ToString(), Encoding.UTF8);
        Console.WriteLine($"[БАНДЛ СОЗДАН] Упаковано файлов: {fileCount} в {outputFile}");
    }
}
