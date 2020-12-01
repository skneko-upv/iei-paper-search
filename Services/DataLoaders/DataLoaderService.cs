using IEIPaperSearch.DataExtractors;
using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.DataExtractors.Bibtex;
using IEIPaperSearch.DataExtractors.IeeeXplore;
using IEIPaperSearch.Persistence;
using System;
using System.IO;

namespace IEIPaperSearch.Services.DataLoaders
{
    internal class DataLoaderService : IDataLoaderService
    {
        private readonly PaperSearchContext context;

        public DataLoaderService(PaperSearchContext context)
        {
            this.context = context;
        }

        public void LoadFromAllSources()
        {
            LoadFromDblp();
            LoadFromIeeeXplore();
            LoadFromBibtex();
        }

        public void LoadFromDblp() => ExtractFromJsonSource(
            new DblpDataExtractor(context),
            File.ReadAllText(Environment.GetEnvironmentVariable("IEIPS_JSONSRC_DBLP")!));

        public void LoadFromIeeeXplore() => ExtractFromJsonSource(
            new IeeeXploreDataExtractor(context),
            File.ReadAllText(Environment.GetEnvironmentVariable("IEIPS_JSONSRC_IEEEXPLORE")!));

        public void LoadFromBibtex() => ExtractFromJsonSource(
            new BibtexDataExtractor(context),
            File.ReadAllText(Environment.GetEnvironmentVariable("IEIPS_JSONSRC_BIBTEX")!));

        private void ExtractFromJsonSource(IJsonDataExtractor<SubmissionDataExtractorResult> extractor, string json)
        {
            var data = extractor.Extract(json);

            context.Books.AddRange(data.Books);
            context.Articles.AddRange(data.Articles);
            context.InProceedings.AddRange(data.InProceedings);
            context.SaveChanges();
        }
    }
}
