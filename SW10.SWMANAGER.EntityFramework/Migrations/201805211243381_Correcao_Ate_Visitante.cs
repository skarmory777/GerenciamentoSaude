namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Correcao_Ate_Visitante : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteVisitante", "FornecedorId", c => c.Long());
            CreateIndex("dbo.AteVisitante", "FornecedorId");
            AddForeignKey("dbo.AteVisitante", "FornecedorId", "dbo.SisFornecedor", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteVisitante", "FornecedorId", "dbo.SisFornecedor");
            DropIndex("dbo.AteVisitante", new[] { "FornecedorId" });
            DropColumn("dbo.AteVisitante", "FornecedorId");
        }
    }
}
