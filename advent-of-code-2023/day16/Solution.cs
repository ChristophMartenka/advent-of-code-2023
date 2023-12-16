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
        var pointer = new Pointer(Direction.Right, new Point(-1, 0));
        return GetTotalEnergizedPoints(map, pointer);
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

        return startingPointers.AsParallel()
            .Select(pointer => GetTotalEnergizedPoints(map, pointer))
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

    private static int GetTotalEnergizedPoints(IReadOnlyList<string> map, Pointer pointer) {
        var energizedMap = map[0].Select(_ => new bool[map.Count]).ToArray();

        var pointers = new List<Pointer> { pointer };
        var pointerCache = new HashSet<Pointer>();

        while (pointers.Count > 0) {
            pointers = pointers.SelectMany(curPointer => GetNextPointersAndEnergize(curPointer, map, energizedMap))
                // filter out pointers that are already in the cache and add the rest to the cache
                .Where(nextPointer => pointerCache.Add(nextPointer))
                .ToList();
        }

        return energizedMap.Select(line => line.Count(energized => energized)).Sum();
    }

    private static IEnumerable<Pointer> GetNextPointersAndEnergize(
        Pointer pointer,
        IReadOnlyList<string> map,
        IReadOnlyList<bool[]> energizedMap
    ) {
        var nextPos = pointer.Pos.Offset(pointer.Direction);
        if (!nextPos.IsWithinBounds(map[0].Length, map.Count)) {
            yield break;
        }

        // energize the next position
        energizedMap[nextPos.Y][nextPos.X] = true;

        // move forward until we hit the next mirror
        while (map[nextPos.Y][nextPos.X] == '.') {
            var next = nextPos.Offset(pointer.Direction);
            // check if we have reached the end of the map
            if (!next.IsWithinBounds(map[0].Length, map.Count)) {
                yield break;
            }

            nextPos = next;

            // energize along the path until we hit the next mirror
            energizedMap[nextPos.Y][nextPos.X] = true;
        }

        var direction = pointer.Direction;
        switch (map[nextPos.Y][nextPos.X]) {
            case '/':
                yield return direction switch {
                    Direction.Right => new Pointer(Direction.Up, nextPos),
                    Direction.Left => new Pointer(Direction.Down, nextPos),
                    Direction.Up => new Pointer(Direction.Right, nextPos),
                    Direction.Down => new Pointer(Direction.Left, nextPos),
                    _ => throw new Exception()
                };
                break;
            case '\\':
                yield return direction switch {
                    Direction.Right => new Pointer(Direction.Down, nextPos),
                    Direction.Left => new Pointer(Direction.Up, nextPos),
                    Direction.Up => new Pointer(Direction.Left, nextPos),
                    Direction.Down => new Pointer(Direction.Right, nextPos),
                    _ => throw new Exception()
                };
                break;
            case '|' when direction != Direction.Up && direction != Direction.Down:
                yield return new Pointer(Direction.Up, nextPos);
                yield return new Pointer(Direction.Down, nextPos);
                break;
            case '-' when direction != Direction.Left && direction != Direction.Right:
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

        public bool IsWithinBounds(int maxX, int maxY) {
            return X >= 0 && X < maxX && Y >= 0 && Y < maxY;
        }
    }
}