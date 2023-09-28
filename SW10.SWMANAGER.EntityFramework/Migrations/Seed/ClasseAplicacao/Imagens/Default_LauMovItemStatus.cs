using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Imagens
{
    public class Default_LauMovItemStatus
    {
        private readonly SWMANAGERDbContext _context;
        public Default_LauMovItemStatus(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            var valores = _context.LaudoMovimentoItemStatus.ToList();
            if (valores == null || valores.Count() == 0)
            {
                string[] nomeTipoUnidade = new string[3];
                nomeTipoUnidade[0] = "Inicial";
                nomeTipoUnidade[1] = "Realizando";
                nomeTipoUnidade[2] = "Finalizado";

                for (int i = 0; i < nomeTipoUnidade.Length; i++)
                {
                    LaudoMovimentoStatus novoObjeto = new LaudoMovimentoStatus()
                    {
                        Id = i + 1,
                        Codigo = (i + 1).ToString(),
                        Descricao = nomeTipoUnidade[i]
                    };

                    SeedImagens.CamposPadraoCRUD(novoObjeto);
                    _context.LaudoMovimentoItemStatus.Add(novoObjeto);
                }
            }
        }
    }
}
