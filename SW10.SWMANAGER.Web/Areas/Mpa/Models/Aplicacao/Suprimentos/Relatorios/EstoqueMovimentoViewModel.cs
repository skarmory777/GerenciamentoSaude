using System;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Relatorios
{
    public class EstoqueMovimentoViewModel
    {
        public long EmpresaId { get; set; }
        public long EstoqueId { get; set; }
        public long GrupoId { get; set; }
        public long ClasseId { get; set; }
        public long SubclasseId { get; set; }
        public string Produto { get; set; }
        public string Lote { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TipoRel { get; set; }
    }
}