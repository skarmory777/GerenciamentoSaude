using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using System;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.AtendimentosLeitosMov.Altas
{
    public class AltaModalViewModel
    {
        public long? AtendimentoId { get; set; }
        public DateTime Data { get; set; }
        public long? MotivoAltaId { get; set; }
        public long? GrupoCIDId { get; set; }
        public long LeitoId { get; set; }
        public MotivoAltaDto MotivoAlta { get; set; }
        public GrupoCIDDto GrupoCID { get; set; }
        public LeitoDto Leito { get; set; }
        public string NumeroObito { get; set; }
        public DateTime DataAltaMedica { get; set; }
        public DateTime PrevisaoAlta { get; set; }
        public DateTime? DataTomadaDecisao { get; set; }
    }
}