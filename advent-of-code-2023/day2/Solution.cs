using System.Text.RegularExpressions;

namespace advent.of.code.day2;

internal static partial class Solution {

    [GeneratedRegex(@"^Game\s(?<id>\d+):((\s(?<cube>\d+\s(red|green|blue)),?)+;?)+$")]
    private static partial Regex GameRegex();

    internal static int Task1(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var match = GameRegex().Match(line);

            var cubes = match.Groups["cube"].Captures.Select(capture => {
                var cube = capture.Value.Split(' ');
                return (count: int.Parse(cube[0]), color: cube[1]);
            });

            var notPossible = cubes.Any(cube => cube.color switch {
                "red" => cube.count > 12,
                "green" => cube.count > 13,
                "blue" => cube.count > 14,
                _ => true
            });

            if (notPossible) continue;
            total += int.Parse(match.Groups["id"].Value);
        }

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var match = GameRegex().Match(line);

            var cubes = match.Groups["cube"].Captures.Select(capture => {
                var cube = capture.Value.Split(' ');
                return (count: int.Parse(cube[0]), color: cube[1]);
            });

            var maxCounts = new Dictionary<string, int> {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 }
            };

            foreach (var cube in cubes) {
                if (maxCounts[cube.color] < cube.count) maxCounts[cube.color] = cube.count;
            }

            total += maxCounts.Values.Aggregate((a, b) => a * b);
        }

        return total;
    }
}