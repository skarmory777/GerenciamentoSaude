namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeInclusaoEstadoMovimentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId");
            CreateIndex("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId");
            AddForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "PreMovimentoItemEstadoId" });
            DropIndex("dbo.EstoqueMovimentoItem", new[] { "MovimentoItemEstadoId" });
            DropColumn("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId");
            DropColumn("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId");
        }
    }
}
