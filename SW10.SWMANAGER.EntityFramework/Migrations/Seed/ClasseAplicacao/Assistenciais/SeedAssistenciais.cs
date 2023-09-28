using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.EntityFramework;
using SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Assistenciais;
using System;
using System.IO;
using System.Reflection;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao
{
    public class SeedAssistenciais
    {
        private readonly SWMANAGERDbContext _context;

        public SeedAssistenciais(SWMANAGERDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Determina o caminho relativo
        /// </summary>
        /// <param name="seedFile">Nome do arquivo a ser lido</param>
        /// <returns></returns>
        public static string MapPath(string seedFile)
        {
            seedFile = @"~\..\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\" + seedFile;
            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
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
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('{nomeTabela}', RESEED, 0);");
            }
        }

        public static void ReSeedTable<T>(SWMANAGERDbContext context, int value) where T : CamposPadraoCRUD
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
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('{nomeTabela}', RESEED, {value});");
            }
        }

        public static T CamposPadraoCRUD<T>(T classe) where T : CamposPadraoCRUD
        {
            classe.IsSistema = false;
            classe.IsDeleted = false;
            classe.DeleterUserId = null;
            classe.DeletionTime = null;
            classe.LastModificationTime = null;
            classe.LastModifierUserId = null;
            classe.CreationTime = DateTime.Now;
            classe.CreatorUserId = 3;
            return classe;
        }

        public void Create()
        {
            //cria estados do movimento
            new DefaultCBOFamilia(_context).Create();

            new DefaultCBO(_context).Create();
        }

        public static void SemAutoIncrementoTable<T>(SWMANAGERDbContext context) where T : CamposPadraoCRUD
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
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('" + nomeTabela + "', RESEED, 0);");
                context.Database.ExecuteSqlCommand(string.Concat(" set identity_insert ", nomeTabela, " on "));
            }
        }


    }
}
