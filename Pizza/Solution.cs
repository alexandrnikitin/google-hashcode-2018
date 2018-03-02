using System;
using System.Collections.Generic;
using System.Text;

namespace Pizza
{
    public class Solution
    {
        public List<List<int>> Vehicles { get; set; }

        public Solution(int fleet)
        {
            Vehicles = new List<List<int>>(fleet);
            for (var i = 0; i < fleet; i++)
            {
                Vehicles.Add(new List<int>());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var v in Vehicles)
            {
                sb.AppendLine($"{v.Count} {String.Join(" ", v)}");
            }

            return sb.ToString();
        }
    }
}