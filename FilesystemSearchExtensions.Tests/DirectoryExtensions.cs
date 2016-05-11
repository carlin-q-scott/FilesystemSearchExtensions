using System;
using System.IO;
using System.IO.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace FilesystemSearchExtensions.Tests
{
    [TestFixture]
    public class DirectoryExtensions
    {
        [Test]
        public void GetDirectoriesRecursively_FindCurrentDirectorySibling()
        {
            using (ShimsContext.Create())
            {
                var targetDirectory = new ShimDirectoryInfo { NameGet = () => "sibling" }.Instance;
                var currentDirectory = new ShimDirectoryInfo
                {
                    GetDirectoriesString = s => new[] { targetDirectory }
                }.Instance;

                var result = currentDirectory.GetDirectoriesRecursively("sibling");
                Assert.That(result, Has.Length.EqualTo(1));
                Assert.That(result, Has.Member(targetDirectory));
            }
        }

        [Test]
        public void GetDirectoriesRecursively_FindParentSibling()
        {
            using (ShimsContext.Create())
            {
                var targetDirectory = new ShimDirectoryInfo {NameGet = () => "sibling"}.Instance;
                var currentDirectory = new ShimDirectoryInfo
                {
                    GetDirectoriesString = s => new DirectoryInfo[0],
                    ParentGet = () => new ShimDirectoryInfo
                    {
                        GetDirectoriesString = s => new[] { targetDirectory },
                    }.Instance
                }.Instance;

                var result = currentDirectory.GetDirectoriesRecursively("aunt/uncle");
                Assert.That(result, Has.Length.EqualTo(1));
                Assert.That(result, Has.Member(targetDirectory));
            }
        }

        [Test]
        public void GetDirectoriesRecursively_HandlesFailedSearch()
        {
            using (ShimsContext.Create())
            {
                var currentDirectory = new ShimDirectoryInfo
                {
                    GetDirectoriesString = s => new DirectoryInfo[0]
                }.Instance;

                var result = currentDirectory.GetDirectoriesRecursively("sibling");
                Assert.That(result, Has.Length.EqualTo(0));
            }
        }
    }
}