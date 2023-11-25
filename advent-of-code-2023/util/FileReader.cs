using System.Reflection;

namespace advent.of.code.util; 

public static class FileReader {
    
    private static StreamReader GetFile(string fileNamespace, string fileName) {
        var file = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{fileNamespace}.{fileName}");
        return new StreamReader(file);
    }

    public static StreamReader GetFileForDay(int day, string fileName) {
        return GetFile($"advent.of.code.day{day}", fileName);
    }

    public static StreamReader GetInputForDay(int day) {
        return GetFileForDay(day, "input.txt");
    }
}