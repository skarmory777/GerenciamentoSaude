namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Faturamento_Alteracoes_Add_Qtde_FatPacote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatPacote", "Qtde", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FatPacote", "Qtde");
        }
    }
}
