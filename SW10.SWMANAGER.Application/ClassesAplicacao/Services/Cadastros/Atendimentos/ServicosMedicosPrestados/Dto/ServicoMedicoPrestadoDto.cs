using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados.Dto
{
    [AutoMapTo(typeof(ServicoMedicoPrestado))]
    public class ServicoMedicoPrestadoDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string ModeloAnamnese { get; set; }

        public bool Caucao { get; set; }

        public long? EspecialidadeId { get; set; }


        public static ServicoMedicoPrestadoDto Mapear(ServicoMedicoPrestado entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ServicoMedicoPrestadoDto>(entity);

            dto.ModeloAnamnese = entity.ModeloAnamnese;
            dto.Caucao = entity.Caucao;
            dto.EspecialidadeId = entity.EspecialidadeId;
            return dto;
        }

        public static List<ServicoMedicoPrestadoDto> Mapear(List<ServicoMedicoPrestado> entityList)
        {
            var dtoList = new List<ServicoMedicoPrestadoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }
    }
}
