namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    using Abp.AutoMapper;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Receituarios;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
    using System;

    [AutoMap(typeof(ReceituarioMedico))]
    public class ReceituarioMedicoDto : CamposPadraoCRUDDto
    {
        public long AtendimentoId { get; set; }

        public DateTime DataReceituario { get; set; }

        public long MedicoId { get; set; }
        public Medico Medico { get; set; }

        public static ReceituarioMedicoDto Mapear(ReceituarioMedico entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ReceituarioMedicoDto>(entity);
            dto.AtendimentoId = entity.AtendimentoId;
            dto.DataReceituario = entity.DataReceituario;
            dto.MedicoId = entity.MedicoId;
            dto.Id = entity.Id;
            dto.Medico = entity.Medico;

            return dto;
        }
    }
}
