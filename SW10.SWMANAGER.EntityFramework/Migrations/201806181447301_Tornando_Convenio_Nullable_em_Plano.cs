namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Tornando_Convenio_Nullable_em_Plano : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisPlano", "SisConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.SisPlano", new[] { "SisConvenioId" });
            AlterColumn("dbo.SisPlano", "SisConvenioId", c => c.Long());
            CreateIndex("dbo.SisPlano", "SisConvenioId");
            AddForeignKey("dbo.SisPlano", "SisConvenioId", "dbo.SisConvenio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisPlano", "SisConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.SisPlano", new[] { "SisConvenioId" });
            AlterColumn("dbo.SisPlano", "SisConvenioId", c => c.Long(nullable: false));
            CreateIndex("dbo.SisPlano", "SisConvenioId");
            AddForeignKey("dbo.SisPlano", "SisConvenioId", "dbo.SisConvenio", "Id", cascadeDelete: true);
        }
    }
}
