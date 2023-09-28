using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    using Abp.Application.Services.Dto;
    using Abp.Auditing;
    using Abp.AutoMapper;
    using Abp.Dependency;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;
    using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Exporting;
    using SW10.SWMANAGER.Dto;
    using SW10.SWMANAGER.Helpers;
    using System;

    public class EstoqueKitAppService : SWMANAGERAppServiceBase, IEstoqueKitAppService
    {
        private readonly IRepository<EstoqueKit, long> _estoqueKitRepository;
        private readonly IListarEstoqueKitExcelExporter _listarEstoqueKitExcelExporter;

        public EstoqueKitAppService(
            IRepository<EstoqueKit, long> estoqueKitRepository,
            IListarEstoqueKitExcelExporter listarEstoqueKitExcelExporter
            )
        {
            _estoqueKitRepository = estoqueKitRepository;
            _listarEstoqueKitExcelExporter = listarEstoqueKitExcelExporter;
        }

        public EstoqueKitDto Obter(long id)
        {
            using (var estoqueKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKit, long>>())
            {
                var estoqueKit = estoqueKitRepository.Object.GetAll().AsNoTracking()
                                                      .Include(i => i.Itens)
                                                      .Include(i => i.Itens.Select(s => s.EstoqueKit))
                                                      .Include(i => i.Itens.Select(s => s.Produto))
                                                      .Where(w => w.Id == id)
                                                      .FirstOrDefault();

                var estoqueKitDto = EstoqueKitDto.Mapear(estoqueKit);

                estoqueKitDto.ItensDto = new List<EstoqueKitItemDto>();

                foreach (var item in estoqueKit.Itens)
                {
                    estoqueKitDto.ItensDto.Add(EstoqueKitItemDto.Mapear(item));
                }
                return estoqueKitDto;
            }
        }

        public async Task Excluir(EstoqueKitDto input)
        {
            try
            {
                await _estoqueKitRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public EstoqueKitDto ObterPeloId(long id)
        {
            using (var estoqueKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKit, long>>())
            {
                var estoqueKit = estoqueKitRepository.Object.GetAll().AsNoTracking().Where(w => w.Id == id).FirstOrDefault();

                var estoqueKitDto = EstoqueKitDto.Mapear(estoqueKit);

                return estoqueKitDto;
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEstoqueKitInput input)
        {
            try
            {
                var result = await Listar(input);
                var itens = result.Items;
                return _listarEstoqueKitExcelExporter.ExportToFile(itens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<PagedResultDto<EstoqueKitDto>> Listar(ListarEstoqueKitInput input)
        {
            var itemrKits = 0;
            List<EstoqueKit> itens;
            List<EstoqueKitDto> itensDtos = new List<EstoqueKitDto>();
            try
            {
                var query = _estoqueKitRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                itemrKits = await query
                    .CountAsync();

                itensDtos = (await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync())
                    .Select(s => EstoqueKitDto.Mapear(s))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<EstoqueKitDto>(
                itemrKits,
                itensDtos
                );
        }

        public async Task<ListResultDto<EstoqueKitDto>> ListarTodos()
        {
            List<EstoqueKit> estoqueKit;
            List<EstoqueKitDto> estoqueKitDtos = new List<EstoqueKitDto>();
            try
            {
                estoqueKit = await _estoqueKitRepository.GetAll().ToListAsync();

                estoqueKitDtos = estoqueKit
                    .MapTo<List<EstoqueKitDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<EstoqueKitDto> { Items = estoqueKitDtos };
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var estoqueKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKit, long>>())
            {
                return await this.CreateSelect2(estoqueKitRepository.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        public List<EstoqueKitItemDto> ObterItensKit(long id)
        {
            using (var estoqueKitItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKitItem, long>>())
            {
                var itens = estoqueKitItemRepository.Object.GetAll().AsNoTracking()
                                                 .Include(i => i.Produto)
                                                 .Include(i => i.EstoqueKit)
                                                 .Include(i => i.Unidade)
                                                 .Where(w => w.EstoqueKitId == id)
                                                 .ToList();

                return EstoqueKitItemDto.Mapear(itens);
            }
        }

        public async Task<long?> CriarOuEditar(EstoqueKitDto input)
        {
            try
            {
                var kit = EstoqueKitDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    return await _estoqueKitRepository.InsertAndGetIdAsync(kit);
                }
                else
                {
                    var estoqueKit = await _estoqueKitRepository.UpdateAsync(kit);
                    var estoqueKitDto = EstoqueKitDto.Mapear(estoqueKit);
                    return estoqueKitDto.Id;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }
    }
}
