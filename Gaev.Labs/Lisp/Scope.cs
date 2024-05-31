using System;
using System.Collections.Generic;

namespace Gaev.Labs.Lisp;

public interface IScope
{
    void DefineVariable(VariableDefinitionNode variable);
    VariableDefinitionNode GetVariable(string name);
    void DefineFunction(FunctionDefinitionNode func);
    FunctionDefinitionNode GetFunction(string name);
}

public class Scope : IScope
{
    private readonly Dictionary<string, VariableDefinitionNode> _variables = new();
    private readonly Dictionary<string, FunctionDefinitionNode> _functions = new();

    public void DefineVariable(VariableDefinitionNode variable)
    {
        _variables[variable.Name] = variable;
    }

    public VariableDefinitionNode GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out var variable))
            return variable;

        throw new Exception($"Variable {name} is not defined");
    }

    public void DefineFunction(FunctionDefinitionNode func)
    {
        _functions[func.Name] = func;
    }

    public FunctionDefinitionNode GetFunction(string name)
    {
        if (_functions.TryGetValue(name, out var function))
            return function;

        throw new Exception($"Function {name} is not defined");
    }
}
