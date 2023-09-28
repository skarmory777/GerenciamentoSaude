using Abp.Runtime.Validation;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class DetalhamentoExameInput : ListarInput, IShouldNormalize
    {
        public long Id { get; set; }

        public string Tipo { get; set; }

        public virtual void Normalize()
        {
            Sorting = "";
        }
    }
}
