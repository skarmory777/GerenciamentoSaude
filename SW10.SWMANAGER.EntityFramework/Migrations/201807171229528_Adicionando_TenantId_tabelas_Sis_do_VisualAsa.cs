namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicionando_TenantId_tabelas_Sis_do_VisualAsa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sis_Ambulatorio", "TenantId", c => c.Int());
            AddColumn("dbo.Sis_Atendimento", "TenantId", c => c.Int());
            AddColumn("dbo.Sis_ContaMedica", "TenantId", c => c.Int());
            AddColumn("dbo.Sis_Internacao", "TenantId", c => c.Int());
            AddColumn("dbo.Sis_Paciente", "TenantId", c => c.Int());
            AddColumn("dbo.Sis_Pessoa", "TenantId", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.Sis_Pessoa", "TenantId");
            DropColumn("dbo.Sis_Paciente", "TenantId");
            DropColumn("dbo.Sis_Internacao", "TenantId");
            DropColumn("dbo.Sis_ContaMedica", "TenantId");
            DropColumn("dbo.Sis_Atendimento", "TenantId");
            DropColumn("dbo.Sis_Ambulatorio", "TenantId");
        }
    }
}
