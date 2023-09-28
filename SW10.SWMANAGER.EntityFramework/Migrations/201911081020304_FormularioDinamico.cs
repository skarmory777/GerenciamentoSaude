namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormularioDinamico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "ColConfigReservadoId", c => c.Long());
            AddColumn("dbo.SisFormResposta", "IsPreenchido", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisFormResposta", "IsPreenchido");
            DropColumn("dbo.SisFormColConfig", "ColConfigReservadoId");
        }
    }
}
