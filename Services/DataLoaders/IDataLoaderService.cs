namespace IEIPaperSearch.Services.DataLoaders
{
    public interface IDataLoaderService
    {
        public void LoadFromAllSources();

        public void LoadFromDblp(string xml);

        public void LoadFromIeeeXplore();

        public void LoadFromGoogleScholar();
    }
}
