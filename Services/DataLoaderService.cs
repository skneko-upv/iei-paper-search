using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.DataExtractors.Bibtex;
using IEIPaperSearch.DataExtractors.IeeeXplore;
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
            LoadFromDblp();
            LoadFromIeeeXplore();
            LoadFromBibtex();
        }

        public void LoadFromDblp()
        {
            var extractor = new DblpDataExtractor(context);
            var articles = extractor.Extract(File.ReadAllText(@"E:\Proyectos Visual Studio\iei-paper-search-master\Json\DBLP-SOLO_ARTICLE_SHORT.json"));

            context.Articles.AddRange(articles);
            context.SaveChanges();
        }

        public void LoadFromIeeeXplore()
        {
            var extractor = new IeeeXploreDataExtractor(context);
            var submissions = extractor.Extract(File.ReadAllText(@"E:\Proyectos Visual Studio\iei-paper-search-master\Json\ieeeXplore_2018-2020-short.json"));

            context.Books.AddRange(submissions.Where(s => s is Book).Select(s => (Book)s).ToList());
            context.Articles.AddRange(submissions.Where(s => s is Article).Select(s => (Article)s).ToList());
            context.InProceedings.AddRange(submissions.Where(s => s is InProceedings).Select(s => (InProceedings)s).ToList());
            context.SaveChanges();
        }

        public void LoadFromBibtex()
        {
            var extractor = new BibtexDataExtractor(context);
            var submissions = extractor.Extract(File.ReadAllText(@"E:\Proyectos Visual Studio\iei-paper-search-master\Json\sample_array.json"));

            context.Books.AddRange(submissions.Where(s => s is Book).Select(s => (Book)s).ToList());
            context.Articles.AddRange(submissions.Where(s => s is Article).Select(s => (Article)s).ToList());
            context.InProceedings.AddRange(submissions.Where(s => s is InProceedings).Select(s => (InProceedings)s).ToList());
            context.SaveChanges();
        }
    }
}
