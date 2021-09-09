namespace CatApi.interpreting
{
    public interface IInterpreter<T>
    {
        public T Interpret(INode node);
    }
}