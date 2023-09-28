using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    //[AutoMap(typeof(Banco))]
    public class BancoDto : CamposPadraoCRUDDto
    {
        public List<AgenciaDto> Agencias { get; set; }

        public static BancoDto Mapear(Banco banco, bool isService = false)
        {
            BancoDto bancoDto = new BancoDto();

            bancoDto.Id = banco.Id;
            bancoDto.Codigo = banco.Codigo;
            bancoDto.Descricao = banco.Descricao;

            if (isService)
            {
                var list = new List<AgenciaDto>();
                foreach (var item in banco.Agencias)
                {
                    list.Add(AgenciaDto.Mapear(item));
                    bancoDto.Agencias = list;
                }
            }

            return bancoDto;
        }

        public static Banco Mapear(BancoDto bancoDto, bool isService = false)
        {
            Banco tipoLocalChamada = new Banco();

            tipoLocalChamada.Id = bancoDto.Id;
            tipoLocalChamada.Codigo = bancoDto.Codigo;
            tipoLocalChamada.Descricao = bancoDto.Descricao;

            if (isService)
            {
                var list = new List<Agencia>();
                foreach (var item in bancoDto.Agencias)
                {
                    list.Add(AgenciaDto.Mapear(item));
                    tipoLocalChamada.Agencias = list;
                }
            }

            return tipoLocalChamada;
        }
    }
}
