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
                var currentDirectory = new ShimDirectoryInfo()
                {
                    GetDirectoriesString = (s) => new[] {new ShimDirectoryInfo() {NameGet = () => "sibling"}.Instance}
                }.Instance;

                var result = currentDirectory.GetDirectoriesRecursively("sibling");
                Assert.That(result, Has.Length.EqualTo(1));
            }
        }
    }
}