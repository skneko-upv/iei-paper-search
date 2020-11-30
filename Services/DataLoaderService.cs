using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.DataExtractors.Bibtex;
using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var extractor = new BibtexDataExtractor(context);
            var submissions = extractor.Extract(File.ReadAllText(@"C:\Users\Neko\Desktop\sample_array.json"));

            context.Books.AddRange(submissions.Where(s => s is Book).Select(s => (Book)s).ToList());
            context.Articles.AddRange(submissions.Where(s => s is Article).Select(s => (Article)s).ToList());
            context.InProceedings.AddRange(submissions.Where(s => s is InProceedings).Select(s => (InProceedings)s).ToList());
            context.SaveChanges();
        }
    }
}
