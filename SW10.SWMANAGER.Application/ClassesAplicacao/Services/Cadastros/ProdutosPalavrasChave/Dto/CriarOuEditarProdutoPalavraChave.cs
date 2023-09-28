using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPalavrasChave;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto
{
    [AutoMap(typeof(ProdutoPalavraChave))]
    public class CriarOuEditarProdutoPalavraChave : CamposPadraoCRUDDto
    {

        public string Palavra { get; set; }

        public string Observacao { get; set; }

    }
}