using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.IO;
using System.Reflection;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Imagens
{
    public class SeedImagens
    {
        private readonly SWMANAGERDbContext _context;

        public SeedImagens(SWMANAGERDbContext context)
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
            seedFile = @"~\..\Migrations\Seed\ClasseAplicacao\Imagens\scripts\" + seedFile;
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
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('" + nomeTabela + "', RESEED, 0);");
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
            classe.CreatorUserId = 2;
            return classe;
        }

        public void Create()
        {
            //Cria status
            new Default_LauMovItemStatus(_context).Create();

        }
    }
}
