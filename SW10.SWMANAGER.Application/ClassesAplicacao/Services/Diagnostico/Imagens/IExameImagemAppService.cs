using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens
{
    public interface IExameImagemAppService : IApplicationService
    {
        Task<PagedResultDto<ExameListDto>> GetExames(GetExamesInput input);

        Task<PagedResultDto<ExameItemListDto>> GetExameItens(GetExamesInput input);

        Task<FileDto> GetExamesToExcel();

        Task<GetExameForEditOutput> GetExameForEdit(NullableIdDto<long> input);

        Task<GetExameItemForEditOutput> GetExameItemForEdit(NullableIdDto<long> input);

        Task CreateOrUpdateExame(CreateOrUpdateExameInput input);

        Task CreateOrUpdateExameItem(CreateOrUpdateExameItemInput input);

        Task DeleteExame(EntityDto<long> input);

        Task DeleteExameItem(EntityDto<long> input);

        Task<ListResultDto<ExameListDto>> GetAllExames();

        Task<PagedResultDto<ExameItemListDto>> GetAllExameItens(GetExamesInput input);


        // Relatorio
        Task<LauMovimentoReportModel> ObterReportModel(long id);
        Task<PagedResultDto<LauMovimentoItemReportModel>> ListarItensReportModel(ListarLauMovimentoItensInput input);
    }
}
