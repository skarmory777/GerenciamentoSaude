using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto
{
    [AutoMap(typeof(Cep))]
    public class CepDto : CamposPadraoCRUDDto
    {
        public string CEP { get; set; }

        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public string Complemento { get; set; }

        public string Complemento2 { get; set; }

        public string UnidadePostagem { get; set; }

        public long TipoLogradouroId { get; set; }

        public virtual TipoLogradouroDto TipoLogradouro { get; set; }

        public long CidadeId { get; set; }

        public virtual CidadeDto Cidade { get; set; }

        public long EstadoId { get; set; }

        public virtual EstadoDto Estado { get; set; }

        public long PaisId { get; set; }

        public virtual PaisDto Pais { get; set; }

        public static Cep Mapear(CepDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<Cep>(dto);
            entity.CEP = dto.CEP;
            entity.Logradouro = dto.Logradouro;
            entity.Bairro = dto.Bairro;
            entity.Complemento = dto.Complemento;
            entity.Complemento2 = dto.Complemento2;
            entity.UnidadePostagem = dto.UnidadePostagem;
            entity.TipoLogradouroId = dto.TipoLogradouroId;
            entity.CidadeId = dto.CidadeId;
            entity.EstadoId = dto.EstadoId;
            entity.PaisId = dto.PaisId;
            entity.Pais = PaisDto.Mapear(dto.Pais);
            entity.Estado = EstadoDto.Mapear(dto.Estado);
            entity.Cidade = CidadeDto.Mapear(dto.Cidade);
            entity.TipoLogradouro = TipoLogradouroDto.Mapear(dto.TipoLogradouro);

            return entity;
        }

        public static CepDto Mapear(Cep entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CepDto>(entity);
            dto.CEP = entity.CEP;
            dto.Logradouro = entity.Logradouro;
            dto.Bairro = entity.Bairro;
            dto.Complemento = entity.Complemento;
            dto.Complemento2 = entity.Complemento2;
            dto.UnidadePostagem = entity.UnidadePostagem;
            dto.TipoLogradouroId = entity.TipoLogradouroId ?? 0;
            dto.CidadeId = entity.CidadeId ?? 0;
            dto.EstadoId = entity.EstadoId ?? 0;
            dto.PaisId = entity.PaisId ?? 0;
            dto.Pais = PaisDto.Mapear(entity.Pais);
            dto.Estado = EstadoDto.Mapear(entity.Estado);
            dto.Cidade = CidadeDto.Mapear(entity.Cidade);
            dto.TipoLogradouro = TipoLogradouroDto.Mapear(entity.TipoLogradouro);

            return dto;
        }

        public static List<CepDto> Mapear(List<Cep> entityList)
        {
            var dtoList = new List<CepDto>();

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
