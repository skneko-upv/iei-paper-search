using System.Collections.Generic;

namespace IEIPaperSearch.Models
{
    public class Journal    // Revista
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public ICollection<Issue> Issues { get; set; } = new HashSet<Issue>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Journal()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Journal(string name)
        {
            Name = name;
        }
    }
}
