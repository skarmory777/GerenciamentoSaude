using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos
{
    [AutoMap(typeof(Conselho))]
    public class ConselhoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public string Uf { get; set; }

        public string NomeEstado { get; set; }

        #region Mapeamento

        public static ConselhoDto Mapear(Conselho conselho)
        {
            var conselhoDto = MapearBase<ConselhoDto>(conselho);
            conselhoDto.Sigla = conselho.Sigla;
            conselhoDto.Uf = conselho.Uf;
            conselhoDto.NomeEstado = conselho.NomeEstado;

            return conselhoDto;
        }

        public static Conselho Mapear(ConselhoDto conselhoDto)
        {
            Conselho conselho = MapearBase<Conselho>(conselhoDto);
            conselho.Sigla = conselhoDto.Sigla;
            conselho.Uf = conselhoDto.Uf;
            conselho.NomeEstado = conselhoDto.NomeEstado;

            return conselho;
        }


        #endregion

    }
}
