using Abp.Application.Services.Dto;
using Abp.Auditing;
//using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Helper;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class TipoMovimentoAppService : SWMANAGERAppServiceBase, ITipoMovimentoAppService
    {
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<TipoMovimentoDto>> Listar(bool isEntrada)
        {
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                var query = tipoMovimentoRepository.Object.GetAll().Where(w => w.IsEntrada == isEntrada);

                var contarTipoMovimentos = await query.CountAsync();

                var tipoMovimentos = await query.AsNoTracking().ToListAsync();

                var tiposDto = TipoMovimentoDto.Mapear(tipoMovimentos);

                return new PagedResultDto<TipoMovimentoDto>(contarTipoMovimentos, tiposDto);
            }

        }

        public async Task<PagedResultDto<TipoMovimentoDto>> ListarTodos()
        {
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                var query = tipoMovimentoRepository.Object.GetAll();

                var contarTipoMovimentos = await query.CountAsync();

                var tipoMovimentos = await query.AsNoTracking().ToListAsync();

                var tiposDto = TipoMovimentoDto.Mapear(tipoMovimentos);

                return new PagedResultDto<TipoMovimentoDto>(contarTipoMovimentos, tiposDto);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarPorEntradaOuSaida(DropdownInput input)
        {
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                return await Select2Helper
                    .CreateSelect2(this, tipoMovimentoRepository.Object)
                    .AddTextField(" Descricao ")
                    .AddWhereMethod((dto, dapperParameters) =>
                    {
                        var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(dto, dapperParameters));

                        if (!dto.filtro.IsNullOrEmpty())
                        {
                            whereBuilder.Append(" AND IsEntrada = @Entrada ");

                            dapperParameters.Add("Entrada", FormatterHelper.ParseBoolean(dto.filtro));
                        }
                        return whereBuilder.ToString();
                    }).ExecuteAsync<long>(input).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<TipoMovimentoDto>> ListarTipoMovimentoDevolucao()
        {
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                var query = tipoMovimentoRepository.Object.GetAll()
                  .Where(w => w.Id == (long)EnumTipoMovimento.GastoSala_Saida
                           || w.Id == (long)EnumTipoMovimento.Paciente_Saida
                           || w.Id == (long)EnumTipoMovimento.Setor_Saida);

                var contarTipoMovimentos = await query.CountAsync();


                var tipoMovimentos = await query.AsNoTracking().ToListAsync();

                var tiposMovimentosDto = TipoMovimentoDto.Mapear(tipoMovimentos);

                return new PagedResultDto<TipoMovimentoDto>(contarTipoMovimentos, tiposMovimentosDto);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdownEntrada(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<TipoMovimentoDto> tipoMovimentoDto = new List<TipoMovimentoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }
                using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
                {
                    bool isEntrada = true;

                    var query = from p in tipoMovimentoRepository.Object.GetAll()
                  .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.ToString().Contains(dropdownInput.search)).Where(x => x.IsEntrada.Equals(isEntrada))
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

                    int total = result.Count();

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdownSaida(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<TipoMovimentoDto> tipoMovimentoDto = new List<TipoMovimentoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isEntrada = false;
                using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
                {
                    var query = from p in tipoMovimentoRepository.Object.GetAll()
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
                                .Where(x => x.IsEntrada.Equals(isEntrada))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var result = queryResultPage.ToList();

                    //  int total = await query.CountAsync();

                    int total = result.Count();
                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdownSolicitacaoSaida(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<TipoMovimentoDto> tipoMovimentoDto = new List<TipoMovimentoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
                {
                    var query = from p in tipoMovimentoRepository.Object.GetAll()
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
                                .Where(x => x.IsEntrada.Equals(false) && !x.Id.Equals((long)EnumTipoMovimento.Emprestimo_Saida))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);
                    var result = queryResultPage.ToList();
                    int total = result.Count();

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdownDevolucao(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<TipoMovimentoDto> tipoMovimentoDto = new List<TipoMovimentoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isEntrada = false;
                using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
                {
                    var query = from p in tipoMovimentoRepository.Object.GetAll()
                      .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
                      .Where(x => x.Id == 2 || x.Id == 3 || x.Id == 4 || x.Id == 5)
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var result = queryResultPage.ToList();

                    //  int total = await query.CountAsync();

                    int total = result.Count();

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
