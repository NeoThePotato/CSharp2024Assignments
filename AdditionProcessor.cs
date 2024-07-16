public class AdditionProcessor : MatrixProcessor
{
	public int Value { get; set; } = 0;

	protected override void Process(int[,] val)
	{
		for (int i = 0; i < val.GetLength(0); i++)
		{
			for (int j = 0; j < val.GetLength(1); j++)
			{
				val[i, j] += Value;
			}
		}
	}
}
