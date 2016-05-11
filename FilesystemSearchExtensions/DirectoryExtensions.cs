using System.IO;
using System.Linq;

namespace FilesystemSearchExtensions
{
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Finds the path to an ancestor directory, which is a directory that is a sibling of the current directory or its parent/grandparent/etc.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static DirectoryInfo[] GetDirectoriesRecursively(this DirectoryInfo currentDirectory, string searchPattern)
        {
            var matchingSubDirectories = currentDirectory.GetDirectories(searchPattern);

            if (matchingSubDirectories.Length > 0) return matchingSubDirectories;

            return GetDirectoriesRecursively(currentDirectory.Parent, searchPattern);
        }

        public static string[] GetDirectoriesRecursively(string searchPattern)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            return GetDirectoriesRecursively(currentDirectory, searchPattern).Select(di => di.FullName).ToArray();
        }
    }
}
