namespace advent.of.code.day5;

internal static class Solution {

    internal static long Task1(StreamReader reader) {
        var dictionaries = Read(reader, out var seeds);
        return seeds.Select(s => dictionaries.Aggregate(s, (current, dictionary) => Find(dictionary, current))).Min();
    }

    internal static long Task2(StreamReader reader) {
        # region Read seeds and dictionaries
        var dictionaries = Read(reader, out var seedsList);
        var seeds = new List<Range>();
        for (var i = 0; i < seedsList.Count; i += 2) {
            seeds.Add(new Range(seedsList[i], seedsList[i + 1]));
        }

        // sort seeds so that the highest start value comes first
        seeds.Sort((a, b) => b.Start.CompareTo(a.Start));

        # endregion
        # region Approxmiate locations of seeds

        // create approximation window from seed with highest start value
        var approxWindow = (int)(seeds.First().Start * 0.00001);
        if (approxWindow == 0) approxWindow = 100;

        var approxSeedRange = seeds.First();
        var approxSeed = approxSeedRange.Start;
        var min = long.MaxValue;

        foreach (var seedRange in seeds) {
            Console.Out.WriteLine("Approximating seeds " + seedRange);

            var locationsWithSeeds = seedRange.ApproxValues(approxWindow).AsParallel()
                .Select(seed => (
                    location: dictionaries.Aggregate(seed, (current, dictionary) => Find(dictionary, current)),
                    currentSeed: seed
                ))
                .ToList();

            foreach (var (location, currentSeed) in locationsWithSeeds) {
                if (location >= min) continue;
                Console.Out.WriteLine("New minimum found: " + min);

                min = location;
                approxSeedRange = seedRange;
                approxSeed = currentSeed;
            }
        }

        # endregion

        Console.Out.WriteLine("------------------------------------------------");

        var range = approxSeedRange.WithApproximationWindow(approxSeed, approxWindow);

        Console.Out.WriteLine("Searching for seeds " + range);

        return range.AllValues().AsParallel()
            .Select(s => dictionaries.Aggregate(s, (current, dictionary) => Find(dictionary, current)))
            .Min();
    }

    private static Dictionary<Range, long>[] Read(StreamReader reader, out List<long> seeds) {
        var dictionaries = new[] {
            new Dictionary<Range, long>(), // soils
            new Dictionary<Range, long>(), // fertilizer
            new Dictionary<Range, long>(), // water
            new Dictionary<Range, long>(), // light
            new Dictionary<Range, long>(), // temperature
            new Dictionary<Range, long>(), // humidity
            new Dictionary<Range, long>() // location
        };

        var seedsLine = reader.ReadLine() ?? throw new Exception();
        seeds = seedsLine.Split(":")[1].Split(" ").Where(n => n != "").Select(long.Parse).ToList();

        // empty line
        reader.ReadLine();

        foreach (var dictionary in dictionaries) {
            // title
            reader.ReadLine();

            while (!reader.EndOfStream) {
                var line = reader.ReadLine() ?? throw new Exception();

                if (line == "") break;

                var s = line.Split(" ").Where(n => n != "").Select(long.Parse).ToList();
                dictionary.Add(new Range(s[1], s[2]), s[0]);
            }
        }

        return dictionaries;
    }

    private static long Find(Dictionary<Range, long> map, long val) {
        foreach (var entry in map) {
            if (entry.Key.Start > val || val > entry.Key.Start + entry.Key.Size) continue;

            var offset = val - entry.Key.Start;
            return entry.Value + offset;
        }

        return val;
    }

    private record Range(long Start, long Size) {
        public IEnumerable<long> AllValues() {
            for (var i = Start; i <= Start + Size; i++) {
                yield return i;
            }
        }

        public IEnumerable<long> ApproxValues(int approximationDistance) {
            for (var i = Start; i <= Start + Size; i += approximationDistance) {
                yield return i;
            }
        }

        public Range WithApproximationWindow(long approximatedStart, int approximationWindow) {
            var start = approximatedStart - approximationWindow < Start
                ? Start
                : approximatedStart - approximationWindow;
            var size = approximationWindow > Size - (start - Start)
                ? Size - (start - Start)
                : approximationWindow;
            return new Range(start, size);
        }
    }
}