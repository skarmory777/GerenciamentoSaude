using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;
//using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Informacoes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto
{
    [AutoMap(typeof(Material))]
    public class MaterialDto : CamposPadraoCRUDDto
    {
        public int Ordem { get; set; }
        
        public bool IsDescriminaLocal { get; set; }

        //public int TipoLayout { get; set; }
        //public string DiretorioOrdem { get; set; }
        //public string DiretorioResultado { get; set; }
        //public InformacaoDto Informacao { get; set; }

        #region Mapeamento
        public static MaterialDto Mapear(Material input)
        {
            var result = MapearBase<MaterialDto>(input);
            result.Ordem = input.Ordem;
            result.IsDescriminaLocal = input.IsDescriminaLocal;

            return result;
        }

        public static Material Mapear(MaterialDto input)
        {
            var result = MapearBase<Material>(input);
            result.Ordem = input.Ordem;
            result.IsDescriminaLocal = input.IsDescriminaLocal;
            return result;
        }

        public static IEnumerable<MaterialDto> Mapear(List<Material> input)
        {
            foreach (var item in input)
            {
                var result = Mapear(item);

                yield return result;
            }
        }

        public static IEnumerable<Material> Mapear(List<MaterialDto> input)
        {
            foreach (var item in input)
            {
                var result = Mapear(item);

                yield return result;
            }
        }
        #endregion

    }
}
