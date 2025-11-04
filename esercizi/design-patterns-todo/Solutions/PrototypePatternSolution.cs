using System.Collections.ObjectModel;

namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Versione risolta dell'esercizio sul pattern Prototype.
/// La copia profonda dell'elenco di abilità garantisce che clone e originale
/// possano evolvere indipendentemente.
/// </summary>
public static class PrototypePatternSolution
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

        /// <summary>
        /// La proprietà è <c>private set</c> così la clonazione può sostituire la collezione
        /// con una nuova istanza evitando la condivisione dei riferimenti.
        /// </summary>
        public ObservableCollection<string> Abilities { get; private set; }

        public Monster Clone()
        {
            var copy = (Monster)MemberwiseClone();

            // Copia profonda: creiamo una nuova ObservableCollection popolata con gli stessi valori
            // così le modifiche sul clone non impattano l'istanza originale.
            copy.Abilities = new ObservableCollection<string>(Abilities);

            return copy;
        }
    }
}
