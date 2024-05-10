using NUnit.Framework.Internal;

namespace Gaev.Labs.Interpreters.Tests.Lisp;

public class VariableDefinitionNodeTests
{
    private static Randomizer Random => TestContext.CurrentContext.Random;

    [Test]
    public void Evaluate()
    {
        // Given
        var scope = Substitute.For<IScope>();
        var name = Random.GetString();
        var value = Random.Next();
        var expression = Substitute.For<INode>();
        expression.Evaluate(scope).Returns(value);

        // When
        var node = new VariableDefinitionNode(name, expression);
        var actual = node.Evaluate(scope);

        // Then
        actual.Should().Be(value);
        scope.Received().DefineVariable(node);
    }
}
