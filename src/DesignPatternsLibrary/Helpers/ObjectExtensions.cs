namespace DesignPatternsLibrary.Helpers
{
    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object o) => (T)o;
        public static T As<T>(this object o) where T : class => o as T;
    }
}
