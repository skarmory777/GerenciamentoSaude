using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class ClassificacaoRiscoAppService : SWMANAGERAppServiceBase, IClassificacaoRiscoAppService
    {
        private readonly IRepository<ClassificacaoRisco, long> _classificacaoRiscoRepository;
        private readonly IListarClassificacoesRiscoExcelExporter _listarClassificacoesRiscoExcelExporter;
        private readonly ICidadeAppService _cidadeAppService;

        public ClassificacaoRiscoAppService(
            IRepository<ClassificacaoRisco, long> classificacaoRiscoRepository,
            IListarClassificacoesRiscoExcelExporter listarClassificacoesRiscoExcelExporter,
            ICidadeAppService cidadeAppService
            )
        {
            _classificacaoRiscoRepository = classificacaoRiscoRepository;
            _listarClassificacoesRiscoExcelExporter = listarClassificacoesRiscoExcelExporter;
            _cidadeAppService = cidadeAppService;
        }

        public async Task CriarOuEditar(CriarOuEditarClassificacaoRisco input)
        {
            try
            {
                var classificacaoRisco = input.MapTo<ClassificacaoRisco>();

                if (input.Id.Equals(0))
                {
                    await _classificacaoRiscoRepository.InsertAsync(classificacaoRisco);
                }
                else
                {
                    await _classificacaoRiscoRepository.UpdateAsync(classificacaoRisco);
                }
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
                await _classificacaoRiscoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ClassificacaoRiscoDto>> ListarTodos()
        {
            var contarLeitos = 0;
            List<ClassificacaoRisco> leitos;
            List<ClassificacaoRiscoDto> leitosDtos = new List<ClassificacaoRiscoDto>();
            try
            {
                var query = _classificacaoRiscoRepository
                    .GetAll();

                contarLeitos = await query
                                   .CountAsync().ConfigureAwait(false);

                leitos = await query
                             .AsNoTracking()
                             .ToListAsync().ConfigureAwait(false);

                leitosDtos = leitos
                    .MapTo<List<ClassificacaoRiscoDto>>();

                return new PagedResultDto<ClassificacaoRiscoDto>(
                contarLeitos,
                leitosDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<PagedResultDto<ClassificacaoRiscoDto>> Listar(ListarClassificacoesRiscoInput input)
        //{
        //    var contarClassificacoesRisco = 0;
        //    List<ClassificacaoRisco> classificacoesRisco;
        //    List<ClassificacaoRiscoDto> classificacoesRiscoDtos = new List<ClassificacaoRiscoDto>();
        //    try
        //    {
        //        var query = _classificacaoRiscoRepository
        //            .GetAll()
        //            .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
        //                m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper())
        //            );

        //        contarClassificacoesRisco = await query
        //            .CountAsync();

        //        classificacoesRisco = await query
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();

        //        classificacoesRiscoDtos = classificacoesRisco
        //            .MapTo<List<ClassificacaoRiscoDto>>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //    return new PagedResultDto<ClassificacaoRiscoDto>(
        //        contarClassificacoesRisco,
        //        classificacoesRiscoDtos
        //        );
        //}

        public async Task<FileDto> ListarParaExcel(ListarClassificacoesRiscoInput input)
        {
            try
            {
                //var result = await Listar(input);
                var result = await ListarTodos();
                var classificacoesRisco = result.Items;
                return _listarClassificacoesRiscoExcelExporter.ExportToFile(classificacoesRisco.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarClassificacaoRisco> Obter(long id)
        {
            try
            {
                var result = await _classificacaoRiscoRepository
                    .GetAsync(id);

                var classificacaoRisco = result
                    .MapTo<CriarOuEditarClassificacaoRisco>();

                return classificacaoRisco;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
