using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposParticipacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes
{
    public class TipoParticipacaoAppService : SWMANAGERAppServiceBase, ITipoParticipacaoAppService
    {
        private readonly IRepository<TipoParticipacao, long> _TipoParticipacaoRepository;
        private readonly IListarTiposParticipacoesExcelExporter _listarTiposParticipacoesExcelExporter;

        public TipoParticipacaoAppService(IRepository<TipoParticipacao, long> TipoParticipacaoRepository, IListarTiposParticipacoesExcelExporter listarTiposParticipacoesExcelExporter)
        {
            _TipoParticipacaoRepository = TipoParticipacaoRepository;
            _listarTiposParticipacoesExcelExporter = listarTiposParticipacoesExcelExporter;
        }

        public async Task CriarOuEditar(TipoParticipacaoDto input)
        {
            try
            {
                var TipoParticipacao = input.MapTo<TipoParticipacao>();
                if (input.Id.Equals(0))
                {
                    await _TipoParticipacaoRepository.InsertAsync(TipoParticipacao);
                }
                else
                {
                    await _TipoParticipacaoRepository.UpdateAsync(TipoParticipacao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TipoParticipacaoDto input)
        {
            try
            {
                await _TipoParticipacaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoParticipacaoDto>> Listar(ListarTiposParticipacoesInput input)
        {
            var contarTiposParticipacoes = 0;
            List<TipoParticipacao> TiposParticipacoes;
            List<TipoParticipacaoDto> TiposParticipacoesDtos = new List<TipoParticipacaoDto>();
            try
            {
                var query = _TipoParticipacaoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposParticipacoes = await query
                    .CountAsync();

                TiposParticipacoes = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                TiposParticipacoesDtos = TiposParticipacoes
                    .MapTo<List<TipoParticipacaoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoParticipacaoDto>(
                contarTiposParticipacoes,
                TiposParticipacoesDtos
                );
        }

        public async Task<ListResultDto<TipoParticipacaoDto>> ListarTodos()
        {
            try
            {
                var TiposParticipacoes = await _TipoParticipacaoRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                var TiposParticipacoesDtos = TiposParticipacoes
                    .MapTo<List<TipoParticipacaoDto>>();

                return new ListResultDto<TipoParticipacaoDto> { Items = TiposParticipacoesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposParticipacoesInput input)
        {
            try
            {
                var result = await Listar(input);
                var TiposParticipacoes = result.Items;
                return _listarTiposParticipacoesExcelExporter.ExportToFile(TiposParticipacoes.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<TipoParticipacaoDto> Obter(long id)
        {
            try
            {
                var result = await _TipoParticipacaoRepository
                    .GetAsync(id);

                var TipoParticipacao = result
                    //.FirstOrDefault()
                    .MapTo<TipoParticipacaoDto>();

                return TipoParticipacao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
