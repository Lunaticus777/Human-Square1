using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_Evolution.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableBiens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Biens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Ville = table.Column<string>(type: "TEXT", nullable: false),
                    Quartier = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Prix = table.Column<decimal>(type: "TEXT", nullable: false),
                    Surface = table.Column<decimal>(type: "TEXT", nullable: true),
                    NbPieces = table.Column<int>(type: "INTEGER", nullable: true),
                    NbSdb = table.Column<int>(type: "INTEGER", nullable: true),
                    Etage = table.Column<string>(type: "TEXT", nullable: false),
                    AnneeConstruction = table.Column<int>(type: "INTEGER", nullable: true),
                    Statut = table.Column<string>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePrincipale = table.Column<string>(type: "TEXT", nullable: false),
                    Images = table.Column<string>(type: "TEXT", nullable: false),
                    Caracteristiques = table.Column<string>(type: "TEXT", nullable: false),
                    Etat = table.Column<string>(type: "TEXT", nullable: false),
                    Visible = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biens");
        }
    }
}
