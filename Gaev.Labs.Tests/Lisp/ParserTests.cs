using Gaev.Labs.Lisp;

namespace Gaev.Labs.Tests.Lisp;

public class ParserTests
{
    [Test]
    public void It_should_parse_operator()
    {
        // Given
        var parser = new Parser();

        // When
        var actual = parser.Parse("(+ 2 2)");

        // Then
        var expected = new OperatorNode("+", [new LiteralNode(2), new LiteralNode(2)]);
        actual.Should().BeEquivalentTo(expected);
    }
}
