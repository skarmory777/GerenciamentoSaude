using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    //[AutoMap(typeof(TipoContaCorrente))]
    public class TipoContaCorrenteDto : CamposPadraoCRUDDto
    {
        public static TipoContaCorrenteDto Mapear(TipoContaCorrente tipoConta, bool isService = false)
        {
            TipoContaCorrenteDto tipoContaDto = new TipoContaCorrenteDto();

            tipoContaDto.Id = tipoConta.Id;
            tipoContaDto.Codigo = tipoConta.Codigo;
            tipoContaDto.Descricao = tipoConta.Descricao;

            return tipoContaDto;
        }

        public static TipoContaCorrente Mapear(TipoContaCorrenteDto tipoContaDto, bool isService = false)
        {
            TipoContaCorrente tipoConta = new TipoContaCorrente();

            tipoConta.Id = tipoContaDto.Id;
            tipoConta.Codigo = tipoContaDto.Codigo;
            tipoConta.Descricao = tipoContaDto.Descricao;

            return tipoConta;
        }
    }
}
