using SW10.SWMANAGER.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Erros
{
    public class ErrosViewModel
    {
        public List<ErroDto> Erros { get; set; }
        public List<ErroDto> Warnings { get; set; }
    }
}