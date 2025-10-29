using System.Collections.ObjectModel;

namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Prototype.
/// Completa la copia profonda dei campi mutabili per evitare effetti collaterali.
/// </summary>
public static class PrototypePattern
{
    public static void Run()
    {
        var boss = new Monster("Drago Ancestrale", 250);
        boss.Abilities.Add("Fiato di fuoco");
        boss.Abilities.Add("Rigenerazione");

        var miniBoss = boss.Clone();
        miniBoss.Name = "Drago Giovane";
        miniBoss.HitPoints = 120;
        miniBoss.Abilities.Add("Sbuffo di fumo");

        Console.WriteLine($"Originale: {boss.Name} ({boss.HitPoints} HP) Abilità: {string.Join(", ", boss.Abilities)}");
        Console.WriteLine($"Clone: {miniBoss.Name} ({miniBoss.HitPoints} HP) Abilità: {string.Join(", ", miniBoss.Abilities)}");
        Console.WriteLine("Nota come la lista delle abilità è condivisa: correggi completando il TODO nella clonazione.\n");
    }

    private interface IPrototype<out T>
    {
        T Clone();
    }

    private sealed class Monster : IPrototype<Monster>
    {
        public Monster(string name, int hitPoints)
        {
            Name = name;
            HitPoints = hitPoints;
            Abilities = new ObservableCollection<string>();
        }

        public string Name { get; set; }
        public int HitPoints { get; set; }
        public ObservableCollection<string> Abilities { get; }

        public Monster Clone()
        {
            var copy = (Monster)MemberwiseClone();

            // TODO: crea una nuova ObservableCollection con gli stessi valori per evitare la condivisione di riferimenti.

            return copy;
        }
    }
}
