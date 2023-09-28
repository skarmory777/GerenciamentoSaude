namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Criando_Relacao_LabItemResultado_x_LabEquipamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabItemResultado", "LabEquipamentoId", c => c.Long());
            CreateIndex("dbo.LabItemResultado", "LabEquipamentoId");
            AddForeignKey("dbo.LabItemResultado", "LabEquipamentoId", "dbo.LabEquipamento", "Id");
            DropColumn("dbo.LabItemResultado", "Equipamento");
        }

        public override void Down()
        {
            AddColumn("dbo.LabItemResultado", "Equipamento", c => c.String());
            DropForeignKey("dbo.LabItemResultado", "LabEquipamentoId", "dbo.LabEquipamento");
            DropIndex("dbo.LabItemResultado", new[] { "LabEquipamentoId" });
            DropColumn("dbo.LabItemResultado", "LabEquipamentoId");
        }
    }
}
