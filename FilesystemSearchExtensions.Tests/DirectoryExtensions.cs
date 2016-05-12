using System;
using System.IO;
using System.IO.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using NUnit.Framework;

namespace FilesystemSearchExtensions.Tests
{
    [TestFixture]
    public class DirectoryExtensions
    {
        private IDisposable _shimsContext;

        [SetUp]
        public void TestSetup()
        {
            _shimsContext = ShimsContext.Create();
            ShimBehaviors.Current = ShimBehaviors.DefaultValue;
        }

        [TearDown]
        public void TestTearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void GetDirectoriesRecursively_FindCurrentDirectoryChild()
        {
            var targetDirectory = new ShimDirectoryInfo { NameGet = () => "child" }.Instance;
            var currentDirectory = new ShimDirectoryInfo
            {
                GetDirectoriesString = s => new[] { targetDirectory }
            }.Instance;

            var result = currentDirectory.GetDirectoriesRecursively("child");
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result, Has.Member(targetDirectory));
        }

        [Test]
        public void GetDirectoriesRecursively_FindCurrentDirectorySibling()
        {
            var targetDirectory = new ShimDirectoryInfo {NameGet = () => "sibling"}.Instance;
            var currentDirectory = new ShimDirectoryInfo
            {
                GetDirectoriesString = s => new DirectoryInfo[0],
                ParentGet = () => new ShimDirectoryInfo
                {
                    GetDirectoriesString = s => new[] { targetDirectory }
                }.Instance
            }.Instance;

            var result = currentDirectory.GetDirectoriesRecursively("sibling");
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result, Has.Member(targetDirectory));
        }

        [Test]
        public void GetDirectoriesRecursively_HandlesFailedSearch()
        {
            var currentDirectory = new ShimDirectoryInfo
            {
                GetDirectoriesString = s => new DirectoryInfo[0]
            }.Instance;

            var result = currentDirectory.GetDirectoriesRecursively("sibling");
            Assert.That(result, Has.Length.EqualTo(0));
        }

        [Test]
        public void GetFilesRecursively_FindCurrentFileSibling()
        {
            var targetFile = new ShimFileInfo { NameGet = () => "sibling" }.Instance;
            var currentDirectory = new ShimDirectoryInfo
            {
                GetFilesString = s => new[] { targetFile }
            }.Instance;

            var result = currentDirectory.GetFilesRecursively("sibling");
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result, Has.Member(targetFile));
        }

        [Test]
        public void GetFilesRecursively_FindParentSibling()
        {
            var targetFile = new ShimFileInfo { NameGet = () => "aunt/uncle" }.Instance;
            var currentDirectory = new ShimDirectoryInfo
            {
                GetFilesString = s => new FileInfo[0],
                ParentGet = () => new ShimDirectoryInfo
                {
                    GetFilesString = s => new[] { targetFile }
                }.Instance
            }.Instance;

            var result = currentDirectory.GetFilesRecursively("aunt/uncle");
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result, Has.Member(targetFile));
        }

        [Test]
        public void GetFilesRecursively_HandlesFailedSearch()
        {
            var currentDirectory = new ShimDirectoryInfo
            {
                GetFilesString = s => new FileInfo[0]
            }.Instance;

            var result = currentDirectory.GetFilesRecursively("sibling");
            Assert.That(result, Has.Length.EqualTo(0));
        }
    }
}