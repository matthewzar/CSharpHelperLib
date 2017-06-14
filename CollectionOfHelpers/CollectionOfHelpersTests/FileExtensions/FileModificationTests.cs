using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CollectionOfHelpersTests.FileExtensions
{
    [TestFixture]
    public class FileModificationTests
    {
        //Feedback about testing a static method with side effects:
        //Avoid it when possible. If you can't then these are some options:
        //So typically, I create an interface and a wrapper class for the static class
        //and I always interact with it via the interface
        //so then in tests I can simple subst the interface
        //and then I can have one or two simple tests to validate the wrapper if possible
        //
        //alternately just put some files in a TestData folder in your project and write 
        //a test the evals that the files have been moved

        [Test]
        public void RenameFilesInFolder_NoChangesTest()
        {
            //arrange
            try
            {
                //TODO: create several on-the-fly files that will be cleared away afterwards

                //act
                //TODO: try and move the files

                //assert
                //TODO: check that the files move as expected
            }
            finally
            {
                //cleanup
                //TODO: clear away all files in the dynamic file-directory
            }
        }
    }
}
