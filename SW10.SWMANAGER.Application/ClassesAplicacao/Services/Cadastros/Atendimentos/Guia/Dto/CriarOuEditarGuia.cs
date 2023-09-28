using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto
{
    [AutoMap(typeof(Guia))]
    public class CriarOuEditarGuia : CamposPadraoCRUDDto
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
            var result = new List<Type>();
            var atual = tipo.BaseType;
            while (atual != null)
            {
                //yield return atual;
                result.Add(atual);
                atual = atual.BaseType;
            }
            return result;
        }

        public static Guia Mapear(CriarOuEditarGuia dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<Guia>(dto);
            entity.OriginariaId = dto.OriginariaId;
            entity.ModeloPDF = dto.ModeloPDF;
            entity.ModeloPDFMimeType = dto.ModeloPDFMimeType;
            entity.ModeloPNG = dto.ModeloPNG;
            entity.ModeloPNGMimeType = dto.ModeloPNGMimeType;
            entity.CamposJson = dto.CamposJson;

            return entity;
        }

        public static CriarOuEditarGuia Mapear(Guia entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CriarOuEditarGuia>(entity);
            dto.OriginariaId = entity.OriginariaId;
            dto.ModeloPDF = entity.ModeloPDF;
            dto.ModeloPDFMimeType = entity.ModeloPDFMimeType;
            dto.ModeloPNG = entity.ModeloPNG;
            dto.ModeloPNGMimeType = entity.ModeloPNGMimeType;
            dto.CamposJson = entity.CamposJson;

            return dto;
        }
    }
}