using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionOfHelpers.StringExtensions;
using NUnit.Framework;

namespace CollectionOfHelpersTests.StringExtensions
{
    [TestFixture]
    public class StringFormattingTests
    {
        [TestCase("Singleton", "Singleton")]
        [TestCase("", "")]
        [TestCase("HelloThere", "Hello There")]
        public void RemoveCamelCase_TrivialStrings(string input, string expected)
        {
            //arrange

            //act
            var actual = input.RemoveCamelCase();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("IBMWatson", "IBM Watson")]
        [TestCase("IWouldBuyIMB", "I Would Buy IMB")]
        [TestCase("AAStandsForAcholicsAnonymous", "AA Stands For Acholics Anonymous")]
        [TestCase("IBMAsACompanyIsPTY", "IBM As A Company Is PTY")]
        public void RemoveCamelCase_AcronymStrings(string input, string expected)
        {
            //arrange

            //act
            var actual = input.RemoveCamelCase();

            //assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestCase("IWouldBuy2IBMMachines", "I Would Buy 2 IBM Machines")]
        [TestCase("59ANNCannotDoIt", "59 ANN Cannot Do It")]
        [TestCase("Insert99Bottles", "Insert 99 Bottles")]
        [TestCase("OnTheWall59", "On The Wall 59")]
        public void RemoveCamelCase_NumbersAndAcronymsStrings(string input, string expected)
        {
            //arrange

            //act
            var actual = input.RemoveCamelCase();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Its_A_TestYo", "Its_A_Test Yo")]
        [TestCase("OneMiddle_Underscore", "One Middle_Underscore")]
        [TestCase("TwoWords_TwoMore", "Two Words_Two More")]
        public void RemoveCamelCase_IgnoreUnderscores(string input, string expected)
        {
            //arrange

            //act
            var actual = input.RemoveCamelCase();

            //assert
            Assert.AreEqual(expected, actual);
        }


        [TestCase("Big_Bunny.LikeHotChocolate", "Big_Bunny - Like Hot Chocolate")]
        [TestCase("BigBunny.LikeChocolate", "BigBunny - Like Chocolate")]
        [TestCase("BigBunny.LikeHotChocolate", "BigBunny - Like Hot Chocolate")]
        [TestCase("BigBunny.LikeHotChocolate1", "BigBunny - Like Hot Chocolate 1")]
        [TestCase("BigBunny.10TimesHappyLikeHotChocolate", "BigBunny - 10 Times Happy Like Hot Chocolate")]
        public void TemporarySpecialFormattingTest(string input, string expected)
        {
            //arrange

            //act
            string[] split = input.Split('.');
            var actual = split[0] + " - " + split[1].RemoveCamelCase();

            //assert
            Assert.AreEqual(expected, actual);
        }


    }
}
