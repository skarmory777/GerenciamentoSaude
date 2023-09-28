using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosCodigosMedicamento;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Dto
{
    [AutoMap(typeof(ProdutoCodigoMedicamento))]
    public class ProdutoCodigoMedicamentoDto : CamposPadraoCRUDDto
    {

    }
}
