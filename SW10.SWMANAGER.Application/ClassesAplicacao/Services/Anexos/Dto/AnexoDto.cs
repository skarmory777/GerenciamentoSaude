using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Anexos;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Anexos.Dto
{
    [AutoMapTo(typeof(Anexo))]
    public class AnexoDto
    {
        public Guid AnexoListaId { get; set; }

        public string FileName { get; set; }

        public string BucketName { get; set; }

        public string Key { get; set; }

        public string Url { get => "https://sw-american.s3.us-east-1.amazonaws.com/" + Key; }
    }
}
