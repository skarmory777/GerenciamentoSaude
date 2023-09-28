using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class ProfissaoAppService : SWMANAGERAppServiceBase, IProfissaoAppService
    {
        private readonly IRepository<Profissao, long> _profissaoRepository;
        private readonly IListarProfissoesExcelExporter _listarProfissoesExcelExporter;

        public ProfissaoAppService(IRepository<Profissao, long> profissaoRepository, IListarProfissoesExcelExporter listarProfissoesExcelExporter)
        {
            _profissaoRepository = profissaoRepository;
            _listarProfissoesExcelExporter = listarProfissoesExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarProfissao input)
        {
            try
            {
                var profissao = input.MapTo<Profissao>();
                if (input.Id.Equals(0))
                {
                    await _profissaoRepository.InsertAsync(profissao);
                }
                else
                {
                    var ori = await _profissaoRepository.GetAsync(input.Id);

                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;

                    await _profissaoRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarProfissao input)
        {
            try
            {
                await _profissaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProfissaoDto>> Listar(ListarProfissoesInput input)
        {
            var contarProfissoes = 0;
            List<Profissao> profissoes;
            List<ProfissaoDto> profissoesDtos = new List<ProfissaoDto>();
            try
            {
                var query = _profissaoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProfissoes = await query
                                       .CountAsync().ConfigureAwait(false);

                profissoes = await query
                                 .AsNoTracking()
                                 .OrderBy(input.Sorting)
                                 .PageBy(input)
                                 .ToListAsync().ConfigureAwait(false);

                profissoesDtos = profissoes
                    .MapTo<List<ProfissaoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProfissaoDto>(
                contarProfissoes,
                profissoesDtos
                );
        }

        public async Task<ListResultDto<ProfissaoDto>> ListarPorNome(string input)
        {
            List<Profissao> profissoes;
            List<ProfissaoDto> profissoesDtos = new List<ProfissaoDto>();
            try
            {
                var query = _profissaoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    );

                profissoes = await query
                    .AsNoTracking()
                    .ToListAsync();

                profissoesDtos = profissoes
                    .MapTo<List<ProfissaoDto>>();

                return new ListResultDto<ProfissaoDto> { Items = profissoesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _profissaoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarProfissoesInput input)
        {
            try
            {
                var result = await Listar(input);
                var profissoes = result.Items;
                return _listarProfissoesExcelExporter.ExportToFile(profissoes.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarProfissao> Obter(long id)
        {
            try
            {
                var result = await _profissaoRepository.GetAsync(id);
                var profissao = result.MapTo<CriarOuEditarProfissao>();
                return profissao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<ProfissaoDto>> ListarTodos()
        {
            //var query = await _profissaoRepository.GetAll().ToListAsync();
            //var profissoesDto = query.MapTo<List<ProfissaoDto>>();
            //return new ListResultDto<ProfissaoDto> { Items = profissoesDto };

            List<Profissao> profissoes;
            List<ProfissaoDto> profissoesDtos = new List<ProfissaoDto>();
            try
            {
                profissoes = await _profissaoRepository
                  .GetAll()
                  .AsNoTracking()
                  .ToListAsync();

                profissoesDtos = profissoes
                    .MapTo<List<ProfissaoDto>>();

                return new ListResultDto<ProfissaoDto> { Items = profissoesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._profissaoRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
