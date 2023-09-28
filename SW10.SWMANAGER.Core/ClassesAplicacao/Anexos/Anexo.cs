using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Anexos
{
    [Table("SisAnexo")]
    public class Anexo : Entity<long>
    {        
        public Guid AnexoListaId { get; set; }

        public string FileName { get; set; }

        public string BucketName { get; set; }

        public string Key { get; set; }
    }
}
