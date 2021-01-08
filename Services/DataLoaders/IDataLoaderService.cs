using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.Services.DataLoaders
{
    public interface IDataLoaderService
    {
        /// <summary>
        /// Load data from DBLP static XML files into the local persistent storage.
        /// </summary>
        /// <returns>The number of new submissions added.</returns>
        public DataLoaderResult LoadFromDblp();

        /// <summary>
        /// Load data from the IEEE Xplore REST API files into the local persistent storage.
        /// </summary>
        /// <returns>The number of new submissions added.</returns>
        public DataLoaderResult LoadFromIeeeXplore();

        /// <summary>
        /// Load scraped data from the Google Scholar website into the local persistent storage.
        /// </summary>
        /// <returns>The number of new submissions added.</returns>
        public DataLoaderResult LoadFromGoogleScholar();

        /// <summary>
        /// An aggregation of diagnostic data of a data loading procedure.
        /// </summary>
        public struct DataLoaderResult
        {
            public int InsertedSubmissionCount { get; }
            public ICollection<string> Errors { get; }

            public DataLoaderResult(int insertedSubmissionCount, IEnumerable<string>? errors = null)
            {
                InsertedSubmissionCount = insertedSubmissionCount;
                Errors = errors is null ? new HashSet<string>() : errors.ToHashSet();
            }

            public static DataLoaderResult operator +(DataLoaderResult a, DataLoaderResult b) =>
                new DataLoaderResult(
                    a.InsertedSubmissionCount + b.InsertedSubmissionCount,
                    a.Errors.Concat(b.Errors));
        }
    }
}
