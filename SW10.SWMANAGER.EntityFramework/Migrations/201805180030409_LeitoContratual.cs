namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LeitoContratual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "SisTipoAcomodacaoId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "SisTipoAcomodacaoId");
            AddForeignKey("dbo.AteAtendimento", "SisTipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "SisTipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropIndex("dbo.AteAtendimento", new[] { "SisTipoAcomodacaoId" });
            DropColumn("dbo.AteAtendimento", "SisTipoAcomodacaoId");
        }
    }
}
