using System.Text.RegularExpressions;

namespace advent.of.code.day1;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            total += GetTwoDigitNumber(line);
        }

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            total += GetTwoDigitNumber(ReplaceSpelledNumbers(line));
        }
        return total;
    }
    
    private static int GetTwoDigitNumber(string input) {
        var number = Regex.Replace(input, "[^0-9]", "");
        return int.Parse(number[..1] + number[^1..]);
    }

    private static string ReplaceSpelledNumbers(string input) {
        var numberMapping = new Dictionary<string, string> {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };
            
        var firstIndex = input.Length;
        var firstValue = "";
        foreach (var keyValue in numberMapping) {
            var index = input.IndexOf(keyValue.Key, StringComparison.Ordinal);
            if (index != -1 && index <= firstIndex) {
                firstIndex = index;
                firstValue = keyValue.Value;
            }
        }

        input = input.Insert(firstIndex, firstValue);
            
        var lastIndex = 0;
        var lastValue = "";
        foreach (var keyValue in numberMapping) {
            var index = input.LastIndexOf(keyValue.Key, StringComparison.Ordinal);
            if (index != -1 && index >= lastIndex) {
                lastIndex = index;
                lastValue = keyValue.Value;
            }
        }

        return input.Insert(lastIndex, lastValue);
    }
}