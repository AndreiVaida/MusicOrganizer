using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicOrganizer.repository;
using MusicOrganizer.service;
using System.Collections.Generic;
using System.Linq;

namespace MusicOrganizerTests.service {
    [TestClass]
    public class SongFolderServiceTests {

        [TestMethod]
        public void GivenFolders_WhenGetAll_ThenAllFoldersAreProvided() {
            IEnumerable<string> expectedFolders = new List<string>() { "folder/1", "folder/2" };
            Mock<SongFolderRepository> mockRepository = new ();
            mockRepository.Setup(repository => repository.GetAll()).Returns(expectedFolders);
            SongFolderService service = new SongFolderServiceImpl(mockRepository.Object);

            var folders = service.GetAll();
            CollectionAssert.AreEqual(expectedFolders.ToList(), folders.ToList());
        }
    }
}
