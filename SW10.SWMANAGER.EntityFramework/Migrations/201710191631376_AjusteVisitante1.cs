namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteVisitante1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AteVisitante", name: "FornedcedorId", newName: "ForncedorId");
            RenameIndex(table: "dbo.AteVisitante", name: "IX_FornedcedorId", newName: "IX_FornecedorId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.AteVisitante", name: "IX_FornecedorId", newName: "IX_FornedcedorId");
            RenameColumn(table: "dbo.AteVisitante", name: "FornecedorId", newName: "FornedcedorId");
        }
    }
}
