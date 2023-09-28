using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cbos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto
{
    [AutoMap(typeof(Especialidade))]
    public class EspecialidadeDto : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public string Cbo { get; set; }
        public string CboSus { get; set; }

        public bool IsAtivo { get; set; }

        public long CboId { get; set; }
        public CboDto SisCbo { get; set; }

        public static EspecialidadeDto Mapear(Especialidade especialidade)
        {
            if (especialidade == null)
            {
                return null;
            }

            EspecialidadeDto especialidadeDto = MapearBase<EspecialidadeDto>(especialidade);

            especialidadeDto.Id = especialidade.Id;
            especialidadeDto.Codigo = especialidade.Codigo;
            especialidadeDto.Descricao = especialidade.Descricao;
            especialidadeDto.Nome = especialidade.Nome;
            especialidadeDto.Cbo = especialidade.Cbo;
            especialidadeDto.CboSus = especialidade.CboSus;
            especialidadeDto.IsAtivo = especialidade.IsAtivo;
            especialidadeDto.CboId = especialidade.CboId ?? 0;

            if (especialidade.SisCbo != null)
            {
                especialidadeDto.SisCbo = CboDto.Mapear(especialidade.SisCbo);
            }

            return especialidadeDto;
        }

        public static Especialidade Mapear(EspecialidadeDto dto)
        {
            if (dto == null) return null;

            var especialidade = MapearBase<Especialidade>(dto);

            especialidade.Id = dto.Id;
            especialidade.Codigo = dto.Codigo;
            especialidade.Descricao = dto.Descricao;
            especialidade.Nome = dto.Nome;
            especialidade.Cbo = dto.Cbo;
            especialidade.CboSus = dto.CboSus;
            especialidade.IsAtivo = dto.IsAtivo;
            especialidade.CboId = dto.CboId;

            if (dto.SisCbo != null)
            {
                especialidade.SisCbo = CboDto.Mapear(dto.SisCbo);
            }

            return especialidade;
        }
    }
}