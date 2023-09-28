using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.Core.Internal;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.ClassesAplicacao.Avisos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto;
using SW10.SWMANAGER.Helper;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Avisos
{
    public class AvisosAppService : SWMANAGERAppServiceBase, IAvisosAppService
    {
        public async Task<PagedResultDto<AvisoDto>> Listar(IndexFiltroAvisoViewModel input)
        {
            const string defaultField = "Id";
            const string selectPart = defaultField + @", 
                [Aviso].[DataProgramada],
                [Aviso].[DataInicioDisparo],
                [Aviso].[DataFinalDisparo],
                [Aviso].[Titulo],
                [Aviso].[Mensagem],
                [Aviso].[Bloquear],
                [Aviso].[TotalEnviado]";
            const string fromPart = "SisAvisos AS Aviso";
            const string wherePart = "AND Aviso.IsDeleted = @deleted";
            return await this.CreateDataTable<AvisoDto, IndexFiltroAvisoViewModel>()
                .AddDefaultField(defaultField)
                .AddSelectClause(selectPart)
                .AddFromClause(fromPart)
                .AddWhereMethod((filtro, dapperParameters) =>
                {
                    dapperParameters.Add("deleted", false);
                    dapperParameters.Add("isSistema", false);
                    return wherePart;
                }).ExecuteAsync(input);
        }

        public async Task<AvisoDto> CriarOuEditar(AvisoDto input)
        {
            try
            {
                using (var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
                using (var avisoGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AvisoGrupo, long>>())
                {
                    var model = AvisoDto.Mapear(input);
                    model.Grupos = null;
                    if (!model.DataProgramada.HasValue)
                    {
                        model.DataProgramada = DateTime.Now;
                    }
                    model.DataInicioDisparo = model.DataInicioDisparo.ToNullIfTooEarlyForDb();
                    model.DataFinalDisparo = model.DataFinalDisparo.ToNullIfTooEarlyForDb();
                    if (!model.IsTransient())
                    {
                        await avisoGrupoRepository.Object.DeleteAsync(x=> x.AvisoId == input.Id);
                    }
                    input.Id = await avisoRepository.Object.InsertOrUpdateAndGetIdAsync(model).ConfigureAwait(false);
                    if (!input.Grupos.IsNullOrEmpty())
                    {
                        var grupos = AvisoGrupoDto.MapearList(input.Grupos);
                        foreach (var grupo in grupos)
                        {
                            grupo.AvisoId = input.Id;
                            await avisoGrupoRepository.Object.InsertOrUpdateAndGetIdAsync(grupo).ConfigureAwait(false);
                        }
                    }

                    return input;
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public AvisoDto Obter(long id)
        {
            using (var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
            {
                var aviso = avisoRepository.Object.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == id);
                return AvisoDto.Mapear(aviso);
            }
        }

        public void Excluir(long id)
        {
            using (var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
            {
                avisoRepository.Object.Delete(id);
            }
        }
    }
}