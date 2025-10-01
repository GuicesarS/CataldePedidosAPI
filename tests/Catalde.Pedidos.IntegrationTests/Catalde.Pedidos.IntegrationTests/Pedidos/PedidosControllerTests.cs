using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Catalde.Pedidos.IntegrationTests.Pedidos;

public class PedidosControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PedidosControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient() => _factory.CreateClient();

    private async Task<string> GetToken(HttpClient client)
    {
        var login = new { username = "admin", password = "123456" };
        var response = await client.PostAsJsonAsync("api/auth/login", login);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result!.Token;
    }

    [Fact]
    public async Task GetAll_WhenCalled_ReturnsOk()
    {
        var client = CreateClient();
        var token = await GetToken(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("api/Pedidos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pedidos = await response.Content.ReadFromJsonAsync<List<PedidoResponse>>();
        pedidos.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WhenPedidoExists_ReturnsOk()
    {
        var client = CreateClient();
        var token = await GetToken(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var novoPedido = new { NumeroPedido = new Random().Next(1000, 9999) };
        var criarPedido = await client.PostAsJsonAsync("api/Pedidos", novoPedido);
        criarPedido.EnsureSuccessStatusCode();

        var pedidoCriado = await criarPedido.Content.ReadFromJsonAsync<PedidoResponse>();

        var response = await client.GetAsync($"/api/Pedidos/{pedidoCriado!.IdPedido}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pedido = await response.Content.ReadFromJsonAsync<PedidoResponse>();
        pedido.Should().NotBeNull();
        pedido.IdPedido.Should().Be(pedidoCriado.IdPedido);
    }

    [Fact]
    public async Task GetById_WhenPedidoDoesNotExist_ReturnsNotFound()
    {
        var client = CreateClient();
        var token = await GetToken(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/Pedidos/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CriarPedido_WhenCalled_ReturnsCreatedAndPedido()
    {
        var client = CreateClient();
        var token = await GetToken(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var novoPedido = new { NumeroPedido = new Random().Next(1000, 9999) };
        var response = await client.PostAsJsonAsync("api/Pedidos", novoPedido);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var pedido = await response.Content.ReadFromJsonAsync<PedidoResponse>();
        pedido.Should().NotBeNull();
        pedido.IdPedido.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AdicionarOcorrencia_WhenCalled_AddsOcorrencia()
    {
        var client = CreateClient();
        var token = await GetToken(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var novoPedido = new { NumeroPedido = new Random().Next(1000, 9999) };
        var criarPedido = await client.PostAsJsonAsync("/api/Pedidos", novoPedido);

        criarPedido.EnsureSuccessStatusCode();

        var pedidoCriado = await criarPedido.Content.ReadFromJsonAsync<PedidoResponse>();

        var ocorrencia = new { TipoOcorrencia = 0 };
        var response = await client.PostAsJsonAsync($"/api/Pedidos/{pedidoCriado!.IdPedido}/ocorrencia", ocorrencia);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

       
        var getPedido = await client.GetAsync($"/api/Pedidos/{pedidoCriado.IdPedido}");
        var pedidoAtualizado = await getPedido.Content.ReadFromJsonAsync<PedidoResponse>();

        pedidoAtualizado.Should().NotBeNull();
        pedidoAtualizado!.Ocorrencias.Should().ContainSingle(o => o.TipoOcorrencia == 0);
    }

    [Fact]
    public async Task ExcluirOcorrencia_WhenInexistente_ReturnsNotFound()
    {
        var client = CreateClient();
        var token = await GetToken(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync("/api/Pedidos/1/ocorrencia/9999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    private class PedidoResponse
    {
        public int IdPedido { get; set; }
        public List<OcorrenciaResponse> Ocorrencias { get; set; } = new();
    }

    private class OcorrenciaResponse
    {
        public int TipoOcorrencia { get; set; }
    }
}
