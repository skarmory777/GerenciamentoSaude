namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtendimentoStatus_CorDeFundoECorDeTexto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimentoStatus", "CorFundo", c => c.String());
            AddColumn("dbo.AteAtendimentoStatus", "CorTexto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteAtendimentoStatus", "CorTexto");
            DropColumn("dbo.AteAtendimentoStatus", "CorFundo");
        }
    }
}
