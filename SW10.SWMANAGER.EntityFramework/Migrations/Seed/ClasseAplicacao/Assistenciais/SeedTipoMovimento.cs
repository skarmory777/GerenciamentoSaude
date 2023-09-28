using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.Text;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao
{
    public class SeedTipoMovimento
    {
        private readonly SWMANAGERDbContext _context;

        public SeedTipoMovimento(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public static void ReSeedTable<T>(SWMANAGERDbContext context) where T : CamposPadraoCRUD
        {
            string nomeTabela = string.Empty;
            Attribute[] attrs = Attribute.GetCustomAttributes(typeof(T));
            foreach (Attribute attr in attrs)
            {
                var valor = attr as System.ComponentModel.DataAnnotations.Schema.TableAttribute;
                if (valor == null)
                    continue;
                nomeTabela = valor.Name;
                break;
            }

            if (!string.IsNullOrWhiteSpace(nomeTabela))
            {
                //context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('" + nomeTabela + "', RESEED, 0);");
                var comandoSql = "set identity_insert " + nomeTabela + " on " +

                                 "INSERT " + nomeTabela +
                                           " (Id" +
                                           ",Descricao" +
                                           ",IsEntrada" +
                                           ",IsSistema" +
                                           ",IsDeleted" +
                                           ",DeleterUserId" +
                                           ",DeletionTime" +
                                           ",LastModificationTime" +
                                           ",LastModifierUserId" +
                                           ",CreationTime" +
                                           ",CreatorUserId" +
                                           ",Codigo" +
                                           ",ImportaId" +
                                           ",IsOrdemCompra" +
                                           ",IsPessoa" +
                                           ",IsOrdemCompraObrigatoria" +
                                           ",IsFiscal" +
                                           ",IsFrete" +
                                           ",IsFinanceiro)" +
                                 "VALUES({0},'Inventário', 1, 0, 0, NULL, NULL, NULL, NULL, GETDATE(), NULL, NULL, NULL, 0, 0, 0, 0, 0, 0) " +

                                 "set identity_insert " + nomeTabela + " off";

                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(comandoSql, (long)EnumTipoMovimento.Inventario_Entrada);

                context.Database.ExecuteSqlCommand(sql.ToString());
            }
        }
    }
}
