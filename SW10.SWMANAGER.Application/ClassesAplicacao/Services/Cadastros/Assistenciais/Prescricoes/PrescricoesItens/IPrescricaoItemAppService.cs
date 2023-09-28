using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public interface IPrescricaoItemAppService : IApplicationService
    {
        Task<PagedResultDto<PrescricaoItemDto>> Listar(ListarInput input);

        Task<PagedResultDto<SubPrescricaoItemDto>> ListarPorPrescricaoItemId(ListarInput input);
        
        Task<PagedResultDto<SubPrescricaoItemDto>> ListarSelecionarPorPrescricaoItemId(ListarInput input);

        Task<ListResultDto<PrescricaoItemDto>> ListarTodos();

        Task<ListResultDto<PrescricaoItemDto>> ListarFiltro(string filtro);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<PrescricaoItemDto> CriarOuEditar(PrescricaoItemDto input);

        Task Excluir(long id);

        Task<PrescricaoItemDto> Obter(long id);

        PrescricaoItemDto ObterSync(long id);

        Task<IEnumerable<PrescricaoItemDto>> ObterDapper(long id);

        Task<IEnumerable<PrescricaoItemDto>> ObterIds(List<long> ids);

        Task<PrescricaoItemDto> ObterPorProduto(long produtoId);

        Task<PagedResultDto<GenericoIdNome>> ListarProdutosDisponiveis(ListarInput input);

        Task<PagedResultDto<GenericoIdNome>> ListarProdutosIncluidos(ListarInput input);

        Task InserirPorProduto(CadastroAgilizadoDto input);

        Task ExcluirPorProduto(List<string> ids);

        Task InserirPorFatItem(CadastroAgilizadoDto input);

        Task ExcluirPorFatItem(List<string> ids);

        Task<PagedResultDto<GenericoIdNome>> ListarExamesLaboratoriaisDisponiveis(ListarInput input);

        Task<PagedResultDto<GenericoIdNome>> ListarExamesLaboratoriaisIncluidos(ListarInput input);

        Task<PagedResultDto<GenericoIdNome>> ListarExamesImagemDisponiveis(ListarInput input);

        Task<PagedResultDto<GenericoIdNome>> ListarExamesImagemIncluidos(ListarInput input);

        Task<IResultDropdownList<long>> ListarDiluenteDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput);



        Task<IResultDropdownList<long>> ListarUnidadePorProdutoDropdown(ConfiguracaoPrescricaoItemDropDownInput input);
        
        Task<PrescricaoItemDto> CriarOuEditarSubItem(CriarSubItemPrescricaoViewModel input);
    }
}
