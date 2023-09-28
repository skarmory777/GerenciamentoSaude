namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inserindo_campos_WebService_Convenio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "VerificaElegibilidadeHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "ComunicacaoBeneficiarioHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "CancelaGuiaHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitacaoProcedimentoHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitastatusAutorizacaoHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "LoteAnexoHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "LoteGuiasHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitaStatusProtocoloHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitacaoDemonstrativoRetornoHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "RecursoGlosaHomologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "VerificaElegibilidade", c => c.String());
            AddColumn("dbo.SisConvenio", "ComunicacaoBeneficiario", c => c.String());
            AddColumn("dbo.SisConvenio", "CancelaGuia", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitacaoProcedimento", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitastatusAutorizacao", c => c.String());
            AddColumn("dbo.SisConvenio", "LoteAnexo", c => c.String());
            AddColumn("dbo.SisConvenio", "LoteGuias", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitaStatusProtocolo", c => c.String());
            AddColumn("dbo.SisConvenio", "SolicitacaoDemonstrativoRetorno", c => c.String());
            AddColumn("dbo.SisConvenio", "RecursoGlosa", c => c.String());
            AddColumn("dbo.SisConvenio", "WebService", c => c.String());
            AddColumn("dbo.SisConvenio", "Usuario", c => c.String());
            AddColumn("dbo.SisConvenio", "Senha", c => c.String());
            AddColumn("dbo.SisConvenio", "Homologacao", c => c.String());
            AddColumn("dbo.SisConvenio", "Certificado", c => c.String());
            AddColumn("dbo.SisConvenio", "CaCerfts", c => c.String());
            AddColumn("dbo.SisConvenio", "SenhaCertificado", c => c.String());
            AddColumn("dbo.SisConvenio", "SenhaCacerts", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisConvenio", "SenhaCacerts");
            DropColumn("dbo.SisConvenio", "SenhaCertificado");
            DropColumn("dbo.SisConvenio", "CaCerfts");
            DropColumn("dbo.SisConvenio", "Certificado");
            DropColumn("dbo.SisConvenio", "Homologacao");
            DropColumn("dbo.SisConvenio", "Senha");
            DropColumn("dbo.SisConvenio", "Usuario");
            DropColumn("dbo.SisConvenio", "WebService");
            DropColumn("dbo.SisConvenio", "RecursoGlosa");
            DropColumn("dbo.SisConvenio", "SolicitacaoDemonstrativoRetorno");
            DropColumn("dbo.SisConvenio", "SolicitaStatusProtocolo");
            DropColumn("dbo.SisConvenio", "LoteGuias");
            DropColumn("dbo.SisConvenio", "LoteAnexo");
            DropColumn("dbo.SisConvenio", "SolicitastatusAutorizacao");
            DropColumn("dbo.SisConvenio", "SolicitacaoProcedimento");
            DropColumn("dbo.SisConvenio", "CancelaGuia");
            DropColumn("dbo.SisConvenio", "ComunicacaoBeneficiario");
            DropColumn("dbo.SisConvenio", "VerificaElegibilidade");
            DropColumn("dbo.SisConvenio", "RecursoGlosaHomologacao");
            DropColumn("dbo.SisConvenio", "SolicitacaoDemonstrativoRetornoHomologacao");
            DropColumn("dbo.SisConvenio", "SolicitaStatusProtocoloHomologacao");
            DropColumn("dbo.SisConvenio", "LoteGuiasHomologacao");
            DropColumn("dbo.SisConvenio", "LoteAnexoHomologacao");
            DropColumn("dbo.SisConvenio", "SolicitastatusAutorizacaoHomologacao");
            DropColumn("dbo.SisConvenio", "SolicitacaoProcedimentoHomologacao");
            DropColumn("dbo.SisConvenio", "CancelaGuiaHomologacao");
            DropColumn("dbo.SisConvenio", "ComunicacaoBeneficiarioHomologacao");
            DropColumn("dbo.SisConvenio", "VerificaElegibilidadeHomologacao");
        }
    }
}
