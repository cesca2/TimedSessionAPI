
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;


namespace SessionAPI.IntegrationTests;

public class SessionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SessionsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory(DisplayName = "Test API endpoint returns success")]
    [InlineData("api/Sessions")]
    public async Task Get_EndpointsReturnSucces(string url)
    {

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299

    }

    [Fact(DisplayName = "Get by invalid Id returns not found")]
    public async Task Get_Sessions_With_Pagination_Returns_CorrectSessions()
    {

        // 🔧 Initialize the database at startup
        using (var scope = _factory.Services.CreateScope())
        {

            var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();

            using var connection = factory.CreateConnection();
            connection.Open();

            DbInitializer.Initialize(connection, reInitialize: true);
        }

        // Arrange
        var url = "api/Sessions?LastDate=2026-04-03";

        var exactItem = new Session(
        type: "C#",
        date: "03/04/2026",
        start: "11:00",
        end: "12:30")
        ;

        var unexpectedItem = new Session(
        type: "C#",
        date: "03/02/2026",
        start: "11:00",
        end: "12:30")
        ;

        var expectedItem = new Session(
        type: "C#",
        date: "03/04/2026",
        start: "11:00",
        end: "12:30")
        ;

        await _client.PostAsJsonAsync("api/Sessions", exactItem);
        await _client.PostAsJsonAsync("api/Sessions", unexpectedItem);
        await _client.PostAsJsonAsync("api/Sessions", expectedItem);


        // Act
        var items = await _client.GetFromJsonAsync<List<Session>>(url);

        // Assert 
        Assert.DoesNotContain(items, item => item.Id == unexpectedItem.Id);
        Assert.Contains(items, item => item.Id == expectedItem.Id);
        Assert.Contains(items, item => item.Id == exactItem.Id);


    }

    [Theory(DisplayName = "Get by invalid Id returns not found")]
    [InlineData("api/Sessions/7ab50dd5-c918-45ef-b1f2-99c24d6a7f24")]
    public async Task Get_By_Invalid_Id_Returns_NotFound(string url)
    {
        // Act
        var response = await _factory.CreateClient().GetAsync(url);

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory(DisplayName = "Test CRUD pipeline")]
    [InlineData("/api/Sessions")]

    public async Task Can_Manage_Sessions_SuccessiveCRUD(string sessionsEndpoint)
    {

        // 🔧 Initialize the database at startup
        using (var scope = _factory.Services.CreateScope())
        {

            var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();

            using var connection = factory.CreateConnection();
            connection.Open();

            DbInitializer.Initialize(connection, reInitialize: true);
        }

        // Act - Get all the items

        var response = await _client.GetAsync(sessionsEndpoint);

        var itemsInitial = await _client.GetFromJsonAsync<List<Session>>(sessionsEndpoint);

        // Arrange
        var initialCount = itemsInitial.Count;

        // Assert 0 Ensure no items initially
        Assert.Equal(0, initialCount);

        // Arrange

        var newItem = new Session(
        type: "C#",
        date: "27/03/2026",
        start: "11:00",
        end: "12:30")
        ;

        // Act - Add a new item
        using var createdResponse = await _client.PostAsJsonAsync(sessionsEndpoint, newItem);

        // Assert - An item was created
        Assert.Equal(HttpStatusCode.Created, createdResponse.StatusCode);

        // arrange 
        var jsonString = await createdResponse.Content.ReadAsStringAsync();
        var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

        string createdId = (string)jsonDict["id"];

        Assert.Equal(newItem.Id.ToString(), createdId);

        // Act - Get the item
        var item = await _client.GetFromJsonAsync<Session>($"{sessionsEndpoint}/{createdId}");

        // Assert - Verify the item was created correctly
        Assert.NotNull(item);
        Assert.Equal(item.Id.ToString(), createdId);
        Assert.Equal(item.Type, newItem.Type);
        Assert.Equal(item.End, newItem.End);
        Assert.Equal(item.Start, newItem.Start);
        Assert.Equal(item.Type, newItem.Type);
        Assert.Equal(item.Date, newItem.Date);


        // Act - Get all the items
        var items = await _client.GetFromJsonAsync<List<Session>>(sessionsEndpoint);

        // Assert - An item is now present
        Assert.NotNull(items);
        Assert.Equal(initialCount + 1, items.Count);

        // Arrange - Create updated Item
        var updatedItem = new Session(
        type: "Python",
        date: "28/03/2026",
        start: "11:30",
        end: "13:00")
        { Id = Guid.Parse(createdId) }
        ;
        // Act - Update the item
        using var updatedResponse = await _client.PutAsJsonAsync($"{sessionsEndpoint}/{createdId}", updatedItem);

        // Assert - Item has been updated 
        Assert.Equal(HttpStatusCode.Created, updatedResponse.StatusCode);

        var updatedCheck = await _client.GetFromJsonAsync<Session>($"{sessionsEndpoint}/{createdId}");

        // Assert - Verify the item was updated correctly
        Assert.NotNull(updatedCheck);
        Assert.Equal(updatedCheck.Id, updatedItem.Id);
        Assert.Equal(updatedItem.End, updatedCheck.End);
        Assert.Equal(updatedItem.Start, updatedCheck.Start);
        Assert.Equal(updatedItem.Type, updatedCheck.Type);
        Assert.Equal(updatedItem.Date, updatedCheck.Date);


        // Act - Delete the item
        using var deletedResponse = await _client.DeleteAsync($"{sessionsEndpoint}/{createdId}");

        // Assert - The item no longer exists
        deletedResponse.EnsureSuccessStatusCode();

        items = await _client.GetFromJsonAsync<List<Session>>(sessionsEndpoint);

        Assert.Equal(initialCount, items.Count);
        Assert.DoesNotContain(items, item => item.Id == newItem.Id);

        // Act -- Check new item no longer exists
        using var getResponse = await _client.GetAsync($"{sessionsEndpoint}/{createdId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

    }


}