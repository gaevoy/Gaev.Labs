using System.Text.Json;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace Gaev.Labs.Interpreters.Tests;

public class KnapsackSolverTests
{
    [TestCase("2, 3, 4, 5", "3, 4, 5, 6", 5, 7)]
    [TestCase("5, 4, 6, 3, 2", "10, 40, 30, 50, 10", 10, 100)]
    [TestCase("3, 5, 7, 9, 2, 1", "5, 12, 8, 10, 3, 2", 12, 22)]
    [TestCase("1, 2, 4, 6, 3", "1, 4, 5, 6, 7", 8, 13)]
    [TestCase("6, 7", "10, 20", 5, 0)]
    public void Solve(string weights, string values, int capacity, int expectedValue)
    {
        // Given
        var w = weights.Split(',').Select(int.Parse).ToArray();
        var v = values.Split(',').Select(int.Parse).ToArray();

        // When
        var nodes = new List<Node>();
        var root = new Node(default, default, default);
        nodes.Add(root);
        for (var i = 0; i < w.Length; i++)
        {
            var weight = w[i];
            var value = v[i];
            foreach (var node in nodes.ToList())
            {
                var child = new Node(weight, value, node);
                if (child.IsFit(capacity))
                    nodes.Add(child);
            }
        }

        var resultingNode = nodes.MaxBy(e => e.TotalValue);
        Console.WriteLine(JsonSerializer.Serialize(resultingNode, new JsonSerializerOptions { WriteIndented = true }));

        // Then
        Assert.That(resultingNode?.TotalValue, Is.EqualTo(expectedValue));
    }
}

record Node(int Weight, int Value, Node? Parent)
{
    public readonly int TotalWeight = Weight + (Parent?.TotalWeight ?? 0);
    public readonly int TotalValue = Value + (Parent?.TotalValue ?? 0);
    public bool IsFit(int capacity) => TotalWeight <= capacity;
}
