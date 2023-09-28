namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_ContaMedica_Auxiliares : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "Auxiliar1Id", c => c.Long());
            AddColumn("dbo.FatContaItem", "Auxiliar1EspecialidadeId", c => c.Long());
            AddColumn("dbo.FatContaItem", "Auxiliar2Id", c => c.Long());
            AddColumn("dbo.FatContaItem", "Auxiliar2EspecialidadeId", c => c.Long());
            AddColumn("dbo.FatContaItem", "Auxiliar3Id", c => c.Long());
            AddColumn("dbo.FatContaItem", "Auxiliar3EspecialidadeId", c => c.Long());
            AddColumn("dbo.FatContaItem", "InstrumentadorId", c => c.Long());
            AddColumn("dbo.FatContaItem", "InstrumentadorEspecialidadeId", c => c.Long());
            AddColumn("dbo.FatContaItem", "AnestesistaId", c => c.Long());
            AddColumn("dbo.FatContaItem", "EspecialidadeAnestesistaId", c => c.Long());
            AddColumn("dbo.FatContaItem", "FatPrecoId", c => c.Long());
            CreateIndex("dbo.FatContaItem", "Auxiliar1Id");
            CreateIndex("dbo.FatContaItem", "Auxiliar1EspecialidadeId");
            CreateIndex("dbo.FatContaItem", "Auxiliar2Id");
            CreateIndex("dbo.FatContaItem", "Auxiliar2EspecialidadeId");
            CreateIndex("dbo.FatContaItem", "Auxiliar3Id");
            CreateIndex("dbo.FatContaItem", "Auxiliar3EspecialidadeId");
            CreateIndex("dbo.FatContaItem", "InstrumentadorId");
            CreateIndex("dbo.FatContaItem", "InstrumentadorEspecialidadeId");
            CreateIndex("dbo.FatContaItem", "AnestesistaId");
            CreateIndex("dbo.FatContaItem", "EspecialidadeAnestesistaId");
            CreateIndex("dbo.FatContaItem", "FatPrecoId");
            AddForeignKey("dbo.FatContaItem", "AnestesistaId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.FatContaItem", "Auxiliar1Id", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.FatContaItem", "Auxiliar1EspecialidadeId", "dbo.SisMedicoEspecialidade", "Id");
            AddForeignKey("dbo.FatContaItem", "Auxiliar2Id", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.FatContaItem", "Auxiliar2EspecialidadeId", "dbo.SisMedicoEspecialidade", "Id");
            AddForeignKey("dbo.FatContaItem", "Auxiliar3Id", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.FatContaItem", "Auxiliar3EspecialidadeId", "dbo.SisMedicoEspecialidade", "Id");
            AddForeignKey("dbo.FatContaItem", "EspecialidadeAnestesistaId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.FatContaItem", "InstrumentadorId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.FatContaItem", "InstrumentadorEspecialidadeId", "dbo.SisMedicoEspecialidade", "Id");
            AddForeignKey("dbo.FatContaItem", "FatPrecoId", "dbo.FatTabelaPrecoItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatContaItem", "FatPrecoId", "dbo.FatTabelaPrecoItem");
            DropForeignKey("dbo.FatContaItem", "InstrumentadorEspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropForeignKey("dbo.FatContaItem", "InstrumentadorId", "dbo.SisMedico");
            DropForeignKey("dbo.FatContaItem", "EspecialidadeAnestesistaId", "dbo.SisMedico");
            DropForeignKey("dbo.FatContaItem", "Auxiliar3EspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropForeignKey("dbo.FatContaItem", "Auxiliar3Id", "dbo.SisMedico");
            DropForeignKey("dbo.FatContaItem", "Auxiliar2EspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropForeignKey("dbo.FatContaItem", "Auxiliar2Id", "dbo.SisMedico");
            DropForeignKey("dbo.FatContaItem", "Auxiliar1EspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropForeignKey("dbo.FatContaItem", "Auxiliar1Id", "dbo.SisMedico");
            DropForeignKey("dbo.FatContaItem", "AnestesistaId", "dbo.SisMedico");
            DropIndex("dbo.FatContaItem", new[] { "FatPrecoId" });
            DropIndex("dbo.FatContaItem", new[] { "EspecialidadeAnestesistaId" });
            DropIndex("dbo.FatContaItem", new[] { "AnestesistaId" });
            DropIndex("dbo.FatContaItem", new[] { "InstrumentadorEspecialidadeId" });
            DropIndex("dbo.FatContaItem", new[] { "InstrumentadorId" });
            DropIndex("dbo.FatContaItem", new[] { "Auxiliar3EspecialidadeId" });
            DropIndex("dbo.FatContaItem", new[] { "Auxiliar3Id" });
            DropIndex("dbo.FatContaItem", new[] { "Auxiliar2EspecialidadeId" });
            DropIndex("dbo.FatContaItem", new[] { "Auxiliar2Id" });
            DropIndex("dbo.FatContaItem", new[] { "Auxiliar1EspecialidadeId" });
            DropIndex("dbo.FatContaItem", new[] { "Auxiliar1Id" });
            DropColumn("dbo.FatContaItem", "FatPrecoId");
            DropColumn("dbo.FatContaItem", "EspecialidadeAnestesistaId");
            DropColumn("dbo.FatContaItem", "AnestesistaId");
            DropColumn("dbo.FatContaItem", "InstrumentadorEspecialidadeId");
            DropColumn("dbo.FatContaItem", "InstrumentadorId");
            DropColumn("dbo.FatContaItem", "Auxiliar3EspecialidadeId");
            DropColumn("dbo.FatContaItem", "Auxiliar3Id");
            DropColumn("dbo.FatContaItem", "Auxiliar2EspecialidadeId");
            DropColumn("dbo.FatContaItem", "Auxiliar2Id");
            DropColumn("dbo.FatContaItem", "Auxiliar1EspecialidadeId");
            DropColumn("dbo.FatContaItem", "Auxiliar1Id");
        }
    }
}
