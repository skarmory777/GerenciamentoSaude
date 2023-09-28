using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultEstoque
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultEstoque(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            //estoques e locais estoque
            var estoques = _context.Estoques.ToList();
            if (estoques == null || estoques.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<Estoque>(_context);
                SeedSuprimentos.ReSeedTable<EstoqueLocalizacao>(_context);

                string[] nomeEstoques = new string[3];
                nomeEstoques[0] = "Estoque Central";
                nomeEstoques[1] = "Filial Botafogo";
                nomeEstoques[2] = "Filial Barra da Tijuca";
                for (int i = 0; i < nomeEstoques.Length; i++)
                {
                    Estoque estoque = new Estoque()
                    {
                        Descricao = nomeEstoques[i]
                    };


                    SeedSuprimentos.CamposPadraoCRUD(estoque);
                    _context.Estoques.Add(estoque);
                    _context.SaveChanges();

                    EstoqueLocalizacao estoqueLocalizacao = new EstoqueLocalizacao()
                    {
                        Descricao = nomeEstoques[i],
                        Codigo = "Padrão",
                        Estoque = estoque
                    };

                    SeedSuprimentos.CamposPadraoCRUD(estoqueLocalizacao);
                    _context.EstoqueLocalizacao.Add(estoqueLocalizacao);
                }
            }
        }
    }
}
