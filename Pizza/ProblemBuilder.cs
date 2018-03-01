using System.Linq;

namespace Pizza
{
    public class ProblemBuilder
    {
        public static Problem Build(string[] lines)
        {
            var pars = lines[0].Split(' ');
            var R = int.Parse(pars[0]);
            var C = int.Parse(pars[1]);
            var L = int.Parse(pars[2]);
            var H = int.Parse(pars[3]);

            var pizza = PizzaBuilder.BuildPizza(lines.Skip(1).ToArray(), R, C);

            return new Problem(L, H, pizza);
        }
    }
}