using System.Text.RegularExpressions;

namespace advent.of.code.day2;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var regex = new Regex(@"^Game\s(?<id>\d+):((\s(?<cube>\d+\s(red|green|blue)),?)+;?)+$");
            var match = regex.Match(line);

            var cubes = match.Groups["cube"].Captures.Select(capture => {
                var cube = capture.Value.Split(' ');
                return (count: int.Parse(cube[0]), color: cube[1]);
            });

            var notPossible = cubes.Any(cube => cube.color switch {
                    "red" => cube.count > 12,
                    "green" => cube.count > 13,
                    "blue" => cube.count > 14,
                    _ => true
                }
            );
            
            if (notPossible) continue;
            total += int.Parse(match.Groups["id"].Value);
        }

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var regex = new Regex(@"^Game\s\d+:((\s(?<cube>\d+\s(red|green|blue)),?)+;?)+$");
            var match = regex.Match(line);

            var cubes = match.Groups["cube"].Captures.Select(capture => {
                var cube = capture.Value.Split(' ');
                return (count: int.Parse(cube[0]), color: cube[1]);
            });

            var minRed = 0;
            var minGreen = 0;
            var minBlue = 0;

            foreach (var cube in cubes) {
                switch (cube.color) {
                    case "red":
                        minRed = cube.count > minRed ? cube.count : minRed;
                        break;
                    case "green":
                        minGreen = cube.count > minGreen ? cube.count : minGreen;
                        break;
                    case "blue":
                        minBlue = cube.count > minBlue ? cube.count : minBlue;
                        break;
                }
            }

            total += minRed * minGreen * minBlue;
        }

        return total;
    }
}