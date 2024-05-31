using Gaev.Labs.Lisp;

namespace Gaev.Labs.Tests.Lisp;

public class OperatorNodeTests
{
    [TestCase("+", "2 2", 4)]
    [TestCase("+", "2 2 1", 5)]
    [TestCase("<", "2 4 16", 1)]
    [TestCase("<", "2 32 4", 0)]
    public void Evaluate(string @operator, string operands, int expected)
    {
        // Given
        var operandNodes = operands
            .Split(' ')
            .Select(int.Parse)
            .Select(e => new LiteralNode(e))
            .ToImmutableList<INode>();
        var operatorNode = new OperatorNode(@operator, operandNodes);

        // When
        var actual = operatorNode.Evaluate(new Scope());

        // Then
        actual.Should().Be(expected);
    }
}
