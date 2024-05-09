using System;
using System.Collections.Immutable;
using System.Linq;

namespace Gaev.Labs.Interpreters.Lisp;

public interface INode
{
    int Evaluate(Scope scope);
}

public class LiteralNode(int value) : INode
{
    public int Evaluate(Scope scope) => value;
}

public class VariableNode(string name) : INode
{
    public int Evaluate(Scope scope) => scope.GetVariable(name).Evaluate(scope);
}

public class OperatorNode(string @operator, ImmutableList<INode> operands) : INode
{
    public static readonly string[] AllowedOperators = ["+", "-", "*", "/", "<", ">"];

    public int Evaluate(Scope scope)
    {
        var results = operands.Select(op => op.Evaluate(scope)).ToList();
        return @operator switch
        {
            "+" => results.Sum(),
            "-" => results.Aggregate((a, b) => a - b),
            "*" => results.Aggregate(1, (a, b) => a * b),
            "/" => results.Aggregate((a, b) => a / b),
            ">" => results.Zip(results.Skip(1), (a, b) => a > b).All(x => x) ? 1 : 0,
            "<" => results.Zip(results.Skip(1), (a, b) => a < b).All(x => x) ? 1 : 0,
            _ => throw new Exception($"Unsupported operator {@operator}")
        };
    }
}

public class VariableDefinitionNode(string name, INode expression) : INode
{
    public string Name { get; } = name;

    public int Evaluate(Scope scope)
    {
        var value = expression.Evaluate(scope);
        scope.DefineVariable(this);
        return value;
    }
}

public class IfNode(INode condition, INode thenBranch, INode elseBranch) : INode
{
    public int Evaluate(Scope scope)
    {
        return condition.Evaluate(scope) != 0
            ? thenBranch.Evaluate(scope)
            : elseBranch.Evaluate(scope);
    }
}

public class FunctionDefinitionNode(string name, ImmutableList<string> parameters, INode body) : INode
{
    public string Name { get; } = name;
    public ImmutableList<string> Parameters { get; } = parameters;
    public INode Body { get; } = body;

    public int Evaluate(Scope scope)
    {
        scope.DefineFunction(this);
        return 0; // Conventionally, defining a function returns 0 or void.
    }
}

public class FunctionCallNode(string functionName, ImmutableList<INode> arguments) : INode
{
    public int Evaluate(Scope scope)
    {
        var function = scope.GetFunction(functionName);
        if (function.Parameters.Count != arguments.Count)
            throw new Exception("Argument count mismatch");
        var functionScope = new Scope();
        for (var i = 0; i < function.Parameters.Count; i++)
            functionScope.DefineVariable(new VariableDefinitionNode(function.Parameters[i], arguments[i]));
        return function.Body.Evaluate(functionScope);
    }
}
