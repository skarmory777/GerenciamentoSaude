using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas
{
    public class EntradaItemAppService : SWMANAGERAppServiceBase, IEntradaItemAppService
    {
        private readonly IRepository<EntradaItem, long> _entradaItemRepository;

        public EntradaItemAppService(IRepository<EntradaItem, long> entradaItemRepository)
        {
            _entradaItemRepository = entradaItemRepository;
        }

        public async Task<EntradaItemDto> CriarOuEditar(CriarOuEditarEntradaItem input, long id)
        {
            try
            {
                var entradaItem = new EntradaItem();
                entradaItem = input.MapTo<EntradaItem>();
                var entradaItemDto = entradaItem.MapTo<EntradaItemDto>();
                if (input.Id.Equals(0))
                {
                    input.Id = await _entradaItemRepository.InsertAndGetIdAsync(entradaItem);

                }
                else
                {

                    await _entradaItemRepository.UpdateAsync(entradaItem);
                }
                var query = await Obter(input.Id);
                var newObj = query.MapTo<EntradaItem>();

                entradaItemDto = newObj.MapTo<EntradaItemDto>();

                return entradaItemDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<CriarOuEditarEntradaItem> Obter(long id)
        {
            try
            {
                var query = await _entradaItemRepository
                    .GetAll()
                    //.Include(m => m.EntradaId)
                    //.Include(m => m.ProdutoId)
                    //.Include(m => m.Quantidade)
                    //.Include(m => m.CustoUnitario)
                    .Include(m => m.Produto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var entrada = query.MapTo<CriarOuEditarEntradaItem>();

                return entrada;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }


        public async Task Editar(CriarOuEditarEntradaItem input)
        {
            try
            {
                await _entradaItemRepository.UpdateAsync(input.MapTo<EntradaItem>());
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }


        public async Task Excluir(long id)
        {
            try
            {
                await _entradaItemRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public Task<PagedResultDto<EntradaItemDto>> Listar(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
