namespace Gaev.Labs.Interpreters.Lisp;

public class LispInterpreter
{
    private readonly Scope _globalScope = new();

    public int Evaluate(string expression)
    {
        var parser = new Parser(expression);
        var node = parser.ParseExpression();
        return node.Evaluate(_globalScope);
    }
}
