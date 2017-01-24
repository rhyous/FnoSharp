namespace FnoSharp.Builder
{
    public class BuilderBase<T> where T : class, new()
    {
        public T Object
        {
            get { return _Object ?? (_Object = new T()); }
            set { _Object = value; }
        } private T _Object;
    }
}
