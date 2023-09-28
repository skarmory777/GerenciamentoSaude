using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.EntityFramework;
using System.IO;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Assistenciais
{
    public class DefaultCBOFamilia
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultCBOFamilia(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {

            //Permissao();

            var cbosFamilias = _context.CboFamilias.ToList();
            if (cbosFamilias == null || cbosFamilias.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<CboFamilia>(_context);

                _context.SaveChanges();

                //----------------------------------------------------------------------------------------------------------------

                #region importador_csv
                //a importacao por este caminho é demorada. Para 10.000 cbos leva cerca de 10 minutos

                //string[] familiasCsv = File.ReadAllText(SeedAssistenciais.MapPath("CBOFAMILIAS_26_01_2018.csv"), Encoding.Default).Split('\r');
                ////primeira linha ignorada, título das colunas

                //for (int i = 1; i < familiasCsv.Length; i++)
                //{
                //    string[] linha = familiasCsv[i].Replace("\n", "").Split(';');
                //    if (linha.Length == 1)
                //    {
                //        ///TODO:implementar log
                //        continue;
                //    }

                //    string _codigo = linha[0];
                //    string _descricao = linha[1];

                //    CboFamilia cboFamilia = new CboFamilia
                //    {
                //        Codigo = _codigo,
                //        Descricao = _descricao
                //    };

                //    SeedAssistenciais.CamposPadraoCRUD(cboFamilia);
                //    _context.CboFamilias.Add(cboFamilia);
                //}
                #endregion

                #region importador_script
                var arquivoScript = SeedAssistenciais.MapPath("carga_cbo_familia_v1.sql");
                _context.Database.ExecuteSqlCommand(File.ReadAllText(arquivoScript));
                #endregion

                #region importador_sql
                //Não tá funcionando porque dá erro de permissão na pasta do arquivo
                //Vale a pena porque importa direto do arquivo csv para o banco

                //var arquivoCSV = @"C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\cbo_familia_v1.csv";
                //var arquivoFMT = @"C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\scripts\cbo_familia_v1.fmt";
                ////var arquivoCSV = SeedAssistenciais.MapPath("cbo_familia_v1.csv");
                ////var arquivoFMT = SeedAssistenciais.MapPath("cbo_familia_v1.fmt");

                //var comandoSql = "INSERT INTO SisCboFamilia " +
                //    "SELECT 1, Codigo, Descricao, 0, NULL, NULL, NULL, NULL, GETDATE(), 2 " +
                //    "FROM OPENROWSET(BULK N'" + arquivoCSV + "', " +
                //    "    FORMATFILE = N'" + arquivoFMT + "' " +
                //    "    , FIRSTROW = 2 " +
                //    "    ) AS cbo_familias ";

                //_context.Database.ExecuteSqlCommand(comandoSql);
                #endregion

                _context.SaveChanges();

            }
        }

        //public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        //{
        //    // Create a new DirectoryInfo object.
        //    DirectoryInfo dInfo = new DirectoryInfo(FileName);

        //    // Get a DirectorySecurity object that represents the 
        //    // current security settings.
        //    DirectorySecurity dSecurity = dInfo.GetAccessControl();

        //    // Add the FileSystemAccessRule to the security settings. 
        //    dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
        //                                                    Rights,
        //                                                    ControlType));

        //    // Set the new access settings.
        //    dInfo.SetAccessControl(dSecurity);

        //}

        //public void DarPermissao()
        //{
        //    try
        //    {
        //        var DirectoryName = "scripts";

        //        var caminho = @"C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.EntityFramework\Migrations\Seed\ClasseAplicacao\Assistenciais\";

        //        //SWDEV07 \SQLServerMSSQLUser$ SWDev07 $MSSQLSERVER

        //        Console.WriteLine("Adding access control entry for " + DirectoryName);

        //        // Add the access control entry to the directory.
        //        AddDirectorySecurity(caminho + DirectoryName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AccessControlType.Allow);

        //        //Console.WriteLine("Removing access control entry from " + DirectoryName);

        //        // Remove the access control entry from the directory.
        //        //RemoveDirectorySecurity(DirectoryName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AccessControlType.Allow);

        //        Console.WriteLine("Done.");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }

        //    Console.ReadLine();
        //};

    }
}