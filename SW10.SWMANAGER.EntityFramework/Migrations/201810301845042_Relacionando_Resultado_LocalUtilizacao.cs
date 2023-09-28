namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Relacionando_Resultado_LocalUtilizacao : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Terceirizado", newName: "SisTerceirizado");
            AddColumn("dbo.LabResultado", "SisLocalUtilizacaoId", c => c.Long());
            AddColumn("dbo.LabResultado", "SisTerceirizadoId", c => c.Long());
            CreateIndex("dbo.LabResultado", "SisLocalUtilizacaoId");
            CreateIndex("dbo.LabResultado", "SisTerceirizadoId");
            AddForeignKey("dbo.LabResultado", "SisLocalUtilizacaoId", "dbo.SisUnidadeOrganizacional", "Id");
            AddForeignKey("dbo.LabResultado", "SisTerceirizadoId", "dbo.SisTerceirizado", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultado", "SisTerceirizadoId", "dbo.SisTerceirizado");
            DropForeignKey("dbo.LabResultado", "SisLocalUtilizacaoId", "dbo.SisUnidadeOrganizacional");
            DropIndex("dbo.LabResultado", new[] { "SisTerceirizadoId" });
            DropIndex("dbo.LabResultado", new[] { "SisLocalUtilizacaoId" });
            DropColumn("dbo.LabResultado", "SisTerceirizadoId");
            DropColumn("dbo.LabResultado", "SisLocalUtilizacaoId");
            RenameTable(name: "dbo.SisTerceirizado", newName: "Terceirizado");
        }
    }
}
