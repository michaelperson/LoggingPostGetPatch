using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingAndErrors.Infra
{
    public static class MyExtensions
    {

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                yield return await Task.FromResult(item);
            }
        } 
    }
}

