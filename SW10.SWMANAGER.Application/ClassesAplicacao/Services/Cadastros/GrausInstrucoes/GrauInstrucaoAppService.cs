using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GrausInstrucoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrocoes
{
    public class GrauInstrucaoAppService : SWMANAGERAppServiceBase, IGrauInstrucaoAppService
    {
        private readonly IRepository<GrauInstrucao, long> _grauInstrucaoRepository;
        private readonly IListarGrausInstrucoesExcelExporter _listarGrausInstrocoesExcelExporter;

        public GrauInstrucaoAppService(IRepository<GrauInstrucao, long> grauInstrucaoRepository, IListarGrausInstrucoesExcelExporter listarGrausInstrucoesExcelExporter)
        {
            _grauInstrucaoRepository = grauInstrucaoRepository;
            _listarGrausInstrocoesExcelExporter = listarGrausInstrucoesExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarGrauInstrucao input)
        {
            try
            {
                var GrauInstrucao = input.MapTo<GrauInstrucao>();
                if (input.Id.Equals(0))
                {
                    await _grauInstrucaoRepository.InsertAsync(GrauInstrucao);
                }
                else
                {
                    await _grauInstrucaoRepository.UpdateAsync(GrauInstrucao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarGrauInstrucao input)
        {
            try
            {
                await _grauInstrucaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<GrauInstrucaoDto>> Listar(ListarGrausInstrucoesInput input)
        {
            var contarIndicacoes = 0;
            List<GrauInstrucao> grauInstrucao;
            List<GrauInstrucaoDto> grausInstrucoesDtos = new List<GrauInstrucaoDto>();
            try
            {
                var query = _grauInstrucaoRepository
                  .GetAll()
                  .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                      m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                  );

                contarIndicacoes = await query
                    .CountAsync();

                grauInstrucao = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                grausInstrucoesDtos = grauInstrucao.MapTo<List<GrauInstrucaoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GrauInstrucaoDto>(
                contarIndicacoes,
                grausInstrucoesDtos
                );
        }


        public async Task<ListResultDto<GrauInstrucaoDto>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _grauInstrucaoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    ).Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var grausInstrucoes = new ListResultDto<GenericoIdNome> { Items = query };

                List<GrauInstrucaoDto> grausInstrucoesList = new List<GrauInstrucaoDto>();

                foreach (var grauInstrucao in grausInstrucoes.Items)
                {
                    grausInstrucoesList.Add(grauInstrucao.MapTo<GrauInstrucaoDto>());
                }

                ListResultDto<GrauInstrucaoDto> grausInstrucoesDto = new ListResultDto<GrauInstrucaoDto> { Items = grausInstrucoesList };

                return grausInstrucoesDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarGrausInstrucoesInput input)
        {
            try
            {
                var result = await Listar(input);
                var grausInstrucoes = result.Items;
                return _listarGrausInstrocoesExcelExporter.ExportToFile(grausInstrucoes.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }


        public async Task<CriarOuEditarGrauInstrucao> Obter(long id)
        {
            try
            {
                var query = await _grauInstrucaoRepository
                    .GetAsync(id);
                //.Where(
                //    m => m.Id.Equals(id)
                //);

                //var result = await query
                //    .FirstOrDefaultAsync();

                var grauInstrucao = query
                    .MapTo<CriarOuEditarGrauInstrucao>();

                return grauInstrucao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }



    }
}
