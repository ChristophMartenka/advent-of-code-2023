namespace advent.of.code.day5;

internal static class Solution {

    internal static long Task1(StreamReader reader) {
        var dictionaries = Read(reader, out var seeds);
        var min = long.MaxValue;

        foreach (var s in seeds) {
            var val = dictionaries.Aggregate(s, (current, dictionary) => Find(dictionary, current));

            if (val < min) {
                min = val;
                Console.Error.WriteLine(min);
            }
        }

        return min;
    }

    internal static long Task2(StreamReader reader) {
        var dictionaries = Read(reader, out var seedsList);
        var seeds = new List<Range>();
        for (var i = 0; i < seedsList.Count; i += 2) {
            seeds.Add(new Range(seedsList[i], seedsList[i + 1]));
        }

        seeds.Sort((a, b) => b.Start.CompareTo(a.Start));

        var approx = new SortedDictionary<long, Range>();
        var min = long.MaxValue;
        foreach (var seed in seeds) {
            Console.Error.WriteLine("approx " + seed);

            seed.ApproxValues(500).AsParallel()
                .Select(s => dictionaries.Aggregate(s, (current, dictionary) => Find(dictionary, current)))
                .ForAll(s => {
                    if (s < min) {
                        min = s;
                        Console.Error.WriteLine(min);
                        approx.TryAdd(s, seed);
                    }
                });

            Console.Error.WriteLine("------------------------------------------------");
        }

        Console.Error.WriteLine(approx.First().Value);

        approx.First().Value.AllValues().AsParallel()
            .Select(s => dictionaries.Aggregate(s, (current, dictionary) => Find(dictionary, current)))
            .ForAll(s => {
                if (s < min) {
                    min = s;
                    Console.Error.WriteLine(min);
                }
            });

        Console.Error.WriteLine("------------------------------------------------");

        return min;
    }

    private static Dictionary<Range, long>[] Read(StreamReader reader, out List<long> seeds) {
        var dictionaries = new[] {
            new Dictionary<Range, long>(), // soils
            new Dictionary<Range, long>(), // fertilizer
            new Dictionary<Range, long>(), // water
            new Dictionary<Range, long>(), // light
            new Dictionary<Range, long>(), // temperature
            new Dictionary<Range, long>(), // humidity
            new Dictionary<Range, long>()  // location
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

    private record Range(long Start, long Size) : IComparable<Range> {
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

        public int CompareTo(Range? other) {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : Start.CompareTo(other.Start);
        }
    }
}