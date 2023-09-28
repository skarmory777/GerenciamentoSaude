using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.ModelosPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Linq;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Application.Services.Dto;
    using Abp.Auditing;
    using Abp.Domain.Uow;
    using Dapper;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Modelos.Dto;
    using SW10.SWMANAGER.Helpers;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    public class ModeloPrescricaoAppService : SWMANAGERAppServiceBase, IModeloPrescricaoAppService
    {
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var modeloPrescricaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloPrescricao, long>>())
            {
                return await this.CreateSelect2(modeloPrescricaoRepository.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ModelePrescricaoIndex>> Listar(ModeloPrescricaoListarInput input)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var query = DataTableHelper.BuildQueryWithTakeSkip("Id", "*", "AssModeloPrescricaoMedica", "IsDeleted = @deleted", input.Sorting);

                using (var queryMultiple = await connection.QueryMultipleAsync(query, new { deleted = false, skip = input.SkipCount, take = input.MaxResultCount }).ConfigureAwait(false))
                {
                    return new PagedResultDto<ModelePrescricaoIndex>(await queryMultiple.ReadFirstOrDefaultAsync<int>(), await queryMultiple.ReadFirstOrDefaultAsync<List<ModelePrescricaoIndex>>());
                }
            }
        }

        public async Task<ModeloPrescricaoDto> Obter(long id)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var query = @"SELECT AssModeloPrescricaoMedica.*,AssPrescricaoMedica.*,AteAtendimento.*  FROM AssModeloPrescricaoMedica  
                LEFT JOIN AssPrescricaoMedica 
                    ON AssPrescricaoMedica.Id = AssModeloPrescricaoMedica.PrescricaoMedicaId
                LEFT JOIN AteAtendimento
                    ON AssPrescricaoMedica.AteAtendimentoId = AteAtendimento.Id
                WHERE AssModeloPrescricaoMedica.Id = @modeloPrescricaoId";

                return (await connection.QueryAsync<ModeloPrescricaoDto, PrescricaoMedicaDto, AtendimentoDto, ModeloPrescricaoDto>(query,
                    (modeloPrescricaoDto, prescricaoMedicaDto, atendimentoDto) =>
                     {
                         if (modeloPrescricaoDto == null)
                         {
                             return null;
                         }

                         modeloPrescricaoDto.PrescricaoMedica = prescricaoMedicaDto;
                         if (modeloPrescricaoDto.PrescricaoMedica != null)
                         {
                             modeloPrescricaoDto.PrescricaoMedica.Atendimento = atendimentoDto;
                         }
                         return modeloPrescricaoDto;
                     }, new { modeloPrescricaoId = id }, commandTimeout: 0)).FirstOrDefault();
            }

            //using (var ModeloPrescricaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloPrescricao, long>>())
            //{
            //    var query = await ModeloPrescricaoRepository.Object
            //                                          .GetAll()
            //                                          .Include(i => i.PrescricaoMedica.Atendimento)
            //                                          .Where(w => w.Id == id)
            //                                          .FirstOrDefaultAsync();

            //    return ModeloPrescricaoDto.Mapear(query);
            //}
        }
    }
}
