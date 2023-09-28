namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario_InclusaoCamposContagem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Produto", "UltimoValorCompra", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Est_Produto", "ValorCompraMedia", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.EstInventarioItem", "QuantidadeContagem2", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.EstInventarioItem", "QuantidadeContagem3", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.EstInventarioItem", "UsuarioContegemId", c => c.Long());
            AddColumn("dbo.EstInventarioItem", "UsuarioContegemId2", c => c.Long());
            AddColumn("dbo.EstInventarioItem", "UsuarioContegemId3", c => c.Long());
            AddColumn("dbo.EstInventarioItem", "DataContagem", c => c.DateTime());
            AddColumn("dbo.EstInventarioItem", "DataContagem2", c => c.DateTime());
            AddColumn("dbo.EstInventarioItem", "DataContagem3", c => c.DateTime());
            AddColumn("dbo.Est_EstoqueEmpresa", "IsPrincipal", c => c.Boolean(nullable: false));
            CreateIndex("dbo.EstInventarioItem", "UsuarioContegemId");
            CreateIndex("dbo.EstInventarioItem", "UsuarioContegemId2");
            CreateIndex("dbo.EstInventarioItem", "UsuarioContegemId3");
            AddForeignKey("dbo.EstInventarioItem", "UsuarioContegemId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.EstInventarioItem", "UsuarioContegemId2", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.EstInventarioItem", "UsuarioContegemId3", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstInventarioItem", "UsuarioContegemId3", "dbo.AbpUsers");
            DropForeignKey("dbo.EstInventarioItem", "UsuarioContegemId2", "dbo.AbpUsers");
            DropForeignKey("dbo.EstInventarioItem", "UsuarioContegemId", "dbo.AbpUsers");
            DropIndex("dbo.EstInventarioItem", new[] { "UsuarioContegemId3" });
            DropIndex("dbo.EstInventarioItem", new[] { "UsuarioContegemId2" });
            DropIndex("dbo.EstInventarioItem", new[] { "UsuarioContegemId" });
            DropColumn("dbo.Est_EstoqueEmpresa", "IsPrincipal");
            DropColumn("dbo.EstInventarioItem", "DataContagem3");
            DropColumn("dbo.EstInventarioItem", "DataContagem2");
            DropColumn("dbo.EstInventarioItem", "DataContagem");
            DropColumn("dbo.EstInventarioItem", "UsuarioContegemId3");
            DropColumn("dbo.EstInventarioItem", "UsuarioContegemId2");
            DropColumn("dbo.EstInventarioItem", "UsuarioContegemId");
            DropColumn("dbo.EstInventarioItem", "QuantidadeContagem3");
            DropColumn("dbo.EstInventarioItem", "QuantidadeContagem2");
            DropColumn("dbo.Est_Produto", "ValorCompraMedia");
            DropColumn("dbo.Est_Produto", "UltimoValorCompra");
        }
    }
}
