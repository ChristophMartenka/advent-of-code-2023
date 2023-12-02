using advent.of.code.util;
using NUnit.Framework;

namespace advent.of.code.day0;

[TestFixture]
internal class Test {

    [Test]
    public void Task1_TestInput() {
        var result = Solution.Task1(FileReader.GetFileForDay(0, "testInput.txt"));
        Assert.AreEqual(result, 0);
    }

    [Test]
    public void Task1_RealInput() {
        var result = Solution.Task1(FileReader.GetInputForDay(0));
        Assert.AreEqual(result, 0);
    }

    [Test]
    public void Task2_TestInput() {
        var result = Solution.Task2(FileReader.GetFileForDay(0, "testInput.txt"));
        Assert.AreEqual(result, 0);
    }

    [Test]
    public void Task2_RealInput() {
        var result = Solution.Task2(FileReader.GetInputForDay(0));
        Assert.AreEqual(result, 0);
    }
}