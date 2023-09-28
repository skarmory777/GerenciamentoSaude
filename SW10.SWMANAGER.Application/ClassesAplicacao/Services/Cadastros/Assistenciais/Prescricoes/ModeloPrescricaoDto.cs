using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.ModelosPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes
{
    public class ModeloPrescricaoDto: CamposPadraoCRUDDto
    {
        public long PrescricaoMedicaId { get; set; }
        public PrescricaoMedicaDto PrescricaoMedica { get; set; }

        // public List<PrescricaoMedicaDto> PrescricoesDto { get; set; }

        public static ModeloPrescricaoDto Mapear(ModeloPrescricao modeloPrescricao)
        {
            if(modeloPrescricao==null)
            {
                return null;
            }

            var modeloPrescricaoDto = MapearBase<ModeloPrescricaoDto>(modeloPrescricao);
            modeloPrescricaoDto.PrescricaoMedica = PrescricaoMedicaDto.Mapear(modeloPrescricao.PrescricaoMedica);

            //if (modeloPrescricao.Prescricoes != null)
            //{
            //    modeloPrescricaoDto.PrescricoesDto = new List<PrescricaoMedicaDto>();
            //    foreach (var item in modeloPrescricao.Prescricoes)
            //    {
            //        modeloPrescricaoDto.PrescricoesDto.Add(PrescricaoMedicaDto.Mapear(item));
            //    }
            //}
            return modeloPrescricaoDto;
        }

        public static ModeloPrescricao Mapear(ModeloPrescricaoDto modeloPrescricaoDto)
        {
            if (modeloPrescricaoDto == null)
            {
                return null;
            }

            var modeloPrescricao = MapearBase<ModeloPrescricao>(modeloPrescricaoDto);

            modeloPrescricao.PrescricaoMedica = PrescricaoMedicaDto.Mapear(modeloPrescricaoDto.PrescricaoMedica);

            //if (modeloPrescricaoDto.PrescricoesDto != null)
            //{
            //    modeloPrescricao.Prescricoes = new List<PrescricaoMedica>();
            //    foreach (var item in modeloPrescricaoDto.PrescricoesDto)
            //    {
            //        modeloPrescricao.Prescricoes.Add(PrescricaoMedicaDto.Mapear(item));
            //    }
            //}
            return modeloPrescricao;
        }
    }


}
