using NUnit.Framework.Internal;

namespace Gaev.Labs.Interpreters.Tests.Lisp;

[TestFixture]
public class LispInterpreterTests
{
    private Randomizer Random => TestContext.CurrentContext.Random;

    [Test]
    public void Evaluate()
    {
        // Given
        var expression = Random.GetString();
        var expectedResult = Random.Next();
        var globalScope = Substitute.For<IScope>();
        var parsedNode = Substitute.For<INode>();
        parsedNode.Evaluate(Arg.Any<IScope>()).Returns(expectedResult);
        var parser = Substitute.For<IParser>();
        parser.Parse(Arg.Any<string>()).Returns(parsedNode);

        // When
        var interpreter = new LispInterpreter(globalScope, parser);
        var actualResult = interpreter.Evaluate(expression);

        // Then
        actualResult.Should().Be(expectedResult);
        parser.Received().Parse(expression);
        parsedNode.Received().Evaluate(globalScope);
    }
}
