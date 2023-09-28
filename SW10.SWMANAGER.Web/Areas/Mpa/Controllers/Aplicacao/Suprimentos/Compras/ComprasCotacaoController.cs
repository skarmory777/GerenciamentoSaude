﻿using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ComprasRequisicao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Compras;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ComprasCotacaoController : Controller // Web.Controllers.SWMANAGERControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession _abpSession;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly ICompraRequisicaoAppService _compraCotacaoAppService;
        private readonly IFormaPagamentoAppService _formaPagamentoAppService;

        public ComprasCotacaoController(
            IUserAppService userAppService,
            IAbpSession abpSession,
            IEmpresaAppService empresaAppService,
            ICompraRequisicaoAppService compraCotacaoAppService,
            IFormaPagamentoAppService formaPagamentoAppService
            )
        {
            _userAppService = userAppService;
            _abpSession = abpSession;
            _empresaAppService = empresaAppService;
            _compraCotacaoAppService = compraCotacaoAppService;
            _formaPagamentoAppService = formaPagamentoAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new ComprasRequisicaoViewModel(new CompraRequisicaoDto());

            #region Filtro Status Requisicao
            List<GenericoIdNome> statusRequisicao = new List<GenericoIdNome>();
            var opcaoStatusRequisicao1 = new GenericoIdNome();
            opcaoStatusRequisicao1.Id = 1;
            opcaoStatusRequisicao1.Nome = "Encerradas";
            statusRequisicao.Add(opcaoStatusRequisicao1);

            var opcaoStatusRequisicao2 = new GenericoIdNome();
            opcaoStatusRequisicao2.Id = 2;
            opcaoStatusRequisicao2.Nome = "Não Encerradas";
            statusRequisicao.Add(opcaoStatusRequisicao2);
            #endregion

            #region Filtro Status Aprovacao
            List<GenericoIdNome> statusAprovacao = new List<GenericoIdNome>();
            var opcaoStatusAprovacao1 = new GenericoIdNome();
            opcaoStatusAprovacao1.Id = 1;
            opcaoStatusAprovacao1.Nome = "Aprovadas";
            statusAprovacao.Add(opcaoStatusAprovacao1);

            var opcaoStatusAprovacao2 = new GenericoIdNome();
            opcaoStatusAprovacao2.Id = 2;
            opcaoStatusAprovacao2.Nome = "Não Aprovadas";
            statusAprovacao.Add(opcaoStatusAprovacao2);
            #endregion

            model.StatusRequisicao = new SelectList(statusRequisicao, "Id", "Nome");

            model.StatusAprovacao = new SelectList(statusAprovacao, "Id", "Nome");

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Compras/ComprasCotacao/Index.cshtml", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST", "PUT")]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao_Create, AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var userId = _abpSession.UserId.Value;

            var viewModel = new CriarOuEditarCompraRequisicaoViewModel(new CompraRequisicaoDto());

            var isEdicao = id.HasValue;

            if (isEdicao)
            {
                var output = await _compraCotacaoAppService.Obter((long)id);
                viewModel = new CriarOuEditarCompraRequisicaoViewModel(output);

                var itensList = await _compraCotacaoAppService.ListarCotacaoFornecedorItem(id.Value, null);
                viewModel.RequisicoesItensJson = JsonConvert.SerializeObject(itensList.Items.ToList());
            }
            else 
            {
                viewModel.RequisicoesItensJson = JsonConvert.SerializeObject(new List<CompraRequisicaoDto>());
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Compras/ComprasCotacao/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}