using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Screenmedia.ToDo.Web.Data;
using Screenmedia.ToDo.Web.Data.Models;
using Screenmedia.ToDo.Web.Exceptions;
using Screenmedia.ToDo.Web.Models.ToDoNotes;
using Screenmedia.ToDo.Web.Services;

namespace Screenmedia.ToDo.Tests.Unit
{
    [Category("Unit")]
    public class ToDoNoteServiceTests
    {
        private const int NumberOfNotes = 17;
        private string ApplicationUserId { get; set; } = default!;

        private IQueryable<ToDoNote> _toDoNotes = default!;
        private IFixture _fixture = default!;
        private Mock<IApplicationDbContext> _mockContext = default!;
        private ToDoNoteService _toDoNoteService = default!;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture { OmitAutoProperties = true };

            ApplicationUserId = _fixture.Create<string>();

            var toDoNotesList = new List<ToDoNote>();

            for (var i = 0; i < NumberOfNotes; i++)
            {
                toDoNotesList.Add(
                    new ToDoNote
                    {
                        Id = _fixture.Create<int>(),
                        ApplicationUserId = ApplicationUserId,
                        Title = _fixture.Create<string>(),
                        Description = _fixture.Create<string>()
                    });
            }

            // This note belongs to another user so we should never see it in the tests
            toDoNotesList.Add(
                new ToDoNote
                {
                    Id = _fixture.Create<int>(),
                    ApplicationUserId = _fixture.Create<string>(),
                    Title = _fixture.Create<string>(),
                    Description = _fixture.Create<string>()
                });

            _toDoNotes = toDoNotesList.AsQueryable();

            var mockSet = new Mock<DbSet<ToDoNote>>();
            mockSet.As<IQueryable<ToDoNote>>().Setup(m => m.Provider).Returns(_toDoNotes.Provider);
            mockSet.As<IQueryable<ToDoNote>>().Setup(m => m.Expression).Returns(_toDoNotes.Expression);
            mockSet.As<IQueryable<ToDoNote>>().Setup(m => m.ElementType).Returns(_toDoNotes.ElementType);
            mockSet.As<IQueryable<ToDoNote>>().Setup(m => m.GetEnumerator()).Returns(_toDoNotes.GetEnumerator());

            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.ToDoNotes).Returns(mockSet.Object);

            _toDoNoteService = new ToDoNoteService(_mockContext.Object);
        }

        [Test]
        public void Create_ValidViewModel_AddedToContext()
        {
            // Arrange
            var viewModel = new ToDoNoteViewModel
            {
                Title = _fixture.Create<string>(),
                Description = _fixture.Create<string>()
            };

            // Act
            _toDoNoteService.Create(viewModel, ApplicationUserId);

            // Assert
            _mockContext.Verify(c => 
                c.ToDoNotes.Add(It.Is<ToDoNote>(n => 
                    n.Title == viewModel.Title && 
                    n.Description == viewModel.Description &&
                    n.ApplicationUserId == ApplicationUserId)), Times.Once);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void Create_NullViewModel_ArgumentNullException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => _toDoNoteService.Create(null!, ApplicationUserId));
        }

        [Test]
        public void Read_ValidParameters_ViewModelReturned()
        {
            // Arrange
            var toDoNote = _toDoNotes.First();

            // Act
            var viewModel = _toDoNoteService.Read(toDoNote.Id, ApplicationUserId);

            // Assert
            viewModel.Id.Should().Be(toDoNote.Id);
        }

        [Test]
        public void Read_IncorrectIds_ExceptionThrown()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<EntityNotFoundException<ToDoNote>>(() => _toDoNoteService.Read(_fixture.Create<int>(), _fixture.Create<string>()));
        }

        [Test]
        public void List_ValidParameters_ViewModelIsReturned()
        {
            // Arrange

            // Act
            var viewModel = _toDoNoteService.List(ApplicationUserId, 1);

            // Assert
            viewModel.ToDoNotes.Should().NotBeNull();
            viewModel.ToDoNotes.Count.Should().Be(5);

            var toDoNotes = _toDoNotes.ToList();

            for (var i = 0; i < 5; i++)
                viewModel.ToDoNotes[i].Id.Should().Be(toDoNotes[i].Id);
        }

        [Test]
        public void List_InvalidParameters_ViewModelIsReturnedWithEmptyList()
        {
            // Arrange

            // Act
            var viewModel = _toDoNoteService.List(_fixture.Create<string>(), 1);

            // Assert
            viewModel.ToDoNotes.Should().NotBeNull();
            viewModel.ToDoNotes.Should().BeEmpty();
        }

        [Test]
        public void List_ValidPagination_ViewModelIsReturned()
        {
            // Arrange

            // Act
            var viewModel = _toDoNoteService.List(ApplicationUserId, 2);

            // Assert
            viewModel.ToDoNotes.Should().NotBeNull();
            viewModel.ToDoNotes.Count.Should().Be(5);

            var toDoNotes = _toDoNotes.ToList();

            for (var i = 0; i < 5; i++)
                viewModel.ToDoNotes[i].Id.Should().Be(toDoNotes[i+5].Id);
        }

        [Test]
        public void List_InvalidPaginationLow_ViewModelIsReturned()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _toDoNoteService.List(ApplicationUserId, -1));
        }

        [Test]
        public void List_InvalidPaginationHigh_ViewModelIsReturned()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _toDoNoteService.List(ApplicationUserId, 5));
        }

        [Test]
        public void Update_ValidParameters_ToDoNoteIsUpdated()
        {
            // Arrange
            var toDoNote = _toDoNotes.First();

            var viewModel = new ToDoNoteViewModel
            {
                Id = toDoNote.Id,
                Done = true
            };

            // Act
            _toDoNoteService.Update(viewModel, ApplicationUserId);

            // Assert
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);

            toDoNote = _toDoNotes.First();
            toDoNote.Done.Should().BeTrue();
        }

        [Test]
        public void Update_NullViewModel_ArgumentNullException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => _toDoNoteService.Update(null!, ApplicationUserId));
        }

        [Test]
        public void Update_IncorrectUserId_ExceptionThrown()
        {
            // Arrange
            var toDoNote = _toDoNotes.First();

            var viewModel = new ToDoNoteViewModel
            {
                Id = toDoNote.Id,
                Done = true
            };

            // Act

            // Assert
            Assert.Throws<EntityNotFoundException<ToDoNote>>(() => _toDoNoteService.Update(viewModel, _fixture.Create<string>()));
        }

        [Test]
        public void Delete_ValidParameters_ToDoNoteIsDeleted()
        {
            // Arrange
            var toDoNote = _toDoNotes.First();

            // Act
            _toDoNoteService.Delete(toDoNote.Id, ApplicationUserId);

            // Assert
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);

            toDoNote = _toDoNotes.First();
            toDoNote.Deleted.Should().BeTrue();
        }

        [Test]
        public void Delete_IncorrectUserId_ExceptionThrown()
        {
            // Arrange
            var toDoNote = _toDoNotes.First();

            // Act

            // Assert
            Assert.Throws<EntityNotFoundException<ToDoNote>>(() => _toDoNoteService.Delete(toDoNote.Id, _fixture.Create<string>()));
        }
    }
}