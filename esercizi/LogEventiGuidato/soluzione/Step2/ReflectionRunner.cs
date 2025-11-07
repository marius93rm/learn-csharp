using System;
using System.Linq;
using System.Reflection;

namespace LogGuidato.Step2;

/// <summary>
/// Esegue automaticamente i metodi decorati con <see cref="LoggableAttribute"/>.
/// La classe è totalmente generica: funziona con qualsiasi istanza passata in input.
/// </summary>
public static class ReflectionRunner
{
    /// <summary>
    /// Scansiona l'oggetto fornito e invoca tutti i metodi che espongono l'attributo.
    /// </summary>
    /// <param name="istanza">Oggetto contenente i metodi da eseguire.</param>
    public static void EseguiLoggable(object istanza)
    {
        if (istanza is null)
        {
            throw new ArgumentNullException(nameof(istanza));
        }

        var tipo = istanza.GetType();
        Console.WriteLine($"== Analisi della classe {tipo.Name} ==");

        // 1) Recuperiamo tutti i metodi di istanza (pubblici e non) e filtriamo
        //    solo quelli che hanno il nostro attributo personalizzato.
        var metodiLoggabili = tipo
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(metodo => metodo.GetCustomAttribute<LoggableAttribute>() is not null)
            .OrderBy(metodo => metodo.Name) // ordinamento puramente estetico.
            .ToArray();

        if (metodiLoggabili.Length == 0)
        {
            Console.WriteLine("Nessun metodo decorato trovato. Nulla da eseguire.");
            return;
        }

        foreach (var metodo in metodiLoggabili)
        {
            Console.WriteLine($"\n-> Esecuzione dinamica di {metodo.Name}");

            // 2) Leggiamo l'attributo collegato al metodo per stampare la descrizione.
            var attributo = metodo.GetCustomAttribute<LoggableAttribute>();
            if (!string.IsNullOrWhiteSpace(attributo?.Descrizione))
            {
                Console.WriteLine($"Descrizione: {attributo!.Descrizione}");
            }

            // 3) In questa soluzione gli esempi non richiedono parametri. Se il metodo
            //    ne avesse, qui potremmo valorizzarli dinamicamente (es. valori di default).
            var argomenti = metodo.GetParameters().Length == 0
                ? Array.Empty<object?>()
                : throw new InvalidOperationException(
                    $"Il metodo {metodo.Name} richiede parametri non gestiti in questo esempio.");

            metodo.Invoke(istanza, argomenti);
        }
    }
}
