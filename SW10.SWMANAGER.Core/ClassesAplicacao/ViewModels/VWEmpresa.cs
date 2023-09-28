using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("VWSis_Empresa")]
    public class VWEmpresa : Entity<long>
    {
        public string Nome { get; set; }
    }
}
