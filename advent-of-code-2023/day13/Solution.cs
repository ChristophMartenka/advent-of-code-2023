namespace advent.of.code.day13;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        return ReadMaps(reader)
            .Select(map => {
                var mirrorIndex = CheckHorizontalMirror(map, false);
                return mirrorIndex < 0 ? CheckHorizontalMirror(RotateRight(map), false) : mirrorIndex * 100;
            })
            .Sum();
    }

    internal static int Task2(StreamReader reader) {
        return ReadMaps(reader)
            .Select(map => {
                var mirrorIndex = CheckHorizontalMirror(map, true);
                return mirrorIndex < 0 ? CheckHorizontalMirror(RotateRight(map), true) : mirrorIndex * 100;
            })
            .Sum();
    }

    private static IEnumerable<List<string>> ReadMaps(StreamReader reader) {
        var currentMap = new List<string>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            if (line != "") {
                currentMap.Add(line);
            } else {
                yield return currentMap;
                currentMap = new List<string>();
            }
        }

        yield return currentMap;
    }

    private static int CheckHorizontalMirror(IReadOnlyList<string> map, bool allowSmudge) {
        for (var i = 0; i < map.Count - 1; i++) {
            if (CheckHorizontalMirrorAt(map, i, allowSmudge)) return i + 1;
        }

        return -1;
    }

    private static bool CheckHorizontalMirrorAt(IReadOnlyList<string> map, int index, bool allowSmudge) {
        var pointerUp = index;
        var pointerDown = index + 1;
        while (pointerUp >= 0 && pointerDown < map.Count) {
            for (var c = 0; c < map[index].Length; c++) {
                if (map[pointerUp][c] == map[pointerDown][c]) continue;
                if (!allowSmudge) return false;
                allowSmudge = false;
            }

            pointerUp--;
            pointerDown++;
        }

        return !allowSmudge;
    }

    private static List<string> RotateRight(IReadOnlyList<string> map) {
        var rotatedMap = map[0].Select(_ => "").ToList();

        foreach (var line in map) {
            for (var i = 0; i < line.Length; i++) {
                rotatedMap[i] += line[i];
            }
        }

        return rotatedMap;
    }
}