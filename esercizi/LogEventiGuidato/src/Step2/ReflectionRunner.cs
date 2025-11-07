using System;
using System.Linq;
using System.Reflection;

namespace LogGuidato.Step2;

/// <summary>
/// Esegue automaticamente i metodi decorati con <see cref="LoggableAttribute"/>.
/// </summary>
public static class ReflectionRunner
{
    public static void EseguiLoggable(object istanza)
    {
        if (istanza is null)
        {
            throw new ArgumentNullException(nameof(istanza));
        }

        var tipo = istanza.GetType();

        // TODO: usare reflection per popolare l'elenco dei metodi con attributo LoggableAttribute.
        var metodiLoggabili = Array.Empty<MethodInfo>();

        foreach (var metodo in metodiLoggabili)
        {
            Console.WriteLine($"\nEsecuzione dinamica: {metodo.Name}");

            // TODO: leggere l'attributo per stampare la descrizione prima dell'invocazione.
            var attributo = metodo.GetCustomAttribute<LoggableAttribute>();
            if (attributo is not null)
            {
                Console.WriteLine($"Descrizione: {attributo.Descrizione}");
            }

            // TODO: invocare il metodo (senza parametri) sull'istanza passata.
        }
    }
}
