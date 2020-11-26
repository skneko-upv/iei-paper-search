using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Person : IEquatable<Person>
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }
        public string Surnames { get; set; }

        public ICollection<Submission> AuthorOf { get; set; } = new HashSet<Submission>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Person()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Person(string name, string surnames)
        {
            Name = name;
            Surnames = surnames;
        }

        public bool Equals([AllowNull] Person other)
            => (other != null) && ($"{other.Name} {other.Surnames}" == $"{Name} {Surnames}");
    }
}
