using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.InternacoesTev;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.TiposAcompanhantes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosPerdaProdutos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Grupos;
//using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposDocumento;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Pessoas;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Honorarios;

namespace SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao
{
    public class ConteudoFixoBuilder
    {
        private readonly SWMANAGERDbContext _context;

        public ConteudoFixoBuilder(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateConteudoFixo();

        }

        private void CreateConteudoFixo()
        {
            #region sexo
            var sexos = _context.Sexos.ToList();
            if (sexos == null || sexos.Count() == 0)
            {
                var listSexo = new List<Sexo>
                {
                    new Sexo
                    {
                        Descricao="Masculino",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new Sexo
                    {
                        Descricao="Feminino",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new Sexo
                    {
                        Descricao="Ambos",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listSexo.ForEach(s => _context.Sexos.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region corpele
            var coresPele = _context.CoresPele.ToList();
            if (coresPele == null || coresPele.Count() == 0)
            {
                var listCores = new List<CorPele>
                {
                    new CorPele
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Branca",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new CorPele
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Parda",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new CorPele
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Preta",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new CorPele
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Amarela",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new CorPele
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Indígena",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
                listCores.ForEach(c => _context.CoresPele.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region escolaridade
            var escolaridades = _context.Escolaridades.ToList();
            if (escolaridades == null || escolaridades.Count() == 0)
            {
                var listaEscolaridades = new List<Escolaridade>
                {
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Analfabeto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Ensino fundamental incompleto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Ensino fundamental completo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Ensino médio incompleto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Ensino médio completo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Curso superior incompleto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Curso superior completo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Pós graduação incompleta",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Pós graduação completa",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Mestrado incompleto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Mestrado completo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Doutorado incompleto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Escolaridade
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Doutorado completo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
                listaEscolaridades.ForEach(e => _context.Escolaridades.Add(e));
                _context.SaveChanges();
            }
            #endregion

            #region religiao
            var religioes = _context.Religioes.ToList();
            if (religioes == null || religioes.Count() == 0)
            {
                var listaReligioes = new List<Religiao>
                {
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Católica",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Protestante",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Adventista",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Mórmon",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Ortodoxa",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Testemunha de Jeová",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Espírita",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Islâmica",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Judaica",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Neo Pagã",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Afro Brasileira",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Indígena",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Hinduísta",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Hoasqueira",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Religiao
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Não Religiosa",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
                listaReligioes.ForEach(r => _context.Religioes.Add(r));
                _context.SaveChanges();
            }
            #endregion

            #region estadocivil
            var estadoscivis = _context.EstadosCivis.ToList();
            if (estadoscivis == null || estadoscivis.Count() == 0)
            {
                var listaEstadosCivis = new List<EstadoCivil>
                {
                    new EstadoCivil
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Solteiro",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new EstadoCivil
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Casado",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new EstadoCivil
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Divorciado",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new EstadoCivil
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Viúvo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new EstadoCivil
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Separado",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new EstadoCivil
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Companheiro",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
                listaEstadosCivis.ForEach(e => _context.EstadosCivis.Add(e));
                _context.SaveChanges();
            }
            #endregion

            #region tipotelefone
            var tipostelefone = _context.TiposTelefone.ToList();
            if (tipostelefone == null || tipostelefone.Count() == 0)
            {
                var listaTiposTelefone = new List<TipoTelefone>
                {
                    new TipoTelefone
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Residencial",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoTelefone
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Celular",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoTelefone
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Comercial",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoTelefone
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Rádio",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoTelefone
                    {
                        CreationTime=DateTime.Now,
                        Descricao="Fax",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
                listaTiposTelefone.ForEach(t => _context.TiposTelefone.Add(t));
                _context.SaveChanges();
            }
            #endregion

            #region tipopessoa
            var tiposPessoa = _context.TiposPessoa.ToList();
            if (tiposPessoa == null || tiposPessoa.Count() == 0)
            {
                var listTipoPessoa = new List<TipoPessoa>
                {
                    new TipoPessoa
                    {
                        Descricao = "Física"
                    },
                    new TipoPessoa
                    {
                        Descricao = "Jurídica"
                    }
                };
                listTipoPessoa.ForEach(s => _context.TiposPessoa.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region tipocadastroexistente
            var tiposCadastroExistente = _context.TiposCadastroExistente.ToList();
            if (tiposCadastroExistente == null || tiposCadastroExistente.Count() == 0)
            {
                var listTipoCadastroExistente = new List<TipoCadastroExistente>
                {
                    new TipoCadastroExistente
                    {
                        Descricao="Paciente"
                    },
                    new TipoCadastroExistente
                    {
                        Descricao="Médico"
                    },
                    new TipoCadastroExistente
                    {
                        Descricao="Convênio"
                    },
                    new TipoCadastroExistente
                    {
                        Descricao="Empresa"
                    }
                };
                listTipoCadastroExistente.ForEach(s => _context.TiposCadastroExistente.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region ultimoid

            var totAtendimentos = _context.Atendimentos.Count() + 1;
            var totFatItem = _context.FaturamentoItens.Count() + 1;
            var totExame = _context.SolicitacoesExames.Count() + 1;
            var totMat = _context.Materiais.Count() + 1;
            var totProduto = _context.Produtos.Max(m => m.Codigo) + 1;
            var totMedico = _context.Medicos.Max(m => m.Codigo) + 1;
            var totEspecialidade = _context.Especialidades.Max(m => m.Codigo) + 1;
            var totEstGrupo = _context.Grupos.Max(m => m.Codigo) + 1;
            var totCompraRequisicao = _context.CompraRequisicao.Max(m => m.Codigo) + 1;
            var totPaciente = _context.Pacientes.Max(m => m.Codigo) + 1;

            var listUltimosIds = new List<UltimoId>
                {
                    new UltimoId
                    {
                        NomeTabela="Atendimento",
                        Codigo=totAtendimentos.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new UltimoId
                    {
                        NomeTabela="AdmissaoMedica",
                        Codigo=totAtendimentos.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new UltimoId
                    {
                        NomeTabela="EvolucaoMedica",
                        Codigo=totAtendimentos.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                     new UltimoId
                    {
                        NomeTabela="SaidaProduto",
                        Codigo="0",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new UltimoId
                    {
                        NomeTabela="SolicitacaoExame",
                        Codigo=totAtendimentos.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                      new UltimoId
                    {
                        NomeTabela="TransferenciaProduto",
                        Codigo="0",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },

                    new UltimoId
                    {
                        NomeTabela="DevolucaoProduto",
                        Codigo="0",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new UltimoId
                    {
                        NomeTabela="FaturamentoItem",
                        Codigo=totFatItem.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    // new UltimoId
                    //{
                    //    NomeTabela="SolicitacaoExame",
                    //    Codigo=totExame.ToString(),
                    //    CreationTime=DateTime.Now,
                    //    CreatorUserId=2,
                    //    IsDeleted=false,
                    //    IsSistema=true
                    //},
                     new UltimoId
                    {
                        NomeTabela="Material",
                        Codigo=totMat.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                     new UltimoId
                    {
                        NomeTabela="Produto",
                        Codigo=totProduto.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },

                    new UltimoId
                    {
                        NomeTabela="Solicitacao",
                        Codigo="0",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                     new UltimoId
                    {
                        NomeTabela="Medico",
                        Codigo=totMedico.ToString(),
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                     new UltimoId
                     {
                         NomeTabela = "Especialidade",
                         Codigo = totEspecialidade.ToString(),
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                    },
                     new UltimoId
                     {
                         NomeTabela = "Grupo",
                         Codigo = totEstGrupo.ToString(),
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                    },
                     new UltimoId
                     {
                         NomeTabela = "CompraRequisicao",
                         Codigo = totCompraRequisicao.ToString(),
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     }
                     ,
                     new UltimoId
                     {
                         NomeTabela = "RegistroExame",
                         Codigo = "1",
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     },
                     new UltimoId
                     {
                         NomeTabela = "ColetaLaboratorio",
                         Codigo = "1",
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     },
                     new UltimoId
                     {
                         NomeTabela = "Paciente",
                         Codigo = totPaciente.ToString(),
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     },

                      new UltimoId
                     {
                         NomeTabela = "Emergencia",
                         Codigo = "1",
                         TamanhoCampo=8,
                         complementoEsquerda="0",
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     },

                    new UltimoId
                     {
                         NomeTabela = "Internacao",
                         Codigo = "1",
                         TamanhoCampo=8,
                         complementoEsquerda="0",
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     },

                      new UltimoId
                     {
                         NomeTabela = "Inventario",
                         Codigo = "1",
                         TamanhoCampo=8,
                         complementoEsquerda="0",
                         CreationTime = DateTime.Now,
                         CreatorUserId = 2,
                         IsDeleted = false,
                         IsSistema = true
                     }

                };



            var ultimosIds = _context.UltimosIds.ToList();
            if (ultimosIds == null || ultimosIds.Count() == 0 || ultimosIds.Count() < listUltimosIds.Count())
            {
                var itens = new List<UltimoId>();
                foreach (var item in listUltimosIds)
                {
                    var temp = ultimosIds.Where(m => m.NomeTabela == item.NomeTabela).FirstOrDefault();
                    if (temp != null)
                    {
                        itens.Add(item);
                    }
                }
                //listUltimosIds.Remove(itens);
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                itens.ForEach(c => listUltimosIds.Remove(c));
                listUltimosIds.ForEach(c => _context.UltimosIds.Add(c));
                _context.SaveChanges();
            }

            //if (ultimosIds == null || ultimosIds.Count() < listUltimosIds.Count())
            //{
            //    listUltimosIds.RemoveRange(0, ultimosIds.Count());
            //    listUltimosIds.ForEach(c => _context.UltimosIds.Add(c));
            //    _context.SaveChanges();
            //}
















            //var ultimosIds = _context.UltimosIds.ToList();
            //if (ultimosIds == null || ultimosIds.Count() != 7)
            //{


            //    foreach (var item in ultimosIds)
            //    {
            //        var temp = listUltimosIds.Where(m => m.NomeTabela == item.NomeTabela).FirstOrDefault();
            //        if (temp != null)
            //        {
            //            listUltimosIds.Remove(temp);
            //        }
            //    }
            //    //listUltimosIds.RemoveRange(0, ultimosIds.Count());
            //    listUltimosIds.ForEach(s => _context.UltimosIds.Add(s));
            //}
            #endregion

            #region EstTipoMovimento

            var listEstTipoMovimentos = new List<TipoMovimento>
                {
                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.NotaFiscal_Entrada,
                        Descricao = "Nota Fiscal",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Setor_Saida,
                        Descricao = "Setor",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Paciente_Saida,
                        Descricao = "Paciente",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Perda_Saida,
                        Descricao = "Perda",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.GastoSala_Saida,
                        Descricao = "Gasto de Sala",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Emprestimo_Entrada,
                        Descricao = "Empréstimo",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Doacao_Entrada,
                        Descricao = "Doação",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Vale_Entrada,
                        Descricao = "Vale",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Emprestimo_Saida,
                        Descricao = "Empréstimo",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    },

                    new TipoMovimento
                    {
                        //Id = (long)EnumTipoMovimento.Consignado_Entrada,
                        Descricao = "Consignado",
                        IsEntrada = true,
                        IsOrdemCompra = true,
                        IsPessoa = true,
                        IsOrdemCompraObrigatoria = true,
                        IsFiscal = true,
                        IsFrete = true,
                        IsFinanceiro = true

                    }
                    ,
                     new TipoMovimento
                    {
                        Id = (long)EnumTipoMovimento.Inventario_Entrada,
                        Descricao = "Inventário",
                        IsEntrada = true,
                        IsOrdemCompra = false,
                        IsPessoa = false,
                        IsOrdemCompraObrigatoria = false,
                        IsFiscal = false,
                        IsFrete = false,
                        IsFinanceiro = false

                    },
                       new TipoMovimento
                    {
                        Id = (long)EnumTipoMovimento.Inventario_Saida,
                        Descricao = "Inventário",
                        IsEntrada = false,
                        IsOrdemCompra = false,
                        IsPessoa = false,
                        IsOrdemCompraObrigatoria = false,
                        IsFiscal = false,
                        IsFrete = false,
                        IsFinanceiro = false

                    }
                };

            var lstEstTipoMovimentos = _context.EstTipoMovimentos.ToList();
            if (lstEstTipoMovimentos != null || lstEstTipoMovimentos.Count > 0)
                foreach (var item in lstEstTipoMovimentos)
                {
                    if (!item.Descricao.Contains("Inventário"))
                    {
                        item.IsOrdemCompra = true;
                        item.IsPessoa = true;
                        item.IsOrdemCompraObrigatoria = true;
                        item.IsFiscal = true;
                        item.IsFrete = true;
                        item.IsFinanceiro = true;
                    }
                }
            if (lstEstTipoMovimentos == null || lstEstTipoMovimentos.Count() == 0 || lstEstTipoMovimentos.Count() < listEstTipoMovimentos.Count())
            {
                foreach (var item in lstEstTipoMovimentos)
                {
                    var temp = listEstTipoMovimentos.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listEstTipoMovimentos.Remove(temp);
                    }
                }
                listEstTipoMovimentos.ForEach(c => _context.EstTipoMovimentos.Add(c));
                //_context.SaveChanges();                
            };

            var isInventario = true;
            foreach (var item in _context.EstTipoMovimentos)
            {
                if (item.Descricao.Contains("Inventário"))
                    isInventario = false;
            }
            if (isInventario)
                SeedTipoMovimento.ReSeedTable<TipoMovimento>(_context);
            _context.SaveChanges();

            #endregion

            #region TextoModelo           

            var listTextoModelo = _context.TextoModelos.ToList();
            if (listTextoModelo.Count() == 0)
            {
                var textoModelo = new TextoModelo();
                textoModelo.Codigo = "0001";
                textoModelo.Descricao = "Modelo Padrão";
                textoModelo.Codigo = "0001";
                textoModelo.IsAmbulatorioEmergencia = false;
                textoModelo.IsInternacao = false;
                textoModelo.IsMostraAtendimento = false;
                textoModelo.Texto = "<table class=\"table table-bordered\"><tbody><tr><td><p style=\"font-size: 13px;\"><span style=\"font-size: 12px;\"><span style=\"font-size: 10px;\">&nbsp; &nbsp; &nbsp;</span><span style=\"font-size: 9px;\">@EmpresaRazaoSocial</span></span></p></td><td style=\"text-align: right;\"><span style=\"font-size: 9px;\"><span style=\"font-size: 8px;\">@DataHora</span><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp;</span></span></td></tr><tr><td><span style=\"font-size: 10px;\"><span style=\"font-size: 8px;\">&nbsp;</span><span style=\"font-size: 8px;\"> &nbsp; &nbsp;</span><span style=\"font-size: 8px;\">@Empresa</span></span><br></td><td style=\"text-align: right;\"><span style=\"font-size: 9px;\"><span style=\"font-size: 8px;\">@Usuario</span><span style=\"font-size: 8px;\">&nbsp; &nbsp;&nbsp;</span></span></td></tr></tbody></table><p style=\"text-align: center;\"><b><span style=\"font-size: 9px;\">__________________________________________________________________________________________________________________</span></b></p><table class=\"table table-bordered\"><tbody><tr><td><span style=\"font-size: 8px;\">@CodigoBarra</span><br></td><td style=\"text-align: center;\"><span style=\"font-size: 8px;\"><b>Ficha de Atendimento</b></span><br></td><td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</td></tr><tr><td><span style=\"color: rgb(0, 0, 255);\"><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">PACIENTE</span></span><span style=\"font-size: 8px;\">:</span><span style=\"font-size: 8px;\">&nbsp;</span></span><span style=\"font-size: 8px;\">@CodigoPaciente</span><br></td><td style=\"text-align: center;\"><span style=\"font-size: 8px;\"><i>@Paciente</i></span></td><td style=\"text-align: center;\"><span style=\"font-size: 8px;\">&nbsp;</span><span style=\"color: rgb(0, 0, 255);\"><span style=\"font-weight: 700; text-align: right;\"><span style=\"font-size: 8px;\">ATENDIMENTO</span></span><span style=\"text-align: right; font-size: 8px;\">:</span></span><span style=\"text-align: right; font-size: 8px;\">&nbsp;</span><span style=\"text-align: right; font-size: 8px;\">@DataAtendimento</span></td></tr></tbody></table><table class=\"table table-bordered\"><tbody><tr><td><b><span style=\"font-size: 9px;\"><span style=\"color: rgb(0, 0, 255); font-size: 8px;\">END.</span><span style=\"font-size: 8px;\">&nbsp;</span></span></b><span style=\"font-size: 8px;\">@Endereco</span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">Nº</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Numero</span></span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">COMP.</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Complemento</span></span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">NASCIMENTO</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Nascimento</span></span></td></tr><tr><td><b><span style=\"font-size: 12px;\"><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">BAIRRO</span><span style=\"font-size: 8px;\">&nbsp;</span></span></b><span style=\"font-size: 8px;\">@Bairro</span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">PAÍS</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Pais</span></span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">CIDADE</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Cidade</span></span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">IDENTIDADE</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Identidade</span></span></td></tr><tr><td><b><span style=\"font-size: 12px;\"><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">ESTADO</span><span style=\"font-size: 8px;\">&nbsp;</span></span></b><span style=\"font-size: 8px;\">@Estado</span></td><td><br></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">TEL.</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Telefone</span></span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">CPF </span></b><span style=\"font-size: 8px;\">@Cpf</span></span></td></tr><tr><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 9px;\"><span style=\"color: rgb(0, 0, 255); font-size: 8px;\">NAC.</span><span style=\"font-size: 8px;\">&nbsp;</span></span></b><span style=\"font-size: 8px;\">@Nacionalidade</span></span></td><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 8px; color: rgb(0, 0, 255);\">SEXO</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Sexo</span></span></td><td><span style=\"font-size: 12px;\"><b style=\"color: rgb(0, 0, 255);\"><span style=\"font-size: 8px;\">PROF.</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@Profissao</span></span></td><td><span style=\"font-size: 12px;\"><b style=\"color: rgb(0, 0, 255);\"><span style=\"font-size: 8px;\">E.CIV.</span><span style=\"font-size: 8px;\"></span></b><span style=\"font-size: 8px;\">@SituacaoCivil</span></span></td></tr><tr><td><span style=\"font-size: 12px;\"><b><span style=\"font-size: 9px;\"><span style=\"color: rgb(0, 0, 255); font-size: 8px;\">FIL</span><span style=\"font-size: 8px;\">.</span></span><span style=\"font-size: 8px;\">&nbsp;</span></b><span style=\"font-size: 8px;\">@Filiacao&nbsp;</span></span></td><td><p><span style=\"font-size: 8px;\"></span><br></p></td><td><p><span style=\"font-size: 8px;\"></span><br></p></td><td><p><span style=\"font-size: 8px;\"></span><br></p></td></tr></tbody></table><table class=\"table table-bordered\"><tbody><tr><td><b><span style=\"font-size: 9px;\">&nbsp; &nbsp; &nbsp; <span style=\"font-size: 8px;\">&nbsp; &nbsp; MÉDICO:</span></span></b><span style=\"font-size: 8px;\"> @Medico</span></td><td><b><span style=\"font-size: 8px;\">CÓD.ATENDIMENTO:</span></b><span style=\"font-size: 8px;\"> @CodigoAtendimento</span></td></tr><tr><td><b><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ESPECIALIDADE:</span></b><span style=\"font-size: 8px;\"> @Especialidade</span></td><td><b><span style=\"font-size: 8px;\">DATA ALTA:</span></b><span style=\"font-size: 8px;\"> @DataAlta</span></td></tr><tr><td><b><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; INDICADO:</span></b><span style=\"font-size: 8px;\"> @IndicadoPor</span></td><td><b><span style=\"font-size: 8px;\">ALTA:</span></b><span style=\"font-size: 8px;\"> @Alta</span></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">ORIGEM:</span></b><span style=\"font-size: 8px;\"> @Origem</span></td><td><b><span style=\"font-size: 8px;\">ASS.RESPONSÁVEL: ___________________________</span></b></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">TRATAMENTO:</span></b><span style=\"font-size: 8px;\"> @Tratamento</span></td><td><b><span style=\"font-size: 8px;\">MATRÍCULA:</span></b><span style=\"font-size: 8px;\"> @Matricula</span></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">CONVÊNIO:</span></b><span style=\"font-size: 8px;\"> @Convenio</span></td><td><b><span style=\"font-size: 8px;\">DATA VALIDADE: </span></b><span style=\"font-size: 8px;\"></span><font size=\"1\"><span style=\"font-size: 8px;\">@DataValidade</span></font></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">PLANO:</span></b><span style=\"font-size: 8px;\"> @Plano</span></td><td><b><span style=\"font-size: 8px;\">DATA PAGTO:</span></b><span style=\"font-size: 8px;\">&nbsp;@DataPagto</span></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">GUIA:</span></b><span style=\"font-size: 8px;\"> @Guia</span></td><td><b><span style=\"font-size: 8px;\">ID. ACOMPANHANTE: </span></b><span style=\"font-size: 8px;\">@</span><font size=\"1\"><span style=\"font-size: 8px;\">IdAcompanhante</span></font></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">NUM.GUIA:</span></b><span style=\"font-size: 8px;\"> @NumberGuide</span></td><td><br></td></tr><tr><td><span style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span></span><b><span style=\"font-size: 8px;\">TITULAR:</span></b><span style=\"font-size: 8px;\"> @Titular</span></td><td><b><span style=\"font-size: 8px;\">CÓD.DEP.:</span></b><span style=\"font-size: 8px;\"> @CodDep</span></td></tr></tbody></table><p style=\"text-align: center;\"><span style=\"font-weight: 700;\"><span style=\"font-size: 9px;\">__________________________________________________________________________________________________________________</span></span><br></p><p style=\"text-align: center;\"><span style=\"font-size: 9px;\">@[<b>&nbsp; &nbsp; </b><span style=\"font-weight: bold; font-size: 8px;\">ÚLTIMOS ATENDIMENTOS</span></span><span style=\"font-size: 14px;\"></span></p><table class=\"table table-bordered\"><tbody><tr><td><span style=\"font-weight: bold; font-size: 8px;\">Data:</span><span style=\"font-size: 8px;\"> @1DtHoraAtendimento</span></td><td><span style=\"font-weight: bold; font-size: 8px;\">Médico:</span><span style=\"font-size: 8px;\"> @1MedicoAtendimento</span></td><td><span style=\"font-weight: bold; font-size: 8px;\">Convênio:</span><span style=\"font-size: 8px;\"> @1ConvenioAtendimento</span></td><td><span style=\"font-weight: bold; font-size: 8px;\">Espec.: </span><span style=\"font-size: 8px;\">@1Espec</span></td></tr><tr><td><span style=\"font-weight: bold; font-size: 8px;\">Data:&nbsp;</span><span style=\"font-size: 8px;\">@2DtHoraAtendimento</span><br></td><td><span style=\"font-weight: bold; font-size: 8px;\">Médico:&nbsp;</span><span style=\"font-size: 8px;\">@2MedicoAtendimento</span><br></td><td><span style=\"font-weight: bold; font-size: 8px;\">Convênio:&nbsp;</span><span style=\"font-size: 8px;\">@2ConvenioAtendimento</span><br></td><td><span style=\"font-weight: bold; font-size: 8px;\">Espec.:&nbsp;</span><span style=\"font-size: 8px;\">@2Espec</span><br></td></tr></tbody></table><p style=\"text-align: center;\"><span style=\"font-size: 9px;\">@]<b>&nbsp; </b><span style=\"font-weight: bold; text-decoration-line: underline; font-size: 8px;\">História da Doença Atual</span></span></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span><br></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span><br></p><p style=\"text-align: center;\"><b><u><span style=\"font-size: 8px;\">Exame Físico Dirigido</span></u></b></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span><br></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span></p><p style=\"text-align: center;\"><span style=\"font-weight: 700;\"><u><span style=\"font-size: 8px;\">Exame Complementares Solicitados e Resultados</span></u></span></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span><br></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span></p><p style=\"text-align: center;\"><span style=\"font-weight: 700;\"><u><span style=\"font-size: 8px;\">Hipótese Diagnóstica</span></u></span></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span><br></p><p style=\"text-align: center;\"><span style=\"font-size: 8px;\">__________________________________________________________________________________________________________________</span></p><p style=\"text-align: center;\"><u style=\"font-weight: 700;\"><span style=\"font-size: 8px;\">Tratamentos</span></u></p><table class=\"table table-bordered\" style=\"text-align: center;\"><tbody><tr><td><u style=\"font-size: 13px; text-align: center; font-weight: 700;\"><span style=\"font-size: 9px;\">Hospitalar</span></u><br></td><td><u style=\"font-size: 13px; text-align: center; font-weight: 700;\"><span style=\"font-size: 9px;\">Domiciliar</span></u><br></td></tr><tr><td style=\"text-align: center;\"><span style=\"font-size: 9px;\">___________________________________________</span><br></td><td style=\"text-align: center;\"><span style=\"font-size: 9px;\">___________________________________________</span><br></td></tr><tr><td style=\"text-align: center;\"><p><span style=\"font-size: 9px;\">___________________________________________</span><br></p></td><td style=\"text-align: center;\"><span style=\"font-size: 9px;\">___________________________________________</span></td></tr><tr><td style=\"text-align: center;\"><span style=\"font-size: 9px;\">___________________________________________</span><br></td><td style=\"text-align: center;\"><span style=\"font-size: 9px;\">___________________________________________</span><br></td></tr></tbody></table><p style=\"text-align: center;\"><span style=\"font-size: 14px;\"></span><b style=\"font-size: 8px;\">________________________</b><span style=\"font-size: 14px;\"></span><b style=\"font-size: 8px;\">________________</b></p><p style=\"text-align: center;\"><b style=\"font-size: 8px;\">Médico - Assinatura e Carimbo</b><br></p><p style=\"text-align: center;\"><u style=\"font-weight: 700;\"><span style=\"font-size: 11px;\"><br></span></u><span style=\"font-size: 9px;\"><br></span></p>";

                var objTextoModelo = _context.TextoModelos.Add(textoModelo);

                foreach (var itemEmpresa in _context.Empresas.ToList())
                {
                    var textoEmpresa = new TextoModeloEmpresa();
                    textoEmpresa.TextoId = objTextoModelo.Id;
                    textoEmpresa.EmpresaId = itemEmpresa.Id;
                    textoEmpresa.Codigo = "0001";
                    textoEmpresa.Descricao = "Modelo padrão da empresa";
                    var objTextoEmpresa = _context.TextoModeloEmpresas.Add(textoEmpresa);
                }

                foreach (var itemGuia in _context.FaturamentoGuias.ToList())
                {
                    if (itemGuia.Id == 14)
                    {
                        var textoGuia = new TextoModeloGuia();
                        textoGuia.TextoId = objTextoModelo.Id;
                        textoGuia.FatGuiaId = itemGuia.Id;
                        textoGuia.Codigo = "0001";
                        textoGuia.Descricao = "Modelo padrão de guia da empresa";

                        _context.TextoModeloGuias.Add(textoGuia);
                    }
                }

                _context.SaveChanges();
            }

            #endregion

            #region TipoAcompanhante           

            var listTipoAcompanhante = _context.TipoAcompanhante.ToList();
            if (listTipoAcompanhante.Count() == 0)
            {
                var lstTiposAcompanhantes = new List<TipoAcompanhante>();
                var convenio = new TipoAcompanhante();
                convenio.Id = (long)EnumTipoAcompanhante.Convenio;
                convenio.Codigo = "0001";
                convenio.Descricao = "Convênio";
                convenio.IsInternacao = true;
                lstTiposAcompanhantes.Add(convenio);

                var nenhum = new TipoAcompanhante();
                nenhum.Id = (long)EnumTipoAcompanhante.Nenhum;
                nenhum.Codigo = "0003";
                nenhum.Descricao = "Nenhum";
                nenhum.IsInternacao = true;
                lstTiposAcompanhantes.Add(nenhum);

                var particular = new TipoAcompanhante();
                particular.Id = (long)EnumTipoAcompanhante.Particular;
                particular.Codigo = "0002";
                particular.Descricao = "Particular";
                particular.IsInternacao = true;
                lstTiposAcompanhantes.Add(particular);

                var porIdade = new TipoAcompanhante();
                porIdade.Id = (long)EnumTipoAcompanhante.Por_Idade;
                porIdade.Codigo = "0004";
                porIdade.Descricao = "Por Idade";
                porIdade.IsInternacao = true;
                lstTiposAcompanhantes.Add(porIdade);

                lstTiposAcompanhantes.ForEach(s => _context.TipoAcompanhante.Add(s));

                _context.SaveChanges();
            }

            #endregion

            #region EstTipoOperacao

            //var estTipoOperacoes = _context.EstTipoOperacoes.ToList();
            //if (estTipoOperacoes == null || estTipoOperacoes.Count() == 0)
            //{

            //    SeedSuprimentos.ReSeedTable<TipoOperacao>(_context);

            //    var listEstTipoOperacaoes = new List<TipoOperacao>
            //    {
            //        new TipoOperacao
            //        {
            //            Descricao = "Entrada"
            //        },

            //        new TipoOperacao
            //        {
            //            Descricao = "Transferência"
            //        },

            //        new TipoOperacao
            //        {
            //            Descricao = "Saída"
            //        },

            //        new TipoOperacao
            //        {
            //            Descricao = "Devolução"
            //        },

            //        new TipoOperacao
            //        {
            //            Descricao = "Produção de Kits"
            //        },


            //        new TipoOperacao
            //        {
            //            Descricao = "Solicitações"
            //        },

            //        new TipoOperacao
            //        {
            //            Descricao = "Inventário"
            //        }
            //    };

            //    listEstTipoOperacaoes.ForEach(s => _context.EstTipoOperacoes.Add(s));
            //    _context.SaveChanges();
            // }
            //SeedSuprimentos.ReSeedTable<TipoOperacao>(_context);
            SeedSuprimentos.SemAutoIncrementoTable<TipoOperacao>(_context);
            var listEstTipoOperacaoes = new List<TipoOperacao>
            {
                 new TipoOperacao { Id=1, Descricao = "Entrada" },
                 new TipoOperacao { Id=2, Descricao = "Transferência" },
                 new TipoOperacao { Id=3, Descricao = "Saída" },
                 new TipoOperacao { Id=4, Descricao = "Devolução" },
                 new TipoOperacao { Id=5, Descricao = "Produção de Kits" },
                 new TipoOperacao { Id=6, Descricao = "Solicitações" },
                 new TipoOperacao { Id=7, Descricao = "Inventário" }
            };

            var tipoOperacoes = _context.EstTipoOperacoes.ToList();
            if (tipoOperacoes == null || tipoOperacoes.Count() < listEstTipoOperacaoes.Count())
            {
                foreach (var item in tipoOperacoes)
                {
                    var temp = listEstTipoOperacaoes.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listEstTipoOperacaoes.Remove(temp);
                    }
                }

                listEstTipoOperacaoes.ForEach(c => _context.EstTipoOperacoes.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region FaturamentoAtendimentoStatus

            var faturamentoAtendimentoStatus = _context.FaturamentoAtendimentoStatus.ToList();
            if (faturamentoAtendimentoStatus == null || faturamentoAtendimentoStatus.Count() == 0)
            {
                var listFaturamentoAtendimentoStatus = new List<FaturamentoAtendimentoStatus>
                {
                    new FaturamentoAtendimentoStatus
                    {
                        Id = FaturamentoAtendimentoStatus.Pendente,
                        Descricao = "Pendente",
                        IsDeleted = false
                    },
                    new FaturamentoAtendimentoStatus
                    {
                        Id = FaturamentoAtendimentoStatus.Parcial,
                        Descricao = "Parcial",
                        IsDeleted = false
                    },
                    new FaturamentoAtendimentoStatus
                    {
                        Id = FaturamentoAtendimentoStatus.Glosado,
                        Descricao = "Glosado",
                        IsDeleted = false
                    },
                    new FaturamentoAtendimentoStatus
                    {
                        Id = FaturamentoAtendimentoStatus.Finalizado,
                        Descricao = "Finalizado",
                        IsDeleted = false
                    }
                };
                listFaturamentoAtendimentoStatus.ForEach(s => _context.FaturamentoAtendimentoStatus.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region faturamentotipogrupo
            var faturamentoTiposGrupo = _context.FaturamentoTiposGrupo.ToList();
            if (faturamentoTiposGrupo == null || faturamentoTiposGrupo.Count() == 0)
            {
                var listTipoGrupo = new List<FaturamentoTipoGrupo>
                {
                    new FaturamentoTipoGrupo
                    {
                        Descricao="Honorários",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FaturamentoTipoGrupo
                    {
                        Descricao="Serviços",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FaturamentoTipoGrupo
                    {
                        Descricao="Produtos",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FaturamentoTipoGrupo
                    {
                        Descricao="Pacote",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listTipoGrupo.ForEach(s => _context.FaturamentoTiposGrupo.Add(s));
                _context.SaveChanges();
            }

            #endregion

            #region MotivosPerdaProdutos

            var MotivosPerdaProdutos = _context.MotivosPerdaProdutos.ToList();
            if (MotivosPerdaProdutos == null || MotivosPerdaProdutos.Count() == 0)
            {
                SeedSuprimentos.ReSeedTable<MotivoPerdaProduto>(_context);

                var listMotivosPerdaProdutos = new List<MotivoPerdaProduto>
                {
                    new MotivoPerdaProduto {Descricao="Validade Vencida" },
                    new MotivoPerdaProduto {Descricao="Danificado" },
                    new MotivoPerdaProduto {Descricao="Extravio" },
                };

                listMotivosPerdaProdutos.ForEach(s => _context.MotivosPerdaProdutos.Add(s));
            }

            #endregion

            #region solicitacaoexameprioridade
            var solicitacaoExamePrioridades = _context.SolicitacaoExamePrioridades.ToList();
            if (solicitacaoExamePrioridades == null || solicitacaoExamePrioridades.Count() == 0)
            {
                var listSolicitacaoExamePrioridades = new List<SolicitacaoExamePrioridade>
                {
                    new SolicitacaoExamePrioridade
                    {
                        Codigo="1",
                        Descricao="Rotina",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new SolicitacaoExamePrioridade
                    {
                        Codigo="2",
                        Descricao="Urgência",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listSolicitacaoExamePrioridades.ForEach(s => _context.SolicitacaoExamePrioridades.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region Frequencia
            var frequencias = _context.Frequencias.ToList();
            if (frequencias == null || frequencias.Count() == 0)
            {
                var listFrequencias = new List<Frequencia>
                {
                    new Frequencia
                    {
                        Codigo="1",
                        Descricao="2/2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new Frequencia
                    {
                        Codigo="2",
                        Descricao="4/4",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new Frequencia
                    {
                        Codigo="3",
                        Descricao="6/6",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    //new Frequencia
                    //{
                    //    Codigo="4",
                    //    Descricao="ACM",
                    //    CreationTime=DateTime.Now,
                    //    CreatorUserId=2,
                    //    IsDeleted=false,
                    //    IsSistema=true
                    //},
                    //new Frequencia
                    //{
                    //    Codigo="5",
                    //    Descricao="Agora",
                    //    CreationTime=DateTime.Now,
                    //    CreatorUserId=2,
                    //    IsDeleted=false,
                    //    IsSistema=true
                    //},
                    new Frequencia
                    {
                        Codigo="4",
                        Descricao="Continuo",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                };
                listFrequencias.ForEach(s => _context.Frequencias.Add(s));
                _context.SaveChanges();
            }

            #endregion

            #region FormaAplicacao
            var listFormasAplicacoes = new List<FormaAplicacao>
                {
                    new FormaAplicacao
                    {
                        Codigo="1",
                        Descricao="n/a",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="2",
                        Descricao="EV",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="3",
                        Descricao="SC",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="4",
                        Descricao="IM",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="5",
                        Descricao="SNC",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="6",
                        Descricao="AEROSOL",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="7",
                        Descricao="ID",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="8",
                        Descricao="NO TUBO",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="9",
                        Descricao="VO",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="10",
                        Descricao="SL",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="11",
                        Descricao="NO SORO",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="12",
                        Descricao="NA ETAPA",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="13",
                        Descricao="PUNÇÃO VEIA CENTRAL",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="14",
                        Descricao="RESPIRADOR",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="15",
                        Descricao="Tópico",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="16",
                        Descricao="IRRIGAÇÃO VESICAL",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="17",
                        Descricao="VAS",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="18",
                        Descricao="MACRONEBULIZAÇÃO",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="19",
                        Descricao="CNE",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="20",
                        Descricao="GTT",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="21",
                        Descricao="VR",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="22",
                        Descricao="NO SORO",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new FormaAplicacao
                    {
                        Codigo="23",
                        Descricao="INAL",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                };

            var formasAplicacoes = _context.FormasAplicacao.ToList();
            if (formasAplicacoes == null || formasAplicacoes.Count() == 0 || formasAplicacoes.Count() < listFormasAplicacoes.Count())
            {
                foreach (var item in formasAplicacoes)
                {
                    var temp = listFormasAplicacoes.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listFormasAplicacoes.Remove(temp);
                    }
                }
                listFormasAplicacoes.ForEach(s => _context.FormasAplicacao.Add(s));
                _context.SaveChanges();
            }

            #endregion

            #region VelocidadeInfusao
            var listVelocidadesInfusao = new List<VelocidadeInfusao>
                {
                    new VelocidadeInfusao
                    {
                        Codigo="1",
                        Descricao="n/a",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="2",
                        Descricao="Bolus",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="3",
                        Descricao="INF. LENTA",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="4",
                        Descricao="Infusão Contínua",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="5",
                        Descricao="BI",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="6",
                        Descricao="Soro",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="7",
                        Descricao="Na Sonda",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="8",
                        Descricao="Microgotas",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new VelocidadeInfusao
                    {
                        Codigo="9",
                        Descricao="Aplicação Direta",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                };

            var velocidadesInfusao = _context.VelocidadesInfusao.ToList();
            if (velocidadesInfusao == null || velocidadesInfusao.Count() == 0 || velocidadesInfusao.Count() < listVelocidadesInfusao.Count())
            {
                foreach (var item in velocidadesInfusao)
                {
                    var temp = listVelocidadesInfusao.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listVelocidadesInfusao.Remove(temp);
                    }
                }
                listVelocidadesInfusao.ForEach(s => _context.VelocidadesInfusao.Add(s));
                _context.SaveChanges();
            }

            #endregion

            #region Turno
            var turnos = _context.Turnos.ToList();
            if (turnos == null || turnos.Count() == 0)
            {
                var listTurnos = new List<Turno>
                {
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Normal",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Noturno",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Sábado à tarde",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="Domingo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="Feriado",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "PRO",
                        Descricao="Produto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Turno
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "PCT",
                        Descricao="Pacote",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
                listTurnos.ForEach(c => _context.Turnos.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region TipoResposta
            var listTiposRespostas = new List<TipoResposta>
                {
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Quantidade",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Unidade de medida",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Velocidade de infusao",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="Duração",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="Forma de aplicação",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "6",
                        Descricao="Frequência",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "7",
                        Descricao="Setor",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "8",
                        Descricao="Médico",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "9",
                        Descricao="Início",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "10",
                        Descricao="Dias de aplicação",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "11",
                        Descricao="Observação",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "12",
                        Descricao="Cópia de Descrição",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "13",
                        Descricao="Tipo de Medicação",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "14",
                        Descricao="Exame de diagnóstico de imagem",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "15",
                        Descricao="Exame laboratorial",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "16",
                        Descricao="Setor de exames",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "17",
                        Descricao="Produtos do estoque",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "18",
                        Descricao="Controla volume",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoResposta
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "19",
                        Descricao="Sangue e derivados",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    }
                };
            var tiposRespostas = _context.TiposRespostas.ToList();
            if (tiposRespostas == null || tiposRespostas.Count() < listTiposRespostas.Count())
            {
                listTiposRespostas.RemoveRange(0, tiposRespostas.Count());
                listTiposRespostas.ForEach(c => _context.TiposRespostas.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region TipoPrescricao
            var listTiposPrescricoes = new List<TipoPrescricao>
                {
                    new TipoPrescricao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Adulto",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoPrescricao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Pediátrica",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoPrescricao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Neonatal",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoPrescricao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="Enfermagem",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoPrescricao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="Fisioterapia",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new TipoPrescricao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "6",
                        Descricao="Nutrição",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                };
            var tiposPrescricoes = _context.TiposPrescricoes.ToList();
            if (tiposPrescricoes == null || tiposPrescricoes.Count() < listTiposPrescricoes.Count())
            {
                foreach (var item in tiposPrescricoes)
                {
                    var temp = listTiposPrescricoes.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTiposPrescricoes.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listTiposPrescricoes.ForEach(c => _context.TiposPrescricoes.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region Divisao
            var listDivisoes = new List<Divisao>
                {
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "ACO",
                        Descricao="Acomodação",
                        TipoPrescricaoId=4,
                        Ordem=1,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "MED",
                        Descricao="Medicamento",
                        TipoPrescricaoId=1,
                        Ordem=2,
                        IsDeleted=false,
                        IsDivisaoPrincipal=true,
                        IsSistema=false,
                        CreatorUserId=2,
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "MAT",
                        Descricao="Material médico hospitalar",
                        Ordem=3,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "CUI",
                        Descricao="Cuidados de enfermagem",
                        Ordem=4,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "GAS",
                        Descricao="Gases",
                        Ordem=5,
                        IsDeleted=false,
                        IsDivisaoPrincipal=true,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "EXA",
                        Descricao="Exames",
                        Ordem=6,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "AVA",
                        Descricao="Avaliação especialista",
                        Ordem=7,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "ORI",
                        Descricao="Orientações",
                        Ordem=8,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "SAN",
                        Descricao="Sangue e derivados",
                        Ordem=9,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    },
                    new Divisao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "NUT",
                        Descricao="Tipo de nutrição (dieta)",
                        Ordem=10,
                        IsDeleted=false,
                        IsSistema=false,
                        IsDivisaoPrincipal=true,
                        CreatorUserId=2
                    }
                };
            var divisoes = _context.Divisoes.ToList();
            if (divisoes == null || divisoes.Count() < listDivisoes.Count())
            {
                listDivisoes.RemoveRange(0, divisoes.Count());
                listDivisoes.ForEach(c => _context.Divisoes.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region EstGrupoOperações


            var estoqueGrupoOperacoes = _context.EstoqueGrupoOperacoes.ToList();
            if (estoqueGrupoOperacoes == null || estoqueGrupoOperacoes.Count() == 0)
            {
                // SeedSuprimentos.ReSeedTable<EstoqueGrupoOperacao>(_context);
                SeedSuprimentos.SemAutoIncrementoTable<EstoqueGrupoOperacao>(_context);
                var listEstoqueGrupoOperacoes = new List<EstoqueGrupoOperacao>
                {
                    new EstoqueGrupoOperacao {Id=1, Descricao="Movimentação" },
                    new EstoqueGrupoOperacao {Id=2, Descricao="Solicitação" }
                };

                listEstoqueGrupoOperacoes.ForEach(c => _context.EstoqueGrupoOperacoes.Add(c));
                _context.SaveChanges();


                _context.Database.ExecuteSqlCommand("UPDATE ESTOQUEMOVIMENTO SET EstGrupoOperacaoId = 1 WHERE EstGrupoOperacaoId is null or EstGrupoOperacaoId=0");
                _context.Database.ExecuteSqlCommand("UPDATE ESTOQUEPREMOVIMENTO SET EstGrupoOperacaoId = 1 WHERE EstGrupoOperacaoId is null or EstGrupoOperacaoId=0");
            }

            #endregion

            #region Tipo Documento

            //var tipoDocumentos = _context.TipoDocumentos.ToList();
            //if (tipoDocumentos == null || tipoDocumentos.Count() == 0)
            //{
            //    SeedSuprimentos.ReSeedTable<TipoDocumento>(_context);

            //    var listTipoDocumentos = new List<TipoDocumento>
            //    {
            //        new TipoDocumento {Descricao="Nota Fiscal", IsEntrada=true },
            //        new TipoDocumento {Descricao="Setor", IsEntrada=false },
            //        new TipoDocumento {Descricao="Paciente", IsEntrada=false },
            //        new TipoDocumento {Descricao="Perda", IsEntrada=false },
            //    };

            //    listTipoDocumentos.ForEach(s => _context.TipoDocumentos.Add(s));
            //}

            #endregion

            #region Modulo
            var listModulos = new List<Modulo>
                {
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Das",
                        Descricao="Dashboard",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Ate",
                        Descricao="Atendimento",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Ass",
                        Descricao="Assistencial",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Dia",
                        Descricao="Diagnóstico",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Lab",
                        Descricao="Laboratório",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Sup",
                        Descricao="Suprimentos",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Fat",
                        Descricao="Faturamento",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Fin",
                        Descricao="Financeiro",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Ctr",
                        Descricao="Controladoria",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Apo",
                        Descricao="Apoio",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Cad",
                        Descricao="Cadastros",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Cfg",
                        Descricao="Configurações",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Man",
                        Descricao="Manutenção",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Modulo
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "Adm",
                        Descricao="Administração",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                };
            var modulos = _context.Modulos.ToList();
            if (modulos == null || modulos.Count() < listModulos.Count())
            {
                foreach (var item in modulos)
                {
                    var temp = listModulos.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listModulos.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listModulos.ForEach(c => _context.Modulos.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region Operacao
            var listOperacoes = new List<Operacao>
                {
                    //Atendimento
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Orcamento",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="AgendamentoConsulta",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="AgendamentoExame",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="AgendamentoCirurgia",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="PreAtendimento",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "6",
                        Descricao="ClassificaoRiscoTriagem",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "7",
                        Descricao="AmbulatorioEmergencia",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "8",
                        Descricao="Internacao",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "9",
                        Descricao="Autorizacao",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "10",
                        Descricao="Prorrogacao",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "11",
                        Descricao="HomeCare",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "10",
                        Descricao="RelatorioInternacao",
                        ModuloId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "11",
                        Descricao="EnfermagemAdmissao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "12",
                        Descricao="EnfermagemPassagemPlantao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "13",
                        Descricao="EnfermagemPrescricao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "14",
                        Descricao="EnfermagemSinaisVitais",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "15",
                        Descricao="EnfermagemChecagem",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "16",
                        Descricao="EnfermagemControleBalancoHidrico",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "17",
                        Descricao="MedicoAdmissao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "18",
                        Descricao="MedicoAlta",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "19",
                        Descricao="MedicoAnamnese",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "20",
                        Descricao="MedicoEvolucao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "21",
                        Descricao="MedicoParecerEspecialista",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "22",
                        Descricao="MedicoPrescricao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "24",
                        Descricao="MedicoSolicitacaoExame",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "25",
                        Descricao="MedicoResultadoExame",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "26",
                        Descricao="MedicoResumoAlta",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "27",
                        Descricao="MedicoDescricaoAtoCirurgico",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "28",
                        Descricao="MedicoDescricaoAtoAnestesico",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "29",
                        Descricao="MedicoFolhaGastoCentroCirurgico",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "30",
                        Descricao="MedicoPartograma",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "31",
                        Descricao="AdministrativoCAT",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "32",
                        Descricao="AdministrativoAlergia",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "33",
                        Descricao="AdministrativoDocumentacaoPaciente",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "34",
                        Descricao="AdministrativoConfirmacaoAgendamentoConsulta",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "35",
                        Descricao="AdministrativoConfirmacaoAgendamentoExame",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "36",
                        Descricao="AdministrativoConfirmacaoAgendamentoCirurgia",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "37",
                        Descricao="AdministrativoTransferenciaLeito",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "38",
                        Descricao="AdministrativoTransferenciaMedicoResponsavel",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "39",
                        Descricao="AdministrativoTransferenciaSetor",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "40",
                        Descricao="AdministrativoAlta",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "41",
                        Descricao="AdministrativoAlteracaoAtendimento",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "42",
                        Descricao="AdministrativoPassagemPlantaoEnfermagem",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "43",
                        Descricao="AdministrativoSolicitacaoProrrogacao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "44",
                        Descricao="AdministrativoSolicitacaoProdutoSetor",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "45",
                        Descricao="AdministrativoSolicitacaoProdutoSOS",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "46",
                        Descricao="AdministrativoLiberacaoInterdicaoLeito",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "47",
                        Descricao="DiagnosticoLaboratorial",
                        ModuloId=4,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "47",
                        Descricao="DiagnosticoImagemRegistroExame",
                        ModuloId=4,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "48",
                        Descricao="DiagnosticoImagemLaudos",
                        ModuloId=4,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "49",
                        Descricao="DiagnosticoImagemDicom",
                        ModuloId=4,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "50",
                        Descricao="LaboratorioMetodo",
                        ModuloId=5,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "51",
                        Descricao="LaboratorioUnidade",
                        ModuloId=5,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "52",
                        Descricao="LaboratorioMaterial",
                        ModuloId=5,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "53",
                        Descricao="LaboratorioTecnico",
                        ModuloId=5,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "54",
                        Descricao="LaboratorioFormatacao",
                        ModuloId=5,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "55",
                        Descricao="SuprimentoCompras",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "56",
                        Descricao="SuprimentoEstoqueEntrada",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "57",
                        Descricao="SuprimentoEstoqueSaida",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "58",
                        Descricao="SuprimentoEstoqueTransferencia",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "59",
                        Descricao="SuprimentoEstoqueConfirmacaoMovimento",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "60",
                        Descricao="SuprimentoEstoqueBaixaVale",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "61",
                        Descricao="SuprimentoEstoqueBaixaConsignado",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "62",
                        Descricao="SuprimentoEstoqueDevolucao",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "63",
                        Descricao="SuprimentoEstoqueRelatorioSaldoPorProduto",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "64",
                        Descricao="SuprimentoEstoqueRelatorioMovimentoPorProduto",
                        ModuloId=6,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "65",
                        Descricao="FaturamentoContasMedicas",
                        ModuloId=7,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "66",
                        Descricao="FaturamentoSusInternacao",
                        ModuloId=7,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "67",
                        Descricao="FaturamentoAuditoria",
                        ModuloId=7,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "68",
                        Descricao="FaturamentoRecursoGlosa",
                        ModuloId=7,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "69",
                        Descricao="FaturamentoAutorizacaoGuias",
                        ModuloId=7,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "70",
                        Descricao="FaturamentoRegrasParticularConvenio",
                        ModuloId=7,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "71",
                        Descricao="FinanceiroContasPagar",
                        ModuloId=8,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "72",
                        Descricao="FinanceiroContasReceber",
                        ModuloId=8,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "73",
                        Descricao="FinanceiroControleBancario",
                        ModuloId=8,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "74",
                        Descricao="FinanceiroTesouraria",
                        ModuloId=8,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "75",
                        Descricao="FinanceiroFluxoCaixa",
                        ModuloId=8,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "76",
                        Descricao="FinanceiroRepasseMedico",
                        ModuloId=8,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "77",
                        Descricao="ControladoriaOrcamentos",
                        ModuloId=9,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "78",
                        Descricao="ControladoriaPatrimonios",
                        ModuloId=9,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "79",
                        Descricao="ControladoriaPatrimonios",
                        ModuloId=9,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "80",
                        Descricao="ControladoriaContabilidade",
                        ModuloId=9,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "81",
                        Descricao="ControladoriaCustos",
                        ModuloId=9,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "82",
                        Descricao="ControladoriaNotasFiscais",
                        ModuloId=9,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "83",
                        Descricao="ApoioNutricao",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "84",
                        Descricao="ApoioCentralMaterial",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "85",
                        Descricao="ApoioEsterilizados",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "86",
                        Descricao="ApoioManutencao",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "87",
                        Descricao="ApoioHigienizacao",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "88",
                        Descricao="ApoioPortariaControleAcesso",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "89",
                        Descricao="ApoioLavanderiaRouparia",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "90",
                        Descricao="ApoioSAC",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "91",
                        Descricao="ApoioSame",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "92",
                        Descricao="ApoioControleInfeccao",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "93",
                        Descricao="ApoioHospitalar",
                        ModuloId=10,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "94",
                        Descricao="CadastroPaciente",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "95",
                        Descricao="CadastroMedico",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "96",
                        Descricao="CadastroEspecialidade",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "97",
                        Descricao="CadastroProfissao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "98",
                        Descricao="CadastroOrigem",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "99",
                        Descricao="CadastroNaturalidade",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "100",
                        Descricao="CadastroNacionalidade",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "101",
                        Descricao="CadastroConvenio",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "102",
                        Descricao="CadastroPlano",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "103",
                        Descricao="CadastroTipoLogradouro",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "104",
                        Descricao="CadastroPais",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "105",
                        Descricao="CadastroEstado",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "106",
                        Descricao="CadastroCidade",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "107",
                        Descricao="CadastroCep",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "108",
                        Descricao="CadastroGrupoCentroCusto",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "109",
                        Descricao="CadastroIntervalo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "110",
                        Descricao="CadastroTipoAcomodacao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "111",
                        Descricao="CadastroFornecedor",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "112",
                        Descricao="CadastroAcaoTerapeutica",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "113",
                        Descricao="CadastroCentroCusto",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "114",
                        Descricao="CadastroGrauInstrucao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "115",
                        Descricao="CadastroFeriado",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "116",
                        Descricao="CadastroTipoParticipacao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "117",
                        Descricao="CadastroTipoVinculoEmpregaticio",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "118",
                        Descricao="CadastroGrupoCID",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "119",
                        Descricao="CadastroParentesco",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "120",
                        Descricao="CadastroIndicacao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "121",
                        Descricao="CadastroIntervalo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "122",
                        Descricao="CadastroTipoSanguineo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "123",
                        Descricao="CadastroElementoHtml",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "124",
                        Descricao="CadastroTipoElementoHtml",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "125",
                        Descricao="CadastroTissTipoTabela",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "126",
                        Descricao="CadastroTissTipoTabelaGrupo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "127",
                        Descricao="CadastroTiss",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "128",
                        Descricao="CadastroTissTabelaItem",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "129",
                        Descricao="CadastroTissVersao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "130",
                        Descricao="CadastroTipoAtendimento",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "131",
                        Descricao="CadastroConsultaMedicoDisponibilidade",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "132",
                        Descricao="CadastroMotivoAlta",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "133",
                        Descricao="CadastroLeito",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "134",
                        Descricao="CadastroLeitoStatus",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "135",
                        Descricao="CadastroLeitoCaracteristicas",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "136",
                        Descricao="CadastroLeitoServico",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "137",
                        Descricao="CadastroUniddeInternacao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "138",
                        Descricao="CadastroUnidadeInternacaoTipo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "139",
                        Descricao="CadastroAtestado",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "140",
                        Descricao="CadastroAtestadoTipo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "141",
                        Descricao="CadastroAtestadoModelo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "142",
                        Descricao="CadastroDivisao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "143",
                        Descricao="CadastroTipoResposta",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "144",
                        Descricao="CadastroTipoRespostaConfiguracao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "145",
                        Descricao="CadastroFaturamentoItem",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "145",
                        Descricao="CadastroFaturamentoKit",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "146",
                        Descricao="CadastroTabelaPreco",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "147",
                        Descricao="CadastroTabelaPrecoConvenio",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "148",
                        Descricao="CadastroFaturamentoGrupo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "149",
                        Descricao="CadastroBrasindice",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "150",
                        Descricao="CadastroBrasindiceItem",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "151",
                        Descricao="CadastroBrasindiceApresentacao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "152",
                        Descricao="CadastroMoeda",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "153",
                        Descricao="CadastroProduto",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "154",
                        Descricao="CadastroProduto",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "155",
                        Descricao="CadastroPalavraChave",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "156",
                        Descricao="CadastroAcaoTerapeutica",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "157",
                        Descricao="CadastroGrupo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "158",
                        Descricao="CadastroPortaria",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "159",
                        Descricao="CadastroGrupoTratamento",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "160",
                        Descricao="CadastroProdutoLocalizacao",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "161",
                        Descricao="CadastroUnidade",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "162",
                        Descricao="CadastroUnidadeTipo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "163",
                        Descricao="CadastroCodigoMedicamento",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "164",
                        Descricao="CadastroEstoque",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "165",
                        Descricao="CadastroSubstancia",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "166",
                        Descricao="CadastroEntradaTipo",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "167",
                        Descricao="CadastroDocumento",
                        ModuloId=11,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "168",
                        Descricao="ConfiguracaoEmpresa",
                        ModuloId=12,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "169",
                        Descricao="ConfiguracaoGeradorFormulario",
                        ModuloId=12,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "170",
                        Descricao="ConfiguracaoGeradorRelatorio",
                        ModuloId=12,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "171",
                        Descricao="ManutencaoConsultorTabela",
                        ModuloId=13,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "172",
                        Descricao="ManutencaoCamposTabelaSistema",
                        ModuloId=13,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "173",
                        Descricao="ManutencaoModeloEmail",
                        ModuloId=13,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "174",
                        Descricao="ManutencaoGuia",
                        ModuloId=13,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "175",
                        Descricao="AdministracaoUnidadeOrganizacional",
                        ModuloId=14,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "176",
                        Descricao="AdministracaoGrupoUsuario",
                        ModuloId=14,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "177",
                        Descricao="AdministracaoUsuario",
                        ModuloId=14,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "178",
                        Descricao="AdministracaoLinguagens",
                        ModuloId=14,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "179",
                        Descricao="AdministracaoLogAuditoria",
                        ModuloId=14,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "180",
                        Descricao="AdministracaoConfiguracao",
                        ModuloId=14,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "186",
                        Descricao="EnfermagemEvolucao",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=true
                    },
                    new Operacao
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "187",
                        Descricao="MedicoReceituario",
                        ModuloId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2,
                        IsFormulario=false,
                        Name = SW10.SWMANAGER.Authorization.AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Receituario
                    },
                };
            var operacoes = _context.Operacoes.ToList();
            if (operacoes == null || operacoes.Count() < listOperacoes.Count())
            {
                foreach (var item in operacoes)
                {
                    var temp = listOperacoes.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listOperacoes.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listOperacoes.ForEach(c => _context.Operacoes.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region EstoquePreMovimentoEstado


            //SeedSuprimentos.SemAutoIncrementoTable<EstoquePreMovimentoEstado>(_context);

            var listPreMovimentoEstado = new List<EstoquePreMovimentoEstado>
            {
                new EstoquePreMovimentoEstado { Id=1, Descricao="Aguardando Confirmação", IsDeleted=false, IsSistema=true, CreatorUserId=2 },
                new EstoquePreMovimentoEstado {Id=2, Descricao="Confirmado", IsDeleted=false, IsSistema=true, CreatorUserId=2 },
                new EstoquePreMovimentoEstado {Id=3, Descricao="Pendente informação de lote/validade", IsDeleted=false, IsSistema=true, CreatorUserId=2 },
                new EstoquePreMovimentoEstado {Id=4, Descricao="Pendente", IsDeleted=false, IsSistema=true, CreatorUserId=2 },
                new EstoquePreMovimentoEstado {Id=5, Descricao="Parcialmente atendido", IsDeleted=false, IsSistema=true, CreatorUserId=2 },
                new EstoquePreMovimentoEstado {Id=6,Descricao="Totalmente atendido", IsDeleted=false, IsSistema=true, CreatorUserId=2 },
                new EstoquePreMovimentoEstado {Id=7, Descricao="Parcialmento suspenso", IsDeleted=false, IsSistema=true },
                new EstoquePreMovimentoEstado {Id=8, Descricao="Totalmente suspenso", IsDeleted=false, IsSistema=true },
                new EstoquePreMovimentoEstado {Id=9, Descricao="Emprestado", IsDeleted=false, IsSistema=true },
                new EstoquePreMovimentoEstado {Id=10, Descricao="Parcialmente Devolvido", IsDeleted=false, IsSistema=true },
                new EstoquePreMovimentoEstado {Id=11, Descricao="Totalmente Devolvido", IsDeleted=false, IsSistema=true },
            };


            var preMovimentosEstados = _context.EstoquePreMovimentoEstado.ToList();


            if (preMovimentosEstados == null || preMovimentosEstados.Count() < listPreMovimentoEstado.Count())
            {
                var ultimoId = preMovimentosEstados.Count() > 0 ? preMovimentosEstados.Max(m => m.Id) : 1;
                SeedSuprimentos.ReSeedTable<EstoquePreMovimentoEstado>(_context, ultimoId);

                foreach (var item in preMovimentosEstados)
                {
                    var temp = listPreMovimentoEstado.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listPreMovimentoEstado.Remove(temp);
                    }
                }

                listPreMovimentoEstado.ForEach(c => _context.EstoquePreMovimentoEstado.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region TipoFrete

            SeedSuprimentos.SemAutoIncrementoTable<TipoFrete>(_context);

            var listTipoFrete = new List<TipoFrete>
            {
                new TipoFrete { Id=0, Codigo ="CIF", Descricao="Contratação do Frete por conta do Remetente" },
                new TipoFrete { Id=1, Codigo ="FOB", Descricao="Contratação do Frete por conta do destinatário/remetente" },
                new TipoFrete { Id=2 , Descricao="Contratação do Frete por conta de terceiros" },
                new TipoFrete { Id=3 , Descricao="Transporte próprio por conta do remetente" },
                new TipoFrete { Id=4 , Descricao="Transporte próprio por conta do destinatário" },
                new TipoFrete { Id=9 , Descricao="Sem Ocorrência de transporte." },
            };

            var tipoFretes = _context.TipoFrete.ToList();

            if (tipoFretes == null || tipoFretes.Count() < listTipoFrete.Count())
            {
                _context.Database.ExecuteSqlCommand(@"ALTER TABLE [dbo].[EstoqueMovimento] DROP CONSTRAINT [FK_dbo.EstoqueMovimento_dbo.TipoFrete_TipoFreteId];
                    ALTER TABLE [dbo].[EstoquePreMovimento] DROP CONSTRAINT [FK_dbo.EstoquePreMovimento_dbo.TipoFrete_TipoFreteId];
                    SET IDENTITY_INSERT TipoFrete ON;

                    delete from TipoFrete;
                    INSERT INTO TipoFrete(id,Descricao,IsSistema,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Codigo,ImportaId)
                    values(0,'Contratação do Frete por conta do Remetente',0,0,null,null,null,null,GETDATE(),null,'CIF',null)

                    INSERT INTO TipoFrete(id,Descricao,IsSistema,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Codigo,ImportaId)
                    values(1,'Contratação do Frete por conta do destinatário/remetente',0,0,null,null,null,null,GETDATE(),null,'FOB',null)

                    INSERT INTO TipoFrete(id,Descricao,IsSistema,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Codigo,ImportaId)
                    values(2,'Contratação do Frete por conta de terceiros',0,0,null,null,null,null,GETDATE(),null,null,null)

                    INSERT INTO TipoFrete(id,Descricao,IsSistema,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Codigo,ImportaId)
                    values(3,'Transporte próprio por conta do remetente',0,0,null,null,null,null,GETDATE(),null,null,null)

                    INSERT INTO TipoFrete(id,Descricao,IsSistema,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Codigo,ImportaId)
                    values(4,'Transporte próprio por conta do destinatário',0,0,null,null,null,null,GETDATE(),null,null,null)

                    INSERT INTO TipoFrete(id,Descricao,IsSistema,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Codigo,ImportaId)
                    values(9,'Sem Ocorrência de transporte.',0,0,null,null,null,null,GETDATE(),null,null,null)

                    SET IDENTITY_INSERT TipoFrete OFF;

                    ALTER TABLE [dbo].[EstoqueMovimento]  WITH CHECK ADD  CONSTRAINT [FK_dbo.EstoqueMovimento_dbo.TipoFrete_TipoFreteId] FOREIGN KEY([TipoFreteId])
                    REFERENCES [dbo].[TipoFrete] ([Id]);

                    ALTER TABLE [dbo].[EstoqueMovimento] CHECK CONSTRAINT [FK_dbo.EstoqueMovimento_dbo.TipoFrete_TipoFreteId];

                    ALTER TABLE [dbo].[EstoquePreMovimento]  WITH CHECK ADD  CONSTRAINT [FK_dbo.EstoquePreMovimento_dbo.TipoFrete_TipoFreteId] FOREIGN KEY([TipoFreteId])
                    REFERENCES [dbo].[TipoFrete] ([Id]);

                    ALTER TABLE [dbo].[EstoquePreMovimento] CHECK CONSTRAINT [FK_dbo.EstoquePreMovimento_dbo.TipoFrete_TipoFreteId];");
            }

            #endregion

            #region AteStatusSolicitacoes

            var statusSolicitacoes = _context.StatusSolicitacoesProcedimentos.ToList();
            if (statusSolicitacoes == null || statusSolicitacoes.Count() == 0)
            {
                // SeedSuprimentos.ReSeedTable<EstoqueGrupoOperacao>(_context);
                SeedSuprimentos.SemAutoIncrementoTable<StatusSolicitacaoProcedimento>(_context);
                var listStatusSolicitacoesProcedimentos = new List<StatusSolicitacaoProcedimento>
                {
                    new StatusSolicitacaoProcedimento {Id=1, Descricao="Autorizado", Codigo = "1" },
                    new StatusSolicitacaoProcedimento {Id=2, Descricao="Em análise", Codigo = "2"  },
                    new StatusSolicitacaoProcedimento {Id=3, Descricao="Negado" , Codigo = "3" },
                    new StatusSolicitacaoProcedimento {Id=4, Descricao="Aguardando justificativa técnica do solicitante", Codigo = "4"  },
                    new StatusSolicitacaoProcedimento {Id=5, Descricao="Aguardando documentacao do prestador" , Codigo = "5" },
                    new StatusSolicitacaoProcedimento {Id=6, Descricao="Solicitação cancelada", Codigo = "6"  },
                    new StatusSolicitacaoProcedimento {Id=7, Descricao="Autorizado parcialmente", Codigo = "7"  }
                };

                listStatusSolicitacoesProcedimentos.ForEach(c => _context.StatusSolicitacoesProcedimentos.Add(c));
                _context.SaveChanges();


            }

            #endregion

            #region TipoMeioPagamento



            var listTiposMeioPagamentos = new List<TipoMeioPagamento>
                {
                    new TipoMeioPagamento {Id=1, Descricao="Dinheiro", Codigo = "01" },
                    new TipoMeioPagamento {Id=2, Descricao="Cheque", Codigo = "02"  },
                    new TipoMeioPagamento {Id=3, Descricao="Cartão de crédito" , Codigo = "03" },
                    new TipoMeioPagamento {Id=4, Descricao="Cartão de débito", Codigo = "04"  },
                    new TipoMeioPagamento {Id=5, Descricao="Crédito loja" , Codigo = "05" },
                    new TipoMeioPagamento {Id=6, Descricao="Outros", Codigo = "99"  },
                };

            var tiposMeiosPagametos = _context.TiposMeioPagamentos.ToList();
            if (tiposMeiosPagametos == null || tiposMeiosPagametos.Count() < listTiposMeioPagamentos.Count())
            {
                foreach (var item in tiposMeiosPagametos)
                {
                    var temp = listTiposMeioPagamentos.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTiposMeioPagamentos.Remove(temp);
                    }
                }

                SeedSuprimentos.SemAutoIncrementoTable<TipoMeioPagamento>(_context);
                listTiposMeioPagamentos.ForEach(c => _context.TiposMeioPagamentos.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region CboTipo
            var cboTipos = _context.CboTipos.ToList();
            if (cboTipos == null || cboTipos.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<CboTipo>(_context);

                _context.SaveChanges();

                var listCboTipo = new List<CboTipo>
                {
                    new CboTipo
                    {
                        Descricao="OCUPACAO",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CboTipo
                    {
                        Descricao="SINONIMO",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listCboTipo.ForEach(s => _context.CboTipos.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region TipoMeioPagamento



            var listSituacaoLancamento = new List<SituacaoLancamento>
                {
                    new SituacaoLancamento {Id=10, Descricao="Aberto", Codigo = "01" },
                    new SituacaoLancamento {Id=20, Descricao="Cancelado", Codigo = "02"  },
                    new SituacaoLancamento {Id=30, Descricao="Parcialmente Quitado" , Codigo = "03" },
                    new SituacaoLancamento {Id=40, Descricao="Quitado", Codigo = "04"  },
                };

            var situacoesPagamentos = _context.SituacoesLancamentos.ToList();
            if (situacoesPagamentos == null || situacoesPagamentos.Count() < listSituacaoLancamento.Count())
            {
                foreach (var item in situacoesPagamentos)
                {
                    var temp = listSituacaoLancamento.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listSituacaoLancamento.Remove(temp);
                    }
                }

                SeedSuprimentos.SemAutoIncrementoTable<SituacaoLancamento>(_context);
                listSituacaoLancamento.ForEach(c => _context.SituacoesLancamentos.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region FatContaStatus
            var listFatContaStatus = new List<FaturamentoContaStatus>
            {
                new FaturamentoContaStatus { Descricao="Inicial"  , Codigo = FaturamentoContaStatus.Inicial.ToString(), Cor = "#f4eb42" },
                new FaturamentoContaStatus { Descricao="Conferido", Codigo = FaturamentoContaStatus.Conferido.ToString(), Cor = "#204dd6" },
                new FaturamentoContaStatus { Descricao="Entregue" , Codigo = FaturamentoContaStatus.Entregue.ToString(), Cor = "#61d3e8" },
                new FaturamentoContaStatus { Descricao="Lote"     , Codigo = FaturamentoContaStatus.Lote.ToString(), Cor = "#45c645" },
                new FaturamentoContaStatus { Descricao="Pendente" , Codigo = FaturamentoContaStatus.Pendente.ToString(), Cor = "#c92626" },
                new FaturamentoContaStatus { Descricao="Auditoria Interna" , Codigo = FaturamentoContaStatus.AuditoriaInterna.ToString(), Cor = "#c92626" },
                new FaturamentoContaStatus { Descricao="Auditoria Externa" , Codigo = FaturamentoContaStatus.AuditoriaExterna.ToString(), Cor = "#c92626" },
            };
            var fatContaStatus = _context.FaturamentoContaStatus.ToList();
            if (fatContaStatus == null || fatContaStatus.Count() < listFatContaStatus.Count())
            {
                foreach (var item in fatContaStatus)
                {
                    var temp = listFatContaStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listFatContaStatus.Remove(temp);
                    }
                }

                listFatContaStatus.ForEach(c => _context.FaturamentoContaStatus.Add(c));
                _context.SaveChanges();
            }

            #endregion _fatContaStatus_

            #region FaturamentoValoresHonorario
            var faturamentoValoresHonorarios = _context.FaturamentoValoresHonorarios.ToList();

            if (faturamentoValoresHonorarios.IsNullOrEmpty())
            {
                var faturamentoValoresHonorario = new FaturamentoValoresHonorario()
                {
                    PercentualMedico = 1,
                    PercentualAuxiliar1 = 0.3F,
                    PercentualAuxiliar2 = 0.2F,
                    PercentualAuxiliar3 = 0.1F,
                    PercentualInstrumentador = 0.1F,
                    PercentualAnestesista = 0.1F
                };

                _context.FaturamentoValoresHonorarios.Add(faturamentoValoresHonorario);
                _context.SaveChanges();
            }

            #endregion

            #region PrescricaoStatus
            var listPrescricoesStatus = new List<PrescricaoStatus>
                {
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Inicial",
                        Cor="#FFFF00",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Liberada",
                        Cor="#0000FF",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Aprazada",
                        Cor = "#008000",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="Cancelada",
                        Cor = "#000000",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="Suspensa",
                        Cor = "#FF0000",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "6",
                        Descricao="Aprovada",
                        Cor = "#008B8B",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "7",
                        Descricao="Liberada com Acréscimos",
                        Cor = "#00ddff",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "8",
                        Descricao="Aprovada com Acréscimos",
                        Cor = "#00d8d8",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                };
            var prescricoesStatus = _context.PrescricoesStatus.ToList();
            if (prescricoesStatus == null || prescricoesStatus.Count() == 0 || prescricoesStatus.Count() < listPrescricoesStatus.Count())
            {
                foreach (var item in prescricoesStatus)
                {
                    var temp = listPrescricoesStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listPrescricoesStatus.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listPrescricoesStatus.ForEach(c => _context.PrescricoesStatus.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region PrescricaoItemStatus
            var listPrescricoesItensStatus = new List<PrescricaoItemStatus>
                {
                    new PrescricaoItemStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Ativo",
                        Cor="#008000",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoItemStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Suspenso",
                        Cor="#FF0000",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                    new PrescricaoItemStatus
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Acréscimo",
                        Cor = "#FFFF00",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                };
            var prescricoesItensStatus = _context.PrescricoesItensStatus.ToList();
            if (prescricoesItensStatus == null || prescricoesItensStatus.Count() == 0 || prescricoesItensStatus.Count() < listPrescricoesItensStatus.Count())
            {
                foreach (var item in prescricoesItensStatus)
                {
                    var temp = listPrescricoesItensStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listPrescricoesItensStatus.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listPrescricoesItensStatus.ForEach(c => _context.PrescricoesItensStatus.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region Tipos SolicitaçãoMicrobianos Indicação
            var tipoSolicitacaoAntimicrobianosIndicacoes = _context.TipoSolicitacaoAntimicrobianosIndicacoes.ToList();
            if (tipoSolicitacaoAntimicrobianosIndicacoes == null || tipoSolicitacaoAntimicrobianosIndicacoes.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<TipoSolicitacaoAntimicrobianosCultura>(_context, 1);

                _context.SaveChanges();

                var listTipoSolicitacaoAntimicrobianosIndicacao = new List<TipoSolicitacaoAntimicrobianosIndicacao>
                {
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 1,
                        Descricao="PNM",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 2,
                        Descricao="PAV",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 3,
                        Descricao="Infecção de corrente sanguínea + cateter central",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 4,
                        Descricao="ITU + SVD",
                        Codigo="4",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 5,
                        Descricao="ITU Não-SVD",
                        Codigo="5",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 6,
                        Descricao="Sítio Cirúrgico",
                        Codigo="6",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 7,
                        Descricao="Neutropenia Febril",
                        Codigo="7",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 8,
                        Descricao="Cutãnea",
                        Codigo="8",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 9,
                        Descricao="Sepse Clínica",
                        Codigo="9",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosIndicacao
                    {
                        Id = 10,
                        Descricao="Profilaxia Cirurgica",
                        Codigo="10",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listTipoSolicitacaoAntimicrobianosIndicacao.ForEach(s => _context.TipoSolicitacaoAntimicrobianosIndicacoes.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region Tipos SolicitaçãoMicrobianos Resultado
            var tipoSolicitacaoAntimicrobianosResultado = _context.TipoSolicitacaoAntimicrobianosResultados.ToList();
            if (tipoSolicitacaoAntimicrobianosResultado == null || tipoSolicitacaoAntimicrobianosResultado.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<TipoSolicitacaoAntimicrobianosCultura>(_context, 1);

                _context.SaveChanges();

                var listTipoSolicitacaoAntimicrobianosResultado = new List<TipoSolicitacaoAntimicrobianosResultado>
                {
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 1,
                        Descricao="S. aureus",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 2,
                        Descricao="MRSA",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 3,
                        Descricao="PNSSP",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 4,
                        Descricao="ESBL",
                        Codigo="4",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 5,
                        Descricao="E. faecium",
                        Codigo="5",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 6,
                        Descricao="E. faecalis",
                        Codigo="6",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 7,
                        Descricao="P. aeroginosa",
                        Codigo="7",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 8,
                        Descricao="S. epidermidis",
                        Codigo="8",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 9,
                        Descricao="S. marcescens",
                        Codigo="9",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 10,
                        Descricao="KPC",
                        Codigo="10",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 11,
                        Descricao="P. stuartti",
                        Codigo="11",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 12,
                        Descricao="C. topicalis",
                        Codigo="12",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 13,
                        Descricao="P. mirabilis",
                        Codigo="13",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 14,
                        Descricao="E. coli",
                        Codigo="14",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosResultado
                    {
                        Id = 15,
                        Descricao="A.baumanii",
                        Codigo="15",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listTipoSolicitacaoAntimicrobianosResultado.ForEach(s => _context.TipoSolicitacaoAntimicrobianosResultados.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region Tipos SolicitaçãoMicrobianos Culturas
            var tipoSolicitacaoAntimicrobianosCulturas = _context.TipoSolicitacaoAntimicrobianosCulturas.ToList();
            if (tipoSolicitacaoAntimicrobianosCulturas == null || tipoSolicitacaoAntimicrobianosCulturas.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<TipoSolicitacaoAntimicrobianosCultura>(_context, 1);

                _context.SaveChanges();

                var listTipoSolicitacaoAntimicrobianosCulturas = new List<TipoSolicitacaoAntimicrobianosCultura>
                {
                    new TipoSolicitacaoAntimicrobianosCultura
                    {
                        Id = 1,
                        Descricao="URINOCULTURA",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosCultura
                    {
                        Id = 2,
                        Descricao="HEMOCULTURA",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new TipoSolicitacaoAntimicrobianosCultura
                    {
                        Id = 3,
                        Descricao="SECREÇÂO TRAQUEAL",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },

                };
                listTipoSolicitacaoAntimicrobianosCulturas.ForEach(s => _context.TipoSolicitacaoAntimicrobianosCulturas.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region  Configuracao Prescricao Item Campos

            var configuracaoPrescricaoItemCampos = _context.ConfiguracaoPrescricaoItemCampos.ToList();
            if (configuracaoPrescricaoItemCampos == null || configuracaoPrescricaoItemCampos.Count() == 0)
            {
                SeedAssistenciais.ReSeedTable<ConfiguracaoPrescricaoItemCampo>(_context, 1);

                _context.SaveChanges();
                var listConfiguracaoPrescricaoItemCampos = typeof(ConfiguracaoPrescricaoItemCampo).GetFields(BindingFlags.Static | BindingFlags.Public).ToList()
                    .Select(x => new ConfiguracaoPrescricaoItemCampo()
                    {
                        Id = (long)x.GetValue(null),
                        Descricao = x.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description ?? "",
                        CreationTime = DateTime.Now,
                        Codigo = x.GetValue(null).ToString(),
                        CreatorUserId = 2,
                        IsDeleted = false,
                        IsSistema = true
                    }).OrderBy(x => x.Id).ToList();

                listConfiguracaoPrescricaoItemCampos.ForEach(s => _context.ConfiguracaoPrescricaoItemCampos.Add(s));
                _context.SaveChanges();
            }


            #endregion

            #region MotivoPedido
            var motivosPedido = _context.CompraMotivoPedido.ToList();
            if (motivosPedido == null || motivosPedido.Count() == 0)
            {
                var listMotivosPedido = new List<CompraMotivoPedido>
                {
                    new CompraMotivoPedido
                    {
                        Descricao="Reposição de Estoque",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraMotivoPedido
                    {
                        Descricao="Aumento de Consumo",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraMotivoPedido
                    {
                        Descricao="Setor",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraMotivoPedido
                    {
                        Descricao="Paciente",
                        Codigo="4",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listMotivosPedido.ForEach(s => _context.CompraMotivoPedido.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region SisPessoa
            var sisTipoPessoa = _context.SisTiposPessoa.ToList();
            if (sisTipoPessoa == null || sisTipoPessoa.Count() == 0)
            {
                var listSisTipoPessoa = new List<SisTipoPessoa>
                {
                    new SisTipoPessoa
                    {
                       IsPagar = true,
                       IsReceber = false
                    },

                     new SisTipoPessoa
                    {
                       IsPagar = false,
                       IsReceber = true
                    },

                };
                listSisTipoPessoa.ForEach(s => _context.SisTiposPessoa.Add(s));
                _context.SaveChanges();
            }
            #endregion _SisPessoa_

            #region FormularioDinamico_CamposReservados

            var formConfigs = _context.FormsConfig.ToList();
            FormConfig reservado = null;
            reservado = formConfigs?.FirstOrDefault(x => x.Descricao == "RESERVADO");

            if (reservado == null)
            {
                // SeedSuprimentos.ReSeedTable<EstoqueGrupoOperacao>(_context);
                //SeedSuprimentos.SemAutoIncrementoTable<StatusSolicitacaoProcedimento>(_context);
                var fc = new FormConfig();
                fc.Descricao = "RESERVADO";
                fc.Codigo = "RESERVADO";
                fc.DataAlteracao = DateTime.Now;
                fc.LastModificationTime = DateTime.Now;
                fc.CreationTime = DateTime.Now;
                fc.DeletionTime = null;
                fc.Nome = "Campos reservados";

                var listFormConfigs = new List<FormConfig>
                {
                    fc
                };

                listFormConfigs.ForEach(c => _context.FormsConfig.Add(c));
                _context.SaveChanges();
            }

            #endregion _FormularioDinamico_CamposReservados_

            #region CompraTipoRequisicao
            var compraTipoRequisicao = _context.CompraTipoRequisicao.ToList();
            if (compraTipoRequisicao == null || compraTipoRequisicao.Count() == 0)
            {
                var listCompraTipoRequisicao = new List<CompraRequisicaoTipo>
                {
                    new CompraRequisicaoTipo
                    {
                        Descricao="Produto",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraRequisicaoTipo
                    {
                        Descricao="Serviço",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listCompraTipoRequisicao.ForEach(s => _context.CompraTipoRequisicao.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region CompraRequisicaoModo
            var compraRequisicaoModo = _context.CompraRequisicaoModo.ToList();
            if (compraRequisicaoModo == null || compraRequisicaoModo.Count() == 0)
            {
                var listCompraRequisicaoModo = new List<CompraRequisicaoModo>
                {
                    new CompraRequisicaoModo
                    {
                        Descricao= "Manual",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraRequisicaoModo
                    {
                        Descricao= "Automático",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listCompraRequisicaoModo.ForEach(s => _context.CompraRequisicaoModo.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region CompraCotacaoStatus
            var compraCotacaoStatus = _context.CompraCotacaoStatus.ToList();
            if (compraCotacaoStatus == null || compraCotacaoStatus.Count() == 0)
            {
                var listCompraCotacaoStatus = new List<CompraCotacaoStatus>
                {
                    new CompraCotacaoStatus
                    {
                        Descricao= "Aguardando Envio",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraCotacaoStatus
                    {
                        Descricao= "Aguardando Resposta",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraCotacaoStatus
                    {
                        Descricao= "Respondida",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listCompraCotacaoStatus.ForEach(s => _context.CompraCotacaoStatus.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region CompraAprovacaoStatus
            var compraAprovacaoStatus = _context.CompraAprovacaoStatus.ToList();
            if (compraAprovacaoStatus == null || compraAprovacaoStatus.Count() == 0)
            {
                var listCompraAprovacaoStatus = new List<CompraAprovacaoStatus>
                {
                    new CompraAprovacaoStatus
                    {
                        Descricao= "Aguardando Aprovação",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraAprovacaoStatus
                    {
                        Descricao= "Aprovada",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    },
                    new CompraAprovacaoStatus
                    {
                        Descricao= "Recusada",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true
                    }
                };
                listCompraAprovacaoStatus.ForEach(s => _context.CompraAprovacaoStatus.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region FaturamentoGuia
            var faturamentoGuia = _context.FaturamentoGuias.ToList();
            if (faturamentoGuia == null || faturamentoGuia.Count() == 0)
            {
                var listFaturamentoGuia = new List<FaturamentoGuia>
                {
                    new FaturamentoGuia
                    {
                        Descricao= "GUIA DE CONSULTA",
                        Codigo="1",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true,
                        IsAmbulatorio = true,
                        IsInternacao = false

                    },
                    new FaturamentoGuia
                    {
                        Descricao= "GUIA DE SP/SADT",
                        Codigo="2",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true,
                        IsAmbulatorio = true,
                        IsInternacao = true
                     },
                    new FaturamentoGuia
                    {
                        Descricao= "GUIA DE RESUMO DE INTERNAÇÃO",
                        Codigo="3",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true,
                        IsAmbulatorio = false,
                        IsInternacao = true
                     },
                    new FaturamentoGuia
                    {
                        Descricao= "GUIA DE HONORÁRIO INDIVIDUAL",
                        Codigo="4",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true,
                        IsAmbulatorio = false,
                        IsInternacao = true
                     },
                    new FaturamentoGuia
                    {
                        Descricao= "GUIA DE PARTICULAR",
                        Codigo="5",
                        CreationTime=DateTime.Now,
                        CreatorUserId=2,
                        IsDeleted=false,
                        IsSistema=true,
                        IsAmbulatorio = false,
                        IsInternacao = true
                    }
                };

                listFaturamentoGuia.ForEach(s => _context.FaturamentoGuias.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region StatusSolicitacaoExameItem
            var listStatusSolicitacaoExameItem = new List<SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos.StatusSolicitacaoExameItem>
                {
                    new SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos.StatusSolicitacaoExameItem
                    {
                        Id=1,
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Inicial",
                        CorStatus="#e9f50e",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos.StatusSolicitacaoExameItem
                    {
                        Id=2,
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Cancelado",
                        CorStatus="#f50a11",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },
                     new SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos.StatusSolicitacaoExameItem
                    {
                        Id=3,
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Liberado",
                        CorStatus="#19f50a",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                      new SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos.StatusSolicitacaoExameItem
                    {
                        Id=4,
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="Registrado",
                        CorStatus="#0ae9f5",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                        new SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos.StatusSolicitacaoExameItem
                    {
                        Id=5,
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="Realizado",
                        CorStatus="#eb710e",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                };
            var statusSolicitacaoExameItens = _context.StatusSolicitacoesExameItens.ToList();
            if (statusSolicitacaoExameItens == null || statusSolicitacaoExameItens.Count() == 0 || statusSolicitacaoExameItens.Count() < listStatusSolicitacaoExameItem.Count())
            {
                foreach (var item in statusSolicitacaoExameItens)
                {
                    var temp = listStatusSolicitacaoExameItem.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listStatusSolicitacaoExameItem.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listStatusSolicitacaoExameItem.ForEach(c => _context.StatusSolicitacoesExameItens.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region LabTipoResultado

            var listLabTipoResultado = new List<TipoResultado>
                {
                    new TipoResultado
                    {
                        Id=1,
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="Numerico",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new TipoResultado
                    {
                        Id=2,
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="Alfanumerico",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new TipoResultado
                    {
                        Id=3,
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="Calculado",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new TipoResultado
                    {
                        Id=4,
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="Tabela",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new TipoResultado
                    {
                        Id=5,
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="Memo",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },

                    new TipoResultado
                    {
                        Id=6,
                        CreationTime=DateTime.Now,
                        Codigo = "6",
                        Descricao="Gráfico",
                        IsDeleted=false,
                        IsSistema=false,
                        CreatorUserId=2
                    },



                };
            var labTipoResultados = _context.TiposResultados.ToList();
            if (labTipoResultados == null || labTipoResultados.Count() == 0 || labTipoResultados.Count() < listLabTipoResultado.Count())
            {
                foreach (var item in labTipoResultados)
                {
                    var temp = listLabTipoResultado.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listLabTipoResultado.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listLabTipoResultado.ForEach(c => _context.TiposResultados.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region HorasDia

            var listHorasDia = new List<HoraDia>
                {
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "1",
                        Descricao="00",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "2",
                        Descricao="01",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "3",
                        Descricao="02",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "4",
                        Descricao="03",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "5",
                        Descricao="04",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "6",
                        Descricao="05",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "7",
                        Descricao="06",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "8",
                        Descricao="07",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "9",
                        Descricao="08",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "10",
                        Descricao="09",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "11",
                        Descricao="10",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "12",
                        Descricao="11",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "13",
                        Descricao="12",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "14",
                        Descricao="13",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "15",
                        Descricao="14",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "16",
                        Descricao="15",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "17",
                        Descricao="16",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "18",
                        Descricao="17",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "19",
                        Descricao="18",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "20",
                        Descricao="19",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "21",
                        Descricao="20",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "22",
                        Descricao="21",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "23",
                        Descricao="22",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new HoraDia
                    {
                        CreationTime=DateTime.Now,
                        Codigo = "24",
                        Descricao="23",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                };

            var horasDia = _context.HorasDia.ToList();
            if (horasDia == null || horasDia.Count() == 0 || horasDia.Count() < listHorasDia.Count())
            {
                foreach (var item in horasDia)
                {
                    var temp = listHorasDia.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listHorasDia.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listHorasDia.ForEach(c => _context.HorasDia.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region TabelaRegistro

            var listRegistroTabela = new List<RegistroTabela>
                {
                    new RegistroTabela
                    {
                        Id=1,
                        Codigo = "LAB",
                        Descricao="Laboratório",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new RegistroTabela
                    {
                        Id=2,
                        Codigo = "LABEXA",
                        Descricao="Exame de Laboratório",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new RegistroTabela
                    {
                        Id=3,
                        Codigo = "ASSPRESC",
                        Descricao="Prescrição Médica",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                     new RegistroTabela
                    {
                        Id=4,
                        Codigo = "PRESCENF",
                        Descricao="Prescrição Enfermagem",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                      new RegistroTabela
                    {
                        Id=5,
                        Codigo = "EVOLUENF",
                        Descricao="Evolução Enfermagem",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                       new RegistroTabela
                    {
                        Id=6,
                        Codigo = "ADMISENF",
                        Descricao="Admisão Enfermagem",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                          new RegistroTabela
                    {
                        Id=7,
                        Codigo = "PASSPLAN",
                        Descricao="Passagem de Pantão",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                     new RegistroTabela
                    {
                        Id=8,
                        Codigo = "ADMISMED",
                        Descricao="Admissão Médica",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                       new RegistroTabela
                    {
                        Id=9,
                        Codigo = "ALTAMED",
                        Descricao="Alta Médica",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                     new RegistroTabela
                    {
                        Id=10,
                        Codigo = "ANAMNESE",
                        Descricao="Anamnese",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                      new RegistroTabela
                    {
                        Id=11,
                        Codigo = "EVOLUMED",
                        Descricao="Evolução Médica",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new RegistroTabela
                    {
                        Id=12,
                        Codigo = "LOTEXML",
                        Descricao="Lote XML",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new RegistroTabela
                    {
                        Id=14,
                        Codigo = "BH",
                        Descricao="Balanço Hídrico",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new RegistroTabela
                    {
                        Id=15,
                        Codigo = "RECEITAMED",
                        Descricao="Receituário Médico",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2,
                        TabelaPrincipal = "SisReceituario"
                    },
            };


            var registrosTabelas = _context.RegistrosTabelas.ToList();
            if (registrosTabelas == null || registrosTabelas.Count() == 0 || registrosTabelas.Count() < listRegistroTabela.Count())
            {

                foreach (var item in registrosTabelas)
                {
                    var temp = listRegistroTabela.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listRegistroTabela.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                try
                {
                    listRegistroTabela.ForEach(c => _context.RegistrosTabelas.Add(c));
                }
                catch (Exception)
                {

                }

            }
            #endregion

            #region AtendimentoStatus
            var listAtendimentosStatus = new List<AtendimentoStatus>
            {
                 new AtendimentoStatus
                {
                    Id=1,
                    Descricao= "Aguardando",
                    Codigo="A",
                    CreationTime=DateTime.Now,
                    CreatorUserId=2,
                    IsDeleted=false,
                    IsSistema=true
                },


                 new AtendimentoStatus
                {
                     Id=2,
                    Descricao= "Em Atendimento",
                    Codigo="EmA",
                    CreationTime=DateTime.Now,
                    CreatorUserId=2,
                    IsDeleted=false,
                    IsSistema=true
                },

                 new AtendimentoStatus
                {
                     Id=3,
                    Descricao= "Pendente",
                    Codigo="P",
                    CreationTime=DateTime.Now,
                    CreatorUserId=2,
                    IsDeleted=false,
                    IsSistema=true
                },



                 new AtendimentoStatus
                {
                     Id=4,
                    Descricao= "Atendido",
                    Codigo="At",
                    CreationTime=DateTime.Now,
                    CreatorUserId=2,
                    IsDeleted=false,
                    IsSistema=true
                },

            };
            var atendimentosStatus = _context.AtendimentosStatus.ToList();
            if (atendimentosStatus == null || atendimentosStatus.Count() == 0 || atendimentosStatus.Count() < listAtendimentosStatus.Count())
            {
                foreach (var item in atendimentosStatus)
                {
                    var temp = listAtendimentosStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listAtendimentosStatus.Remove(temp);
                    }
                }
                listAtendimentosStatus.ForEach(s => _context.AtendimentosStatus.Add(s));
                _context.SaveChanges();
            }
            #endregion

            #region ExameStatus

            var listExameStatus = new List<ExameStatus>
                {
                    new ExameStatus
                    {
                        Id=1,
                        Codigo = "INI",
                        Descricao="Inicial",
                        Cor = "#ef1a07",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },

                    new ExameStatus
                    {
                        Id=2,
                        Codigo = "PEND",
                        Descricao="Pendente",
                        Cor = "#e5ed09",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },

                    new ExameStatus
                    {
                        Id=3,
                        Codigo = "DIGI",
                        Descricao="Digitado",
                        Cor = "#0a07ef",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },

                    new ExameStatus
                    {
                        Id=4,
                        Codigo = "CONF",
                        Descricao="Conferido",
                        Cor = "#44b6ae",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new ExameStatus
                    {
                        Id=5,
                        Codigo = "ECO",
                        Descricao="Em Coleta",
                        Cor = "#67809F",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new ExameStatus
                    {
                        Id=6,
                        Codigo = "COL",
                        Descricao="Coletado",
                        Cor = "#67809F",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    },
                    new ExameStatus
                    {
                        Id=7,
                        Codigo = "INT",
                        Descricao="Interfaceado",
                        Cor = "#f3c200",
                        IsDeleted=false,
                        IsSistema=true,
                        CreatorUserId=2
                    }
                };

            var examesStatus = _context.ExamesStatus.ToList();

            if (examesStatus == null || examesStatus.Count() == 0 || examesStatus.Count() < listExameStatus.Count())
            {
                foreach (var item in examesStatus)
                {
                    var temp = listExameStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listExameStatus.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                try
                {
                    listExameStatus.ForEach(c => _context.ExamesStatus.Add(c));
                }
                catch (Exception)
                {

                }



            }


            #endregion

            #region ServicoMedicoPrestado
            var listServicosMedicosPrestados = new List<ServicoMedicoPrestado>
                {
                    new ServicoMedicoPrestado
                    {
                        Codigo = "1",
                        Descricao="Primeira Consulta",
                        ImportaId=1,
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new ServicoMedicoPrestado
                    {
                        Codigo = "2",
                        Descricao="Retorno",
                        ImportaId=2,
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new ServicoMedicoPrestado
                    {
                        Codigo = "3",
                        Descricao="Pré-Natal",
                        ImportaId=3,
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new ServicoMedicoPrestado
                    {
                        Codigo = "4",
                        Descricao="Por Encaminhamento",
                        ImportaId=4,
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                };
            var servicosMedicosPrestados = _context.ServicosMedicosPrestados.ToList();
            if (servicosMedicosPrestados == null || servicosMedicosPrestados.Count() == 0 || servicosMedicosPrestados.Count() < listServicosMedicosPrestados.Count())
            {
                foreach (var item in servicosMedicosPrestados)
                {
                    var temp = listServicosMedicosPrestados.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listServicosMedicosPrestados.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listServicosMedicosPrestados.ForEach(c => _context.ServicosMedicosPrestados.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region TevRisco
            var listTevRiscos = new List<TevRisco>
                {
                    new TevRisco
                    {
                        Codigo = "1",
                        Descricao="R0",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new TevRisco
                    {
                        Codigo = "2",
                        Descricao="R1",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new TevRisco
                    {
                        Codigo = "3",
                        Descricao="R2",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new TevRisco
                    {
                        Codigo = "4",
                        Descricao="R3",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new TevRisco
                    {
                        Codigo = "5",
                        Descricao="R4",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new TevRisco
                    {
                        Codigo = "6",
                        Descricao="R5",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                };
            var tevRiscos = _context.TevRiscos.ToList();
            if (tevRiscos == null || tevRiscos.Count() == 0 || tevRiscos.Count() < listTevRiscos.Count())
            {
                foreach (var item in tevRiscos)
                {
                    var temp = listTevRiscos.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTevRiscos.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listTevRiscos.ForEach(c => _context.TevRiscos.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region SisParametro
            var listParametros = new List<Parametro>
                {
                    new Parametro
                    {
                        Codigo = "LABCABECEX",
                        Descricao="<p><br></p><table class=\"table table-bordered\"><tbody><tr><td><span style=\"font-size: 11px;\">Paciente:</span><span style=\"font-size: 11px;\">&nbsp; &nbsp;&nbsp;</span><span style=\"font-size: 11px;\">[PACIENTE]</span><span style=\"font-size: 11px;\">&nbsp;&nbsp;</span><br></td><td><span style=\"font-size: 11px;\">Código:</span><span style=\"font-size: 11px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span><span style=\"font-size: 11px;\">[CODIGO]</span><br></td></tr><tr><td><span style=\"font-size: 11px; background-color: rgb(255, 255, 255);\">Convênio:</span><span style=\"font-size: 11px;\"><span style=\"background-color: rgb(255, 255, 255);\">&nbsp;</span> &nbsp;</span><span style=\"font-size: 11px;\">[CONVENIO]</span><span style=\"font-size: 11px;\">&nbsp; &nbsp;</span><br></td><td><span style=\"font-size: 11px;\">Sexo:</span><span style=\"font-size: 11px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span><span style=\"font-size: 11px;\">[SEXO]</span><br></td></tr><tr><td><span style=\"font-size: 11px;\">Nº Exame: [EXAME]</span><br></td><td><span style=\"font-size: 11px;\">Dt. Coleta:&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;[DTCOLETA]</span><br></td></tr><tr><td><span style=\"font-size: 11px;\">Médico:&nbsp; &nbsp; &nbsp; [MEDICO]</span><br></td><td><span style=\"font-size: 11px;\">Idade (Na coleta):&nbsp; [IDADE]</span><br></td></tr><tr><td><span style=\"font-size: 11px;\">Origem:&nbsp; &nbsp; &nbsp; [ORIGEM]</span><br></td><td><span style=\"font-size: 11px;\">Dt. Impressão:&nbsp; &nbsp; &nbsp; &nbsp;[DTIMPRESSAO]</span><br></td></tr></tbody></table>",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new Parametro
                    {
                        Codigo = "LABTEXTCAB",
                        Descricao="SES 109/2012 CRFRJ 8029/02        DIREÇÃO  TÉCNICA: Dr. NERO BARRETO           CRFRJ 3040",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new Parametro
                    {
                        Codigo = "TEVDALERTA",
                        Descricao="2",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                };
            var parametros = _context.Parametros.ToList();
            if (parametros == null || parametros.Count() == 0 || parametros.Count() < listParametros.Count())
            {
                foreach (var item in parametros)
                {
                    var temp = listParametros.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listParametros.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listParametros.ForEach(c => _context.Parametros.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region SisVersaoTISS

            var listVersaoTiss = new List<VersaoTiss>
                {
                    new VersaoTiss
                    {
                        Id=1,
                        Codigo = "03.03.00",
                        Descricao="03.03.00",
                        DataInicio =  new DateTime (2016,6,1),
                        DataFim =  new DateTime (2016,11,30),
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new VersaoTiss
                    {
                        Id=2,
                        Codigo = "03.03.01",
                        Descricao="03.03.01",
                        DataInicio =  new DateTime (2016,10,1),
                        DataFim =  new DateTime (2016,11,30),
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new VersaoTiss
                    {
                        Id=3,
                        Codigo = "03.03.02",
                        Descricao="03.03.02",
                        DataInicio =  new DateTime (2017,4,10),
                        DataFim =  new DateTime (2017,4,10),
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new VersaoTiss
                    {
                        Id=4,
                        Codigo = "03.03.03",
                        Descricao="03.03.03",
                        DataInicio =  new DateTime (2018,1,1),
                        DataFim =  new DateTime (2018,6,25),
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                };

            var versaoTiss = _context.VersoesTiss.ToList();
            if (versaoTiss == null || versaoTiss.Count() == 0 || versaoTiss.Count() < listVersaoTiss.Count())
            {
                foreach (var item in versaoTiss)
                {
                    var temp = listVersaoTiss.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listVersaoTiss.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listVersaoTiss.ForEach(c => _context.VersoesTiss.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region FaturamentoCodigoDespesa

            var listFaturamentoCodigoDespesa = new List<FaturamentoCodigoDespesa>
                {
                    new FaturamentoCodigoDespesa
                    {
                        Id=1,
                        Codigo = "01",
                        Descricao="Gases Medicinais",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new FaturamentoCodigoDespesa
                    {
                        Id=2,
                        Codigo = "02",
                        Descricao="Medicamentos",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new FaturamentoCodigoDespesa
                    {
                        Id=3,
                        Codigo = "03",
                        Descricao="Materiais",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new FaturamentoCodigoDespesa
                    {
                        Id=4,
                        Codigo = "05",
                        Descricao="Diárias",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new FaturamentoCodigoDespesa
                    {
                        Id=5,
                        Codigo = "07",
                        Descricao="Taxas e aluguéis",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new FaturamentoCodigoDespesa
                    {
                        Id=6,
                        Codigo = "08",
                        Descricao="OPME",
                        IsDeleted=false,
                        IsSistema=false,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    };

            var faturamentosCodigosDespesas = _context.FaturamentosCodigosDespesas.ToList();
            if (faturamentosCodigosDespesas == null || faturamentosCodigosDespesas.Count() == 0 || faturamentosCodigosDespesas.Count() < listFaturamentoCodigoDespesa.Count())
            {
                foreach (var item in faturamentosCodigosDespesas)
                {
                    var temp = listFaturamentoCodigoDespesa.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listFaturamentoCodigoDespesa.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listFaturamentoCodigoDespesa.ForEach(c => _context.FaturamentosCodigosDespesas.Add(c));
                _context.SaveChanges();

            }
            #endregion

            #region LeitoStatus
            var listLeitosStatus = new List<LeitoStatus>
                {
                    new LeitoStatus
                    {
                        Codigo = "V",
                        Descricao="Vago",
                        Cor="#2dfa1e",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new LeitoStatus
                    {
                        Codigo = "O",
                        Descricao="Ocupado",
                        Cor="#fa0000",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                };
            var leitosStatus = _context.LeitosStatus.ToList();
            if (leitosStatus == null || leitosStatus.Count() == 0 || leitosStatus.Count() < listLeitosStatus.Count())
            {
                foreach (var item in leitosStatus)
                {
                    var temp = listLeitosStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listLeitosStatus.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listLeitosStatus.ForEach(c => _context.LeitosStatus.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region MotivoAltaTiposAlta

            var listMotivoAltaTipoAlta = new List<MotivoAltaTipoAlta>
                {
                    new MotivoAltaTipoAlta
                    {
                        Id =1,
                        Descricao = "Alta",
                        Codigo= "001",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new MotivoAltaTipoAlta
                    {
                        Id =2,
                        Descricao = "Óbito",
                        Codigo= "002",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new MotivoAltaTipoAlta
                    {
                        Id =3,
                        Descricao = "Transferência",
                        Codigo= "003",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new MotivoAltaTipoAlta
                    {
                        Id =4,
                        Descricao = "Internação",
                        Codigo= "004",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
            };


            var motivoAltaTiposAltas = _context.MotivoAltaTiposAlta.ToList();
            if (motivoAltaTiposAltas == null || motivoAltaTiposAltas.Count() == 0 || motivoAltaTiposAltas.Count() < listMotivoAltaTipoAlta.Count())
            {
                foreach (var item in motivoAltaTiposAltas)
                {
                    var temp = listMotivoAltaTipoAlta.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listMotivoAltaTipoAlta.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listMotivoAltaTipoAlta.ForEach(c => _context.MotivoAltaTiposAlta.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region AtendimentoMotivoCancelamento

            var listAtendimentoMotivoCancelamento = new List<AtendimentoMotivoCancelamento>
                {
                    new AtendimentoMotivoCancelamento
                    {
                        Id =1,
                        Descricao = "Criação de atendimento duplicado",
                        Codigo= "01",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
            };

            var motivosCancelamentos = _context.AtendimentoMotivosCancelamentos.ToList();
            if (motivosCancelamentos == null || motivosCancelamentos.Count() == 0 || motivosCancelamentos.Count() < listAtendimentoMotivoCancelamento.Count())
            {
                foreach (var item in motivosCancelamentos)
                {
                    var temp = listAtendimentoMotivoCancelamento.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listAtendimentoMotivoCancelamento.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listAtendimentoMotivoCancelamento.ForEach(c => _context.AtendimentoMotivosCancelamentos.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region TipoTabelaDominio

            var listTipoTabelaDominio = new List<TipoTabelaDominio>
                {
                    new TipoTabelaDominio
                    {
                        Id =1,
                        Descricao = "Diárias, taxas e gases medicinais",
                        Codigo= "18",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =2,
                        Descricao = "Materiais e Órteses, Próteses e Materiais Especiais (OPME)",
                        Codigo= "19",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =3,
                        Descricao = "Medicamento",
                        Codigo= "20",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =4,
                        Descricao = "Procedimentos e eventos em saúde",
                        Codigo= "22",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =5,
                        Descricao = "Caráter do atendimento",
                        Codigo= "23",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                        Id =6,
                        Descricao = "Classificação Brasileira de Ocupações (CBO)",
                        Codigo= "24",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =7,
                        Descricao = "Código da despesa",
                        Codigo= "25",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =8,
                        Descricao = "Conselho profissional",
                        Codigo= "26",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =9,
                        Descricao = "Débitos e crédito",
                        Codigo= "27",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                        Id =10,
                        Descricao = "Dente",
                        Codigo= "28",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =11,
                        Descricao = "Diagnóstico por imagem",
                        Codigo= "29",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =12,
                        Descricao = "Escala de capacidade funcional (ECOG - Escala de Zubrod)",
                        Codigo= "30",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                        Id =13,
                        Descricao = "Estadiamento do tumor",
                        Codigo= "31",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                        Id =14,
                        Descricao = "Faces do dente",
                        Codigo= "32",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                        Id =15,
                        Descricao = "Finalidade do tratamento",
                        Codigo= "33",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                        Id =16,
                        Descricao = "Forma de pagamento",
                        Codigo= "34",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =17,
                        Descricao = "Grau de participação",
                        Codigo= "35",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =18,
                        Descricao = "Indicador de acidente",
                        Codigo= "36",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =19,
                        Descricao = "Indicador de débito ou crédito",
                        Codigo= "37",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =20,
                        Descricao = "Mensagens (glosas, negativas e outras)",
                        Codigo= "38",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                        new TipoTabelaDominio
                    {
                       Id =21,
                        Descricao = "Motivo de encerramento",
                        Codigo= "39",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =22,
                        Descricao = "Origem da Guia",
                        Codigo= "40",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =23,
                        Descricao = "Regime de internação",
                        Codigo= "41",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =24,
                        Descricao = "Regiões da boca",
                        Codigo= "42",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TipoTabelaDominio
                    {
                       Id =25,
                        Descricao = "Sexo",
                        Codigo= "43",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                         new TipoTabelaDominio
                    {
                       Id =26,
                        Descricao = "Situação inicial do dente",
                        Codigo= "44",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =27,
                        Descricao = "Status da solicitação",
                        Codigo= "45",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =28,
                        Descricao = "Status do cancelamento",
                        Codigo= "46",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TipoTabelaDominio
                    {
                       Id =29,
                        Descricao = "Status do protocolo",
                        Codigo= "47",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =30,
                        Descricao = "Técnica utilizada",
                        Codigo= "48",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =31,
                        Descricao = "Tipo de acomodação",
                        Codigo= "49",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =32,
                        Descricao = "Tipo de atendimento",
                        Codigo= "50",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =33,
                        Descricao = "Tipo de atendimento em odontologia",
                        Codigo= "51",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =34,
                        Descricao = "Tipo de consulta",
                        Codigo= "52",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =35,
                        Descricao = "Tipo de demonstrativo",
                        Codigo= "53",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =36,
                        Descricao = "Tipo de guia",
                        Codigo= "54",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =37,
                        Descricao = "Tipo de faturamento",
                        Codigo= "55",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =38,
                        Descricao = "Natureza da guia",
                        Codigo= "56",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                         new TipoTabelaDominio
                    {
                       Id =39,
                        Descricao = "Tipo de internação",
                        Codigo= "57",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =40,
                        Descricao = "Tipo de quimioterapia",
                        Codigo= "58",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =41,
                        Descricao = "Unidade da federação",
                        Codigo= "59",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                        new TipoTabelaDominio
                    {
                       Id =42,
                        Descricao = "Unidade de medida",
                        Codigo= "60",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =43,
                        Descricao = "Via de acesso",
                        Codigo= "61",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =44,
                        Descricao = "Via de administração",
                        Codigo= "62",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =45,
                        Descricao = "Grupos de procedimentos e itens assistenciais para envio para ANS",
                        Codigo= "63",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =46,
                        Descricao = "Forma de envio de procedimentos e itens assistenciais para ANS",
                        Codigo= "64",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TipoTabelaDominio
                    {
                       Id =47,
                        Descricao = "metástases",
                        Codigo= "65",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                          new TipoTabelaDominio
                    {
                       Id =48,
                        Descricao = "nódulo",
                        Codigo= "66",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TipoTabelaDominio
                    {
                       Id =49,
                        Descricao = "tumor",
                        Codigo= "67",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TipoTabelaDominio
                    {
                       Id =50,
                        Descricao = "Categoria de Despesa",
                        Codigo= "68",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TipoTabelaDominio
                    {
                       Id =51,
                        Descricao = "versão do padrão",
                        Codigo= "69",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TipoTabelaDominio
                    {
                       Id =52,
                        Descricao = "forma de envio do padrão",
                        Codigo= "70",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                          new TipoTabelaDominio
                    {
                       Id =53,
                        Descricao = "Tipo de atendimento por operadora intermediário",
                        Codigo= "71",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TipoTabelaDominio
                    {
                       Id =54,
                        Descricao = "Relação das terminologias unificadas na saúde suplementar",
                        Codigo= "87",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

            };


            var tiposTabelasDominios = _context.TiposTabelaDominio.ToList();
            if (tiposTabelasDominios == null || tiposTabelasDominios.Count() == 0 || tiposTabelasDominios.Count() < listTipoTabelaDominio.Count())
            {
                foreach (var item in tiposTabelasDominios)
                {
                    var temp = listTipoTabelaDominio.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTipoTabelaDominio.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listTipoTabelaDominio.ForEach(c => _context.TiposTabelaDominio.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region Tabela de Dominio - 

            var listTabelaDominio = new List<TabelaDominio>
             {

                #region  Tipos de Atendimento
                new TabelaDominio
                    {
                         TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Remoção",
                        Codigo= "01",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                           new TabelaDominio
                    {
                               TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Pequena Cirurgia",
                        Codigo= "02",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Outras Terapias",
                        Codigo= "03",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Consulta",
                        Codigo= "04",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Exame Ambulatorial",
                        Codigo= "05",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                         TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Atendimento Domiciliar",
                        Codigo= "06",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                         TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Internação",
                        Codigo= "07",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                     new TabelaDominio
                    {
                         TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Quimioterapia",
                        Codigo= "08",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                         TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Radioterapia",
                        Codigo= "09",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Terapia Renal Substitutiva (TRS)",
                        Codigo= "10",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Pronto Socorro",
                        Codigo= "11",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Pequeno atendimento (sutura, gesso e outros)",
                        Codigo= "13",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                     new TabelaDominio
                    {
                         TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Admissional",
                        Codigo= "14",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Demissional",
                        Codigo= "15",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Periódico",
                        Codigo= "16",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Retorno ao trabalho",
                        Codigo= "17",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Mudança de função",
                        Codigo= "18",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Promoção a saúde",
                        Codigo= "19",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Beneficiário novo",
                        Codigo= "20",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAtendimento,
                        Descricao = "Saúde Ocupacional - Assistência a demitidos",
                        Codigo= "21",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region Tipo de Internação

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoInternação,
                        Descricao = "Clínica",
                        Codigo= "1",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoInternação,
                        Descricao = "Cirúrgica",
                        Codigo= "2",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoInternação,
                        Descricao = "Obstétrica",
                        Codigo= "3",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoInternação,
                        Descricao = "Pediátrica",
                        Codigo= "4",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                        new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoInternação,
                        Descricao = "Psiquiátrica",
                        Codigo= "5",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },



                #endregion

                #region Código de Despesa

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Gases medicinais",
                        Codigo= "01",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Medicamentos",
                        Codigo= "02",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                   new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Materiais",
                        Codigo= "03",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Diárias",
                        Codigo= "05",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Taxas e aluguéis",
                        Codigo= "07",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                         new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "OPME",
                        Codigo= "08",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region Conselho Profissional

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.ConselhoProfissional,
                        Descricao = "Conselho Regional de Assistência Social (CRAS)",
                        Codigo= "01",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Enfermagem (COREN)",
                        Codigo= "02",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Farmácia (CRF)",
                        Codigo= "03",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Fonoaudiologia (CRFA)",
                        Codigo= "04",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Fisioterapia e Terapia Ocupacional (CREFITO)",
                        Codigo= "05",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Medicina (CRM)",
                        Codigo= "06",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Nutrição (CRN)",
                        Codigo= "07",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Odontologia (CRO)",
                        Codigo= "08",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Conselho Regional de Psicologia (CRP)",
                        Codigo= "09",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.CodigoDespesa,
                        Descricao = "Outros Conselhos",
                        Codigo= "10",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },
                #endregion

                #region Tipo de Diagnostíco por imagem

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.DiagnosticoPorImagem,
                        Descricao = "Tomografia",
                        Codigo= "1",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.DiagnosticoPorImagem,
                        Descricao = "Ressonância Magnética",
                        Codigo= "2",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.DiagnosticoPorImagem,
                        Descricao = "Raios-X",
                        Codigo= "3",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                   new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.DiagnosticoPorImagem,
                        Descricao = "Outras",
                        Codigo= "4",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.DiagnosticoPorImagem,
                        Descricao = "Ultrassonografia",
                        Codigo= "5",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.DiagnosticoPorImagem,
                        Descricao = "PET",
                        Codigo= "6",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region Grau de Participação

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Cirurgião",
                        Codigo= "00",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Primeiro Auxiliar",
                        Codigo= "01",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Segundo Auxiliar",
                        Codigo= "02",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                   new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Terceiro Auxiliar",
                        Codigo= "03",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Quarto Auxiliar",
                        Codigo= "04",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Instrumentador",
                        Codigo= "05",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Anestesista",
                        Codigo= "06",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Auxiliar de Anestesista",
                        Codigo= "07",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                        new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Consultor",
                        Codigo= "08",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                         new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Perfusionista",
                        Codigo= "09",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                          new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Pediatra na sala de parto",
                        Codigo= "10",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Auxiliar SADT",
                        Codigo= "11",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Clínico",
                        Codigo= "12",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                       new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.GrauParticipacao,
                        Descricao = "Intensivista",
                        Codigo= "13",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region  Indicador de Acidente

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.IndicadorAcidente,
                        Descricao = "Trabalho",
                        Codigo= "0",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.IndicadorAcidente,
                        Descricao = "Trânsito",
                        Codigo= "1",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.IndicadorAcidente,
                        Descricao = "Outros",
                        Codigo= "2",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                   new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.IndicadorAcidente,
                        Descricao = "Não Acidente",
                        Codigo= "9",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region  Motivo de Encerramento

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta Curado",
                        Codigo= "11",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta Melhorado",
                        Codigo= "12",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta a pedido",
                        Codigo= "14",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                   new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta com previsão de retorno para acompanhamento do paciente",
                        Codigo= "15",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta por Evasão",
                        Codigo= "16",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta por outros motivos",
                        Codigo= "18",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta de Paciente Agudo em Psiquiatria",
                        Codigo= "19",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por características próprias da doença",
                        Codigo= "21",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por intercorrência",
                        Codigo= "22",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por impossibilidade sócio-familiar",
                        Codigo= "23",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por Processo de doação de órgãos, tecidos e células - doador vivo",
                        Codigo= "24",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por Processo de doação de órgãos, tecidos e células - doador morto",
                        Codigo= "25",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por mudança de Procedimento",
                        Codigo= "26",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, por reoperação",
                        Codigo= "27",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Permanência, outros motivos",
                        Codigo= "28",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Transferido para outro estabelecimento",
                        Codigo= "31",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Transferência para Internação Domicilia",
                        Codigo= "32",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Óbito com declaração de óbito fornecida pelo médico assistente",
                        Codigo= "41",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Óbito com declaração de Óbito fornecida pelo Instituto Médico Legal - IML",
                        Codigo= "42",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Óbito com declaração de Óbito fornecida pelo Serviço de Verificação de Óbito - SVO.",
                        Codigo= "43",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Encerramento Administrativo",
                        Codigo= "51",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Encerramento Administrativo",
                        Codigo= "51",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta da mãe/puérpera e do recém-nascido",
                        Codigo= "61",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta da mãe/puérpera e permanência do recém-nascido",
                        Codigo= "62",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta da mãe/puérpera e óbito do recém-nascido",
                        Codigo= "63",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Alta da mãe/puérpera com óbito fetal",
                        Codigo= "64",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Óbito da gestante e do concepto",
                        Codigo= "65",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Óbito da mãe/puérpera  e alta do recém-nascido",
                        Codigo= "66",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.MotivoEncerramento,
                        Descricao = "Óbito da mãe/puérpera  e permanência do recém-nascido",
                        Codigo= "67",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region  Regime de Internação

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.RegimeInternacao,
                        Descricao = "Hospitalar",
                        Codigo= "1",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.RegimeInternacao,
                        Descricao = "Hospital–dia",
                        Codigo= "2",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.RegimeInternacao,
                        Descricao = "Domiciliar",
                        Codigo= "3",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region  Sexo

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.Sexo,
                        Descricao = "Masculino",
                        Codigo= "1",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.Sexo,
                        Descricao = "Feminino",
                        Codigo= "3",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

                #region  Tecnica Utilizada

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TecnicaUtilizada,
                        Descricao = "Convencional",
                        Codigo= "1",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TecnicaUtilizada,
                        Descricao = "Video",
                        Codigo= "2",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TecnicaUtilizada,
                        Descricao = "Robótica",
                        Codigo= "3",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion
                    
                #region  Tipo de Acomodacao

                new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "QUARTO PRIVATIVO / PARTICULAR",
                        Codigo= "02",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                 new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO DE LUXO DA MATERNIDADE",
                        Codigo= "09",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                  new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO DE LUXO DE PSIQUIATRIA",
                        Codigo= "10",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                   new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO DE LUXO",
                        Codigo= "11",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO SIMPLES",
                        Codigo= "12",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO STANDARD",
                        Codigo= "13",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                      new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO SUÍTE",
                        Codigo= "14",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                     new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO COM ALOJAMENTO CONJUNTO",
                        Codigo= "15",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO PARA PACIENTE COM OBESIDADE MÓRBIDA",
                        Codigo= "16",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO SIMPLES DA MATERNIDADE",
                        Codigo= "17",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO SIMPLES DE PSIQUIATRIA",
                        Codigo= "18",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO SUÍTE DA MATERNIDADE",
                        Codigo= "19",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO SUÍTE DE PSIQUIATRIA",
                        Codigo= "20",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "BERÇÁRIO NORMAL",
                        Codigo= "21",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "BERÇÁRIO PATOLÓGICO / PREMATURO",
                        Codigo= "22",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "ENFERMARIA DE 3 LEITOS DA MATERNIDADE",
                        Codigo= "25",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "ENFERMARIA DE 4 OU MAIS LEITOS DA MATERNIDADE",
                        Codigo= "26",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "HOSPITAL DIA APARTAMENTO",
                        Codigo= "27",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "HOSPITAL DIA ENFERMARIA",
                        Codigo= "28",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "HOSPITAL DIA PSIQUIATRIA",
                        Codigo= "29",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "QUARTO COLETIVO DE 2 LEITOS DA MATERNIDADE",
                        Codigo= "30",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "ENFERMARIA DE 3 LEITOS",
                        Codigo= "31",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "ENFERMARIA DE 4 OU MAIS LEITOS",
                        Codigo= "32",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "ENFERMARIA COM ALOJAMENTO CONJUNTO",
                        Codigo= "33",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "QUARTO PRIVATIVO / PARTICULAR DA MATERNIDADE",
                        Codigo= "36",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "QUARTO PRIVATIVO / PARTICULAR DE PSIQUIATRI",
                        Codigo= "37",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI UTI ADULTO GERAL",
                        Codigo= "38",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI UTI CORONARIANA",
                        Codigo= "39",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI UTI NEONATAL",
                        Codigo= "40",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "QUARTO COLETIVO DE 2 LEITOS",
                        Codigo= "41",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "QUARTO COM ALOJAMENTO CONJUNTO",
                        Codigo= "43",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI UTI NEUROLÓGICA",
                        Codigo= "44",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI UTI INFANTIL/PEDIÁTRICA",
                        Codigo= "45",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI",
                        Codigo= "40",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },


                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "SEMI UTI QUEIMADOS",
                        Codigo= "46",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UNIDADE DE TRANSPLANTE DE MEDULA ÓSSEA",
                        Codigo= "47",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UNIDADE DE TRANSPLANTE EM GERAL",
                        Codigo= "48",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO STANDARD DA MATERNIDADE",
                        Codigo= "49",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "APARTAMENTO STANDARD DE PSIQUIATRIA",
                        Codigo= "50",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UTI ADULTO GERAL",
                        Codigo= "51",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UTI INFANTIL/PEDIÁTRICA",
                        Codigo= "52",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UTI NEONATAL",
                        Codigo= "53",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UNIDADE PARA TRATAMENTO RADIOATIVO",
                        Codigo= "56",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UTI CORONARIANA",
                        Codigo= "57",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UTI NEUROLÓGICA",
                        Codigo= "58",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                    new TabelaDominio
                    {
                        TipoTabelaDominioId = (long)EnumTipoTabelaDominio.TipoAcomodacao,
                        Descricao = "UTI QUEIMADOS",
                        Codigo= "59",
                        IsDeleted=false,
                        IsSistema=true,
                        CreationTime=DateTime.Now,
                        CreatorUserId=2
                    },

                #endregion

            };



            var tabelasDominios = _context.TabelasDominio.ToList();
            if (tabelasDominios == null || tabelasDominios.Count() == 0 || tabelasDominios.Count() < listTabelaDominio.Count())
            {
                foreach (var item in tabelasDominios)
                {
                    var temp = listTabelaDominio.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTabelaDominio.Remove(temp);
                    }
                }
                //listTiposPrescricoes.RemoveRange(0, tiposPrescricoes.Count());
                listTabelaDominio.ForEach(c => _context.TabelasDominio.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region ResultadoStatus
            var listResultadoStatus = new List<ResultadoStatus>
                {
                    new ResultadoStatus
                    {
                        Id = 1,
                        Codigo = "001",
                        Descricao = "Não Autorizado",
                        CorFonte = "#FFF",
                        CorFundo = "#FF0000",
                        Sequencia = 1,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 2,
                        Codigo = "002",
                        Descricao = "Autorizado",
                        CorFonte = "#000",
                        CorFundo = "#FFFF00",
                        Sequencia = 2,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 3,
                        Codigo = "003",
                        Descricao = "Realizado",
                        CorFonte = "#FFF",
                        CorFundo = "#0000FF",
                        Sequencia = 3,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 4,
                        Codigo = "004",
                        Descricao = "Pronto",
                        CorFonte = "#FFF",
                        CorFundo = "#32CD32",
                        Sequencia = 4,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 5,
                        Codigo = "005",
                        Descricao = "Pendete",
                        CorFonte = "#FFF",
                        CorFundo = "#808000",
                        Sequencia = 5,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 6,
                        Codigo = "006",
                        Descricao = "Inicial",
                        CorFonte = "#FFF",
                        CorFundo = "#525e64",
                        Sequencia = 6,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 7,
                        Codigo = "007",
                        Descricao = "Em andamento",
                        CorFonte = "#FFF",
                        CorFundo = "#f3c200",
                        Sequencia = 7,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 8,
                        Codigo = "008",
                        Descricao = "Liberado",
                        CorFonte = "#FFF",
                        CorFundo = "#44b6ae",
                        Sequencia = 8,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 9,
                        Codigo = "009",
                        Descricao = "Digitado",
                        CorFonte = "#FFF",
                        CorFundo = "#67809F",
                        Sequencia = 9,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 10,
                        Codigo = "010",
                        Descricao = "Conferido",
                        CorFonte = "#FFF",
                        CorFundo = "#44b6ae",
                        Sequencia = 10,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 11,
                        Codigo = "011",
                        Descricao = "Em Coleta",
                        CorFonte = "#FFF",
                        CorFundo = "#67809F",
                        Sequencia = 11,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 12,
                        Codigo = "012",
                        Descricao = "Coletado",
                        CorFonte = "#FFF",
                        CorFundo = "#67809F",
                        Sequencia = 12,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 13,
                        Codigo = "013",
                        Descricao = "Interfaceado",
                        CorFonte = "#FFF",
                        CorFundo = "#f3c200",
                        Sequencia = 13,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new ResultadoStatus
                    {
                        Id = 14,
                        Codigo = "014",
                        Descricao = "Enviado Equipamento",
                        CorFonte = "#FFF",
                        CorFundo = "#f3c200",
                        Sequencia = 13,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    }
                };

            var resultadoStatus = _context.ResultadosStatus.ToList();

            if (resultadoStatus == null || resultadoStatus.Count() == 0 || resultadoStatus.Count() < listResultadoStatus.Count())
            {
                foreach (var item in resultadoStatus)
                {
                    var temp = listResultadoStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listResultadoStatus.Remove(temp);
                    }
                }
                listResultadoStatus.ForEach(c => _context.ResultadosStatus.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region Terceirizado

            string DACASA = "DA CASA";

            var terceirizado = _context.Terceirizados.Where(w => w.Codigo == DACASA).FirstOrDefault();

            if (terceirizado == null)
            {
                var pessoaDaCasa = _context.SisPessoas.Where(w => w.Codigo == DACASA).FirstOrDefault();

                if (pessoaDaCasa == null)
                {
                    var _pessoaDaCasa = new SW10.SWMANAGER.ClassesAplicacao.SisPessoa
                    {
                        Codigo = DACASA,
                        NomeCompleto = DACASA,
                        IsDeleted = false,
                        IsSistema = true,
                        IsAtivo = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    };
                    _context.SisPessoas.Add(_pessoaDaCasa);
                    _context.SaveChanges();


                    pessoaDaCasa = _context.SisPessoas.Where(w => w.Codigo == DACASA).FirstOrDefault();

                }

                var _terceirizado = new Terceirizado { Codigo = DACASA, SisPessoaId = pessoaDaCasa.Id };

                _context.Terceirizados.Add(_terceirizado);
                _context.SaveChanges();
            }

            var parametroTerceirzado = _context.Parametros.Where(w => w.Codigo == "AGETERCE").FirstOrDefault();

            if (parametroTerceirzado == null)
            {
                terceirizado = _context.Terceirizados.Where(w => w.Codigo == DACASA).FirstOrDefault();

                var _parametroTerceirzado = new Parametro
                {
                    Codigo = "AGETERCE",
                    Descricao = terceirizado.Id.ToString(),
                    IsDeleted = false,
                    IsSistema = false,
                    CreationTime = DateTime.Now,
                    CreatorUserId = 2
                };

                _context.Parametros.Add(_parametroTerceirzado);
                _context.SaveChanges();
            }

            var parametroTurno = _context.Parametros.Where(w => w.Codigo == "AGETURNO").FirstOrDefault();

            if (parametroTurno == null)
            {
                var _turno = _context.Turnos.Where(w => w.Codigo == "1").FirstOrDefault();

                if (_turno != null)
                {
                    var _parametroTurno = new Parametro
                    {
                        Codigo = "AGETURNO",
                        Descricao = _turno.Id.ToString(),
                        IsDeleted = false,
                        IsSistema = false,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    };

                    _context.Parametros.Add(_parametroTurno);
                    _context.SaveChanges();
                }

            }


            #endregion

            #region Agendamento Status
            var listAgendamentoStatus = new List<AgendamentoStatus>
                {
                    new AgendamentoStatus
                    {
                        Id = 1,
                        Codigo = "In",
                        Descricao = "Inicial",
                        IsDeleted = false,
                        IsSistema = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                    new AgendamentoStatus
                    {
                        Id = 2,
                        Codigo = "C1",
                        Descricao = "Primeira confirmação",
                        IsDeleted = false,
                        IsSistema = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                     new AgendamentoStatus
                    {
                        Id = 3,
                        Codigo = "C2",
                        Descricao = "Segunda confirmação",
                        IsDeleted = false,
                        IsSistema = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                      new AgendamentoStatus
                    {
                        Id = 4,
                        Codigo = "Ca",
                        Descricao = "Cancelado",
                        IsDeleted = false,
                        IsSistema = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                       new AgendamentoStatus
                    {
                        Id = 5,
                        Codigo = "Fa",
                        Descricao = "Faltou",
                        IsDeleted = false,
                        IsSistema = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                        new AgendamentoStatus
                    {
                        Id = 6,
                        Codigo = "At",
                        Descricao = "Atendido",
                        IsDeleted = false,
                        IsSistema = true,
                        CreationTime = DateTime.Now,
                        CreatorUserId = 2
                    },
                };

            var agendamentoStatus = _context.AgendamentosStatus.ToList();

            if (agendamentoStatus == null || agendamentoStatus.Count() == 0 || agendamentoStatus.Count() < listAgendamentoStatus.Count())
            {
                foreach (var item in agendamentoStatus)
                {
                    var temp = listAgendamentoStatus.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listAgendamentoStatus.Remove(temp);
                    }
                }
                listAgendamentoStatus.ForEach(c => _context.AgendamentosStatus.Add(c));
                _context.SaveChanges();
            }
            #endregion

            #region ClassificacaoAtendimento

            var listClassificacaoAtendimento = new List<ClassificacaoAtendimento>
            {
                new ClassificacaoAtendimento
                    {
                        Codigo = "1",
                        Descricao = "Vermelho",
                        Prioridade = 1,
                        PrazoAtendimento = 0,
                         Ativo = true,
                         Cor = "#f21004"
                    },

                 new ClassificacaoAtendimento
                    {
                        Codigo = "2",
                        Descricao = "Laranja",
                         Prioridade = 2,
                        PrazoAtendimento = 10,
                         Ativo = true,
                         Cor = "#f18e03"
                    },

                   new ClassificacaoAtendimento
                    {
                        Codigo = "3",
                        Descricao = "Amarelo",
                         Prioridade = 3,
                        PrazoAtendimento = 60,
                         Ativo = true,
                         Cor = "#f0e102"
                    },

                     new ClassificacaoAtendimento
                    {
                        Codigo = "4",
                        Descricao = "Verde",
                         Prioridade = 4,
                        PrazoAtendimento = 120,
                         Ativo = true,
                         Cor = "#0aad4e"
                    },

                      new ClassificacaoAtendimento
                    {
                        Codigo = "5",
                        Descricao = "Azul",
                         Prioridade = 5,
                        PrazoAtendimento = 240,
                         Ativo = true,
                         Cor = "#090fad"
                    },

            };



            var classificacoesAtendimentos = _context.ClassificacoesAtendimentos.ToList();

            if (classificacoesAtendimentos == null || classificacoesAtendimentos.Count() == 0 || classificacoesAtendimentos.Count() < listClassificacaoAtendimento.Count())
            {
                foreach (var item in classificacoesAtendimentos)
                {
                    var temp = listClassificacaoAtendimento.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listClassificacaoAtendimento.Remove(temp);
                    }
                }
                listClassificacaoAtendimento.ForEach(c => _context.ClassificacoesAtendimentos.Add(c));
                _context.SaveChanges();
            }



            #endregion

            #region FormaAutorizacao

            var listFormaAutorizacao = new List<FormaAutorizacao>
            {
                new FormaAutorizacao
                    {
                        Id = 1,
                        Codigo= "001",
                        Descricao = "Email",

                    },

                 new FormaAutorizacao
                    {
                        Id = 2,
                        Codigo= "002",
                        Descricao = "Site",

                    },
                  new FormaAutorizacao
                    {
                        Id = 3,
                        Codigo= "003",
                        Descricao = "Automático",

                    },
                   new FormaAutorizacao
                    {
                        Id = 4,
                        Codigo= "004",
                        Descricao = "Auditor interno",

                    },

                    new FormaAutorizacao
                    {
                        Id = 5,
                        Codigo= "005",
                        Descricao = "Telefone",

                    },

            };

            var formasAutorizacoes = _context.FormasAutorizacoes.ToList();

            if (formasAutorizacoes == null || formasAutorizacoes.Count() == 0 || formasAutorizacoes.Count() < listFormaAutorizacao.Count())
            {
                foreach (var item in formasAutorizacoes)
                {
                    var temp = listFormaAutorizacao.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listFormaAutorizacao.Remove(temp);
                    }
                }
                listFormaAutorizacao.ForEach(c => _context.FormasAutorizacoes.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region Tipo de Inventário

            var listTipoInventario = new List<TipoInventario>
            {
                new TipoInventario
                    {
                        Id = 1,
                        Codigo= "Conf",
                        Descricao = "Conferência",
                    },

                new TipoInventario
                    {
                        Id = 2,
                        Codigo= "Acert",
                        Descricao = "Acerto",
                    },
            };

            var tiposInventarios = _context.TiposInventarios.ToList();

            if (tiposInventarios == null || tiposInventarios.Count() == 0 || tiposInventarios.Count() < listTipoInventario.Count())
            {
                foreach (var item in formasAutorizacoes)
                {
                    var temp = listTipoInventario.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTipoInventario.Remove(temp);
                    }
                }
                listTipoInventario.ForEach(c => _context.TiposInventarios.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region Status do Inventário

            var listStatusInventario = new List<StatusInventario>
            {
                new StatusInventario
                    {
                        Id = 1,
                        Codigo= "Ini",
                        Descricao = "Inicial",
                    },
                new StatusInventario
                    {
                        Id = 4,
                        Codigo= "Fechado",
                        Descricao = "Fechado",
                    },
            };


            var statusInventarios = _context.StatusInventarios.ToList();

            if (statusInventarios == null || statusInventarios.Count() == 0 || statusInventarios.Count() != listStatusInventario.Count())
            {
                foreach (var item in statusInventarios)
                {
                    var temp = listStatusInventario.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listStatusInventario.Remove(temp);
                    }
                }
                listStatusInventario.ForEach(c => _context.StatusInventarios.Add(c));
                _context.SaveChanges();
            }


            #endregion

            #region Status do Inventário

            var listStatusInventarioItem = new List<StatusInventarioItem>
            {
                new StatusInventarioItem
                    {
                        Id = 1,
                        Codigo= "Ini",
                        Descricao = "Inicial",
                    },

                new StatusInventarioItem
                    {
                        Id = 2,
                        Codigo= "1º Cont",
                        Descricao = "1º Contagem",
                    },

                new StatusInventarioItem
                    {
                        Id = 3,
                        Codigo= "2º Cont",
                        Descricao = "2º Contagem",
                    },
                new StatusInventarioItem
                    {
                        Id = 4,
                        Codigo= "Fechado",
                        Descricao = "Fechado",
                    },
            };


            var statusInventarioItems = _context.StatusInventarioItems.ToList();

            if (statusInventarioItems == null || statusInventarioItems.Count() == 0 || statusInventarioItems.Count() != listStatusInventarioItem.Count())
            {
                foreach (var item in statusInventarioItems)
                {
                    if (listStatusInventarioItem.Where(m => m.Descricao == item.Descricao).FirstOrDefault() != null)
                    {
                        listStatusInventarioItem.Remove(listStatusInventarioItem.Where(m => m.Descricao == item.Descricao).FirstOrDefault());
                    }
                }
                listStatusInventarioItem.ForEach(c => _context.StatusInventarioItems.Add(c));
                _context.SaveChanges();
            }


            #endregion

            #region disparoDeMensagemItemTipo
            var listStatusDisparoDeMensagemItemTipo = new List<DisparoDeMensagemItemTipo>
            {
                new DisparoDeMensagemItemTipo
                    {
                        Id = 1,
                        Codigo= "E-mail",
                        Descricao = "E-mail",
                    },

                new DisparoDeMensagemItemTipo
                    {
                        Id = 2,
                        Codigo= "WhatsApp",
                        Descricao = "WhatsApp",
                    }
            };


            var disparoDeMensagemItemTipos = _context.DisparoDeMensagemItemTipo.ToList();

            if (disparoDeMensagemItemTipos == null || disparoDeMensagemItemTipos.Count() == 0 || disparoDeMensagemItemTipos.Count() < listStatusDisparoDeMensagemItemTipo.Count())
            {
                foreach (var item in disparoDeMensagemItemTipos)
                {
                    var temp = listStatusDisparoDeMensagemItemTipo.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listStatusDisparoDeMensagemItemTipo.Remove(temp);
                    }
                }
                listStatusDisparoDeMensagemItemTipo.ForEach(c => _context.DisparoDeMensagemItemTipo.Add(c));
                _context.SaveChanges();
            }

            #endregion

            #region TiposOcorrencia

            var listTipoOcorrencia = new List<TipoOcorrencia>
            {
                new TipoOcorrencia
                {
                    Id = TipoOcorrencia.ContaMedica,
                    Codigo= TipoOcorrencia.ContaMedica.ToString(),
                    Descricao = "Conta Médica",
                },
                new TipoOcorrencia
                {
                    Id = TipoOcorrencia.PrescricaoMedica,
                    Codigo= TipoOcorrencia.PrescricaoMedica.ToString(),
                    Descricao = "Prescrição",
                },
                new TipoOcorrencia
                {
                    Id = TipoOcorrencia.ResultadoExame,
                    Codigo= TipoOcorrencia.ResultadoExame.ToString(),
                    Descricao = "Laboratório Resultado",
                },
            };
            var tipoOcorrencias = _context.TipoOcorrencias.ToList();

            if (tipoOcorrencias == null || tipoOcorrencias.Count() == 0 || tipoOcorrencias.Count() < listTipoOcorrencia.Count())
            {
                foreach (var item in tipoOcorrencias)
                {
                    var temp = listTipoOcorrencia.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listTipoOcorrencia.Remove(temp);
                    }
                }
                listTipoOcorrencia.ForEach(c => _context.TipoOcorrencias.Add(c));
                _context.SaveChanges();
            }


            #endregion

            #region SubTipoOcorrencia

            var listSubTipoOcorrencia = new List<SubTipoOcorrencia>
            {
                new SubTipoOcorrencia
                {
                    Id = SubTipoOcorrencia.ContaMedicaItem,
                    Codigo= SubTipoOcorrencia.ContaMedicaItem.ToString(),
                    TipoOcorrenciaId = TipoOcorrencia.ContaMedica,
                    Descricao = "Conta Médica Item",
                },
                new SubTipoOcorrencia
                {
                    Id = SubTipoOcorrencia.ContaMedicaKit,
                    Codigo= SubTipoOcorrencia.ContaMedicaKit.ToString(),
                    TipoOcorrenciaId = TipoOcorrencia.ContaMedica,
                    Descricao = "Conta Médica Kit",
                },
                new SubTipoOcorrencia
                {
                    Id = SubTipoOcorrencia.ContaMedicaPacote,
                    Codigo= SubTipoOcorrencia.ContaMedicaPacote.ToString(),
                    TipoOcorrenciaId = TipoOcorrencia.ContaMedica,
                    Descricao = "Conta Médica Pacote",
                },
            };
            var subTipoOcorrencias = _context.SubTipoOcorrencias.ToList();

            if (subTipoOcorrencias == null || subTipoOcorrencias.Count() == 0 || subTipoOcorrencias.Count() < listSubTipoOcorrencia.Count())
            {
                foreach (var item in subTipoOcorrencias)
                {
                    var temp = listSubTipoOcorrencia.Where(m => m.Descricao == item.Descricao).FirstOrDefault();
                    if (temp != null)
                    {
                        listSubTipoOcorrencia.Remove(temp);
                    }
                }
                listSubTipoOcorrencia.ForEach(c => _context.SubTipoOcorrencias.Add(c));
                _context.SaveChanges();
            }


            #endregion


            _context.SaveChanges();
        }
    }


    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex(@"FROM\s+(?<table>.+)\s+AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }

}
