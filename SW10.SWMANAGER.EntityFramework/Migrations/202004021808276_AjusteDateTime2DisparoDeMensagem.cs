namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteDateTime2DisparoDeMensagem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisDisparoDeMensagem", "DataProgramada", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.SisDisparoDeMensagem", "DataInicioDisparo", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.SisDisparoDeMensagem", "DataFinalDisparo", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataProgramada", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataInicioDisparo", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataFinalDisparo", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataRecebimento", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataRecebimento", c => c.DateTime());
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataFinalDisparo", c => c.DateTime());
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataInicioDisparo", c => c.DateTime());
            AlterColumn("dbo.SisDisparoDeMensagemItem", "DataProgramada", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisDisparoDeMensagem", "DataFinalDisparo", c => c.DateTime());
            AlterColumn("dbo.SisDisparoDeMensagem", "DataInicioDisparo", c => c.DateTime());
            AlterColumn("dbo.SisDisparoDeMensagem", "DataProgramada", c => c.DateTime(nullable: false));
        }
    }
}
