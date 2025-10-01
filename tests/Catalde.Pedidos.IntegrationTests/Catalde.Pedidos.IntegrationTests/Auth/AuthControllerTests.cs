using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Catalde.Pedidos.IntegrationTests.Auth;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_Sucesso()
    {
        var request = new
        {
            Username = "admin",
            Password = "123456"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_Erro()
    {
        var request = new
        {
            Username = "admin",
            Password = "222121"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Contain("Usuário ou senha inválidos");

       
    }

    private class TokenResponse
    {
        public string Token { get; set; } 
    }
}

