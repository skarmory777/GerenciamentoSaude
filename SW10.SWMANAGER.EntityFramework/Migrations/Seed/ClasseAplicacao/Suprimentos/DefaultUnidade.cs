using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Suprimentos
{
    public class DefaultUnidade
    {
        private readonly SWMANAGERDbContext _context;
        public DefaultUnidade(SWMANAGERDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            var unidades = _context.Unidades.ToList();
            if (unidades == null || unidades.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<Unidade>(_context);

                String[] values = File.ReadAllText(SeedSuprimentos.MapPath("Unidade.csv"), Encoding.Default).Split('\r');

                Unidade unidadeReferencia;
                long unidadeReferenciaId;
                for (int i = 1; i < values.Length; i++)
                {
                    string[] linha = values[i].Replace("\n", "").Split(';');
                    string _id = linha[0];
                    string _sigla = linha[1];
                    string _descricao = linha[2];
                    string _fator = linha[3];
                    string _unidadeReferenciaId = linha[4];

                    unidadeReferencia = new Unidade();

                    unidadeReferencia.Id = long.Parse(_id);
                    unidadeReferencia.Sigla = _sigla;
                    unidadeReferencia.Descricao = _descricao;
                    unidadeReferencia.Fator = Decimal.Parse(_fator, new NumberFormatInfo() { NumberDecimalSeparator = "." });
                    unidadeReferencia.UnidadeReferenciaId = long.TryParse(_unidadeReferenciaId, out unidadeReferenciaId) ? unidadeReferenciaId : (long?)null;

                    SeedSuprimentos.CamposPadraoCRUD(unidadeReferencia);
                    _context.Unidades.Add(unidadeReferencia);
                    _context.SaveChanges();
                }

                //string[][] listaUnidades = new string[4][];
                //listaUnidades[0] = new string[9];
                //listaUnidades[0][0] = "AMP|AMPOLA|1.00|true";
                //listaUnidades[0][1] = "CX1500|CAIXA 1500|1500.00|false";
                //listaUnidades[0][2] = "CX100|CAIXA 100|100.00|false";
                //listaUnidades[0][3] = "CX25|CAIXA 25|25.00|false";
                //listaUnidades[0][4] = "CX3|CAIXA 3|3.00|false";
                //listaUnidades[0][5] = "CX40|CAIXA 40|40.00|false";
                //listaUnidades[0][6] = "CX5|CAIXA 5|5.00|false";
                //listaUnidades[0][7] = "CX50|CAIXA 50|50.00|false";
                //listaUnidades[0][8] = "CX6|CAIXA 6|6.00|false";
                //listaUnidades[1] = new string[7];
                //listaUnidades[1][0] = "ML|MILILITRO|1.00|true";
                //listaUnidades[1][1] = "FRASCO15ML|FRASCO 15ML|15.00|false";
                //listaUnidades[1][2] = "FRASCO20ML|FRASCO 20ML|20.00|false";
                //listaUnidades[1][3] = "CX10|CX 10|150.00|false";
                //listaUnidades[1][4] = "GOTAS30|GOTAS 30|2.00|false";
                //listaUnidades[1][5] = "GOTAS40|GOTAS 40|2.70|false";
                //listaUnidades[1][6] = "GOTAS60|GOTAS 60|4.00|false";
                //listaUnidades[2] = new string[8];
                //listaUnidades[2][0] = "UND|UNIDADE|1.00|true";
                //listaUnidades[2][1] = "BL|BLOCO|1.00|false";
                //listaUnidades[2][2] = "FOLHA|FOLHA A4|1.00|false";
                //listaUnidades[2][3] = "RESMAA4|RESMA A4|500.00|false";
                //listaUnidades[2][4] = "PCT1000|PACOTE 1000|1000.00|false";
                //listaUnidades[2][5] = "CX100|CAIXA 100|100.00|false";
                //listaUnidades[2][6] = "CX10|CAIXA 10|10.00|false";
                //listaUnidades[2][7] = "PCT100|PACOTE 100|100.00|false";
                //listaUnidades[3] = new string[7];
                //listaUnidades[3][0] = "CPR|COMPRIMIDO|1.00|true";
                //listaUnidades[3][1] = "CX10|CAIXA 10|10.00|false";
                //listaUnidades[3][2] = "CX20|CAIXA 20|20.00|false";
                //listaUnidades[3][3] = "CX50|CAIXA 50|50.00|false";
                //listaUnidades[3][4] = "CX500|CAIXA 500|500.00|false";
                //listaUnidades[3][5] = "CPR10MG|COMPRIMIDO 10MG|1.00|false";
                //listaUnidades[3][6] = "CPR20MG|COMPRIMIDO 20MG|1.00|false";

                //Unidade unidadeReferencia = null;

                //for (int index1 = 0; index1 < listaUnidades.Length; index1++)
                //{
                //    for (int index2 = 0; index2 < listaUnidades[index1].Length; index2++)
                //    {
                //        string[] valores = listaUnidades[index1][index2].Split('|');
                //        string sigla = valores[0];
                //        string descricao = valores[1];
                //        Decimal fator = Decimal.Parse(valores[2], new NumberFormatInfo() { NumberDecimalSeparator = "." });
                //        bool isReferencia = Boolean.Parse(valores[3]);

                //        Unidade unidade = new Unidade()
                //        {
                //            Sigla = sigla,
                //            Descricao = descricao,
                //            Fator = fator,
                //            UnidadeReferencia = unidadeReferencia
                //        };

                //        if (isReferencia == true)
                //            unidadeReferencia = unidade;

                //        SeedSuprimentos.CamposPadraoCRUD(unidade);
                //        _context.Unidades.Add(unidade);
                //    }

                //    unidadeReferencia = null;
                //}
            }
        }
    }
}
