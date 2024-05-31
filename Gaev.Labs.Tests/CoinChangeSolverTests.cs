using System.Text.Json;

namespace Gaev.Labs.Tests;

// ReSharper disable NotAccessedPositionalProperty.Local
public class CoinChangeSolverTests
{
    [TestCase("1, 2, 5", 11, 3)]
    [TestCase("25, 10, 5", 30, 2)]
    [TestCase("9, 6, 5, 1", 11, 2)]
    public void Check(string coinsAsString, int amount, int expectedNumberOfCoins)
    {
        // Given
        var coins = coinsAsString.Split(',').Select(int.Parse).ToArray();

        // When
        var result = SolveCoinChange(amount, coins);

        // Then
        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        Assert.That(result?.NumberOfCoins, Is.EqualTo(expectedNumberOfCoins));
    }

    record Node(int Coin, Node? Parent)
    {
        public readonly int TotalAmount = Coin + (Parent?.TotalAmount ?? 0);
        public readonly int NumberOfCoins = (Parent?.NumberOfCoins ?? -1) + 1;
        public bool CanFit(int amount) => TotalAmount < amount;
        public bool IsEqualTo(int amount) => TotalAmount == amount;
    }

    private static Node? SolveCoinChange(int amount, int[] coins)
    {
        var queue = new Queue<Node>();
        queue.Enqueue(new Node(0, null));
        while (queue.Any())
        {
            var node = queue.Dequeue();
            foreach (var coin in coins)
            {
                var child = new Node(coin, node);
                if (child.IsEqualTo(amount))
                    return child;

                if (child.CanFit(amount))
                    queue.Enqueue(child);
            }
        }

        return null;
    }
}
