namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatContaItem_Grupo_Correcoes_IsLaudo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatContaItem", "EspecialidadeAnestesistaId", "dbo.SisMedico");
            DropIndex("dbo.FatContaItem", new[] { "EspecialidadeAnestesistaId" });
            AddColumn("dbo.FatGrupo", "IsLaudo", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatSubGrupo", "IsLaudo", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsMedCredenciado", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "AnestesistaEspecialidadeId", c => c.Long());
            AddColumn("dbo.FatContaItem", "IsAnestCredenciado", c => c.Boolean(nullable: false));
            CreateIndex("dbo.FatContaItem", "AnestesistaEspecialidadeId");
            AddForeignKey("dbo.FatContaItem", "AnestesistaEspecialidadeId", "dbo.SisMedicoEspecialidade", "Id");
            DropColumn("dbo.FatContaItem", "IsMedCrendenciado");
            DropColumn("dbo.FatContaItem", "EspecialidadeAnestesistaId");
        }

        public override void Down()
        {
            AddColumn("dbo.FatContaItem", "EspecialidadeAnestesistaId", c => c.Long());
            AddColumn("dbo.FatContaItem", "IsMedCrendenciado", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.FatContaItem", "AnestesistaEspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropIndex("dbo.FatContaItem", new[] { "AnestesistaEspecialidadeId" });
            DropColumn("dbo.FatContaItem", "IsAnestCredenciado");
            DropColumn("dbo.FatContaItem", "AnestesistaEspecialidadeId");
            DropColumn("dbo.FatContaItem", "IsMedCredenciado");
            DropColumn("dbo.FatSubGrupo", "IsLaudo");
            DropColumn("dbo.FatGrupo", "IsLaudo");
            CreateIndex("dbo.FatContaItem", "EspecialidadeAnestesistaId");
            AddForeignKey("dbo.FatContaItem", "EspecialidadeAnestesistaId", "dbo.SisMedico", "Id");
        }
    }
}
