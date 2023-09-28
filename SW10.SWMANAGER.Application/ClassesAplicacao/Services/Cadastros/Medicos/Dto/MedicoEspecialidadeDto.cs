using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto
{
    [AutoMap(typeof(MedicoEspecialidade))]
    //public class MedicoEspecialidadeDto : EntityDto<long>
    public class MedicoEspecialidadeDto : CamposPadraoCRUDDto
    {
        public long MedicoId { get; set; }
        public MedicoDto Medico { get; set; }

        public long EspecialidadeId { get; set; }
        public EspecialidadeDto Especialidade { get; set; }

        public long? IdGridMedicoEspecialidade { get; set; }

        public static MedicoEspecialidadeDto Mapear(MedicoEspecialidade medicoEspecialidade)
        {
            MedicoEspecialidadeDto medicoEspecialidadeDto = new MedicoEspecialidadeDto();

            medicoEspecialidadeDto.Id = medicoEspecialidade.Id;
            medicoEspecialidadeDto.Codigo = medicoEspecialidade.Codigo;
            medicoEspecialidadeDto.Descricao = medicoEspecialidade.Descricao;
            medicoEspecialidadeDto.MedicoId = medicoEspecialidade.MedicoId ?? 0;
            medicoEspecialidadeDto.EspecialidadeId = medicoEspecialidade.EspecialidadeId;

            if (medicoEspecialidade.Especialidade != null)
            {
                medicoEspecialidadeDto.Especialidade = EspecialidadeDto.Mapear(medicoEspecialidade.Especialidade);
            }

            return medicoEspecialidadeDto;
        }

        public static MedicoEspecialidade Mapear(MedicoEspecialidadeDto dto)
        {
            var medicoEspecialidade = new MedicoEspecialidade();

            if (dto == null) return null;

            medicoEspecialidade.Id = dto.Id;
            medicoEspecialidade.Codigo = dto.Codigo;
            medicoEspecialidade.Descricao = dto.Descricao;
            medicoEspecialidade.MedicoId = dto.MedicoId;
            medicoEspecialidade.EspecialidadeId = dto.EspecialidadeId;

            if (dto.Especialidade != null)
                medicoEspecialidade.Especialidade = EspecialidadeDto.Mapear(dto.Especialidade);
            
            return medicoEspecialidade;
        }
    }
}
