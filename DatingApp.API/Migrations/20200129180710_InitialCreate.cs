using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class InitialCreate : Migration // Migration klassen kommer fra entity framwork core
    {
        // i Up metoden er det som skjer når vi applier denne migrationen
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Values", //får dette navnet ettersom det var det vi kalte vår Dbset i DataContext.cs 
                columns: table => new
                {
                    // MERK entity gjenkjenner navnet vi lagde i Value.cs: Id som en ID for tabellen og gjør denne constraint og som primary key automatisk
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true), 
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                });
        }

        // down gjør det motsatte og dropper tabellen med navn Values i Db
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Values");
        }
    }
}
