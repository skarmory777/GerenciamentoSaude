using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public  class RastroDto
    {
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public string Serie { get; set; }
        public decimal Quantidade { get; set; }
    }
}
