using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Atendimentos
{
    public class MovimentosAutomaticosController : SWMANAGERControllerBase
    {

        private readonly IMovimentoAutomaticoAppService _movimentoAutomaticoAppService;

        public MovimentosAutomaticosController(IMovimentoAutomaticoAppService movimentoAutomaticoAppService)
        {
            _movimentoAutomaticoAppService = movimentoAutomaticoAppService;
        }

        public ActionResult Index()
        {
            var model = new MovimentoAutomaticoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/MovimentosAutomaticos/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            MovimentoAutomaticoViewModel viewModel;

            if (id == null || id == 0)
            {

                viewModel = new MovimentoAutomaticoViewModel();
                viewModel.TiposLeitos = JsonConvert.SerializeObject(new List<TipoLeitoIndex>());
                viewModel.TiposGuias = JsonConvert.SerializeObject(new List<TipoGuiaIndex>());
                viewModel.Especialidades = JsonConvert.SerializeObject(new List<EspecialidadeIndex>());
                viewModel.ConveniosPlanos = JsonConvert.SerializeObject(new List<ConvenioPlanoIndex>());
            }
            else
            {
                var movimentoAutomatico = await _movimentoAutomaticoAppService.Obter((long)id);
                viewModel = new MovimentoAutomaticoViewModel(movimentoAutomatico);

                long i = 0;
                var lstTiposGuias = movimentoAutomatico.MovimentosAutomaticosTiposGuias.Select(s => new TipoGuiaIndex
                {
                    Id = s.Id,
                    TipoGuiaId = s.FaturamentoGuiaId,
                    TipoGuiaDescricao = string.Concat(s.FaturamentoGuia.Codigo, " - ", s.FaturamentoGuia.Descricao),
                    IdGrid = i++
                });
                viewModel.TiposGuias = JsonConvert.SerializeObject(lstTiposGuias);



                i = 0;
                var lstEspecialidades = movimentoAutomatico.MovimentosAutomaticosEspecialidades.Select(s => new EspecialidadeIndex
                {
                    Id = s.Id,
                    EspecialidadeId = s.EspecialidadeId,
                    EspecialidadeDescricao = string.Concat(s.Especialidade.Codigo, " - ", s.Especialidade.Descricao),
                    IdGrid = i++
                });
                viewModel.Especialidades = JsonConvert.SerializeObject(lstEspecialidades);

                if (movimentoAutomatico.MovimentosAutomaticosFaturamentosItens != null && movimentoAutomatico.MovimentosAutomaticosFaturamentosItens.Count > 0)
                {
                    var fatItem = movimentoAutomatico.MovimentosAutomaticosFaturamentosItens[0].FaturamentoItem;

                    viewModel.FaturamentoItemId = fatItem.Id;
                    viewModel.FaturamentoItem = fatItem;
                }



                i = 0;
                var lstConveniosPlanos = movimentoAutomatico.MovimentosAutomaticosConveiosPlanos.Select(s => new ConvenioPlanoIndex
                {
                    Id = s.Id,
                    ConvenioId = s.ConvenioId,
                    ConvenioDescricao = s.Convenio?.NomeFantasia,
                    PlanoId = s.PlanoId,
                    PlanoDescricao = s.Plano?.Descricao,
                    //EspecialidadeId = s.EspecialidadeId,
                    //EspecialidadeDescricao = string.Concat(s.Especialidade.Codigo, " - ", s.Especialidade.Descricao),
                    IdGrid = i++
                });
                viewModel.ConveniosPlanos = JsonConvert.SerializeObject(lstConveniosPlanos);


            }

            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/MovimentosAutomaticos/_CriarOuEditarModal.cshtml", viewModel);
        }


        public JsonResult Salvar(MovimentoAutomaticoDto input)
        {
            try
            {
                //  var preMovimento = JsonConvert.DeserializeObject<EstoquePreMovimentoDto>(input, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                var result = _movimentoAutomaticoAppService.CriarOuEditar(input);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}