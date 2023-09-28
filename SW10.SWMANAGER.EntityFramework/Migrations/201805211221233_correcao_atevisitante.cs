namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class correcao_atevisitante : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteVisitante", "FornecedorId", "dbo.Fornecedor");
            DropIndex("dbo.AteVisitante", new[] { "FornecedorId" });
            //DropColumn("dbo.AteVisitante", "FornecedorId");
        }

        public override void Down()
        {
            //AddColumn("dbo.AteVisitante", "FornecedorId", c => c.Long());
            CreateIndex("dbo.AteVisitante", "FornecedorId");
            AddForeignKey("dbo.AteVisitante", "FornecedorId", "dbo.Fornecedor", "Id");
        }
    }
}
