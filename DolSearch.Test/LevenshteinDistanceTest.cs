using DolSearch.LD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DolSearch.Test
{
    
    
    /// <summary>
    ///This is a test class for LevenshteinDistanceTest and is intended
    ///to contain all LevenshteinDistanceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LevenshteinDistanceTest
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
        ///A test for Compute
        ///</summary>
        [TestMethod()]
        public void ComputeTest()
        {
            LevenshteinDistanceBase target = new LevenshteinDistance(); // TODO: Initialize to an appropriate value
            var a=target.Compute("aunt", "ant");
            var b=target.Compute("Sam", "Samantha");
            var c=target.Compute("flomax", "volmax");
            var d = target.Compute("acfmax", "efgmax");
            var e = target.Compute("acfmax", "ekgmax");


            Assert.AreEqual(1, a);
            Assert.AreEqual(5, b);
            Assert.AreEqual(3, c);
        }

    }
}
