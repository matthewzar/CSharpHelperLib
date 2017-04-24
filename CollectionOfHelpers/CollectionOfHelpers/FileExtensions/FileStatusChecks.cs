using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionOfHelpers.FileExtensions
{
    public static class FileStatusChecks
    {
        /// <summary>
        /// Returns true of the given file is:
        ///  still being written to
        ///  or being processed by another thread
        ///  or does not exist (has already been processed)
        /// IMPORTANT: File permissions (even file existence) are volatile — they can change at any time.
        /// What this means is that you still have to be ready to handle the exception if file permissions
        /// or existence are bad, in spite of using this check. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }
    }
}
