namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Paciente_DataNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisPaciente", "NomeCompleto", c => c.String(maxLength: 100));
            AlterColumn("dbo.SisPaciente", "Nascimento", c => c.DateTime());
            AlterColumn("dbo.SisPaciente", "Cpf", c => c.String(maxLength: 14));
        }

        public override void Down()
        {
            AlterColumn("dbo.SisPaciente", "Cpf", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.SisPaciente", "Nascimento", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisPaciente", "NomeCompleto", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
