using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public class ProdutoUnidadeTipoAppService : SWMANAGERAppServiceBase, IProdutoUnidadeTipoAppService
    {
        private readonly IRepository<ProdutoRelacaoUnidade, long> _produtoUnidadeTipoRepository;

        public ProdutoUnidadeTipoAppService(IRepository<ProdutoRelacaoUnidade, long> produtoUnidadeTipoRepository)
        {
            _produtoUnidadeTipoRepository = produtoUnidadeTipoRepository;
        }

        public async Task<ProdutoUnidadeDto> CriarOuEditar(ProdutoUnidadeDto input, long id)
        {
            try
            {
                var produtoUnidadeTipo = new ProdutoRelacaoUnidade();
                produtoUnidadeTipo = input.MapTo<ProdutoRelacaoUnidade>();
                var produtoUnidadeTipoDto = produtoUnidadeTipo.MapTo<ProdutoUnidadeDto>();
                if (input.Id.Equals(0))
                {
                    input.Id = await _produtoUnidadeTipoRepository.InsertAndGetIdAsync(produtoUnidadeTipo);

                }
                else
                {

                    await _produtoUnidadeTipoRepository.UpdateAsync(produtoUnidadeTipo);
                }
                var query = await Obter(input.Id);
                var newObj = query.MapTo<ProdutoRelacaoUnidade>();

                produtoUnidadeTipoDto = newObj.MapTo<ProdutoUnidadeDto>();
                //produtoUnidadeTipoDto.TipoUnidade = newObj.TipoUnidade;

                return produtoUnidadeTipoDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ProdutoUnidadeDto> Obter(long id)
        {
            try
            {
                var query = await _produtoUnidadeTipoRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .Include(m => m.TipoUnidade)
                    .Include(m => m.ProdutoUnidade)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var produto = query.MapTo<ProdutoUnidadeDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }


        public async Task<ProdutoUnidadeDto> ObterPorUnidadeProduto(long unidadeId, long produtoid)
        {
            try
            {
                var query = await _produtoUnidadeTipoRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .Include(m => m.TipoUnidade)
                    .Include(m => m.ProdutoUnidade)
                    .FirstOrDefaultAsync(m => m.UnidadeId == unidadeId
                                           && m.ProdutoId == produtoid);


                var produto = query.MapTo<ProdutoUnidadeDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }



        public async Task Editar(ProdutoUnidadeDto input)
        {
            try
            {
                //var produtoUnidadeTipo = new ProdutoUnidadeTipo();
                //produtoUnidadeTipo = input.MapTo<ProdutoUnidadeTipo>();
                //if (input.Id.Equals(0))
                //{
                //    await _produtoUnidadeTipoRepository.InsertAsync(produtoUnidadeTipo);
                //}
                //else
                //{

                //await _produtoUnidadeTipoRepository.UpdateAsync(produtoUnidadeTipo);
                await _produtoUnidadeTipoRepository.UpdateAsync(input.MapTo<ProdutoRelacaoUnidade>());
                //}
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }


        public async Task Excluir(ProdutoUnidadeDto input)
        {
            try
            {
                await _produtoUnidadeTipoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public Task<PagedResultDto<ProdutoUnidadeDto>> Listar(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
