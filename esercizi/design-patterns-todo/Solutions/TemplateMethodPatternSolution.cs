namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del template method con condimenti personalizzabili.
/// </summary>
public static class TemplateMethodPatternSolution
{
    public static void Run()
    {
        CookingProcess recipe = new PastaCooking();
        recipe.Execute();
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
        }

        /// <summary>
        /// Hook opzionale per permettere alle sottoclassi di aggiungere condimenti.
        /// </summary>
        protected virtual void AddCondiments()
        {
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

        protected override void Serve()
        {
            base.Serve();
            AddCondiments();
            Console.WriteLine("Piatto servito con formaggio e pepe.");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine("Aggiungo parmigiano e una spolverata di pepe nero.");
        }
    }
}
