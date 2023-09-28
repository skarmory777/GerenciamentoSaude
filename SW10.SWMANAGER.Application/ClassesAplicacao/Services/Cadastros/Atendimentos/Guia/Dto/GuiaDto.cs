using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto
{
    [AutoMap(typeof(Guia))]
    public class GuiaDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public long? OriginariaId { get; set; }

        public byte[] ModeloPDF { get; set; }

        public string ModeloPDFMimeType { get; set; }

        public byte[] ModeloPNG { get; set; }

        public string ModeloPNGMimeType { get; set; }

        public string CamposJson { get; set; }

        public IEnumerable<Type> PegarClassesBase(Type tipo)
        {
            var atual = tipo.BaseType;
            while (atual != null)
            {
                yield return atual;
                atual = atual.BaseType;
            }
        }

        public static GuiaDto Mapear(Guia entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<GuiaDto>(entity);
            dto.OriginariaId = entity.OriginariaId;
            dto.ModeloPDF = entity.ModeloPDF;
            dto.ModeloPDFMimeType = entity.ModeloPDFMimeType;
            dto.ModeloPNG = entity.ModeloPNG;
            dto.ModeloPNGMimeType = entity.ModeloPNGMimeType;
            dto.CamposJson = entity.CamposJson;

            return dto;
        }

        public static List<GuiaDto> Mapear(List<Guia> entityList)
        {
            var dtoList = new List<GuiaDto>();

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
