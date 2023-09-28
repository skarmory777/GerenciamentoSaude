namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoColunaProtocoloAtendimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "ProtocoloAtendimentoId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "ProtocoloAtendimentoId");
            AddForeignKey("dbo.AteAtendimento", "ProtocoloAtendimentoId", "dbo.AteProtocoloAtendimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "ProtocoloAtendimentoId", "dbo.AteProtocoloAtendimento");
            DropIndex("dbo.AteAtendimento", new[] { "ProtocoloAtendimentoId" });
            DropColumn("dbo.AteAtendimento", "ProtocoloAtendimentoId");
        }
    }
}
