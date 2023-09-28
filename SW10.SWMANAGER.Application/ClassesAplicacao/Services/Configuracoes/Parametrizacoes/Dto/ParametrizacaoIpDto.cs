using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto
{
    public class ParametrizacaoIpDto: CamposPadraoCRUDDto
    {
        public static ParametrizacaoIpDto Mapear(ParametrizacaoIp input)
        {
            return MapearBase<ParametrizacaoIpDto>(input);
        }

        public static ParametrizacaoIp Mapear(ParametrizacaoIpDto input)
        {
            return MapearBase<ParametrizacaoIp>(input);
        }
    }
}
