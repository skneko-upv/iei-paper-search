using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace IEIPaperSearch.DataSourceWrappers.DBLP
{
    public class DblpDataExtractor
    {
        public ICollection<dblpArticle> ExtractData(string path)
        {
            var stream = File.OpenRead(path);

            ICollection<dblpArticle> articles;
            try
            {
                articles = DeserializeXml(stream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.Close();
            }

            return articles;
        }

        private ICollection<dblpArticle> DeserializeXml(Stream xml)
        {
            var dblp = new XmlSerializer(typeof(dblp)).Deserialize(xml);
            return ((dblp)dblp).Items;
        }
    }
}
