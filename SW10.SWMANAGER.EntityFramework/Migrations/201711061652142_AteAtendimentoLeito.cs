namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AteAtendimentoLeito : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Atendimento", newName: "AteAtendimento");
            RenameTable(name: "dbo.SisLeito", newName: "AteLeito");
            RenameColumn(table: "dbo.AteLeito", name: "SisLeitoStatusId", newName: "AteLeitoStatusId");
            RenameIndex(table: "dbo.AteLeito", name: "IX_SisLeitoStatusId", newName: "IX_AteLeitoStatusId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.AteLeito", name: "IX_AteLeitoStatusId", newName: "IX_SisLeitoStatusId");
            RenameColumn(table: "dbo.AteLeito", name: "AteLeitoStatusId", newName: "SisLeitoStatusId");
            RenameTable(name: "dbo.AteLeito", newName: "SisLeito");
            RenameTable(name: "dbo.AteAtendimento", newName: "Atendimento");
        }
    }
}
