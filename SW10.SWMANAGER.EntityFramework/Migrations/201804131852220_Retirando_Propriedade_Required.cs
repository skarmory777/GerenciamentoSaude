namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Retirando_Propriedade_Required : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisEstado", "SisPaisId", "dbo.SisPais");
            DropIndex("dbo.SisEstado", new[] { "SisPaisId" });
            AddColumn("dbo.SisCidade", "IsCapital", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SisCep", "CEP", c => c.String(maxLength: 8));
            AlterColumn("dbo.SisCidade", "Nome", c => c.String(maxLength: 120));
            AlterColumn("dbo.SisEstado", "Nome", c => c.String(maxLength: 75));
            AlterColumn("dbo.SisEstado", "Uf", c => c.String(maxLength: 5));
            AlterColumn("dbo.SisEstado", "SisPaisId", c => c.Long());
            AlterColumn("dbo.SisPais", "Nome", c => c.String(maxLength: 60));
            AlterColumn("dbo.SisPais", "Sigla", c => c.String(maxLength: 10));
            AlterColumn("dbo.SisEmpresa", "RazaoSocial", c => c.String());
            AlterColumn("dbo.SisEmpresa", "NomeFantasia", c => c.String());
            AlterColumn("dbo.SisEmpresa", "Cnpj", c => c.String());
            AlterColumn("dbo.SisMedico", "NomeCompleto", c => c.String(maxLength: 100));
            AlterColumn("dbo.SisMedico", "Cpf", c => c.String(maxLength: 14));
            AlterColumn("dbo.PacientePeso", "DataPesagem", c => c.DateTime());
            AlterColumn("dbo.SisPrestador", "NomeCompleto", c => c.String(maxLength: 100));
            AlterColumn("dbo.SisPrestador", "Cpf", c => c.String(maxLength: 14));
            AlterColumn("dbo.FornecedorPessoaFisica", "NomeCompleto", c => c.String(maxLength: 100));
            AlterColumn("dbo.FornecedorPessoaFisica", "Cpf", c => c.String(maxLength: 14));
            AlterColumn("dbo.FornecedorPessoaJuridica", "RazaoSocial", c => c.String());
            AlterColumn("dbo.FornecedorPessoaJuridica", "NomeFantasia", c => c.String());
            AlterColumn("dbo.FornecedorPessoaJuridica", "Cnpj", c => c.String());
            AlterColumn("dbo.SisBairro", "Nome", c => c.String(maxLength: 120));
            AlterColumn("dbo.Feriado", "DiaMesAno", c => c.DateTime());
            AlterColumn("dbo.MailingTemplate", "Name", c => c.String());
            AlterColumn("dbo.MailingTemplate", "Titulo", c => c.String());
            AlterColumn("dbo.MailingTemplate", "EmailSaida", c => c.String());
            AlterColumn("dbo.MailingTemplate", "NomeSaida", c => c.String());
            AlterColumn("dbo.MailingTemplate", "ContentTemplate", c => c.String());
            AlterColumn("dbo.MailingTemplate", "CamposDisponiveis", c => c.String());
            AlterColumn("dbo.AssSolicitacaoExamePrioridade", "Descricao", c => c.String(maxLength: 30));
            CreateIndex("dbo.SisEstado", "SisPaisId");
            AddForeignKey("dbo.SisEstado", "SisPaisId", "dbo.SisPais", "Id");
            DropColumn("dbo.SisCidade", "Capital");
        }

        public override void Down()
        {
            AddColumn("dbo.SisCidade", "Capital", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.SisEstado", "SisPaisId", "dbo.SisPais");
            DropIndex("dbo.SisEstado", new[] { "SisPaisId" });
            AlterColumn("dbo.AssSolicitacaoExamePrioridade", "Descricao", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.MailingTemplate", "CamposDisponiveis", c => c.String(nullable: false));
            AlterColumn("dbo.MailingTemplate", "ContentTemplate", c => c.String(nullable: false));
            AlterColumn("dbo.MailingTemplate", "NomeSaida", c => c.String(nullable: false));
            AlterColumn("dbo.MailingTemplate", "EmailSaida", c => c.String(nullable: false));
            AlterColumn("dbo.MailingTemplate", "Titulo", c => c.String(nullable: false));
            AlterColumn("dbo.MailingTemplate", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Feriado", "DiaMesAno", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisBairro", "Nome", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.FornecedorPessoaJuridica", "Cnpj", c => c.String(nullable: false));
            AlterColumn("dbo.FornecedorPessoaJuridica", "NomeFantasia", c => c.String(nullable: false));
            AlterColumn("dbo.FornecedorPessoaJuridica", "RazaoSocial", c => c.String(nullable: false));
            AlterColumn("dbo.FornecedorPessoaFisica", "Cpf", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.FornecedorPessoaFisica", "NomeCompleto", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.SisPrestador", "Cpf", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.SisPrestador", "NomeCompleto", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.PacientePeso", "DataPesagem", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisMedico", "Cpf", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.SisMedico", "NomeCompleto", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.SisEmpresa", "Cnpj", c => c.String(nullable: false));
            AlterColumn("dbo.SisEmpresa", "NomeFantasia", c => c.String(nullable: false));
            AlterColumn("dbo.SisEmpresa", "RazaoSocial", c => c.String(nullable: false));
            AlterColumn("dbo.SisPais", "Sigla", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.SisPais", "Nome", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.SisEstado", "SisPaisId", c => c.Long(nullable: false));
            AlterColumn("dbo.SisEstado", "Uf", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.SisEstado", "Nome", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.SisCidade", "Nome", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.SisCep", "CEP", c => c.String(nullable: false, maxLength: 8));
            DropColumn("dbo.SisCidade", "IsCapital");
            CreateIndex("dbo.SisEstado", "SisPaisId");
            AddForeignKey("dbo.SisEstado", "SisPaisId", "dbo.SisPais", "Id", cascadeDelete: true);
        }
    }
}
