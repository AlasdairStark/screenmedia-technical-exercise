using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Screenmedia.ToDo.Web;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using AngleSharp.Html.Dom;
using System.Collections.Generic;

namespace Screenmedia.ToDo.Tests.Integration
{
    public class ToDoNotesTest :
        IClassFixture<InMemoryDatabaseWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly InMemoryDatabaseWebApplicationFactory<Startup>
            _factory;

        public ToDoNotesTest(
            InMemoryDatabaseWebApplicationFactory<Startup> factory)
        {
            _factory = factory;

            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            })
            .CreateClient();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task List_ToDoNotesAreReturned()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/ToDoNotes/List");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await HtmlHelpers.GetDocumentAsync(response);
            var notes = content.QuerySelectorAll(".note");
            Assert.Equal(2, notes.Length);
        }

        [Fact]
        public async Task Create_ToDoNotesAreReturnedIncludingNewlyCreated()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/ToDoNotes/Create");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            // Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form"),
                (IHtmlButtonElement)content.QuerySelector("button"),
                new Dictionary<string, string>
                {
                    ["Title"] = "Test Note 3",
                    ["Description"] = "Test Description 3"
                });


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            content = await HtmlHelpers.GetDocumentAsync(response);
            var notes = content.QuerySelectorAll(".note");
            Assert.Equal(3, notes.Length);
        }

        [Fact]
        public async Task Update_ToDoNotesAreReturnedWithEdit()
        {
            // Arrange
            var expectedTitle = "Test Note 1 Edit";
            var expectedDescription = "Test Description 1 Edit";

            // Load the list page
            var listPage = await _client.GetAsync("/ToDoNotes/List");
            var content = await HtmlHelpers.GetDocumentAsync(listPage);

            // Load the edit page
            var updatePage = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form"),
                (IHtmlInputElement)content.QuerySelector("input[type='submit']"),
                new Dictionary<string, string>
                {
                    ["Id"] = ((IHtmlInputElement)content.QuerySelector("input[name='Id']")).Value,
                });

            content = await HtmlHelpers.GetDocumentAsync(updatePage);

            // Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form"),
                (IHtmlButtonElement)content.QuerySelector("button"),
                new Dictionary<string, string>
                {
                    ["Id"] = ((IHtmlInputElement)content.QuerySelector("input[name='Id']")).Value,
                    ["Title"] = expectedTitle,
                    ["Description"] = expectedDescription
                });


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            content = await HtmlHelpers.GetDocumentAsync(response);
            var title = (IHtmlDivElement)content.QuerySelector(".title");
            var description = (IHtmlDivElement)content.QuerySelector(".description");
            
            Assert.Contains(expectedTitle, title.InnerHtml);
            Assert.Contains(expectedDescription, description.InnerHtml);
        }

        [Fact]
        public async Task Delete_ToDoNotesAreReturnedExcludingDeleted()
        {
            // Arrange
            // Load the list page
            var listPage = await _client.GetAsync("/ToDoNotes/List");
            var content = await HtmlHelpers.GetDocumentAsync(listPage);

            /// Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelectorAll("form")[1],
                (IHtmlInputElement)content.QuerySelectorAll("input[type='submit']")[1],
                new Dictionary<string, string>
                {
                    ["Id"] = ((IHtmlInputElement)content.QuerySelector("input[name='Id']")).Value,
                });


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            content = await HtmlHelpers.GetDocumentAsync(response);
            var notes = content.QuerySelectorAll(".note");
            Assert.Equal(1, notes.Length);
        }
    }
}