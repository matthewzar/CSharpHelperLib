using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace CollectionOfHelpers.Specialised
{
    public class BulkFileDecompressor
    {
        public static string ExtractToUniqueSubDirectory(string fileName, string targetDirectory)
        {
            string conditionalDash = targetDirectory.EndsWith("\\") ? "" : "\\";
            string uniqueAddress = $"{targetDirectory}{conditionalDash}{Guid.NewGuid()}";
            ZipFile.ExtractToDirectory(fileName, uniqueAddress);
            return uniqueAddress;
        }

        public static void DecompressAndMoveAllChildren(string fileName, string targetDirectory)
        {
            if (!File.Exists(fileName)) return;

            //ExtractToUniqueSubDirectory() // create a temporary holding cell for all children
            string temporaryHoldingDirectory = ExtractToUniqueSubDirectory(fileName, targetDirectory);

            //Merge all the extracted files and folders into the targetDir directory
            CopyAll(new DirectoryInfo(temporaryHoldingDirectory), new DirectoryInfo(targetDirectory));

            File.Delete(fileName);
            foreach (var zipFile in GetAllZipFilesInDirectory(targetDirectory))
            {
                DecompressAndMoveAllChildren(zipFile, targetDirectory);
            }
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the targetDir directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                MoveFileAndOverwriteOnSizeMatch(target, fi);

                fi.Delete();
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }

            source.Delete();
        }

        private static void MoveFileAndOverwriteOnSizeMatch(DirectoryInfo targetDir, FileInfo fileToMove)
        {
            string destFileName = Path.Combine(targetDir.ToString(), fileToMove.Name);

            //Decide between a an straight move/overwrite, and a rename to prevent overlap
            if (!File.Exists(destFileName) || new FileInfo(destFileName).Length == fileToMove.Length)
            {
                fileToMove.CopyTo(Path.Combine(targetDir.ToString(), fileToMove.Name), true);
            }
            else
            {
                int increment = 0;
                while (File.Exists(destFileName.Replace(".", $"({++increment})."))) ;
                fileToMove.CopyTo(Path.Combine(targetDir.ToString(), destFileName.Replace(".", $"({increment}).")), true);
            }
        }

        private static List<string> GetAllZipFilesInDirectory(string sourceDirectory)
        {
            return GetAllZipFilesInDirectory(sourceDirectory, new List<string>());
        }

        private static List<string> GetAllZipFilesInDirectory(string sourceDirectory, List<string> listToPopulate)
        {
            foreach (var filename in Directory.GetFiles(sourceDirectory))
            {
                if (filename.EndsWith(".zip"))
                {
                    listToPopulate.Add(filename);
                }
            }

            foreach (var directory in Directory.GetDirectories(sourceDirectory))
            {
                GetAllZipFilesInDirectory(directory, listToPopulate);
            }

            return listToPopulate;
        }
    }
}