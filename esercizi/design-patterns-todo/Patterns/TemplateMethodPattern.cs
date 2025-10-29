namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Template Method.
/// Completa i TODO per personalizzare i passaggi dell'algoritmo.
/// </summary>
public static class TemplateMethodPattern
{
    public static void Run()
    {
        CookingProcess recipe = new PastaCooking();
        recipe.Execute();

        Console.WriteLine("\nPersonalizza i passaggi completando i TODO.\n");
    }

    private abstract class CookingProcess
    {
        public void Execute()
        {
            BoilWater();
            AddMainIngredient();
            Cook();
            Serve();
        }

        protected virtual void BoilWater()
        {
            Console.WriteLine("Metto a bollire l'acqua.");
        }

        protected abstract void AddMainIngredient();
        protected abstract void Cook();

        protected virtual void Serve()
        {
            Console.WriteLine("Servo il piatto.");
            // TODO: consenti alle sottoclassi di aggiungere condimenti personalizzati.
        }
    }

    private sealed class PastaCooking : CookingProcess
    {
        protected override void AddMainIngredient()
        {
            Console.WriteLine("Aggiungo la pasta all'acqua salata.");
        }

        protected override void Cook()
        {
            Console.WriteLine("Cuocio per 10 minuti.");
        }

        // TODO: sovrascrivi Serve() per aggiungere condimento e chiamare eventualmente la logica base.
    }
}
