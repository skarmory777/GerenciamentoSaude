using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using Abp.Dependency;

    public class FormConfigEspecialidadeAppService : SWMANAGERAppServiceBase, IFormConfigEspecialidadeAppService
    {
        [UnitOfWork]
        public async Task CriarOuEditar(FormConfigEspecialidadeDto input)
        {
            try
            {
                using (var formConfigEspecialidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var especialidadesIncluidas = string.IsNullOrWhiteSpace(input.EspecialidadesIncluidas)
                                                      ? new string[0]
                                                      : input.EspecialidadesIncluidas.Split(',');
                    var especialidadesRemovidas = string.IsNullOrWhiteSpace(input.EspecialidadesRemovidas)
                                                      ? new string[0]
                                                      : input.EspecialidadesRemovidas.Split(',');
                    foreach (var especialidadeId in especialidadesIncluidas)
                    {
                        var formConfigEspecialidade = new FormConfigEspecialidade
                        {
                            FormConfigId = input.FormConfigId,
                            IsDeleted = false,
                            IsSistema = false,
                            CreatorUserId = AbpSession.UserId,
                            EspecialidadeId = Convert.ToInt64(especialidadeId)
                        };
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await formConfigEspecialidadeRepository.Object.InsertAsync(formConfigEspecialidade).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    foreach (var especialidadeId in especialidadesRemovidas)
                    {
                        var formConfigEspecialidade = await this.Obter(
                                                          input.FormConfigId.Value,
                                                          Convert.ToInt64(especialidadeId)).ConfigureAwait(false);

                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await formConfigEspecialidadeRepository.Object.DeleteAsync(formConfigEspecialidade.Id).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(FormConfigEspecialidadeDto input)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    await formConfigEspecialidadeRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<FormConfigEspecialidadeDto>> Listar(ListarInput input)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.FormConfig.Nome.Contains(input.Filtro)
                                 || m.Especialidade.Codigo.Contains(input.Filtro)
                                 || m.Especialidade.Descricao.Contains(input.Filtro) || m.FormConfig.DataAlteracao
                                     .ToShortTimeString().Contains(input.Filtro));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig =
                        await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigEspecialidadeDto>>();

                    return new PagedResultDto<FormConfigEspecialidadeDto>(
                        contarGeradorFormularios,
                        //formsConfigResult
                        formsConfigDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigEspecialidadeDto>> ListarTodos()
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade);


                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigEspecialidadeDto>>();

                    //var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigEspecialidadeDto> { Items = formsConfigDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigEspecialidadeDto> Obter(long id)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var formConfig = new FormConfigEspecialidade();
                    var formConfigDto = new FormConfigEspecialidadeDto();
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).Where(m => m.Id == id);

                    formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    formConfigDto = formConfig.MapTo<FormConfigEspecialidadeDto>();

                    return formConfigDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var query = from p in formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking()
                                    .Include(m => m.FormConfig).Include(m => m.Especialidade)
                                    .WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.FormConfig.Codigo.Contains(dropdownInput.search)
                                             || m.FormConfig.Nome.Contains(dropdownInput.search))
                                orderby p.FormConfig.Nome ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(
                                                   p.FormConfig.Codigo,
                                                   " - ",
                                                   p.FormConfig.Nome,
                                                   " / ",
                                                   p.Especialidade.Descricao)
                                };

                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigEspecialidadeDto>> ListarPorForm(long id)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).Where(m => m.FormConfigId == id);


                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigEspecialidadeDto>>();

                    //var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigEspecialidadeDto> { Items = formsConfigDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigEspecialidadeDto>> ListarPorEspecialidade(long id)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).Where(m => m.EspecialidadeId == id);


                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigEspecialidadeDto>>();

                    //var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigEspecialidadeDto> { Items = formsConfigDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigEspecialidadeDto> Obter(long formConfigId, long especialidadeId)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                {
                    var formConfig = new FormConfigEspecialidade();
                    var formConfigDto = new FormConfigEspecialidadeDto();
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).Where(
                            m => m.FormConfigId == formConfigId && m.EspecialidadeId == especialidadeId);

                    formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    formConfigDto = formConfig.MapTo<FormConfigEspecialidadeDto>();

                    return formConfigDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EspecialidadeDto>> ListarEspecialidadePorForm(ListarInput input)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                using (var especialidadeService =
                    IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
                {
                    long id = 0;
                    var result = long.TryParse(input.PrincipalId, out id);
                    if (!result)
                    {
                        throw new UserFriendlyException(L("IdNaoInformado"));
                    }

                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).Where(m => m.FormConfigId == id).WhereIf(
                            !input.Filtro.IsNullOrWhiteSpace(),
                            m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro))
                        .Select(m => m.Especialidade);

                    var query1 = await query.ToListAsync().ConfigureAwait(false);
                    var unidades = query1.Select(m => m.Id).ToArray();

                    //var unidades = associadas.Select(m => m.Id).ToArray();
                    var disponiveis = await especialidadeService.Object.ListarTodos().ConfigureAwait(false);
                    var especialidades = disponiveis.Items.Where(m => m.Id.IsIn(unidades)).AsQueryable();

                    var count = especialidades.Count();

                    var especialidadesDto = especialidades.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToList();
                    return new PagedResultDto<EspecialidadeDto> { TotalCount = count, Items = especialidadesDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EspecialidadeDto>> ListarEspecialidadeSemForm(ListarInput input)
        {
            try
            {
                using (var formConfigEspecialidadeRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigEspecialidade, long>>())
                using (var especialidadeService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
                {
                    long id = 0;
                    var result = long.TryParse(input.PrincipalId, out id);
                    if (!result)
                    {
                        throw new UserFriendlyException(L("IdNaoInformado"));
                    }

                    //var associadas = await ListarEspecialidadePorForm(input);
                    var query = formConfigEspecialidadeRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Especialidade).Where(m => m.FormConfigId == id).Select(m => m.Especialidade);

                    var associadas = await query.ToListAsync().ConfigureAwait(false);

                    var unidades = associadas.Select(m => m.Id).ToArray();

                    var disponiveis = await especialidadeService.Object.ListarTodos().ConfigureAwait(false);

                    var unidadesDisponiveis = disponiveis.Items.Where(m => !m.Id.IsIn(unidades)).ToList();

                    var unidadesFiltradas = unidadesDisponiveis.WhereIf(
                        !input.Filtro.IsNullOrWhiteSpace(),
                        m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro)).AsQueryable();

                    var count = unidadesFiltradas.Count();
                    var unidadesDto = unidadesFiltradas.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToList()
                        .MapTo<List<EspecialidadeDto>>();

                    return new PagedResultDto<EspecialidadeDto> { TotalCount = count, Items = unidadesDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
