using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultCFOP
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultCFOP(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {

            string[] values = File.ReadAllText(SeedSuprimentos.MapPath("cfop.csv"), Encoding.Default).Split('\r');
            var cfops = _context.Cfops.ToList();
            if (cfops == null || cfops.Count() == 0 || cfops.Count() < values.Count())
            {

                //primeira linha ignorada, título das colunas
                Cfop cfop = null;
                for (int i = 1; i < values.Length; i++)
                {
                    string[] linha = values[i].Replace("\n", "").Split(';');
                    if (linha.Length == 1)
                    {
                        ///TODO:implementar log
                        continue;
                    }
                    string numero = linha[0].Replace(".", "");
                    string _descricao = linha[1];

                    long _numero;
                    long.TryParse(numero, out _numero);


                    var _cfop = cfops.Where(w => w.Numero == _numero).FirstOrDefault();


                    if (_cfop == null)
                    {
                        var vigencia = DateTime.Now;
                        cfop = new Cfop()
                        {
                            Numero = _numero,
                            Descricao = _descricao,
                            Vigencia = vigencia
                        };

                        SeedSuprimentos.CamposPadraoCRUD(cfop);
                        _context.Cfops.Add(cfop);
                    }
                }
                _context.SaveChanges();
            }
        }
    }
}