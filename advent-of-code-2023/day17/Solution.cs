namespace advent.of.code.day17;

internal static class Solution {

    private enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    private static readonly Direction[] Directions = { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

    internal static int Task1(StreamReader reader) {
        var map = ReadMap(reader);

        var pointers = new List<Pointer> {
            new(Direction.Right, new Point(0, 0), 0),
            new(Direction.Down, new Point(0, 0), 0)
        };
        var targetPoint = new Point(map[0].Length - 1, map.Count - 1);

        return GetShortestDistance(map, pointers, targetPoint, IsValidMove);

        bool IsValidMove(Pointer pointer, Direction direction) {
            // can not move back
            if (direction == pointer.Direction.Flip()) return false;
            // can not move in same direction more than 3 times
            return direction != pointer.Direction || pointer.SameDirectionCount != 3;
        }
    }

    internal static int Task2(StreamReader reader) {
        var map = ReadMap(reader);

        var pointers = new List<Pointer> {
            new(Direction.Right, new Point(0, 0), 0),
            new(Direction.Down, new Point(0, 0), 0)
        };
        var targetPoint = new Point(map[0].Length - 1, map.Count - 1);

        return GetShortestDistance(map, pointers, targetPoint, IsValidMove);

        bool IsValidMove(Pointer pointer, Direction direction) {
            // can not move back
            if (direction == pointer.Direction.Flip()) return false;
            // can not move in same direction less than 4 times
            if (direction != pointer.Direction && pointer.SameDirectionCount < 4) return false;
            // can not move in same direction more than 10 times
            return direction != pointer.Direction || pointer.SameDirectionCount != 10;
        }
    }

    private static List<int[]> ReadMap(StreamReader reader) {
        var map = new List<int[]>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            map.Add(line.Select(c => (int)char.GetNumericValue(c)).ToArray());
        }

        return map;
    }

    #region A* Algorithm with priority queue

    private static int GetShortestDistance(
        IReadOnlyList<int[]> map,
        List<Pointer> startingPointers,
        Point targetPoint,
        Func<Pointer, Direction, bool> movePredicate
    ) {
        var distances = new Dictionary<Pointer, int>();
        var queue = new PriorityQueue<Pointer, int>();

        startingPointers.ForEach(pointer => {
            queue.Enqueue(pointer, 0);
            distances.Add(pointer, 0);
        });

        var mapMax = new Point(map[0].Length, map.Count);

        while (queue.TryDequeue(out var pointer, out var priority)) {
            // found pointer with lowest priority
            if (pointer.Pos == targetPoint) return priority;

            foreach (var next in GetNext(pointer, mapMax, movePredicate)) {
                var nextPriority = distances[pointer] + map[next.Pos.Y][next.Pos.X];

                if (nextPriority >= distances.GetValueOrDefault(next, int.MaxValue)) continue;

                distances[next] = nextPriority;
                queue.Enqueue(next, nextPriority + next.Pos.ManhattenDistance(targetPoint));
            }
        }

        throw new Exception($"No path found from any ({string.Join(",", startingPointers)}) to {targetPoint}");
    }

    private static IEnumerable<Pointer> GetNext(Pointer pointer, Point max, Func<Pointer, Direction, bool> predicate) {
        foreach (var direction in Directions) {
            // check if predicate is fulfilled
            if (!predicate.Invoke(pointer, direction)) continue;

            var nextPoint = pointer.Pos.Offset(direction);

            // can not move outside of map
            if (!nextPoint.IsWithinBounds(max)) continue;

            var sameDirectionCount = direction == pointer.Direction ? pointer.SameDirectionCount + 1 : 1;
            yield return new Pointer(direction, nextPoint, sameDirectionCount);
        }
    }

    #endregion

    private static Direction Flip(this Direction direction) {
        return direction switch {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => throw new Exception()
        };
    }

    private record Pointer(Direction Direction, Point Pos, int SameDirectionCount);

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

        public int ManhattenDistance(Point other) {
            // Manhatten distance as heuristic, no tie breaker needed here
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public bool IsWithinBounds(Point max) {
            return X >= 0 && X < max.X && Y >= 0 && Y < max.Y;
        }
    }
}