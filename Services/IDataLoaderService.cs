namespace IEIPaperSearch.Services
{
    internal interface IDataLoaderService
    {
        public void LoadFromAllSources();

        public void LoadFromDblp();

        public void LoadFromIeeeXplore();

        public void LoadFromBibtex();
    }
}
