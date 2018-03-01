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
            var F = int.Parse(pars[2]);
            var N = int.Parse(pars[3]);
            var B = int.Parse(pars[4]);
            var T = int.Parse(pars[5]);

            return new Problem(R, C, F, N, B, T);
        }
    }
}