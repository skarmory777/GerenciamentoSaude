using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.EntityFramework;
using SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos;
using System;
using System.IO;
using System.Reflection;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao
{
    public class SeedSuprimentos
    {
        private readonly SWMANAGERDbContext _context;

        public SeedSuprimentos(SWMANAGERDbContext context)
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
            seedFile = @"~\..\Migrations\Seed\ClasseAplicacao\Suprimentos\scripts\" + seedFile;
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

        public static void ReSeedTable<T>(SWMANAGERDbContext context, long valorId) where T : CamposPadraoCRUD
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
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('" + nomeTabela + "', RESEED," + valorId + ");");
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
            // classe.CreatorUserId = 3;
            return classe;
        }

        public void Create()
        {
            //cria Estoque, EstoqueLocalizacao
            new DefaultEstoque(_context).Create();

            //cria UnidadeTipo
            new DefaultUnidadeTipo(_context).Create();

            //cria Unidade
            new DefaultUnidade(_context).Create();

            //cria Genero
            new DefaultGenero(_context).Create();

            //cria Grupo, Classe, Sub-Classe
            new DefaultGrupoClasseSubClasse(_context).Create();

            //cria Grupo, Classe, Sub-Classe
            new DefaultDCB(_context).Create();

            //cria produtos
            new DefaultProdutos(_context).Create();

            //cria produtos unidades
            new DefaultProdutoUnidades(_context).Create();

            //cria estados do movimento
            new DefaultMovimentoEstado(_context).Create();

            new DefaultCFOP(_context).Create();
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
                // context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('" + nomeTabela + "', RESEED, 0);");
                context.Database.ExecuteSqlCommand(string.Concat(" set identity_insert ", nomeTabela, " on "));
            }
        }


        public static void ComAutoIncrementoTable<T>(SWMANAGERDbContext context) where T : CamposPadraoCRUD
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
                context.Database.ExecuteSqlCommand(string.Concat(" set identity_insert ", nomeTabela, " off "));
            }
        }


    }
}
