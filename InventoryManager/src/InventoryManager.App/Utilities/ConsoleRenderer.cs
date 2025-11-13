using System;
using System.Collections.Generic;
using System.Globalization;
using InventoryManager.App.Models;

namespace InventoryManager.App.Utilities;

/// <summary>
/// Gestisce la presentazione a console dei prodotti e delle statistiche.
/// </summary>
public class ConsoleRenderer
{
    private const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    /// Stampa una tabella semplice con le informazioni principali dei prodotti.
    /// </summary>
    public void RenderProductTable(IEnumerable<Product> products)
    {
        Console.WriteLine("\nID  | Nome                           | Categoria     | Prezzo    | Stock | Attivo | Creato il");
        Console.WriteLine(new string('-', 90));

        foreach (var product in products)
        {
            Console.WriteLine(
                $"{product.Id,3} | {product.Name,-30} | {product.Category,-12} | {product.Price,8:C} | {product.StockQuantity,5} | {(product.IsActive ? "Si" : "No"),6} | {product.CreatedAt.ToString(DateFormat, CultureInfo.InvariantCulture)}");
        }

        Console.WriteLine();
        // TODO Milestone 5: migliorare la resa grafica (colori, colonne adattive, ecc.).
    }

    /// <summary>
    /// Stampa un riepilogo delle statistiche principali.
    /// </summary>
    public void RenderStatistics(decimal totalStockValue, IEnumerable<(Category Category, int Count, decimal AveragePrice)> categoryStats)
    {
        Console.WriteLine("\n=== Statistiche Inventario ===");
        Console.WriteLine($"Valore totale stock (placeholder finché non completi la Milestone 4): {totalStockValue:C}");
        Console.WriteLine();

        Console.WriteLine("Categoria         | # Prodotti | Prezzo medio");
        Console.WriteLine(new string('-', 50));

        foreach (var stat in categoryStats)
        {
            Console.WriteLine($"{stat.Category,-16} | {stat.Count,10} | {stat.AveragePrice,12:C}");
        }

        Console.WriteLine();
        // TODO Milestone 5: aggiungere altre metriche (es. stock medio, prodotto più venduto, ecc.).
    }

    /// <summary>
    /// Visualizza il menu principale.
    /// </summary>
    public void RenderMenu()
    {
        Console.WriteLine("==============================");
        Console.WriteLine(" Inventory Manager - Demo");
        Console.WriteLine("==============================");
        Console.WriteLine("1) Mostra tutti i prodotti (pagina 1)");
        Console.WriteLine("2) Filtra prodotti (ricerca semplice)");
        Console.WriteLine("3) Mostra statistiche");
        Console.WriteLine("0) Esci");
        Console.Write("Scelta: ");
    }

    /// <summary>
    /// Fornisce un messaggio di separazione leggibile tra le azioni.
    /// </summary>
    public void RenderSeparator()
    {
        Console.WriteLine(new string('=', 40));
    }
}
