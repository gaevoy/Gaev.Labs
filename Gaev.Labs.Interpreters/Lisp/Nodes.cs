using System;
using System.Collections.Immutable;
using System.Linq;

namespace Gaev.Labs.Interpreters.Lisp;

public interface INode
{
    int Evaluate(IScope scope);
}

public record LiteralNode(int Value) : INode
{
    public int Evaluate(IScope scope) => Value;
}

public record VariableNode(string Name) : INode
{
    public int Evaluate(IScope scope) => scope.GetVariable(Name).Evaluate(scope);
}

public record OperatorNode(string Operator, ImmutableList<INode> Operands) : INode
{
    public static bool CanHandle(string @operator) => @operator is "+" or "-" or "*" or "/" or "<" or ">";

    public int Evaluate(IScope scope)
    {
        var results = Operands.Select(op => op.Evaluate(scope)).ToList();
        return Operator switch
        {
            "+" => results.Sum(),
            "-" => results.Aggregate((a, b) => a - b),
            "*" => results.Aggregate(1, (a, b) => a * b),
            "/" => results.Aggregate((a, b) => a / b),
            ">" => results.Zip(results.Skip(1), (a, b) => a > b).All(x => x) ? 1 : 0,
            "<" => results.Zip(results.Skip(1), (a, b) => a < b).All(x => x) ? 1 : 0,
            _ => throw new Exception($"Unsupported operator {Operator}")
        };
    }
}

public record VariableDefinitionNode(string Name, INode Expression) : INode
{
    public string Name { get; } = Name;

    public int Evaluate(IScope scope)
    {
        var value = Expression.Evaluate(scope);
        scope.DefineVariable(this);
        return value;
    }
}

public record IfNode(INode Condition, INode ThenBranch, INode ElseBranch) : INode
{
    public int Evaluate(IScope scope)
    {
        return Condition.Evaluate(scope) != 0
            ? ThenBranch.Evaluate(scope)
            : ElseBranch.Evaluate(scope);
    }
}

public record FunctionDefinitionNode(string Name, ImmutableList<string> Parameters, INode Body) : INode
{
    public string Name { get; } = Name;
    public ImmutableList<string> Parameters { get; } = Parameters;
    public INode Body { get; } = Body;

    public int Evaluate(IScope scope)
    {
        scope.DefineFunction(this);
        return 0; // Conventionally, defining a function returns 0 or void.
    }
}

public record FunctionCallNode(string FunctionName, ImmutableList<INode> Arguments) : INode
{
    public int Evaluate(IScope scope)
    {
        var function = scope.GetFunction(FunctionName);
        if (function.Parameters.Count != Arguments.Count)
            throw new Exception("Argument count mismatch");
        var functionScope = new Scope();
        for (var i = 0; i < function.Parameters.Count; i++)
            functionScope.DefineVariable(new VariableDefinitionNode(function.Parameters[i], Arguments[i]));
        return function.Body.Evaluate(functionScope);
    }
}
