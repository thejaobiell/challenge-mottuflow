using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuFlowApi.Migrations
{
    /// <inheritdoc />
    public partial class Iniciando : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    id_funcionario = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    cargo = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcionario", x => x.id_funcionario);
                });

            migrationBuilder.CreateTable(
                name: "patio",
                columns: table => new
                {
                    id_patio = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    capacidade_maxima = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patio", x => x.id_patio);
                });

            migrationBuilder.CreateTable(
                name: "camera",
                columns: table => new
                {
                    id_camera = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    status_operacional = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    localizacao_fisica = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    id_patio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PatioIdPatio = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_camera", x => x.id_camera);
                    table.ForeignKey(
                        name: "FK_camera_patio_PatioIdPatio",
                        column: x => x.PatioIdPatio,
                        principalTable: "patio",
                        principalColumn: "id_patio");
                });

            migrationBuilder.CreateTable(
                name: "moto",
                columns: table => new
                {
                    id_moto = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    placa = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    modelo = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    fabricante = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_patio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    localizacao_atual = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PatioIdPatio = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moto", x => x.id_moto);
                    table.ForeignKey(
                        name: "FK_moto_patio_PatioIdPatio",
                        column: x => x.PatioIdPatio,
                        principalTable: "patio",
                        principalColumn: "id_patio");
                });

            migrationBuilder.CreateTable(
                name: "aruco_tag",
                columns: table => new
                {
                    id_tag = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    codigo = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    id_moto = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MotoIdMoto = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aruco_tag", x => x.id_tag);
                    table.ForeignKey(
                        name: "FK_aruco_tag_moto_MotoIdMoto",
                        column: x => x.MotoIdMoto,
                        principalTable: "moto",
                        principalColumn: "id_moto");
                });

            migrationBuilder.CreateTable(
                name: "localidade",
                columns: table => new
                {
                    id_localidade = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    data_hora = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ponto_referencia = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    id_moto = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_patio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_camera = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MotoIdMoto = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PatioIdPatio = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CameraIdCamera = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localidade", x => x.id_localidade);
                    table.ForeignKey(
                        name: "FK_localidade_camera_CameraIdCamera",
                        column: x => x.CameraIdCamera,
                        principalTable: "camera",
                        principalColumn: "id_camera");
                    table.ForeignKey(
                        name: "FK_localidade_moto_MotoIdMoto",
                        column: x => x.MotoIdMoto,
                        principalTable: "moto",
                        principalColumn: "id_moto");
                    table.ForeignKey(
                        name: "FK_localidade_patio_PatioIdPatio",
                        column: x => x.PatioIdPatio,
                        principalTable: "patio",
                        principalColumn: "id_patio");
                });

            migrationBuilder.CreateTable(
                name: "registro_status",
                columns: table => new
                {
                    id_status = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    tipo_status = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    descricao = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    data_status = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    id_moto = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_funcionario = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MotoIdMoto = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FuncionarioIdFuncionario = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registro_status", x => x.id_status);
                    table.ForeignKey(
                        name: "FK_registro_status_funcionario_FuncionarioIdFuncionario",
                        column: x => x.FuncionarioIdFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id_funcionario");
                    table.ForeignKey(
                        name: "FK_registro_status_moto_MotoIdMoto",
                        column: x => x.MotoIdMoto,
                        principalTable: "moto",
                        principalColumn: "id_moto");
                });

            migrationBuilder.CreateIndex(
                name: "IX_aruco_tag_MotoIdMoto",
                table: "aruco_tag",
                column: "MotoIdMoto");

            migrationBuilder.CreateIndex(
                name: "IX_camera_PatioIdPatio",
                table: "camera",
                column: "PatioIdPatio");

            migrationBuilder.CreateIndex(
                name: "IX_localidade_CameraIdCamera",
                table: "localidade",
                column: "CameraIdCamera");

            migrationBuilder.CreateIndex(
                name: "IX_localidade_MotoIdMoto",
                table: "localidade",
                column: "MotoIdMoto");

            migrationBuilder.CreateIndex(
                name: "IX_localidade_PatioIdPatio",
                table: "localidade",
                column: "PatioIdPatio");

            migrationBuilder.CreateIndex(
                name: "IX_moto_PatioIdPatio",
                table: "moto",
                column: "PatioIdPatio");

            migrationBuilder.CreateIndex(
                name: "IX_registro_status_FuncionarioIdFuncionario",
                table: "registro_status",
                column: "FuncionarioIdFuncionario");

            migrationBuilder.CreateIndex(
                name: "IX_registro_status_MotoIdMoto",
                table: "registro_status",
                column: "MotoIdMoto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aruco_tag");

            migrationBuilder.DropTable(
                name: "localidade");

            migrationBuilder.DropTable(
                name: "registro_status");

            migrationBuilder.DropTable(
                name: "camera");

            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropTable(
                name: "moto");

            migrationBuilder.DropTable(
                name: "patio");
        }
    }
}
