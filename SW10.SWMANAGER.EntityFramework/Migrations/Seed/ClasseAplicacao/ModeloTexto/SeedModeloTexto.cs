// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModeloTexto.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ModeloTextoBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.ModeloTexto
{
    using ClassesAplicacao.Atendimentos.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using SW10.SWMANAGER.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// The modelo texto builder.
    /// </summary>
    public class SeedModeloTextoBuilder
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly SWMANAGERDbContext context;

        /// <inheritdoc />
        public SeedModeloTextoBuilder(SWMANAGERDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// The create.
        /// </summary>
        public void Create()
        {
            var listFichaPaciente = new List<string>
                                        {
                                            "EmpresaRazaoSocial",
                                            "Empresa",
                                            "DataHora",
                                            "CodigoPaciente",
                                            "Endereco",
                                            "Contrato",
                                            "Acompanhante",
                                            "Responsavel",
                                            "CodInternacao",
                                            "Leito",
                                            "DataValidade",
                                            "Senha",
                                            "DiasAutorizados",
                                            "Numero",
                                            "Complemento",
                                            "Bairro",
                                            "Estado",
                                            "Cidade",
                                            "Telefone",
                                            "Cep",
                                            "Pais",
                                            "Nacionalidade",
                                            "Filiacao",
                                            "Sexo",
                                            "Profissao",
                                            "Paciente",
                                            "Cid",
                                            "Nascimento",
                                            "Idade",
                                            "Cpf",
                                            "SituacaoCivil",
                                            "DataAtendimento",
                                            "Identidade",
                                            "Medico",
                                            "Especialidade",
                                            "IndicadoPor",
                                            "Origem",
                                            "Tratamento",
                                            "Convenio",
                                            "Plano",
                                            "Guia",
                                            "NumberGuide",
                                            "Titular",
                                            "CodigoAtendimento",
                                            "DataAlta",
                                            "Alta",
                                            "Matricula",
                                            "DataPagto",
                                            "IdAcompanhante",
                                            "CodDep",
                                            "Usuario",
                                            "CodigoBarra",
                                            "1DtHoraAtendimento",
                                            "2DtHoraAtendimento",
                                            "1MedicoAtendimento",
                                            "2MedicoAtendimento",
                                            "1ConvenioAtendimento",
                                            "2ConvenioAtendimento",
                                            "1Espec",
                                            "2Espec",
                                            "[",
                                            "]"
                                        };

            var etiquetaPacienteVariaveis = new List<TipoModeloVariaveis>
                                                {
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 2, Descricao = "AtendimentoId"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 2, Descricao = "Paciente"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 2, Descricao = "DataNascimento"
                                                        },
                                                    new TipoModeloVariaveis { TipoModeloId = 2, Descricao = "Idade" },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 2, Descricao = "DataAtendimento"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 2, Descricao = "CodigoAtendimento"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 2, Descricao = "MatriculaConvenio"
                                                        },
                                                    new TipoModeloVariaveis { TipoModeloId = 2, Descricao = "Convenio" }
                                                };

            var etiquetaVisitanteVariaveis = new List<TipoModeloVariaveis>
                                                 {
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "NomePaciente"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Nascimento"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "CodigoAtendimento"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Atendimento"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Matricula"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Convenio"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "NomeVisitante"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Documento"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Entrada"
                                                         },
                                                     new TipoModeloVariaveis
                                                         {
                                                             TipoModeloId = 3, Descricao = "Fornecedor"
                                                         },
                                                     new TipoModeloVariaveis { TipoModeloId = 3, Descricao = "Local" },
                                                     new TipoModeloVariaveis { TipoModeloId = 3, Descricao = "Tipo" },
                                                 };

            var etiquetaPulseiraVariaveis = new List<TipoModeloVariaveis>
                                                {
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 4, Descricao = "NomePaciente"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 4, Descricao = "Nascimento"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 4, Descricao = "CodigoAtendimento"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 4, Descricao = "Atendimento"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 4, Descricao = "Matricula"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 4, Descricao = "Convenio"
                                                        },
                                                };

            var terminalDeSenhaVariaveis = new List<TipoModeloVariaveis>
                                                {
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 5, Descricao = "NomeHospital"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 5, Descricao = "Numero"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 5, Descricao = "Fila"
                                                        },
                                                    new TipoModeloVariaveis
                                                        {
                                                            TipoModeloId = 5, Descricao = "Data"
                                                        }
                                                };

            this.context.TipoModelo.AddOrUpdate(x => x.Id, new TipoModelo { Id = 1, Descricao = "Ficha Paciente", });
            this.context.TipoModelo.AddOrUpdate(x => x.Id, new TipoModelo { Id = 2, Descricao = "Etiqueta Paciente" });

            this.context.TipoModelo.AddOrUpdate(x => x.Id, new TipoModelo { Id = 3, Descricao = "Etiqueta Visitante" });

            this.context.TipoModelo.AddOrUpdate(x => x.Id, new TipoModelo { Id = 4, Descricao = "Pulseira", });

            this.context.TipoModelo.AddOrUpdate(x => x.Id, new TipoModelo { Id = 5, Descricao = "Terminal De Senha", });

            this.context.SaveChanges();

            this.context.TipoModeloVariaveis.AddOrUpdate(
                x => new { x.TipoModeloId, x.Descricao },
                listFichaPaciente.Select(item => new TipoModeloVariaveis { TipoModeloId = 1, Descricao = item })
                    .Concat(etiquetaPacienteVariaveis)
                    .Concat(etiquetaVisitanteVariaveis)
                    .Concat(terminalDeSenhaVariaveis)
                    .Concat(etiquetaPulseiraVariaveis).ToArray());

            this.context.SaveChanges();

            var tamanhoList = new List<TamanhoModelo>
                                  {
                                      new TamanhoModelo
                                          {
                                              Id = 1,
                                              Descricao = "A0",
                                              AlturaCm = 118.9,
                                              LarguraCm = 84.1,
                                              LarguraPixel = 2384,
                                              AlturaPixel = 3370
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 2,
                                              Descricao = "A1",
                                              AlturaCm = 84.1,
                                              LarguraCm = 59.4,
                                              LarguraPixel = 2245.0393701,
                                              AlturaPixel = 3178.5826772,
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 3,
                                              Descricao = "A2",
                                              AlturaCm = 59.4,
                                              LarguraCm = 42,
                                              LarguraPixel = 1587.4015748,
                                              AlturaPixel = 2245.0393701
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 4,
                                              Descricao = "A3",
                                              AlturaCm = 42,
                                              LarguraCm = 29.7,
                                              LarguraPixel = 1122.519685,
                                              AlturaPixel = 1587.4015748
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 5,
                                              Descricao = "A4",
                                              AlturaCm = 29.7,
                                              LarguraCm = 21,
                                              LarguraPixel = 793.7007874,
                                              AlturaPixel = 1122.519685
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 6,
                                              Descricao = "A5",
                                              AlturaCm = 21,
                                              LarguraCm = 14.8,
                                              LarguraPixel = 559.37007874,
                                              AlturaPixel = 793.7007874
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 7,
                                              Descricao = "Etiqueta Visitante 5cm por 9.6cm",
                                              AlturaCm = 9.6,
                                              LarguraCm = 5,
                                              LarguraPixel = 188.97637795,
                                              AlturaPixel = 362.83464567
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 8,
                                              Descricao = "Etiqueta Paciente 3cm por 5cm",
                                              AlturaCm = 5,
                                              LarguraCm = 3,
                                              LarguraPixel = 113.38582677,
                                              AlturaPixel = 188.97637795
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 9,
                                              Descricao = "Pulseira  10cm por 3cm",
                                              AlturaCm = 3,
                                              LarguraCm = 10,
                                              AlturaPixel = 113.38582677,
                                              LarguraPixel = 377.95275591
                                          },
                                      new TamanhoModelo
                                          {
                                              Id = 10,
                                              Descricao = "Terminal de senha  8cm por 7cm",
                                              AlturaCm = 8,
                                              LarguraCm = 7,
                                              AlturaPixel = 302.36220472,
                                              LarguraPixel = 264.56692913
                                          }
                                  };

            this.context.TamanhoModelo.AddOrUpdate(x => x.Id, tamanhoList.ToArray());

            this.context.SaveChanges();

            var textosModelo = new List<TextoModelo>
            {
               new TextoModelo
                   {
                       Codigo = "ET01",
                       Descricao = "Etiqueta Paciente",
                       TipoModeloId = 2,
                       TamanhoModeloId = 8,
                       Texto =
                           @"<p style='margin: 0 0 0 0.20cm; '><span style='font-size:11px; font-weight:bold'>@Paciente</span></p> <table class='table table-bordered' style='margin: 0 0 0 0.15cm; ' border='0'> <tbody> <tr> <td style='padding 0; '><p style='margin: 0'><span style='font-size: 10px; '>Nascimento</span></p></td> <td><p style='margin: 0'><span style='font-size: 10px; '>@DataNascimento</span></p></td> </tr> <tr> <td style='padding 0; '><span style='font-size: 10px; '>Atendimento</span><br></td> <td style='padding 0; '><p style='margin: 0'><span style='font-size: 10px; '>@DataAtendimento</span></p></td></tr> <tr> <td style='padding 0; '><span style='font-size: 10px; '>Convênio</span><br></td> <td style='padding 0; '><p style='margin: 0'><span style='font-size: 10px; '>@Convenio</span></p></td> </tr> <tr><td style='padding 0; '><span style='font-size: 10px; '>Cód. Atend</span><br></td> <td style='padding 0; '><p style='margin: 0'><span style='font-size: 10px; '>@CodigoAtendimento</span></p></td> </tr> <tr> <td style='padding 0; '><span style='font-size: 10px; '>Matrícula</span><br></td> <td style='padding 0; '><p style='margin: 0'><span style='font-size: 10px; '>@MatriculaConvenio</span></p></td> </tr> </tbody> </table>"
                   },
               new TextoModelo
                   {
                       Codigo = "ET02",
                       Descricao = "Etiqueta Visitante",
                       TipoModeloId = 3,
                       TamanhoModeloId = 7,
                       Texto =
                           @"<p style='margin:0 0 0 0.2cm;font-size:16px;font-weight:bold'>@Tipo</p> <table class='table table-bordered' style='margin:0 0 0 0.2cm;'> <tbody> <tr> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 18px;font-weight:bold;'>Nome</span></p></td> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 18px;font-weight:bold;'>@NomeVisitante</span></p></td> </tr> <tr> <td style='padding:0;border0;'><span style='font-size: 16px;'>Documento</span><br></td> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 16px;'>@Documento</span></p></td></tr> <tr> <td style='padding:0;border0;'><span style='font-size: 16px;'>Entrada</span><br></td> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 16px;'>@Entrada</span></p></td></tr> <tr> <td style='padding:0;border0;'><span style='font-size: 16px;'>Fornecedor</span><br></td> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 16px;'>@Fornecedor</span></p></td></tr> <tr> <td style='padding:0;border0;'><span style='font-size: 16px;'>Paciente</span><br></td> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 16px;'>@NomePaciente</span></p></td></tr> <tr> <td style='padding:0;border0;'><span style='font-size: 16px;'>Leito</span></td> <td style='padding:0;border0;'><p style='margin:0'><span style='font-size: 16px;'>@Local</span></p></td></tr> </tbody> </table>"
                   },
               new TextoModelo
                   {
                       Codigo = "ET03",
                       Descricao = "Modelo Pulseira",
                       TipoModeloId = 4,
                       TamanhoModeloId = 9,
                       Texto =
                           @"<p>@NomePaciente</p><table class='table table-bordered'><tbody><tr><td><p style='margin:0'><span style='font-size: 8pt; '>Nascimento</span></p></td><td><p style='margin: 0'><span style='font-size: 8pt; '>@Nascimento</span></p></td></tr><tr><td><span style='font-size: 8pt; '>Cód. Atendimento</span><br></td><td><p style='margin: 0'><span style='font-size: 8pt; '>@CodigoAtendimento</span></p></td></tr><tr><td><span style='font-size: 8pt; '>Atendimento</span><br></td><td><p style='margin: 0'><span style='font-size: 8pt; '>@Atendimento</span></p></td></tr><tr><td><span style='font-size: 8pt; '>Matrícula</span><br></td><td><p style='margin: 0'><span style='font-size: 8pt; '>@Matricula</span></p></td></tr><tr><td><span style='font-size: 8pt; '>Convênio</span><br></td><td><p style='margin: 0'><span style='font - size: 8pt; '>@Convenio</span></p></td></tr></tbody></table>"
                   }
            };

            this.context.TextoModelos.AddOrUpdate(x => new { x.Codigo, x.Descricao }, textosModelo.ToArray());
        }
    }
}