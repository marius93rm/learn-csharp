using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del pattern Strategy con strategie dinamiche e costi realistici.
/// </summary>
public static class StrategyPatternSolution
{
    public static void Run()
    {
        var cart = new ShoppingCart();
        cart.AddItem(new CartItem("Libro", 19.90m, 0.5m));
        cart.AddItem(new CartItem("Tastiera", 49.90m, 0.9m));

        cart.SelectStrategyByDestination("IT");
        cart.CalculateTotal();

        cart.SelectStrategyByDestination("US");
        cart.CalculateTotal();

        cart.ShippingStrategy = new FreePickupStrategy();
        cart.CalculateTotal();
    }
}

public sealed class ShoppingCart
{
    private readonly List<CartItem> _items = new();

    public IShippingStrategy ShippingStrategy { get; set; } = new StandardShippingStrategy();

    public void AddItem(CartItem item) => _items.Add(item);

    public void SelectStrategyByDestination(string countryCode)
    {
        ShippingStrategy = countryCode switch
        {
            "IT" => new StandardShippingStrategy(),
            "US" => new ExpressShippingStrategy(),
            _ => _items.Sum(i => i.Weight) > 5 ? new HeavyItemsShippingStrategy() : new StandardShippingStrategy()
        };
    }

    public decimal CalculateTotal()
    {
        var subtotal = _items.Sum(item => item.Price);
        var shippingCost = ShippingStrategy.CalculateShipping(_items);
        var total = subtotal + shippingCost;

        if (ShippingStrategy is ExpressShippingStrategy)
        {
            total += 5m; // sovrapprezzo gestione doganale
        }
        else if (ShippingStrategy is FreePickupStrategy)
        {
            total -= 2m; // incentivo al ritiro in negozio
        }

        Console.WriteLine($"Totale con {ShippingStrategy.Name}: {total:C}");
        return total;
    }
}

public sealed record CartItem(string Name, decimal Price, decimal Weight);

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
        var subtotal = items.Sum(item => item.Price);
        return 4m + subtotal * 0.05m;
    }
}

public sealed class ExpressShippingStrategy : IShippingStrategy
{
    public string Name => "Spedizione Express";

    public decimal CalculateShipping(IReadOnlyCollection<CartItem> items)
    {
        var weight = items.Sum(item => item.Weight);
        return 10m + weight * 2m;
    }
}

public sealed class HeavyItemsShippingStrategy : IShippingStrategy
{
    public string Name => "Spedizione Collettiva Pesante";

    public decimal CalculateShipping(IReadOnlyCollection<CartItem> items)
    {
        var weight = items.Sum(item => item.Weight);
        return 8m + weight * 1.5m;
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
