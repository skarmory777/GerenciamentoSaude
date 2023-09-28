using TissBase;
using System;

namespace TissV03_05_00
{
    public class V03_05_00 : BaseDomainServiceTiss, IV03_05_00
    {
        public override string GerarXmlPorLoteId(long loteId)
        {
            throw new NotImplementedException();
        }
    }

    public interface IV03_05_00:IBaseDomainServiceTiss
    {

    }
}
