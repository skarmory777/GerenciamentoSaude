using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha.HangfireJob
{
    public class ZerarFilaJobArgs
    {
        public long FilaId { get; set; }
        public int? TenantId { get; set; }

        public static ZerarFilaJobArgs GerarZerarFilaJobArgs (long filaId, int? tenantId)
        {
            return new ZerarFilaJobArgs { FilaId = filaId, TenantId = tenantId };
        }
    }
}
