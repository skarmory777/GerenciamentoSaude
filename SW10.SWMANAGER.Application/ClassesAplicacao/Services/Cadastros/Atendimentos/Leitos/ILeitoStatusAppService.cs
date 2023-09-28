using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public interface ILeitoStatusAppService : IApplicationService
    {
        Task<PagedResultDto<LeitoStatusDto>> Listar(ListarLeitosStatusInput input);

        Task CriarOuEditar(CriarOuEditarLeitoStatus input);

        Task Excluir(CriarOuEditarLeitoStatus input);

        Task<CriarOuEditarLeitoStatus> Obter(long id);
        Task<IResultDropdownList<long>> ListarDropDownTrasferencia(DropdownInput dropdownInput);

        //	Task<FileDto> ListarParaExcel (ListarLeitosStatusInput input);
    }
}
