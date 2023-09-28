namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Add_inventario_column1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento");
            DropForeignKey("dbo.EstoqueMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento");
            DropPrimaryKey("dbo.EstTipoMovimento");
            AlterColumn("dbo.EstTipoMovimento", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.EstTipoMovimento", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoqueMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento");
            DropForeignKey("dbo.EstoquePreMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento");
            DropPrimaryKey("dbo.EstTipoMovimento");
            AlterColumn("dbo.EstTipoMovimento", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.EstTipoMovimento", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento", "Id");
        }
    }
}
