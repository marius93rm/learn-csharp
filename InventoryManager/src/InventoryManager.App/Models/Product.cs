using System;

namespace InventoryManager.App.Models;

/// <summary>
/// Rappresenta un prodotto del catalogo inventariale con le principali informazioni di dominio.
/// </summary>
public class Product
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public Category Category { get; init; }

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }
}
