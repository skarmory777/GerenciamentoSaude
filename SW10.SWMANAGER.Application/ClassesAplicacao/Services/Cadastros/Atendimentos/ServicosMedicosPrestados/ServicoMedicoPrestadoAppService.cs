using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados
{
    public class ServicoMedicoPrestadoAppService : SWMANAGERAppServiceBase, IServicoMedicoPrestadoAppService
    {
        public async Task CriarOuEditar(CriarOuEditarServicoMedicoPrestado input)
        {
            try
            {
                using (var _servicoMedicoPrestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ServicoMedicoPrestado, long>>())
                {
                    var servicoMedicoPrestado = CriarOuEditarServicoMedicoPrestado.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _servicoMedicoPrestadoRepository.Object.InsertAsync(servicoMedicoPrestado);
                    }
                    else
                    {
                        await _servicoMedicoPrestadoRepository.Object.UpdateAsync(servicoMedicoPrestado);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarServicoMedicoPrestado input)
        {
            try
            {
                using (var _servicoMedicoPrestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ServicoMedicoPrestado, long>>())
                    await _servicoMedicoPrestadoRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ServicoMedicoPrestadoDto>> Listar(ListarServicosMedicosPrestadosInput input)
        {
            var contarServicosMedicosPrestados = 0;
            List<ServicoMedicoPrestado> servicosMedicosPrestados;
            List<ServicoMedicoPrestadoDto> servicosMedicosPrestadosDtos = new List<ServicoMedicoPrestadoDto>();
            try
            {
                using (var _servicoMedicoPrestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ServicoMedicoPrestado, long>>())
                {
                    var query = _servicoMedicoPrestadoRepository.Object
                    .GetAll()
                    .Include(m => m.Especialidade)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.Descricao.Contains(input.Filtro)
                    );

                    contarServicosMedicosPrestados = await query
                        .CountAsync();

                    servicosMedicosPrestados = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    servicosMedicosPrestadosDtos = ServicoMedicoPrestadoDto.Mapear(servicosMedicosPrestados);



                    return new PagedResultDto<ServicoMedicoPrestadoDto>(
                    contarServicosMedicosPrestados,
                    servicosMedicosPrestadosDtos
                    );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ServicoMedicoPrestadoDto>> ListarTodos()
        {
            try
            {
                using (var _servicoMedicoPrestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ServicoMedicoPrestado, long>>())
                {
                    var servicosMedicosPrestados = await _servicoMedicoPrestadoRepository.Object
                    .GetAll()
                    .Include(m => m.Especialidade)
                    .AsNoTracking()
                    .ToListAsync();

                    var servicosMedicosPrestadosDtos = ServicoMedicoPrestadoDto.Mapear(servicosMedicosPrestados);


                    return new ListResultDto<ServicoMedicoPrestadoDto> { Items = servicosMedicosPrestadosDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<CriarOuEditarServicoMedicoPrestado> Obter(long id)
        {
            try
            {
                using (var _servicoMedicoPrestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ServicoMedicoPrestado, long>>())
                {
                    var query = await _servicoMedicoPrestadoRepository.Object
                    .GetAll()
                    .Include(m => m.Especialidade)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var servicoMedicoPrestado = CriarOuEditarServicoMedicoPrestado.Mapear(query);


                    return servicoMedicoPrestado;
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
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var _servicoMedicoPrestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ServicoMedicoPrestado, long>>())
                {
                    //get com filtro
                    var query = from p in _servicoMedicoPrestadoRepository.Object.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Descricao.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                        .Replace("à", "a").Replace("è", "e").Replace("ì", "i").Replace("ò", "o").Replace("ù", "u")
                        .Replace("â", "a").Replace("ê", "e").Replace("î", "i").Replace("ô", "o").Replace("û", "u")
                        .Replace("ã", "a").Replace("õ", "o")
                        .Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U")
                        .Replace("À", "A").Replace("È", "E").Replace("Ì", "I").Replace("Ô", "O").Replace("Ù", "U")
                        .Replace("Â", "A").Replace("Ê", "E").Replace("Î", "I").Replace("Õ", "O").Replace("Û", "U")
                        .Replace("Ã", "A").Replace("Õ", "O")
                        .Contains(dropdownInput.search.ToLower())
                        )
                                orderby p.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                    //paginação 
                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync();

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
