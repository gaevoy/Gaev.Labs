using System.Text.Json;

// ReSharper disable NotAccessedPositionalProperty.Local

namespace Gaev.Labs.Tests;

public class KnapsackSolverTests
{
    [TestCase("2, 3, 4, 5", "3, 4, 5, 6", 5, 7)]
    [TestCase("5, 4, 6, 3, 2", "10, 40, 30, 50, 10", 10, 100)]
    [TestCase("3, 5, 7, 9, 2, 1", "5, 12, 8, 10, 3, 2", 12, 22)]
    [TestCase("1, 2, 4, 6, 3", "1, 4, 5, 6, 7", 8, 13)]
    [TestCase("6, 7", "10, 20", 5, 0)]
    public void Solve(string weightsAsString, string valuesAsString, int capacity, int expectedValue)
    {
        // Given
        var weights = weightsAsString.Split(',').Select(int.Parse).ToArray();
        var values = valuesAsString.Split(',').Select(int.Parse).ToArray();

        // When
        var result = SolveKnapsack(weights, values, capacity);

        // Then
        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        Assert.That(result?.TotalValue, Is.EqualTo(expectedValue));
    }

    private static Node? SolveKnapsack(int[] weights, int[] values, int capacity)
    {
        var nodes = new List<Node>();
        var root = new Node(0, 0, null);
        nodes.Add(root);
        for (var i = 0; i < weights.Length; i++)
        {
            var weight = weights[i];
            var value = values[i];
            foreach (var node in nodes.ToList())
            {
                var child = new Node(weight, value, node);
                if (child.CanFit(capacity))
                    nodes.Add(child);
            }
        }

        return nodes.MaxBy(e => e.TotalValue);
    }

    record Node(int Weight, int Value, Node? Parent)
    {
        public readonly int TotalWeight = Weight + (Parent?.TotalWeight ?? 0);
        public readonly int TotalValue = Value + (Parent?.TotalValue ?? 0);
        public bool CanFit(int capacity) => TotalWeight <= capacity;
    }
}
