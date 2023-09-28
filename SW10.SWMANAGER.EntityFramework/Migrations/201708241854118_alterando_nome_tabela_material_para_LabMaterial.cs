namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class alterando_nome_tabela_material_para_LabMaterial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Material", newName: "LabMaterial");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.LabMaterial", newName: "Material");
        }
    }
}
