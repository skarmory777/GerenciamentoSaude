using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias
{
    public interface IGuiaCampoAppService : IApplicationService
    {
        Task<PagedResultDto<GuiaCampoDto>> Listar(ListarGuiaCamposInput input);

        PagedResultDto<GuiaCampoDto> ListarParaConjunto(long conjuntoId);

        //Task<CriarOuEditarGuiaCampo> CriarOuEditar(CriarOuEditarGuiaCampo input);
        Task<KeyValuePair<CriarOuEditarGuiaCampo, bool>> CriarOuEditar(CriarOuEditarGuiaCampo input);

        Task Excluir(CriarOuEditarGuiaCampo input);

        Task<CriarOuEditarGuiaCampo> Obter(long id);

        GuiaCampoDto ObterDto(long id);
    }
}
