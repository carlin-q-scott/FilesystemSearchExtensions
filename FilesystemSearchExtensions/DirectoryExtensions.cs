using System.IO;
using System.Linq;

namespace FilesystemSearchExtensions
{
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Finds the path to matching related directories, which are directories that are children, siblings of the current directory or its parent/grandparent/etc. The search stops as soon as any matches are found.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns>array of matching directories</returns>
        public static DirectoryInfo[] GetDirectoriesRecursively(this DirectoryInfo currentDirectory, string searchPattern)
        {
            var matchingSubDirectories = currentDirectory.GetDirectories(searchPattern);

            if (matchingSubDirectories.Length > 0) return matchingSubDirectories;
            if (currentDirectory.Parent == null) return new DirectoryInfo[0];

            return GetDirectoriesRecursively(currentDirectory.Parent, searchPattern);
        }

        /// <summary>
        /// Finds the path to matching related directories, which are directories that are children, siblings of the current directory or its parent/grandparent/etc. The search stops as soon as any matches are found.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns>array of matching directory paths</returns>
        public static string[] GetDirectoriesRecursively(string searchPattern)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            return GetDirectoriesRecursively(currentDirectory, searchPattern).Select(di => di.FullName).ToArray();
        }

        /// <summary>
        /// Finds the path to matching related directories, which are directories that are children, siblings of the current directory or its parent/grandparent/etc. The search stops as soon as any matches are found.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns>matching directory path or null if one wasn't found</returns>
        public static string GetDirectoryRecursively(string searchPattern)
        {
            return GetDirectoriesRecursively(searchPattern).SingleOrDefault();
        }

        /// <summary>
        /// Finds matching related files, which are files that are in the current directory or in a parent/grandparent/etc directory. The search stops as soon as any matches are found.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns>array of matching files</returns>
        public static FileInfo[] GetFilesRecursively(this DirectoryInfo currentDirectory, string searchPattern)
        {
            var matchingFiles = currentDirectory.GetFiles(searchPattern);

            if (matchingFiles.Length > 0) return matchingFiles;
            if (currentDirectory.Parent == null) return new FileInfo[0];

            return GetFilesRecursively(currentDirectory.Parent, searchPattern);
        }

        /// <summary>
        /// Finds filepaths to matching related files, which is a file that is in the current directory or in a parent/grandparent/etc directory. The search stops as soon as any matches are found.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns>array of matching file paths</returns>
        public static string[] GetFilesRecursively(string searchPattern)
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            return GetFilesRecursively(currentDirectory, searchPattern).Select(di => di.FullName).ToArray();
        }

        /// <summary>
        /// Finds the filepath to a matching related file, which is a file that is in the current directory or in a parent/grandparent/etc directory. The search stops as soon as any matches are found.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns>matching file path or null if one wasn't found</returns>
        public static string GetFileRecursively(string searchPattern)
        {
            return GetFilesRecursively(searchPattern).SingleOrDefault();
        }
    }
}
