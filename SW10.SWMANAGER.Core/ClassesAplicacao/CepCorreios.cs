using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public class CepCorreios : CamposPadraoCRUD
    {
        [MaxLength(9)]
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string End { get; set; }
        public string Complemento { get; set; }
        public string Complemento2 { get; set; }
        public string Cidade { get; set; }
        public long CidadeId { get; set; }
        public string Uf { get; set; }
        public long EstadoId { get; set; }
        public string UnidadesPostagem { get; set; }
        public long PaisId { get; set; }
    }
}
