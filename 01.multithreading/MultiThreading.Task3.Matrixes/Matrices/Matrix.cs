using System;
using System.Text;

namespace MultiThreading.Task3.MatrixMultiplier.Matrices
{
    /// <summary>
    /// The Matrix class. It shouldn't be changed during the task implementation.
    /// </summary>
    public sealed class Matrix : IMatrix
    {
        private const byte RandomMax = 100;
        private const byte MaxPrintElements = 5;
        private readonly long[,] matrix;

        public long RowCount { get; }

        public long ColCount { get; }

        public Matrix(long rows, long cols, bool randomInit = false)
        {
            if (rows < 1 || cols < 1)
            {
                throw new ArgumentException($"The matrix should have at least 1 row and 1 column");
            }

            this.RowCount = rows;
            this.ColCount = cols;

            this.matrix = new long[this.RowCount, this.ColCount];
            Initialize();

            void Initialize()
            {
                if (randomInit)
                {
                    var r = new Random();
                    for (var i = 0; i < this.RowCount; i++)
                    {
                        for (var j = 0; j < this.ColCount; j++)
                        {
                            this.matrix[i, j] = r.Next(RandomMax);
                        }
                    }
                }
            }
        }

        public void SetElement(long row, long col, long value)
        {
            this.matrix[row, col] = value;
        }

        public long GetElement(long row, long col)
        {
            return this.matrix[row, col];
        }

        public void Write()
        {
            for (long r = 0; r < this.RowCount; r++)
            {
                if (r >= MaxPrintElements)
                {
                    Console.WriteLine("...");
                    return;
                }

                var sb = new StringBuilder();
                for (long c = 0; c < this.ColCount; c++)
                {
                    if (c >= MaxPrintElements)
                    {
                        sb.Append("...".PadRight(7));
                        break;
                    }

                    sb.Append($"{this.GetElement(r, c)}".PadRight(7));
                }

                Console.WriteLine(sb.ToString());
            }
        }
    }
}