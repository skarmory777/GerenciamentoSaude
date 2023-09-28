using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto
{
    public class ListarFornecedoresInput : ListarInput
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "IsAtivo desc";
            }
        }
    }
}
