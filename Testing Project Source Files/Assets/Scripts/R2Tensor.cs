using UnityEngine;
[System.Serializable]
public class R2Tensor
{
    public int row;
    public int col;
    public float[,] tensor;

    #region Constructor
    public R2Tensor(int Row, int Col)
    {
        row = Row;
        col = Col;
        tensor = new float[Row, Col];
        for (int y = 0; y < Row; y++)
        {
            for (int x = 0; x < Col; x++)
            {
                tensor[y, x] = Random.Range(-1f, 1f);
            }
        }
    }
    public R2Tensor(int Row, int Col, params float[] Tensor)
    {
        if (Row * Col != Tensor.Length) { throw new MismatchedDimensionException("Number of parameters don't match the dimension specified"); }
        row = Row;
        col = Col;
        tensor = new float[Row, Col];
        for (int y = 0; y < Row; y++)
        {
            for (int x = 0; x < Col; x++)
            {
                tensor[y, x] = Tensor[x + y * Col];
            }
        }
    }
    public R2Tensor(float[,] Tensor)
    {
        row = Tensor.GetLength(0);
        col = Tensor.GetLength(1);
        tensor = (float[,])Tensor.Clone();
    }
    #endregion
    #region Vector Operators
    public float[] GetRow(int Row)
    {
        float[] tensor_out = new float[col];
        for (int x = 0; x < col; x++)
        {
            tensor_out[x] = tensor[Row, x];
        }
        return tensor_out;
    }
    public float[] GetCol(int Col)
    {
        float[] tensor_out = new float[row];
        for (int y = 0; y < row; y++)
        {
            tensor_out[y] = tensor[y, Col];
        }
        return tensor_out;
    }
    public static float Dot(float[] a, float[] b)
    {
        if (a.Length != b.Length) { throw new MismatchedDimensionException("The length must be the same in order to take dot product"); }
        float float_out = 0;
        for (int i = 0; i < a.Length; i++)
        {
            float_out += a[i] * b[i];
        }
        return float_out;
    }
    public static float[] ToVector(R2Tensor Tensor)
    {
        if (Tensor.row != 1 & Tensor.col != 1) { throw new MismatchedDimensionException("The matrix must be a row or column matrix"); }
        if (Tensor.row == 1)
        {
            float[] vector_out = new float[Tensor.col];
            for (int i = 0; i < Tensor.col; i++)
            {
                vector_out[i] = Tensor.tensor[0, i];
            }
            return vector_out;
        }
        if (Tensor.col == 1)
        {
            float[] vector_out = new float[Tensor.row];
            for (int i = 0; i < Tensor.row; i++)
            {
                vector_out[i] = Tensor.tensor[i, 0];
            }
            return vector_out;
        }
        else { return null; }
    }
    public float[] Rowify()
    {
        float[] array_out = new float[row * col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                array_out[col * i + j] = tensor[i, j];
            }
        }
        return array_out;

    }
    #endregion
    #region Matrix Operators
    public static R2Tensor MatMul(R2Tensor A, R2Tensor B)
    {
        if (A.col != B.row) { throw new MismatchedDimensionException("Number of column of the first matrix must match the number of row of the second matrix"); }
        float[,] tensor_out = new float[A.row, B.col];
        for (int y = 0; y < A.row; y++)
        {
            for (int x = 0; x < B.col; x++)
            {
                tensor_out[y, x] = Dot(A.GetRow(y), B.GetCol(x));
            }
        }
        return new R2Tensor(tensor_out);
    }
    public static R2Tensor Add(R2Tensor A, R2Tensor B)
    {
        if (A.row != B.row | A.col != B.col) { throw new MismatchedDimensionException("Both Matrix must have the same dimension in order to be added together"); }
        float[,] tensor_out = new float[A.row, A.col];
        for (int y = 0; y < A.row; y++)
        {
            for (int x = 0; x < A.col; x++)
            {
                tensor_out[y, x] = A.tensor[y, x] + B.tensor[y, x];
            }
        }
        return new R2Tensor(tensor_out);
    }
    public R2Tensor Transpose()
    {
        float[,] tensor_out = new float[col, row];
        for (int y = 0; y < col; y++)
        {
            for (int x = 0; x < row; x++)
            {
                tensor_out[y, x] = tensor[x, y];
            }
        }
        return new R2Tensor(tensor_out);
    }
    public static R2Tensor HadamardMul(R2Tensor A, R2Tensor B)
    {
        if (A.row != B.row | A.col != B.col) { throw new MismatchedDimensionException("Both Matrix must have the same dimension in order to be multiplied together"); }
        float[,] tensor_out = new float[A.row, A.col];
        for (int y = 0; y < A.row; y++)
        {
            for (int x = 0; x < A.col; x++)
            {
                tensor_out[y, x] = A.tensor[y, x] * B.tensor[y, x];
            }
        }
        return new R2Tensor(tensor_out);
    }
    public static R2Tensor ToMatrix(float[] Vector, string dir = "col")
    {
        if (dir == "row")
        {
            float[,] rowvect = new float[1, Vector.Length];
            for (int i = 0; i < Vector.Length; i++)
            {
                rowvect[0, i] = Vector[i];
            }
            return new R2Tensor(rowvect);
        }
        else
        {
            float[,] colvect = new float[Vector.Length, 1];
            for (int i = 0; i < Vector.Length; i++)
            {
                colvect[i, 0] = Vector[i];
            }
            return new R2Tensor(colvect);
        }
    }
    public R2Tensor Scale(float ratio)
    {
        float[,] tensor_out = new float[row, col];
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                tensor_out[y, x] = tensor[y, x] * ratio;
            }
        }
        return new R2Tensor(tensor_out);
    }
    public void PrintMatrix()
    {
        for (int y = 0; y < row; y++)
        {
            string printrow = "| ";
            for (int x = 0; x < col; x++)
            {
                printrow += tensor[y, x].ToString() + " | ";
            }
            Debug.Log(printrow);
        }
    }
    public R2Tensor Copy()
    {
        return new R2Tensor(tensor);
    }
    #endregion
}
public class MismatchedDimensionException : System.Exception
{
    public MismatchedDimensionException()
    {
    }
    public MismatchedDimensionException(string message) : base(message)
    {
    }
    public MismatchedDimensionException(string message, System.Exception inner) : base(message, inner)
    {
    }
}