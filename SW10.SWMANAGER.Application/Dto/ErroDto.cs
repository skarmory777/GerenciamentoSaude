using System.Collections.Generic;

namespace SW10.SWMANAGER.Dto
{
    public class ErroDto
    {
        public string CodigoErro { get; set; }
        public string Descricao { get; set; }
        public List<object> Parametros { get; set; }


        public static ErroDto Criar(string codigoErro = null, string descricao = null)
        {
            return Criar(codigoErro, descricao, null);
        }

        public static ErroDto Criar(string codigoErro, string descricao, List<object> Parametros)
        {
            return new ErroDto()
            {
                CodigoErro = codigoErro,
                Descricao = descricao,
                Parametros = Parametros
            };
        }
    }
}
