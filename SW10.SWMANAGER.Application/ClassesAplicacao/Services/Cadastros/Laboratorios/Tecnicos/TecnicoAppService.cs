using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos
{
    public class TecnicoAppService : SWMANAGERAppServiceBase, ITecnicoAppService
    {
        #region Cabecalho
        private readonly IRepository<Tecnico, long> _TecnicoRepository;
        private readonly IListarTecnicosExcelExporter _listarTecnicosExcelExporter;

        public TecnicoAppService(IRepository<Tecnico, long> TecnicoRepository, IListarTecnicosExcelExporter listarTecnicosExcelExporter)
        {
            _TecnicoRepository = TecnicoRepository;
            _listarTecnicosExcelExporter = listarTecnicosExcelExporter;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<TecnicoDto>> Listar(ListarTecnicosInput input)
        {
            var contarTecnicos = 0;
            List<Tecnico> Tecnicos;
            List<TecnicoDto> TecnicosDtos = new List<TecnicoDto>();
            try
            {
                var query = _TecnicoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTecnicos = await query
                    .CountAsync();

                Tecnicos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                TecnicosDtos = Tecnicos
                    .MapTo<List<TecnicoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TecnicoDto>(
                contarTecnicos,
                TecnicosDtos
                );
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _TecnicoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Tecnicos = new ListResultDto<GenericoIdNome> { Items = query };

                List<TecnicoDto> TecnicosList = new List<TecnicoDto>();

                //foreach (var Tecnico in Tecnicos.Items)
                //{
                //    TecnicosList.Add(Tecnico.MapTo<TecnicoDto>());
                //}

                //ListResultDto<TecnicoDto> TecnicosDto = new ListResultDto<TecnicoDto> { Items = TecnicosList };

                return Tecnicos;

                //	return new ListResultDto<TecnicoDto> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(TecnicoDto input)
        {
            try
            {
                var Tecnico = TecnicoDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    await _TecnicoRepository.InsertAsync(Tecnico);
                }
                else
                {
                    var ori = await _TecnicoRepository.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Codigo = input.Descricao;
                    ori.Codigo = input.RegConselho;

                    await _TecnicoRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(TecnicoDto input)
        {
            try
            {
                await _TecnicoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<TecnicoDto> Obter(long id)
        {
            try
            {
                var query = await _TecnicoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var Tecnico = TecnicoDto.Mapear(query);

                return Tecnico;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarTecnicosInput input)
        {
            try
            {
                var result = await Listar(input);
                var Tecnicos = result.Items;
                return _listarTecnicosExcelExporter.ExportToFile(Tecnicos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<TecnicoDto> pacientesDtos = new List<TecnicoDto>();
            try
            {
                //get com filtro
                var query = from p in _TecnicoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
