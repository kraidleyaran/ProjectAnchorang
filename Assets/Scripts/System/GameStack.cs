namespace Assets.Scripts.System
{
    public class GameStack<T>
    {
        public GameStack(T item, int stack = 1)
        {
            Item = item;
            Stack = stack;
        }
        public T Item { get; private set; }
        public int Stack { get; set; }
    }
}