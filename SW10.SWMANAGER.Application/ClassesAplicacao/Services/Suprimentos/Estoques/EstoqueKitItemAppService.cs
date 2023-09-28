using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    using Abp.Application.Services.Dto;
    using Abp.Dependency;
    using Abp.Linq.Extensions;
    using Abp.UI;
    using System;

    public class EstoqueKitItemAppService : SWMANAGERAppServiceBase, IEstoqueKitItemAppService
    {
        private readonly IRepository<EstoqueKitItem, long> _estoqueKitItemRepository;
        private readonly IRepository<EstoqueGrupo, long> _estoqueGrupoRepositorio;

        public EstoqueKitItemAppService(
            IRepository<EstoqueKitItem, long> estoqueKitItemRepository,
            IRepository<EstoqueGrupo, long> estoqueGrupoRepositorio
            )
        {
            _estoqueKitItemRepository = estoqueKitItemRepository;
            _estoqueGrupoRepositorio = estoqueGrupoRepositorio;
        }

        public async Task<List<EstoqueKitItemDto>> ListarPeloKitEstoqueIdEEstoqueId(long kitEstoqueId, long estoqueId)
        {
            List<EstoqueKitItem> itens;
            try
            {
                var estoquesGrupos = _estoqueGrupoRepositorio.GetAll();

                itens = await _estoqueKitItemRepository.GetAll().AsNoTracking()
                                                 .Include(i => i.Produto)
                                                 .Include(i => i.EstoqueKit)
                                                 .Include(i => i.Unidade)
                                                 .Where(w => w.EstoqueKitId == kitEstoqueId && estoquesGrupos.Any(a => a.GrupoId == w.Produto.GrupoId && a.EstoqueId == estoqueId)).ToListAsync();

                return EstoqueKitItemDto.Mapear(itens);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<EstoqueKitItemDto>> ObterItensKit(long estoqueKitItemId)
        {
            List<EstoqueKitItem> itens;
            try
            {
                var estoqueKitItem = await _estoqueKitItemRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(w => w.Id == estoqueKitItemId);

                itens = await _estoqueKitItemRepository.GetAll().AsNoTracking()
                                                 .Include(i => i.Produto)
                                                 .Include(i => i.EstoqueKit)
                                                 .Include(i => i.Unidade)
                                                 .Where(w => w.EstoqueKitId == estoqueKitItem.EstoqueKitId)
                                                 .ToListAsync();

                return EstoqueKitItemDto.Mapear(itens);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EstoqueKitItemDto>> ListarItensKit(ListarEstoqueKitItemInput input)
        {
            var count = 0;
            List<EstoqueKitItem> itens;
            List<EstoqueKitItemDto> itensDtos = new List<EstoqueKitItemDto>();
            try
            {
                var query = _estoqueKitItemRepository.GetAll().AsNoTracking()
                                                 .Include(i => i.Produto)
                                                 .Include(i => i.EstoqueKit)
                                                 .Include(i => i.Unidade)
                                                 .Where(w => w.EstoqueKitId == input.EstoqueKitId);

                count = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = EstoqueKitItemDto.Mapear(itens);

                return new PagedResultDto<EstoqueKitItemDto>(
                    count,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long?> CriarOuEditar(EstoqueKitItemDto input)
        {
            try
            {
                var kit = EstoqueKitItemDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    return await _estoqueKitItemRepository.InsertAndGetIdAsync(kit);
                }
                else
                {
                    var estoqueKit = await _estoqueKitItemRepository.UpdateAsync(kit);
                    var estoqueKitDto = EstoqueKitItemDto.Mapear(estoqueKit);
                    return estoqueKitDto.Id;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(EstoqueKitItemDto input)
        {
            try
            {
                await this._estoqueKitItemRepository.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }
    }
}
