namespace System
{
    using Collections;
    using Linq;

    public class Null
    {
        private static readonly Null Instance = new Null();

        private Null() { }

        public static Null OrEmpty
        {
            get { return Instance; }
        }

        public static bool operator ==(IEnumerable collection, Null n)
        {
            return collection == null || !collection.Cast<object>().Any();
        }

        public static bool operator !=(IEnumerable collection, Null n)
        {
            return !(collection == n);
        }

        public static bool operator ==(string s, Null n)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool operator !=(string s, Null n)
        {
            return !(s == n);
        }

        public static implicit operator string(Null n)
        {
            return string.Empty;
        }

        public bool Any(params string[] checks)
        {
            return checks.Any(s => s == Instance);
        }

        public override bool Equals(object obj)
        {
            var asNull = obj as Null;
            if (asNull != null) return true;

            var asString = obj as string;
            if (asString != null) return asString == Instance;

            var asEnumerable = obj as IEnumerable;
            if (asEnumerable != null) return asEnumerable == Instance;

            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}