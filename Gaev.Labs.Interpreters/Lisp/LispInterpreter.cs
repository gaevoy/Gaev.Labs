namespace Gaev.Labs.Interpreters.Lisp;

public class LispInterpreter(IScope globalScope, IParser parser)
{
    public LispInterpreter() : this(new Scope(), new Parser())
    {
    }

    public int Evaluate(string expression)
    {
        var node = parser.Parse(expression);
        return node.Evaluate(globalScope);
    }
}
