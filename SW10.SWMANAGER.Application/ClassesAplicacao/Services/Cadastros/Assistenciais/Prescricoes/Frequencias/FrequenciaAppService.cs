using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias
{
    public class FrequenciaAppService : SWMANAGERAppServiceBase, IFrequenciaAppService
    {
        [UnitOfWork]
        public async Task<FrequenciaDto> CriarOuEditar(FrequenciaDto input)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var frequencia = FrequenciaDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await frequenciaRepositorio.Object.InsertAndGetIdAsync(frequencia).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            frequencia = await frequenciaRepositorio.Object.UpdateAsync(frequencia).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(FrequenciaDto input)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await frequenciaRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FrequenciaDto>> Listar(ListarInput input)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                {
                    var query = frequenciaRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contarFrequencia = await query.CountAsync().ConfigureAwait(false);

                    var items = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);
                    var itensDto = FrequenciaDto.Mapear(items).ToList();

                    return new PagedResultDto<FrequenciaDto>(contarFrequencia, itensDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<FrequenciaDto> Obter(long id)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                {
                    var result = await frequenciaRepositorio.Object
                                 .GetAll()
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    var frequencia = FrequenciaDto.Mapear(result);

                    return frequencia;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public FrequenciaDto ObterSync(long id)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                {
                    var result = frequenciaRepositorio.Object
                                 .GetAll()
                                 .Where(m => m.Id == id)
                                 .FirstOrDefault();
                    var frequencia = FrequenciaDto.Mapear(result);

                    return frequencia;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IEnumerable<FrequenciaDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<Frequencia>(tableAlias: "Frequencia").GetFields()}
                    FROM AssFrequencia AS Frequencia
                    WHERE 
                        Frequencia.Id IN @ids
                        AND Frequencia.IsDeleted = 0
                    ";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync<FrequenciaDto>(query, new { ids = ids.Distinct().ToList() });
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<FrequenciaDto> Obter(string frequencia)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                {
                    var query = frequenciaRepositorio.Object.GetAll().WhereIf(!frequencia.IsNullOrWhiteSpace(), m => m.Codigo.Contains(frequencia) || m.Descricao.Contains(frequencia));
                    var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                    return FrequenciaDto.Mapear(result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FrequenciaDto>> ListarTodos()
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                {
                    var query = frequenciaRepositorio.Object
                    .GetAll();

                    var frequencia = await query
                                         .AsNoTracking()
                                         .ToListAsync().ConfigureAwait(false);

                    var frequenciasDto = FrequenciaDto.Mapear(frequencia).ToList();

                    return new ListResultDto<FrequenciaDto>
                    {
                        Items = frequenciasDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FrequenciaDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                {
                    var query = frequenciaRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                    var frequencia = await query
                                         .AsNoTracking()
                                         .ToListAsync().ConfigureAwait(false);

                    var frequenciasDto = FrequenciaDto.Mapear(frequencia).ToList();

                    return new ListResultDto<FrequenciaDto>
                    {
                        Items = frequenciasDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput)
        {
            using (var frequenciaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
            {
                return await this.CreateSelect2(frequenciaRepositorio.Object).AddTextField("Descricao").AddOrderByClause("Descricao")
                .AddWhereMethod((select2Input, dapperParameters) =>
                {
                    var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(select2Input,dapperParameters));
                    var ids = new List<long>();

                    if (!dropdownInput.JsonFilter.IsNullOrEmpty())
                    {
                        var jsonFilter = JsonConvert.DeserializeObject<ConfiguracaoPrescricaoItemJsonFilter>(dropdownInput.JsonFilter);
                        dapperParameters.Add("options", jsonFilter.Options);


                        if (!jsonFilter.Options.IsNullOrEmpty())
                        {
                            ids = jsonFilter.Options.ToList();

                            if (!jsonFilter.Id.IsNullOrEmpty())
                            {
                                ids = jsonFilter.Id.ToList();
                            }
                            whereBuilder.Append(" AND Id IN @id ");

                            dapperParameters["id"] = ids;
                        }
                        else if (!jsonFilter.Id.IsNullOrEmpty())
                        {
                            dapperParameters["id"] = jsonFilter.Id;
                            whereBuilder.Append(" AND Id IN @id ");
                        }

                        if (!jsonFilter.Id.IsNullOrEmpty())
                        {
                            whereBuilder.Append(" AND  id IN @id");
                        }
                        return whereBuilder.ToString();
                    }

                    if (!dropdownInput.id.IsNullOrEmpty())
                    {
                        int.TryParse(dropdownInput.id, out int id);
                        dapperParameters["id"] = id;
                        whereBuilder.Append(" AND  id = @id");
                        return whereBuilder.ToString();
                    }
                    return whereBuilder.ToString();
                })
                .ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [UnitOfWork(false)]
        public Task<FileDto> ListarParaExcel(ListarInput input)
        {
            throw new NotImplementedException();
        }
    }
}
