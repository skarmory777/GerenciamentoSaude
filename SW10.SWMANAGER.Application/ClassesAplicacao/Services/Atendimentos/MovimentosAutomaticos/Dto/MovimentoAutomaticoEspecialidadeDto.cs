using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto
{
    public class MovimentoAutomaticoEspecialidadeDto : CamposPadraoCRUDDto
    {
        public long MovimentoAutomaticoId { get; set; }
        public long EspecialidadeId { get; set; }

        public MovimentoAutomaticoDto MovimentoAutomatico { get; set; }
        public EspecialidadeDto Especialidade { get; set; }

        public static MovimentoAutomaticoEspecialidadeDto Mapear(MovimentoAutomaticoEspecialidade movimentoAutomaticoEspecialidade)
        {
            MovimentoAutomaticoEspecialidadeDto movimentoAutomaticoEspecialidadeDto = new MovimentoAutomaticoEspecialidadeDto();

            movimentoAutomaticoEspecialidadeDto.Id = movimentoAutomaticoEspecialidade.Id;
            movimentoAutomaticoEspecialidadeDto.Codigo = movimentoAutomaticoEspecialidade.Codigo;
            movimentoAutomaticoEspecialidadeDto.Descricao = movimentoAutomaticoEspecialidade.Descricao;
            movimentoAutomaticoEspecialidadeDto.MovimentoAutomaticoId = movimentoAutomaticoEspecialidade.MovimentoAutomaticoId;
            movimentoAutomaticoEspecialidadeDto.EspecialidadeId = movimentoAutomaticoEspecialidade.EspecialidadeId;

            if (movimentoAutomaticoEspecialidade.Especialidade != null)
            {
                movimentoAutomaticoEspecialidadeDto.Especialidade = EspecialidadeDto.Mapear(movimentoAutomaticoEspecialidade.Especialidade);
            }

            return movimentoAutomaticoEspecialidadeDto;
        }

        public static List<MovimentoAutomaticoEspecialidadeDto> Mapear(List<MovimentoAutomaticoEspecialidade> movimentoAutomaticoEspecialidade)
        {
            List<MovimentoAutomaticoEspecialidadeDto> movimentoAutomaticoEspecialidadeDto = new List<MovimentoAutomaticoEspecialidadeDto>();

            foreach (var item in movimentoAutomaticoEspecialidade)
            {
                movimentoAutomaticoEspecialidadeDto.Add(Mapear(item));
            }

            return movimentoAutomaticoEspecialidadeDto;
        }

    }
}
