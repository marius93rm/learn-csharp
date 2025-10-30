/*
 * Pattern: Interpreter
 * Obiettivi didattici:
 *   - Definire una grammatica semplice e interpretarla tramite espressioni composte.
 *   - Utilizzare un contesto che fornisce le variabili necessarie.
 *   - Estendere facilmente la grammatica con nuove regole.
 * Istruzioni:
 *   - Completa i TODO per arricchire il linguaggio (es. operatori logici aggiuntivi, funzioni).
 */

namespace DesignPatternsTodo2.Patterns;

public static class InterpreterPattern
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

        // TODO: prova ad interpretare una nuova espressione (es. combinazione AND/NOT) dopo aver implementato i TODO.
    }
}

public sealed class EvaluationContext
{
    private readonly Dictionary<string, object> _variables = new();

    public void SetVariable(string name, object value) => _variables[name] = value;

    public T GetVariable<T>(string name)
    {
        if (_variables.TryGetValue(name, out var value) && value is T typed)
        {
            return typed;
        }

        Console.WriteLine($"Variabile '{name}' non trovata. Restituisco default.");
        // TODO: decidi come gestire variabili mancanti (eccezione, default personalizzato, logging avanzato, ...).
        return default!;
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
        // TODO: consenti valori non booleani (es. numeri) convertendoli opportunamente.
        return RawValue is bool boolean && boolean;
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
        // TODO: gestisci i casi in cui i tipi non sono numerici o la conversione fallisce.
        return leftValue > rightValue;
    }

    private static int ResolveNumericValue(EvaluationContext context, IExpression expression) => expression switch
    {
        VariableExpression variable => context.GetVariable<int>(variable.Name),
        ConstantExpression constant => Convert.ToInt32(constant.RawValue),
        _ => 0
    };
}

// TODO: aggiungi nuove espressioni (es. NotExpression, EqualsExpression) per ampliare il linguaggio.
