using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.TISS
{
    public class SeedTISS
    {
        private readonly SWMANAGERDbContext _context;

        public SeedTISS(SWMANAGERDbContext context)
        {
            _context = context;
        }


        public static string MapPath(string seedFile)
        {
            seedFile = @"~\..\Migrations\Seed\ClasseAplicacao\TISS\scripts\" + seedFile;
            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }

        public void Create()
        {
            ImportarTabela("Tabela18.csv", (long)EnumTipoTabelaDominio.DiariasTaxasGasesMedicinais);
            ImportarTabela("Tabela20.csv", (long)EnumTipoTabelaDominio.Medicamento);
            ImportarTabela("Tabela22.csv", (long)EnumTipoTabelaDominio.ProcedimentosEventosEmSaude);
            ImportarTabela("Tabela23.csv", (long)EnumTipoTabelaDominio.CaraterAtendimento);
            ImportarTabela("Tabela24.csv", (long)EnumTipoTabelaDominio.ClassificaçãoBrasileiraOcupações_CBO);
            ImportarTabela("Tabela26.csv", (long)EnumTipoTabelaDominio.ConselhoProfissional);
            ImportarTabela("Tabela27.csv", (long)EnumTipoTabelaDominio.DébitosCredito);
            ImportarTabela("Tabela29.csv", (long)EnumTipoTabelaDominio.DiagnosticoPorImagem);
            ImportarTabela("Tabela30.csv", (long)EnumTipoTabelaDominio.EscalaCapacidadeFuncional_ECOG_EscalaZubrod);
            ImportarTabela("Tabela31.csv", (long)EnumTipoTabelaDominio.EstadiamentoTumor);
            ImportarTabela("Tabela61.csv", (long)EnumTipoTabelaDominio.ViaAcesso);
            ImportarTabela("Tabela62.csv", (long)EnumTipoTabelaDominio.ViaAdministracao);
            ImportarTabela("Tabela63.csv", (long)EnumTipoTabelaDominio.GruposProcedimentosItensAssistenciaisEnvioPara_ANS);
        }


        public void ImportarTabela(string nomeArquivo, long tipoTabelaDominioId)
        {

            string[] values = File.ReadAllText(MapPath(nomeArquivo), Encoding.Default).Split('\r');

            var tabelasDominio = _context.TabelasDominio.Where(w => w.TipoTabelaDominioId == tipoTabelaDominioId).ToList();

            //Descontando cabecalho e ultinha linha (\n)
            var totalLinhasArquivo = values.Count() - 2;

            if (tabelasDominio == null || tabelasDominio.Count() == 0 || tabelasDominio.Count() < totalLinhasArquivo)
            {

                //primeira linha ignorada, título das colunas
                TabelaDominio tabelaDominio = null;
                for (int i = 1; i < values.Length; i++)
                {
                    string[] linha = values[i].Replace("\n", "").Split(';');
                    if (linha.Length == 1)
                    {
                        ///TODO:implementar log
                        continue;
                    }
                    string codigo = linha[0];
                    string descricao = linha[1];


                    var medicamento = tabelasDominio.Where(w => w.Codigo == codigo && w.TipoTabelaDominioId == tipoTabelaDominioId).FirstOrDefault();


                    if (medicamento == null)
                    {
                        //var vigencia = DateTime.Now;
                        tabelaDominio = new TabelaDominio()
                        {
                            Codigo = codigo,
                            Descricao = descricao,
                            TipoTabelaDominioId = tipoTabelaDominioId
                        };

                        SeedSuprimentos.CamposPadraoCRUD(tabelaDominio);
                        _context.TabelasDominio.Add(tabelaDominio);

                        if (i % 200 == 0)
                        {
                            _context.SaveChanges();
                        }

                    }
                }
                _context.SaveChanges();
            }
        }

    }
}
