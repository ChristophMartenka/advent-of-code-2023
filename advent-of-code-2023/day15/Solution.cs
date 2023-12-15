using System.Text;

namespace advent.of.code.day15;

internal static class Solution {

    internal static int Task1(StreamReader reader) {
        var line = reader.ReadLine() ?? throw new Exception();
        return line.Split(',').Sum(Hash);
    }

    internal static int Task2(StreamReader reader) {
        var line = reader.ReadLine() ?? throw new Exception();
        
        var boxes = new List<Lens>[256];
        for (var i = 0; i < boxes.Length; i++) {
            boxes[i] = new List<Lens>();
        }

        foreach (var lensEntry in line.Split(',')) {
            var labelAndValue = lensEntry.Split('=', '-');
            var label = labelAndValue[0];

            var box = boxes[Hash(label)];

            if (lensEntry.Contains('=')) {
                var lens = new Lens(label, int.Parse(labelAndValue[1]));
                var index = box.FindIndex(containedLens => containedLens.Label == label);
                if (index >= 0) {
                    box[index] = lens;
                } else {
                    box.Add(lens);
                }
            } else {
                box.RemoveAll(lens => lens.Label == label);
            }
        }

        return GetTotalFocusingPower(boxes);
    }

    private static int Hash(string s) {
        var currentValue = 0;
        foreach (var c in Encoding.ASCII.GetBytes(s)) {
            currentValue += c;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    private static int GetTotalFocusingPower(IEnumerable<List<Lens>> boxes) {
        return boxes.Select((box, i) => (i + 1) * GetTotalBoxFocusingPower(box)).Sum();
    }

    private static int GetTotalBoxFocusingPower(IEnumerable<Lens> box) {
        return box.Select((lens, i) => (i + 1) * lens.FocalLength).Sum();
    }

    private record Lens(string Label, int FocalLength);
}