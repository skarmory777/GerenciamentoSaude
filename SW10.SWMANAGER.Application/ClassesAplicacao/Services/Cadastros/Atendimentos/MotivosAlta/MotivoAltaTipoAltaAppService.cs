using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta
{
    public class MotivoAltaTipoAltaAppService : SWMANAGERAppServiceBase, IMotivoAltaTipoAltaAppService
    {
        public async Task CriarOuEditar(CriarOuEditarMotivoAltaTipoAlta input)
        {
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAltaTipoAlta, long>>())
                {

                    var MotivoAltaTipoAlta = CriarOuEditarMotivoAltaTipoAlta.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _motivoAltaRepository.Object.InsertAsync(MotivoAltaTipoAlta);
                    }
                    else
                    {
                        await _motivoAltaRepository.Object.UpdateAsync(MotivoAltaTipoAlta);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarMotivoAltaTipoAlta input)
        {
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAltaTipoAlta, long>>())
                    await _motivoAltaRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<MotivoAltaTipoAltaDto>> Listar(ListarMotivosAltaInput input)
        {
            var contarMotivosAlta = 0;
            List<MotivoAltaTipoAlta> motivosAlta;
            List<MotivoAltaTipoAltaDto> motivosAltaDtos = new List<MotivoAltaTipoAltaDto>();
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAltaTipoAlta, long>>())
                {
                    var query = _motivoAltaRepository.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                    contarMotivosAlta = await query
                        .CountAsync();

                    motivosAlta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    motivosAltaDtos = MotivoAltaTipoAltaDto.Mapear(motivosAlta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<MotivoAltaTipoAltaDto>(
                contarMotivosAlta,
                motivosAltaDtos
                );
        }


        public async Task<CriarOuEditarMotivoAltaTipoAlta> Obter(long id)
        {
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAltaTipoAlta, long>>())
                {
                    var query = await _motivoAltaRepository.Object.GetAsync(id);

                    var motivoAlta = CriarOuEditarMotivoAltaTipoAlta.Mapear(query);

                    return motivoAlta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<MotivoAltaTipoAltaDto> motivoaltatipoaltadto = new List<MotivoAltaTipoAltaDto>();
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAltaTipoAlta, long>>())
                {
                    if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                    {
                        throw new Exception("NotANumber");
                    }

                    var query = from p in _motivoAltaRepository.Object.GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                )
                                    //.Where(f => f.IsLaudo.Equals(isLaudo))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var result = queryResultPage.ToList();

                    int total = await Task.Run(() => query.Count());

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
