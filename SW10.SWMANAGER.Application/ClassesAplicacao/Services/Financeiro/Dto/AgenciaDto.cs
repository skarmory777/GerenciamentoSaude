using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    //[AutoMap(typeof(Agencia))]
    public class AgenciaDto : CamposPadraoCRUDDto
    {
        public long? BancoId { get; set; }
        public BancoDto Banco { get; set; }

        public static AgenciaDto Mapear(Agencia agencia)
        {
            AgenciaDto agenciaDto = new AgenciaDto();

            agenciaDto.Id = agencia.Id;
            agenciaDto.Codigo = agencia.Codigo;
            agenciaDto.Descricao = agencia.Descricao;
            agenciaDto.BancoId = agencia.BancoId;

            if (agencia.Banco != null)
            {
                agenciaDto.Banco = BancoDto.Mapear(agencia.Banco);
            }

            return agenciaDto;
        }

        public static Agencia Mapear(AgenciaDto agenciaDto)
        {
            Agencia agencia = new Agencia();

            agencia.Id = agenciaDto.Id;
            agencia.Codigo = agenciaDto.Codigo;
            agencia.Descricao = agenciaDto.Descricao;
            agencia.BancoId = agenciaDto.BancoId;

            if (agenciaDto.Banco != null)
            {
                agencia.Banco = BancoDto.Mapear(agenciaDto.Banco);
            }


            return agencia;
        }
    }
}
