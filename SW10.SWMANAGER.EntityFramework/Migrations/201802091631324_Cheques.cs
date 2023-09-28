namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Cheques : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinCheque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TalaoChequeId = c.Long(nullable: false),
                    Numero = c.Long(nullable: false),
                    Nominal = c.String(),
                    Data = c.DateTime(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cheque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinTalaoCheque", t => t.TalaoChequeId, cascadeDelete: true)
                .Index(t => t.TalaoChequeId);

            CreateTable(
                "dbo.FinTalaoCheque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ContaCorrenteId = c.Long(nullable: false),
                    DataAbertura = c.DateTime(nullable: false),
                    NumeroInicial = c.Int(nullable: false),
                    NumeroFinal = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TalaoCheque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinContaCorrente", t => t.ContaCorrenteId, cascadeDelete: true)
                .Index(t => t.ContaCorrenteId);

            AddColumn("dbo.FinLancamentoQuitacao", "Desconto", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinLancamentoQuitacao", "Acrescimo", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinLancamentoQuitacao", "MoraMulta", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinLancamentoQuitacao", "ValorQuitacao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FinLancamentoQuitacao", "ValorEfetivo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.FinQuitacao", "Desconto");
            DropColumn("dbo.FinQuitacao", "Acrescimo");
            DropColumn("dbo.FinQuitacao", "MoraMulta");
            DropColumn("dbo.FinQuitacao", "ValorQuitacao");
            DropColumn("dbo.FinQuitacao", "ValorEfetivo");
        }

        public override void Down()
        {
            AddColumn("dbo.FinQuitacao", "ValorEfetivo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FinQuitacao", "ValorQuitacao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FinQuitacao", "MoraMulta", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinQuitacao", "Acrescimo", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinQuitacao", "Desconto", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.FinCheque", "TalaoChequeId", "dbo.FinTalaoCheque");
            DropForeignKey("dbo.FinTalaoCheque", "ContaCorrenteId", "dbo.FinContaCorrente");
            DropIndex("dbo.FinTalaoCheque", new[] { "ContaCorrenteId" });
            DropIndex("dbo.FinCheque", new[] { "TalaoChequeId" });
            DropColumn("dbo.FinLancamentoQuitacao", "ValorEfetivo");
            DropColumn("dbo.FinLancamentoQuitacao", "ValorQuitacao");
            DropColumn("dbo.FinLancamentoQuitacao", "MoraMulta");
            DropColumn("dbo.FinLancamentoQuitacao", "Acrescimo");
            DropColumn("dbo.FinLancamentoQuitacao", "Desconto");
            DropTable("dbo.FinTalaoCheque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TalaoCheque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinCheque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cheque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
