using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultProdutos
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultProdutos(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            //referencia, gerencial, compras, consumo

            var produtos = _context.Produtos.ToList();
            if (produtos == null || produtos.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<Produto>(_context);

                Produto produtoPrincipal = new Produto();
                produtoPrincipal.CodigoBarra = string.Empty;
                produtoPrincipal.Descricao = "SULFATO DE AMICACINA - 250 MG/ML (PRINCIPAL)";
                produtoPrincipal.DescricaoResumida = "AMICACINA 250 MG/ML (PRINCIPAL)";
                produtoPrincipal.Especificacao = "250 MG/ML SOL INJ CX 50 AMP VD INC X 2 ML (EMB HOSP)";
                produtoPrincipal.IsBloqueioCompra = false;
                produtoPrincipal.IsCurvaABC = true;
                produtoPrincipal.IsKit = false;
                produtoPrincipal.IsLiberadoMovimentacao = true;
                produtoPrincipal.IsLote = true;
                produtoPrincipal.IsValidade = true;
                produtoPrincipal.IsMedicamento = true;
                produtoPrincipal.IsMedicamentoControlado = false;
                produtoPrincipal.IsOPME = false;
                produtoPrincipal.DCB = _context.DCBs.Find(9703);
                produtoPrincipal.EstoqueLocalizacao = _context.EstoqueLocalizacao.Find(1);
                produtoPrincipal.Genero = _context.Generos.Find(1);
                produtoPrincipal.Grupo = _context.Grupos.Find(1);
                produtoPrincipal.Classe = _context.GrupoClasses.Find(1);
                produtoPrincipal.SubClasse = _context.GrupoSubClasses.Find(13);
                produtoPrincipal.ProdutoPrincipal = null;
                produtoPrincipal.IsPrincipal = true; //indica que é mestre
                produtoPrincipal.CodigoSistemas = string.Empty;
                produtoPrincipal.CodigoTISS = string.Empty;
                SeedSuprimentos.CamposPadraoCRUD(produtoPrincipal);
                _context.Produtos.Add(produtoPrincipal);
                _context.SaveChanges();



                produtoPrincipal = _context.Produtos.Find(1);

                Produto produto1 = new Produto();
                produto1.CodigoBarra = "7898208144564";
                produto1.Descricao = "SULFATO DE AMICACINA - 250 MG/ML NOVAFARMA";
                produto1.DescricaoResumida = "AMICACINA 250 MG/ML NOVAFARMA";
                produto1.Especificacao = "250 MG/ML SOL INJ CX 50 AMP VD INC X 2 ML (EMB HOSP)";
                produto1.IsBloqueioCompra = false;
                produto1.IsCurvaABC = true;
                produto1.IsKit = false;
                produto1.IsLiberadoMovimentacao = true;
                produto1.IsLote = true;
                produto1.IsValidade = true;
                produto1.IsMedicamento = true;
                produto1.IsMedicamentoControlado = false;
                produto1.IsOPME = false;
                produto1.DCB = _context.DCBs.Find(9703);
                produto1.EstoqueLocalizacao = _context.EstoqueLocalizacao.Find(1);
                produto1.Genero = _context.Generos.Find(1);
                produto1.Grupo = _context.Grupos.Find(1);
                produto1.Classe = _context.GrupoClasses.Find(1);
                produto1.SubClasse = _context.GrupoSubClasses.Find(13);
                produto1.ProdutoPrincipal = produtoPrincipal;
                produto1.IsPrincipal = false;
                produto1.CodigoSistemas = string.Empty;
                produto1.CodigoTISS = string.Empty;
                SeedSuprimentos.CamposPadraoCRUD(produto1);
                _context.Produtos.Add(produto1);

                Produto produto2 = new Produto();
                produto2.CodigoBarra = "7896112190707";
                produto2.Descricao = "SULFATO DE AMICACINA - 250 MG/ML LABORATÓRIO TEUTO BRASILEIRO S/A";
                produto2.DescricaoResumida = "AMICACINA 250 MG/ML TEUTO BRASIL";
                produto2.Especificacao = "250 MG/ML SOL INJ CX 50 AMP VD INC X 2 ML (EMB HOSP)";
                produto2.IsBloqueioCompra = false;
                produto2.IsCurvaABC = true;
                produto2.IsKit = false;
                produto2.IsLiberadoMovimentacao = true;
                produto2.IsLote = true;
                produto2.IsValidade = true;
                produto2.IsMedicamento = true;
                produto2.IsMedicamentoControlado = false;
                produto2.IsOPME = false;
                produto2.DCB = _context.DCBs.Find(9703);
                produto2.EstoqueLocalizacao = _context.EstoqueLocalizacao.Find(1);
                produto2.Genero = _context.Generos.Find(1);
                produto2.Grupo = _context.Grupos.Find(1);
                produto2.Classe = _context.GrupoClasses.Find(1);
                produto2.SubClasse = _context.GrupoSubClasses.Find(13);
                produto2.ProdutoPrincipal = produtoPrincipal;
                produto2.IsPrincipal = false;
                produto2.CodigoSistemas = string.Empty;
                produto2.CodigoTISS = string.Empty;
                SeedSuprimentos.CamposPadraoCRUD(produto2);
                _context.Produtos.Add(produto2);


                /* ---------------  Outro Produto ----------------------*/
                Produto produto3 = new Produto();
                produto3.CodigoBarra = "7894916144605";
                produto3.Descricao = "AMOXICILINA 500 MG - EMS SIGMA PHARMA LTDA";
                produto3.DescricaoResumida = "AMOXICILINA 500 MG EMS";
                produto3.Especificacao = "500 MG CAP GEL DURA CT BL AL PLAS INC X 21";
                produto3.IsBloqueioCompra = false;
                produto3.IsCurvaABC = true;
                produto3.IsKit = false;
                produto3.IsLiberadoMovimentacao = true;
                produto3.IsLote = true;
                produto3.IsValidade = true;
                produto3.IsMedicamento = true;
                produto3.IsMedicamentoControlado = false;
                produto3.IsOPME = false;
                produto3.DCB = _context.DCBs.Find(853);
                produto3.EstoqueLocalizacao = _context.EstoqueLocalizacao.Find(1);
                produto3.Genero = _context.Generos.Find(1);
                produto3.Grupo = _context.Grupos.Find(1);
                produto3.Classe = _context.GrupoClasses.Find(1);
                produto3.SubClasse = _context.GrupoSubClasses.Find(2);
                produto3.IsPrincipal = false;
                produto3.ProdutoPrincipal = null;
                produto3.CodigoSistemas = string.Empty;
                produto3.CodigoTISS = string.Empty;
                SeedSuprimentos.CamposPadraoCRUD(produto3);
                _context.Produtos.Add(produto3);
            }
        }
    }
}
