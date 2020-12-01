using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IEIPaperSearch.DataExtractors
{
    public static class DbSetExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MatchingOrNew<T>(this DbSet<T> set, ICollection<T> cache, T reference)
            where T: class, IEquatable<T>
        {
            var cacheMatch = cache.FirstOrDefault(e => e.Equals(reference));
            if (cacheMatch is not null)
            {
                return cacheMatch;
            }

            return set.MatchingOrNew(reference);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MatchingOrNew<T>(this DbSet<T> set, T reference)
            where T : class, IEquatable<T>
        {
            var dbMatch = set.FirstOrDefault(e => e.Equals(reference));
            if (dbMatch is not null)
            {
                return dbMatch;
            }

            return reference;
        }
    }
}
