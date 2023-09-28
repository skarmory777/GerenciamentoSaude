using Castle.Core.Internal;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.FaturamentoAtendimento
{
    public class FaturarAtendimentoContaMedicaModel : FaturarAtendimentoContaMedicaViewModel
    {
        
    }

    public class FaturarAtendimentoResumoContaMedicaModel
    {
        public static string TipoAberta = "Aberta";
        public static string TipoFechada = "Fechada";
        public FaturarAtendimentoResumoContaMedicaModel(long atendimentoId, long contaMedicaId, string tipo = "Aberta")
        {
            this.AtendimentoId = atendimentoId;
            this.ContaMedicaId = contaMedicaId;
            this.Tipo = tipo;
        }
        public long AtendimentoId { get; set; }

        public long ContaMedicaId { get; set; }


        public string Tipo { get; set; }

    }

    public class RptFaturarAtendimentoResumoContaMedicaModel : FaturarAtendimentoResumoContaMedicaModel
    {
        public CultureInfo Culture { get; set; }
        public RptFaturarAtendimentoResumoContaMedicaModel(long atendimentoId, long contaMedicaId, string tipo = "Aberta") : base(atendimentoId, contaMedicaId, tipo)
        {
            Culture = CultureInfo.CreateSpecificCulture("pt-BR");
        }

        public ResumoContaDto ResumoConta { get; set; }

        public string Url { get; internal set; }

        public string HeaderHtml { get; set; }

        public MvcHtmlString RenderConta()
        {
            if (ResumoConta == null || ResumoConta.ResumoContaGrupo.IsNullOrEmpty())
            {
                return new MvcHtmlString("");
            }

            string RenderHonorario(ResumoDetalhamentoHonorariosPessoaDados dadosProfissonalSaude, float profissionalValor, bool isCredenciado, string tipoProfissional)
            {
                return $@"<div class='row line-data'>
                        <div class='col-xs-1 col-xs-offset-2 text-left'>
                            {Sanitize(tipoProfissional)}
                        </div>
                        <div class='col-xs-8 text-left'>
                            {Sanitize(dadosProfissonalSaude.Conselho)}: ${Sanitize(dadosProfissonalSaude.NumeroConselho)} - CPF: ${Sanitize(dadosProfissonalSaude.Cpf)} - ${Sanitize(dadosProfissonalSaude.Nome)}
                        </div>
                        <div class='col-xs-1 text-right'>
                            {(isCredenciado ? "" : profissionalValor.ToString("C", Culture))}
                        </div>
                    </div>";
            }

            string RenderInfos(string tipo, string texto, float valor)
            {
                return @$"
                <div class='row line-data'>
                     <div class='col-xs-2 text-left' style='margin-left:10px'>
                       {tipo} ({texto})
                     </div>
                     <div class='col-xs-10 text-right' style='margin-left:-10px'>
                        {valor.ToString("C", Culture)}
                     </div>
                 </div>";
            }

            static string Sanitize(string value)
            {
                return string.IsNullOrEmpty(value) ? "" : value;
            }


            var stringBuilder = new StringBuilder();
            stringBuilder = stringBuilder.AppendLine(@"
                <nav class='navbar navbar-default navbar-el' style='margin-bottom: 0px'>
                    <div class='container-fluid'>
                        <div class='row' style='min-height: 50px;line-height: 50px;'>
                        <div class='col-xs-2 text-center'>
                            <h5>Código</h5>
                        </div>
                        <div class='col-xs-5 text-left'>
                            <h5>Descrição</h5>
                        </div>
                        <div class='col-xs-1 text-right'>
                            <h5>Qtde</h5>
                        </div>
                        <div class='col-xs-1 text-right'>
                            <h5>Preco Unitario</h5>
                        </div>
                        <div class='col-xs-1 text-right'>
                            <h5>Valor Total</h5>
                        </div>
                        <div class='col-xs-1 text-right'>
                            <h5>Percentual</h5>
                        </div>
                        <div class='col-xs-1 text-right'>
                            <h5>Total</h5>
                        </div>
                    </div>
                    </div>
                </nav>");

            foreach (var grupo in ResumoConta.ResumoContaGrupo.OrderBy(x => x.GrupoOrdem))
            {
                var totalGrupo = grupo.ResumoContaSubGrupo.SelectMany(x => x.ResumoContaItem.Select(z => z.ResumoDetalhamento.ValorTotal)).Sum();
                stringBuilder = stringBuilder.AppendLine($"<div class='row'><h3 class='text-center bold' style='height: 50px;line-height: 50px;'> {grupo.GrupoDescricao}</h3> </div>");
                foreach (var subGrupo in grupo.ResumoContaSubGrupo.OrderBy(x => x.SubGrupoId))
                {
                    stringBuilder = stringBuilder.AppendLine($"<div class='row'><h4 class='text-center bold' style='height: 50px;line-height: 50px;color:#337ab7'> {subGrupo.SubGrupoDescricao}</h4> </div>");

                    foreach (var subItem in subGrupo.ResumoContaItem)
                    {
                        stringBuilder = stringBuilder.AppendLine($@"
                        <div class='row line-data'>
                            <div class='col-xs-2 text-center'>
                                {subItem.FatContaItemCodigo}
                            </div>
                            <div class='col-xs-5 text-left'>
                                {subItem.FatContaItemDescricao}
                            </div>
                            <div class='col-xs-1 text-right'>
                                {subItem.ResumoDetalhamento.Qtde.ToString("G", Culture)}
                            </div>
                            <div class='col-xs-1 text-right'>
                                {(subItem.ResumoDetalhamento.Moeda?.Codigo == "R$"
                                    ? subItem.ResumoDetalhamento.ValorTotal.ToString("C", Culture) :
                                    subItem.ResumoDetalhamento.Qtde.ToString("G", Culture))} {subItem.ResumoDetalhamento.Moeda?.Codigo}
                            </div>
                            <div class='col-xs-1 text-right'>
                                {subItem.ResumoDetalhamento.ValorTotal.ToString("C", Culture)}
                            </div>
                            <div class='col-xs-1 text-right'>
                                {subItem.ResumoDetalhamento.Percentual.ToString("G", Culture)}%
                            </div>
                            <div class='col-xs-1 text-right'>
                                {subItem.ResumoDetalhamento.Valor.ToString("C", Culture)}
                            </div>
                        </div>");

                        if (subItem.ResumoDetalhamento.Honorarios != null)
                        {
                            if (subItem.ResumoDetalhamento.Honorarios.HasMedico)
                            {
                                stringBuilder = stringBuilder.AppendLine(
                                    RenderHonorario(
                                    subItem.ResumoDetalhamento.Honorarios.DadosMedico,
                                    subItem.ResumoDetalhamento.Honorarios.MedicoValor,
                                    subItem.ResumoDetalhamento.Honorarios.MedicoIsCredenciado, "Médico"));
                            }

                            if (subItem.ResumoDetalhamento.Honorarios.HasAuxiliar1)
                            {
                                stringBuilder = stringBuilder.AppendLine(
                                    RenderHonorario(
                                    subItem.ResumoDetalhamento.Honorarios.DadosAuxiliar1,
                                    subItem.ResumoDetalhamento.Honorarios.Auxiliar1Valor,
                                    subItem.ResumoDetalhamento.Honorarios.Auxiliar1IsCredenciado, "1° Auxiliar"));
                            }

                            if (subItem.ResumoDetalhamento.Honorarios.HasAuxiliar2)
                            {
                                stringBuilder = stringBuilder.AppendLine(
                                    RenderHonorario(
                                    subItem.ResumoDetalhamento.Honorarios.DadosAuxiliar2,
                                    subItem.ResumoDetalhamento.Honorarios.Auxiliar2Valor,
                                    subItem.ResumoDetalhamento.Honorarios.Auxiliar2IsCredenciado, "2° Auxiliar"));
                            }

                            if (subItem.ResumoDetalhamento.Honorarios.HasAuxiliar3)
                            {
                                stringBuilder = stringBuilder.AppendLine(
                                    RenderHonorario(
                                    subItem.ResumoDetalhamento.Honorarios.DadosAuxiliar3,
                                    subItem.ResumoDetalhamento.Honorarios.Auxiliar3Valor,
                                    subItem.ResumoDetalhamento.Honorarios.Auxiliar3IsCredenciado, "3° Auxiliar"));
                            }

                            if (subItem.ResumoDetalhamento.Honorarios.HasInstrumentador)
                            {
                                stringBuilder = stringBuilder.AppendLine(
                                    RenderHonorario(
                                    subItem.ResumoDetalhamento.Honorarios.DadosInstrumentador,
                                    subItem.ResumoDetalhamento.Honorarios.InstrumentadorValor,
                                    subItem.ResumoDetalhamento.Honorarios.InstrumentadorIsCredenciado, "Instrumentador"));
                            }

                            if (subItem.ResumoDetalhamento.Honorarios.HasInstrumentador)
                            {
                                stringBuilder = stringBuilder.AppendLine(
                                    RenderHonorario(
                                    subItem.ResumoDetalhamento.Honorarios.DadosAnestesista,
                                    subItem.ResumoDetalhamento.Honorarios.AnestesistaValor,
                                    subItem.ResumoDetalhamento.Honorarios.AnestesistaIsCredenciado, "Anestesista"));
                            }
                        }

                        if(subItem.ResumoDetalhamento.ValorFilme != 0)
                        {
                            stringBuilder = stringBuilder.AppendLine(RenderInfos("Filme", subItem.ResumoDetalhamento.MetragemFilme.ToString("G", Culture), subItem.ResumoDetalhamento.ValorFilme));
                        }

                        if (subItem.ResumoDetalhamento.ValorCOCH != 0)
                        {
                            stringBuilder = stringBuilder.AppendLine(RenderInfos("COCH", subItem.ResumoDetalhamento.COCH.ToString("G", Culture), subItem.ResumoDetalhamento.ValorCOCH));
                        }

                        if (subItem.ResumoDetalhamento.ValorHMCH != 0)
                        {
                            stringBuilder = stringBuilder.AppendLine(RenderInfos("HMCH", subItem.ResumoDetalhamento.HMCH.ToString("G", Culture), subItem.ResumoDetalhamento.ValorHMCH));
                        }

                        if (!string.IsNullOrEmpty(subItem.Observacao))
                        {
                            stringBuilder = stringBuilder.AppendLine($@"
                            <div class='row line-data'>
                                 <div class='col-xs-12 text-left' style='margin-left:10px'>
                                    <b>{subItem.Observacao}</b>
                                 </div>
                             </div>");
                        }

                    }


                    var totalSubGrupo = subGrupo.ResumoContaItem.Sum(x => x.ResumoDetalhamento.ValorTotal);
                    stringBuilder = stringBuilder.AppendLine($@"<div class='row total'>
                        <div class='col-xs-10 text-left'>
                            <h5 class='bold' style='color:#337ab7'> {subGrupo.SubGrupoDescricao}</h5>
                        </div>
                        <div class='col-xs-2 text-right' style='border-top: solid'>
                            <h6 class='bold valor' style='color:#337ab7'> {totalSubGrupo.ToString("C", Culture)}</h6>
                        </div>
                    </div>");
                }

                stringBuilder = stringBuilder.AppendLine($@"<div class='row total'>
                    <div class='col-xs-10 text-left'>
                        <h4 class='bold'> Total de {grupo.GrupoDescricao}</h4>
                    </div>
                    <div class='col-xs-2 text-right' style='border-top: solid'>
                        <h5 class='bold valor'> {totalGrupo.ToString("C", Culture)}</h5>
                    </div>
                </div>");
            }
            var total = ResumoConta.ResumoContaGrupo.SelectMany(c => c.ResumoContaSubGrupo.SelectMany(x => x.ResumoContaItem.Select(z => z.ResumoDetalhamento.ValorTotal))).Sum();
            stringBuilder = stringBuilder.AppendLine($@"<div class='row total' style='margin: 50px 0' >
                  <div class='col-xs-10 text-left'>
                      <h3 class='bold'> TOTAL DO PERIODO</h3>
                  </div>
                  <div class='col-xs-2 text-right' style='border-top: solid'>
                      <h5 class='bold valor'> {total.ToString("C", Culture)}</h5>
                  </div>
                </div>");

            return new MvcHtmlString(stringBuilder.ToString());

            

        }
    }
}