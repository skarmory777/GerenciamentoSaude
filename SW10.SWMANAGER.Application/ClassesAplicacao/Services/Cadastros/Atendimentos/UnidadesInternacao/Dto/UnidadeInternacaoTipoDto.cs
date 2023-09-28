using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto
{
    [AutoMap(typeof(UnidadeInternacaoTipo))]
    public class UnidadeInternacaoTipoDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }



        #region Mapeamento

        public static UnidadeInternacaoTipo Mapear(UnidadeInternacaoTipoDto unidadeInternacaoTipoDto)
        {
            if (unidadeInternacaoTipoDto == null)
            {
                return null;
            }

            UnidadeInternacaoTipo unidadeInternacaoTipo = new UnidadeInternacaoTipo();

            unidadeInternacaoTipo.Id = unidadeInternacaoTipoDto.Id;
            unidadeInternacaoTipo.Codigo = unidadeInternacaoTipoDto.Codigo;
            unidadeInternacaoTipo.Descricao = unidadeInternacaoTipoDto.Descricao;

            return unidadeInternacaoTipo;
        }

        public static UnidadeInternacaoTipoDto Mapear(UnidadeInternacaoTipo unidadeInternacaoTipo)
        {
            if (unidadeInternacaoTipo == null)
            {
                return null;
            }

            UnidadeInternacaoTipoDto unidadeInternacaoTipoDto =
                MapearBase<UnidadeInternacaoTipoDto>(unidadeInternacaoTipo);

            unidadeInternacaoTipoDto.Id = unidadeInternacaoTipo.Id;
            unidadeInternacaoTipoDto.Codigo = unidadeInternacaoTipo.Codigo;
            unidadeInternacaoTipoDto.Descricao = unidadeInternacaoTipo.Descricao;

            return unidadeInternacaoTipoDto;
        }

        public static List<UnidadeInternacaoTipoDto> Mapear(List<UnidadeInternacaoTipo> entityList)
        {
            var dtoList = new List<UnidadeInternacaoTipoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }

        #endregion
    }

}
