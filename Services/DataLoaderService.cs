using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.DataExtractors.BibTeX;
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
            var extractor = new BibTeXDataExtractor(context);
            var submissions = extractor.Extract(File.ReadAllText(@"E:\Proyectos Visual Studio\iei-paper-search-master\Json\sample_array.json"));

            context.Books.AddRange((ICollection<Book>)submissions.Where(s => s is Book).ToList());
            context.Articles.AddRange((ICollection<Article>)submissions.Where(s => s is Article).ToList());
            context.InProceedings.AddRange((ICollection<InProceedings>)submissions.Where(s => s is InProceedings).ToList());
            context.SaveChanges();
        }
    }
}
