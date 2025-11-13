using System;
using InventoryManager.App.Models;
using InventoryManager.App.Services;
using InventoryManager.App.Utilities;

// Programma console minimale che orchestra InventoryService e ConsoleRenderer.
// Milestone 5 guiderà lo studente ad arricchire menu, input e presentazione.
var service = new InventoryService();
var renderer = new ConsoleRenderer();

var running = true;
while (running)
{
    renderer.RenderMenu();
    var choice = Console.ReadLine() ?? string.Empty;
    renderer.RenderSeparator();

    switch (choice)
    {
        case "1":
            ShowProducts(new ProductFilterOptions());
            break;
        case "2":
            HandleFilterFlow();
            break;
        case "3":
            ShowStatistics();
            break;
        case "0":
            running = false;
            Console.WriteLine("Uscita dal programma. A presto!");
            break;
        default:
            Console.WriteLine("Scelta non valida. Inserire 0, 1, 2 o 3.");
            break;
    }

    if (running)
    {
        Console.WriteLine("Premi INVIO per tornare al menu principale...");
        Console.ReadLine();
    }
}

void ShowProducts(ProductFilterOptions filter)
{
    Console.WriteLine($"
Filtri attuali: {filter}");
    var products = service.GetProducts(filter);
    renderer.RenderProductTable(products);
}

void HandleFilterFlow()
{
    var filter = new ProductFilterOptions();

    Console.Write("Testo da cercare (ENTER per saltare): ");
    var search = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(search))
    {
        filter.SearchText = search;
    }

    // TODO Milestone 5: leggere ulteriori parametri (categoria, range di prezzo, stock, ordinamento, paginazione)
    //  e popolare le corrispondenti proprietà di ProductFilterOptions.

    var products = service.GetProducts(filter);
    renderer.RenderProductTable(products);
}

void ShowStatistics()
{
    var total = service.GetTotalStockValue();
    var stats = service.GetCategoryStats();

    renderer.RenderStatistics(total, stats);

    var mostExpensive = service.GetMostExpensiveProduct();
    if (mostExpensive is not null)
    {
        Console.WriteLine($"Prodotto più costoso (già calcolato): {mostExpensive.Name} - {mostExpensive.Price:C}");
    }

    // TODO Milestone 5: mostrare altre statistiche, ad esempio il prodotto meno costoso o i prodotti esauriti.
}
