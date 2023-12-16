namespace advent.of.code.day16;

internal static class Solution {

    private enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    internal static int Task1(StreamReader reader) {
        var map = ReadMap(reader);
        return GetTotalEnergizedPoints(map, GetNextPointers(new Pointer(Direction.Right, new Point(-1, 0)), map).ToList());
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadMap(reader);

        var startingPointers = new HashSet<Pointer>();

        for (var x = 0; x < map[0].Length; x++) {
            startingPointers.Add(new Pointer(Direction.Down, new Point(x, -1)));
            startingPointers.Add(new Pointer(Direction.Up, new Point(x, map.Count)));
        }

        for (var y = 0; y < map.Count; y++) {
            startingPointers.Add(new Pointer(Direction.Right, new Point(-1, y)));
            startingPointers.Add(new Pointer(Direction.Left, new Point(map[0].Length, y)));
        }

        return startingPointers.Select(pointer => GetNextPointers(pointer, map))
            .AsParallel()
            .Select(pointers => GetTotalEnergizedPoints(map, pointers.ToList()))
            .Max();
    }

    private static List<string> ReadMap(StreamReader reader) {
        var map = new List<string>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            map.Add(line);
        }

        return map;
    }

    private static int GetTotalEnergizedPoints(IReadOnlyList<string> map, List<Pointer> pointers) {
        var energizedMap = map[0].Select(_ => new bool[map.Count]).ToArray();

        var pointerCache = new HashSet<Pointer>(pointers);

        while (pointers.Count > 0) {
            pointers.ForEach(pointer => energizedMap[pointer.Pos.Y][pointer.Pos.X] = true);
            pointers = GetAllNextPointers(pointers, pointerCache, map).ToList();
        }

        return energizedMap.Select(line => line.Count(energized => energized)).Sum();
        // calculate with just all unique points in pointer cache
        // return pointerCache.Select(pointer => pointer.Pos).Distinct().Count();
    }

    private static IEnumerable<Pointer> GetAllNextPointers(IEnumerable<Pointer> pointers, ISet<Pointer> pointerCache, IReadOnlyList<string> map) {
        return from pointer in pointers
            from nextPointers in GetNextPointers(pointer, map)
            // filter out pointers that are already in the cache and add the rest to the cache
            where pointerCache.Add(nextPointers)
            select nextPointers;
    }
    
    private static IEnumerable<Pointer> GetNextPointers(Pointer pointer, IReadOnlyList<string> map) {
        var nextPos = pointer.Pos.Offset(pointer.Direction);
        if (nextPos.X < 0 || nextPos.X >= map[0].Length || nextPos.Y < 0 || nextPos.Y >= map.Count) {
            yield break;
        }

        switch (map[nextPos.Y][nextPos.X]) {
            case '/':
                yield return pointer.Direction switch {
                    Direction.Right => new Pointer(Direction.Up, nextPos),
                    Direction.Left => new Pointer(Direction.Down, nextPos),
                    Direction.Up => new Pointer(Direction.Right, nextPos),
                    Direction.Down => new Pointer(Direction.Left, nextPos),
                    _ => throw new Exception()
                };
                break;
            case '\\':
                yield return pointer.Direction switch {
                    Direction.Right => new Pointer(Direction.Down, nextPos),
                    Direction.Left => new Pointer(Direction.Up, nextPos),
                    Direction.Up => new Pointer(Direction.Left, nextPos),
                    Direction.Down => new Pointer(Direction.Right, nextPos),
                    _ => throw new Exception()
                };
                break;
            case '|' when pointer.Direction != Direction.Up && pointer.Direction != Direction.Down:
                yield return new Pointer(Direction.Up, nextPos);
                yield return new Pointer(Direction.Down, nextPos);
                break;
            case '-' when pointer.Direction != Direction.Left && pointer.Direction != Direction.Right:
                yield return new Pointer(Direction.Left, nextPos);
                yield return new Pointer(Direction.Right, nextPos);
                break;
            default:
                yield return pointer with { Pos = nextPos };
                break;
        }
    }

    private record Pointer(Direction Direction, Point Pos);

    private record Point(int X, int Y) {
        public Point Offset(Direction direction) {
            return direction switch {
                Direction.Up => this with { Y = Y - 1 },
                Direction.Down => this with { Y = Y + 1 },
                Direction.Left => this with { X = X - 1 },
                Direction.Right => this with { X = X + 1 },
                _ => this
            };
        }
    }
}