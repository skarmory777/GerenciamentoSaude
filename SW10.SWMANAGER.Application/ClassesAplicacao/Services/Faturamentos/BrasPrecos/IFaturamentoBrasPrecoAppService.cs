using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos
{
    public interface IFaturamentoBrasPrecoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoBrasPrecoDto>> Listar(ListarBrasPrecosInput input);

        Task CriarOuEditar(FaturamentoBrasPrecoDto input);

        Task Excluir(FaturamentoBrasPrecoDto input);

        Task<FaturamentoBrasPrecoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarBrasPrecosInput input);

        Task<ICollection<FaturamentoBrasApresentacao>> ListarBrasApresentacaoPorBrasItem(long brasItemId);

        Task<ICollection<FaturamentoBrasLaboratorio>> ListarBrasLaboratorioPorBrasItem(long brasItemId);

    }
}
