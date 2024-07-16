public abstract class MatrixProcessor : IHandler<int[,]>
{
	MatrixProcessor _next;

	public void Handle(int[,] val)
	{
		Process(val);
		_next?.Handle(val);
	}

	protected abstract void Process(int[,] val);

	public void SetNext(MatrixProcessor handler) => _next = handler;

	public void SetNext(IHandler<int[,]> handler) => SetNext(handler as MatrixProcessor);
}
