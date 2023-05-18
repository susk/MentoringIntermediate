/*
 * 3. Write a program, which multiplies two matrices and uses class Parallel.
 * a. Implement logic of MatricesMultiplierParallel.cs
 *    Make sure that all the tests within MultiThreading.Task3.MatrixMultiplier.Tests.csproj run successfully.
 * b. Create a test inside MultiThreading.Task3.MatrixMultiplier.Tests.csproj to check which multiplier runs faster.
 *    Find out the size which makes parallel multiplication more effective than the regular one.
 */

using System;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("3. Write a program, which multiplies two matrices and uses class Parallel. ");
            Console.WriteLine();

            const byte matrixSize = 2;
            CreateAndProcessMatrices(matrixSize);
            Console.ReadLine();
        }

        private static void CreateAndProcessMatrices(byte sizeOfMatrix)
        {
            Console.WriteLine("Multiplying...");
            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);

            IMatrix resultMatrix = new MatricesMultiplier().Multiply(firstMatrix, secondMatrix);
            IMatrix resultParallelMatrix = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);

            Console.WriteLine("firstMatrix:");
            firstMatrix.Write();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Write();
            Console.WriteLine("resultMatrix:");
            resultMatrix.Write();
            Console.WriteLine("resultParallelMatrix:");
            resultParallelMatrix.Write();
        }
    }
}
