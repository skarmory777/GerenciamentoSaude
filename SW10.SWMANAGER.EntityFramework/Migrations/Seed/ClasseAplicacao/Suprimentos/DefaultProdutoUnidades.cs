using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultProdutoUnidades
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultProdutoUnidades(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            //referencia, gerencial, compras, consumo

            var produtoUnidades = _context.ProdutoUnidades.ToList();
            if (produtoUnidades == null || produtoUnidades.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<ProdutoUnidade>(_context);

                //tipos de uniaddes
                UnidadeTipo unidadeReferencial = _context.UnidadeTipos.Find(1);//referência
                UnidadeTipo unidadeGerencial = _context.UnidadeTipos.Find(2);//gerencial
                UnidadeTipo unidadeCompras = _context.UnidadeTipos.Find(3);//compras

                //unidade AMPOLA e filhos
                Unidade unidadesReferenciaAmpola = _context.Unidades.Find(1);//ampola
                Unidade unidadeCaixa50ampolas = unidadesReferenciaAmpola.Unidades.First(u => u.Id == 8);//caixa 50

                //associa o produto a unidade
                Produto produto = _context.Produtos.Find(1);//busca o produto
                ProdutoUnidade produtoUnidade = new ProdutoUnidade();
                produtoUnidade.Produto = produto;//associa produto
                produtoUnidade.Tipo = unidadeReferencial;//associa tipo
                produtoUnidade.Unidade = unidadesReferenciaAmpola;//associa unidade
                produtoUnidade.IsAtivo = true;
                produtoUnidade.IsPrescricao = true;
                SeedSuprimentos.CamposPadraoCRUD(produtoUnidade);
                _context.ProdutoUnidades.Add(produtoUnidade);

                produtoUnidade = new ProdutoUnidade();
                produtoUnidade.Produto = produto;//associa produto
                produtoUnidade.Tipo = unidadeGerencial;//associa tipo
                produtoUnidade.Unidade = unidadesReferenciaAmpola;//associa unidade
                produtoUnidade.IsAtivo = true;
                produtoUnidade.IsPrescricao = false;
                SeedSuprimentos.CamposPadraoCRUD(produtoUnidade);
                _context.ProdutoUnidades.Add(produtoUnidade);

                produtoUnidade = new ProdutoUnidade();
                produtoUnidade.Produto = produto;//associa produto
                produtoUnidade.Tipo = unidadeCompras;//associa tipo
                produtoUnidade.Unidade = unidadeCaixa50ampolas;//associa unidade
                produtoUnidade.IsAtivo = true;
                produtoUnidade.IsPrescricao = false;
                SeedSuprimentos.CamposPadraoCRUD(produtoUnidade);
                _context.ProdutoUnidades.Add(produtoUnidade);
            }
        }
    }
}
