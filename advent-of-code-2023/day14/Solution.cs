namespace advent.of.code.day14;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var map = ReadMap(reader);
        var rotatedMap = RotateAndMove(map);
        return GetTotalLoad(rotatedMap);
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadMap(reader);

        var cache = new Dictionary<string, int>();
        var cycleLength = 1_000_000_000;
        var insideLoop = false;

        for (var cycle = 0; cycle < cycleLength; cycle++) {
            map = CycleRotateAndMove(map);

            // currently inside loop, continue cycles until the end is reached
            if (insideLoop) continue;

            var mapAsString = string.Join("\n", map);
            if (cache.TryGetValue(mapAsString, out var index)) {
                // loop detected, calculate remaining cycles
                cycleLength = cycle + (cycleLength - cycle) % (cycle - index);
                insideLoop = true;
            } else {
                cache.Add(mapAsString, cycle);
            }
        }

        // rotation is in initial orientation, rotate once more as total load is calculated in rotated orientation
        return GetTotalLoad(RotateRight(map));
    }

    private static List<string> ReadMap(StreamReader reader) {
        var map = new List<string>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            map.Add(line);
        }

        return map;
    }

    private static List<string> RotateAndMove(IReadOnlyList<string> map) {
        return RotateRight(map).Select(MoveRocks).ToList();
    }

    private static string MoveRocks(string line) {
        var pointer = line.LastIndexOf(".", StringComparison.Ordinal);
        var chars = line.ToCharArray();
        for (var i = pointer - 1; i >= 0; i--) {
            switch (chars[i]) {
                // position is blocked, move pointer to current index, as future rocks can not be moved further
                case '#':
                    pointer = i;
                    break;
                // position is rock and pointer is empty, move rock to pointer
                case 'O':
                    chars[i] = '.';
                    chars[pointer] = 'O';
                    break;
            }

            // move pointer to next empty space
            while (pointer > 0 && chars[pointer] != '.') {
                pointer--;
            }

            // pointer moved ahead of index, move index to pointer
            if (i >= pointer) {
                i = pointer;
            }
        }

        return new string(chars);
    }

    private static int GetTotalLoad(IEnumerable<string> map) {
        return map.Select(GetLoadOnLine).Sum();
    }

    private static int GetLoadOnLine(IEnumerable<char> chars) {
        return chars.Select((c, i) => c == 'O' ? i + 1 : 0).Sum();
    }

    private static IEnumerable<string> RotateRight(IReadOnlyList<string> map) {
        var rotatedMap = map[0].Select(_ => new char[map.Count]).ToArray();

        for (var y = 0; y < map.Count; y++) {
            for (var x = 0; x < map[y].Length; x++) {
                rotatedMap[x][map.Count - y - 1] = map[y][x];
            }
        }

        return rotatedMap.Select(chars => new string(chars));
    }

    private static List<string> CycleRotateAndMove(List<string> map) {
        for (var cycle = 0; cycle < 4; cycle++) {
            map = RotateAndMove(map);
        }

        return map;
    }
}