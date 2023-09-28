using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultGenero
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultGenero(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            //referencia, gerencial, compras, consumo

            var generos = _context.Generos.ToList();
            if (generos == null || generos.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<Genero>(_context);

                string[] generoInput = new string[3];
                generoInput[0] = "Ambos";
                generoInput[1] = "Feminino";
                generoInput[2] = "Masculino";
                for (int i = 0; i < generoInput.Length; i++)
                {
                    Genero genero = new Genero()
                    {
                        Id = i + 1,
                        Descricao = generoInput[i]
                    };

                    SeedSuprimentos.CamposPadraoCRUD(genero);
                    _context.Generos.Add(genero);
                }
                _context.SaveChanges();
            }
        }
    }
}
