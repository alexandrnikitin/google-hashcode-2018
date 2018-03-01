using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pizza
{
    public class Pizza : ICloneable
    {
        public int Rows { get; }
        public int Columns { get; }
        public char[,] Data { get; }

        public Pizza(int rows, int columns) :
            this(rows, columns, new char[rows, columns])
        {
        }

        public Pizza(int rows, int columns, char[,] array)
        {
            Rows = rows;
            Columns = columns;
            Data = array;
        }

        public void PutMashroom(int row, int column)
        {
            Data[row, column] = 'M';
        }

        public void PutTomato(int row, int column)
        {
            Data[row, column] = 'T';
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Rows; i += 1)
            {
                for (var j = 0; j < Columns; j++)
                {
                    sb.Append(Data[i, j]);
                    sb.Append('\t');
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public object Clone()
        {
            return new Pizza(Rows, Columns, (char[,]) Data.Clone());
        }

        public void Slice(Slice slice)
        {
            for (var i = slice.Row1; i <= slice.Row2; i++)
            {
                for (var j = slice.Column1; j <= slice.Column2; j++)
                {
                    Data[i, j] = '0';
                }
            }
        }

        public static List<Slice> FindSlicesForPosition(Pizza pizza, int row, int column, int minIngr, int maxCells)
        {
            var set = new HashSet<Slice>();
            Expand(pizza, new Slice(row, column, row, column), minIngr, maxCells, set, Direction.Right);
            return set.ToList();
        }

        public enum Direction
        {
            Right,
            Down,
        }

        public static void Expand(Pizza pizza, Slice slice, int minIngr, int maxCells, HashSet<Slice> set, Direction direction)
        {
            if (!slice.ValidIndexes(pizza.Rows, pizza.Columns)) return;
            if (slice.Area > maxCells) return;
            if (slice.HasHoles(pizza)) return;
            if (slice.HasMinIngredients(pizza, minIngr))
            {
                if (set.Contains(slice))return;
                set.Add(slice);
            }

            if (direction == Direction.Right)
            {
                var right = new Slice(slice.Row1, slice.Column1, slice.Row2, slice.Column2 + 1);
                Expand(pizza, right, minIngr, maxCells, set, Direction.Right);
                var down = new Slice(slice.Row1, slice.Column1, slice.Row2 + 1, slice.Column2);
                Expand(pizza, down, minIngr, maxCells, set, Direction.Down);
            }
            else
            {
                var down = new Slice(slice.Row1, slice.Column1, slice.Row2 + 1, slice.Column2);
                Expand(pizza, down, minIngr, maxCells, set, Direction.Down);
            }
        }
    }
}