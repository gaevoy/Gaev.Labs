namespace Gaev.Labs.Interpreters.Tests.Lisp;

[TestFixture]
public class LispInterpreterTests
{
    private readonly LispInterpreter _interpreter = new();

    [Test]
    public void TestLiteralEvaluation()
    {
        Assert.That(_interpreter.Evaluate("5"), Is.EqualTo(5));
    }

    [Test]
    public void TestArithmeticOperations()
    {
        Assert.That(_interpreter.Evaluate("(+ 2 3)"), Is.EqualTo(5));
        Assert.That(_interpreter.Evaluate("(* 4 5)"), Is.EqualTo(20));
        Assert.That(_interpreter.Evaluate("(* (+ 2 3) (- 5 2))"), Is.EqualTo(15));
    }

    [Test]
    public void TestVariableDefinitionAndUsage()
    {
        _interpreter.Evaluate("(define a 10)");
        Assert.That(_interpreter.Evaluate("(+ a 5)"), Is.EqualTo(15));
    }

    [Test]
    public void TestConditionalLogic()
    {
        Assert.That(_interpreter.Evaluate("(if (> 10 5) (+ 1 1) (+ 2 2))"), Is.EqualTo(2));
        Assert.That(_interpreter.Evaluate("(if (< 10 5) (+ 1 1) (+ 2 2))"), Is.EqualTo(4));
    }

    [Test]
    public void TestFunctionDefinitionAndExecution()
    {
        _interpreter.Evaluate("(define (add x y) (+ x y))");
        var actual = _interpreter.Evaluate("(add 3 4)");
        Assert.That(actual, Is.EqualTo(7));
    }

    [Test]
    public void TestFunctionWithConditional()
    {
        _interpreter.Evaluate("(define (check x) (if (> x 5) 1 0))");
        Assert.That(_interpreter.Evaluate("(check 6)"), Is.EqualTo(1));
        Assert.That(_interpreter.Evaluate("(check 5)"), Is.EqualTo(0));
    }
}
