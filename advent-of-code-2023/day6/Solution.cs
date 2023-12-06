namespace advent.of.code.day6; 

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var line = reader.ReadLine() ?? throw new Exception();
        var times = line.Split(":")[1].Split(" ").Where(n => n != "").Select(int.Parse).ToArray();
            
        line = reader.ReadLine() ?? throw new Exception();
        var distancesToBeat = line.Split(":")[1].Split(" ").Where(n => n != "").Select(int.Parse).ToArray();
        
        var total = 1;
        
        for (var i = 0; i < times.Length; i++) {
            total *= GetPossibilitiesToBeat(times[i], distancesToBeat[i]);
        }
        
        return total;
    }

    internal static long Task2(StreamReader reader) {
        var line = reader.ReadLine() ?? throw new Exception();
        var time = long.Parse(line.Split(":")[1].Replace(" ", ""));
            
        line = reader.ReadLine() ?? throw new Exception();
        var distanceToBeat = long.Parse(line.Split(":")[1].Replace(" ", ""));
        
        return GetPossibilitiesToBeat(time, distanceToBeat);
    }

    private static int GetPossibilitiesToBeat(long remainingTime, long distanceToBeat) {
        var possibilitiesToBeat = 0;

        for (var time = 0; time < remainingTime; time++) {
            if (time * (remainingTime - time) > distanceToBeat) {
                possibilitiesToBeat++;
            }
        }

        return possibilitiesToBeat;
    }
}