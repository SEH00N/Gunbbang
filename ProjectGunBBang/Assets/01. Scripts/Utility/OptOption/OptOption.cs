namespace GB.Utility
{
    [System.Serializable]
    public class OptOption<T>
    {
        public T this[bool option] => GetOption(option);

        public T PositiveOption;
        public T NegativeOption;

        public T GetOption(bool decision) => decision ? PositiveOption : NegativeOption;
    }
}