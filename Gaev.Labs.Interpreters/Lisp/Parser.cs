using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gaev.Labs.Interpreters.Lisp;

public class Parser
{
    private readonly List<string> _tokens;
    private int _position;
    private string Token => _tokens[_position];

    public Parser(string input)
    {
        _tokens = Regex.Matches(input, @"[\(\)]|[^\s\(\)]+")
            .Select(m => m.Value)
            .ToList();
        _position = 0;
    }

    public INode ParseExpression()
    {
        if (Token == "(")
        {
            MoveToNextToken();
            if (Token == "define")
            {
                MoveToNextToken();
                return ParseDefineExpression();
            }

            if (Token == "if")
            {
                MoveToNextToken();
                return ParseIfExpression();
            }

            if (NumericOperatorNode.AllowedOperators.Contains(Token))
                return ParseNumericOperatorExpression();

            if (BooleanOperatorNode.AllowedOperators.Contains(Token))
                return ParseBooleanOperatorExpression();

            return ParseFunctionCallExpression();
        }

        if (int.TryParse(Token, out var number))
        {
            MoveToNextToken();
            return new LiteralNode(number);
        }
        else
        {
            MoveToNextToken();
            return new VariableNode(Token);
        }
    }

    private void MoveToNextToken()
    {
        _position++;
    }

    private INode ParseDefineExpression()
    {
        var name = Token;
        MoveToNextToken();
        if (name == "(")
        {
            name = Token;
            MoveToNextToken();
            var parameters = new List<string>();
            while (Token != ")")
            {
                parameters.Add(Token);
                MoveToNextToken();
            }

            MoveToNextToken(); // Skip ')'
            var body = ParseExpression();
            MoveToNextToken(); // Skip final ')'
            return new FunctionDefinitionNode(name, parameters.ToImmutableList(), body);
        }

        var expression = ParseExpression();
        MoveToNextToken(); // Skip final ')'
        return new VariableDefinitionNode(name, expression);
    }

    private INode ParseFunctionCallExpression()
    {
        var name = Token;
        MoveToNextToken();
        var arguments = new List<INode>();
        while (Token != ")")
        {
            arguments.Add(ParseExpression());
        }

        MoveToNextToken(); // Skip final ')'
        return new FunctionCallNode(name, arguments.ToImmutableList());
    }

    private INode ParseIfExpression()
    {
        var condition = ParseExpression();
        var thenBranch = ParseExpression();
        var elseBranch = ParseExpression();
        MoveToNextToken(); // Skip final ')'
        return new IfNode(condition, thenBranch, elseBranch);
    }

    private INode ParseNumericOperatorExpression()
    {
        var @operator = Token;
        MoveToNextToken();
        var operands = new List<INode>();
        while (Token != ")")
        {
            operands.Add(ParseExpression());
        }

        MoveToNextToken();
        return new NumericOperatorNode(@operator, operands.ToImmutableList());
    }

    private INode ParseBooleanOperatorExpression()
    {
        var @operator = Token;
        MoveToNextToken();
        var leftOperand = ParseExpression();
        var rightOperand = ParseExpression();
        MoveToNextToken();
        return new BooleanOperatorNode(@operator, leftOperand, rightOperand);
    }
}
