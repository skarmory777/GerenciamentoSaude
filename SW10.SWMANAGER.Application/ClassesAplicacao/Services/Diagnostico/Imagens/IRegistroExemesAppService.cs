using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnosticos.Laudos.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens
{
    public interface IRegistroExemesAppService : IApplicationService
    {
        Task<PagedResultDto<RegistroExameIndex>> Listar(ListarLauMovimentoItensInput input);
        Task<LaudoMovimentoDto> Obter(long id);
        DefaultReturn<LaudoMovimentoDto> CriarOuEditar(LaudoMovimentoDto input);
        Task<LaudoMovimentoItemDto> ObterMovimentoItem(long id);
        Task<PagedResultDto<RegistroExameIndex>> ListarMovimentosItens(ListarLauMovimentoItensInput input);
        DefaultReturn<LaudoMovimentoDto> RegistrarLaudo(LaudoMovimentoItemDto input);
        Task<PagedResultDto<RegistroExameIndex>> ListarExamesFaturadosSemregistros(ListarLauMovimentoItensInput input);
        Task<LaudoMovimentoDto> ObterExamesFaturadosSemregistros(List<long> ids);
    }
}
