namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alterando_Relacionamento_Bairro_Cidade_Estado_para_nullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisCidade", "SisEstadoId", "dbo.SisEstado");
            DropForeignKey("dbo.SisBairro", "SisCidadeId", "dbo.SisCidade");
            DropIndex("dbo.SisCidade", new[] { "SisEstadoId" });
            DropIndex("dbo.SisBairro", new[] { "SisCidadeId" });
            AlterColumn("dbo.SisCidade", "SisEstadoId", c => c.Long());
            AlterColumn("dbo.SisBairro", "SisCidadeId", c => c.Long());
            CreateIndex("dbo.SisCidade", "SisEstadoId");
            CreateIndex("dbo.SisBairro", "SisCidadeId");
            AddForeignKey("dbo.SisCidade", "SisEstadoId", "dbo.SisEstado", "Id");
            AddForeignKey("dbo.SisBairro", "SisCidadeId", "dbo.SisCidade", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisBairro", "SisCidadeId", "dbo.SisCidade");
            DropForeignKey("dbo.SisCidade", "SisEstadoId", "dbo.SisEstado");
            DropIndex("dbo.SisBairro", new[] { "SisCidadeId" });
            DropIndex("dbo.SisCidade", new[] { "SisEstadoId" });
            AlterColumn("dbo.SisBairro", "SisCidadeId", c => c.Long(nullable: false));
            AlterColumn("dbo.SisCidade", "SisEstadoId", c => c.Long(nullable: false));
            CreateIndex("dbo.SisBairro", "SisCidadeId");
            CreateIndex("dbo.SisCidade", "SisEstadoId");
            AddForeignKey("dbo.SisBairro", "SisCidadeId", "dbo.SisCidade", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SisCidade", "SisEstadoId", "dbo.SisEstado", "Id", cascadeDelete: true);
        }
    }
}
