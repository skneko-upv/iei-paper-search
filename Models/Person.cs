using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Person : IEquatable<Person>
    {
        public string Name { get; set; }
        public string Surnames { get; set; }

        public ICollection<Submission> AuthorOf { get; set; } = new HashSet<Submission>();

        public Person(string name, string surnames)
        {
            Name = name;
            Surnames = surnames;
        }

        public bool Equals([AllowNull] Person other)
            => (other != null) && ($"{other.Name} {other.Surnames}" == $"{Name} {Surnames}");
    }
}
