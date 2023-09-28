using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultGrupoClasseSubClasse
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultGrupoClasseSubClasse(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var grupoSubClasses = _context.GrupoSubClasses.ToList();
            if (grupoSubClasses == null || grupoSubClasses.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<Grupo>(_context);
                SeedSuprimentos.ReSeedTable<GrupoClasse>(_context);
                SeedSuprimentos.ReSeedTable<GrupoSubClasse>(_context);

                String[] values = File.ReadAllText(SeedSuprimentos.MapPath("Grupo, Classe, Subclasse.csv"), Encoding.Default).Split('\r');
                //primeira linha ignorada, título das colunas
                Grupo grupo = null;
                GrupoClasse grupoClasse = null;
                GrupoSubClasse grupoSubClasse = null;
                string _grupoAnterior = string.Empty;
                string _grupoClasseAnterior = string.Empty;
                string _grupoSubClasseAnterior = string.Empty;
                for (int i = 1; i < values.Length; i++)
                {
                    string[] linha = values[i].Replace("\n", "").Split(';');
                    string _grupo = linha[0];
                    string _classe = linha[1];
                    string _subClasse = linha[2];

                    if ((!string.IsNullOrWhiteSpace(_grupo)) && _grupoAnterior != _grupo)
                    {
                        grupo = new Grupo()
                        {
                            Descricao = _grupo
                        };

                        SeedSuprimentos.CamposPadraoCRUD(grupo);
                        _context.Grupos.Add(grupo);
                        _context.SaveChanges();
                        _grupoAnterior = _grupo;
                    }

                    if ((!string.IsNullOrWhiteSpace(_classe)) && _grupoClasseAnterior != _classe)
                    {
                        grupoClasse = new GrupoClasse()
                        {
                            Descricao = _classe,
                            Grupo = grupo
                        };

                        SeedSuprimentos.CamposPadraoCRUD(grupoClasse);
                        _context.GrupoClasses.Add(grupoClasse);
                        _context.SaveChanges();
                        _grupoClasseAnterior = _classe;
                    }

                    if ((!string.IsNullOrWhiteSpace(_subClasse)) && _grupoSubClasseAnterior != _subClasse)
                    {
                        grupoSubClasse = new GrupoSubClasse()
                        {
                            Descricao = _subClasse,
                            Classe = grupoClasse
                        };

                        SeedSuprimentos.CamposPadraoCRUD(grupoSubClasse);
                        _context.GrupoSubClasses.Add(grupoSubClasse);
                        _context.SaveChanges();
                        _grupoSubClasseAnterior = _subClasse;
                    }
                }
            }
        }
    }
}
