using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosPerdaProdutos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class MotivoPerdaProdutoAppService : SWMANAGERAppServiceBase, IMotivoPerdaProdutoAppService
    {
        public async Task<ListResultDto<MotivoPerdaProdutoDto>> ListarTodos()
        {
            var contarMotivoPerdadProduto = 0;
            List<MotivoPerdaProdutoDto> motivosPerdadProdutosDtos = new List<MotivoPerdaProdutoDto>();
            try
            {
                using (var motivoPerdaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoPerdaProduto, long>>())
                {
                    var query = motivoPerdaProdutoRepository.Object
                        .GetAll()
                        .AsNoTracking()
                        .Select(s => new MotivoPerdaProdutoDto { Id = s.Id, Codigo = s.Codigo, Descricao = s.Descricao });

                    contarMotivoPerdadProduto = await query
                        .CountAsync().ConfigureAwait(false);

                    motivosPerdadProdutosDtos = await query
                        .ToListAsync();

                    //motivosPerdadProdutosDtos = MotivoPerdaProdutoDto.Mapear(motivosPerdadProdutos);
                    //    .MapTo<List<MotivoPerdaProdutoDto>>();

                    return new ListResultDto<MotivoPerdaProdutoDto> { Items = motivosPerdadProdutosDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropDown(DropdownInput input)
        {
            try
            {
                using (var motivoPerdaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoPerdaProduto, long>>())
                {
                    return await this.CreateSelect2(motivoPerdaProdutoRepository.Object).ExecuteAsync(input).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
