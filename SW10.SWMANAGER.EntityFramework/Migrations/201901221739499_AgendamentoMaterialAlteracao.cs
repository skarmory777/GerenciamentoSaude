namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AgendamentoMaterialAlteracao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "AgendamentoCirurgicoId" });
            AlterColumn("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", c => c.Long());
            CreateIndex("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId");
            AddForeignKey("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "AgendamentoCirurgicoId" });
            AlterColumn("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", c => c.Long(nullable: false));
            CreateIndex("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId");
            AddForeignKey("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico", "Id", cascadeDelete: true);
        }
    }
}
