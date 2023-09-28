using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultMovimentoEstado
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultMovimentoEstado(SWMANAGERDbContext context)
        {
            _context = context;
        }


        public void Create()
        {
            string[] descricaoEstado = new string[3];
            descricaoEstado[0] = "Aguardando Confirmação";
            descricaoEstado[1] = "Confirmado";
            descricaoEstado[2] = "Pendente informação de lote/validade";

            var estados = _context.EstoquePreMovimentoEstado.ToList();

            if (estados == null || estados.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<EstoquePreMovimentoEstado>(_context);

                foreach (var item in descricaoEstado)
                {
                    var estoquePreMovimentoEstado = new EstoquePreMovimentoEstado { Descricao = item };

                    SeedSuprimentos.CamposPadraoCRUD(estoquePreMovimentoEstado);
                    _context.EstoquePreMovimentoEstado.Add(estoquePreMovimentoEstado);
                    _context.SaveChanges();
                }
            }
            else
            {
                var estado1 = estados.Where(w => w.Id == 1).FirstOrDefault();
                if (estado1 != null)
                {
                    estado1.Descricao = descricaoEstado[0];
                }

                var estado2 = estados.Where(w => w.Id == 2).FirstOrDefault();
                if (estado2 != null)
                {
                    estado2.Descricao = descricaoEstado[1];
                }
                else
                {
                    var estoquePreMovimentoEstado = new EstoquePreMovimentoEstado { Descricao = descricaoEstado[1] };

                    SeedSuprimentos.CamposPadraoCRUD(estoquePreMovimentoEstado);
                    _context.EstoquePreMovimentoEstado.Add(estoquePreMovimentoEstado);
                }


                _context.SaveChanges();
            }
        }
    }
}
