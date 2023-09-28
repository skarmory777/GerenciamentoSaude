using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto
{
    [AutoMap(typeof(Cidade))]
    public class CidadeDto : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }

        public virtual EstadoDto Estado { get; set; }
        public long EstadoId { get; set; }

        public bool Capital { get; set; }



        public static CidadeDto Mapear(Cidade cidade)
        {
            if (cidade == null)
            {
                return null;
            }
            var cidadeDto = MapearBase<CidadeDto>(cidade);
            cidadeDto.Nome = cidade.Nome;
            cidadeDto.EstadoId = cidade.EstadoId ?? 0;
            cidadeDto.Capital = cidade.IsCapital;

            if (cidade.Estado != null)
            {
                cidadeDto.Estado = EstadoDto.Mapear(cidade.Estado);
            }

            return cidadeDto;

        }

        public static Cidade Mapear(CidadeDto cidadeDto)
        {
            if (cidadeDto == null)
            {
                return null;
            }
            var cidade = MapearBase<Cidade>(cidadeDto);
            cidade.Nome = cidadeDto.Nome;
            cidade.EstadoId = cidadeDto.EstadoId;
            cidade.IsCapital = cidadeDto.Capital;

            if (cidadeDto.Estado != null)
            {
                cidade.Estado = EstadoDto.Mapear(cidadeDto.Estado);
            }

            return cidade;
        }

    }
}
