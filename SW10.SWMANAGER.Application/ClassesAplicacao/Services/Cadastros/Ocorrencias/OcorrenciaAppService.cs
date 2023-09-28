using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias.Dto;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias
{
    public class OcorrenciaAppService : SWMANAGERAppServiceBase, IOcorrenciaAppService
    {
        public async Task<OcorrenciaDto> CriarOuEditar(OcorrenciaDto input)
        {
            if (input == null)
            {
                return input;
            }
            
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            {
                var entity = OcorrenciaDto.Mapear(input);
                input.Id = await ocorrenciaRepository.Object.InsertOrUpdateAndGetIdAsync(entity);
                return input;
            }
        }

        public async Task<PagedResultDto<OcorrenciaListaDto>> Listar(OcorrenciaListaFiltroDto input)
        {
            const string defaultField = "Ocorrencia.id";
            const string selectCaluse = @"
            Ocorrencia.id,
            Ocorrencia.Data,
            TipoOcorrencia.Descricao AS TipoOcorrenciaDescricao,
            SubTipoOcorrencia.Descricao AS SubTipoOcorrenciaDescricao,
            Ocorrencia.SourceModel AS Origem,
            Ocorrencia.SourceModel,
            Ocorrencia.SourceId,
            Ocorrencia.Texto,
            Ocorrencia.IsSistema
            ";
            const string fromClause = @"SisOcorrencias AS Ocorrencia 
            LEFT JOIN SisTipoOcorrencias AS TipoOcorrencia ON Ocorrencia.TipoOcorrenciaId = TipoOcorrencia.id
            LEFT JOIN SisSubTipoOcorrencias AS SubTipoOcorrencia ON Ocorrencia.SubTipoOcorrenciaId = SubTipoOcorrencia.id  ";
            const string whereClause = "Ocorrencia.IsDeleted = @isDeleted";
            
            return await this.CreateDataTable<OcorrenciaListaDto, OcorrenciaListaFiltroDto>()
                .AddDefaultField(defaultField)
                .AddSelectClause(selectCaluse)
                .AddFromClause(fromClause)
                .AddWhereClause(whereClause)
                .AddWhereMethod(ListarWhereMethod)
                .ExecuteAsync(input);
        }

        private static string ListarWhereMethod(OcorrenciaListaFiltroDto inputDto, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("isDeleted", false);
            var sourceModel = Ocorrencia.GetEntityModelById(inputDto.SourceModel);
            
            if (dapperParameters.ContainsKey("SourceModel"))
            {
                dapperParameters["SourceModel"] = sourceModel;
            }
            else
            {
                dapperParameters.Add("SourceModel", sourceModel);
            }

            if (inputDto.RelationModel.IsNullOrEmpty())
            {
                return "(sourceModel = @SourceModel AND sourceId = @SourceId )";
            }
            
            var relationModel = Ocorrencia.GetEntityModelById(inputDto.RelationModel);
            if (dapperParameters.ContainsKey("RelationModel"))
            {
                dapperParameters["RelationModel"] = relationModel;
            }
            else
            {
                dapperParameters.Add("RelationModel", relationModel);
            }
            return "((sourceModel = @SourceModel AND sourceId = @SourceId ) OR (relationModel = @RelationModel AND relationId = @RelationId))";
        }
    }
}