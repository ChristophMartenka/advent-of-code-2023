using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day18;

[TestFixture]
internal class Test {

    private const int Day = 18;

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 1 should be successful with test input", ExpectedResult = 62)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 1 should be successful with real input", ExpectedResult = 52035)]
    public long Task1_Test(int day, string fileName) {
        return Solution.Task1(FileReader.GetFileForDay(day, fileName));
    }

    [TestCase(Day, "testInput.txt", TestName = "Day {0} Part 2 should be successful with test input", ExpectedResult = 952408144115)]
    [TestCase(Day, "input.txt", TestName = "Day {0} Part 2 should be successful with real input", ExpectedResult = 60612092439765)]
    public long Task2_Test(int day, string fileName) {
        return Solution.Task2(FileReader.GetFileForDay(day, fileName));
    }
}