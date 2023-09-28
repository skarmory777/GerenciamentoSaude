using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class LoteValidadeGridDto
    {
        public long Id { get; set; }
        public DateTime Validade { get; set; }
        public string Lote { get; set; }
        public long? IdGridLoteValidade { get; set; }
        public decimal? Quantidade { get; set; }
        public long? LaboratorioId { get; set; }
        public string Laboratorio { get; set; }
        public long LoteValidadeId { get; set; }
    }
}
