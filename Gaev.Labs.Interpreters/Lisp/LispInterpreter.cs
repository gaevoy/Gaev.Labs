namespace Gaev.Labs.Interpreters.Lisp;

public class LispInterpreter
{
    private IScope GlobalScope { get; } = new Scope();

    public int Evaluate(string expression)
    {
        var parser = new Parser(expression);
        var node = parser.ParseExpression();
        return node.Evaluate(GlobalScope);
    }
}
