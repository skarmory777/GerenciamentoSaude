using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AltaMedica
{
    public class AltaMedicaViewModel
    {
        public DateTime Data { get; set; }
        public long LeitoId { get; set; }
        public LeitoDto Leito { get; set; }

    }
}