using System.Text.RegularExpressions;

namespace advent.of.code.day8; 

internal static partial class Solution {

    [GeneratedRegex(@"(?<key>[0-9A-Z]{3})\s=\s\((?<left>[0-9A-Z]{3}),\s(?<right>[0-9A-Z]{3})\)")]
    private static partial Regex DirectionMapRegex();

    internal static long Task1(StreamReader reader) {
        Read(reader, out var directions, out var left, out var right);
        
        return CountSteps("AAA", directions, left, right);
    }

    internal static long Task2(StreamReader reader) {
        var startingNodes = Read(reader, out var directions, out var left, out var right);
        
        return startingNodes.AsParallel()
            // count steps for each starting node to get to a node ending with Z
            .Select(pos => CountSteps(pos, directions, left, right))
            // get lowest common multiple of all cycles
            .Aggregate((cycleA, cycleB) => cycleA * cycleB / GreatestCommonDelimiter(cycleA, cycleB));
    }

    private static List<string> Read(StreamReader reader, out char[] directions, out Dictionary<string, string> left, out Dictionary<string, string> right) {
        var directionsLine = reader.ReadLine() ?? throw new Exception();
        directions = directionsLine.ToCharArray();

        // empty line
        reader.ReadLine();
        
        left = new Dictionary<string, string>();
        right = new Dictionary<string, string>();

        var startingNodes = new List<string>();
        
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            
            var match = DirectionMapRegex().Match(line);

            var key = match.Groups["key"].Value;
            
            left.Add(key, match.Groups["left"].Value);
            right.Add(key, match.Groups["right"].Value);

            if (key.EndsWith("A")) startingNodes.Add(key);
        }

        return startingNodes;
    }

    private static long CountSteps(string pos, IReadOnlyList<char> directions, IReadOnlyDictionary<string, string> left, IReadOnlyDictionary<string, string> right) {
        var currentDirection = 0;
        var steps = 0L;
        while (!pos.EndsWith("Z")) {
            steps++;
            pos = directions[currentDirection] == 'L' ? left[pos] : right[pos];
            
            currentDirection++;
            if (currentDirection >= directions.Count) {
                currentDirection = 0;
            }
        }

        return steps;
    }

    private static long GreatestCommonDelimiter(long a, long b) {
        if (b == 0) return a;
        return GreatestCommonDelimiter(b, a % b);
    }
}