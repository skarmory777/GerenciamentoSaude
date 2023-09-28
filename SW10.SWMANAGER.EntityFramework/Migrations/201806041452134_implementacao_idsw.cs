namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class implementacao_idsw : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sis_Ambulatorio", "IDSW", c => c.Int());
            AddColumn("dbo.Sis_Atendimento", "IDSW", c => c.Int());
            AddColumn("dbo.Sis_ContaMedica", "IDSW", c => c.Int());
            AddColumn("dbo.Sis_Internacao", "IDSW", c => c.Int());
            AddColumn("dbo.Sis_Paciente", "IDSW", c => c.Int());
            AddColumn("dbo.Sis_Pessoa", "IDSW", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.Sis_Pessoa", "IDSW");
            DropColumn("dbo.Sis_Paciente", "IDSW");
            DropColumn("dbo.Sis_Internacao", "IDSW");
            DropColumn("dbo.Sis_ContaMedica", "IDSW");
            DropColumn("dbo.Sis_Atendimento", "IDSW");
            DropColumn("dbo.Sis_Ambulatorio", "IDSW");
        }
    }
}
