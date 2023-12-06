using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day6;

[TestFixture]
internal class Test {

    private const int Day = 6;

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 288)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 32076)]
    public int Task1_Test(int day, string fileName) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName));
    }

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 71503)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 34278221)]
    public long Task2_Test(int day, string fileName) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName));
    }
}