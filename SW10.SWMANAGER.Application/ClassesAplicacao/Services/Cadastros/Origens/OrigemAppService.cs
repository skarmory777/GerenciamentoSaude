using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class OrigemAppService : SWMANAGERAppServiceBase, IOrigemAppService
    {
        public OrigemAppService(IRepository<Origem, long> origemRepository, IListarOrigensExcelExporter listarOrigensExcelExporter)
        {
            _origemRepository = origemRepository;
            _listarOrigensExcelExporter = listarOrigensExcelExporter;
        }

        private readonly IRepository<Origem, long> _origemRepository;
        private readonly IListarOrigensExcelExporter _listarOrigensExcelExporter;

        public async Task<CriarOuEditarOrigem> Obter(long id)
        {
            try
            {
                var result = await _origemRepository.GetAsync(id);
                var origem = result.MapTo<CriarOuEditarOrigem>();
                return origem;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<PagedResultDto<OrigemDto>> Listar(ListarOrigensInput input)
        {
            var contarOrigens = 0;
            List<Origem> origens;
            List<OrigemDto> origensDtos = new List<OrigemDto>();
            try
            {
                var query = _origemRepository
                    .GetAll()
                    .Include(m => m.UnidadeOrganizacional)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarOrigens = await query
                    .CountAsync();

                origens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                origensDtos = origens
                    .MapTo<List<OrigemDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<OrigemDto>(
                contarOrigens,
                origensDtos
                );
        }

        public async Task<ListResultDto<OrigemDto>> ListarTodos()
        {
            List<Origem> origens;
            List<OrigemDto> origensDtos = new List<OrigemDto>();
            try
            {
                origens = await _origemRepository
                  .GetAll()
                    .Include(m => m.UnidadeOrganizacional)
                  .AsNoTracking()
                  .ToListAsync();

                origensDtos = origens
                    .MapTo<List<OrigemDto>>();

                return new ListResultDto<OrigemDto> { Items = origensDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public ListResultDto<OrigemDto> ListarDropdown()
        {
            List<Origem> origens;
            List<OrigemDto> origensDtos = new List<OrigemDto>();
            try
            {
                origens = _origemRepository
                  .GetAll()
                    .Include(m => m.UnidadeOrganizacional)
                  .AsNoTracking()
                  .ToList();

                origensDtos = origens
                    .MapTo<List<OrigemDto>>();

                return new ListResultDto<OrigemDto> { Items = origensDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarOrigensInput input)
        {
            try
            {
                var result = await Listar(input);
                var origens = result.Items;
                return _listarOrigensExcelExporter.ExportToFile(origens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        //ACERTAR MÉTODO AUTOCOMPLETE
        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _origemRepository
                    .GetAll()
                    .Include(m => m.UnidadeOrganizacional)
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    ).Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var origens = new ListResultDto<GenericoIdNome> { Items = query };

                return origens;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarOrigem input)
        {
            try
            {
                await _origemRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task CriarOuEditar(CriarOuEditarOrigem input)
        {
            try
            {
                var origem = input.MapTo<Origem>();
                if (input.Id.Equals(0))
                {
                    await _origemRepository.InsertAsync(origem);
                }
                else
                {
                    var ori = await _origemRepository.GetAsync(origem.Id);
                    ori.Codigo = origem.Codigo;
                    ori.Descricao = origem.Descricao;
                    ori.IsAtivo = origem.IsAtivo;
                    ori.UnidadeOrganizacionalId = origem.UnidadeOrganizacionalId;

                    await _origemRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {

            return await this.CreateSelect2(this._origemRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
