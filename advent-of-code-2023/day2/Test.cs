using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day2;

[TestFixture]
internal class Test {

    [Test]
    public void Task1_TestInput() {
        var result = Solution.Task1(FileReader.GetFileForDay(2, "testInput.txt"));
        Assert.AreEqual(result, 8);
    }

    [Test]
    public void Task1_RealInput() {
        var result = Solution.Task1(FileReader.GetInputForDay(2));
        Assert.AreEqual(result, 2505);
    }

    [Test]
    public void Task2_TestInput() {
        var result = Solution.Task2(FileReader.GetFileForDay(2, "testInput.txt"));
        Assert.AreEqual(result, 2286);
    }

    [Test]
    public void Task2_RealInput() {
        var result = Solution.Task2(FileReader.GetInputForDay(2));
        Assert.AreEqual(result, 70265);
    }
}