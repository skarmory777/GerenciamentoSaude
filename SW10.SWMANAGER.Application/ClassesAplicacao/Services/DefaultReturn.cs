using SW10.SWMANAGER.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public class DefaultReturn<T> where T : class
    {
        public DefaultReturn()
        {
        }
        
        public DefaultReturn(List<ErroDto> erros)
        {
            Errors = erros;
        }
        
        public DefaultReturn(List<ErroDto> erros, List<ErroDto> warnings)
        {
            Errors = erros;
            Warnings = warnings;
        }

        //public TipoRetorno TipoRetorno { get; set; }
        public List<ErroDto> Errors { get; set; } = new List<ErroDto>();
        public List<ErroDto> Warnings { get; set; } = new List<ErroDto>();
        public T ReturnObject { get; set; }
    }

    public class DefaultReturnBool
    {
        public bool Sucesso { get; set; }
    }
}
