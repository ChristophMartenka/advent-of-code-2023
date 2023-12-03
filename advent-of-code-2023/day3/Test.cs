using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day3;

[TestFixture]
internal class Test {

    [Test]
    public void Task1_TestInput() {
        var result = Solution.Task1(FileReader.GetFileForDay(3, "testInput.txt"));
        Assert.AreEqual(result, 4361);
    }

    [Test]
    public void Task1_RealInput() {
        var result = Solution.Task1(FileReader.GetInputForDay(3));
        Assert.AreEqual(result, 535078);
    }

    [Test]
    public void Task2_TestInput() {
        var result = Solution.Task2(FileReader.GetFileForDay(3, "testInput.txt"));
        Assert.AreEqual(result, 467835);
    }

    [Test]
    public void Task2_RealInput() {
        var result = Solution.Task2(FileReader.GetInputForDay(3));
        Assert.AreEqual(result, 75312571);
    }
}