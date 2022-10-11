using System;

namespace DesignPatternsLibrary.Helpers
{
    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object o) => (T)o;
        public static T As<T>(this object o) where T : class => o as T;
        public static bool TryCastTo<T>(this object o, out T casted) 
        {
            if (o is null)
            {
                casted = default(T);
                return false;
            }

            if (o is T)
            { 
                casted = (T)o;
                return true;
            }
            casted = default(T);
            return false;
        }
    }
}
