
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CapitulosCID;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID
{
    public class CapituloCIDAppService : SWMANAGERAppServiceBase, ICapituloCIDAppService
    {
        private readonly IRepository<CapituloCID, long> _capituloCIDRepository;
        private readonly IListarCapitulosCIDExcelExporter _listarCapitulosCIDExcelExporter;

        public CapituloCIDAppService(IRepository<CapituloCID, long> capituloCIDRepository, IListarCapitulosCIDExcelExporter listarCapitulosCIDExcelExporter)
        {
            _capituloCIDRepository = capituloCIDRepository;
            _listarCapitulosCIDExcelExporter = listarCapitulosCIDExcelExporter;
        }

        public async Task CriarOuEditar(CapituloCIDDto input)
        {
            try
            {
                var estado = input.MapTo<CapituloCID>();
                if (input.Id.Equals(0))
                {
                    await _capituloCIDRepository.InsertAsync(estado);
                }
                else
                {
                    await _capituloCIDRepository.UpdateAsync(estado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CapituloCIDDto input)
        {
            try
            {
                await _capituloCIDRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<CapituloCIDDto>> Listar(ListarCapitulosCIDInput input)
        {
            var contarCapitulosCID = 0;
            List<CapituloCID> capituloCID = new List<CapituloCID>();
            List<CapituloCIDDto> indecacaoDtos = new List<CapituloCIDDto>();
            try
            {
                var query = _capituloCIDRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                contarCapitulosCID = await query
                    .CountAsync();

                capituloCID = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                indecacaoDtos = capituloCID.MapTo<List<CapituloCIDDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CapituloCIDDto>(
                contarCapitulosCID,
                indecacaoDtos
                );
        }


        public async Task<FileDto> ListarParaExcel(ListarCapitulosCIDInput input)
        {
            try
            {
                var result = await Listar(input);
                var capitulosCID = result.Items;
                return _listarCapitulosCIDExcelExporter.ExportToFile(capitulosCID.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CapituloCIDDto> Obter(long id)
        {
            var query = await _capituloCIDRepository
                .GetAsync(id);

            var capituloCID = query.MapTo<CapituloCIDDto>();

            return capituloCID;
        }

    }
}
