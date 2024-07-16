public static class Program
{
	public static void Main()
	{
		var matrix = GetCustomMatrix();
		Console.WriteLine("Created matrix:");
		PrintMatrix(matrix);
		GetCustomMatrixProcessor().Handle(matrix);
		Console.WriteLine("Processed matrix:");
		PrintMatrix(matrix);
	}

	private static void PrintMatrix(int[,] matrix)
	{
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				Console.Write(matrix[i, j]);
				Console.Write(' ');
			}
			Console.Write('\n');
		}
	}

	private static int[,] GetCustomMatrix()
	{
		var matrix = new int[3, 3];
		var mp = new AdditionProcessor();
		mp.Value = 1;
		mp.Handle(matrix);
		return matrix;
	}

	private static MatrixProcessor GetCustomMatrixProcessor()
	{
		// *2
		var mp1 = new MultiplicationProcessor();
		mp1.Value = 2;

		// +3
		var mp2 = new AdditionProcessor();
		mp2.Value = 3;

		// -1
		var mp3 = new AdditionProcessor();
		mp3.Value = -1;

		mp1.SetNext(mp2);
		mp2.SetNext(mp3);

		return mp1;
	}
}
