using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Sefaz
{
    [Serializable]
    public class SefazTecnoSpeedSincronizaNotasJobArgs
    {
        public long SefazTecnoSpeedConfiguracaoId { get; set; }
        public int TenantId { get; set; }
    }
}
