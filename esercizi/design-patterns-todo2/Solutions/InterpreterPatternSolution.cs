using System.Collections.Generic;
using System.Globalization;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione dell'Interpreter con gestione robusta del contesto e nuovi operatori.
/// </summary>
public static class InterpreterPatternSolution
{
    public static void Run()
    {
        var context = new EvaluationContext();
        context.SetVariable("isAdmin", true);
        context.SetVariable("isPremium", false);
        context.SetVariable("age", 25);

        IExpression expression = new AndExpression(
            new VariableExpression("isAdmin"),
            new OrExpression(
                new VariableExpression("isPremium"),
                new GreaterThanExpression(new VariableExpression("age"), new ConstantExpression(21))
            )
        );

        var result = expression.Evaluate(context);
        Console.WriteLine($"Risultato interpretazione: {result}");

        IExpression advanced = new AndExpression(
            new NotExpression(new VariableExpression("isPremium")),
            new EqualsExpression(new ConstantExpression("admin"), new ConstantExpression("ADMIN"))
        );

        Console.WriteLine($"Risultato espressione avanzata: {advanced.Evaluate(context)}");
    }
}

public sealed class EvaluationContext
{
    private readonly Dictionary<string, object> _variables = new();

    public void SetVariable(string name, object value) => _variables[name] = value;

    public T GetVariable<T>(string name)
    {
        if (_variables.TryGetValue(name, out var value))
        {
            if (value is T typed)
            {
                return typed;
            }

            throw new InvalidCastException($"La variabile '{name}' non è del tipo richiesto ({typeof(T).Name}).");
        }

        throw new KeyNotFoundException($"Variabile '{name}' non trovata nel contesto.");
    }
}

public interface IExpression
{
    bool Evaluate(EvaluationContext context);
}

public sealed class ConstantExpression : IExpression
{
    public ConstantExpression(object value) => RawValue = value;

    public object RawValue { get; }

    public bool Evaluate(EvaluationContext context)
    {
        return RawValue switch
        {
            bool boolean => boolean,
            int integer => integer != 0,
            double doubleValue => Math.Abs(doubleValue) > double.Epsilon,
            string text => !string.IsNullOrWhiteSpace(text),
            _ => RawValue is not null
        };
    }
}

public sealed class VariableExpression : IExpression
{
    public VariableExpression(string name) => Name = name;

    public string Name { get; }

    public bool Evaluate(EvaluationContext context) => context.GetVariable<bool>(Name);
}

public sealed class AndExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public AndExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Evaluate(EvaluationContext context) => _left.Evaluate(context) && _right.Evaluate(context);
}

public sealed class OrExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public OrExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Evaluate(EvaluationContext context) => _left.Evaluate(context) || _right.Evaluate(context);
}

public sealed class NotExpression : IExpression
{
    private readonly IExpression _inner;

    public NotExpression(IExpression inner) => _inner = inner;

    public bool Evaluate(EvaluationContext context) => !_inner.Evaluate(context);
}

public sealed class EqualsExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public EqualsExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Evaluate(EvaluationContext context)
    {
        var leftValue = ResolveValue(context, _left);
        var rightValue = ResolveValue(context, _right);
        return string.Equals(leftValue, rightValue, StringComparison.OrdinalIgnoreCase);
    }

    private static string ResolveValue(EvaluationContext context, IExpression expression) => expression switch
    {
        VariableExpression variable => context.GetVariable<string>(variable.Name),
        ConstantExpression constant => constant.RawValue?.ToString() ?? string.Empty,
        _ => string.Empty
    };
}

public sealed class GreaterThanExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public GreaterThanExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Evaluate(EvaluationContext context)
    {
        var leftValue = ResolveNumericValue(context, _left);
        var rightValue = ResolveNumericValue(context, _right);
        return leftValue > rightValue;
    }

    private static double ResolveNumericValue(EvaluationContext context, IExpression expression) => expression switch
    {
        VariableExpression variable => Convert.ToDouble(context.GetVariable<int>(variable.Name)),
        ConstantExpression constant when constant.RawValue is IConvertible convertible => convertible.ToDouble(CultureInfo.InvariantCulture),
        _ => throw new InvalidOperationException("Espressione non numerica utilizzata in GreaterThanExpression.")
    };
}
