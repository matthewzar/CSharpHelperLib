using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using CollectionOfHelpers.GeneralExtensions;

namespace CollectionOfHelpersTests
{
    /// <summary>
    /// Summary description for DictionaryExtensions
    /// </summary>
    [TestFixture]
    public class DictionaryExtensionTests
    {
        [TestCase("Hi","there",false)]
        [TestCase("Hi", "you", false)]
        public void Dictionary_TryAdd_KeyExists(string key, string value, bool expected)
        {
            //Arrange
            var dic = new Dictionary<string, string>();
            dic.Add("Hi", "there");

            //act
            var actual = dic.TryAdd(key, value);

            //assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(dic.Count, 1);
        }

        [TestCase("Hello", "there", true)]
        [TestCase("Hello", "you", true)]
        public void Dictionary_TryAdd_KeyDoesntExist(string key, string value, bool expected)
        {
            //Arrange
            var dic = new Dictionary<string, string>();
            dic.Add("Hi", "there");

            //act
            var actual = dic.TryAdd(key, value);

            //assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(dic.Count, 2);
        }

        
        [TestCase("Hi", "there", 1)]
        [TestCase("Hi", "you", 1)]
        public void Dictionary_AddOrUpdate_KeyExists(string key, string value, int expected)
        {
            //Arrange
            var dic = new Dictionary<string, string>();
            dic.Add("Hi", "there");

            //act
            dic.AddOrUpdate(key, value);

            //assert
            Assert.AreEqual(value, dic[key]);
            Assert.AreEqual(expected, dic.Count);
        }

        [TestCase("Hello", "there", 2)]
        [TestCase("Hello", "you", 2)]
        public void Dictionary_AddOrUpdate_KeyDoesntExist(string key, string value, int expected)
        {
            //Arrange
            var dic = new Dictionary<string, string>();
            dic.Add("Hi", "there");

            //act
            dic.AddOrUpdate(key, value);

            //assert
            Assert.AreEqual(value, dic[key]);
            Assert.AreEqual(expected, dic.Count);
        }
    }
}
