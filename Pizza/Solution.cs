using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Pizza
{
    public struct Slice
    {
        public bool Equals(Slice other)
        {
            return Row1 == other.Row1 && Column1 == other.Column1 && Row2 == other.Row2 && Column2 == other.Column2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Slice && Equals((Slice) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Row1;
                hashCode = (hashCode * 397) ^ Column1;
                hashCode = (hashCode * 397) ^ Row2;
                hashCode = (hashCode * 397) ^ Column2;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{{{Row1}:{Column1} - {Row2}:{Column2}}}";
        }

        public Slice(int row1, int column1, int row2, int column2)
        {
            Row1 = row1;
            Column1 = column1;
            Row2 = row2;
            Column2 = column2;
        }

        public int Row1 { get; set; }
        public int Column1 { get; set; }
        public int Row2 { get; set; }
        public int Column2 { get; set; }

        public int Area => (Row2 - Row1 + 1) * (Column2 - Column1 + 1);

        public bool ValidIndexes(int rows, int columns)
        {
            if (Row1 >= 0 && Row2 >= 0 && Row1 < rows && Row2 < rows)
            {
                if (Column1 >= 0 && Column2 >= 0 && Column1 < columns && Column2 < columns)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasHoles(Pizza pizza)
        {
            for (var i = Row1; i <= Row2; i++)
            {
                for (var j = Column1; j <= Column2; j++)
                {
                    if (pizza.Data[i, j] == '0')
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasMinIngredients(Pizza pizza, int minIngr)
        {
            var ts = 0;
            var ms = 0;
            for (var i = Row1; i <= Row2; i++)
            {
                for (var j = Column1; j <= Column2; j++)
                {
                    if (pizza.Data[i, j] == 'T') ts++;
                    if (pizza.Data[i, j] == 'M') ms++;
                    if (ts >= minIngr && ms >= minIngr) return true;
                }
            }

            return false;
        }
    }

    public class Solution
    {
        public Solution(int numberSlices, List<Slice> slices)
        {
            NumberSlices = numberSlices;
            Slices = slices;
        }

        public int NumberSlices { get; set; }
        public List<Slice> Slices { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(NumberSlices.ToString(CultureInfo.InvariantCulture));
            foreach (var slice in Slices)
            {
                sb.AppendLine($"{slice.Row1} {slice.Column1} {slice.Row2} {slice.Column2}");
            }
            return sb.ToString();
        }
    }
}