using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnduranceJudge.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<List<T>> ToList<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var result = await enumerableTask;
            return result.ToList();
        }
    }
}
