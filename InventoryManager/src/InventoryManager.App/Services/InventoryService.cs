using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManager.App.Models;

namespace InventoryManager.App.Services;

/// <summary>
/// Gestisce l'inventario in memoria e offre metodi LINQ per filtrare e aggregare i prodotti.
/// </summary>
public class InventoryService
{
    private readonly List<Product> _products;

    public InventoryService()
    {
        _products = SeedProducts();
    }

    /// <summary>
    /// Restituisce i prodotti applicando i filtri specificati.
    /// La query è costruita step-by-step per mostrare chiaramente ogni fase di filtraggio.
    /// Alcuni filtri sono già implementati, altri restano come TODO per lo studente.
    /// </summary>
    public IEnumerable<Product> GetProducts(ProductFilterOptions filter)
    {
        IEnumerable<Product> query = _products;

        if (filter.OnlyActive)
        {
            query = query.Where(p => p.IsActive);
        }

        // TODO Milestone 2: se SearchText non è nullo o vuoto,
        //  filtrare i prodotti il cui Name contiene il testo (case insensitive).
        // Suggerimento: normalizzare entrambi i testi con ToLowerInvariant().

        // TODO Milestone 2: se Category ha un valore,
        //  applicare un filtro Where per mantenere solo i prodotti della categoria selezionata.

        // TODO Milestone 3: utilizzare MinPrice e MaxPrice per restringere l'intervallo di prezzo.
        // TODO Milestone 3: utilizzare MinStock e MaxStock per filtrare per quantità a magazzino.

        query = ApplyOrdering(query, filter);

        // TODO Milestone 3: aggiungere la paginazione usando Skip e Take prima della materializzazione.
        // Suggerimento: calcolare quante righe saltare con (filter.Page - 1) * filter.PageSize.

        return query.ToList();
    }

    /// <summary>
    /// Calcola il valore totale dello stock considerando i prezzi e le quantità.
    /// </summary>
    public decimal GetTotalStockValue()
    {
        // TODO Milestone 4: calcolare il valore totale dello stock considerando solo i prodotti attivi.
        // Suggerimento: usare Where per i prodotti attivi e Sum su Price * StockQuantity.
        return 0m;
    }

    /// <summary>
    /// Restituisce statistiche per categoria (conteggio e prezzo medio).
    /// </summary>
    public IEnumerable<(Category Category, int Count, decimal AveragePrice)> GetCategoryStats()
    {
        // TODO Milestone 4: raggruppare i prodotti per categoria con GroupBy e proiettare Count e Average.
        // Ricorda di considerare eventualmente solo i prodotti attivi per statistiche più realistiche.
        return Array.Empty<(Category, int, decimal)>();
    }

    /// <summary>
    /// Restituisce il prodotto più costoso presente in inventario.
    /// Questo metodo è già implementato come esempio di uso diretto di OrderByDescending e FirstOrDefault.
    /// </summary>
    public Product? GetMostExpensiveProduct()
    {
        return _products.OrderByDescending(p => p.Price).FirstOrDefault();
    }

    private static IEnumerable<Product> ApplyOrdering(IEnumerable<Product> source, ProductFilterOptions filter)
    {
        // Ordinamento di default: per Id crescente, utile come fallback deterministico.
        IOrderedEnumerable<Product> ordered = source.OrderBy(p => p.Id);

        if (string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            return ordered;
        }

        // TODO Milestone 3: utilizzare OrderBy/OrderByDescending in base a OrderBy e OrderDescending.
        // Suggerimento: gestire i casi "Name", "Price", "CreatedAt" e usare ThenBy per stabilire un ordine secondario per Id.

        return ordered;
    }

    private static List<Product> SeedProducts()
    {
        // Milestone 1: questa lista rappresenta un punto di partenza.
        // TODO Milestone 1: aggiungere nuovi prodotti o nuove categorie per rendere i test più realistici.
        return new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Ultrabook 13\"", Category = Category.Elettronica, Price = 1299.99m, StockQuantity = 12, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-90) },
            new Product { Id = 2, Name = "Smartphone Pro", Category = Category.Elettronica, Price = 999.50m, StockQuantity = 25, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-45) },
            new Product { Id = 3, Name = "Cuffie Wireless", Category = Category.Elettronica, Price = 199.90m, StockQuantity = 40, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new Product { Id = 4, Name = "Monitor 27'' 4K", Category = Category.Elettronica, Price = 449.00m, StockQuantity = 8, IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-120) },
            new Product { Id = 5, Name = "Router Wi-Fi 6", Category = Category.Elettronica, Price = 149.99m, StockQuantity = 18, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-75) },
            new Product { Id = 6, Name = "Pasta Artigianale", Category = Category.Alimentari, Price = 3.90m, StockQuantity = 120, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Product { Id = 7, Name = "Olio Extravergine", Category = Category.Alimentari, Price = 12.50m, StockQuantity = 60, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new Product { Id = 8, Name = "Cioccolato Fondente 80%", Category = Category.Alimentari, Price = 2.99m, StockQuantity = 200, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new Product { Id = 9, Name = "T-shirt Cotone Premium", Category = Category.Abbigliamento, Price = 25.00m, StockQuantity = 75, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new Product { Id = 10, Name = "Jeans Slim Fit", Category = Category.Abbigliamento, Price = 59.90m, StockQuantity = 50, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-35) },
            new Product { Id = 11, Name = "Giacca Impermeabile", Category = Category.Abbigliamento, Price = 129.00m, StockQuantity = 15, IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-200) },
            new Product { Id = 12, Name = "Sneakers Running", Category = Category.Sport, Price = 89.99m, StockQuantity = 65, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-18) },
            new Product { Id = 13, Name = "Pallone da Calcio", Category = Category.Sport, Price = 24.99m, StockQuantity = 110, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-60) },
            new Product { Id = 14, Name = "Bicicletta da Città", Category = Category.Sport, Price = 349.00m, StockQuantity = 5, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-150) },
            new Product { Id = 15, Name = "Set Manubri Regolabili", Category = Category.Sport, Price = 199.00m, StockQuantity = 20, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-90) },
            new Product { Id = 16, Name = "Lampada da Tavolo LED", Category = Category.Casa, Price = 39.99m, StockQuantity = 80, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-25) },
            new Product { Id = 17, Name = "Robot Aspirapolvere", Category = Category.Casa, Price = 299.99m, StockQuantity = 14, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-65) },
            new Product { Id = 18, Name = "Set Pentole Acciaio", Category = Category.Casa, Price = 149.00m, StockQuantity = 22, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-40) },
            new Product { Id = 19, Name = "Cuscino Memory", Category = Category.Casa, Price = 49.90m, StockQuantity = 90, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-12) },
            new Product { Id = 20, Name = "Diffusore di Aromi", Category = Category.Casa, Price = 29.99m, StockQuantity = 55, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-8) },
            new Product { Id = 21, Name = "Siero Viso Vitamina C", Category = Category.Bellezza, Price = 34.90m, StockQuantity = 70, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-22) },
            new Product { Id = 22, Name = "Crema Idratante Giorno", Category = Category.Bellezza, Price = 24.50m, StockQuantity = 95, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-28) },
            new Product { Id = 23, Name = "Shampoo Nutriente", Category = Category.Bellezza, Price = 9.99m, StockQuantity = 140, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new Product { Id = 24, Name = "Balsamo Nutriente", Category = Category.Bellezza, Price = 10.99m, StockQuantity = 130, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new Product { Id = 25, Name = "Barretta Proteica", Category = Category.Alimentari, Price = 1.99m, StockQuantity = 300, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new Product { Id = 26, Name = "Pantaloni Tecnici Trekking", Category = Category.Abbigliamento, Price = 89.00m, StockQuantity = 32, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-55) },
            new Product { Id = 27, Name = "Frullatore Professionale", Category = Category.Casa, Price = 189.00m, StockQuantity = 10, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-95) },
            new Product { Id = 28, Name = "Set Asciugamani Lusso", Category = Category.Casa, Price = 59.90m, StockQuantity = 45, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-32) },
            new Product { Id = 29, Name = "Action Cam 4K", Category = Category.Elettronica, Price = 249.99m, StockQuantity = 16, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-70) },
            new Product { Id = 30, Name = "Tapis Roulant Compatto", Category = Category.Sport, Price = 599.00m, StockQuantity = 7, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-110) }
        };
    }
}
