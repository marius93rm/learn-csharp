using System;

namespace InventoryManager.App.Models;

/// <summary>
/// Incapsula tutti i criteri di filtraggio disponibili per interrogare l'inventario.
/// </summary>
public class ProductFilterOptions
{
    public string? SearchText { get; set; }

    public Category? Category { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int? MinStock { get; set; }

    public int? MaxStock { get; set; }

    public bool OnlyActive { get; set; } = true;

    public string? OrderBy { get; set; }

    public bool OrderDescending { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Restituisce una rappresentazione leggibile dei criteri impostati utile per il debug.
    /// </summary>
    public override string ToString()
    {
        return $"SearchText={SearchText ?? "<null>"}, Category={Category?.ToString() ?? "<null>"}, Prezzo={MinPrice}-{MaxPrice}, Stock={MinStock}-{MaxStock}, OnlyActive={OnlyActive}, OrderBy={OrderBy}, Desc={OrderDescending}, Page={Page}, PageSize={PageSize}";
    }
}
