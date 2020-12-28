using Newtonsoft.Json;
using System.IO;
using System.Xml;

namespace IEIPaperSearch.DataSourceWrappers.DBLP
{
    public class DblpXmlConverterWrapper
    {
        public string ExtractFromXml(string xml)
        {
            var xdocument = new XmlDocument();
            xdocument.LoadXml(xml);

            var json = JsonConvert.SerializeXmlNode(xdocument);
            return json;
        }
    }
}
