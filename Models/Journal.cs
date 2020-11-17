using System.Collections.Generic;

namespace IEIPaperSearch.Models
{
    public class Journal    // Revista
    {
        public string Name { get; set; }

        public ICollection<Issue> Issues { get; set; } = new HashSet<Issue>();

        public Journal(string name)
        {
            Name = name;
        }
    }
}
