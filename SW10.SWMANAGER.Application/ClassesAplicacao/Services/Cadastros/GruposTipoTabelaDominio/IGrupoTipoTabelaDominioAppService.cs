using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio
{
    public interface IGrupoTipoTabelaDominioAppService : IApplicationService
    {
        //ListResultDto<GrupoTipoTabelaDominioDto> GetGruposTipoTabelaDominio(GetGruposTipoTabelaDominioInput input);
        Task<PagedResultDto<GrupoTipoTabelaDominioDto>> Listar(ListarGruposTipoTabelaDominioInput input);

        //Task<ListResultDto<GrupoTipoTabelaDominio>> ListarFull();

        Task<PagedResultDto<GrupoTipoTabelaDominioDto>> ListarFull();

        Task CriarOuEditar(CriarOuEditarGrupoTipoTabelaDominio input);

        Task Excluir(CriarOuEditarGrupoTipoTabelaDominio input);

        Task<CriarOuEditarGrupoTipoTabelaDominio> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarGruposTipoTabelaDominioInput input);

        Task<ListResultDto<GrupoTipoTabelaDominioDto>> ListarPorTipo(long tipoTabelaId);

        Task<ICollection<GrupoTipoTabelaDominio>> ListarParaTabelaDominio(long? tabelaDominioId);
    }
}
