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
        public void GetDirectoriesRecursively_FindCurrentDirectorySibling()
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

        [Test]
        public void GetDirectoriesRecursively_FindParentSibling()
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
    }
}