namespace advent.of.code.day11;

internal static class Solution {

    internal static long Task1(StreamReader reader) {
        var lines = reader.ReadToEnd().Split(Environment.NewLine).ToList();

        var xExpandingIndexes = new HashSet<int>();
        var yExpandingIndexes = new HashSet<int>();
        GetExpandingIndexes(lines, xExpandingIndexes, yExpandingIndexes);
        return TotalGalaxyDistance(lines, xExpandingIndexes, yExpandingIndexes, 2);
    }

    internal static long Task2(StreamReader reader) {
        var lines = reader.ReadToEnd().Split(Environment.NewLine).ToList();

        var xExpandingIndexes = new HashSet<int>();
        var yExpandingIndexes = new HashSet<int>();
        GetExpandingIndexes(lines, xExpandingIndexes, yExpandingIndexes);

        return TotalGalaxyDistance(lines, xExpandingIndexes, yExpandingIndexes, 1000000);
    }

    private static void GetExpandingIndexes(
        IReadOnlyList<string> galaxyMap,
        ISet<int> xExpandingIndexes,
        ISet<int> yExpandingIndexes
    ) {
        for (var y = 0; y < galaxyMap.Count; y++) {
            if (!galaxyMap[y].Contains('#')) {
                yExpandingIndexes.Add(y);
            }
        }

        for (var i = 0; i < galaxyMap[0].Length; i++) {
            if (galaxyMap.Any(point => point[i] == '#')) continue;
            xExpandingIndexes.Add(i);
        }
    }

    private static long TotalGalaxyDistance(
        IReadOnlyList<string> map,
        IReadOnlyCollection<int> xExpandingIndexes,
        IReadOnlyCollection<int> yExpandingIndexes,
        int distanceIncrease
    ) {
        var total = 0L;

        var points = new List<Point>();

        for (var y = 0; y < map.Count; y++) {
            for (var x = 0; x < map[y].Length; x++) {
                if (map[y][x] != '#') continue;

                var currentPoint = new Point(x, y);
                total += points.Select(point =>
                        CalculateDistance(xExpandingIndexes, yExpandingIndexes, distanceIncrease, point, currentPoint))
                    .Sum();

                points.Add(currentPoint);
            }
        }

        return total;
    }

    private static long CalculateDistance(
        IEnumerable<int> xExpandingIndexes,
        IEnumerable<int> yExpandingIndexes,
        int distanceIncrease,
        Point a,
        Point b
    ) {
        var xAdd = xExpandingIndexes.Count(x => x > Math.Min(a.X, b.X) && x < Math.Max(a.X, b.X));
        var yAdd = yExpandingIndexes.Count(y => y > Math.Min(a.Y, b.Y) && y < Math.Max(a.Y, b.Y));
        return a.AbsDistanceTo(b) + (xAdd + yAdd) * (distanceIncrease - 1);
    }

    private record Point(int X, int Y) {
        public long AbsDistanceTo(Point other) {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
    }
}