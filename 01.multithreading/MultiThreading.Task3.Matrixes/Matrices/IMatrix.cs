namespace MultiThreading.Task3.MatrixMultiplier.Matrices
{
    public interface IMatrix
    {
        long RowCount { get; }

        long ColCount { get; }

        void SetElement(long row, long col, long value);

        long GetElement(long row, long col);

        void Write();
    }
}