namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposParametrizacaoHoraSolicitacaoExameEPrescricao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisParametrizacoes", "SolicitacaoExameHoraOutroDia", c => c.Time(precision: 7));
            AddColumn("dbo.SisParametrizacoes", "PrescricaoMedicaHoraOutroDia", c => c.Time(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisParametrizacoes", "PrescricaoMedicaHoraOutroDia");
            DropColumn("dbo.SisParametrizacoes", "SolicitacaoExameHoraOutroDia");
        }
    }
}
