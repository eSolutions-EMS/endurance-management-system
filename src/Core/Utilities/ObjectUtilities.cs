using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Core.Utilities
{
    public static class ObjectUtilities
    {
        public static bool IsEqual(IObject a, IObject? b)
        {
            if (ReferenceEquals(null, b))
            {
                return false;
            }
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (b.GetType() != a.GetType())
            {
                return false;
            }

            return a.GetHashCode().Equals(b.GetHashCode());
        }

        public static int GetUniqueObjectCode(IObject obj)
        {
            var uniqueString = obj.GetType().ToString() + obj.ObjectId;
            return uniqueString.GetHashCode();
        }
    }
}
