namespace IEIPaperSearch.DataExtractors
{
    internal interface IDataExtractor<S, D>
    {
        public D Extract(S source);
    }
}
