using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace IEIPaperSearch.Models
{
    public class Person : IEquatable<Person>
    {
        public int Id { get; private set; }

        public string Name { get; set; }
        public string Surnames { get; set; }

        public ICollection<Submission> AuthorOf { get; set; } = new HashSet<Submission>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Person()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Person(string nameAndSurnames)
        {
            var tokens = nameAndSurnames.Split(' ', count: 2).Select(t => t.Trim()).ToArray();

            Name = tokens[0];
            Surnames = tokens.Length > 1 ? tokens[1] : "";
        }

        public Person(string name, string surnames)
        {
            Name = name;
            Surnames = surnames;
        }

        public bool Equals([AllowNull] Person other)
            => (other != null) && ($"{other.Name} {other.Surnames}" == $"{Name} {Surnames}");

        public override string ToString() => $"{Name[0]}. {Surnames}";
    }
}
