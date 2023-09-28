using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.EntityFramework;
using System.IO;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Assistenciais
{
    public class DefaultCBO
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultCBO(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {

            var cbos = _context.Cbos.ToList();
            if (cbos == null || cbos.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<Cbo>(_context);

                _context.SaveChanges();

                #region importador_script

                var arquivoScript = SeedAssistenciais.MapPath("carga_cbo_v1.sql");

                _context.Database.ExecuteSqlCommand(File.ReadAllText(arquivoScript));

                #endregion

                #region importador_csv
                // string[] cbosCsv = File.ReadAllText(SeedAssistenciais.MapPath("CBO_26_01_2018.csv"), Encoding.Default).Split('\r');
                //// primeira linha ignorada, título das colunas

                // for (int i = 1; i < cbosCsv.Length; i++)
                // {
                //     string[] linha = cbosCsv[i].Replace("\n", "").Split(';');
                //     if (linha.Length == 1)
                //     {
                //         // TODO:implementar log
                //         continue;
                //     }
                //     string _codigo = linha[0];
                //     string _descricao = linha[1];

                //     long _tipo;
                //     long.TryParse(linha[3], out _tipo);

                //     Cbo cbo = new Cbo
                //     {
                //         Codigo = _codigo,
                //         Descricao = _descricao,
                //         CboTipoId = _context.CboTipos.FirstOrDefault(x => x.Codigo == _tipo.ToString())?.Id,
                //         CboFamiliaId = _context.CboFamilias.FirstOrDefault(x => _codigo.Contains(x.Codigo))?.Id
                //     };

                //     SeedAssistenciais.CamposPadraoCRUD(cbo);
                //     _context.Cbos.Add(cbo);

                //     if (i % 1000 == 0)
                //     {
                //         _context.SaveChanges();
                //     }

                // }
                #endregion

                #region importador_sql
                //var arquivoCSV = SeedAssistenciais.MapPath("cbo_v1.csv");
                //var arquivoFMT = SeedAssistenciais.MapPath("cbo_v1.fmt");

                //var comandoSql = "INSERT INTO SisCbo " +
                //                 "SELECT 1, Codigo, Descricao, 0, NULL, NULL, NULL, NULL, GETDATE(), 2, (select id from SisCboFamilia b where b.Codigo = SUBSTRING(a.codigo, 1, 4)) cbofamiliaid, TIPOID " +
                //                 "FROM OPENROWSET(BULK N'" + arquivoCSV + "', " +
                //                 "    FORMATFILE = N'" + arquivoFMT + "' " + 
                //                 "    , FIRSTROW = 2 " +
                //                 "    ) AS a ";


                //_context.Database.ExecuteSqlCommand(File.ReadAllText(comandoSql));
                #endregion

                _context.SaveChanges();
            }
        }

    }
}