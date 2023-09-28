using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public class DocItemAppService : SWMANAGERAppServiceBase, IDocItemAppService
    {
        private readonly IRepository<DocItem, long> _docItemRepository;

        public DocItemAppService(
            IRepository<DocItem, long> docItemRepository
            )
        {
            _docItemRepository = docItemRepository;
        }

        public async Task CriarOuEditar(DocItemDto input)
        {
            try
            {
                var docItem = input.MapTo<DocItem>();

                if (input.Id.Equals(0))
                {
                    await _docItemRepository.InsertAsync(docItem);
                }
                else
                {
                    await _docItemRepository.UpdateAsync(docItem);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(DocItemDto input)
        {
            try
            {
                await _docItemRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<DocItemDto>> ListarTodos()
        {
            try
            {
                var docItems = await _docItemRepository
                    .GetAll()

                    .AsNoTracking()
                    .ToListAsync();

                var docItemsDtos = docItems
                    .MapTo<List<DocItemDto>>();

                return new PagedResultDto<DocItemDto> { Items = docItemsDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<DocItemDto> Obter(long id)
        {
            try
            {
                var query = await _docItemRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var docItem = query.MapTo<DocItemDto>();

                return docItem;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
