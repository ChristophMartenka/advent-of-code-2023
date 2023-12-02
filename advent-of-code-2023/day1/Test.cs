using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day1;

[TestFixture]
internal class Test {

    [Test]
    public void Task1_TestInput() {
        var result = Solution.Task1(FileReader.GetFileForDay(1, "testInput.txt"));
        Assert.AreEqual(result, 142);
    }

    [Test]
    public void Task1_RealInput() {
        var result = Solution.Task1(FileReader.GetInputForDay(1));
        Assert.AreEqual(result, 54708);
    }

    [Test]
    public void Task2_TestInput() {
        var result = Solution.Task2(FileReader.GetFileForDay(1, "testInput-part2.txt"));
        Assert.AreEqual(result, 281);
    }

    [Test]
    public void Task2_RealInput() {
        var result = Solution.Task2(FileReader.GetInputForDay(1));
        Assert.AreEqual(result, 54087);
    }
}