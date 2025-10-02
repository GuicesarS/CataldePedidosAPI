using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalde.Pedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pedidos",
                columns: new[] { "IdPedido", "HorarioPedido", "IndEntregue", "NumeroPedido" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 1, 9, 0, 0, 0, DateTimeKind.Utc), true, 1001 },
                    { 2, new DateTime(2025, 10, 1, 10, 30, 0, 0, DateTimeKind.Utc), false, 1002 }
                });

            migrationBuilder.InsertData(
                table: "Ocorrencias",
                columns: new[] { "IdOcorrencia", "HoraOcorrencia", "IndFinalizadora", "PedidoId", "TipoOcorrencia" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 1, 9, 15, 0, 0, DateTimeKind.Utc), false, 1, 0 },
                    { 2, new DateTime(2025, 10, 1, 9, 20, 0, 0, DateTimeKind.Utc), true, 1, 1 },
                    { 3, new DateTime(2025, 10, 1, 10, 35, 0, 0, DateTimeKind.Utc), false, 2, 0 },
                    { 4, new DateTime(2025, 10, 1, 10, 44, 0, 0, DateTimeKind.Utc), true, 2, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ocorrencias",
                keyColumn: "IdOcorrencia",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ocorrencias",
                keyColumn: "IdOcorrencia",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ocorrencias",
                keyColumn: "IdOcorrencia",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ocorrencias",
                keyColumn: "IdOcorrencia",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "IdPedido",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "IdPedido",
                keyValue: 2);
        }
    }
}
