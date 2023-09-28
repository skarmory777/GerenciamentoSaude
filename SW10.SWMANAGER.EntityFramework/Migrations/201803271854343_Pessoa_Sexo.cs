namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Pessoa_Sexo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisPessoa", "SexoId", c => c.Long());
            AddColumn("dbo.SisMedico", "SexoId", c => c.Long());
            AddColumn("dbo.SisPaciente", "SexoId", c => c.Long());
            AlterColumn("dbo.SisConvenio", "NomeFantasia", c => c.String());
            AlterColumn("dbo.SisConvenio", "RazaoSocial", c => c.String());
            AlterColumn("dbo.SisConvenio", "Cnpj", c => c.String());
            CreateIndex("dbo.SisPessoa", "SexoId");
            CreateIndex("dbo.SisMedico", "SexoId");
            CreateIndex("dbo.SisPaciente", "SexoId");
            AddForeignKey("dbo.SisPessoa", "SexoId", "dbo.SisSexo", "Id");
            AddForeignKey("dbo.SisMedico", "SexoId", "dbo.SisSexo", "Id");
            AddForeignKey("dbo.SisPaciente", "SexoId", "dbo.SisSexo", "Id");
            DropColumn("dbo.SisPessoa", "Sexo");
            DropColumn("dbo.SisMedico", "Sexo");
            DropColumn("dbo.SisPaciente", "Sexo");
        }

        public override void Down()
        {
            AddColumn("dbo.SisPaciente", "Sexo", c => c.Int());
            AddColumn("dbo.SisMedico", "Sexo", c => c.Int());
            AddColumn("dbo.SisPessoa", "Sexo", c => c.Int());
            DropForeignKey("dbo.SisPaciente", "SexoId", "dbo.SisSexo");
            DropForeignKey("dbo.SisMedico", "SexoId", "dbo.SisSexo");
            DropForeignKey("dbo.SisPessoa", "SexoId", "dbo.SisSexo");
            DropIndex("dbo.SisPaciente", new[] { "SexoId" });
            DropIndex("dbo.SisMedico", new[] { "SexoId" });
            DropIndex("dbo.SisPessoa", new[] { "SexoId" });
            AlterColumn("dbo.SisConvenio", "Cnpj", c => c.String(nullable: false));
            AlterColumn("dbo.SisConvenio", "RazaoSocial", c => c.String(nullable: false));
            AlterColumn("dbo.SisConvenio", "NomeFantasia", c => c.String(nullable: false));
            DropColumn("dbo.SisPaciente", "SexoId");
            DropColumn("dbo.SisMedico", "SexoId");
            DropColumn("dbo.SisPessoa", "SexoId");
        }
    }
}
