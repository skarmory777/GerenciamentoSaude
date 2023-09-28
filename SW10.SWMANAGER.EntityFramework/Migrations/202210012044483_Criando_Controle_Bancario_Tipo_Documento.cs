namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Criando_Controle_Bancario_Tipo_Documento : DbMigration
    {
        public override void Up()
        {
            var command = "INSERT INTO [dbo].[FinTipoDocumento] " +
           "([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime]" +
           ",[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[ImportaId]) " +
           "SELECT 1,'010','Controle Bancário',0,null,null,null,null,GETDATE(),null,null "+
           "WHERE NOT EXISTS(SELECT[Descricao] FROM[dbo].[FinTipoDocumento] WHERE [Descricao] = 'Controle Bancário'); ";

            Sql(command);
        }
        
        public override void Down()
        {
            var command = "DELETE [dbo].[FinTipoDocumento] WHERE [Descricao] = 'Controle Bancário'";

            Sql(command);
        }
    }
}
