namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using HeyRed.Mime;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Impressora;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Filas;
    using SW10.SWMANAGER.Web.Controllers;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <inheritdoc />
    /// <summary>
    /// The terminal senhas controller.
    /// </summary>
    public class TerminalSenhasController : SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index()
        {
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            using (var tipoLocalChamadaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoLocalChamada, long>>())
            {
                try
                {
                    var empresa = await empresaAppService.Object.Obter(1).ConfigureAwait(false);
                    var locaisDto = (await tipoLocalChamadaRepository.Object.GetAll().AsNoTracking().ToListAsync().ConfigureAwait(false))
                        .Select(x => new TipoLocalChamadaIndex()
                        {
                            Id = x.Id,
                            TipoLocalChamadaDescricao = x.Descricao
                        })
                        .OrderBy(x => x.TipoLocalChamadaDescricao)
                        .ToList();
                    locaisDto.Insert(0, new TipoLocalChamadaIndex() { Id = null, TipoLocalChamadaDescricao = "Todos" });
                    var viewModel = new EscolherTermialSenhaViewModel
                    {
                        Locais = locaisDto
                    };


                    var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.TempPath"].ToString());

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    if (empresa != null)
                    {
                        var extension = MimeTypesMap.GetExtension(empresa.LogotipoMimeType);
                        var logoFileName = $"logo_empresa_{((EmpresaAppService)empresaAppService.Object).GetCurrentTenant().Id}_{empresa.Id}.{extension}";
                        System.IO.File.WriteAllBytes(Path.Combine(tempPath, logoFileName), empresa.Logotipo);
                        viewModel.UrlPath = $"/{ConfigurationManager.AppSettings["App.TempPath"]}/{logoFileName}";
                    }
                    return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/TerminalSenhas/Index.cshtml", viewModel);
                }
                catch (Exception e)
                {
                    this.Logger.Error(e.StackTrace);
                    throw;
                }
            }
        }

        public async Task<ActionResult> TerminalDeSenha(long? id)
        {
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            using (var terminalSenhasAppService = IocManager.Instance.ResolveAsDisposable<ITerminalSenhasAppService>())
            {
                try
                {
                    var empresa = await empresaAppService.Object.Obter(1).ConfigureAwait(false);
                    var viewModel = new TermialSenhaViewModel
                    {
                        Filas = terminalSenhasAppService.Object.ListarFilasDisponiveis(id)
                    };

                    var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.TempPath"].ToString());

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    if (empresa != null)
                    {
                        var extension = MimeTypesMap.GetExtension(empresa.LogotipoMimeType);
                        var logoFileName = $"logo_empresa_{((EmpresaAppService)empresaAppService.Object).GetCurrentTenant().Id}_{empresa.Id}.{extension}";
                        System.IO.File.WriteAllBytes(Path.Combine(tempPath, logoFileName), empresa.Logotipo);
                        viewModel.UrlPath = $"/{ConfigurationManager.AppSettings["App.TempPath"]}/{logoFileName}";
                    }
                    return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/TerminalSenhas/TerminalDeSenha.cshtml", viewModel);
                }
                catch (Exception e)
                {
                    this.Logger.Error(e.StackTrace);
                    throw;
                }
            }
        }

        /// <summary>
        /// The alteracao tipo local chamada.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult AlteracaoTipoLocalChamada()
        {
            var viewModel = new FilaViewModel(new FilaDto());

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/FilasSenhas/Index.cshtml", viewModel);
        }

        /// <summary>
        /// The imprimir senha.
        /// </summary>
        /// <param name="tipoLocalChamada">
        /// The tipo local chamada.
        /// </param>
        /// <param name="numero">
        /// The numero.
        /// </param>
        /// <param name="hospital">
        /// The hospital.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public async Task<string> ImprimirSenha(string tipoLocalChamada, string numero, string hospital)
        {
            using (var modeloTextoAppService = IocManager.Instance.ResolveAsDisposable<IModeloTextoAppService>())
            using (var impressoraAppService = IocManager.Instance.ResolveAsDisposable<IImpressoraArquivosAppService>())
            {
                var modelo = await modeloTextoAppService.Object.ObterPorTipoAsync((long)EnumTipoModelo.TerminalSenha).ConfigureAwait(false);

                if (modelo == null)
                {
                    throw new Exception("Não há modelo de impressão");
                }

                var texto = modelo.Texto.MergeTexto("NomeHospital", hospital).MergeTexto("Numero", numero)
                    .MergeTexto("Fila", tipoLocalChamada).MergeTexto("Data", DateTime.Now.ToString("dd/MM/yyyy"));

                var uuidPdf = $"_TerminalDeSenha-{Guid.NewGuid()}.pdf";

                var printerCookie = ImpressoraHelper.CookiePorModelo(EnumTipoModelo.TerminalSenha);

                if (this.HttpContext.Request.Cookies.AllKeys.Any(x => x == printerCookie))
                {
                    var printerName = this.HttpContext.Server.UrlDecode(
                        this.HttpContext.Request.Cookies.Get(printerCookie)?.Value);

                    var file = modelo.GerarPdf(texto);

                    impressoraAppService.Object.EnviarParaImprimir(printerName, file, uuidPdf, 2);
                }

                return uuidPdf;
            }
        }
    }
}