using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day8;

[TestFixture]
internal class Test {

    private const int Day = 8;

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 2)]
    [TestCase(Day, "testInput-2.txt", TestName = "Day {0} Part 1 should be successful with second test input", ExpectedResult = 6)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 13771)]
    public long Task1_Test(int day, string fileName) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName));
    }

    [TestCase(Day, "testInput-part2.txt", TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 6)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 13129439557681)]
    public long Task2_Test(int day, string fileName) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName));
    }
}