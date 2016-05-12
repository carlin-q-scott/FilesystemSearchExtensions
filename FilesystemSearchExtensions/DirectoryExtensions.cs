using System.IO;
using System.Linq;

namespace FilesystemSearchExtensions
{
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Finds the path to a related directory, which is a directory that is a child, sibling of the current directory or its parent/grandparent/etc.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static DirectoryInfo[] GetDirectoriesRecursively(this DirectoryInfo currentDirectory, string searchPattern)
        {
            var matchingSubDirectories = currentDirectory.GetDirectories(searchPattern);

            if (matchingSubDirectories.Length > 0) return matchingSubDirectories;
            if (currentDirectory.Parent == null) return new DirectoryInfo[0];

            return GetDirectoriesRecursively(currentDirectory.Parent, searchPattern);
        }

        /// <summary>
        /// Finds the path to a related directory, which is a directory that is a child, sibling of the current directory or its parent/grandparent/etc.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string[] GetDirectoriesRecursively(string searchPattern)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            return GetDirectoriesRecursively(currentDirectory, searchPattern).Select(di => di.FullName).ToArray();
        }

        /// <summary>
        /// Finds the path to a related file, which is a file that is in the current directory or in a parent/grandparent/etc directory.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static FileInfo[] GetFilesRecursively(this DirectoryInfo currentDirectory, string searchPattern)
        {
            var matchingFiles = currentDirectory.GetFiles(searchPattern);

            if (matchingFiles.Length > 0) return matchingFiles;
            if (currentDirectory.Parent == null) return new FileInfo[0];

            return GetFilesRecursively(currentDirectory.Parent, searchPattern);
        }

        /// <summary>
        /// Finds the path to a related file, which is a file that is in the current directory or in a parent/grandparent/etc directory.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursively(string searchPattern)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            return GetFilesRecursively(currentDirectory, searchPattern).Select(di => di.FullName).ToArray();
        }
    }
}
