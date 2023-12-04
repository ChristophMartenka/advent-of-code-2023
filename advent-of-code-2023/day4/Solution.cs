using System.Text.RegularExpressions;

namespace advent.of.code.day4;

internal static partial class Solution {

    [GeneratedRegex(@".*\s(?<id>\d+)[:](\s*(?<winning>\d+)\s*)+[|](\s*(?<number>\d+)\s*)+")]
    private static partial Regex ScratchCardRegex();

    internal static int Task1(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var match = ScratchCardRegex().Match(line);
            var winningNumbers = match.Groups["winning"].Captures.Select(c => int.Parse(c.Value));
            var numbers = match.Groups["number"].Captures.Select(c => int.Parse(c.Value));

            var matches = winningNumbers.Intersect(numbers).Count();
            var sum = 1;

            for (var i = 1; i < matches; i++) {
                sum *= 2;
            }

            if (matches > 0) {
                total += sum;
            }
        }

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var copies = new Dictionary<int, int>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var match = ScratchCardRegex().Match(line);
            var id = int.Parse(match.Groups["id"].Value);
            var winningNumbers = match.Groups["winning"].Captures.Select(c => int.Parse(c.Value));
            var numbers = match.Groups["number"].Captures.Select(c => int.Parse(c.Value));

            var matches = winningNumbers.Intersect(numbers).Count();

            copies.TryAdd(id, 0);
            copies[id] += 1;

            for (var i = 1; i <= matches; i++) {
                copies.TryAdd(id + i, 0);
                copies[id + i] += 1 * copies[id];
            }
        }

        return copies.Values.Sum();
    }
}