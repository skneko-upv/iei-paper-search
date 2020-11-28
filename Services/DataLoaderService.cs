using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.Persistence;
using System;
using System.IO;

namespace IEIPaperSearch.Services
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
            // TODO: Load data
            throw new NotImplementedException();
        }

        public void Test()
        {
            var extractor = new DblpDataExtractor(context);
            var articles = extractor.Extract(File.ReadAllText(@"C:\Users\Neko\Desktop\dblp-solo-article-1.json"));

            context.Articles.AddRange(articles);
            context.SaveChanges();
        }
    }
}
