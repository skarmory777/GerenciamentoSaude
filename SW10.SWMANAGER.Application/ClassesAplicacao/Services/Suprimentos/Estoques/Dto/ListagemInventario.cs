using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class ListagemInventario :CamposPadraoCRUDDto
    {
        public long Id { get; set; }
        public string Numero { get; set; }
        public DateTime DataInventario { get; set; }
        public string Estoque { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
    }
}

