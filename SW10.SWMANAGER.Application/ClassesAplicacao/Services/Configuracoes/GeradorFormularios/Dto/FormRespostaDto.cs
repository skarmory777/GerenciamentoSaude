using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    using System.Linq;

    [AutoMap(typeof(FormResposta))]
    public class FormRespostaDto : CamposPadraoCRUDDto
    {
        public DateTime DataResposta { get; set; }

        public long FormConfigId { get; set; }

        public FormConfigDto FormConfig { get; set; }

        public string NomeClasse { get; set; }

        public string RegistroClasseId { get; set; }

        public bool IsPreenchido { get; set; }

        public List<FormDataDto> ColRespostas { get; set; }


        public static FormRespostaDto Mapear(FormResposta entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FormRespostaDto>(entity);

            dto.DataResposta = entity.DataResposta;

            dto.FormConfigId = entity.FormConfigId;

            dto.FormConfig = FormConfigDto.Mapear(entity.FormConfig);

            dto.NomeClasse = entity.NomeClasse;

            dto.RegistroClasseId = entity.RegistroClasseId;

            dto.IsPreenchido = entity.IsPreenchido;

            dto.ColRespostas = entity.ColRespostas?.ToList().Select(FormDataDto.Mapear).ToList();

            return dto;

        }
    }
}