using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolrWind.Net;

namespace SolrWind.Tests
{
    [TestClass]
    public class FunctionalityTests
    {
        class Brand
        {
            public string id;
            public string brand;
            public string attorney;
            public string classes;
            public string holders;
        }

        [TestMethod]
        public void DataPumpTest()
        {
            var documents = new[]
            {
                new Brand()
                {
                    id ="B2009/67185",
                    brand = "plast textil şekil",
                    attorney = "YUSUF ÖZDAMAR YÖA MAKRO PATENT MARKA VE FİKRİ HAKL",
                    holders = "PLAST-TEXTIL SL",
                    classes ="22"
                },

                new Brand(){
                    id = "B2014/6752",
                    brand = "thomson",
                    attorney = "GÜLÇİN GÖKÜŞ DERİŞ PATENT VE MARKA A. Ş.",
                    holders = "TECHNICOLOR",
                    classes = "7, 9, 11"
                },

                new Brand(){
                    id = "B2004/44966",
                    brand = "imza",
                    attorney = "DESTEK PATENT A. Ş.",
                    holders = "TAŞKINIRMAK GİYİM SAN. VE TİC. A. Ş.",
                    classes = "3, 5, 24, 25, 27, 35"
                }
            };

            var solr = new SolrService("192.168.99.26");

            var coll = solr.GetCollection("Brands");
            
            coll.DataPump(documents, CancellationToken.None)
            .Wait();

            coll.DeleteSingle("B2004/44966");

            coll.CommitAndOptimize();
        }
    }
}
