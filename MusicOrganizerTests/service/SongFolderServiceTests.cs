using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicOrganizer.events;
using MusicOrganizer.repository;
using MusicOrganizer.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;

namespace MusicOrganizerTests.service {
    [TestClass]
    public class SongFolderServiceTests : ReactiveTest {

        [TestMethod]
        public void GivenFolders_WhenGetAll_ThenAllFoldersAreProvided() {
            var expectedFolders = new List<string>() { "folder/1", "folder/2" };
            var mockRepository = new Mock<SongFolderRepository>();
            mockRepository.Setup(repository => repository.GetAll()).Returns(expectedFolders);
            var service = new SongFolderServiceImpl(mockRepository.Object);

            var folders = service.GetAll();

            CollectionAssert.AreEqual(expectedFolders.ToList(), folders.ToList());
        }

        [TestMethod]
        public void GivenFolders_WhenAddSuccesfully_ThenReturnTrueAndNotify() {
            var newFolder = "folder/1";
            var mockRepository = new Mock<SongFolderRepository>();
            mockRepository.Setup(repository => repository.Add(newFolder)).Returns(true);
            var service = new SongFolderServiceImpl(mockRepository.Object);
            var observer = CreateTestObserver(service.SongFolderUpdates);

            var added = service.Add(newFolder);

            Assert.IsTrue(added);
            Assert.AreEqual(1, observer.Messages.Count);
            observer.Messages.AssertEqual(OnNext(1, new FolderEvent(newFolder, EventType.Add)));
        }

        [TestMethod]
        public void GivenFolders_WhenAddFailed_ThenReturnFalseAndDontNotify() {
            var newFolder = "folder/1";
            var mockRepository = new Mock<SongFolderRepository>();
            mockRepository.Setup(repository => repository.Add(newFolder)).Returns(false);
            var service = new SongFolderServiceImpl(mockRepository.Object);
            var observer = CreateTestObserver(service.SongFolderUpdates);
            
            var added = service.Add(newFolder);

            Assert.IsFalse(added);
            Assert.AreEqual(0, observer.Messages.Count);
        }

        [TestMethod]
        public void GivenFolders_WhenRemoveSuccesfully_ThenReturnTrueAndNotify() {
            var removedFolder = "folder/1";
            var mockRepository = new Mock<SongFolderRepository>();
            mockRepository.Setup(repository => repository.Remove(removedFolder)).Returns(true);
            var service = new SongFolderServiceImpl(mockRepository.Object);
            var observer = CreateTestObserver(service.SongFolderUpdates);

            var removed = service.Remove(removedFolder);

            Assert.IsTrue(removed);
            Assert.AreEqual(1, observer.Messages.Count);
            observer.Messages.AssertEqual(OnNext(1, new FolderEvent(removedFolder, EventType.Remove)));
        }

        [TestMethod]
        public void GivenFolders_WhenRemoveFailed_ThenReturnFalseAndDontNotify() {
            var removedFolder = "folder/1";
            var mockRepository = new Mock<SongFolderRepository>();
            mockRepository.Setup(repository => repository.Remove(removedFolder)).Returns(false);
            var service = new SongFolderServiceImpl(mockRepository.Object);
            var observer = CreateTestObserver(service.SongFolderUpdates);

            var removed = service.Remove(removedFolder);

            Assert.IsFalse(removed);
            Assert.AreEqual(0, observer.Messages.Count);
        }

        private static ITestableObserver<FolderEvent> CreateTestObserver(IObservable<FolderEvent> observable) {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<FolderEvent>();
            scheduler.Schedule(() => observable.Subscribe(observer));
            scheduler.Start();
            return observer;
        }
    }
}
