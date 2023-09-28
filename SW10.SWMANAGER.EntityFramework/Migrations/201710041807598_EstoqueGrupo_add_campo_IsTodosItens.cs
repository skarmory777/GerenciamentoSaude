namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EstoqueGrupo_add_campo_IsTodosItens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_EstoqueGrupo", "IsTodosItens", c => c.Boolean());
        }

        public override void Down()
        {
            DropColumn("dbo.Est_EstoqueGrupo", "IsTodosItens");
        }
    }
}
