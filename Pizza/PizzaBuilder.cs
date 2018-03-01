namespace Pizza
{
    public class PizzaBuilder
    {
        public static Pizza BuildPizza(string[] lines, int rows, int columns)
        {
            var pizza = new Pizza(rows, columns);
            for (var i = 0; i < rows; i++)
            {
                var line = lines[i];
                for (var j = 0; j < columns; j++)
                {
                    var ch = line[j];
                    if (ch == 'M') pizza.PutMashroom(i, j);
                    if (ch == 'T') pizza.PutTomato(i, j);
                }
            }

            return pizza;
        }
    }
}