using System.Text.RegularExpressions;

namespace advent.of.code.day18;

internal static partial class Solution {

    [GeneratedRegex(@"^(?<direction>[UDLR])\s(?<amount>\d+)\s\(#(?<color>[0-9a-z]+)\)$")]
    private static partial Regex DigPlanRegex();

    private enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    private delegate void ParserDelegate(Match match, out Direction direction, out long amount);

    internal static long Task1(StreamReader reader) {
        return GetCountOfDugOutPoints(reader, Parser);

        void Parser(Match match, out Direction direction, out long amount) {
            direction = match.Groups["direction"].Value switch {
                "U" => Direction.Up,
                "D" => Direction.Down,
                "L" => Direction.Left,
                "R" => Direction.Right,
                _ => throw new Exception()
            };
            amount = long.Parse(match.Groups["amount"].Value);
        }
    }

    internal static long Task2(StreamReader reader) {
        return GetCountOfDugOutPoints(reader, Parser);

        void Parser(Match match, out Direction direction, out long amount) {
            var color = match.Groups["color"].Value;
            direction = color[^1] switch {
                '3' => Direction.Up,
                '1' => Direction.Down,
                '2' => Direction.Left,
                '0' => Direction.Right,
                _ => throw new Exception()
            };
            amount = Convert.ToInt64(color[..^1], 16);
        }
    }

    private static long GetCountOfDugOutPoints(StreamReader reader, ParserDelegate parser) {
        var regex = DigPlanRegex();

        var current = new Point(0, 0);
        var points = new List<Point> { current };

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            parser(regex.Match(line), out var direction, out var amount);
            current = current.Offset(direction, amount);
            points.Add(current);
        }

        return GetInnerSurfaceArea(points) + GetPerimeter(points) / 2 + 1;
    }

    private static long GetInnerSurfaceArea(IReadOnlyList<Point> points) {
        // shoelace formula
        return points.Select((point, i) => {
            var next = i == points.Count - 1 ? points[0] : points[i + 1];
            return (point.Y + next.Y) * (point.X - next.X);
        }).Sum() / 2;
    }

    private static long GetPerimeter(IReadOnlyList<Point> points) {
        return points.Select((point, i) => point.DistanceTo(i == points.Count - 1 ? points[0] : points[i + 1])).Sum();
    }

    private record Point(long X, long Y) {
        public Point Offset(Direction direction, long amount) {
            return direction switch {
                Direction.Up => this with { Y = Y - amount },
                Direction.Down => this with { Y = Y + amount },
                Direction.Left => this with { X = X - amount },
                Direction.Right => this with { X = X + amount },
                _ => this
            };
        }

        public long DistanceTo(Point other) {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
    }
}