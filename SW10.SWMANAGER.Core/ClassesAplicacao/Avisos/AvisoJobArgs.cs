using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Avisos
{
    [Serializable]
    public class AvisoJobArgs
    {
        public long Id { get; set; }
        public int TenantId { get; set; }
    }
}