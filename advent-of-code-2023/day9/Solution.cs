namespace advent.of.code.day9; 

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var list = ReadDifferenceLists(reader);
            for (var i = 0; i < list.Count; i++) {
                var l = list[i];
                if (i == 0) {
                    l.Add(0);
                } else {
                    l.Add(l.Last() + list[i - 1].Last());
                }
            }

            total += list.Last().Last();
        }

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var total = 0;
        while (!reader.EndOfStream) {
            var list = ReadDifferenceLists(reader);
            for (var i = 0; i < list.Count; i++) {
                var l = list[i];
                if (i == 0) {
                    l.Insert(0, 0);
                } else {
                    l.Insert(0, l.First() - list[i - 1].First());
                }
            }

            total += list.Last().First();
        }

        return total;
    }
    
    private static List<List<int>> ReadDifferenceLists(TextReader reader) {
        var line = reader.ReadLine() ?? throw new Exception();

        var numbers = line.Split(" ").Select(int.Parse).ToList();
            
        var list = new List<List<int>> { numbers };

        var curList = numbers;
        while (curList.Any(n => n != 0)) {
            var newList = new List<int>();
            for (var i = 0; i < curList.Count - 1; i++) {
                newList.Add(curList[i + 1] - curList[i]);
            }
            list.Add(newList);
            curList = newList;
        }

        list.Reverse();

        return list;
    }
}