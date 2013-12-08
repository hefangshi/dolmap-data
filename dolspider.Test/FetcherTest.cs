using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using HtmlAgilityPack;
using dolspider.Spiders.ENorth.Quest.Handlers;

namespace dolspider.Test
{
    
    
    /// <summary>
    ///This is a test class for FetcherTest and is intended
    ///to contain all FetcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FetcherTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetContentTest()
        {
            Uri uri = new Uri("http://dols.enorth.com.cn/mission.do?page=1&act=list"); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Util.GetContent(uri, Encoding.GetEncoding("GBK"));
            Assert.IsTrue(actual.Contains("<title>任务----大航海时代OL互助系统</title>"));
        }

        /// <summary>
        ///A test for GetDoc
        ///</summary>
        [TestMethod()]
        public void GetDocTest()
        {
            Uri uri = new Uri("http://dols.enorth.com.cn/mission.do?page=1&act=list"); // TODO: Initialize to an appropriate value
            Encoding encoding = Encoding.GetEncoding("GBK"); // TODO: Initialize to an appropriate value
            string expected = "任务----大航海时代OL互助系统"; // TODO: Initialize to an appropriate value
            HtmlDocument actual;
            actual = Util.GetDoc(uri, encoding);
            var c = PageCountHandler.GetPageCount(actual);
            Assert.AreEqual(8, c);
            Assert.AreEqual(expected, actual.DocumentNode.SelectSingleNode("//title").InnerText);
        }
    }
}
