using System.Text.RegularExpressions;

namespace advent.of.code.day12;

internal static partial class Solution {

    [GeneratedRegex("^(?<springs>[?.#]+)\\s(?<groups>[0-9,]+)$")]
    private static partial Regex SpringGroupRegex();

    internal static long Task1(StreamReader reader) {
        var regex = SpringGroupRegex();
        var cache = new Cache();
        return reader.ReadToEnd().Split(Environment.NewLine)
            .Select(line => {
                var match = regex.Match(line);

                var springs = match.Groups["springs"].Value;
                var groups = match.Groups["groups"].Value.Split(",").Select(int.Parse).ToList();

                return GetTotalArrangements(springs, groups, 0, cache);
            })
            .Sum();
    }

    internal static long Task2(StreamReader reader) {
        var regex = SpringGroupRegex();
        var cache = new Cache();
        return reader.ReadToEnd().Split(Environment.NewLine)
            .Select(line => {
                var match = regex.Match(line);

                var springs = match.Groups["springs"].Value;
                var groups = match.Groups["groups"].Value.Split(",").Select(int.Parse).ToList();

                var newSprings = springs;
                var newGroups = new List<int>(groups);
                for (var i = 0; i < 4; i++) {
                    newSprings += "?" + springs;
                    newGroups.AddRange(groups);
                }

                return GetTotalArrangements(newSprings, newGroups, 0, cache);
            })
            .Sum();
    }

    private static long GetTotalArrangements(string springs, List<int> groups, int streak, Cache cache) {
        // springs string is not long enough to contain any further groups
        if (springs.Length == 0) {
            if (groups.Count == 1 && streak == groups[0]) {
                return 1;
            }

            return groups.Count == 0 ? 1 : 0;
        }

        // no groups left, but springs still contain '#'
        if (groups.Count == 0) {
            return streak != 0 || springs.IndexOf('#') < 0 ? 1 : 0;
        }

        // currently active search has no streak of '#'
        if (streak == 0) {
            var total = cache.Get(springs, groups);
            // if value was cached return it
            if (total >= 0) {
                return total;
            }

            var nextSprings = springs[1..];
            total = springs[0] switch {
                // call without streak
                '.' => GetTotalArrangements(nextSprings, groups, 0, cache),
                // call with started streak of '#'
                '#' => GetTotalArrangements(nextSprings, groups, 1, cache),
                // fork one with started streak of '#' and one without
                '?' => GetTotalArrangements(nextSprings, groups, 1, cache)
                       + GetTotalArrangements(nextSprings, groups, 0, cache),
                _ => throw new Exception()
            };
            cache.Add(springs, groups, total);
            return total;
        }

        // group is complete
        if (streak == groups[0]) {
            // next spring should not be '#'
            if (springs[0] == '#') return 0;

            // call next without completed group
            return GetTotalArrangements(springs[1..], groups.GetRange(1, groups.Count - 1), 0, cache);
        }

        // group is incomplete
        return springs[0] switch {
            // next must not be '.'
            '.' => 0,
            // must treat '?' as '#' and increment the index
            '?' or '#' => GetTotalArrangements(springs[1..], groups, streak + 1, cache),
            _ => throw new Exception()
        };
    }

    private class Cache {
        private readonly Dictionary<string, long> _cache = new();

        public void Add(string springs, IEnumerable<int> groups, long total) => _cache.TryAdd(ToKey(springs, groups), total);

        public long Get(string springs, IEnumerable<int> groups) => _cache.GetValueOrDefault(ToKey(springs, groups), -1);

        private static string ToKey(string springs, IEnumerable<int> groups) => springs + string.Join(",", groups);
    }
}