using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class PreAtendimentoAppService : SWMANAGERAppServiceBase, IPreAtendimentoAppService
    {
        private readonly IRepository<PreAtendimento, long> _preAtendimentoRepository;
        private readonly IListarPreAtendimentosExcelExporter _listarPreAtendimentosExcelExporter;
        private readonly ICidadeAppService _cidadeAppService;

        public PreAtendimentoAppService(
            IRepository<PreAtendimento, long> preAtendimentoRepository,
            IListarPreAtendimentosExcelExporter listarPreAtendimentosExcelExporter,
            ICidadeAppService cidadeAppService
            )
        {
            _preAtendimentoRepository = preAtendimentoRepository;
            _listarPreAtendimentosExcelExporter = listarPreAtendimentosExcelExporter;
            _cidadeAppService = cidadeAppService;
        }

        public async Task CriarOuEditar(CriarOuEditarPreAtendimento input)
        {
            try
            {
                var preAtendimento = input.MapTo<PreAtendimento>();

                if (input.Id.Equals(0))
                {
                    preAtendimento.DataRegistro = DateTime.Now;
                    await _preAtendimentoRepository.InsertAsync(preAtendimento);
                }
                else
                {
                    await _preAtendimentoRepository.UpdateAsync(preAtendimento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<long> CriarGetId(CriarOuEditarPreAtendimento input)
        {
            try
            {
                var preAtendimento = input.MapTo<PreAtendimento>();
                long id;

                if (input.Id.Equals(0))
                {
                    preAtendimento.DataRegistro = DateTime.Now;
                    id = await _preAtendimentoRepository.InsertAndGetIdAsync(preAtendimento);
                }
                else
                {
                    var inserido = await _preAtendimentoRepository.UpdateAsync(preAtendimento);
                    id = inserido.Id;
                }
                return id;
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
                await _preAtendimentoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PreAtendimentoDto>> ListarTodos()
        {
            var contarLeitos = 0;
            List<PreAtendimento> leitos;
            List<PreAtendimentoDto> leitosDtos = new List<PreAtendimentoDto>();
            try
            {
                var query = _preAtendimentoRepository
                    .GetAll().AsNoTracking();

                contarLeitos = await query
                                   .CountAsync().ConfigureAwait(false);

                leitos = await query
                             .ToListAsync().ConfigureAwait(false);

                leitosDtos = leitos
                    .MapTo<List<PreAtendimentoDto>>();

                return new PagedResultDto<PreAtendimentoDto>(contarLeitos, leitosDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ListarPreAtendimentosIndex>> ListarParaIndex(ListarPreAtendimentosInput input)
        {
            var contarPreAtendimentos = 0;
            List<PreAtendimento> preAtendimentos;
            List<ListarPreAtendimentosIndex> preAtendimentosDtos = new List<ListarPreAtendimentosIndex>();
            try
            {
                var query = _preAtendimentoRepository
                    .GetAll().AsNoTracking();

                contarPreAtendimentos = await query
                                            .CountAsync().ConfigureAwait(false);

                preAtendimentos = await query
                                      .AsNoTracking()
                                      //    .OrderBy(input.Sorting)
                                      //    .PageBy(input)
                                      .ToListAsync().ConfigureAwait(false);

                preAtendimentosDtos = preAtendimentos
                    .MapTo<List<ListarPreAtendimentosIndex>>();

                return new PagedResultDto<ListarPreAtendimentosIndex>(
                contarPreAtendimentos,
                preAtendimentosDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarPreAtendimentosInput input)
        {
            try
            {
                //var result = await Listar(input);
                var result = await ListarTodos();
                var preAtendimentos = result.Items;
                return _listarPreAtendimentosExcelExporter.ExportToFile(preAtendimentos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarPreAtendimento> Obter(long id)
        {
            try
            {
                var result = await _preAtendimentoRepository
                    .GetAsync(id);

                var preAtendimento = result
                    .MapTo<CriarOuEditarPreAtendimento>();

                return preAtendimento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
