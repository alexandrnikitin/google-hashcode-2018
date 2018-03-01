namespace Pizza
{
    public class Problem
    {
        public Problem(int minIngredients, int maxCells, Pizza pizza)
        {
            MinIngredients = minIngredients;
            MaxCells = maxCells;
            Pizza = pizza;
        }

        public int MinIngredients { get; set; }
        public int MaxCells { get; set; }
        public Pizza Pizza { get; set; }
    }
}