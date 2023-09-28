using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta
{
    public class MotivoAltaAppService : SWMANAGERAppServiceBase, IMotivoAltaAppService
    {
        [UnitOfWork]
        public async Task CriarOuEditar(CriarOuEditarMotivoAlta input)
        {
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAlta, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var MotivoAlta = CriarOuEditarMotivoAlta.Mapear(input);

                    using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                    {
                        if (input.Id.Equals(0))
                        {
                            await _motivoAltaRepository.Object.InsertAsync(MotivoAlta);
                        }
                        else
                        {
                            await _motivoAltaRepository.Object.UpdateAsync(MotivoAlta);
                        }
                        unitOfWork.Complete();
                        _unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarMotivoAlta input)
        {
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAlta, long>>())
                    await _motivoAltaRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<MotivoAltaDto>> Listar(ListarMotivosAltaInput input)
        {
            var contarMotivosAlta = 0;
            List<MotivoAlta> motivosAlta;
            List<MotivoAltaDto> motivosAltaDtos = new List<MotivoAltaDto>();
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAlta, long>>())
                {
                    var query = _motivoAltaRepository.Object
                    .GetAllIncluding(
                        m => m.MotivoAltaTipoAlta
                    )
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

                    motivosAltaDtos = MotivoAltaDto.Mapear(motivosAlta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<MotivoAltaDto>(
                contarMotivosAlta,
                motivosAltaDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarMotivosAltaInput input)
        {
            try
            {
                using (var _listarMotivosAltaExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarMotivosAltaExcelExporter>())
                {
                    var result = await Listar(input);
                    var motivosAlta = result.Items;
                    return _listarMotivosAltaExcelExporter.Object.ExportToFile(motivosAlta.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarMotivoAlta> Obter(long id)
        {
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAlta, long>>())
                {
                    var query = await _motivoAltaRepository.Object
                    .GetAll()
                    .Include(m => m.MotivoAltaTipoAlta)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var motivoAlta = CriarOuEditarMotivoAlta.Mapear(query);

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

            List<MotivoAltaDto> modalidadeDto = new List<MotivoAltaDto>();
            try
            {
                using (var _motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAlta, long>>())
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
