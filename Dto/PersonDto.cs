#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace IEIPaperSearch.Dto
{
    public class PersonDto
    {
        /// <summary>
        /// The name of this person up to the first space.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The tail of the name of this person.
        /// </summary>
        public string Surnames { get; set; }
    }
}
