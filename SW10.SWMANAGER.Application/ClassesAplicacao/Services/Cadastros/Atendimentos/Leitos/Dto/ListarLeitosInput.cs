using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto
{
    public class ListarLeitosInput : ListarInput
    {
        public string TipoAtendimento { get; set; }

        public string TipoAcomodacao { get; set; }

        public string UO { get; set; }

        public long? UnidadeId { get; set; }

        public bool SomenteInternados { get; set; }

        public bool SomenteVagos { get; set; }

        public bool Todos { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
