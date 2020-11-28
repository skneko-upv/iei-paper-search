using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.Persistence
{
    public static class DbSetExtensions
    {
        public static T MatchingOrNew<T>(this DbSet<T> set, T reference)
            where T : class, IEquatable<T>
        {
            var match = set.FirstOrDefault(e => e.Equals(reference));
            return (match is null) ? reference : match;
        }

        public static T MatchingOrNew<T>(this IEnumerable<T> set, T reference)
            where T: class, IEquatable<T>
        {
            var match = set.FirstOrDefault(e => e.Equals(reference));
            return (match is null) ? reference : match;
        }
    }
}
