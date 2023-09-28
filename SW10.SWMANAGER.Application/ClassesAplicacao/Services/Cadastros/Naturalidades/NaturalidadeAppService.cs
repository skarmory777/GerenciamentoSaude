using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class NaturalidadeAppService : SWMANAGERAppServiceBase, INaturalidadeAppService
    {
        private readonly IRepository<Naturalidade, long> _naturalidadeRepository;
        private readonly IListarNaturalidadesExcelExporter _listarNaturalidadesExcelExporter;

        public NaturalidadeAppService(
            IRepository<Naturalidade, long> naturalidadeRepository,
            IListarNaturalidadesExcelExporter listarNaturalidadesExcelExporter
        )
        {
            _naturalidadeRepository = naturalidadeRepository;
            _listarNaturalidadesExcelExporter = listarNaturalidadesExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarNaturalidade input)
        {
            try
            {
                var naturalidade = input.MapTo<Naturalidade>();
                if (input.Id.Equals(0))
                {
                    await _naturalidadeRepository.InsertAsync(naturalidade);
                }
                else
                {
                    await _naturalidadeRepository.UpdateAsync(naturalidade);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarNaturalidade input)
        {
            try
            {
                await _naturalidadeRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<NaturalidadeDto>> Listar(ListarNaturalidadesInput input)
        {
            var contarNaturalidades = 0;
            List<Naturalidade> naturalidades;
            List<NaturalidadeDto> naturalidadesDtos = new List<NaturalidadeDto>();
            try
            {
                var query = _naturalidadeRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarNaturalidades = await query
                    .CountAsync();

                naturalidades = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                naturalidadesDtos = naturalidades
                    .MapTo<List<NaturalidadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<NaturalidadeDto>(
                contarNaturalidades,
                naturalidadesDtos
                );
        }

        public async Task<ListResultDto<NaturalidadeDto>> ListarTodos()
        {
            List<Naturalidade> naturalidades;
            List<NaturalidadeDto> naturalidadesDtos = new List<NaturalidadeDto>();
            try
            {
                naturalidades = await _naturalidadeRepository
                  .GetAll()
                  .AsNoTracking()
                  .ToListAsync();

                naturalidadesDtos = naturalidades
                    .MapTo<List<NaturalidadeDto>>();

                return new ListResultDto<NaturalidadeDto> { Items = naturalidadesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<NaturalidadeDto>> ListarAutoComplete(string input)
        {
            List<Naturalidade> naturalidades;
            List<NaturalidadeDto> naturalidadesDtos = new List<NaturalidadeDto>();
            try
            {
                var query = _naturalidadeRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    );

                naturalidades = await query
                    .AsNoTracking()
                    .ToListAsync();

                naturalidadesDtos = naturalidades
                    .MapTo<List<NaturalidadeDto>>();

                return new ListResultDto<NaturalidadeDto> { Items = naturalidadesDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarNaturalidadesInput input)
        {
            try
            {
                var result = await Listar(input);
                var naturalidades = result.Items;
                return _listarNaturalidadesExcelExporter.ExportToFile(naturalidades.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarNaturalidade> Obter(long id)
        {
            try
            {
                var result = await _naturalidadeRepository.GetAsync(id);
                var naturalidade = result.MapTo<CriarOuEditarNaturalidade>();
                return naturalidade;
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
            return await this.CreateSelect2(this._naturalidadeRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
