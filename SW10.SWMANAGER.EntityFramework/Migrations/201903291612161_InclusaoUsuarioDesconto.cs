namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoUsuarioDesconto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAgendamentoItemFaturamento", "UsuarioDescontoId", c => c.Long());
            CreateIndex("dbo.AteAgendamentoItemFaturamento", "UsuarioDescontoId");
            AddForeignKey("dbo.AteAgendamentoItemFaturamento", "UsuarioDescontoId", "dbo.AbpUsers", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "UsuarioDescontoId", "dbo.AbpUsers");
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "UsuarioDescontoId" });
            DropColumn("dbo.AteAgendamentoItemFaturamento", "UsuarioDescontoId");
        }
    }
}
