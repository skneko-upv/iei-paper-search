namespace IEIPaperSearch.Services.DataLoaders
{
    public interface IDataLoaderService
    {
        public void LoadFromAllSources();

        public void LoadFromDblp();

        public void LoadFromIeeeXplore();

        public void LoadFromGoogleScholar();
    }
}
