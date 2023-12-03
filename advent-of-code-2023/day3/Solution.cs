namespace advent.of.code.day3;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var total = 0;

        ReadSlidingWindow(reader, lineBuffer => {
            for (var i = 0; i < lineBuffer[0].Length; i++) {
                if (lineBuffer[1][i] == '.' || char.IsNumber(lineBuffer[1][i])) continue;

                for (var y = -1; y < 2; y++) {
                    for (var x = -1; x < 2; x++) {
                        var curLine = lineBuffer[1 + y];
                        var number = ExtractNumberAndReplace(ref curLine, i + x);

                        if (number < 0) continue;

                        lineBuffer[1 + y] = curLine;
                        total += number;
                    }
                }
            }
        });

        return total;
    }

    internal static int Task2(StreamReader reader) {
        var total = 0;
        
        ReadSlidingWindow(reader, lineBuffer => {
            for (var i = 0; i < lineBuffer[0].Length; i++) {
                if (lineBuffer[1][i] != '*') continue;

                var countOfAdjacent = 0;
                var product = 1;
                
                for (var y = -1; y < 2; y++) {
                    for (var x = -1; x < 2; x++) {
                        var curLine = lineBuffer[1 + y];
                        var number = ExtractNumberAndReplace(ref curLine, i + x);

                        if (number < 0) continue;

                        lineBuffer[1 + y] = curLine;
                        product *= number;
                        countOfAdjacent++;
                    }
                }

                if (countOfAdjacent == 2) {
                    total += product;
                }
            }
        });

        return total;
    }

    private static void ReadSlidingWindow(StreamReader reader, Action<List<string>> lineBufferHandler) {
        const int windowSize = 3;
        var lineBuffer = new List<string>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            // insert empty line at the beginning so that we can always access the previous line
            if (lineBuffer.Count == 0) {
                lineBuffer.Add(".".PadRight(line.Length, '.'));
            }

            lineBuffer.Add(line);
            if (lineBuffer.Count < windowSize) continue;
            if (lineBuffer.Count > windowSize) lineBuffer.RemoveAt(0);

            lineBufferHandler.Invoke(lineBuffer);
        }
    }

    private static int ExtractNumberAndReplace(ref string line, int index) {
        if (index < 0 || index >= line.Length) return -1;
        if (!char.IsDigit(line[index])) return -1;

        // determine start and length of number
        var startIndex = index;
        while (startIndex > 0 && char.IsDigit(line[startIndex - 1])) {
            startIndex--;
        }

        var length = index - startIndex;
        while (startIndex + length < line.Length && char.IsDigit(line[startIndex + length])) {
            length++;
        }

        var number = int.Parse(line.Substring(startIndex, length));

        // replace number with dots
        line = line.Remove(startIndex, length);
        line = line.Insert(startIndex, ".".PadLeft(length, '.'));
        return number;
    }
}