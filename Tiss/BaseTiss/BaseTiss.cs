using System.Data.SqlClient;
using Dapper;

namespace TissBase
{
    public abstract class BaseDomainServiceTiss : IBaseDomainServiceTiss
    {
        protected string ConnString { get; set; }

        public void DefinirConexao(string connectionString)
        {
            this.ConnString = connectionString;
        }

        public abstract string GerarXmlPorLoteId(long loteId);

        public FaturamentoLoteTissModel ObterLote(long loteId)
        {
            var query = $@"
                SELECT 
                    FatEntregaConta.*,
                    FatEntregaLote.*,
                    SisConvenio.*,
                    SisVersaoTiss.*
                FROM FatEntregaConta 
                LEFT JOIN FatEntregaLote ON FatEntregaLote.Id = FatEntregaConta.FatEntregaLoteId AND FatEntregaLote.IsDeleted = @isDeleted
                LEFT JOIN SisConvenio ON SisConvenio.Id = FatEntregaLote.SisConvenioId AND SisConvenio.IsDeleted = @isDeleted
                LEFT JOIN SisVersaoTiss ON SisVersaoTiss.Id = SisConvenio.VersaoTissId AND SisVersaoTiss.IsDeleted = @isDeleted
                WHERE FatEntregaConta.IsDeleted = @isDeleted AND FatEntregaConta.EntregaLoteId = @loteId
            ";
            using(var conn = new SqlConnection(ConnString))
            {
                //return conn.QueryFirstOrDefault<>
            }

            return new FaturamentoLoteTissModel();
        }
    }
}
