public interface IHandler<T>
{
	void SetNext(IHandler<T> handler);

	void Handle(T val);
}
