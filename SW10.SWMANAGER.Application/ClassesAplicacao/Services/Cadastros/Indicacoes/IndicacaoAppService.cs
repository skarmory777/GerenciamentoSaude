
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Indicacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes
{
    public class IndicacaoAppService : SWMANAGERAppServiceBase, IIndicacaoAppService
    {
        private readonly IRepository<Indicacao, long> _indicacaoRepository;
        private readonly IListarIndicacoesExcelExporter _listarIndicacoesExcelExporter;


        public async Task<ListResultDto<IndicacaoDto>> ListarTodos()
        {
            List<Indicacao> indicacoes;
            List<IndicacaoDto> indicacoesDtos = new List<IndicacaoDto>();
            try
            {
                indicacoes = await _indicacaoRepository
                  .GetAll()
                  .AsNoTracking()
                  .ToListAsync();

                indicacoesDtos = indicacoes
                    .MapTo<List<IndicacaoDto>>();

                return new ListResultDto<IndicacaoDto> { Items = indicacoesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public IndicacaoAppService(IRepository<Indicacao, long> indicacaoRepository, IListarIndicacoesExcelExporter listarIndicacoesExcelExporter)
        {
            _indicacaoRepository = indicacaoRepository;
            _listarIndicacoesExcelExporter = listarIndicacoesExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarIndicacao input)
        {
            try
            {
                var indicacao = input.MapTo<Indicacao>();
                if (input.Id.Equals(0))
                {
                    await _indicacaoRepository.InsertAsync(indicacao);
                }
                else
                {
                    await _indicacaoRepository.UpdateAsync(indicacao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarIndicacao input)
        {
            try
            {
                await _indicacaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<IndicacaoDto>> Listar(ListarIndicacoesInput input)
        {
            var contarIndicacoes = 0;
            List<Indicacao> indicacao = new List<Indicacao>();
            List<IndicacaoDto> indecacaoDtos = new List<IndicacaoDto>();
            try
            {
                var query = _indicacaoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarIndicacoes = await query
                    .CountAsync();

                indicacao = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                indecacaoDtos = indicacao.MapTo<List<IndicacaoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<IndicacaoDto>(
                contarIndicacoes,
                indecacaoDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarIndicacoesInput input)
        {
            try
            {
                var result = await Listar(input);
                var indicacoes = result.Items;
                return _listarIndicacoesExcelExporter.ExportToFile(indicacoes.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarIndicacao> Obter(long id)
        {
            var query = await _indicacaoRepository
                .GetAsync(id);

            var indicacao = query.MapTo<CriarOuEditarIndicacao>();

            return indicacao;
        }

    }
}
