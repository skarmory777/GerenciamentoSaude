using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades
{
    public interface IUnidadeAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<UnidadeDto>> Listar(ListarUnidadesInput input);

        Task<ListResultDto<UnidadeDto>> ListarTodos();

        Task<ListResultDto<UnidadeDto>> ListarUnidadesReferenciais();

        Task<ListResultDto<UnidadeDto>> ListarPorReferencial(long? id, bool addPai);

        Task<PagedResultDto<UnidadeDto>> ListarPorReferencial(ListarUnidadesInput input);

        Task<long> GetIdUnidadelPorSigla(string sigla, bool? isReferencia = null, long? idRef = null);

        Task<string> GetSiglaUnidadePeloId(long id);

        Task CriarOuEditar(UnidadeDto input);
        //Task CriarOuEditar(CriarOuEditarUnidade input);

        Task Excluir(UnidadeDto input);
        //Task Excluir(CriarOuEditarUnidade input);

        UnidadeDto CriarGetId(UnidadeDto input);

        Task<UnidadeDto> Obter(long id);

        Task<IEnumerable<UnidadeDto>> ObterIds(List<long> ids);

        //Task<CriarOuEditarUnidade> Obter(long id);

        Task<UnidadeDto> ObterUnidadeDto(long id);

        Task<FileDto> ListarParaExcel(ListarUnidadesInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        decimal ObterQuantidadeReferencia(long unidadeId, decimal quantidade);

        decimal ObterQuantidadePorFator(long unidadeId, decimal quantidade);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarUnidadePorProduto2Dropdown(DropdownInput dropdownInput);

    }
}
