using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados.Dto
{
    [AutoMapTo(typeof(ServicoMedicoPrestado))]
    public class CriarOuEditarServicoMedicoPrestado : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string ModeloAnamnese { get; set; }

        public bool Caucao { get; set; }

        public long? EspecialidadeId { get; set; }

        public static ServicoMedicoPrestado Mapear(CriarOuEditarServicoMedicoPrestado dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<ServicoMedicoPrestado>(dto);
            entity.ModeloAnamnese = dto.ModeloAnamnese;
            entity.Caucao = dto.Caucao;
            entity.EspecialidadeId = dto.EspecialidadeId;

            return entity;
        }

        public static CriarOuEditarServicoMedicoPrestado Mapear(ServicoMedicoPrestado entidade)
        {
            if (entidade == null) return null;

            var dto = MapearBase<CriarOuEditarServicoMedicoPrestado>(entidade);
            dto.ModeloAnamnese = entidade.ModeloAnamnese;
            dto.Caucao = entidade.Caucao;
            dto.EspecialidadeId = entidade.EspecialidadeId;

            return dto;
        }
    }
}