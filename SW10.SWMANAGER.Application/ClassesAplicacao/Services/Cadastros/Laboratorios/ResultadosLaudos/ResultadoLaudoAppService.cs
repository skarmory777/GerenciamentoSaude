using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos
{
    using Abp.Linq.Extensions;
    using Castle.Core.Internal;
    using RestSharp;
    using SW10.SWMANAGER.Helpers;
    using System.Configuration;
    using TheArtOfDev.HtmlRenderer.PdfSharp;

    public class ResultadoLaudoAppService : SWMANAGERAppServiceBase, IResultadoLaudoAppService
    {

        private readonly IListarResultadoLaudosExcelExporter _listarResultadoLaudosExcelExporter;
        private readonly IRepository<ResultadoLaudo, long> _resultadoLaudoRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ResultadoExame, long> _resultadoExameRepository;
        private readonly IRepository<RegistroArquivo, long> _registroArquivoRepository;
        private readonly IRepository<TabelaResultado, long> _tabelaResultadoRepositoy;
        private readonly IRepository<ItemResultado, long> _itemResultadoRepository;
        private readonly IRepository<Resultado, long> _resultadoRepository;


        public ResultadoLaudoAppService(IRepository<ResultadoLaudo, long> resultadoLaudoRepositorio
                                     , IListarResultadoLaudosExcelExporter listarResultadoLaudosExcelExporter
            , IUnitOfWorkManager unitOfWorkManager
            , IRepository<ResultadoExame, long> resultadoExameRepository
            , IRepository<RegistroArquivo, long> registroArquivoRepository
            , IRepository<TabelaResultado, long> tabelaResultadoRepositoy
            , IRepository<ItemResultado, long> itemResultadoRepository
            , IRepository<Resultado, long> resultadoRepository

            )
        {
            _resultadoLaudoRepositorio = resultadoLaudoRepositorio;
            _listarResultadoLaudosExcelExporter = listarResultadoLaudosExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _resultadoExameRepository = resultadoExameRepository;
            _registroArquivoRepository = registroArquivoRepository;
            _tabelaResultadoRepositoy = tabelaResultadoRepositoy;
            _itemResultadoRepository = itemResultadoRepository;
            _resultadoRepository = resultadoRepository;

        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(ResultadoLaudoDto input)
        {
            try
            {
                var resultadoLaudo = ResultadoLaudoDto.Mapear(input); //.MapTo<ResultadoLaudo>();
                if (input.Id.Equals(0))
                {
                    await _resultadoLaudoRepositorio.InsertOrUpdateAsync(resultadoLaudo);
                }
                else
                {
                    var ori = await _resultadoLaudoRepositorio.GetAsync(input.Id);

                    ori.CasaDecimal = input.CasaDecimal;
                    ori.Codigo = input.Codigo;
                    ori.DataDigitadoLaudo = input.DataDigitadoLaudo;
                    ori.Descricao = input.Descricao;
                    ori.Formula = input.Formula;
                    ori.IsInterface = input.IsInterface;
                    ori.IsSistema = input.IsSistema;
                    ori.ItemResultadoId = input.ItemResultadoId;
                    ori.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
                    ori.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
                    ori.MaximoFeminino = input.MaximoFeminino;
                    ori.MaximoMasculino = input.MaximoMasculino;
                    ori.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
                    ori.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
                    ori.MinimoFeminino = input.MinimoFeminino;
                    ori.MinimoMasculino = input.MinimoMasculino;
                    ori.NormalFeminino = input.NormalFeminino;
                    ori.NormalMasculino = input.NormalMasculino;
                    ori.Numerico = input.Numerico;
                    ori.Ordem = input.Ordem;
                    ori.OrdemRegistro = input.OrdemRegistro;
                    ori.Referencia = input.Referencia;
                    ori.Resultado = input.Resultado;
                    ori.ResultadoExameId = input.ResultadoExameId;
                    ori.TabelaResultadoId = input.TabelaResultadoId;
                    ori.TipoResultadoId = input.TipoResultadoId;
                    ori.UnidadeId = input.UnidadeId;
                    ori.UsuarioLaudoId = input.UsuarioLaudoId;
                    ori.VersaoAtual = input.VersaoAtual;

                    await _resultadoLaudoRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ResultadoLaudoDto input)
        {
            try
            {
                await _resultadoLaudoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ResultadoLaudoDto>> Listar(ListarResultadoLaudosInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<ResultadoLaudo> resultadoLaudos;
            List<ResultadoLaudoDto> resultadoLaudosDtos = new List<ResultadoLaudoDto>();
            try
            {
                var query = _resultadoLaudoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query.CountAsync().ConfigureAwait(false);

                resultadoLaudos = await query
                                      .AsNoTracking()
                                      .OrderBy(input.Sorting)
                                      .PageBy(input)
                                      .ToListAsync().ConfigureAwait(false);

                resultadoLaudosDtos = ResultadoLaudoDto.Mapear(resultadoLaudos).ToList();
                //.MapTo<List<ResultadoLaudoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ResultadoLaudoDto>(
                contarTiposTabelaDominio,
                resultadoLaudosDtos
                );
        }

        public async Task<ResultadoLaudoDto> Obter(long id)
        {
            try
            {
                var result = await _resultadoLaudoRepositorio.GetAsync(id);
                var resultadoLaudo = ResultadoLaudoDto.Mapear(result); //.MapTo<ResultadoLaudoDto>();
                return resultadoLaudo;
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
                var query = await _resultadoLaudoRepositorio.GetAll().WhereIf(!input.IsNullOrEmpty(), m => m.Descricao.Contains(input))
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var ResultadoLaudos = new ListResultDto<GenericoIdNome> { Items = query };

                List<ResultadoLaudoDto> ResultadoLaudosList = new List<ResultadoLaudoDto>();

                return ResultadoLaudos;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<FileDto> ListarParaExcel(ListarResultadoLaudosInput input)
        //{
        //    try
        //    {
        //        var result = await Listar(input);
        //        var ResultadoLaudos = result.Items;
        //        return _listarResultadoLaudosExcelExporter.ExportToFile(ResultadoLaudos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }
        //}

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<ResultadoLaudoDto> pacientesDtos = new List<ResultadoLaudoDto>();
            try
            {
                //get com filtro
                var query = from p in _resultadoLaudoRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        || m.Descricao.Contains(dropdownInput.search)
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

        [HttpPost]
        public async Task<DefaultReturn<ResultadoLaudoDto>> CriarOuEditarLista(string input, long coletaId)
        {
            var retornoPadrao = new DefaultReturn<ResultadoLaudoDto>
            {
                ReturnObject = new ResultadoLaudoDto(),
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };

            try
            {
                var resultadosDto = JsonConvert.DeserializeObject<List<FormatacaoItemIndexDto>>(input);

                var resultadoLaudoValidacaoService = new ResultadoLaudoValidacaoService(this._itemResultadoRepository, this._resultadoRepository);
                retornoPadrao.Errors = resultadoLaudoValidacaoService.Validar(resultadosDto, coletaId);

                if (retornoPadrao.Errors.Count == 0)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var resultadosDtoIds = resultadosDto.Select(x => x.LaudoResultadoId).Distinct().ToList();
                        var resultadoLaudos = this._resultadoLaudoRepositorio.GetAll().Where(w => resultadosDtoIds.Contains(w.Id)).ToList();
                        foreach (var resultadoLaudo in resultadoLaudos)
                        {
                            var resultadoDto = resultadosDto.FirstOrDefault(x => x.LaudoResultadoId == resultadoLaudo.Id);
                            resultadoLaudo.DataDigitadoLaudo = DateTime.Now;
                            resultadoLaudo.Resultado = resultadoDto.Resultado;
                            resultadoLaudo.UsuarioLaudoId = AbpSession.UserId;
                            this._resultadoLaudoRepositorio.Update(resultadoLaudo);
                        }

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();

                        unitOfWork.Dispose();
                        retornoPadrao.ReturnObject.RegistroArquivoId = await this.GerarArquivo(coletaId);
                    }

                }
                return retornoPadrao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<long> GerarArquivo(long coletaId)
        {
            long id = 0;
            try
            {
                var exames = await _resultadoExameRepository.GetAll().Where(w => w.ResultadoId == coletaId).AsNoTracking().ToListAsync();
                var systemBaseUrl = ConfigurationManager.AppSettings.Get("baseUrl");

                foreach (var exame in exames)
                {
                    var data = this.CreateJasperReport("Resultado")
                        .SetMethod(Method.POST)
                            .AddParameter("LabResultadoId", coletaId.ToString())
                            .AddParameter("Url", $"{systemBaseUrl}/ResultadoLaboratorio/RetornaFormatadoColeta?coletaId={coletaId}&exameId={exame.Id}&tenantId={AbpSession.TenantId}")
                            .AddParameter("UsuarioImpressao", this.GetCurrentUser().FullName)
                            .AddParameter("Dominio", this.GetConnectionStringName())
                        .GenerateReport();

                    var registroArquivo = new RegistroArquivo
                    {
                        Arquivo = data,
                        RegistroTabelaId = (long)EnumArquivoTabela.LaboratorioExame,
                        RegistroId = exame.Id
                    };

                    return await this._registroArquivoRepository.InsertAndGetIdAsync(registroArquivo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return id;
        }

        public async Task<string> FormatacaoColeta(long coletaId)
        {
            var exames = await _resultadoExameRepository.GetAll().Include(i => i.FaturamentoItem)
                                                       .Include(i => i.Formata)
                                                       .Where(w => w.ResultadoId == coletaId)
                                                       .AsNoTracking()
                                                       .ToListAsync();

            if (exames.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var exameIds = exames.Select(x => x.Id).Distinct().ToList();
            var laudos = await _resultadoLaudoRepositorio.GetAll()
                                                         .Where(w => w.ResultadoExameId.HasValue && exameIds.Contains(w.ResultadoExameId.Value))
                                                         .Include(i => i.ItemResultado)
                                                         .Include(i => i.LaboratorioUnidade)
                                                         .OrderBy(x => x.Ordem)
                                                         .AsNoTracking()
                                                         .ToListAsync();

            return await this.RetornaFormatacaoColeta(exames, laudos);
        }

        public async Task<string> FormatacaoColetaExame(long coletaId, long exameId)
        {
            var exames = await _resultadoExameRepository.GetAll().Include(i => i.FaturamentoItem)
                                                       .Include(i => i.Formata)
                                                       .Where(w => w.ResultadoId == coletaId && w.Id == exameId)
                                                       .AsNoTracking()
                                                       .ToListAsync();

            if (exames.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var exameIds = exames.Select(x => x.Id).Distinct().ToList();
            var laudos = await _resultadoLaudoRepositorio.GetAll()
                                                         .Where(w => w.ResultadoExameId.HasValue && exameIds.Contains(w.ResultadoExameId.Value))
                                                         .Include(i => i.ItemResultado)
                                                         .Include(i => i.LaboratorioUnidade)
                                                         .OrderBy(x => x.Ordem)
                                                         .AsNoTracking()
                                                         .ToListAsync();

            return await this.RetornaFormatacaoColeta(exames, laudos);
        }

        private async Task<string> RetornaFormatacaoColeta(IEnumerable<ResultadoExame> exames, IEnumerable<ResultadoLaudo> laudosExames)
        {
            var formatacao = string.Empty;
            foreach (var exame in exames)
            {
                formatacao = exame.Formata.Formatacao;
                //Alterando o status dos exames para digitado
                exame.ExameStatusId = (long)EnumStatusExame.Digitado;
                var laudos = laudosExames.Where(x => x.ResultadoExameId == exame.Id).ToList();
                foreach (var item in laudos)
                {
                    //numerico e alfanumeico
                    if (item.TipoResultadoId == (long)EnumTipoResultado.Numerico || item.TipoResultadoId == (long)EnumTipoResultado.Alfanumerico)
                    {
                        formatacao = formatacao.Replace(string.Concat("[", item.ItemResultado.Codigo.TrimEnd(), "]"), item.Resultado);
                    }
                    //Calculado
                    else if (item.TipoResultadoId == (long)EnumTipoResultado.Calculado)
                    {
                        var formula = item.Formula;
                        if (!string.IsNullOrEmpty(formula))
                        {
                            string pattern = Regex.Escape("[") + "(.*?)]";
                            var matches = Regex.Matches(formula, pattern);

                            var valido = true;

                            foreach (Match matche in matches)
                            {
                                var _resultado = laudos.FirstOrDefault(w => w.ItemResultado.Codigo.TrimEnd() == matche.Groups[1].Value);

                                var resultado = _resultado?.Resultado;

                                if (string.IsNullOrEmpty(resultado))
                                {
                                    var formulaNumero = 0d;

                                    if (double.TryParse(matche.Groups[1].Value, out formulaNumero))
                                    {
                                        resultado = formulaNumero.ToString();
                                    }
                                }

                                if (string.IsNullOrEmpty(resultado))
                                {
                                    valido = false;
                                    break;
                                }

                                formula = formula.Replace(matche.Value, resultado.Replace(',', '.'));
                            }

                            if (valido)
                            {
                                var dt = new DataTable();
                                var valorFormula = dt.Compute(formula, "");

                                if (valorFormula.GetType() == typeof(decimal))
                                {
                                    var resultado = (decimal)valorFormula;
                                    formatacao = formatacao.Replace(string.Concat("[", item.ItemResultado.Codigo.TrimEnd(), "]"), resultado.ToString(string.Format("N{0}", item.CasaDecimal)));
                                }
                                else if (valorFormula.GetType() == typeof(double))
                                {
                                    var resultado = (double)valorFormula;
                                    formatacao = formatacao.Replace(string.Concat("[", item.ItemResultado.Codigo.TrimEnd(), "]"), resultado.ToString(string.Format("N{0}", item.CasaDecimal)));
                                }
                                else if (valorFormula.GetType() == typeof(float))
                                {
                                    var resultado = (float)valorFormula;
                                    formatacao = formatacao.Replace(string.Concat("[", item.ItemResultado.Codigo.TrimEnd(), "]"), resultado.ToString(string.Format("N{0}", item.CasaDecimal)));
                                }
                                else if (valorFormula.GetType() == typeof(int))
                                {
                                    var resultado = (int)valorFormula;
                                    formatacao = formatacao.Replace(string.Concat("[", item.ItemResultado.Codigo.TrimEnd(), "]"), resultado.ToString(string.Format("N{0}", item.CasaDecimal)));
                                }
                            }
                        }
                    }

                    //Tabela
                    else if (item.TipoResultadoId == (long)EnumTipoResultado.Tabela)
                    {
                        long tabelaResultadoId;

                        long.TryParse(item.Resultado, out tabelaResultadoId);


                        var tabelaResuldado = _tabelaResultadoRepositoy.GetAll().FirstOrDefault(w => w.Id == tabelaResultadoId);

                        if (tabelaResuldado != null)
                        {
                            formatacao = formatacao.Replace(string.Concat("[", item.ItemResultado.Codigo.TrimEnd(), "]"), tabelaResuldado.Descricao);
                        }
                    }
                }
            }

            return formatacao;
        }

        private long GravarArquivo(long resultadoExameLaboratorioId, string formatacao)
        {
            long id;
            if (string.IsNullOrEmpty(formatacao))
            {
                return 0;
            }

            formatacao = string.Concat("<table><tr><td><br><br><br><br><br><br><br><br><br><br><br><br></td></tr> </table>", formatacao);

            using (var ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(formatacao, PdfSharp.PageSize.A4, 10);

                pdf.Save(ms);

                var registroArquivo = new RegistroArquivo
                {
                    Arquivo = ms.ToArray(),
                    RegistroTabelaId = (long)EnumArquivoTabela.LaboratorioExame,
                    RegistroId = resultadoExameLaboratorioId
                };

                id = this._registroArquivoRepository.InsertAndGetId(registroArquivo);
                _unitOfWorkManager.Current.SaveChanges();
            }

            return id;
        }

        public async Task<PagedResultDto<ExameResultadoDto>> ListarHistorioExamePorPaciente(ExameResultadoInput input)
        {

            var queryHistorio = _resultadoLaudoRepositorio.GetAll()
                                                          .Where(w => w.ResultadoExame.FaturamentoItemId == input.exameId
                                                                  && w.ResultadoExame.Resultado.Atendimento.PacienteId == input.pacienteId)

                                                          .Select(s => new ExameResultadoDto
                                                          {
                                                              DataColeta = s.ResultadoExame.Resultado.DataColeta,
                                                              Exame = s.ItemResultado.Descricao,
                                                              Resultado = s.Resultado,
                                                              TabelaId = s.ItemResultado.TabelaId,
                                                              TipoResultadoId = s.TipoResultadoId
                                                          });

            var contarResultados = await queryHistorio
                                       .CountAsync().ConfigureAwait(false);

            var resultadoLaudos = await queryHistorio
                                      .AsNoTracking()
                                      .OrderByDescending(o => o.DataColeta)
                                      .PageBy(input)
                                      .ToListAsync().ConfigureAwait(false);

            foreach (var item in resultadoLaudos)
            {
                if (item.TipoResultadoId == (long)EnumTipoResultado.Tabela)
                {
                    long resultadoId;

                    if (long.TryParse(item.Resultado, out resultadoId))
                    {

                        var resTabela = _tabelaResultadoRepositoy
                                                 .GetAll()
                                                        .FirstOrDefault(
                                                            w => w.TabelaId == item.TabelaId
                                                                 && w.Id == resultadoId);

                        if (resTabela != null)
                        {
                            item.Resultado = resTabela.Descricao;
                        }

                    }
                }
            }

            return new PagedResultDto<ExameResultadoDto>(
                contarResultados,
                resultadoLaudos
                );

        }

    }
}

