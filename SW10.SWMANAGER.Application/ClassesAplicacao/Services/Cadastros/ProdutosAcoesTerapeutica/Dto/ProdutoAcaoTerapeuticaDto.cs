using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosAcoesTerapeutica;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto
{
    [AutoMap(typeof(ProdutoAcaoTerapeutica))]
    public class ProdutoAcaoTerapeuticaDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
