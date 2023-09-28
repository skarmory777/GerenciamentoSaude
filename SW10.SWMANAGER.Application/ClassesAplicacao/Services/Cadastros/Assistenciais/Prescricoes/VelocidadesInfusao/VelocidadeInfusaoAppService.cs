using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFramework.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao
{
    public class VelocidadeInfusaoAppService : SWMANAGERAppServiceBase, IVelocidadeInfusaoAppService
    {
        [UnitOfWork]
        public async Task<VelocidadeInfusaoDto> CriarOuEditar(VelocidadeInfusaoDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var velocidadeInfusaoFormaAplicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusaoFormaAplicacao, long>>())
                using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                {
                    var velocidadeInfusao = VelocidadeInfusaoDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _velocidadeInfusaoRepositorio.Object.InsertAndGetIdAsync(velocidadeInfusao).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {

                            var velocidadeInfusaoFormaAplicacao = velocidadeInfusaoFormaAplicacaoRepository.Object.GetAll().Where(x => x.VelocidadeInfusaoId == velocidadeInfusao.Id);

                            foreach (var item in velocidadeInfusao.FormaAplicacao)
                            {
                                _velocidadeInfusaoRepositorio.Object.GetDbContext().Entry(item).State = item.IsTransient() ? EntityState.Added : EntityState.Modified;
                            }

                            var velocidadeInfusaoFormaAplicacaoIds = velocidadeInfusaoFormaAplicacao.ToList().Select(x => x.Id);
                            var idsForm = input.FormaAplicacao.ToList().Select(x => x.Id);
                            foreach (var itemId in velocidadeInfusaoFormaAplicacaoIds.Where(x => !idsForm.Contains(x)))
                            {
                                _velocidadeInfusaoRepositorio.Object.GetDbContext().Entry(velocidadeInfusaoFormaAplicacao.First(x => x.Id == itemId)).State = EntityState.Deleted;
                            }

                            velocidadeInfusao.Id = await _velocidadeInfusaoRepositorio.Object.InsertOrUpdateAndGetIdAsync(velocidadeInfusao).ConfigureAwait(false);

                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(VelocidadeInfusaoDto input)
        {
            try
            {
                using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _velocidadeInfusaoRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<VelocidadeInfusaoDto>> Listar(ListarInput input)
        {
            var contarVelocidadeInfusao = 0;
            try
            {
                using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                {
                    var query = _velocidadeInfusaoRepositorio.Object
                    .GetAll().AsNoTracking()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    contarVelocidadeInfusao = await query
                                                  .CountAsync().ConfigureAwait(false);

                    var velocidadeInfusao = await query
                                                .OrderBy(input.Sorting)
                                                .PageBy(input)
                                                .ToListAsync().ConfigureAwait(false);

                    return new PagedResultDto<VelocidadeInfusaoDto>(contarVelocidadeInfusao, VelocidadeInfusaoDto.Mapear(velocidadeInfusao).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<VelocidadeInfusaoDto> Obter(long id)
        {
            try
            {
                using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                {
                    var result = await _velocidadeInfusaoRepositorio.Object
                                 .GetAll().AsNoTracking().Include(x => x.FormaAplicacao)
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    return VelocidadeInfusaoDto.Mapear(result);
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<VelocidadeInfusaoDto>> ListarTodos()
        {
            try
            {
                using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                {
                    var query = _velocidadeInfusaoRepositorio.Object
                    .GetAll().AsNoTracking()
                    .OrderBy(m => m.Codigo);

                    var velocidadeInfusao = await query
                                                .ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<VelocidadeInfusaoDto>
                    {
                        Items = VelocidadeInfusaoDto.Mapear(velocidadeInfusao).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<VelocidadeInfusaoDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                {
                    var query = _velocidadeInfusaoRepositorio.Object
                    .GetAll().AsNoTracking()
                    .WhereIf(!filtro.IsNullOrEmpty(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                    var velocidadeInfusao = await query
                                                .ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<VelocidadeInfusaoDto>
                    {
                        Items = VelocidadeInfusaoDto.Mapear(velocidadeInfusao).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput)
        {
            using (var _velocidadeInfusaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                return await this.CreateSelect2(_velocidadeInfusaoRepositorio.Object).AddTextField("Descricao").AddOrderByClause("Descricao")
                .AddWhereMethod((select2Input, dapperParameters) =>
                {

                    var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(select2Input, dapperParameters));

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
                        var id = 0;
                        int.TryParse(dropdownInput.id, out id);
                        dapperParameters["id"] = id;
                        whereBuilder.Append(" AND  id = @id");
                        return whereBuilder.ToString();
                    }
                    return whereBuilder.ToString();
                })
                .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
