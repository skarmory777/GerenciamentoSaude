using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public interface ILeitoAppService : IApplicationService
    {
        Task<PagedResultDto<LeitoDto>> Listar(ListarLeitosInput input);

        Task<List<Leito>> ListarParaRelatorioMapaLeitos(long empresaId, long? statusLeito);

        Task<PagedResultDto<LeitoComAtendimentoDto>> ListarPorUnidadePaginado(ListarLeitosInput input);

        Task<ListResultDto<LeitoComAtendimentoDto>> ListarTodos();

        Task<ListResultDto<LeitoComAtendimentoDto>> ListarPorUnidade(ListarLeitosInput unidadeId);

        Task<ICollection<LeitoDto>> ListarPorUnidadeParaDrop(long? id);

        Task<ListResultDto<LeitoDto>> ListarPorUnidadeParaDrop2(long id);

        Task CriarOuEditar(CriarOuEditarLeito input);

        Task Excluir(CriarOuEditarLeito input);

        Task<LeitoDto> Obter(long id);

        void OcuparLeito(long? leitoId);

        void DesocuparLeito(long? leitoId);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarLeitoVagoDropdown(DropdownInput dropdownInput);

        Task AlterarStausLeito(long leitoId, long statusId);
    }
}
