namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NegritoEItalicoItemDePrescricaoGrupoEProdutoESubGrupo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Produto", "IsNegrito", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Produto", "IsItalico", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_GrupoClasse", "IsNegrito", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_GrupoClasse", "IsItalico", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItem", "IsNegrito", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItem", "IsItalico", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItem", "IsItalico");
            DropColumn("dbo.AssPrescricaoItem", "IsNegrito");
            DropColumn("dbo.Est_GrupoClasse", "IsItalico");
            DropColumn("dbo.Est_GrupoClasse", "IsNegrito");
            DropColumn("dbo.Est_Produto", "IsItalico");
            DropColumn("dbo.Est_Produto", "IsNegrito");
        }
    }
}
