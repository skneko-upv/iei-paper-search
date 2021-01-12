#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using System.Collections.Generic;

namespace IEIPaperSearch.Dto
{
    public class SubmissionDto
    {
        /// <summary>
        /// The title of this paper or publication.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// The year in which this paper or publication was released to the public.
        /// </summary>
        public int Year { get; internal set; }

        /// <summary>
        /// A URL pointing to a readable digital upload of this paper or publication.
        /// </summary>
        public string? URL { get; internal set; }

        /// <summary>
        /// A collection of the people who have authored this paper or publication.
        /// </summary>
        public ICollection<PersonDto> Authors { get; internal set; }
    }
}
