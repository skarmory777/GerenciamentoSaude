namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NegritoEItalicoGrupoESubClasse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Grupo", "IsNegrito", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Grupo", "IsItalico", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_GrupoSubClasse", "IsNegrito", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_GrupoSubClasse", "IsItalico", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Est_GrupoSubClasse", "IsItalico");
            DropColumn("dbo.Est_GrupoSubClasse", "IsNegrito");
            DropColumn("dbo.Est_Grupo", "IsItalico");
            DropColumn("dbo.Est_Grupo", "IsNegrito");
        }
    }
}
