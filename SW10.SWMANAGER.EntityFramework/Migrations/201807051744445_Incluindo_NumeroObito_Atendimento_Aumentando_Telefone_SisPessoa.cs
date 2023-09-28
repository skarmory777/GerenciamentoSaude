namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Incluindo_NumeroObito_Atendimento_Aumentando_Telefone_SisPessoa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "NumeroObito", c => c.String());
            AddColumn("dbo.AteAtendimento", "DataUltimoPagamento", c => c.DateTime());
            AlterColumn("dbo.SisMedico", "Telefone1", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisMedico", "Telefone2", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisMedico", "Telefone3", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisMedico", "Telefone4", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPessoa", "Telefone1", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPessoa", "Telefone2", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPessoa", "Telefone3", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPessoa", "Telefone4", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPaciente", "Telefone1", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPaciente", "Telefone2", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPaciente", "Telefone3", c => c.String(maxLength: 80));
            AlterColumn("dbo.SisPaciente", "Telefone4", c => c.String(maxLength: 80));
        }

        public override void Down()
        {
            AlterColumn("dbo.SisPaciente", "Telefone4", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPaciente", "Telefone3", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPaciente", "Telefone2", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPaciente", "Telefone1", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPessoa", "Telefone4", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPessoa", "Telefone3", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPessoa", "Telefone2", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisPessoa", "Telefone1", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisMedico", "Telefone4", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisMedico", "Telefone3", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisMedico", "Telefone2", c => c.String(maxLength: 20));
            AlterColumn("dbo.SisMedico", "Telefone1", c => c.String(maxLength: 20));
            DropColumn("dbo.AteAtendimento", "DataUltimoPagamento");
            DropColumn("dbo.AteAtendimento", "NumeroObito");
        }
    }
}
