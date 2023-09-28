using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto
{
    [AutoMap(typeof(Estado))]
    public class EstadoDto : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }

        public string Uf { get; set; }

        [ForeignKey("PaisId")]
        public virtual PaisDto Pais { get; set; }
        public long PaisId { get; set; }

        public static EstadoDto Mapear(Estado estado)
        {
            if (estado == null)
            {
                return null;
            }

            var estadoDto = MapearBase<EstadoDto>(estado);

            estadoDto.Id = estado.Id;
            estadoDto.Codigo = estado.Codigo;
            estadoDto.Descricao = estado.Descricao;
            estadoDto.Nome = estado.Nome;
            estadoDto.Uf = estado.Uf;

            if (estado.Pais != null)
            {
                estadoDto.Pais = PaisDto.Mapear(estado.Pais);
            }

            return estadoDto;
        }

        public static Estado Mapear(EstadoDto estadoDto)
        {
            if (estadoDto == null)
            {
                return null;
            }

            var estado = MapearBase<Estado>(estadoDto);
            estado.Nome = estadoDto.Nome;
            estado.Uf = estadoDto.Uf;

            if (estado.Pais != null)
            {
                estado.Pais = PaisDto.Mapear(estadoDto.Pais);
            }

            return estado;
        }

        //public virtual ICollection<CidadeDto> Cidades { get; set; }
    }
}
