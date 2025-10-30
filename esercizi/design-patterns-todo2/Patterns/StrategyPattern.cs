/*
 * Pattern: Strategy
 * Obiettivi didattici:
 *   - Incapsulare algoritmi intercambiabili e selezionarli a runtime.
 *   - Favorire l'estensibilità aggiungendo nuove strategie senza modificare il client.
 *   - Separare il contesto (es. carrello e-commerce) dalla logica specifica (es. calcolo spedizione).
 * Istruzioni:
 *   - Completa i TODO per rendere le strategie dinamiche e configurabili.
 */

namespace DesignPatternsTodo2.Patterns;

public static class StrategyPattern
{
    public static void Run()
    {
        var cart = new ShoppingCart();
        cart.AddItem(new CartItem("Libro", 19.90m));
        cart.AddItem(new CartItem("Tastiera", 49.90m));

        cart.ShippingStrategy = new StandardShippingStrategy();
        Console.WriteLine(cart.CalculateTotal());

        cart.ShippingStrategy = new ExpressShippingStrategy();
        Console.WriteLine(cart.CalculateTotal());

        // TODO: consenti all'utente di scegliere la strategia in base a criteri (es. peso totale, destinazione).
    }
}

public sealed class ShoppingCart
{
    private readonly List<CartItem> _items = new();

    public IShippingStrategy ShippingStrategy { get; set; } = new StandardShippingStrategy();

    public void AddItem(CartItem item) => _items.Add(item);

    public decimal CalculateTotal()
    {
        var subtotal = _items.Sum(item => item.Price);
        var shippingCost = ShippingStrategy.CalculateShipping(_items);
        // TODO: applica sconti o sovrapprezzi in base alla strategia scelta.
        var total = subtotal + shippingCost;
        Console.WriteLine($"Totale con {ShippingStrategy.Name}: {total:C}");
        return total;
    }
}

public sealed record CartItem(string Name, decimal Price)
{
    // TODO: aggiungi campi opzionali (es. Peso, Categoria) utili per strategie avanzate.
}

public interface IShippingStrategy
{
    string Name { get; }
    decimal CalculateShipping(IReadOnlyCollection<CartItem> items);
}

public sealed class StandardShippingStrategy : IShippingStrategy
{
    public string Name => "Spedizione Standard";

    public decimal CalculateShipping(IReadOnlyCollection<CartItem> items)
    {
        // TODO: calcola la spedizione in modo più realistico (es. costo base + percentuale sul totale).
        return 5m;
    }
}

public sealed class ExpressShippingStrategy : IShippingStrategy
{
    public string Name => "Spedizione Express";

    public decimal CalculateShipping(IReadOnlyCollection<CartItem> items)
    {
        // TODO: valuta il numero di articoli o il peso per stabilire il sovrapprezzo.
        return 12m;
    }
}

public sealed class FreePickupStrategy : IShippingStrategy
{
    public string Name => "Ritiro in Negozio";

    public decimal CalculateShipping(IReadOnlyCollection<CartItem> items)
    {
        Console.WriteLine("Ritiro gratuito disponibile nei punti vendita.");
        return 0m;
    }
}
