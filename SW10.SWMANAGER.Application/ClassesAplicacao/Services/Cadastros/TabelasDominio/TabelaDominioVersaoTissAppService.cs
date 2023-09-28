using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio
{
    public class TabelaDominioVersaoTissAppService : SWMANAGERAppServiceBase, ITabelaDominioVersaoTissAppService
    {
        private readonly IRepository<TabelaDominioVersaoTiss, long> _tabelaDominioVersaoTissRepository;

        public TabelaDominioVersaoTissAppService(IRepository<TabelaDominioVersaoTiss, long> tabelaDominioVersaoTissRepository)
        {
            _tabelaDominioVersaoTissRepository = tabelaDominioVersaoTissRepository;
        }

        public async Task CriarOuEditar(TabelaDominioVersaoTissDto input)
        {
            try
            {
                var tabelaDominioVersaoTiss = new TabelaDominioVersaoTiss();
                tabelaDominioVersaoTiss = input.MapTo<TabelaDominioVersaoTiss>();
                if (input.Id.Equals(0))
                {
                    await _tabelaDominioVersaoTissRepository.InsertAsync(tabelaDominioVersaoTiss);
                }
                else
                {

                    await _tabelaDominioVersaoTissRepository.UpdateAsync(tabelaDominioVersaoTiss);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TabelaDominioVersaoTissDto input)
        {
            try
            {
                await _tabelaDominioVersaoTissRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<TabelaDominioVersaoTissDto> Obter(long id)
        {
            try
            {
                var result = await _tabelaDominioVersaoTissRepository
                    .GetAll()
                    .Include(m => m.TabelaDominio)
                    .Include(m => m.VersaoTiss)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var tabelaDominioVersaoTiss = result
                    //.FirstOrDefault()
                    .MapTo<TabelaDominioVersaoTissDto>();

                return tabelaDominioVersaoTiss;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
