using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Prescricoes
{
    using SW10.SWMANAGER.ClassesAplicacao.Services;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
    using System;
    using System.Linq;

    public class PrescricaoCompletaViewModel
    {
        public long AtendimentoId { get; set; }

        public long PrescricaoMedicaId { get; set; }

        public MedicoDto MedicoCorrente { get; set; }

        public IEnumerable<PrescricaoItemRespostaViewModel> PrescricaoItemRespostas { get; set; }

        public IEnumerable<DivisaoDto> Divisoes { get; set; }


        public Dictionary<DivisaoDto,IEnumerable<PrescricaoItemRespostaViewModel>> AgruparRespostasPorDivisao()
        {
            var result = new Dictionary<DivisaoDto, IEnumerable<PrescricaoItemRespostaViewModel>>();

            var groupItems = PrescricaoItemRespostas.GroupBy(x => x.DivisaoId).Select(x=> new { DivisaoId = x.Key, Items = x.AsEnumerable() });
            foreach(var divisao in Divisoes.OrderBy(x=> x.Ordem)) {
                if (groupItems.Any(x => x.DivisaoId == divisao.Id))
                {
                    result.Add(divisao,
                        groupItems.FirstOrDefault(x => x.DivisaoId == divisao.Id).Items.OrderBy(x => x.Id));
                }
            }

            return result;
        }

        public Dictionary<DateTime, IEnumerable<PrescricaoItemRespostaViewModel>> AgruparPorAcrescimosESuspensoes()
        {
            var result = new Dictionary<DateTime, IEnumerable<PrescricaoItemRespostaViewModel>>();

            var groupItems = PrescricaoItemRespostas
                .Where(x=> x.IsAcrescimo || x.IsSuspenso)
                .Where(x=> x.DataAgrupamento.HasValue)
                .GroupBy(x => x.DataAgrupamento.Value)
                .Select(x=> new { DataAgrupamento = x.Key, Items = x.AsEnumerable() });
            foreach (var item in groupItems)
            {
                result.Add(item.DataAgrupamento, item.Items);
            }

            return result;
        }
    }

    public class PrescricaoFormItemRespostaViewModel: PrescricaoItemRespostaViewModel
    {
        public long AtendimentoId { get; set; }
        public MedicoDto  MedicoCorrente { get; set; }
        public DateTime DataCorrente { get; set; }

        public string HoraInicioPrescricao { get; set; }
    }

    
}