
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Feriados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados
{
    public class FeriadoAppService : SWMANAGERAppServiceBase, IFeriadoAppService
    {
        private readonly IRepository<Feriado, long> _feriadoRepository;
        private readonly IListarFeriadosExcelExporter _listarFeriadosExcelExporter;

        public FeriadoAppService(IRepository<Feriado, long> feriadoRepository, IListarFeriadosExcelExporter listarFeriadosExcelExporter)
        {
            _feriadoRepository = feriadoRepository;
            _listarFeriadosExcelExporter = listarFeriadosExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarFeriado input)
        {
            try
            {
                var feriado = input.MapTo<Feriado>();
                if (input.Id.Equals(0))
                {
                    await _feriadoRepository.InsertAsync(feriado);
                }
                else
                {
                    await _feriadoRepository.UpdateAsync(feriado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarFeriado input)
        {
            try
            {
                await _feriadoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<FeriadoDto>> Listar(ListarFeriadosInput input)
        {
            var contarFeriados = 0;
            List<Feriado> feriados = new List<Feriado>();
            List<FeriadoDto> feriadosDtos = new List<FeriadoDto>();
            try
            {
                var query = _feriadoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.DiaMesAno.ToString().Contains(input.Filtro.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarFeriados = await query
                    .CountAsync();

                feriados = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                feriadosDtos = feriados.MapTo<List<FeriadoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FeriadoDto>(
                contarFeriados,
                feriadosDtos
                );
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? paisId)
        {
            try
            {
                var query = await _feriadoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                       m.DiaMesAno.ToString().Contains(input.ToUpper()) ||
                       m.Descricao.ToUpper().Contains(input.ToUpper()))
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarFeriadosInput input)
        {
            try
            {
                var result = await Listar(input);
                var feriados = result.Items;
                return _listarFeriadosExcelExporter.ExportToFile(feriados.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }


        public async Task<CriarOuEditarFeriado> Obter(long id)
        {
            var query = await _feriadoRepository
                .GetAsync(id);

            var feriado = query.MapTo<CriarOuEditarFeriado>();

            return feriado;
        }

        public async Task<CriarOuEditarFeriado> Obter(string uf)
        {
            var query = await _feriadoRepository
                .GetAllListAsync(m => m.Descricao.ToUpper().Equals(uf.ToUpper()));

            var feriado = query
                .FirstOrDefault()
                .MapTo<CriarOuEditarFeriado>();

            return feriado;
        }


    }
}
