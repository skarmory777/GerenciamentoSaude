using Newtonsoft.Json;

using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class QuitacaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        private readonly ILancamentoAppService _lancamentoAppService;

        public QuitacaoController(ILancamentoAppService lancamentoAppService)
        {
            _lancamentoAppService = lancamentoAppService;
        }


        public async Task<ActionResult> CriarOuEditarModalPorLancamentos(List<long> ids)
        {
            var viewModel = new QuitacaoViewModel(new QuitacaoDto());
            var lancamentosIndex = new List<QuitacaoIndex>();
            var lancamentosDto = _lancamentoAppService.ObterLancamentos(ids);

            if (!lancamentosDto.IsNullOrEmpty())
            {
                viewModel.EmpresaId = lancamentosDto.FirstOrDefault()?.Documento?.EmpresaId;
                if (viewModel.EmpresaId.HasValue)
                {
                    using var empresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Empresa, long>>();
                    var empresa = await empresaRepository.Object.GetAsync(viewModel.EmpresaId.Value).ConfigureAwait(false);
                    viewModel.Empresa = EmpresaDto.Mapear(empresa);
                }
            }

            #region Lista Lançamentos
            long idGrid = 0;

            foreach (var item in lancamentosDto)
            {
                var quitacaoIndex = new QuitacaoIndex();

                quitacaoIndex.LancamentoId = item.Id;
                quitacaoIndex.DataVencimento = item.DataVencimento;
                //lancamentoIndex.Juros = item.Juros;
                //lancamentoIndex.Multa = item.Multa;
                //lancamentoIndex.ValorAcrescimoDecrescimo = item.ValorAcrescimoDecrescimo;
                quitacaoIndex.ValorLancamento = item.ValorLancamento;
                quitacaoIndex.ValorRestante = item.ValorRestante;
                quitacaoIndex.ValorRestanteEfetivado = item.ValorRestante;
                quitacaoIndex.Parcela = item.Parcela;
                quitacaoIndex.Documento = item.Documento.Numero;
                quitacaoIndex.Fornecedor = (item.Documento.Pessoa.FisicaJuridica == "F") ? item.Documento.Pessoa.NomeCompleto : item.Documento.Pessoa.NomeFantasia;
                quitacaoIndex.IdGrid = idGrid++;

                lancamentosIndex.Add(quitacaoIndex);
            }
            #endregion

            viewModel.LancamentosJson = JsonConvert.SerializeObject(lancamentosIndex);

            if (lancamentosDto.FirstOrDefault().IsCredito)
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasReceber/_CriarOuEditarModalQuitacaoLancamentos.cshtml", viewModel);
            
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasPagar/_CriarOuEditarModalQuitacaoLancamentos.cshtml", viewModel);
        }

    }
}