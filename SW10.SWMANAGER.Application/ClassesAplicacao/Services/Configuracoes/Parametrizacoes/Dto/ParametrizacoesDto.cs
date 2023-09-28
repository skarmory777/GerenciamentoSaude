using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto
{
    public class ParametrizacoesDto : CamposPadraoCRUDDto
    {
        public bool IsHabilitaControleDeIp { get; set; }
        
        public TimeSpan? SolicitacaoExameHoraOutroDia { get; set; }
        
        public TimeSpan? PrescricaoMedicaHoraOutroDia { get; set; }
        
        public bool IsHabilitaAssistencialColetaAutomatica { get; set; }


        public static Parametrizacao Mapear(ParametrizacoesDto input)
        {
            if(input == null)
            {
                return null;
            }

            var entity = MapearBase<Parametrizacao>(input);
            entity.IsHabilitaControleDeIp = input.IsHabilitaControleDeIp;
            entity.PrescricaoMedicaHoraOutroDia = input.PrescricaoMedicaHoraOutroDia;
            entity.SolicitacaoExameHoraOutroDia = input.SolicitacaoExameHoraOutroDia;
            entity.IsHabilitaAssistencialColetaAutomatica = input.IsHabilitaAssistencialColetaAutomatica;
            return entity;
        }

        public static ParametrizacoesDto Mapear(Parametrizacao input)
        {
            if (input == null)
            {
                return null;
            }

            var dto = MapearBase<ParametrizacoesDto>(input);
            dto.IsHabilitaControleDeIp = input.IsHabilitaControleDeIp;
            dto.PrescricaoMedicaHoraOutroDia = input.PrescricaoMedicaHoraOutroDia;
            dto.SolicitacaoExameHoraOutroDia = input.SolicitacaoExameHoraOutroDia;
            dto.IsHabilitaAssistencialColetaAutomatica = input.IsHabilitaAssistencialColetaAutomatica;
            return dto;
        }
    }
}
