using Gaev.Labs.Lisp;

namespace Gaev.Labs.Tests.Lisp;

[TestFixture]
public class LispInterpreterFunctionalTests
{
    private readonly LispInterpreter _interpreter = new();

    [Test]
    public void EvaluateLiterals()
    {
        _interpreter.Evaluate("5").Should().Be(5);
    }

    [Test]
    public void EvaluateOperations()
    {
        _interpreter.Evaluate("(+ 2 3)").Should().Be(5);
        _interpreter.Evaluate("(* 4 5)").Should().Be(20);
        _interpreter.Evaluate("(* (+ 2 3) (- 5 2))").Should().Be(15);
    }

    [Test]
    public void EvaluateVariableDefinitionAndUsage()
    {
        _interpreter.Evaluate("(define a 10)");
        _interpreter.Evaluate("(+ a 5)").Should().Be(15);
    }

    [Test]
    public void EvaluateConditionalLogic()
    {
        _interpreter.Evaluate("(if (> 10 5) (+ 1 1) (+ 2 2))").Should().Be(2);
        _interpreter.Evaluate("(if (< 10 5) (+ 1 1) (+ 2 2))").Should().Be(4);
    }

    [Test]
    public void EvaluateFunctionDefinitionAndExecution()
    {
        _interpreter.Evaluate("(define (add x y) (+ x y))");
        var actual = _interpreter.Evaluate("(add 3 4)");
        actual.Should().Be(7);
    }

    [Test]
    public void EvaluateFunctionWithConditional()
    {
        _interpreter.Evaluate("(define (check x) (if (> x 5) 1 0))");
        _interpreter.Evaluate("(check 6)").Should().Be(1);
        _interpreter.Evaluate("(check 5)").Should().Be(0);
    }
}
