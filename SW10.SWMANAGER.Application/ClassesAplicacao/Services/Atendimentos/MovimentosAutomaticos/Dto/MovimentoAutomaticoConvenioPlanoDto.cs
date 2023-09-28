using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto
{
    public class MovimentoAutomaticoConvenioPlanoDto : CamposPadraoCRUDDto
    {
        public long MovimentoAutomaticoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? PlanoId { get; set; }

        public MovimentoAutomaticoDto MovimentoAutomatico { get; set; }
        public ConvenioDto Convenio { get; set; }
        public PlanoDto Plano { get; set; }

        public static MovimentoAutomaticoConvenioPlanoDto Mapear(MovimentoAutomaticoConvenioPlano movimentoAutomaticoConvenioPlano)
        {
            MovimentoAutomaticoConvenioPlanoDto movimentoAutomaticoConvenioPlanoDto = new MovimentoAutomaticoConvenioPlanoDto();

            movimentoAutomaticoConvenioPlanoDto.Id = movimentoAutomaticoConvenioPlano.Id;
            movimentoAutomaticoConvenioPlanoDto.Codigo = movimentoAutomaticoConvenioPlano.Codigo;
            movimentoAutomaticoConvenioPlanoDto.Descricao = movimentoAutomaticoConvenioPlano.Descricao;
            movimentoAutomaticoConvenioPlanoDto.MovimentoAutomaticoId = movimentoAutomaticoConvenioPlano.MovimentoAutomaticoId;
            movimentoAutomaticoConvenioPlanoDto.ConvenioId = movimentoAutomaticoConvenioPlano.ConvenioId;
            movimentoAutomaticoConvenioPlanoDto.PlanoId = movimentoAutomaticoConvenioPlano.PlanoId;

            if (movimentoAutomaticoConvenioPlano.Convenio != null)
            {
                movimentoAutomaticoConvenioPlanoDto.Convenio = ConvenioDto.Mapear(movimentoAutomaticoConvenioPlano.Convenio);
            }

            if (movimentoAutomaticoConvenioPlano.Plano != null)
            {
                movimentoAutomaticoConvenioPlanoDto.Plano = PlanoDto.Mapear(movimentoAutomaticoConvenioPlano.Plano);
            }

            return movimentoAutomaticoConvenioPlanoDto;
        }

        public static List<MovimentoAutomaticoConvenioPlanoDto> Mapear(List<MovimentoAutomaticoConvenioPlano> movimentosAutomaticosConveiosPlanos)
        {
            List<MovimentoAutomaticoConvenioPlanoDto> movimentosAutomaticosConveiosPlanosDto = new List<MovimentoAutomaticoConvenioPlanoDto>();

            foreach (var item in movimentosAutomaticosConveiosPlanos)
            {
                movimentosAutomaticosConveiosPlanosDto.Add(Mapear(item));
            }

            return movimentosAutomaticosConveiosPlanosDto;
        }
    }
}
