using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TissBase
{
    public interface IBaseDomainServiceTiss
    {
        void DefinirConexao(string connectionString);
        string GerarXmlPorLoteId(long loteId);

        FaturamentoLoteTissModel ObterLote(long loteId);
    }

    public class FaturamentoLoteTissModel
    {

    }
}
