namespace advent.of.code.day10;

internal static class Solution {

    private enum Direction {
        Left,
        Right,
        Up,
        Down,
        None
    }

    internal static int Task1(StreamReader reader) {
        var map = reader.ReadToEnd().Split(Environment.NewLine);
        return GetDistances(map).Values.Max();
    }

    internal static int Task2(StreamReader reader) {
        var map = reader.ReadToEnd().Split(Environment.NewLine);

        var distances = GetDistances(map);

        // Replace points not included in loop
        for (var y = 0; y < map.Length; y++) {
            for (var x = 0; x < map[y].Length; x++) {
                if (!distances.ContainsKey(new Point(x, y))) {
                    map[y] = map[y].Remove(x, 1).Insert(x, ".");
                }
            }
        }

        // Count all points enclosed by loop
        return map.Sum(line => line.Where((_, i) => IsPointEnclosed(line, i)).Count());
    }

    private static Dictionary<Point, int> GetDistances(IReadOnlyList<string> map) {
        // var pointers = new Pointer[2];
        var distances = new Dictionary<Point, int>();

        var pointers = new Queue<Pointer>();

        for (var y = 0; y < map.Count; y++) {
            if (!map[y].Contains('S')) continue;

            var x = map[y].IndexOf('S');
            var point = new Point(x, y);
            pointers.Enqueue(new Pointer(Direction.None, point));
            pointers.Enqueue(new Pointer(Direction.None, point));
            distances.Add(point, 0);
            break;
        }

        var done = false;
        while (!done) {
            var pointer = pointers.Dequeue();
            // initial pointer have no direction yet
            if (pointer.Direction == Direction.None) {
                foreach (var direction in new[] { Direction.Left, Direction.Right, Direction.Up, Direction.Down }) {
                    var next = Next(pointer with { Direction = direction }, distances, map);
                    if (next.Direction == Direction.None) continue;
                    pointers.Enqueue(next);
                }
            } else {
                var next = Next(pointer, distances, map);
                if (next.Direction == Direction.None) {
                    done = true;
                    continue;
                }

                pointers.Enqueue(next);
            }
        }

        return distances;
    }

    private static Pointer Next(Pointer pointer, IDictionary<Point, int> distances, IReadOnlyList<string> map) {
        var next = pointer.Pos.Offset(pointer.Direction);

        // Point was already checked, loop complete
        if (distances.ContainsKey(next)) {
            return new Pointer(Direction.None, next);
        }

        var nextDirection = GetDirection(map, pointer.Pos, next);
        if (nextDirection == Direction.None) {
            return pointer with { Direction = Direction.None };
        }

        distances.Add(next, distances[pointer.Pos] + 1);
        return new Pointer(nextDirection, next);
    }

    private static Direction GetDirection(IReadOnlyList<string> map, Point current, Point next) {
        if (next.Y < 0 || next.Y >= map.Count || next.X < 0 || next.X >= map[next.Y].Length) return Direction.None;
        return map[next.Y][next.X] switch {
            '|' when current.X != next.X => Direction.None,
            '|' => current.Y < next.Y ? Direction.Down : Direction.Up,
            '-' when current.Y != next.Y => Direction.None,
            '-' => current.X < next.X ? Direction.Right : Direction.Left,
            'L' when current.X < next.X || current.Y > next.Y => Direction.None,
            'L' => current.X != next.X ? Direction.Up : Direction.Right,
            'J' when current.X > next.X || current.Y > next.Y => Direction.None,
            'J' => current.X != next.X ? Direction.Up : Direction.Left,
            '7' when current.X > next.X || current.Y < next.Y => Direction.None,
            '7' => current.X != next.X ? Direction.Down : Direction.Left,
            'F' when current.X < next.X || current.Y < next.Y => Direction.None,
            'F' => current.X != next.X ? Direction.Down : Direction.Right,
            _ => Direction.None
        };
    }

    private static bool IsPointEnclosed(string horizontalLine, int index) {
        // Count all walls or U - loops ahead of point, if the number is odd the point is enclosed
        return horizontalLine[index] == '.' && horizontalLine[index..].Count(c => c is '|' or 'L' or 'J') % 2 == 1;
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