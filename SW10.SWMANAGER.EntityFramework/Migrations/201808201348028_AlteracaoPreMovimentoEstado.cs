namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoPreMovimentoEstado : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoqueMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstSolicitacaoItem", "EstadoSolicitacaoItemId", "dbo.EstoquePreMovimentoEstado");
            DropPrimaryKey("dbo.EstoquePreMovimentoEstado");
            AlterColumn("dbo.EstoquePreMovimentoEstado", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado", "Id", cascadeDelete: false);
            AddForeignKey("dbo.EstoqueMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado", "Id", cascadeDelete: false);
            AddForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstSolicitacaoItem", "EstadoSolicitacaoItemId", "dbo.EstoquePreMovimentoEstado", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstSolicitacaoItem", "EstadoSolicitacaoItemId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoqueMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoquePreMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropPrimaryKey("dbo.EstoquePreMovimentoEstado");
            AlterColumn("dbo.EstoquePreMovimentoEstado", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstSolicitacaoItem", "EstadoSolicitacaoItemId", "dbo.EstoquePreMovimentoEstado", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoqueMovimentoItem", "MovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoItemEstadoId", "dbo.EstoquePreMovimentoEstado", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoquePreMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado", "Id", cascadeDelete: true);
        }
    }
}
