#region Usings
using Abp.Web.Security.AntiForgery;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Relatorios;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos.Relatorios
{
    [DisableAbpAntiForgeryTokenValidation]
    public class FaturamentoRelatoriosController : SWMANAGERControllerBase
    {
        #region Cabecalho
        private IRelatorioFaturamentoAppService _relatorioFaturamentoAppService;
        private ISessionAppService _sessionAppService;
        private readonly IUserAppService _userAppService;
        public FaturamentoRelatoriosController(
            IRelatorioFaturamentoAppService relatorioFaturamentoAppService
            , 
            ISessionAppService sessionAppService
            , 
            IUserAppService userAppService)
        {
            _relatorioFaturamentoAppService = relatorioFaturamentoAppService;
            _sessionAppService = sessionAppService;
            _userAppService = userAppService;
        }
        #endregion cabecalho.
        
        public async Task<ActionResult> Index()
        {
            FiltroModel result = null;// await CarregarIndex();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Relatorios/Index.csHtml", result);
        }
        
        public async Task<ActionResult> Visualizar(FiltroModel filtro)
        {
            //var relatorioInternacao = await _relatorioFaturamentoAppService.ListarRelatorio(filtro.Empresa);

            //var loginInformations = await _sessionAppService.GetCurrentLoginInformations();
         
            FiltroModel model = new FiltroModel();



            //if (relatorioInternacao.Items.Count != 0)
            //{
            //    _filtro.Titulo = string.Concat("Mapa Diário - ", Convert.ToString(DateTime.Now));
            //    _filtro.NomeHospital = relatorioInternacao.Items[0].Empresa.NomeFantasia.ToString();
            //    _filtro.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
            //    _filtro.DataHora = Convert.ToString(DateTime.Now);


            //    _filtro.Lista = relatorioInternacao.Items.Select(m => new TesteObjeto
            //    {
            //        CodAtendimento = m.Codigo,
            //        CodPaciente = m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
            //        Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,
            //        DataInternacao = m.DataRegistro.ToString(),
            //        Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,
            //        Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
            //        Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
            //        Origem = m.Origem == null ? string.Empty : m.Origem.Descricao,
            //        Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
            //        UnidOrganizacional = m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao,
            //        Idade = m.Paciente == null ? string.Empty : Convert.ToString(DateTime.Now.Year - m.Paciente.Nascimento.Year),
            //        DiasInternado = m.DataRegistro == null ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0))
            //    }
            //    ).ToList();
            //}
           
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Relatorios/RelatorioContaMedica.aspx", model);
        }

        //private async Task<FiltroModel> CarregarIndex()
        //{
        //    var loginInformations = await _sessionAppService.GetCurrentLoginInformations();

        //    var userId = AbpSession.UserId;
           
        //    FiltroModel result = new FiltroModel();

        //    result.Empresas = ( _relatorioFaturamentoAppService.ListarEmpresaUsuario(userId.Value))
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.Id.ToString(),
        //            Text = s.Nome
        //        }).ToList();

        //    var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
        //    result.Empresas.Insert(0, padrao);
        //    return result;
        //}

        //private void ProcessarDadosInternacao(FiltroModel filtro)
        //{
        //    var db = new Core.DataSetReportsTableAdapters.RelatorioMovimentoAdapter();

        //    var grupo = filtro.GrupoProduto.GetValueOrDefault();
        //    var classe = filtro.Classe.GetValueOrDefault();
        //    var subClasse = filtro.SubClasse.GetValueOrDefault();
        //    var query = db.GetData().Where(w => w.GrupoId == grupo);

        //    if (classe != 0)
        //    {
        //        query = query.Where(w => w.GrupoClasseId == classe);
        //    }

        //    if (subClasse != 0)
        //    {
        //        query = query.Where(w => w.GrupoSubClasseId == subClasse);
        //    }

        //    filtro.DadosMovimentacao = query.ToList();
        //}
    }
}