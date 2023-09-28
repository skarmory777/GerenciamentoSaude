using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultUnidadeTipo
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultUnidadeTipo(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            var unidadeTipos = _context.UnidadeTipos.ToList();
            if (unidadeTipos == null || unidadeTipos.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<UnidadeTipo>(_context);

                string[] nomeTipoUnidade = new string[4];
                nomeTipoUnidade[0] = "Referência";
                nomeTipoUnidade[1] = "Gerencial";
                nomeTipoUnidade[2] = "Compras";
                nomeTipoUnidade[3] = "Consumo";

                //nomeTipoUnidade[3] = "Referência";
                for (int i = 0; i < nomeTipoUnidade.Length; i++)
                {
                    UnidadeTipo unidadeTipo = new UnidadeTipo()
                    {
                        Descricao = nomeTipoUnidade[i]
                    };

                    SeedSuprimentos.CamposPadraoCRUD(unidadeTipo);
                    _context.UnidadeTipos.Add(unidadeTipo);
                }
            }
        }
    }
}
