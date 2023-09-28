namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_foreignkey_TipoVinculoEmpregaticio_e_TipoParticipacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisMedico", "SisTipoParticipacaoId", c => c.Long());
            AddColumn("dbo.SisMedico", "SisTipoVinculoEmpregaticioId", c => c.Long());
            CreateIndex("dbo.SisMedico", "SisTipoParticipacaoId");
            CreateIndex("dbo.SisMedico", "SisTipoVinculoEmpregaticioId");
            AddForeignKey("dbo.SisMedico", "SisTipoParticipacaoId", "dbo.SisTipoParticipacao", "Id");
            AddForeignKey("dbo.SisMedico", "SisTipoVinculoEmpregaticioId", "dbo.TipoVinculoEmpregaticio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisMedico", "SisTipoVinculoEmpregaticioId", "dbo.TipoVinculoEmpregaticio");
            DropForeignKey("dbo.SisMedico", "SisTipoParticipacaoId", "dbo.SisTipoParticipacao");
            DropIndex("dbo.SisMedico", new[] { "SisTipoVinculoEmpregaticioId" });
            DropIndex("dbo.SisMedico", new[] { "SisTipoParticipacaoId" });
            DropColumn("dbo.SisMedico", "SisTipoVinculoEmpregaticioId");
            DropColumn("dbo.SisMedico", "SisTipoParticipacaoId");
        }
    }
}
