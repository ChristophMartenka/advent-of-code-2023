using System.Text.RegularExpressions;

namespace advent.of.code.day19;

internal static partial class Solution {

    [GeneratedRegex(
        @"^(?<name>[a-z]+){((?<rule>(?<category>[xmas])(?<comparison>[><])(?<number>\d+):(?<target>[a-zAR]+)),)+(?<default>[a-zAR]+)+}$")]
    private static partial Regex WorkflowRegex();

    [GeneratedRegex(@"^{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)}$")]
    private static partial Regex RatingsRegex();

    internal static long Task1(StreamReader reader) {
        var workflows = ReadWorkflows(reader);
        var ratingRegex = RatingsRegex();

        var total = 0L;

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            var match = ratingRegex.Match(line);

            var rating = new Dictionary<char, long> {
                { 'x', long.Parse(match.Groups["x"].Value) },
                { 'm', long.Parse(match.Groups["m"].Value) },
                { 'a', long.Parse(match.Groups["a"].Value) },
                { 's', long.Parse(match.Groups["s"].Value) }
            };

            var workflowName = "in";
            while (workflowName != "R" && workflowName != "A") {
                workflowName = workflows[workflowName].Rules
                    .Where(rule =>
                        rule.Comparison == ">"
                            ? rating[rule.Category] > rule.Number
                            : rating[rule.Category] < rule.Number)
                    .Select(rule => rule.Target)
                    .FirstOrDefault(workflows[workflowName].Default);
            }

            if (workflowName == "A") total += rating.Values.Sum();
        }

        return total;
    }

    internal static long Task2(StreamReader reader) {
        var workflows = ReadWorkflows(reader);
        var ratingRange = new Dictionary<char, Range> {
            { 'x', new Range(1, 4001) },
            { 'm', new Range(1, 4001) },
            { 'a', new Range(1, 4001) },
            { 's', new Range(1, 4001) }
        };

        return CountAcceptedParts(workflows, ratingRange, "in");
    }

    private static Dictionary<string, Workflow> ReadWorkflows(StreamReader reader) {
        var workflowRegex = WorkflowRegex();
        var workflows = new Dictionary<string, Workflow>();
        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();
            if (line == "") break;
            var match = workflowRegex.Match(line);

            var rules = match.Groups["rule"].Captures
                .Select((_, i) => new Rule(
                    match.Groups["category"].Captures[i].Value[0],
                    match.Groups["comparison"].Captures[i].Value,
                    int.Parse(match.Groups["number"].Captures[i].Value),
                    match.Groups["target"].Captures[i].Value
                ))
                .ToList();

            var workflowName = match.Groups["name"].Value;
            workflows.Add(workflowName, new Workflow(match.Groups["default"].Value, rules));
        }

        return workflows;
    }

    private static long CountAcceptedParts(
        IReadOnlyDictionary<string, Workflow> workflows,
        Dictionary<char, Range> ratingRange,
        string workflowName
    ) {
        switch (workflowName) {
            case "R": return 0;
            case "A":
                return ratingRange.Values
                    .Select(range => (long)range.End.Value - range.Start.Value)
                    .Aggregate((a, b) => a * b);
        }

        var workflow = workflows[workflowName];

        var nextWorkflowCalls = new List<(string workflowName, Dictionary<char, Range> ratingRange)>();

        foreach (var rule in workflow.Rules) {
            var range = ratingRange[rule.Category];
            // range is outside of rule, so it can be skipped
            if (!range.Contains(rule.Number)) break;

            // range is fully covered by rule, so we can check the next rule with current ranges
            if (rule.Comparison == "<" && range.End.Value < rule.Number ||
                rule.Comparison == ">" && range.Start.Value > rule.Number) {
                nextWorkflowCalls.Add((rule.Target, ratingRange));
                break;
            }

            Range included;
            Range excluded;

            // range is partially covered by rule, so we need to split the range (one included, one excluded)
            if (rule.Comparison == "<") {
                range.SplitInclusive(rule.Number, out included, out excluded);
            } else {
                range.SplitInclusive(rule.Number + 1, out excluded, out included);
            }

            // copy current rating range and update the included range
            var includedRatingRange = new Dictionary<char, Range>(ratingRange) { [rule.Category] = included };
            nextWorkflowCalls.Add((rule.Target, includedRatingRange));

            // update current rating range with excluded range
            ratingRange[rule.Category] = excluded;
        }

        // check the default workflow with the excluded ranges
        nextWorkflowCalls.Add((workflow.Default, ratingRange));

        // sum up all results of the next workflows
        return nextWorkflowCalls
            .Select(next => CountAcceptedParts(workflows, next.ratingRange, next.workflowName))
            .Sum();
    }

    private static bool Contains(this Range range, int number) {
        return range.Start.Value < number && range.End.Value > number;
    }

    private static void SplitInclusive(this Range range, int number, out Range startToNumber, out Range numberToEnd) {
        startToNumber = new Range(range.Start, number);
        numberToEnd = new Range(number, range.End);
    }

    private record Workflow(string Default, List<Rule> Rules);

    private record Rule(char Category, string Comparison, int Number, string Target);
}