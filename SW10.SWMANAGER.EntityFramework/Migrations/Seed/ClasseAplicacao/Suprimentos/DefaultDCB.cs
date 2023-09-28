using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System.IO;
using System.Linq;
using System.Text;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultDCB
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultDCB(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var dcbs = _context.DCBs.ToList();
            if (dcbs == null || dcbs.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<DCB>(_context);

                string[] values = File.ReadAllText(SeedSuprimentos.MapPath("DCB_08_05_2017.csv"), Encoding.Default).Split('\r');
                //primeira linha ignorada, título das colunas
                DCB dcb = null;
                for (int i = 1; i < values.Length; i++)
                {
                    string[] linha = values[i].Replace("\n", "").Split(';');
                    if (linha.Length == 1)
                    {
                        ///TODO:implementar log
                        continue;
                    }
                    string _codigo = linha[0];
                    string _descricao = linha[1];
                    string _codigoCAS = linha[2];

                    dcb = new DCB()
                    {
                        Codigo = _codigo,
                        Descricao = _descricao,
                        CodigoCAS = _codigoCAS
                    };

                    SeedSuprimentos.CamposPadraoCRUD(dcb);
                    _context.DCBs.Add(dcb);
                }
                _context.SaveChanges();
            }
        }
    }
}