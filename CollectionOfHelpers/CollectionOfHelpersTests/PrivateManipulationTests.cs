using System;
using System.Collections.Generic;
using CollectionOfHelpers.Reflection;
using NUnit.Framework;
using CollectionAssert = NUnit.Framework.CollectionAssert;

namespace CollectionOfHelpersTests
{
    [TestFixture]
    public class PrivateManipulationTests
    {
        private class PrivateFieldEvaluator
        {
            private int[] _privateReferenceArray = {1, 2, 3};
            public int[] PublicReferenceArray = {4, 5, 6};
            private static int[] _privateStaticReferenceArray = { 7, 8, 9 };

            public PrivateFieldEvaluator()
            { }

            public PrivateFieldEvaluator(int[] startingValues)
            {
                _privateReferenceArray = startingValues;
            }

            public int[] ExposePrivateField()
            {
                return _privateReferenceArray;
            }
        }

        [Test]
        public void DefaultPrivateReferenceRead()
        {
            var sut = new PrivateFieldEvaluator();
            var expected = new [] { 1, 2, 3 };

            //act
            int[] actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "_privateReferenceArray", out actualOut);

            //assert
            Assert.IsTrue(actualSuccess, "Failed to read value for _privateReferenceArray field");
            CollectionAssert.AreEqual(actualOut, expected);
        }

        [Test]
        public void DefaultPrivateReferenceReadAndUpcast()
        {
            var sut = new PrivateFieldEvaluator();
            var expected = new[] { 1, 2, 3 };

            //act
            IList<int> actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "_privateReferenceArray", out actualOut);

            //assert
            Assert.IsTrue(actualSuccess, "Upcasting from int[] to IList appears to have failed");
            CollectionAssert.AreEqual(actualOut, expected);
        }

        [Test]
        public void InvalidTypeOnExistingFieldShouldFail()
        {
            var sut = new PrivateFieldEvaluator();
            var expected = new[] { 1, 2, 3 };

            //act
            double[] actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "_privateReferenceArray", out actualOut);

            //assert
            Assert.IsFalse(actualSuccess, "Should return false, as int[] cannot be cast to double[]");
        }

        [Test]
        public void NonExistentPrivateReferenceRead()
        {
            var sut = new PrivateFieldEvaluator();

            //act
            int[] actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "_noSuchPrivateField", out actualOut);

            //assert
            Assert.IsFalse(actualSuccess, "Somehow found a reference to _noSuchPrivateField");
        }

        [Test]
        public void PublicFieldsShouldNotRead()
        {
            var sut = new PrivateFieldEvaluator();

            //act
            int[] actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "PublicReferenceArray", out actualOut);

            //assert
            Assert.IsFalse(actualSuccess, "The public field PublicReferenceArray was read, when we are looking for private-only fields");
        }

        [Test]
        public void PrivateFieldsWithNullValueShouldReadAndReturnTrue()
        {
            var sut = new PrivateFieldEvaluator(null);
            int[] expected = null;

            //act
            int[] actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "_privateReferenceArray", out actualOut);

            //assert
            Assert.IsTrue(actualSuccess, "Failed to read value for _privateReferenceArray after it was set to null");
            Assert.IsNull(actualOut, "A non-null value was found for _privateReferenceArray");
        }

        [Test]
        public void StaticFieldsShouldNotRead()
        {
            var sut = new PrivateFieldEvaluator();

            //act
            int[] actualOut;
            bool actualSuccess = PrivateManipulation.TryGetPrivateReferenceField(sut, "_privateStaticReferenceArray", out actualOut);

            //assert
            Assert.IsFalse(actualSuccess, "Static fields, even private ones, are not supposed to be readable here.");
        }

        [TestCase(new int[] {1,1,1})]
        [TestCase(null)]
        public void SetPrivateFieldValue_ValidInputExpectingSuccess(int[] newValue)
        {
            var sut = new PrivateFieldEvaluator();

            //act
            bool actualSuccess = PrivateManipulation.TrySetPrivateField(sut, "_privateReferenceArray", newValue);
            var newValueReadFromClass = sut.ExposePrivateField();

            //assert
            Assert.IsTrue(actualSuccess, "A valid assignment returned false");
            Assert.AreEqual(newValue, newValueReadFromClass, "The two collection should be identical reference to one another");
        }

        [TestCase(new int[] { 1, 1, 1 })]
        [TestCase(null)]
        public void SetPrivateFieldValue_NonExistentFieldExpectingFailure(int[] newValue)
        {
            var sut = new PrivateFieldEvaluator();

            //act
            bool actualSuccess = PrivateManipulation.TrySetPrivateField(sut, "_noSuchField", newValue);

            //assert
            Assert.IsFalse(actualSuccess);
        }

        [TestCase(new double[] { 1, 1, 1 })]
        [TestCase(1)]
        public void SetPrivateFieldValue_InvalidInputTypeExpectingFailure<T>(T newValue)
        {
            var sut = new PrivateFieldEvaluator();

            //act
            bool actualSuccess = PrivateManipulation.TrySetPrivateField(sut, "_privateReferenceArray", newValue);
            var newValueReadFromClass = sut.ExposePrivateField();

            //assert
            Assert.IsFalse(actualSuccess, "The types don't match, so assignment should have failed");
        }

        //TODO - add tests for TrySetPrivateField that attempt to set public and static fields.
    }
}
