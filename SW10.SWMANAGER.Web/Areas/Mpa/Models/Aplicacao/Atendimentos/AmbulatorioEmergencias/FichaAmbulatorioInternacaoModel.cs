using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AmbulatorioEmergencias
{
    public class FichaAmbulatorioInternacaoModel
    {
        public string Paciente { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public string EmpresaRazaoSocial { get; set; }
        public string DataHora { get; set; }
        public string CodigoPaciente { get; set; }
        public string DataAtendimento { get; set; }
        public string Sexo { get; set; }
        public string Nascimento { get; set; }
        public string Identidade { get; set; }
        public string Cpf { get; set; }
        public string EstadoCivil { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string Profissao { get; set; }
        public string Numero { get; set; }
        public string Pais { get; set; }
        public string Cep { get; set; }
        public string CodigoAtendimento { get; set; }
        public string DataAlta { get; set; }
        public string Alta { get; set; }
        public string Matricula { get; set; }
        public string Validade { get; set; }
        public string DataPagto { get; set; }
        public string IdAcompanhante { get; set; }
        public string CodDep { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Nacionalidade { get; set; }
        public string Filiacao { get; set; }
        public string Medico { get; set; }
        public string Especialidade { get; set; }
        public string IndicadoPor { get; set; }
        public string Origem { get; set; }
        public string Tratamento { get; set; }
        public string Convenio { get; set; }
        public string Plano { get; set; }
        public string Guia { get; set; }
        public string NumeroGuia { get; set; }
        public string Titular { get; set; }
        public string ModeloTexto { get; set; }
        public string Cid { get; set; }
        public string Contrato { get; set; }
        public string CodInternacao { get; set; }
        public string Acompanhante { get; set; }
        public string Responsavel { get; set; }
        public string Leito { get; set; }
        public string Senha { get; set; }
        public string DiasAutorizados { get; set; }


        public static FichaAmbulatorioInternacaoModel MapearFromAtendimento(AtendimentoDto atendimento, VisitanteDto visitante, string modeloTexto = "", string nomeUsusuario = "")
        {
            var ficha = new FichaAmbulatorioInternacaoModel();

            ficha.Paciente = atendimento.Paciente?.NomeCompleto;
            ficha.Empresa = atendimento.Empresa?.NomeFantasia;
            ficha.EmpresaRazaoSocial = atendimento.Empresa?.RazaoSocial;
            ficha.DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");//atendimento.DataRegistro.ToString("dd/MM/yyyy hh:mm");
            ficha.CodigoPaciente = atendimento.Paciente?.CodigoPaciente.ToString();
            ficha.DataAtendimento = atendimento.DataRegistro.ToString("dd/MM/yyyy HH:mm");
            ficha.Sexo = atendimento.Paciente?.Sexo != null ? atendimento.Paciente?.Sexo?.Descricao : atendimento.Paciente?.SisPessoa?.Sexo?.Descricao;
            ficha.Nascimento = !atendimento.Paciente.Nascimento.HasValue ? "" : ((DateTime)atendimento.Paciente.Nascimento).ToString("dd/MM/yyyy");
            ficha.Identidade = atendimento.Paciente?.Rg;
            ficha.Cpf = atendimento.Paciente?.Cpf;
            ficha.EstadoCivil = atendimento?.Paciente?.EstadoCivil != null ? atendimento.Paciente?.EstadoCivil?.Descricao : atendimento.Paciente?.SisPessoa?.EstadoCivil?.Descricao;
            ficha.Complemento = atendimento?.Paciente?.Complemento != null ? atendimento.Paciente?.Complemento : atendimento.Paciente?.SisPessoa?.Enderecos?[0].Complemento;
            ficha.Pais = atendimento.Paciente?.Pais != null && atendimento.Paciente?.Pais.Id != 0 ? atendimento.Paciente?.Pais?.Nome : ((atendimento.Paciente?.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente?.SisPessoa?.Enderecos?[0].Pais?.Nome : "");
            ficha.Estado = atendimento.Paciente?.Estado != null  ? atendimento.Paciente?.Estado.Nome : "";
            ficha.Cidade = atendimento.Paciente?.Cidade != null && atendimento.Paciente?.Cidade.Id != 0 ? atendimento.Paciente?.Cidade?.Nome : ((atendimento.Paciente?.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente?.SisPessoa.Enderecos?[0].Cidade?.Nome : "");
            ficha.Endereco = atendimento.Paciente?.Logradouro != null ? atendimento.Paciente?.Logradouro : ((atendimento.Paciente?.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente?.SisPessoa?.Enderecos?[0].Logradouro : "");
            ficha.Bairro = atendimento.Paciente?.Bairro != null ? atendimento.Paciente?.Bairro : ((atendimento.Paciente?.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente?.SisPessoa?.Enderecos?[0].Bairro : "");
            ficha.Nacionalidade = atendimento.Paciente?.Nacionalidade != null ? atendimento.Paciente?.Nacionalidade?.Descricao : atendimento.Paciente?.SisPessoa?.Nacionalidade?.Descricao;
            ficha.Cep = atendimento.Paciente?.Cep != null ? atendimento.Paciente?.Cep : atendimento.Paciente?.SisPessoa?.Enderecos?[0].Cep;
            ficha.Telefone = atendimento.Paciente?.Telefone1;
            ficha.Profissao = atendimento.Paciente?.Profissao != null ? atendimento.Paciente?.Profissao?.Descricao : atendimento.Paciente?.SisPessoa?.Profissao?.Descricao;
            ficha.Numero = atendimento.Paciente?.Numero;
            ficha.CodigoAtendimento = atendimento.Codigo;
            ficha.DataAlta = atendimento.DataAlta?.ToString();
            ficha.Alta = "";
            ficha.Matricula = atendimento.Matricula;
            ficha.DataPagto = atendimento.DataUltimoPagamento?.ToString("dd/MM/yyyy");
            //ficha.IdAcompanhante = atendimento.RgResponsavel;
            ficha.IdAcompanhante = atendimento.TipoAcompanhante?.Codigo;
            ficha.CodDep = atendimento.CodDependente;
            ficha.Filiacao = string.Concat(atendimento.Paciente?.NomeMae, " - ", atendimento.Paciente?.NomePai);
            ficha.Medico = atendimento.Medico?.NomeCompleto;
            ficha.Especialidade = atendimento.Especialidade?.Descricao;
            ficha.IndicadoPor = "";
            ficha.Origem = atendimento.Origem?.Descricao;
            ficha.Tratamento = "";
            ficha.Convenio = atendimento.Convenio?.NomeFantasia;
            ficha.Plano = atendimento.Plano?.Descricao;
            ficha.Guia = atendimento.FatGuia?.Descricao;
            ficha.NumeroGuia = atendimento.GuiaNumero;
            ficha.Titular = atendimento.Titular;
            ficha.ModeloTexto = modeloTexto;

            ficha.Cid = "";
            ficha.Contrato = atendimento.TipoAcomodacao?.Descricao;
            //ficha.Acompanhante = "";
            ficha.Acompanhante = atendimento.TipoAcompanhante?.Descricao;//descrição
            ficha.Responsavel = atendimento.Responsavel;
            ficha.CodInternacao = atendimento.Codigo;
            ficha.Leito = atendimento.Leito?.Descricao != null ? atendimento.Leito?.Descricao : visitante?.Leito?.Descricao;
            ficha.Validade = atendimento.ValidadeCarteira != null ? ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy") : null;
            ficha.Senha = atendimento.Senha;
            ficha.DiasAutorizados = atendimento.DiasAutorizacao.ToString();

            ficha.Usuario = nomeUsusuario;

            return ficha;
        }

        public List<string> Lista { get; set; }

        public FichaAmbulatorioInternacaoModel()
        {
            Lista = new List<string>();
        }

        public static string ParameterParse(FichaAmbulatorioInternacaoModel dados, TextoModeloDto modeloTexto, IAtendimentoAppService _atendimentoAppService, AtendimentoDto atendimento, string UUID)
        {
            #region [Parametros]
            List<Atendimento> ultimos2Atendimentos = null;
            DateTime dt = new DateTime();
            int numeroIdade = 0;
            string idade = "";
            if (dados.Nascimento != "" && dados.Nascimento != null)
            {
                dt = DateTime.Parse(dados.Nascimento);
                numeroIdade = FuncoesGlobais.CalcularIdade(dt);
                idade = numeroIdade.ToString() + (numeroIdade == 1 ? " ano" : " anos");
            }

            string url = System.Web.HttpContext.Current.Request.Url.Authority;

            StringBuilder sb = new StringBuilder();
            sb.Append(dados.ModeloTexto);

            var htmlParameter = sb
                    .Replace("@EmpresaRazaoSocial", dados.EmpresaRazaoSocial)
                    .Replace("@Empresa", dados.Empresa)
                    .Replace("@DataHora", dados.DataHora)
                    .Replace("@CodigoPaciente", dados.CodigoPaciente)
                    .Replace("@Endereco", dados.Endereco)
                    .Replace("@Numero", dados.Numero)
                    .Replace("@Complemento", dados.Complemento)
                    .Replace("@Bairro", dados.Bairro)
                    .Replace("@Estado", dados.Estado)
                    .Replace("@Cidade", dados.Cidade)
                    .Replace("@Telefone", dados.Telefone)
                    .Replace("@Cep", dados.Cep)
                    .Replace("@Pais", dados.Pais)
                    .Replace("@Nacionalidade", dados.Nacionalidade)
                    .Replace("@Filiacao", dados.Filiacao)
                    .Replace("@Sexo", dados.Sexo)
                    .Replace("@Profissao", dados.Profissao)
                    .Replace("@Paciente", dados.Paciente)
                    .Replace("@Nascimento", dados.Nascimento)
                    .Replace("@Idade", idade.ToString())
                    .Replace("@Cpf", dados.Cpf)
                    .Replace("@SituacaoCivil", dados.EstadoCivil)
                    .Replace("@DataAtendimento", dados.DataAtendimento)
                    .Replace("@Identidade", dados.Identidade)
                    .Replace("@Medico", dados.Medico)
                    //.Replace("@Crm", dados.Crm)
                    .Replace("@Especialidade", dados.Especialidade)
                    .Replace("@IndicadoPor", dados.IndicadoPor)
                    .Replace("@Origem", dados.Origem)
                    .Replace("@Tratamento", dados.Tratamento)
                    .Replace("@Convenio", dados.Convenio)
                    .Replace("@Plano", dados.Plano)
                    .Replace("@Guia", dados.Guia)
                    .Replace("@NumberGuide", dados.NumeroGuia)
                    .Replace("@Titular", dados.Titular)
                    .Replace("@CodigoAtendimento", dados.CodigoAtendimento)
                    .Replace("@DataAlta", dados.DataAlta)
                    .Replace("@Alta", dados.Alta)
                    .Replace("@Matricula", dados.Matricula)
                    //.Replace("@Validade", dados.Validade)
                    .Replace("@Cid", dados.Cid)
                    .Replace("@DataPagto", dados.DataPagto)
                    .Replace("@IdAcompanhante", dados.IdAcompanhante)
                    .Replace("@CodDep", dados.CodDep)
                    .Replace("@Usuario", dados.Usuario)
                    .Replace("@Contrato", dados.Contrato)
                    .Replace("@Acompanhante", dados.Acompanhante)
                    .Replace("@Responsavel", dados.Responsavel)
                    .Replace("@CodInternacao", dados.CodInternacao)
                    .Replace("@Leito", dados.Leito)
                    .Replace("@DataValidade", dados.Validade)
                    .Replace("@Senha", dados.Senha)
                    .Replace("@DiasAutorizados", dados.DiasAutorizados)
                    .Replace("@CodigoBarra", "<img src=http://" + url + "/Temp/" + UUID + " height=20 width=200></img >")
                    .ToString();

            var htmlParameterDynamic = string.Empty;
            var indexFist = htmlParameter.IndexOf("@[");
            var indexLast = htmlParameter.IndexOf("@]");

            if (modeloTexto.IsMostraAtendimento)
            {
                ultimos2Atendimentos = _atendimentoAppService.ListarUltimos2Atendimentos(atendimento.PacienteId);

                var count = 1;
                foreach (var item in ultimos2Atendimentos)
                {
                    htmlParameter = htmlParameter
                  .Replace("@" + count + "DtHoraAtendimento", item.DataRegistro.ToString())
                  .Replace("@" + count + "MedicoAtendimento", item.Medico.NomeCompleto)
                  .Replace("@" + count + "ConvenioAtendimento", item.Convenio.NomeFantasia)
                  .Replace("@" + count + "Espec", item.Especialidade.Descricao);
                    count++;
                }

                if (indexFist != -1 || indexLast != -1)
                    htmlParameterDynamic = htmlParameter.Replace("@[", "").Replace("@]", "");

            }
            else
            {
                if (indexFist != -1 || indexLast != -1)
                    htmlParameterDynamic = string.Concat(htmlParameter.Substring(0, indexFist), htmlParameter.Substring(indexLast + 2));
            }

            if (htmlParameterDynamic == "")
            {
                htmlParameterDynamic = htmlParameter;
            }

            return htmlParameterDynamic;
            #endregion
        }
    }
}