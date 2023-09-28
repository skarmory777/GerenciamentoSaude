using SW10.SWMANAGER.ADORepositorio.Base;
using SW10.SWMANAGER.ClassesAplicacao.VisualASA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ADORepositorio.Repositorios
{
    public class RepositoriosAsa
    {
        private static SqlConnection conn;
        private static SqlCommand cmd;
        private static StringBuilder strSql;

        private static async Task<string> ObterCodigo(string tabela, string cnAsa)
        {
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings[cnAsa].ConnectionString;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetNextKeyValue";
            cmd.Parameters.AddWithValue("@KeyName", tabela.ToUpper());
            cmd.Connection = conn;
            conn.Open();
            var cod = await cmd.ExecuteScalarAsync();
            return cod.ToString();
        }

        public class SisPessoaRepositorio : IRepositorio<Sis_Pessoa>
        {
            private string _cnAsa;
            public SisPessoaRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }
            public async Task Alterar(Sis_Pessoa input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_PESSOA SET ");
                    strSql.AppendFormat("IDPESSOATIPO={0}", input.IDPessoaTipo);
                    //strSql.AppendFormat(",CODPESSOA='{0}'", input.CodPessoa);
                    strSql.AppendFormat(",PESSOA='{0}'", input.Pessoa);
                    strSql.AppendFormat(",ENDERECO='{0}'", string.IsNullOrWhiteSpace(input.Endereco) ? "NULL" : input.Endereco);
                    strSql.AppendFormat(",COMPLEMENTO='{0}'", string.IsNullOrWhiteSpace(input.Endereco) ? "NULL" : input.Complemento);
                    strSql.AppendFormat(",IDBAIRRO={0}", input.IDBairro.HasValue ? input.IDBairro.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDCIDADE={0}", input.IDCidade.HasValue ? input.IDCidade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDESTADO={0}", input.IDEstado.HasValue ? input.IDEstado.Value.ToString() : "NULL");
                    strSql.AppendFormat(",PAIS={0}", string.IsNullOrWhiteSpace(input.Pais) ? "NULL" : input.Pais);
                    strSql.AppendFormat(",CEP='{0}'", string.IsNullOrWhiteSpace(input.CEP) ? "NULL" : input.CEP);
                    strSql.AppendFormat(",NASCIMENTO={0}{1}{0}", input.Nascimento.HasValue ? "'" : string.Empty, input.Nascimento.HasValue ? input.Nascimento.Value.ToString("yyyyMMdd HH:mm") : "NULL"); //'{0}'", input.Nascimento.HasValue ? input.Nascimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",SEXO='{0}'", string.IsNullOrWhiteSpace(input.Sexo) ? "NULL" : input.Sexo);
                    strSql.AppendFormat(",ESTADOCIVIL={0}", string.IsNullOrWhiteSpace(input.EstadoCivil) ? "NULL" : input.EstadoCivil);
                    strSql.AppendFormat(",IDINSTRUCAO={0}", input.IDInstrucao.HasValue ? input.IDInstrucao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDPROFISSAO={0}", input.IDProfissao.HasValue ? input.IDProfissao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDCOBRANCA={0}", input.IDCobranca.HasValue ? input.IDCobranca.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDMEIOPAGAMENTO={0}", input.IDMeioPagamento.HasValue ? input.IDMeioPagamento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDDOCUMENTOTIPO={0}", input.IDDocumentoTipo.HasValue ? input.IDDocumentoTipo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDCENTROCUSTOLOCAL={0}", input.IDCentroCustoLocal.HasValue ? input.IDCentroCustoLocal.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDNATURALIDADE={0}", input.IDNaturalidade.HasValue ? input.IDNaturalidade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",NACIONALIDADE='{0}'", string.IsNullOrWhiteSpace(input.Nacionalidade) ? "NULL" : input.Nacionalidade);
                    strSql.AppendFormat(",IDENTIDADE='{0}'", string.IsNullOrWhiteSpace(input.Identidade) ? "NULL" : input.Identidade);
                    strSql.AppendFormat(",ORGAOEMISSOR='{0}'", string.IsNullOrWhiteSpace(input.OrgaoEmissor) ? "NULL" : input.OrgaoEmissor);
                    strSql.AppendFormat(",EMISSAOIDENTIDADE={0}", input.EmissaoIdentidade.HasValue ? input.EmissaoIdentidade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",CPF='{0}'", string.IsNullOrWhiteSpace(input.CPF) ? "NULL" : input.CPF.Replace(".", "").Replace("-", "").Replace(" ", ""));
                    strSql.AppendFormat(",RAZAOSOCIAL='{0}'", string.IsNullOrWhiteSpace(input.RazaoSocial) ? "NULL" : input.RazaoSocial);
                    strSql.AppendFormat(",NOMINAL='{0}'", string.IsNullOrWhiteSpace(input.Nominal) ? "NULL" : input.Nominal);
                    strSql.AppendFormat(",CGC='{0}'", string.IsNullOrWhiteSpace(input.CGC) ? "NULL" : input.CGC);
                    strSql.AppendFormat(",INSCRICAOESTADUAL='{0}'", string.IsNullOrWhiteSpace(input.InscricaoEstadual) ? "NULL" : input.InscricaoEstadual);
                    strSql.AppendFormat(",INSCRICAOMUNICIPAL='{0}'", string.IsNullOrWhiteSpace(input.InscricaoMunicipal) ? "NULL" : input.InscricaoMunicipal);
                    strSql.AppendFormat(",HOMEPAGE={0}", string.IsNullOrWhiteSpace(input.HomePage) ? "NULL" : input.HomePage);
                    strSql.AppendFormat(",JURIDICO={0}", string.IsNullOrWhiteSpace(input.InscricaoEstadual) ? "0" : input.InscricaoEstadual);
                    strSql.AppendFormat(",ISRECOLHEISS={0}", string.IsNullOrWhiteSpace(input.IsRecolheISS) ? "0" : input.IsRecolheISS);
                    strSql.AppendFormat(",TOTALQUITADO={0}", string.IsNullOrWhiteSpace(input.TotalQuitado) ? "0" : input.TotalQuitado);
                    strSql.AppendFormat(",SALDOATUAL={0}", string.IsNullOrWhiteSpace(input.SaldoAtual) ? "0" : input.SaldoAtual);
                    strSql.AppendFormat(",TOTALPREVISTO={0}", string.IsNullOrWhiteSpace(input.TotalPrevisto) ? "0" : input.TotalPrevisto);
                    strSql.AppendFormat(",NUMEROLANCAMENTOS={0}", input.NumeroLancamentos.HasValue ? input.NumeroLancamentos.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAULTIMOLANCAMENTO={0}{1}{0}", input.DataUltimoLancamento.HasValue ? "'" : string.Empty, input.DataUltimoLancamento.HasValue ? input.DataUltimoLancamento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DATAINCLUSAO={0}{1}{0}", input.DataInclusao.HasValue ? "'" : string.Empty, input.DataInclusao.HasValue ? input.DataInclusao.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",IDUSUARIOINCLUSAO={0}", input.IDUsuarioInclusao.HasValue ? input.IDUsuarioInclusao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAULTIMAALTERACAO='{0}'", input.DataUltimaAlteracao.HasValue ? input.DataUltimaAlteracao.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",IDUSUARIOALTERACAO={0}", input.IDUsuarioAlteracao.HasValue ? input.IDUsuarioAlteracao.Value : 0);
                    strSql.AppendFormat(",ISMALADIRETA={0}", input.IsMalaDireta.HasValue ? (object)input.IsMalaDireta.Value : 0);
                    strSql.AppendFormat(",ISSINCRONIZADO={0}", input.IsSincronizado.HasValue ? (object)input.IsSincronizado.Value : 0);
                    strSql.AppendFormat(",ISALTERADO={0}", input.IsAlterado.HasValue ? (object)input.IsAlterado.Value : 0);
                    strSql.AppendFormat(",IDSW={0}", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DESATIVADO={0}", input.Desativado.HasValue ? (object)input.Desativado.Value : 0);
                    strSql.AppendFormat(",SYSTEM={0}", input.System.HasValue ? (object)input.System.Value : 0);
                    strSql.AppendFormat(",HIDDEN={0}", input.Hidden.HasValue ? (object)input.Hidden.Value : 0);
                    strSql.AppendFormat(",IDBANCO1={0}", input.IDBanco1.HasValue ? input.IDBanco1.Value.ToString() : "NULL");
                    strSql.AppendFormat(",AGENCIA1={0}", string.IsNullOrWhiteSpace(input.Agencia1) ? "NULL" : input.Agencia1);
                    strSql.AppendFormat(",CONTACORRENTE1={0}", string.IsNullOrWhiteSpace(input.ContaCorrente1) ? "NULL" : input.ContaCorrente1);
                    strSql.AppendFormat(",IDBANCO2={0}", input.IDBanco2.HasValue ? input.IDBanco2.Value.ToString() : "NULL");
                    strSql.AppendFormat(",AGENCIA2={0}", string.IsNullOrWhiteSpace(input.Agencia2) ? "NULL" : input.Agencia2);
                    strSql.AppendFormat(",CONTACORRENTE2={0}", string.IsNullOrWhiteSpace(input.ContaCorrente2) ? "NULL" : input.ContaCorrente2);
                    strSql.AppendFormat(",IDFILIALSIN={0}", input.IDFilialSin.HasValue ? input.IDFilialSin.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDTIPOLOGRADOURO={0}", input.IDTipoLogradouro.HasValue ? input.IDTipoLogradouro.Value.ToString() : "NULL");
                    strSql.AppendFormat(",CONTAPADRAO={0}", input.ContaPadrao.HasValue ? input.ContaPadrao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAEXCLUSAO={0}{1}{0}", input.DataExclusao.HasValue ? "'" : string.Empty, input.DataExclusao.HasValue ? input.DataExclusao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOEXCLUSAO={0}", input.IDUsuarioExclusao.HasValue ? input.IDUsuarioExclusao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",NUMERO={0}", string.IsNullOrWhiteSpace(input.Numero) ? "NULL" : input.Numero);
                    strSql.AppendFormat(",IDCNAE={0}", input.IDCNAE.HasValue ? input.IDCNAE.Value.ToString() : "NULL");
                    strSql.AppendFormat(",TITULAR1={0}", string.IsNullOrWhiteSpace(input.Titular1) ? "NULL" : input.Titular1);
                    strSql.AppendFormat(",TITULAR2={0}", string.IsNullOrWhiteSpace(input.Titular2) ? "NULL" : input.Titular2);
                    strSql.AppendFormat(",ISFUNCIONARIO={0}", input.IsFuncionario.HasValue ? (object)input.IsFuncionario.Value : 0);
                    strSql.AppendFormat(",ISAGENDATEL={0}", input.IsAgendaTel.HasValue ? (object)input.IsAgendaTel.Value : 0);
                    strSql.AppendFormat(",FOTO='{0}'", input.Foto);
                    strSql.AppendFormat(",OBSPESSOA={0}", string.IsNullOrWhiteSpace(input.ObsPessoa) ? "NULL" : input.ObsPessoa);
                    //strSql.AppendFormat(",IDEXTERNO={0}", input.IDExterno.HasValue ? input.IDExterno.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",IDNFDESCRICAO={0} ", input.IDNFDescricao.HasValue ? input.IDNFDescricao.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",ID={0} ", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL");
                    strSql.AppendFormat(" WHERE IDPESSOA={0}", input.IDPessoa);

                    conn = new SqlConnection();
                    //conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_PESSOA SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDPESSOA=@IDPESSOA ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDPESSOA", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Sis_Pessoa input)
            {
                strSql = new StringBuilder();
                try
                {
                    int result;
                    if (string.IsNullOrWhiteSpace(input.CodPessoa) && (string.IsNullOrWhiteSpace(input.CodPessoa) && input.CodPessoa != "0"))
                    {
                        var cod = await ObterCodigo("SIS_PESSOA", _cnAsa); // await cmd.ExecuteScalarAsync();
                        input.CodPessoa = cod.ToString();
                    }
                    strSql.AppendFormat("INSERT INTO SIS_PESSOA(");
                    strSql.AppendFormat("IDPESSOATIPO,CODPESSOA,PESSOA,ENDERECO,COMPLEMENTO,IDBAIRRO,IDCIDADE,IDESTADO,");
                    strSql.AppendFormat("PAIS,CEP,NASCIMENTO,SEXO,ESTADOCIVIL,IDINSTRUCAO,IDPROFISSAO,IDCOBRANCA,IDCONTATESOURARIA,");
                    strSql.AppendFormat("IDMEIOPAGAMENTO,IDDOCUMENTOTIPO,IDCENTROCUSTOLOCAL,IDNATURALIDADE,NACIONALIDADE,");
                    strSql.AppendFormat("IDENTIDADE,ORGAOEMISSOR,EMISSAOIDENTIDADE,CPF,RAZAOSOCIAL,NOMINAL,CGC,INSCRICAOESTADUAL,");
                    strSql.AppendFormat("INSCRICAOMUNICIPAL,HOMEPAGE,JURIDICO,ISRECOLHEISS,TOTALQUITADO,SALDOATUAL,TOTALPREVISTO,");
                    strSql.AppendFormat("NUMEROLANCAMENTOS,DATAULTIMOLANCAMENTO,DATAINCLUSAO,IDUSUARIOINCLUSAO,DATAULTIMAALTERACAO,");
                    strSql.AppendFormat("IDUSUARIOALTERACAO,ISMALADIRETA,ISSINCRONIZADO,ISALTERADO,DESATIVADO,");
                    strSql.AppendFormat("SYSTEM,HIDDEN,IDBANCO1,AGENCIA1,CONTACORRENTE1,IDBANCO2,AGENCIA2,CONTACORRENTE2,");
                    strSql.AppendFormat("IDFILIALSIN,IDTIPOLOGRADOURO,CONTAPADRAO,DATAEXCLUSAO,IDUSUARIOEXCLUSAO,NUMERO,IDCNAE,");
                    strSql.AppendFormat("TITULAR1,TITULAR2,ISFUNCIONARIO,ISAGENDATEL,FOTO,OBSPESSOA, IDSW"); //,IDEXTERNO,IDNFDESCRICAO");
                    strSql.AppendFormat(") ");
                    //strSql.AppendFormat("output INSERTED.IDPESSOA ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.IDPessoaTipo.HasValue ? input.IDPessoaTipo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.CodPessoa);
                    strSql.AppendFormat(",'{0}'", input.Pessoa.Replace("'", "''"));
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Endereco) ? "NULL" : "'" + input.Endereco + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Complemento) ? "NULL" : "'" + input.Complemento + "'");
                    strSql.AppendFormat(",{0}", input.IDBairro.HasValue ? input.IDBairro.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDCidade.HasValue ? input.IDCidade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDEstado.HasValue ? input.IDEstado.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Pais) ? "NULL" : "'" + input.Pais + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.CEP) ? "NULL" : "'" + input.CEP + "'");
                    strSql.AppendFormat(",{0}{1}{0}", input.Nascimento.HasValue ? "'" : string.Empty, input.Nascimento.HasValue ? input.Nascimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",'{0}'", string.IsNullOrWhiteSpace(input.Sexo) ? "M" : input.Sexo);
                    strSql.AppendFormat(",'{0}'", string.IsNullOrWhiteSpace(input.EstadoCivil) ? "O" : "'" + input.EstadoCivil + "'");
                    strSql.AppendFormat(",{0}", input.IDInstrucao.HasValue ? input.IDInstrucao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDProfissao.HasValue ? input.IDProfissao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDCobranca.HasValue ? input.IDCobranca.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDContaTesouraria.HasValue ? input.IDContaTesouraria.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDMeioPagamento.HasValue ? input.IDMeioPagamento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDDocumentoTipo.HasValue ? input.IDDocumentoTipo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDCentroCustoLocal.HasValue ? input.IDCentroCustoLocal.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDNaturalidade.HasValue ? input.IDNaturalidade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Nacionalidade) ? "NULL" : "'" + input.Nacionalidade + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Identidade) ? "NULL" : "'" + input.Identidade + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.OrgaoEmissor) ? "NULL" : "'" + input.OrgaoEmissor + "'");
                    strSql.AppendFormat(",{0}{1}{0}", input.EmissaoIdentidade.HasValue ? "'" : string.Empty, input.EmissaoIdentidade.HasValue ? input.EmissaoIdentidade.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.CPF) ? "NULL" : "'" + input.CPF + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.RazaoSocial) ? "NULL" : "'" + input.RazaoSocial + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Nominal) ? "NULL" : "'" + input.Nominal + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.CGC) ? "NULL" : "'" + input.CGC + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.InscricaoEstadual) ? "NULL" : "'" + input.InscricaoEstadual + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.InscricaoMunicipal) ? "NULL" : "'" + input.InscricaoMunicipal + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.HomePage) ? "NULL" : "'" + input.HomePage + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Juridico) ? "0" : "'" + input.Juridico + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.IsRecolheISS) ? "0" : input.IsRecolheISS);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.TotalQuitado) ? "0" : "'" + input.TotalQuitado + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.SaldoAtual) ? "0" : "'" + input.SaldoAtual + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.TotalPrevisto) ? "0" : "'" + input.TotalPrevisto + "'");
                    strSql.AppendFormat(",{0}", input.NumeroLancamentos.HasValue ? input.NumeroLancamentos.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataUltimoLancamento.HasValue ? "'" : string.Empty, input.DataUltimoLancamento.HasValue ? input.DataUltimoLancamento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",'{0}'", input.DataInclusao.HasValue ? input.DataInclusao.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",{0}", input.IDUsuarioInclusao.HasValue ? input.IDUsuarioInclusao.Value : 0);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataUltimaAlteracao.HasValue ? "'" : string.Empty, input.DataUltimaAlteracao.HasValue ? input.DataUltimaAlteracao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioAlteracao.HasValue ? input.IDUsuarioAlteracao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsMalaDireta.HasValue ? (object)input.IsMalaDireta.Value : 0);
                    strSql.AppendFormat(",{0}", input.IsSincronizado.HasValue ? (object)input.IsSincronizado.Value : 0);
                    strSql.AppendFormat(",{0}", input.IsAlterado.HasValue ? (object)input.IsAlterado.Value : 0);

                    strSql.AppendFormat(",{0}", input.Desativado.HasValue ? (object)input.Desativado.Value : 0);
                    strSql.AppendFormat(",{0}", input.System.HasValue ? (object)input.System.Value : 0);
                    strSql.AppendFormat(",{0}", input.Hidden.HasValue ? (object)input.Hidden.Value : 0);
                    strSql.AppendFormat(",{0}", input.IDBanco1.HasValue ? input.IDBanco1.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Agencia1) ? "NULL" : "'" + input.Agencia1 + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.ContaCorrente1) ? "0" : "'" + input.ContaCorrente1 + "'");
                    strSql.AppendFormat(",{0}", input.IDBanco2.HasValue ? input.IDBanco2.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Agencia2) ? "NULL" : "'" + input.Agencia2 + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.ContaCorrente2) ? "NULL" : "'" + input.ContaCorrente2 + "'");
                    strSql.AppendFormat(",{0}", input.IDFilialSin.HasValue ? input.IDFilialSin.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDTipoLogradouro.HasValue ? input.IDTipoLogradouro.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.ContaPadrao.HasValue ? input.ContaPadrao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataExclusao.HasValue ? "'" : string.Empty, input.DataExclusao.HasValue ? input.DataExclusao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioExclusao.HasValue ? input.IDUsuarioExclusao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Numero) ? "NULL" : "'" + input.Numero + "'");
                    strSql.AppendFormat(",{0}", input.IDCNAE.HasValue ? input.IDCNAE.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Titular1) ? "NULL" : "'" + input.Titular1 + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Titular2) ? "NULL" : "'" + input.Titular2 + "'");
                    strSql.AppendFormat(",{0}", input.IsFuncionario.HasValue ? (object)input.IsFuncionario.Value : 0);
                    strSql.AppendFormat(",{0}", input.IsAgendaTel.HasValue ? (object)input.IsAgendaTel.Value : 0);
                    strSql.AppendFormat(",'{0}'", input.Foto);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.ObsPessoa) ? "NULL" : "'" + input.ObsPessoa + "'");
                    //strSql.AppendFormat(",{0}", input.IDExterno.HasValue ? input.IDExterno.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}", input.IDNFDescricao.HasValue ? input.IDNFDescricao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL");
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }
                    var record = await Obter(input.IDSW.Value.ToString());
                    //  result = record.IDPessoa.Value;
                    return record != null ? record.IDPessoa.Value : 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Sis_Pessoa>> Listar()
            {
                var result = new List<Sis_Pessoa>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDPESSOA,IDPESSOATIPO,CODPESSOA,PESSOA,ENDERECO,COMPLEMENTO,IDBAIRRO,IDCIDADE,IDESTADO,");
                strSql.AppendFormat("PAIS,CEP,NASCIMENTO,SEXO,ESTADOCIVIL,IDINSTRUCAO,IDPROFISSAO,IDCOBRANCA,IDCONTATESOURARIA,");
                strSql.AppendFormat("IDMEIOPAGAMENTO,IDDOCUMENTOTIPO,IDCENTROCUSTOLOCAL,IDNATURALIDADE,NACIONALIDADE,");
                strSql.AppendFormat("IDENTIDADE,ORGAOEMISSOR,EMISSAOIDENTIDADE,CPF,RAZAOSOCIAL,NOMINAL,CGC,INSCRICAOESTADUAL,");
                strSql.AppendFormat("INSCRICAOMUNICIPAL,HOMEPAGE,JURIDICO,ISRECOLHEISS,TOTALQUITADO,SALDOATUAL,TOTALPREVISTO,");
                strSql.AppendFormat("NUMEROLANCAMENTOS,DATAULTIMOLANCAMENTO,DATAINCLUSAO,IDUSUARIOINCLUSAO,DATAULTIMAALTERACAO,");
                strSql.AppendFormat("IDUSUARIOALTERACAO,ISMALADIRETA,ISSINCRONIZADO,ISALTERADO,DESATIVADO,");
                strSql.AppendFormat("SYSTEM,HIDDEN,IDBANCO1,AGENCIA1,CONTACORRENTE1,IDBANCO2,AGENCIA2,CONTACORRENTE2,");
                strSql.AppendFormat("IDFILIALSIN,IDTIPOLOGRADOURO,CONTAPADRAO,DATAEXCLUSAO,IDUSUARIOEXCLUSAO,NUMERO,IDCNAE,");
                strSql.AppendFormat("TITULAR1,TITULAR2,ISFUNCIONARIO,ISAGENDATEL,FOTO,OBSPESSOA"); //,IDEXTERNO,IDNFDESCRICAO ");
                strSql.AppendFormat("FROM SIS_PESSOA ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Sis_Pessoa();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPESSOATIPO"].ToString()))
                        {
                            item.IDPessoaTipo = Convert.ToInt32(listDb["IDPESSOATIPO"]);
                        }
                        item.CodPessoa = listDb["CODPESSOA"].ToString();
                        item.Pessoa = listDb["PESSOA"].ToString();
                        item.Endereco = listDb["ENDERECO"].ToString();
                        item.Complemento = listDb["COMPLEMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBAIRRO"].ToString()))
                        {
                            item.IDBairro = Convert.ToInt32(listDb["IDBAIRRO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDADE"].ToString()))
                        {
                            item.IDCidade = Convert.ToInt32(listDb["IDCIDADE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADO"].ToString()))
                        {
                            item.IDEstado = Convert.ToInt32(listDb["IDESTADO"]);
                        }
                        item.Pais = listDb["PAIS"].ToString();
                        item.CEP = listDb["CEP"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["NASCIMENTO"].ToString()))
                        {
                            item.Nascimento = Convert.ToDateTime(listDb["NASCIMENTO"]);
                        }
                        item.Sexo = listDb["SEXO"].ToString();
                        item.EstadoCivil = listDb["ESTADOCIVIL"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDINSTRUCAO"].ToString()))
                        {
                            item.IDInstrucao = Convert.ToInt32(listDb["IDINSTRUCAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPROFISSAO"].ToString()))
                        {
                            item.IDProfissao = Convert.ToInt32(listDb["IDPROFISSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCOBRANCA"].ToString()))
                        {
                            item.IDCobranca = Convert.ToInt32(listDb["IDCOBRANCA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONTATESOURARIA"].ToString()))
                        {
                            item.IDContaTesouraria = Convert.ToInt32(listDb["IDCONTATESOURARIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEIOPAGAMENTO"].ToString()))
                        {
                            item.IDMeioPagamento = Convert.ToInt32(listDb["IDMEIOPAGAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDDOCUMENTOTIPO"].ToString()))
                        {
                            item.IDDocumentoTipo = Convert.ToInt32(listDb["IDDOCUMENTOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCENTROCUSTOLOCAL"].ToString()))
                        {
                            item.IDCentroCustoLocal = Convert.ToInt32(listDb["IDCENTROCUSTOLOCAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDNATURALIDADE"].ToString()))
                        {
                            item.IDNaturalidade = Convert.ToInt32(listDb["IDNATURALIDADE"]);
                        }
                        item.Nacionalidade = listDb["NACIONALIDADE"].ToString();
                        item.Identidade = listDb["IDENTIDADE"].ToString();
                        item.OrgaoEmissor = listDb["ORGAOEMISSOR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["EMISSAOIDENTIDADE"].ToString()))
                        {
                            item.EmissaoIdentidade = Convert.ToDateTime(listDb["EMISSAOIDENTIDADE"]);
                        }
                        item.CPF = listDb["CPF"].ToString();
                        item.RazaoSocial = listDb["RAZAOSOCIAL"].ToString();
                        item.Nominal = listDb["NOMINAL"].ToString();
                        item.CGC = listDb["CGC"].ToString();
                        item.InscricaoEstadual = listDb["INSCRICAOESTADUAL"].ToString();
                        item.InscricaoMunicipal = listDb["INSCRICAOMUNICIPAL"].ToString();
                        item.HomePage = listDb["HOMEPAGE"].ToString();
                        item.Juridico = listDb["JURIDICO"].ToString();
                        item.IsRecolheISS = listDb["ISRECOLHEISS"].ToString();
                        item.TotalQuitado = listDb["TOTALQUITADO"].ToString();
                        item.SaldoAtual = listDb["SALDOATUAL"].ToString();
                        item.TotalPrevisto = listDb["TOTALPREVISTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["NUMEROLANCAMENTOS"].ToString()))
                        {
                            item.NumeroLancamentos = Convert.ToInt32(listDb["NUMEROLANCAMENTOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMOLANCAMENTO"].ToString()))
                        {
                            item.DataUltimoLancamento = Convert.ToDateTime(listDb["DATAULTIMOLANCAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINCLUSAO"].ToString()))
                        {
                            item.DataInclusao = Convert.ToDateTime(listDb["DATAINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOINCLUSAO"].ToString()))
                        {
                            item.IDUsuarioInclusao = Convert.ToInt32(listDb["IDUSUARIOINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMAALTERACAO"].ToString()))
                        {
                            item.DataUltimaAlteracao = Convert.ToDateTime(listDb["DATAULTIMAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISMALADIRETA"].ToString()))
                        {
                            item.IsMalaDireta = Convert.ToBoolean(listDb["ISMALADIRETA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["DESATIVADO"].ToString()))
                        {
                            item.Desativado = Convert.ToBoolean(listDb["DESATIVADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SYSTEM"].ToString()))
                        {
                            item.System = Convert.ToBoolean(listDb["SYSTEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["HIDDEN"].ToString()))
                        {
                            item.Hidden = Convert.ToBoolean(listDb["HIDDEN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDBANCO1"].ToString()))
                        {
                            item.IDBanco1 = Convert.ToInt32(listDb["IDBANCO1"]);
                        }
                        item.Agencia1 = listDb["AGENCIA1"].ToString();
                        item.ContaCorrente1 = listDb["CONTACORRENTE1"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBANCO2"].ToString()))
                        {
                            item.IDBanco2 = Convert.ToInt32(listDb["IDBANCO2"]);
                        }
                        item.Agencia2 = listDb["AGENCIA2"].ToString();
                        item.ContaCorrente2 = listDb["CONTACORRENTE2"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDTIPOLOGRADOURO"].ToString()))
                        {
                            item.IDTipoLogradouro = Convert.ToInt32(listDb["IDTIPOLOGRADOURO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["CONTAPADRAO"].ToString()))
                        {
                            item.ContaPadrao = Convert.ToInt32(listDb["CONTAPADRAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAEXCLUSAO"].ToString()))
                        {
                            item.DataExclusao = Convert.ToDateTime(listDb["DATAEXCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOEXCLUSAO"].ToString()))
                        {
                            item.IDUsuarioExclusao = Convert.ToInt32(listDb["IDUSUARIOEXCLUSAO"]);
                        }
                        item.Numero = listDb["NUMERO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDCNAE"].ToString()))
                        {
                            item.IDCNAE = Convert.ToInt32(listDb["IDCNAE"]);
                        }
                        item.Titular1 = listDb["TITULAR1"].ToString();
                        item.Titular2 = listDb["TITULAR2"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["ISFUNCIONARIO"].ToString()))
                        {
                            item.IsFuncionario = Convert.ToBoolean(listDb["ISFUNCIONARIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAGENDATEL"].ToString()))
                        {
                            item.IsAgendaTel = Convert.ToBoolean(listDb["ISAGENDATEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["FOTO"].ToString()))
                        {
                            item.Foto = (byte[])listDb["FOTO"];
                        }
                        item.ObsPessoa = listDb["OBSPESSOA"].ToString();
                        //if (!string.IsNullOrWhiteSpace(listDb["IDEXTERNO"].ToString()))
                        //{
                        //    item.IDExterno = Convert.ToInt32(listDb["IDEXTERNO"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDNFDESCRICAO"].ToString()))
                        //{
                        //    item.IDNFDescricao = Convert.ToInt32(listDb["IDNFDESCRICAO"]);
                        //}
                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Sis_Pessoa> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDPESSOA,IDPESSOATIPO,CODPESSOA,PESSOA,ENDERECO,COMPLEMENTO,IDBAIRRO,IDCIDADE,IDESTADO,");
                strSql.AppendFormat("PAIS,CEP,NASCIMENTO,SEXO,ESTADOCIVIL,IDINSTRUCAO,IDPROFISSAO,IDCOBRANCA,IDCONTATESOURARIA,");
                strSql.AppendFormat("IDMEIOPAGAMENTO,IDDOCUMENTOTIPO,IDCENTROCUSTOLOCAL,IDNATURALIDADE,NACIONALIDADE,");
                strSql.AppendFormat("IDENTIDADE,ORGAOEMISSOR,EMISSAOIDENTIDADE,CPF,RAZAOSOCIAL,NOMINAL,CGC,INSCRICAOESTADUAL,");
                strSql.AppendFormat("INSCRICAOMUNICIPAL,HOMEPAGE,JURIDICO,ISRECOLHEISS,TOTALQUITADO,SALDOATUAL,TOTALPREVISTO,");
                strSql.AppendFormat("NUMEROLANCAMENTOS,DATAULTIMOLANCAMENTO,DATAINCLUSAO,IDUSUARIOINCLUSAO,DATAULTIMAALTERACAO,");
                strSql.AppendFormat("IDUSUARIOALTERACAO,ISMALADIRETA,ISSINCRONIZADO,ISALTERADO,DESATIVADO,");
                strSql.AppendFormat("SYSTEM,HIDDEN,IDBANCO1,AGENCIA1,CONTACORRENTE1,IDBANCO2,AGENCIA2,CONTACORRENTE2,");
                strSql.AppendFormat("IDFILIALSIN,IDTIPOLOGRADOURO,CONTAPADRAO,DATAEXCLUSAO,IDUSUARIOEXCLUSAO,NUMERO,IDCNAE,");
                strSql.AppendFormat("TITULAR1,TITULAR2,ISFUNCIONARIO,ISAGENDATEL,FOTO,OBSPESSOA");//,IDEXTERNO,IDNFDESCRICAO ");
                strSql.AppendFormat("FROM SIS_PESSOA ");
                strSql.AppendFormat("WHERE IDPESSOA=@IDPESSOA ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    cmd.Parameters.AddWithValue("@IDPESSOA", id);
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    var item = new Sis_Pessoa();
                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPESSOATIPO"].ToString()))
                        {
                            item.IDPessoaTipo = Convert.ToInt32(listDb["IDPESSOATIPO"]);
                        }
                        item.IDPessoa = Convert.ToInt32(listDb["IDPESSOA"]);
                        item.CodPessoa = listDb["CODPESSOA"].ToString();
                        item.Pessoa = listDb["PESSOA"].ToString();
                        item.Endereco = listDb["ENDERECO"].ToString();
                        item.Complemento = listDb["COMPLEMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBAIRRO"].ToString()))
                        {
                            item.IDBairro = Convert.ToInt32(listDb["IDBAIRRO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDADE"].ToString()))
                        {
                            item.IDCidade = Convert.ToInt32(listDb["IDCIDADE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADO"].ToString()))
                        {
                            item.IDEstado = Convert.ToInt32(listDb["IDESTADO"]);
                        }
                        item.Pais = listDb["PAIS"].ToString();
                        item.CEP = listDb["CEP"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["NASCIMENTO"].ToString()))
                        {
                            item.Nascimento = Convert.ToDateTime(listDb["NASCIMENTO"]);
                        }
                        item.Sexo = listDb["SEXO"].ToString();
                        item.EstadoCivil = listDb["ESTADOCIVIL"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDINSTRUCAO"].ToString()))
                        {
                            item.IDInstrucao = Convert.ToInt32(listDb["IDINSTRUCAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPROFISSAO"].ToString()))
                        {
                            item.IDProfissao = Convert.ToInt32(listDb["IDPROFISSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCOBRANCA"].ToString()))
                        {
                            item.IDCobranca = Convert.ToInt32(listDb["IDCOBRANCA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONTATESOURARIA"].ToString()))
                        {
                            item.IDContaTesouraria = Convert.ToInt32(listDb["IDCONTATESOURARIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEIOPAGAMENTO"].ToString()))
                        {
                            item.IDMeioPagamento = Convert.ToInt32(listDb["IDMEIOPAGAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDDOCUMENTOTIPO"].ToString()))
                        {
                            item.IDDocumentoTipo = Convert.ToInt32(listDb["IDDOCUMENTOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCENTROCUSTOLOCAL"].ToString()))
                        {
                            item.IDCentroCustoLocal = Convert.ToInt32(listDb["IDCENTROCUSTOLOCAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDNATURALIDADE"].ToString()))
                        {
                            item.IDNaturalidade = Convert.ToInt32(listDb["IDNATURALIDADE"]);
                        }
                        item.Nacionalidade = listDb["NACIONALIDADE"].ToString();
                        item.Identidade = listDb["IDENTIDADE"].ToString();
                        item.OrgaoEmissor = listDb["ORGAOEMISSOR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["EMISSAOIDENTIDADE"].ToString()))
                        {
                            item.EmissaoIdentidade = Convert.ToDateTime(listDb["EMISSAOIDENTIDADE"]);
                        }
                        item.CPF = listDb["CPF"].ToString();
                        item.RazaoSocial = listDb["RAZAOSOCIAL"].ToString();
                        item.Nominal = listDb["NOMINAL"].ToString();
                        item.CGC = listDb["CGC"].ToString();
                        item.InscricaoEstadual = listDb["INSCRICAOESTADUAL"].ToString();
                        item.InscricaoMunicipal = listDb["INSCRICAOMUNICIPAL"].ToString();
                        item.HomePage = listDb["HOMEPAGE"].ToString();
                        item.Juridico = listDb["JURIDICO"].ToString();
                        item.IsRecolheISS = listDb["ISRECOLHEISS"].ToString();
                        item.TotalQuitado = listDb["TOTALQUITADO"].ToString();
                        item.SaldoAtual = listDb["SALDOATUAL"].ToString();
                        item.TotalPrevisto = listDb["TOTALPREVISTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["NUMEROLANCAMENTOS"].ToString()))
                        {
                            item.NumeroLancamentos = Convert.ToInt32(listDb["NUMEROLANCAMENTOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMOLANCAMENTO"].ToString()))
                        {
                            item.DataUltimoLancamento = Convert.ToDateTime(listDb["DATAULTIMOLANCAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINCLUSAO"].ToString()))
                        {
                            item.DataInclusao = Convert.ToDateTime(listDb["DATAINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOINCLUSAO"].ToString()))
                        {
                            item.IDUsuarioInclusao = Convert.ToInt32(listDb["IDUSUARIOINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMAALTERACAO"].ToString()))
                        {
                            item.DataUltimaAlteracao = Convert.ToDateTime(listDb["DATAULTIMAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISMALADIRETA"].ToString()))
                        {
                            item.IsMalaDireta = Convert.ToBoolean(listDb["ISMALADIRETA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["DESATIVADO"].ToString()))
                        {
                            item.Desativado = Convert.ToBoolean(listDb["DESATIVADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SYSTEM"].ToString()))
                        {
                            item.System = Convert.ToBoolean(listDb["SYSTEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["HIDDEN"].ToString()))
                        {
                            item.Hidden = Convert.ToBoolean(listDb["HIDDEN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDBANCO1"].ToString()))
                        {
                            item.IDBanco1 = Convert.ToInt32(listDb["IDBANCO1"]);
                        }
                        item.Agencia1 = listDb["AGENCIA1"].ToString();
                        item.ContaCorrente1 = listDb["CONTACORRENTE1"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBANCO2"].ToString()))
                        {
                            item.IDBanco2 = Convert.ToInt32(listDb["IDBANCO2"]);
                        }
                        item.Agencia2 = listDb["AGENCIA2"].ToString();
                        item.ContaCorrente2 = listDb["CONTACORRENTE2"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDTIPOLOGRADOURO"].ToString()))
                        {
                            item.IDTipoLogradouro = Convert.ToInt32(listDb["IDTIPOLOGRADOURO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["CONTAPADRAO"].ToString()))
                        {
                            item.ContaPadrao = Convert.ToInt32(listDb["CONTAPADRAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAEXCLUSAO"].ToString()))
                        {
                            item.DataExclusao = Convert.ToDateTime(listDb["DATAEXCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOEXCLUSAO"].ToString()))
                        {
                            item.IDUsuarioExclusao = Convert.ToInt32(listDb["IDUSUARIOEXCLUSAO"]);
                        }
                        item.Numero = listDb["NUMERO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDCNAE"].ToString()))
                        {
                            item.IDCNAE = Convert.ToInt32(listDb["IDCNAE"]);
                        }
                        item.Titular1 = listDb["TITULAR1"].ToString();
                        item.Titular2 = listDb["TITULAR2"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["ISFUNCIONARIO"].ToString()))
                        {
                            item.IsFuncionario = Convert.ToBoolean(listDb["ISFUNCIONARIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAGENDATEL"].ToString()))
                        {
                            item.IsAgendaTel = Convert.ToBoolean(listDb["ISAGENDATEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["FOTO"].ToString()))
                        {
                            item.Foto = (byte[])listDb["FOTO"];
                        }
                        item.ObsPessoa = listDb["OBSPESSOA"].ToString();
                        //if (!string.IsNullOrWhiteSpace(listDb["IDEXTERNO"].ToString()))
                        //{
                        //    item.IDExterno = Convert.ToInt32(listDb["IDEXTERNO"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDNFDESCRICAO"].ToString()))
                        //{
                        //    item.IDNFDescricao = Convert.ToInt32(listDb["IDNFDESCRICAO"]);
                        //}
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public async Task<Sis_Pessoa> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDPESSOA,IDPESSOATIPO,CODPESSOA,PESSOA,ENDERECO,COMPLEMENTO,IDBAIRRO,IDCIDADE,IDESTADO,");
                strSql.AppendFormat("PAIS,CEP,NASCIMENTO,SEXO,ESTADOCIVIL,IDINSTRUCAO,IDPROFISSAO,IDCOBRANCA,IDCONTATESOURARIA,");
                strSql.AppendFormat("IDMEIOPAGAMENTO,IDDOCUMENTOTIPO,IDCENTROCUSTOLOCAL,IDNATURALIDADE,NACIONALIDADE,");
                strSql.AppendFormat("IDENTIDADE,ORGAOEMISSOR,EMISSAOIDENTIDADE,CPF,RAZAOSOCIAL,NOMINAL,CGC,INSCRICAOESTADUAL,");
                strSql.AppendFormat("INSCRICAOMUNICIPAL,HOMEPAGE,JURIDICO,ISRECOLHEISS,TOTALQUITADO,SALDOATUAL,TOTALPREVISTO,");
                strSql.AppendFormat("NUMEROLANCAMENTOS,DATAULTIMOLANCAMENTO,DATAINCLUSAO,IDUSUARIOINCLUSAO,DATAULTIMAALTERACAO,");
                strSql.AppendFormat("IDUSUARIOALTERACAO,ISMALADIRETA,ISSINCRONIZADO,ISALTERADO,DESATIVADO,");
                strSql.AppendFormat("SYSTEM,HIDDEN,IDBANCO1,AGENCIA1,CONTACORRENTE1,IDBANCO2,AGENCIA2,CONTACORRENTE2,");
                strSql.AppendFormat("IDFILIALSIN,IDTIPOLOGRADOURO,CONTAPADRAO,DATAEXCLUSAO,IDUSUARIOEXCLUSAO,NUMERO,IDCNAE,");
                strSql.AppendFormat("TITULAR1,TITULAR2,ISFUNCIONARIO,ISAGENDATEL,FOTO,OBSPESSOA, IDSW ");//,IDEXTERNO,IDNFDESCRICAO ");
                strSql.AppendFormat("FROM SIS_PESSOA ");
                strSql.AppendFormat("WHERE IDSW={0}", id);
                strSql.AppendFormat(" ORDER BY DATAINCLUSAO DESC ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    //cmd.Parameters.AddWithValue("@IDSW", id);
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Pessoa();

                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPESSOATIPO"].ToString()))
                        {
                            item.IDPessoaTipo = Convert.ToInt32(listDb["IDPESSOATIPO"]);
                        }
                        item.IDPessoa = Convert.ToInt32(listDb["IDPESSOA"]);
                        item.CodPessoa = listDb["CODPESSOA"].ToString();
                        item.Pessoa = listDb["PESSOA"].ToString();
                        item.Endereco = listDb["ENDERECO"].ToString();
                        item.Complemento = listDb["COMPLEMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBAIRRO"].ToString()))
                        {
                            item.IDBairro = Convert.ToInt32(listDb["IDBAIRRO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDADE"].ToString()))
                        {
                            item.IDCidade = Convert.ToInt32(listDb["IDCIDADE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADO"].ToString()))
                        {
                            item.IDEstado = Convert.ToInt32(listDb["IDESTADO"]);
                        }
                        item.Pais = listDb["PAIS"].ToString();
                        item.CEP = listDb["CEP"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["NASCIMENTO"].ToString()))
                        {
                            item.Nascimento = Convert.ToDateTime(listDb["NASCIMENTO"]);
                        }
                        item.Sexo = listDb["SEXO"].ToString();
                        item.EstadoCivil = listDb["ESTADOCIVIL"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDINSTRUCAO"].ToString()))
                        {
                            item.IDInstrucao = Convert.ToInt32(listDb["IDINSTRUCAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPROFISSAO"].ToString()))
                        {
                            item.IDProfissao = Convert.ToInt32(listDb["IDPROFISSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCOBRANCA"].ToString()))
                        {
                            item.IDCobranca = Convert.ToInt32(listDb["IDCOBRANCA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONTATESOURARIA"].ToString()))
                        {
                            item.IDContaTesouraria = Convert.ToInt32(listDb["IDCONTATESOURARIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEIOPAGAMENTO"].ToString()))
                        {
                            item.IDMeioPagamento = Convert.ToInt32(listDb["IDMEIOPAGAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDDOCUMENTOTIPO"].ToString()))
                        {
                            item.IDDocumentoTipo = Convert.ToInt32(listDb["IDDOCUMENTOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCENTROCUSTOLOCAL"].ToString()))
                        {
                            item.IDCentroCustoLocal = Convert.ToInt32(listDb["IDCENTROCUSTOLOCAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDNATURALIDADE"].ToString()))
                        {
                            item.IDNaturalidade = Convert.ToInt32(listDb["IDNATURALIDADE"]);
                        }
                        item.Nacionalidade = listDb["NACIONALIDADE"].ToString();
                        item.Identidade = listDb["IDENTIDADE"].ToString();
                        item.OrgaoEmissor = listDb["ORGAOEMISSOR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["EMISSAOIDENTIDADE"].ToString()))
                        {
                            item.EmissaoIdentidade = Convert.ToDateTime(listDb["EMISSAOIDENTIDADE"]);
                        }
                        item.CPF = listDb["CPF"].ToString();
                        item.RazaoSocial = listDb["RAZAOSOCIAL"].ToString();
                        item.Nominal = listDb["NOMINAL"].ToString();
                        item.CGC = listDb["CGC"].ToString();
                        item.InscricaoEstadual = listDb["INSCRICAOESTADUAL"].ToString();
                        item.InscricaoMunicipal = listDb["INSCRICAOMUNICIPAL"].ToString();
                        item.HomePage = listDb["HOMEPAGE"].ToString();
                        item.Juridico = listDb["JURIDICO"].ToString();
                        item.IsRecolheISS = listDb["ISRECOLHEISS"].ToString();
                        item.TotalQuitado = listDb["TOTALQUITADO"].ToString();
                        item.SaldoAtual = listDb["SALDOATUAL"].ToString();
                        item.TotalPrevisto = listDb["TOTALPREVISTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["NUMEROLANCAMENTOS"].ToString()))
                        {
                            item.NumeroLancamentos = Convert.ToInt32(listDb["NUMEROLANCAMENTOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMOLANCAMENTO"].ToString()))
                        {
                            item.DataUltimoLancamento = Convert.ToDateTime(listDb["DATAULTIMOLANCAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINCLUSAO"].ToString()))
                        {
                            item.DataInclusao = Convert.ToDateTime(listDb["DATAINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOINCLUSAO"].ToString()))
                        {
                            item.IDUsuarioInclusao = Convert.ToInt32(listDb["IDUSUARIOINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMAALTERACAO"].ToString()))
                        {
                            item.DataUltimaAlteracao = Convert.ToDateTime(listDb["DATAULTIMAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISMALADIRETA"].ToString()))
                        {
                            item.IsMalaDireta = Convert.ToBoolean(listDb["ISMALADIRETA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["DESATIVADO"].ToString()))
                        {
                            item.Desativado = Convert.ToBoolean(listDb["DESATIVADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SYSTEM"].ToString()))
                        {
                            item.System = Convert.ToBoolean(listDb["SYSTEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["HIDDEN"].ToString()))
                        {
                            item.Hidden = Convert.ToBoolean(listDb["HIDDEN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDBANCO1"].ToString()))
                        {
                            item.IDBanco1 = Convert.ToInt32(listDb["IDBANCO1"]);
                        }
                        item.Agencia1 = listDb["AGENCIA1"].ToString();
                        item.ContaCorrente1 = listDb["CONTACORRENTE1"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBANCO2"].ToString()))
                        {
                            item.IDBanco2 = Convert.ToInt32(listDb["IDBANCO2"]);
                        }
                        item.Agencia2 = listDb["AGENCIA2"].ToString();
                        item.ContaCorrente2 = listDb["CONTACORRENTE2"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDTIPOLOGRADOURO"].ToString()))
                        {
                            item.IDTipoLogradouro = Convert.ToInt32(listDb["IDTIPOLOGRADOURO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["CONTAPADRAO"].ToString()))
                        {
                            item.ContaPadrao = Convert.ToInt32(listDb["CONTAPADRAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAEXCLUSAO"].ToString()))
                        {
                            item.DataExclusao = Convert.ToDateTime(listDb["DATAEXCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOEXCLUSAO"].ToString()))
                        {
                            item.IDUsuarioExclusao = Convert.ToInt32(listDb["IDUSUARIOEXCLUSAO"]);
                        }
                        item.Numero = listDb["NUMERO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDCNAE"].ToString()))
                        {
                            item.IDCNAE = Convert.ToInt32(listDb["IDCNAE"]);
                        }
                        item.Titular1 = listDb["TITULAR1"].ToString();
                        item.Titular2 = listDb["TITULAR2"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["ISFUNCIONARIO"].ToString()))
                        {
                            item.IsFuncionario = Convert.ToBoolean(listDb["ISFUNCIONARIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAGENDATEL"].ToString()))
                        {
                            item.IsAgendaTel = Convert.ToBoolean(listDb["ISAGENDATEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["FOTO"].ToString()))
                        {
                            item.Foto = (byte[])listDb["FOTO"];
                        }
                        item.ObsPessoa = listDb["OBSPESSOA"].ToString();
                        //if (!string.IsNullOrWhiteSpace(listDb["IDEXTERNO"].ToString()))
                        //{
                        //    item.IDExterno = Convert.ToInt32(listDb["IDEXTERNO"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDNFDESCRICAO"].ToString()))
                        //{
                        //    item.IDNFDescricao = Convert.ToInt32(listDb["IDNFDESCRICAO"]);
                        //}

                        if (!string.IsNullOrWhiteSpace(listDb["IDSW"].ToString()))
                        {
                            item.IDSW = Convert.ToInt32(listDb["IDSW"]);
                        }
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class SisAtendimentoRepositorio : IRepositorio<Sis_Atendimento>
        {
            private string _cnAsa;
            public SisAtendimentoRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }

            public async Task Alterar(Sis_Atendimento input)
            {
                strSql = new StringBuilder();
                try
                {
                    var medEsp = MedicoEspecialidade(input.IDEmpresa ?? 0, input.IDMedico ?? 0, input.IDEspecialidade ?? 0, _cnAsa);
                    input.IDMedico = medEsp.IdMedico;
                    input.IDEspecialidade = medEsp.IdEspecialidade;

                    strSql = new StringBuilder();

                    strSql.AppendFormat("UPDATE SIS_ATENDIMENTO SET ");
                    strSql.AppendFormat("IDEMPRESA={0}", input.IDEmpresa.HasValue ? input.IDEmpresa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDFILIAL={0}", input.IDFilial.HasValue ? input.IDFilial.Value.ToString() : "101");
                    strSql.AppendFormat(",IDCONVENIOATEND={0}", input.IDConvenioAtend.HasValue ? input.IDConvenioAtend.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDORIGEM={0}", input.IDOrigem.HasValue ? input.IDOrigem.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",CODATENDIMENTO='{0}'", input.CodAtendimento);
                    strSql.AppendFormat(",IDPACIENTE={0}", input.IDPaciente.HasValue ? input.IDPaciente.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAATENDIMENTO='{0}'", input.DataAtendimento.HasValue ? input.DataAtendimento.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",IDMEDICO={0}", input.IDMedico.HasValue ? input.IDMedico.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDESPECIALIDADE={0}", input.IDEspecialidade.HasValue ? input.IDEspecialidade.Value : 0);
                    strSql.AppendFormat(",IDMEDICOINDICA={0}", input.IDMedicoIndica.HasValue ? input.IDMedicoIndica.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDUSUARIOINCLUSAO={0}", input.IDUsuarioInclusao.HasValue ? input.IDUsuarioInclusao.Value : 0);
                    strSql.AppendFormat(",DATAINCLUSAO='{0}'", input.DataInclusao.HasValue ? input.DataInclusao.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",IDUSUARIOALTERACAO={0}", input.IDUsuarioAlteracao.HasValue ? input.IDUsuarioAlteracao.Value : 0);
                    strSql.AppendFormat(",DATAALTERACAO='{0}'", input.DataAlteracao.HasValue ? input.DataAlteracao.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",IDUSUARIOCANCELAMENTO={0}", input.IDUsuarioCancelamento.HasValue ? input.IDUsuarioCancelamento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATACANCELAMENTO={0}{1}{0}", input.DataCancelamento.HasValue ? "'" : string.Empty, input.DataCancelamento.HasValue ? input.DataCancelamento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDATEMOTCANCELAMENTO={0}", input.IDAteMotCancelamento.HasValue ? input.IDAteMotCancelamento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISSINCRONIZADO={0}", input.IsSincronizado.HasValue ? Convert.ToInt32(input.IsSincronizado) : 0);
                    strSql.AppendFormat(",ISALTERADO={0}", input.IsAlterado.HasValue ? Convert.ToInt32(input.IsAlterado) : 0);
                    strSql.AppendFormat(",IDSW={0}", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DESATIVADO={0}", input.Desativado.HasValue ? Convert.ToInt32(input.Desativado) : 0);
                    strSql.AppendFormat(",SYSTEM={0}", input.System.HasValue ? Convert.ToInt32(input.System) : 0);
                    strSql.AppendFormat(",HIDDEN={0}", input.Hidden.HasValue ? Convert.ToInt32(input.Hidden) : 0);
                    strSql.AppendFormat(",IDFILIALSIN={0}", input.IDFilialSin.HasValue ? input.IDFilialSin.Value.ToString() : "NULL");
                    strSql.AppendFormat(",AGUDACRONICA={0}", string.IsNullOrWhiteSpace(input.AgudaCronica) ? "NULL" : input.AgudaCronica);
                    strSql.AppendFormat(",PACIENTECAIXA={0}", string.IsNullOrWhiteSpace(input.PacienteCaixa) ? "NULL" : input.PacienteCaixa);
                    strSql.AppendFormat(",IDCLINICA={0}", input.IDClinica.HasValue ? input.IDClinica.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDATENDIMENTOSTATUS={0}", input.IDAtendimentoStatus.HasValue ? input.IDAtendimentoStatus.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDINDICADORACIDENTE={0}", input.IDIndicadorAcidente.HasValue ? input.IDIndicadorAcidente.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDREVISAOENTREGA={0}", input.IDRevisaoEntrega.HasValue ? input.IDRevisaoEntrega.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDUSUARIORECEBIMENTO={0}", input.IDUsuarioRecebimento.HasValue ? input.IDUsuarioRecebimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATARECEBIMENTO={0}{1}{0}", input.DataRecebimento.HasValue ? "'" : string.Empty, input.DataRecebimento.HasValue ? input.DataRecebimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOCANCELARECEBIMENTO={0}", input.IDUsuarioCancelaRecebimento.HasValue ? input.IDUsuarioCancelaRecebimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATACANCELARECEBIMENTO={0}{1}{0}", input.DataCancelaRecebimento.HasValue ? "'" : string.Empty, input.DataCancelaRecebimento.HasValue ? input.DataCancelaRecebimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",OBSRECEBIMENTO={0}", string.IsNullOrWhiteSpace(input.ObsRecebimento) ? "NULL" : input.ObsRecebimento);
                    strSql.AppendFormat(",IDUSUARIOOBSRECEBIMENTO={0}", input.IDUsuarioObsRecebimento.HasValue ? input.IDUsuarioObsRecebimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAOBSRECEBIMENTO={0}{1}{0}", input.DataObsRecebimento.HasValue ? "'" : string.Empty, input.DataObsRecebimento.HasValue ? input.DataObsRecebimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",ISINTERNOU={0}", input.IsInternou.HasValue ? Convert.ToInt32(input.IsInternou) : 0);
                    strSql.AppendFormat(",IDULTUSUCONFEMAIL={0}", input.IDUltUsuConfEmail.HasValue ? Convert.ToInt32(input.IDUltUsuConfEmail) : 0);
                    strSql.AppendFormat(",ISSMSENVIADO={0}", input.IsSMSEnviado.HasValue ? Convert.ToInt32(input.IsSMSEnviado) : 0);
                    strSql.AppendFormat(",ISSMSCONFIRMADO={0}", input.IsSMSConfirmado.HasValue ? Convert.ToInt32(input.IsSMSConfirmado) : 0);
                    strSql.AppendFormat(",IDMEDICOCONSULTA={0}", input.IDMedicoConsulta.HasValue ? input.IDMedicoConsulta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAMEDICOCONSULTA={0}{1}{0}", input.DataMedicoConsulta.HasValue ? "'" : string.Empty, input.DataMedicoConsulta.HasValue ? input.DataMedicoConsulta.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",MES='{0}'", input.Mes);
                    strSql.AppendFormat(",ANO='{0}'", input.Ano);
                    strSql.AppendFormat(",IDADE='{0}'", input.Idade);
                    strSql.AppendFormat(",JUSTIFICATIVANUMDECLNASCVIVO={0}", string.IsNullOrWhiteSpace(input.JustificativaNumDeclNascVivo) ? "NULL" : input.JustificativaNumDeclNascVivo);
                    strSql.AppendFormat(",IDATENDIMENTOINICIAL={0}", input.IDAtendimentoInicial.HasValue ? input.IDAtendimentoInicial.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATARETORNO={0}{1}{0}", input.DataRetorno.HasValue ? "'" : string.Empty, input.DataRetorno.HasValue ? input.DataRetorno.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",OBSRETORNO='{0}'",  (input.ObsRetorno==null || input.ObsRetorno.Length==0)? null: input.ObsRetorno);// VERIFICAR O CAMPO COM MARCUS
                    strSql.AppendFormat(",IDUSUARIORETORNO={0}", input.IDUsuarioRetorno.HasValue ? input.IDUsuarioRetorno.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISENCAMINHADO={0}", input.IsEncaminhado.HasValue ? input.IsEncaminhado.Value.ToString() : "0");
                    //strSql.AppendFormat(",DATACONCLUSAO={0}{1}{0}", input.DataConclusao.HasValue ? "'" : string.Empty, input.DataConclusao.HasValue ? input.DataConclusao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",IDESPECIALIDADEMEDINDICA={0}", input.IDEspecialidadeMedIndica.HasValue ? input.IDEspecialidadeMedIndica.Value.ToString() : "NULL");
                    strSql.AppendFormat(" WHERE IDATENDIMENTO={0}", input.IDAtendimento);

                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    conn.Open();
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_ATENDIMENTO SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDATENDIMENTO=@IDATENDIMENTO ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDATENDIMENTO", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Sis_Atendimento input)
            {
                strSql = new StringBuilder();
                try
                {
                    var medEsp = MedicoEspecialidade(input.IDEmpresa ?? 0, input.IDMedico ?? 0, input.IDEspecialidade ?? 0, _cnAsa);
                    input.IDMedico = medEsp.IdMedico;
                    input.IDEspecialidade = medEsp.IdEspecialidade;
                    if (string.IsNullOrWhiteSpace(input.CodAtendimento) && (string.IsNullOrWhiteSpace(input.CodAtendimento) && input.CodAtendimento != "0"))
                    {
                        var cod = await ObterCodigo("SIS_ATENDIMENTO", _cnAsa); // await cmd.ExecuteScalarAsync();
                        input.CodAtendimento = cod.ToString();
                    }
                    strSql = new StringBuilder();
                    strSql.AppendFormat("INSERT INTO SIS_ATENDIMENTO(");
                    strSql.AppendFormat("IDEMPRESA, IDFILIAL, IDCONVENIOATEND, IDORIGEM, CODATENDIMENTO, IDPACIENTE, ");
                    //strSql.AppendFormat("IDEMPRESA, IDFILIAL, IDCONVENIOATEND, IDORIGEM, IDPACIENTE, ");
                    strSql.AppendFormat("DATAATENDIMENTO, IDMEDICO, IDESPECIALIDADE, IDMEDICOINDICA, IDUSUARIOINCLUSAO, DATAINCLUSAO, ");
                    strSql.AppendFormat("IDUSUARIOALTERACAO, DATAALTERACAO, IDUSUARIOCANCELAMENTO, DATACANCELAMENTO, IDATEMOTCANCELAMENTO, ");
                    strSql.AppendFormat("ISSINCRONIZADO, ISALTERADO, IDSW, DESATIVADO, SYSTEM, HIDDEN, IDFILIALSIN, AGUDACRONICA, ");
                    strSql.AppendFormat("PACIENTECAIXA, IDCLINICA, IDATENDIMENTOSTATUS, IDINDICADORACIDENTE, IDREVISAOENTREGA, ");
                    strSql.AppendFormat("IDUSUARIORECEBIMENTO, DATARECEBIMENTO, IDUSUARIOCANCELARECEBIMENTO, DATACANCELARECEBIMENTO, ");
                    strSql.AppendFormat("OBSRECEBIMENTO, IDUSUARIOOBSRECEBIMENTO, DATAOBSRECEBIMENTO, ISINTERNOU, IDULTUSUCONFEMAIL, ");
                    strSql.AppendFormat("ISSMSENVIADO, ISSMSCONFIRMADO, IDMEDICOCONSULTA, DATAMEDICOCONSULTA, MES, ANO, IDADE, ");
                    strSql.AppendFormat("JUSTIFICATIVANUMDECLNASCVIVO, IDATENDIMENTOINICIAL, DATARETORNO, OBSRETORNO, IDUSUARIORETORNO, ");
                    strSql.AppendFormat("ISENCAMINHADO "); //, DATACONCLUSAO, IDESPECIALIDADEMEDINDICA) 
                    strSql.AppendFormat(")");
                    //strSql.AppendFormat("output INSERTED.IDATENDIMENTO ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.IDEmpresa.HasValue ? input.IDEmpresa.Value.ToString() : "0");
                    strSql.AppendFormat(",{0}", input.IDFilial.HasValue ? input.IDFilial.Value.ToString() : "101");
                    strSql.AppendFormat(",{0}", input.IDConvenioAtend.HasValue ? input.IDConvenioAtend.Value.ToString() : "0");
                    strSql.AppendFormat(",{0}", input.IDOrigem.HasValue ? input.IDOrigem.Value.ToString() : "0");
                    //strSql.AppendFormat(",'{0}'", input.CodAtendimento);
                    strSql.AppendFormat(",{0}", "NULL");
                    strSql.AppendFormat(",{0}", input.IDPaciente.HasValue ? input.IDPaciente.Value.ToString() : "NULL");
                    strSql.AppendFormat(",'{0}'", input.DataAtendimento.HasValue ? input.DataAtendimento.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",{0}", input.IDMedico.HasValue ? input.IDMedico.Value.ToString() : "0");
                    strSql.AppendFormat(",{0}", input.IDEspecialidade.HasValue ? input.IDEspecialidade.Value.ToString() : "0");
                    strSql.AppendFormat(",{0}", input.IDMedicoIndica.HasValue ? input.IDMedicoIndica.Value.ToString() : "0");
                    strSql.AppendFormat(",{0}", input.IDUsuarioInclusao.HasValue ? input.IDUsuarioInclusao.Value : 0);
                    strSql.AppendFormat(",'{0}'", input.DataInclusao.HasValue ? input.DataInclusao.Value.ToString("yyyyMMdd HH:mm") : DateTime.Now.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",{0}", input.IDUsuarioAlteracao.HasValue ? input.IDMedicoIndica.Value.ToString() : "0");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAlteracao.HasValue ? "'" : string.Empty, input.DataAlteracao.HasValue ? input.DataAlteracao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioCancelamento.HasValue ? input.IDUsuarioCancelamento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataCancelamento.HasValue ? "'" : string.Empty, input.DataCancelamento.HasValue ? input.DataCancelamento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAteMotCancelamento.HasValue ? input.IDAteMotCancelamento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsSincronizado.HasValue ? Convert.ToInt32(input.IsSincronizado) : 0);
                    strSql.AppendFormat(",{0}", input.IsAlterado.HasValue ? Convert.ToInt32(input.IsAlterado) : 0);
                    strSql.AppendFormat(",{0}", input.IDSW.HasValue ? input.IDSW.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.Desativado.HasValue ? Convert.ToInt32(input.Desativado) : 0);
                    strSql.AppendFormat(",{0}", input.System.HasValue ? Convert.ToInt32(input.System) : 0);
                    strSql.AppendFormat(",{0}", input.Hidden.HasValue ? Convert.ToInt32(input.Hidden) : 0);
                    strSql.AppendFormat(",{0}", input.IDFilialSin.HasValue ? input.IDFilialSin.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.AgudaCronica) ? "NULL" : input.AgudaCronica);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.PacienteCaixa) ? "NULL" : input.PacienteCaixa);
                    strSql.AppendFormat(",{0}", input.IDClinica.HasValue ? input.IDClinica.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAtendimentoStatus.HasValue ? input.IDAtendimentoStatus.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDIndicadorAcidente.HasValue ? input.IDIndicadorAcidente.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDRevisaoEntrega.HasValue ? input.IDRevisaoEntrega.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioRecebimento.HasValue ? input.IDUsuarioRecebimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataRecebimento.HasValue ? "'" : string.Empty, input.DataRecebimento.HasValue ? input.DataRecebimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioCancelaRecebimento.HasValue ? input.IDUsuarioCancelaRecebimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataCancelaRecebimento.HasValue ? "'" : string.Empty, input.DataCancelaRecebimento.HasValue ? input.DataCancelaRecebimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.ObsRecebimento) ? "NULL" : input.ObsRecebimento);
                    strSql.AppendFormat(",{0}", input.IDUsuarioObsRecebimento.HasValue ? input.IDUsuarioObsRecebimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataObsRecebimento.HasValue ? "'" : string.Empty, input.DataObsRecebimento.HasValue ? input.DataObsRecebimento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IsInternou.HasValue ? Convert.ToInt32(input.IsInternou) : 0);
                    strSql.AppendFormat(",{0}", input.IDUltUsuConfEmail.HasValue ? Convert.ToInt32(input.IDUltUsuConfEmail) : 0);
                    strSql.AppendFormat(",{0}", input.IsSMSEnviado.HasValue ? Convert.ToInt32(input.IsSMSEnviado) : 0);
                    strSql.AppendFormat(",{0}", input.IsSMSConfirmado.HasValue ? Convert.ToInt32(input.IsSMSConfirmado) : 0);
                    strSql.AppendFormat(",{0}", input.IDMedicoConsulta.HasValue ? input.IDMedicoConsulta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataMedicoConsulta.HasValue ? "'" : string.Empty, input.DataMedicoConsulta.HasValue ? input.DataMedicoConsulta.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.Mes.HasValue ? input.Mes.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.Ano.HasValue ? input.Ano.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.Idade.HasValue ? input.Idade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.JustificativaNumDeclNascVivo) ? "NULL" : input.JustificativaNumDeclNascVivo);
                    strSql.AppendFormat(",{0}", input.IDAtendimentoInicial.HasValue ? input.IDAtendimentoInicial.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataRetorno.HasValue ? "'" : string.Empty, input.DataRetorno.HasValue ? input.DataRetorno.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",'{0}'", input.ObsRetorno);
                    strSql.AppendFormat(",{0}", input.IDUsuarioRetorno.HasValue ? input.IDUsuarioRetorno.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsEncaminhado.HasValue ? input.IsEncaminhado.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}{1}{0}", input.DataConclusao.HasValue ? "'" : string.Empty, input.DataConclusao.HasValue ? input.DataConclusao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",{0}", input.IDEspecialidadeMedIndica.HasValue ? input.IDEspecialidadeMedIndica.Value.ToString() : "NULL");
                    strSql.AppendFormat(")");

                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    conn.Open();
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {

                        await cmd.ExecuteNonQueryAsync();
                        cmd.Dispose();
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }

                    var record = await Obter(input.IDSW.ToString());
                    if (record != null)
                    {
                        var result = record.IDAtendimento.Value;
                        return result;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Sis_Atendimento>> Listar()
            {
                var result = new List<Sis_Atendimento>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDATENDIMENTO,IDEMPRESA, IDFILIAL, IDCONVENIOATEND, IDORIGEM, CODATENDIMENTO, IDPACIENTE, ");
                strSql.AppendFormat("DATAATENDIMENTO, IDMEDICO, IDESPECIALIDADE, IDMEDICOINDICA, IDUSUARIOINCLUSAO, DATAINCLUSAO, ");
                strSql.AppendFormat("IDUSUARIOALTERACAO, DATAALTERACAO, IDUSUARIOCANCELAMENTO, DATACANCELAMENTO, IDATEMOTCANCELAMENTO, ");
                strSql.AppendFormat("ISSINCRONIZADO, ISALTERADO, DESATIVADO, SYSTEM, HIDDEN, IDFILIALSIN, AGUDACRONICA, ");
                strSql.AppendFormat("PACIENTECAIXA, IDCLINICA, IDATENDIMENTOSTATUS, IDINDICADORACIDENTE, IDREVISAOENTREGA, ");
                strSql.AppendFormat("IDUSUARIORECEBIMENTO, DATARECEBIMENTO, IDUSUARIOCANCELARECEBIMENTO, DATACANCELARECEBIMENTO, ");
                strSql.AppendFormat("OBSRECEBIMENTO, IDUSUARIOOBSRECEBIMENTO, DATAOBSRECEBIMENTO, ISINTERNOU, IDULTUSUCONFEMAIL, ");
                strSql.AppendFormat("ISSMSENVIADO, ISSMSCONFIRMADO, IDMEDICOCONSULTA, DATAMEDICOCONSULTA, MES, ANO, IDADE, ");
                strSql.AppendFormat("JUSTIFICATIVANUMDECLNASCVIVO, IDATENDIMENTOINICIAL, DATARETORNO, OBSRETORNO, IDUSUARIORETORNO, ");
                strSql.AppendFormat("ISENCAMINHADO"); //, DATACONCLUSAO, IDESPECIALIDADEMEDINDICA ");
                strSql.AppendFormat("FROM SIS_ATENDIMENTO ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Sis_Atendimento();

                        item.IDAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESA"].ToString()))
                        {
                            item.IDEmpresa = Convert.ToInt32(listDb["IDEMPRESA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIAL"].ToString()))
                        {
                            item.IDFilial = Convert.ToInt32(listDb["IDFILIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONVENIOATEND"].ToString()))
                        {
                            item.IDConvenioAtend = Convert.ToInt32(listDb["IDCONVENIOATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDORIGEM"].ToString()))
                        {
                            item.IDOrigem = Convert.ToInt32(listDb["IDORIGEM"]);
                        }
                        item.CodAtendimento = listDb["CODATENDIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPACIENTE"].ToString()))
                        {
                            item.IDPaciente = Convert.ToInt32(listDb["IDPACIENTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAATENDIMENTO"].ToString()))
                        {
                            item.DataAtendimento = Convert.ToDateTime(listDb["DATAATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICO"].ToString()))
                        {
                            item.IDMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESPECIALIDADE"].ToString()))
                        {
                            item.IDEspecialidade = Convert.ToInt32(listDb["IDESPECIALIDADE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOINDICA"].ToString()))
                        {
                            item.IDMedicoIndica = Convert.ToInt32(listDb["IDMEDICOINDICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOINCLUSAO"].ToString()))
                        {
                            item.IDUsuarioInclusao = Convert.ToInt32(listDb["IDUSUARIOINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINCLUSAO"].ToString()))
                        {
                            item.DataInclusao = Convert.ToDateTime(listDb["DATAINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTERACAO"].ToString()))
                        {
                            item.DataAlteracao = Convert.ToDateTime(listDb["DATAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCANCELAMENTO"].ToString()))
                        {
                            item.IDUsuarioCancelamento = Convert.ToInt32(listDb["IDUSUARIOCANCELAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATACANCELAMENTO"].ToString()))
                        {
                            item.DataCancelamento = Convert.ToDateTime(listDb["DATACANCELAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATEMOTCANCELAMENTO"].ToString()))
                        {
                            item.IDAteMotCancelamento = Convert.ToInt32(listDb["IDATEMOTCANCELAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["DESATIVADO"].ToString()))
                        {
                            item.Desativado = Convert.ToBoolean(listDb["DESATIVADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SYSTEM"].ToString()))
                        {
                            item.System = Convert.ToBoolean(listDb["SYSTEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["HIDDEN"].ToString()))
                        {
                            item.Hidden = Convert.ToBoolean(listDb["HIDDEN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        item.AgudaCronica = listDb["PESSOA"].ToString();
                        item.PacienteCaixa = listDb["PESSOA"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDCLINICA"].ToString()))
                        {
                            item.IDClinica = Convert.ToInt32(listDb["IDCLINICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTOSTATUS"].ToString()))
                        {
                            item.IDAtendimentoStatus = Convert.ToInt32(listDb["IDATENDIMENTOSTATUS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDINDICADORACIDENTE"].ToString()))
                        {
                            item.IDIndicadorAcidente = Convert.ToInt32(listDb["IDINDICADORACIDENTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDREVISAOENTREGA"].ToString()))
                        {
                            item.IDRevisaoEntrega = Convert.ToInt32(listDb["IDREVISAOENTREGA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORECEBIMENTO"].ToString()))
                        {
                            item.IDUsuarioRecebimento = Convert.ToInt32(listDb["IDUSUARIORECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCANCELARECEBIMENTO"].ToString()))
                        {
                            item.IDUsuarioCancelaRecebimento = Convert.ToInt32(listDb["IDUSUARIOCANCELARECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATACANCELARECEBIMENTO"].ToString()))
                        {
                            item.DataCancelaRecebimento = Convert.ToDateTime(listDb["DATACANCELARECEBIMENTO"]);
                        }
                        item.ObsRecebimento = listDb["OBSRECEBIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOOBSRECEBIMENTO"].ToString()))
                        {
                            item.IDUsuarioObsRecebimento = Convert.ToInt32(listDb["IDUSUARIOOBSRECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAOBSRECEBIMENTO"].ToString()))
                        {
                            item.DataObsRecebimento = Convert.ToDateTime(listDb["DATAOBSRECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISINTERNOU"].ToString()))
                        {
                            item.IsInternou = Convert.ToBoolean(listDb["ISINTERNOU"]);
                        }
                        // TODO: Correção do tipo de dados 
                        // no visual asa está com o tipo TBitControl
                        //if (!string.IsNullOrWhiteSpace(listDb["IDULTUSUCONFEMAIL"].ToString()))
                        //{
                        //    item.IDUltUsuConfEmail = Convert.ToInt32(listDb["IDULTUSUCONFEMAIL"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["ISSMSENVIADO"].ToString()))
                        {
                            item.IsSMSEnviado = Convert.ToBoolean(listDb["ISSMSENVIADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSMSCONFIRMADO"].ToString()))
                        {
                            item.IsSMSConfirmado = Convert.ToBoolean(listDb["ISSMSCONFIRMADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOCONSULTA"].ToString()))
                        {
                            item.IDMedicoConsulta = Convert.ToInt32(listDb["IDMEDICOCONSULTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAMEDICOCONSULTA"].ToString()))
                        {
                            item.DataMedicoConsulta = Convert.ToDateTime(listDb["DATAMEDICOCONSULTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["MES"].ToString()))
                        {
                            item.Mes = Convert.ToInt32(listDb["MES"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ANO"].ToString()))
                        {
                            item.Ano = Convert.ToInt32(listDb["ANO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDADE"].ToString()))
                        {
                            item.Idade = Convert.ToInt32(listDb["IDADE"]);
                        }
                        item.JustificativaNumDeclNascVivo = listDb["JUSTIFICATIVANUMDECLNASCVIVO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTOINICIAL"].ToString()))
                        {
                            item.IDAtendimentoInicial = Convert.ToInt32(listDb["IDATENDIMENTOINICIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATARETORNO"].ToString()))
                        {
                            item.DataRetorno = Convert.ToDateTime(listDb["DATARETORNO"]);
                        }

                        item.ObsRetorno = (byte[])listDb["OBSRETORNO"];

                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORETORNO"].ToString()))
                        {
                            item.IDUsuarioRetorno = Convert.ToInt32(listDb["IDUSUARIORETORNO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISENCAMINHADO"].ToString()))
                        {
                            item.IsEncaminhado = Convert.ToInt32(listDb["ISENCAMINHADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATACONCLUSAO"].ToString()))
                        //{
                        //    item.DataConclusao = Convert.ToDateTime(listDb["DATACONCLUSAO"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDESPECIALIDADEMEDINDICA"].ToString()))
                        //{
                        //    item.IDEspecialidadeMedIndica = Convert.ToInt32(listDb["IDESPECIALIDADEMEDINDICA"]);
                        //}

                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Sis_Atendimento> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDATENDIMENTO,IDEMPRESA, IDFILIAL, IDCONVENIOATEND, IDORIGEM, CODATENDIMENTO, IDPACIENTE, ");
                strSql.AppendFormat("DATAATENDIMENTO, IDMEDICO, IDESPECIALIDADE, IDMEDICOINDICA, IDUSUARIOINCLUSAO, DATAINCLUSAO, ");
                strSql.AppendFormat("IDUSUARIOALTERACAO, DATAALTERACAO, IDUSUARIOCANCELAMENTO, DATACANCELAMENTO, IDATEMOTCANCELAMENTO, ");
                strSql.AppendFormat("ISSINCRONIZADO, ISALTERADO, IDSW, DESATIVADO, SYSTEM, HIDDEN, IDFILIALSIN, AGUDACRONICA, ");
                strSql.AppendFormat("PACIENTECAIXA, IDCLINICA, IDATENDIMENTOSTATUS, IDINDICADORACIDENTE, IDREVISAOENTREGA, ");
                strSql.AppendFormat("IDUSUARIORECEBIMENTO, DATARECEBIMENTO, IDUSUARIOCANCELARECEBIMENTO, DATACANCELARECEBIMENTO, ");
                strSql.AppendFormat("OBSRECEBIMENTO, IDUSUARIOOBSRECEBIMENTO, DATAOBSRECEBIMENTO, ISINTERNOU, IDULTUSUCONFEMAIL, ");
                strSql.AppendFormat("ISSMSENVIADO, ISSMSCONFIRMADO, IDMEDICOCONSULTA, DATAMEDICOCONSULTA, MES, ANO, IDADE, ");
                strSql.AppendFormat("JUSTIFICATIVANUMDECLNASCVIVO, IDATENDIMENTOINICIAL, DATARETORNO, OBSRETORNO, IDUSUARIORETORNO, ");
                strSql.AppendFormat("ISENCAMINHADO"); //, DATACONCLUSAO, IDESPECIALIDADEMEDINDICA ");
                strSql.AppendFormat(" FROM SIS_ATENDIMENTO ");
                strSql.AppendFormat(" WHERE IDATENDIMENTO={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                conn.Open();
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Atendimento();
                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        item.IDAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESA"].ToString()))
                        {
                            item.IDEmpresa = Convert.ToInt32(listDb["IDEMPRESA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIAL"].ToString()))
                        {
                            item.IDFilial = Convert.ToInt32(listDb["IDFILIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONVENIOATEND"].ToString()))
                        {
                            item.IDConvenioAtend = Convert.ToInt32(listDb["IDCONVENIOATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDORIGEM"].ToString()))
                        {
                            item.IDOrigem = Convert.ToInt32(listDb["IDORIGEM"]);
                        }
                        item.CodAtendimento = listDb["CODATENDIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPACIENTE"].ToString()))
                        {
                            item.IDPaciente = Convert.ToInt32(listDb["IDPACIENTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAATENDIMENTO"].ToString()))
                        {
                            item.DataAtendimento = Convert.ToDateTime(listDb["DATAATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICO"].ToString()))
                        {
                            item.IDMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESPECIALIDADE"].ToString()))
                        {
                            item.IDEspecialidade = Convert.ToInt32(listDb["IDESPECIALIDADE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOINDICA"].ToString()))
                        {
                            item.IDMedicoIndica = Convert.ToInt32(listDb["IDMEDICOINDICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOINCLUSAO"].ToString()))
                        {
                            item.IDUsuarioInclusao = Convert.ToInt32(listDb["IDUSUARIOINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINCLUSAO"].ToString()))
                        {
                            item.DataInclusao = Convert.ToDateTime(listDb["DATAINCLUSAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTERACAO"].ToString()))
                        {
                            item.DataAlteracao = Convert.ToDateTime(listDb["DATAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCANCELAMENTO"].ToString()))
                        {
                            item.IDUsuarioCancelamento = Convert.ToInt32(listDb["IDUSUARIOCANCELAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATACANCELAMENTO"].ToString()))
                        {
                            item.DataCancelamento = Convert.ToDateTime(listDb["DATACANCELAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATEMOTCANCELAMENTO"].ToString()))
                        {
                            item.IDAteMotCancelamento = Convert.ToInt32(listDb["IDATEMOTCANCELAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDSW"].ToString()))
                        {
                            item.IDSW = Convert.ToInt32(listDb["IDSW"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DESATIVADO"].ToString()))
                        {
                            item.Desativado = Convert.ToBoolean(listDb["DESATIVADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SYSTEM"].ToString()))
                        {
                            item.System = Convert.ToBoolean(listDb["SYSTEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["HIDDEN"].ToString()))
                        {
                            item.Hidden = Convert.ToBoolean(listDb["HIDDEN"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        item.AgudaCronica = listDb["AGUDACRONICA"].ToString();
                        item.PacienteCaixa = listDb["PACIENTECAIXA"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDCLINICA"].ToString()))
                        {
                            item.IDClinica = Convert.ToInt32(listDb["IDCLINICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTOSTATUS"].ToString()))
                        {
                            item.IDAtendimentoStatus = Convert.ToInt32(listDb["IDATENDIMENTOSTATUS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDINDICADORACIDENTE"].ToString()))
                        {
                            item.IDIndicadorAcidente = Convert.ToInt32(listDb["IDINDICADORACIDENTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDREVISAOENTREGA"].ToString()))
                        {
                            item.IDRevisaoEntrega = Convert.ToInt32(listDb["IDREVISAOENTREGA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORECEBIMENTO"].ToString()))
                        {
                            item.IDUsuarioRecebimento = Convert.ToInt32(listDb["IDUSUARIORECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCANCELARECEBIMENTO"].ToString()))
                        {
                            item.IDUsuarioCancelaRecebimento = Convert.ToInt32(listDb["IDUSUARIOCANCELARECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATACANCELARECEBIMENTO"].ToString()))
                        {
                            item.DataCancelaRecebimento = Convert.ToDateTime(listDb["DATACANCELARECEBIMENTO"]);
                        }
                        item.ObsRecebimento = listDb["OBSRECEBIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOOBSRECEBIMENTO"].ToString()))
                        {
                            item.IDUsuarioObsRecebimento = Convert.ToInt32(listDb["IDUSUARIOOBSRECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAOBSRECEBIMENTO"].ToString()))
                        {
                            item.DataObsRecebimento = Convert.ToDateTime(listDb["DATAOBSRECEBIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISINTERNOU"].ToString()))
                        {
                            item.IsInternou = Convert.ToBoolean(listDb["ISINTERNOU"]);
                        }
                        // TODO: Correção do tipo de dados 
                        // no visual asa está com o tipo TBitControl
                        //if (!string.IsNullOrWhiteSpace(listDb["IDULTUSUCONFEMAIL"].ToString()))
                        //{
                        //    item.IDUltUsuConfEmail = Convert.ToInt32(listDb["IDULTUSUCONFEMAIL"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["ISSMSENVIADO"].ToString()))
                        {
                            item.IsSMSEnviado = Convert.ToBoolean(listDb["ISSMSENVIADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSMSCONFIRMADO"].ToString()))
                        {
                            item.IsSMSConfirmado = Convert.ToBoolean(listDb["ISSMSCONFIRMADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOCONSULTA"].ToString()))
                        {
                            item.IDMedicoConsulta = Convert.ToInt32(listDb["IDMEDICOCONSULTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAMEDICOCONSULTA"].ToString()))
                        {
                            item.DataMedicoConsulta = Convert.ToDateTime(listDb["DATAMEDICOCONSULTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["MES"].ToString()))
                        {
                            item.Mes = Convert.ToInt32(listDb["MES"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ANO"].ToString()))
                        {
                            item.Ano = Convert.ToInt32(listDb["ANO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDADE"].ToString()))
                        {
                            item.Idade = Convert.ToInt32(listDb["IDADE"]);
                        }
                        item.JustificativaNumDeclNascVivo = listDb["JUSTIFICATIVANUMDECLNASCVIVO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTOINICIAL"].ToString()))
                        {
                            item.IDAtendimentoInicial = Convert.ToInt32(listDb["IDATENDIMENTOINICIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATARETORNO"].ToString()))
                        {
                            item.DataRetorno = Convert.ToDateTime(listDb["DATARETORNO"]);
                        }

                        item.ObsRetorno = (byte[])listDb["OBSRETORNO"];

                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORETORNO"].ToString()))
                        {
                            item.IDUsuarioRetorno = Convert.ToInt32(listDb["IDUSUARIORETORNO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISENCAMINHADO"].ToString()))
                        {
                            item.IsEncaminhado = Convert.ToInt32(listDb["ISENCAMINHADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATACONCLUSAO"].ToString()))
                        //{
                        //    item.DataConclusao = Convert.ToDateTime(listDb["DATACONCLUSAO"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDESPECIALIDADEMEDINDICA"].ToString()))
                        //{
                        //    item.IDEspecialidadeMedIndica = Convert.ToInt32(listDb["IDESPECIALIDADEMEDINDICA"]);
                        //}
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public async Task<Sis_Atendimento> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDATENDIMENTO,IDEMPRESA, IDFILIAL, IDCONVENIOATEND, IDORIGEM, CODATENDIMENTO, IDPACIENTE, ");
                strSql.AppendFormat("DATAATENDIMENTO, IDMEDICO, IDESPECIALIDADE, IDMEDICOINDICA, IDUSUARIOINCLUSAO, DATAINCLUSAO, ");
                strSql.AppendFormat("IDUSUARIOALTERACAO, DATAALTERACAO, IDUSUARIOCANCELAMENTO, DATACANCELAMENTO, IDATEMOTCANCELAMENTO, ");
                strSql.AppendFormat("ISSINCRONIZADO, ISALTERADO, DESATIVADO, SYSTEM, HIDDEN, IDFILIALSIN, AGUDACRONICA, ");
                strSql.AppendFormat("PACIENTECAIXA, IDCLINICA, IDATENDIMENTOSTATUS, IDINDICADORACIDENTE, IDREVISAOENTREGA, ");
                strSql.AppendFormat("IDUSUARIORECEBIMENTO, DATARECEBIMENTO, IDUSUARIOCANCELARECEBIMENTO, DATACANCELARECEBIMENTO, ");
                strSql.AppendFormat("OBSRECEBIMENTO, IDUSUARIOOBSRECEBIMENTO, DATAOBSRECEBIMENTO, ISINTERNOU, IDULTUSUCONFEMAIL, ");
                strSql.AppendFormat("ISSMSENVIADO, ISSMSCONFIRMADO, IDMEDICOCONSULTA, DATAMEDICOCONSULTA, MES, ANO, IDADE, ");
                strSql.AppendFormat("JUSTIFICATIVANUMDECLNASCVIVO, IDATENDIMENTOINICIAL, DATARETORNO, OBSRETORNO, IDUSUARIORETORNO, ");
                strSql.AppendFormat("ISENCAMINHADO, IDSW "); //, DATACONCLUSAO, IDESPECIALIDADEMEDINDICA ");
                strSql.AppendFormat(" FROM SIS_ATENDIMENTO ");
                strSql.AppendFormat(" WHERE IDSW={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                conn.Open();
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {

                    var listDb = await cmd.ExecuteReaderAsync();
                    var item = new Sis_Atendimento();

                    if (listDb.HasRows)
                    {
                        listDb.Read();

                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTO"].ToString()))
                        {
                            item.IDAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                            if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESA"].ToString()))
                            {
                                item.IDEmpresa = Convert.ToInt32(listDb["IDEMPRESA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDFILIAL"].ToString()))
                            {
                                item.IDFilial = Convert.ToInt32(listDb["IDFILIAL"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDCONVENIOATEND"].ToString()))
                            {
                                item.IDConvenioAtend = Convert.ToInt32(listDb["IDCONVENIOATEND"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDORIGEM"].ToString()))
                            {
                                item.IDOrigem = Convert.ToInt32(listDb["IDORIGEM"]);
                            }
                            item.CodAtendimento = listDb["CODATENDIMENTO"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["IDPACIENTE"].ToString()))
                            {
                                item.IDPaciente = Convert.ToInt32(listDb["IDPACIENTE"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAATENDIMENTO"].ToString()))
                            {
                                item.DataAtendimento = Convert.ToDateTime(listDb["DATAATENDIMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDMEDICO"].ToString()))
                            {
                                item.IDMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDESPECIALIDADE"].ToString()))
                            {
                                item.IDEspecialidade = Convert.ToInt32(listDb["IDESPECIALIDADE"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOINDICA"].ToString()))
                            {
                                item.IDMedicoIndica = Convert.ToInt32(listDb["IDMEDICOINDICA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOINCLUSAO"].ToString()))
                            {
                                item.IDUsuarioInclusao = Convert.ToInt32(listDb["IDUSUARIOINCLUSAO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAINCLUSAO"].ToString()))
                            {
                                item.DataInclusao = Convert.ToDateTime(listDb["DATAINCLUSAO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                            {
                                item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAALTERACAO"].ToString()))
                            {
                                item.DataAlteracao = Convert.ToDateTime(listDb["DATAALTERACAO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCANCELAMENTO"].ToString()))
                            {
                                item.IDUsuarioCancelamento = Convert.ToInt32(listDb["IDUSUARIOCANCELAMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATACANCELAMENTO"].ToString()))
                            {
                                item.DataCancelamento = Convert.ToDateTime(listDb["DATACANCELAMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDATEMOTCANCELAMENTO"].ToString()))
                            {
                                item.IDAteMotCancelamento = Convert.ToInt32(listDb["IDATEMOTCANCELAMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                            {
                                item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                            {
                                item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDSW"].ToString()))
                            {
                                item.IDSW = Convert.ToInt32(listDb["IDSW"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DESATIVADO"].ToString()))
                            {
                                item.Desativado = Convert.ToBoolean(listDb["DESATIVADO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["SYSTEM"].ToString()))
                            {
                                item.System = Convert.ToBoolean(listDb["SYSTEM"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["HIDDEN"].ToString()))
                            {
                                item.Hidden = Convert.ToBoolean(listDb["HIDDEN"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                            {
                                item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                            }
                            item.AgudaCronica = listDb["AGUDACRONICA"].ToString();
                            item.PacienteCaixa = listDb["PACIENTECAIXA"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["IDCLINICA"].ToString()))
                            {
                                item.IDClinica = Convert.ToInt32(listDb["IDCLINICA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTOSTATUS"].ToString()))
                            {
                                item.IDAtendimentoStatus = Convert.ToInt32(listDb["IDATENDIMENTOSTATUS"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDINDICADORACIDENTE"].ToString()))
                            {
                                item.IDIndicadorAcidente = Convert.ToInt32(listDb["IDINDICADORACIDENTE"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDREVISAOENTREGA"].ToString()))
                            {
                                item.IDRevisaoEntrega = Convert.ToInt32(listDb["IDREVISAOENTREGA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORECEBIMENTO"].ToString()))
                            {
                                item.IDUsuarioRecebimento = Convert.ToInt32(listDb["IDUSUARIORECEBIMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCANCELARECEBIMENTO"].ToString()))
                            {
                                item.IDUsuarioCancelaRecebimento = Convert.ToInt32(listDb["IDUSUARIOCANCELARECEBIMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATACANCELARECEBIMENTO"].ToString()))
                            {
                                item.DataCancelaRecebimento = Convert.ToDateTime(listDb["DATACANCELARECEBIMENTO"]);
                            }
                            item.ObsRecebimento = listDb["OBSRECEBIMENTO"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOOBSRECEBIMENTO"].ToString()))
                            {
                                item.IDUsuarioObsRecebimento = Convert.ToInt32(listDb["IDUSUARIOOBSRECEBIMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAOBSRECEBIMENTO"].ToString()))
                            {
                                item.DataObsRecebimento = Convert.ToDateTime(listDb["DATAOBSRECEBIMENTO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISINTERNOU"].ToString()))
                            {
                                item.IsInternou = Convert.ToBoolean(listDb["ISINTERNOU"]);
                            }
                            // TODO: Correção do tipo de dados 
                            // no visual asa está com o tipo TBitControl
                            //if (!string.IsNullOrWhiteSpace(listDb["IDULTUSUCONFEMAIL"].ToString()))
                            //{
                            //    item.IDUltUsuConfEmail = Convert.ToInt32(listDb["IDULTUSUCONFEMAIL"]);
                            //}
                            if (!string.IsNullOrWhiteSpace(listDb["ISSMSENVIADO"].ToString()))
                            {
                                item.IsSMSEnviado = Convert.ToBoolean(listDb["ISSMSENVIADO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISSMSCONFIRMADO"].ToString()))
                            {
                                item.IsSMSConfirmado = Convert.ToBoolean(listDb["ISSMSCONFIRMADO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOCONSULTA"].ToString()))
                            {
                                item.IDMedicoConsulta = Convert.ToInt32(listDb["IDMEDICOCONSULTA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAMEDICOCONSULTA"].ToString()))
                            {
                                item.DataMedicoConsulta = Convert.ToDateTime(listDb["DATAMEDICOCONSULTA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["MES"].ToString()))
                            {
                                item.Mes = Convert.ToInt32(listDb["MES"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ANO"].ToString()))
                            {
                                item.Ano = Convert.ToInt32(listDb["ANO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["IDADE"].ToString()))
                            {
                                item.Idade = Convert.ToInt32(listDb["IDADE"]);
                            }
                            item.JustificativaNumDeclNascVivo = listDb["JUSTIFICATIVANUMDECLNASCVIVO"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTOINICIAL"].ToString()))
                            {
                                item.IDAtendimentoInicial = Convert.ToInt32(listDb["IDATENDIMENTOINICIAL"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATARETORNO"].ToString()))
                            {
                                item.DataRetorno = Convert.ToDateTime(listDb["DATARETORNO"]);
                            }

                            item.ObsRetorno = (byte[])listDb["OBSRETORNO"];

                            if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORETORNO"].ToString()))
                            {
                                item.IDUsuarioRetorno = Convert.ToInt32(listDb["IDUSUARIORETORNO"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISENCAMINHADO"].ToString()))
                            {
                                item.IsEncaminhado = Convert.ToInt32(listDb["ISENCAMINHADO"]);
                            }
                            //if (!string.IsNullOrWhiteSpace(listDb["DATACONCLUSAO"].ToString()))
                            //{
                            //    item.DataConclusao = Convert.ToDateTime(listDb["DATACONCLUSAO"]);
                            //}
                            //if (!string.IsNullOrWhiteSpace(listDb["IDESPECIALIDADEMEDINDICA"].ToString()))
                            //{
                            //    item.IDEspecialidadeMedIndica = Convert.ToInt32(listDb["IDESPECIALIDADEMEDINDICA"]);
                            //}

                        }
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class SisContaMedicaRepositorio : IRepositorio<Sis_ContaMedica>
        {
            private string _cnAsa;

            public SisContaMedicaRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }

            public async Task Alterar(Sis_ContaMedica input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql = new StringBuilder();
                    strSql.AppendFormat("UPDATE SIS_CONTAMEDICA SET ");
                    //strSql.AppendFormat("IDCONTAMEDICA={0}", input.IDContaMedica);
                    strSql.AppendFormat("IDATENDIMENTO={0}", input.IDAtendimento);
                    strSql.AppendFormat(",IDCONVENIO={0}", input.IDConvenio);
                    strSql.AppendFormat(",IDPLANO={0}", input.IDPlano.HasValue ? input.IDPlano.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDGUIA={0}", input.IDGuia);
                    strSql.AppendFormat(",IDFORMATOMATRICULA={0}", input.IDFormatoMatricula.HasValue ? input.IDFormatoMatricula.Value.ToString() : "NULL");
                    strSql.AppendFormat(",MATRICULA='{0}'", input.Matricula);  //string.IsNullOrWhiteSpace(input.Matricula) ? "NULL" : input.Matricula);
                    strSql.AppendFormat(",CODDEPENDENTE={0}", string.IsNullOrWhiteSpace(input.CodDependente) ? "NULL" : input.CodDependente);
                    strSql.AppendFormat(",NUMEROGUIA={0}", string.IsNullOrWhiteSpace(input.NumeroGuia) ? "NULL" : input.NumeroGuia);
                    //strSql.AppendFormat(",TITULAR='{0}'", input.Titular); //string.IsNullOrWhiteSpace(input.Titular) ? "NULL" : input.Titular);
                    strSql.AppendFormat(",DTPAGAMENTO={0}{1}{0}", input.DtPagamento.HasValue ? "'" : string.Empty, input.DtPagamento.HasValue ? input.DtPagamento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",VALCARTEIRA={0}{1}{0}", input.ValCarteira.HasValue ? "'" : string.Empty, input.ValCarteira.HasValue ? input.ValCarteira.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDENTACOMPANHANTE={0}", string.IsNullOrWhiteSpace(input.IdentAcompanhante) ? "NULL" : input.IdentAcompanhante);
                    strSql.AppendFormat(",STATUSENTREGA={0}", input.StatusEntrega.HasValue ? Convert.ToInt32(input.StatusEntrega) : 0);
                    strSql.AppendFormat(",SENHAAUTORIZACAO='{0}'", input.SenhaAutorizacao); //string.IsNullOrWhiteSpace(input.SenhaAutorizacao) ? "NULL" : input.SenhaAutorizacao);
                    strSql.AppendFormat(",DIASAUTORIZADOS={0}", input.DiasAutorizados.HasValue ? input.DiasAutorizados.Value.ToString() : "NULL"); // input.DiasAutorizados);
                    strSql.AppendFormat(",OBSERVACAO='{0}'", input.Observacao);
                    strSql.AppendFormat(",IDPENDENCIAMOTIVO={0}", input.IDPendenciaMotivo.HasValue ? input.IDPendenciaMotivo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAULTIMACONFERENCIA={0}{1}{0}", input.DataUltimaConferencia.HasValue ? "'" : string.Empty, input.DataUltimaConferencia.HasValue ? input.DataUltimaConferencia.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOCONFERENCIA={0}", input.IDUsuarioConferencia.HasValue ? input.IDUsuarioConferencia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISSINCRONIZADO={0}", input.IsSincronizado.HasValue ? Convert.ToInt32(input.IsSincronizado) : 0);
                    strSql.AppendFormat(",ISALTERADO={0}", input.IsAlterado.HasValue ? Convert.ToInt32(input.IsAlterado) : 0);
                    strSql.AppendFormat(",IDSW={0}", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL"); //input.IDImportado);
                    strSql.AppendFormat(",IDFILIALSIN={0}", input.IDFilialSin.HasValue ? input.IDFilialSin.Value.ToString() : "NULL");
                    strSql.AppendFormat(",NUMEROSEQ={0}", string.IsNullOrWhiteSpace(input.NumeroSeq) ? "NULL" : input.NumeroSeq);
                    strSql.AppendFormat(",IDMEDICO={0}", input.IDMedico.HasValue ? input.IDMedico.Value.ToString() : "NULL"); //input.IDMedico);
                    strSql.AppendFormat(",GUIAPRINCIPAL={0}", string.IsNullOrWhiteSpace(input.GuiaPrincipal) ? "NULL" : input.GuiaPrincipal);
                    strSql.AppendFormat(",IDLEITOTIPO={0}", input.IDLeitoTipo.HasValue ? input.IDLeitoTipo.Value.ToString() : "NULL"); // input.IDLeitoTipo);
                    strSql.AppendFormat(",DATAINICIO={0}{1}{0}", input.DataInicio.HasValue ? "'" : string.Empty, input.DataInicio.HasValue ? input.DataInicio.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DATAFIM={0}{1}{0}", input.DataFim.HasValue ? "'" : string.Empty, input.DataFim.HasValue ? input.DataFim.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDALTA={0}", input.IDAlta.HasValue ? input.IDAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISAUTORIZADOR={0}", input.IsAutorizador.HasValue ? Convert.ToInt32(input.IsAutorizador) : 0);
                    strSql.AppendFormat(",DATAAUTORIZACAO={0}{1}{0}", input.DataAutorizacao.HasValue ? "'" : string.Empty, input.DataAutorizacao.HasValue ? input.DataAutorizacao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",TIPOATENDIMENTO={0}{1}{0}", string.IsNullOrWhiteSpace(input.TipoAtendimento) ? string.Empty : "'", string.IsNullOrWhiteSpace(input.TipoAtendimento) ? "NULL" : string.Format("{0:00}", input.TipoAtendimento));
                    strSql.AppendFormat(",IDEMPRESAPAC={0}", input.IDEmpresaPac.HasValue ? input.IDEmpresaPac.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DIASERIE1={0}{1}{0}", input.DiaSerie1.HasValue ? "'" : string.Empty, input.DiaSerie1.HasValue ? input.DiaSerie1.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE2={0}{1}{0}", input.DiaSerie2.HasValue ? "'" : string.Empty, input.DiaSerie2.HasValue ? input.DiaSerie2.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE3={0}{1}{0}", input.DiaSerie3.HasValue ? "'" : string.Empty, input.DiaSerie3.HasValue ? input.DiaSerie3.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE4={0}{1}{0}", input.DiaSerie4.HasValue ? "'" : string.Empty, input.DiaSerie4.HasValue ? input.DiaSerie4.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE5={0}{1}{0}", input.DiaSerie5.HasValue ? "'" : string.Empty, input.DiaSerie5.HasValue ? input.DiaSerie5.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE6={0}{1}{0}", input.DiaSerie6.HasValue ? "'" : string.Empty, input.DiaSerie6.HasValue ? input.DiaSerie6.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE7={0}{1}{0}", input.DiaSerie7.HasValue ? "'" : string.Empty, input.DiaSerie7.HasValue ? input.DiaSerie7.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE8={0}{1}{0}", input.DiaSerie8.HasValue ? "'" : string.Empty, input.DiaSerie8.HasValue ? input.DiaSerie8.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE9={0}{1}{0}", input.DiaSerie9.HasValue ? "'" : string.Empty, input.DiaSerie9.HasValue ? input.DiaSerie9.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIASERIE10={0}{1}{0}", input.DiaSerie10.HasValue ? "'" : string.Empty, input.DiaSerie10.HasValue ? input.DiaSerie10.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DATAENTRFOLHASALA={0}{1}{0}", input.DataEntrFolhaSala.HasValue ? "'" : string.Empty, input.DataEntrFolhaSala.HasValue ? input.DataEntrFolhaSala.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DATAENTRDESCCIR={0}{1}{0}", input.DataEntrDescCir.HasValue ? "'" : string.Empty, input.DataEntrDescCir.HasValue ? input.DataEntrDescCir.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DATAENTRBOLANEST={0}{1}{0}", input.DataEntrBolAnest.HasValue ? "'" : string.Empty, input.DataEntrBolAnest.HasValue ? input.DataEntrBolAnest.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DATAENTRCDFILME={0}{1}{0}", input.DataEntrCDFilme.HasValue ? "'" : string.Empty, input.DataEntrCDFilme.HasValue ? input.DataEntrCDFilme.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",ISSEMAUTORIZACAO={0}", input.IsSemAutorizacao.HasValue ? Convert.ToInt32(input.IsSemAutorizacao) : 0);
                    strSql.AppendFormat(",ISAUTORIZADO={0}", input.IsAutorizado.HasValue ? Convert.ToInt32(input.IsAutorizado) : 0);
                    strSql.AppendFormat(",INDICACAOCLINICA={0}", string.IsNullOrWhiteSpace(input.IndicacaoClinica) ? "NULL" : input.IndicacaoClinica);
                    strSql.AppendFormat(",TRILHACARTAO={0}", string.IsNullOrWhiteSpace(input.TrilhaCartao) ? "NULL" : input.TrilhaCartao);
                    strSql.AppendFormat(",DATAVALIDADESENHA={0}{1}{0}", input.DataValidadeSenha.HasValue ? "'" : string.Empty, input.DataValidadeSenha.HasValue ? input.DataValidadeSenha.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",GUIAOPERADORA={0}", string.IsNullOrWhiteSpace(input.GuiaOperadora) ? "NULL" : input.GuiaOperadora);
                    strSql.AppendFormat(",IDUSUARIORESPONSAVEL={0}", input.IDUsuarioResponsavel.HasValue ? input.IDUsuarioResponsavel.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAUSUARIORESPONSAVEL={0}{1}{0}", input.DataUsuarioResponsavel.HasValue ? "'" : string.Empty, input.DataUsuarioResponsavel.HasValue ? input.DataUsuarioResponsavel.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOALTERACAO={0}", input.IDUsuarioAlteracao.HasValue ? input.IDUsuarioAlteracao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAALTERACAO={0}{1}{0}", input.DataAlteracao.HasValue ? "'" : string.Empty, input.DataAlteracao.HasValue ? input.DataAlteracao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",ORDEM={0}", input.Ordem.HasValue ? input.Ordem.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISIMPRIMEGUIA={0}", input.IsImprimeGuia.HasValue ? Convert.ToInt32(input.IsImprimeGuia) : 0);
                    //strSql.AppendFormat(",ISCOMPLEMENTAR={0} ", input.IsComplementar.HasValue ? Convert.ToInt32(input.IsComplementar) : 0);
                    strSql.AppendFormat(" WHERE IDCONTAMEDICA={0}", input.IDContaMedica);

                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_CONTAMEDICA SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDCONTAMEDICA=@IDCONTAMEDICA ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDCONTAMEDICA", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Sis_ContaMedica input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("INSERT INTO SIS_CONTAMEDICA(");
                    if (input.IDContaMedica.HasValue)
                    {
                        strSql.AppendFormat("IDCONTAMEDICA,");
                    }
                    strSql.AppendFormat("IDATENDIMENTO, IDCONVENIO, IDPLANO, IDGUIA, IDFORMATOMATRICULA, MATRICULA");
                    strSql.AppendFormat(",CODDEPENDENTE, NUMEROGUIA, TITULAR, DTPAGAMENTO, VALCARTEIRA, IDENTACOMPANHANTE");
                    strSql.AppendFormat(",STATUSENTREGA, SENHAAUTORIZACAO, DIASAUTORIZADOS, OBSERVACAO, IDPENDENCIAMOTIVO");
                    strSql.AppendFormat(",DATAULTIMACONFERENCIA, IDUSUARIOCONFERENCIA, ISSINCRONIZADO, ISALTERADO, IDSW");
                    strSql.AppendFormat(",IDFILIALSIN, NUMEROSEQ, IDMEDICO, GUIAPRINCIPAL, IDLEITOTIPO, DATAINICIO, DATAFIM, IDALTA");
                    strSql.AppendFormat(",ISAUTORIZADOR, DATAAUTORIZACAO, TIPOATENDIMENTO, IDEMPRESAPAC, DIASERIE1, DIASERIE2");
                    strSql.AppendFormat(",DIASERIE3, DIASERIE4, DIASERIE5, DIASERIE6, DIASERIE7, DIASERIE8, DIASERIE9, DIASERIE10");
                    strSql.AppendFormat(",DATAENTRFOLHASALA, DATAENTRDESCCIR, DATAENTRBOLANEST, DATAENTRCDFILME, ISSEMAUTORIZACAO");
                    strSql.AppendFormat(",ISAUTORIZADO, INDICACAOCLINICA, TRILHACARTAO, DATAVALIDADESENHA, GUIAOPERADORA");
                    strSql.AppendFormat(",IDUSUARIORESPONSAVEL, DATAUSUARIORESPONSAVEL, IDUSUARIOALTERACAO, DATAALTERACAO, ORDEM");
                    strSql.AppendFormat(",ISIMPRIMEGUIA"); //, ISCOMPLEMENTAR");
                    strSql.AppendLine(") ");
                    //strSql.AppendLine("output INSERTED.IDCONTAMEDICA ");
                    strSql.AppendLine("VALUES(");
                    if (input.IDContaMedica.HasValue)
                    {
                        strSql.AppendFormat("{0},", input.IDContaMedica);
                    }
                    strSql.AppendFormat("{0}", input.IDAtendimento);
                    strSql.AppendFormat(",{0}", input.IDConvenio.HasValue ? input.IDConvenio.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDPlano.HasValue ? input.IDPlano.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDGuia.HasValue ? input.IDGuia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDFormatoMatricula.HasValue ? input.IDFormatoMatricula.Value.ToString() : "NULL");
                    strSql.AppendFormat(",'{0}'", input.Matricula);  //string.IsNullOrWhiteSpace(input.Matricula) ? "NULL" : input.Matricula);
                    strSql.AppendFormat(",{0}{1}{0}", string.IsNullOrEmpty(input.CodDependente) ? string.Empty : "'", string.IsNullOrWhiteSpace(input.CodDependente) ? "NULL" : input.CodDependente);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.NumeroGuia) ? "NULL" : input.NumeroGuia);
                    strSql.AppendFormat(",{0}{1}{0}", string.IsNullOrEmpty(input.Titular) ? string.Empty : "'", string.IsNullOrWhiteSpace(input.Titular) ? "NULL" : input.Titular);
                    strSql.AppendFormat(",{0}{1}{0}", input.DtPagamento.HasValue ? "'" : string.Empty, input.DtPagamento.HasValue ? input.DtPagamento.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.ValCarteira.HasValue ? "'" : string.Empty, input.ValCarteira.HasValue ? input.ValCarteira.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.IdentAcompanhante) ? "NULL" : input.IdentAcompanhante);
                    strSql.AppendFormat(",{0}", input.StatusEntrega.HasValue ? Convert.ToInt32(input.StatusEntrega) : 0);
                    strSql.AppendFormat(",'{0}'", input.SenhaAutorizacao); //string.IsNullOrWhiteSpace(input.SenhaAutorizacao) ? "NULL" : input.SenhaAutorizacao);
                    strSql.AppendFormat(",{0}", input.DiasAutorizados.HasValue ? input.DiasAutorizados.Value.ToString() : "NULL"); // input.DiasAutorizados);
                    strSql.AppendFormat(",'{0}'", input.Observacao);
                    strSql.AppendFormat(",{0}", input.IDPendenciaMotivo.HasValue ? input.IDPendenciaMotivo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataUltimaConferencia.HasValue ? "'" : string.Empty, input.DataUltimaConferencia.HasValue ? input.DataUltimaConferencia.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioConferencia.HasValue ? input.IDUsuarioConferencia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsSincronizado.HasValue ? Convert.ToInt32(input.IsSincronizado) : 0);
                    strSql.AppendFormat(",{0}", input.IsAlterado.HasValue ? Convert.ToInt32(input.IsAlterado) : 0);
                    strSql.AppendFormat(",{0}", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL"); //input.IDImportado);
                    strSql.AppendFormat(",{0}", input.IDFilialSin.HasValue ? input.IDFilialSin.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.NumeroSeq) ? "NULL" : input.NumeroSeq);
                    strSql.AppendFormat(",{0}", input.IDMedico.HasValue ? input.IDMedico.Value.ToString() : "NULL"); //input.IDMedico);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.GuiaPrincipal) ? "NULL" : input.GuiaPrincipal);
                    strSql.AppendFormat(",{0}", input.IDLeitoTipo.HasValue ? input.IDLeitoTipo.Value.ToString() : "NULL"); // input.IDLeitoTipo);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataInicio.HasValue ? "'" : string.Empty, input.DataInicio.HasValue ? input.DataInicio.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataFim.HasValue ? "'" : string.Empty, input.DataFim.HasValue ? input.DataFim.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAlta.HasValue ? input.IDAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsAutorizador.HasValue ? Convert.ToInt32(input.IsAutorizador) : 0);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAutorizacao.HasValue ? "'" : string.Empty, input.DataAutorizacao.HasValue ? input.DataAutorizacao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", string.IsNullOrWhiteSpace(input.TipoAtendimento) ? string.Empty : "'", string.IsNullOrWhiteSpace(input.TipoAtendimento) ? "NULL" : string.Format("{0:00}", input.TipoAtendimento));
                    strSql.AppendFormat(",{0}", input.IDEmpresaPac.HasValue ? input.IDEmpresaPac.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie1.HasValue ? "'" : string.Empty, input.DiaSerie1.HasValue ? input.DiaSerie1.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie2.HasValue ? "'" : string.Empty, input.DiaSerie2.HasValue ? input.DiaSerie2.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie3.HasValue ? "'" : string.Empty, input.DiaSerie3.HasValue ? input.DiaSerie3.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie4.HasValue ? "'" : string.Empty, input.DiaSerie4.HasValue ? input.DiaSerie4.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie5.HasValue ? "'" : string.Empty, input.DiaSerie5.HasValue ? input.DiaSerie5.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie6.HasValue ? "'" : string.Empty, input.DiaSerie6.HasValue ? input.DiaSerie6.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie7.HasValue ? "'" : string.Empty, input.DiaSerie7.HasValue ? input.DiaSerie7.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie8.HasValue ? "'" : string.Empty, input.DiaSerie8.HasValue ? input.DiaSerie8.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie9.HasValue ? "'" : string.Empty, input.DiaSerie9.HasValue ? input.DiaSerie9.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DiaSerie10.HasValue ? "'" : string.Empty, input.DiaSerie10.HasValue ? input.DiaSerie10.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataEntrFolhaSala.HasValue ? "'" : string.Empty, input.DataEntrFolhaSala.HasValue ? input.DataEntrFolhaSala.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataEntrDescCir.HasValue ? "'" : string.Empty, input.DataEntrDescCir.HasValue ? input.DataEntrDescCir.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataEntrBolAnest.HasValue ? "'" : string.Empty, input.DataEntrBolAnest.HasValue ? input.DataEntrBolAnest.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataEntrCDFilme.HasValue ? "'" : string.Empty, input.DataEntrCDFilme.HasValue ? input.DataEntrCDFilme.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IsSemAutorizacao.HasValue ? Convert.ToInt32(input.IsSemAutorizacao) : 0);
                    strSql.AppendFormat(",{0}", input.IsAutorizado.HasValue ? Convert.ToInt32(input.IsAutorizado) : 0);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.IndicacaoClinica) ? "NULL" : input.IndicacaoClinica);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.TrilhaCartao) ? "NULL" : input.TrilhaCartao);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataValidadeSenha.HasValue ? "'" : string.Empty, input.DataValidadeSenha.HasValue ? input.DataValidadeSenha.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.GuiaOperadora) ? "NULL" : input.GuiaOperadora);
                    strSql.AppendFormat(",{0}", input.IDUsuarioResponsavel.HasValue ? input.IDUsuarioResponsavel.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataUsuarioResponsavel.HasValue ? "'" : string.Empty, input.DataUsuarioResponsavel.HasValue ? input.DataUsuarioResponsavel.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioAlteracao.HasValue ? input.IDUsuarioAlteracao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAlteracao.HasValue ? "'" : string.Empty, input.DataAlteracao.HasValue ? input.DataAlteracao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.Ordem.HasValue ? input.Ordem.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsImprimeGuia.HasValue ? Convert.ToInt32(input.IsImprimeGuia) : 0);
                    //strSql.AppendFormat(",{0} ", input.IsComplementar.HasValue ? Convert.ToInt32(input.IsComplementar) : 0);
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        //var modified = await cmd.ExecuteScalarAsync();
                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    var record = await Obter(input.IDSW.ToString());
                    var result = record.IDContaMedica.Value;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Sis_ContaMedica>> Listar()
            {
                var result = new List<Sis_ContaMedica>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDCONTAMEDICA, IDATENDIMENTO, IDCONVENIO, IDPLANO, IDGUIA, IDFORMATOMATRICULA, MATRICULA, ");
                strSql.AppendFormat("CODDEPENDENTE, NUMEROGUIA, TITULAR, DTPAGAMENTO, VALCARTEIRA, IDENTACOMPANHANTE, ");
                strSql.AppendFormat("STATUSENTREGA, SENHAAUTORIZACAO, DIASAUTORIZADOS, OBSERVACAO, IDPENDENCIAMOTIVO, ");
                strSql.AppendFormat("DATAULTIMACONFERENCIA, IDUSUARIOCONFERENCIA, ISSINCRONIZADO, ISALTERADO, ");
                strSql.AppendFormat("IDFILIALSIN, NUMEROSEQ, IDMEDICO, GUIAPRINCIPAL, IDLEITOTIPO, DATAINICIO, DATAFIM, IDALTA, ");
                strSql.AppendFormat("ISAUTORIZADOR, DATAAUTORIZACAO, TIPOATENDIMENTO, IDEMPRESAPAC, DIASERIE1, DIASERIE2, ");
                strSql.AppendFormat("DIASERIE3, DIASERIE4, DIASERIE5, DIASERIE6, DIASERIE7, DIASERIE8, DIASERIE9, DIASERIE10, ");
                strSql.AppendFormat("DATAENTRFOLHASALA, DATAENTRDESCCIR, DATAENTRBOLANEST, DATAENTRCDFILME, ISSEMAUTORIZACAO, ");
                strSql.AppendFormat("ISAUTORIZADO, INDICACAOCLINICA, TRILHACARTAO, DATAVALIDADESENHA, GUIAOPERADORA, ");
                strSql.AppendFormat("IDUSUARIORESPONSAVEL, DATAUSUARIORESPONSAVEL, IDUSUARIOALTERACAO, DATAALTERACAO, ORDEM, ");
                strSql.AppendFormat("ISIMPRIMEGUIA "); //, ISCOMPLEMENTAR ");
                strSql.AppendFormat("FROM SIS_CONTAMEDICA ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Sis_ContaMedica();

                        item.IDContaMedica = Convert.ToInt32(listDb["IDCONTAMEDICA"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTO"].ToString()))
                        {
                            item.IDAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONVENIO"].ToString()))
                        {
                            item.IDConvenio = Convert.ToInt32(listDb["IDCONVENIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPLANO"].ToString()))
                        {
                            item.IDPlano = Convert.ToInt32(listDb["IDPLANO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDGUIA"].ToString()))
                        {
                            item.IDGuia = Convert.ToInt32(listDb["IDGUIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFORMATOMATRICULA"].ToString()))
                        {
                            item.IDFormatoMatricula = Convert.ToInt32(listDb["IDFORMATOMATRICULA"]);
                        }
                        item.Matricula = listDb["MATRICULA"].ToString();
                        item.CodDependente = listDb["CODDEPENDENTE"].ToString();
                        item.NumeroGuia = listDb["NUMEROGUIA"].ToString();
                        item.Titular = listDb["TITULAR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["DTPAGAMENTO"].ToString()))
                        {
                            item.DtPagamento = Convert.ToDateTime(listDb["DTPAGAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["VALCARTEIRA"].ToString()))
                        {
                            item.ValCarteira = Convert.ToDateTime(listDb["VALCARTEIRA"]);
                        }
                        item.IdentAcompanhante = listDb["IDENTACOMPANHANTE"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSENTREGA"].ToString()))
                        {
                            item.StatusEntrega = Convert.ToInt32(listDb["STATUSENTREGA"]);
                        }
                        item.SenhaAutorizacao = listDb["SENHAAUTORIZACAO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["DIASAUTORIZADOS"].ToString()))
                        {
                            item.DiasAutorizados = Convert.ToInt32(listDb["DIASAUTORIZADOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["OBSERVACAO"].ToString()))
                        {
                            item.Observacao = (byte[])listDb["OBSERVACAO"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPENDENCIAMOTIVO"].ToString()))
                        {
                            item.IDPendenciaMotivo = Convert.ToInt32(listDb["IDPENDENCIAMOTIVO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMACONFERENCIA"].ToString()))
                        {
                            item.DataUltimaConferencia = Convert.ToDateTime(listDb["DATAULTIMACONFERENCIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCONFERENCIA"].ToString()))
                        {
                            item.IDUsuarioConferencia = Convert.ToInt32(listDb["IDUSUARIOCONFERENCIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        item.NumeroSeq = listDb["NUMEROSEQ"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICO"].ToString()))
                        {
                            item.IDMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        }
                        item.GuiaPrincipal = listDb["GUIAPRINCIPAL"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITOTIPO"].ToString()))
                        {
                            item.IDLeitoTipo = Convert.ToInt32(listDb["IDLEITOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINICIO"].ToString()))
                        {
                            item.DataInicio = Convert.ToDateTime(listDb["DATAINICIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAFIM"].ToString()))
                        {
                            item.DataFim = Convert.ToDateTime(listDb["DATAFIM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAUTORIZADOR"].ToString()))
                        {
                            item.IsAutorizador = Convert.ToBoolean(listDb["ISAUTORIZADOR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAAUTORIZACAO"].ToString()))
                        {
                            item.DataAutorizacao = Convert.ToDateTime(listDb["DATAAUTORIZACAO"]);
                        }
                        item.TipoAtendimento = listDb["TIPOATENDIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                        {
                            item.IDEmpresaPac = Convert.ToInt32(listDb["IDEMPRESAPAC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE1"].ToString()))
                        {
                            item.DiaSerie1 = Convert.ToDateTime(listDb["DIASERIE1"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE2"].ToString()))
                        {
                            item.DiaSerie2 = Convert.ToDateTime(listDb["DIASERIE2"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE3"].ToString()))
                        {
                            item.DiaSerie3 = Convert.ToDateTime(listDb["DIASERIE3"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE4"].ToString()))
                        {
                            item.DiaSerie4 = Convert.ToDateTime(listDb["DIASERIE4"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE5"].ToString()))
                        {
                            item.DiaSerie5 = Convert.ToDateTime(listDb["DIASERIE5"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE6"].ToString()))
                        {
                            item.DiaSerie6 = Convert.ToDateTime(listDb["DIASERIE6"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE7"].ToString()))
                        {
                            item.DiaSerie7 = Convert.ToDateTime(listDb["DIASERIE7"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE8"].ToString()))
                        {
                            item.DiaSerie8 = Convert.ToDateTime(listDb["DIASERIE8"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE9"].ToString()))
                        {
                            item.DiaSerie9 = Convert.ToDateTime(listDb["DIASERIE9"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE10"].ToString()))
                        {
                            item.DiaSerie10 = Convert.ToDateTime(listDb["DIASERIE10"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRFOLHASALA"].ToString()))
                        {
                            item.DataEntrFolhaSala = Convert.ToDateTime(listDb["DATAENTRFOLHASALA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRDESCCIR"].ToString()))
                        {
                            item.DataEntrDescCir = Convert.ToDateTime(listDb["DATAENTRDESCCIR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRBOLANEST"].ToString()))
                        {
                            item.DataEntrBolAnest = Convert.ToDateTime(listDb["DATAENTRBOLANEST"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRCDFILME"].ToString()))
                        {
                            item.DataEntrCDFilme = Convert.ToDateTime(listDb["DATAENTRCDFILME"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSEMAUTORIZACAO"].ToString()))
                        {
                            item.IsSemAutorizacao = Convert.ToBoolean(listDb["ISSEMAUTORIZACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAUTORIZADO"].ToString()))
                        {
                            item.IsAutorizado = Convert.ToBoolean(listDb["ISAUTORIZADO"]);
                        }
                        item.IndicacaoClinica = Convert.ToString(listDb["INDICACAOCLINICA"]);
                        item.TrilhaCartao = Convert.ToString(listDb["TRILHACARTAO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAVALIDADESENHA"].ToString()))
                        {
                            item.DataValidadeSenha = Convert.ToDateTime(listDb["DATAVALIDADESENHA"]);
                        }
                        item.GuiaOperadora = Convert.ToString(listDb["GUIAOPERADORA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORESPONSAVEL"].ToString()))
                        {
                            item.IDUsuarioResponsavel = Convert.ToInt32(listDb["IDUSUARIORESPONSAVEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAUSUARIORESPONSAVEL"].ToString()))
                        {
                            item.DataUsuarioResponsavel = Convert.ToDateTime(listDb["DATAUSUARIORESPONSAVEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTERACAO"].ToString()))
                        {
                            item.DataAlteracao = Convert.ToDateTime(listDb["DATAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ORDEM"].ToString()))
                        {
                            item.Ordem = Convert.ToInt32(listDb["ORDEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISIMPRIMEGUIA"].ToString()))
                        {
                            item.IsImprimeGuia = Convert.ToBoolean(listDb["ISIMPRIMEGUIA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ISCOMPLEMENTAR"].ToString()))
                        //{
                        //    item.IsComplementar = Convert.ToBoolean(listDb["ISCOMPLEMENTAR"]);
                        //}

                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Sis_ContaMedica> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDCONTAMEDICA, IDATENDIMENTO, IDCONVENIO, IDPLANO, IDGUIA, IDFORMATOMATRICULA, MATRICULA, ");
                strSql.AppendFormat("CODDEPENDENTE, NUMEROGUIA, TITULAR, DTPAGAMENTO, VALCARTEIRA, IDENTACOMPANHANTE, ");
                strSql.AppendFormat("STATUSENTREGA, SENHAAUTORIZACAO, DIASAUTORIZADOS, OBSERVACAO, IDPENDENCIAMOTIVO, ");
                strSql.AppendFormat("DATAULTIMACONFERENCIA, IDUSUARIOCONFERENCIA, ISSINCRONIZADO, ISALTERADO, ");
                strSql.AppendFormat("IDFILIALSIN, NUMEROSEQ, IDMEDICO, GUIAPRINCIPAL, IDLEITOTIPO, DATAINICIO, DATAFIM, IDALTA, ");
                strSql.AppendFormat("ISAUTORIZADOR, DATAAUTORIZACAO, TIPOATENDIMENTO, IDEMPRESAPAC, DIASERIE1, DIASERIE2, ");
                strSql.AppendFormat("DIASERIE3, DIASERIE4, DIASERIE5, DIASERIE6, DIASERIE7, DIASERIE8, DIASERIE9, DIASERIE10, ");
                strSql.AppendFormat("DATAENTRFOLHASALA, DATAENTRDESCCIR, DATAENTRBOLANEST, DATAENTRCDFILME, ISSEMAUTORIZACAO, ");
                strSql.AppendFormat("ISAUTORIZADO, INDICACAOCLINICA, TRILHACARTAO, DATAVALIDADESENHA, GUIAOPERADORA, ");
                strSql.AppendFormat("IDUSUARIORESPONSAVEL, DATAUSUARIORESPONSAVEL, IDUSUARIOALTERACAO, DATAALTERACAO, ORDEM, ");
                strSql.AppendFormat("ISIMPRIMEGUIA "); //, ISCOMPLEMENTAR ");
                strSql.AppendFormat("FROM SIS_CONTAMEDICA ");
                strSql.AppendFormat("WHERE IDCONTAMEDICA={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_ContaMedica();

                    if (listDb.HasRows)
                    {
                        listDb.Read();

                        item.IDContaMedica = Convert.ToInt32(listDb["IDCONTAMEDICA"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTO"].ToString()))
                        {
                            item.IDAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONVENIO"].ToString()))
                        {
                            item.IDConvenio = Convert.ToInt32(listDb["IDCONVENIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPLANO"].ToString()))
                        {
                            item.IDPlano = Convert.ToInt32(listDb["IDPLANO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDGUIA"].ToString()))
                        {
                            item.IDGuia = Convert.ToInt32(listDb["IDGUIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFORMATOMATRICULA"].ToString()))
                        {
                            item.IDFormatoMatricula = Convert.ToInt32(listDb["IDFORMATOMATRICULA"]);
                        }
                        item.Matricula = listDb["MATRICULA"].ToString();
                        item.CodDependente = listDb["CODDEPENDENTE"].ToString();
                        item.NumeroGuia = listDb["NUMEROGUIA"].ToString();
                        item.Titular = listDb["TITULAR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["DTPAGAMENTO"].ToString()))
                        {
                            item.DtPagamento = Convert.ToDateTime(listDb["DTPAGAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["VALCARTEIRA"].ToString()))
                        {
                            item.ValCarteira = Convert.ToDateTime(listDb["VALCARTEIRA"]);
                        }
                        item.IdentAcompanhante = listDb["IDENTACOMPANHANTE"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSENTREGA"].ToString()))
                        {
                            item.StatusEntrega = Convert.ToInt32(listDb["STATUSENTREGA"]);
                        }
                        item.SenhaAutorizacao = listDb["SENHAAUTORIZACAO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["DIASAUTORIZADOS"].ToString()))
                        {
                            item.DiasAutorizados = Convert.ToInt32(listDb["DIASAUTORIZADOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["OBSERVACAO"].ToString()))
                        {
                            item.Observacao = (byte[])listDb["OBSERVACAO"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPENDENCIAMOTIVO"].ToString()))
                        {
                            item.IDPendenciaMotivo = Convert.ToInt32(listDb["IDPENDENCIAMOTIVO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMACONFERENCIA"].ToString()))
                        {
                            item.DataUltimaConferencia = Convert.ToDateTime(listDb["DATAULTIMACONFERENCIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCONFERENCIA"].ToString()))
                        {
                            item.IDUsuarioConferencia = Convert.ToInt32(listDb["IDUSUARIOCONFERENCIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        item.NumeroSeq = listDb["NUMEROSEQ"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICO"].ToString()))
                        {
                            item.IDMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        }
                        item.GuiaPrincipal = listDb["GUIAPRINCIPAL"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITOTIPO"].ToString()))
                        {
                            item.IDLeitoTipo = Convert.ToInt32(listDb["IDLEITOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINICIO"].ToString()))
                        {
                            item.DataInicio = Convert.ToDateTime(listDb["DATAINICIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAFIM"].ToString()))
                        {
                            item.DataFim = Convert.ToDateTime(listDb["DATAFIM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAUTORIZADOR"].ToString()))
                        {
                            item.IsAutorizador = Convert.ToBoolean(listDb["ISAUTORIZADOR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAAUTORIZACAO"].ToString()))
                        {
                            item.DataAutorizacao = Convert.ToDateTime(listDb["DATAAUTORIZACAO"]);
                        }
                        item.TipoAtendimento = listDb["TIPOATENDIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                        {
                            item.IDEmpresaPac = Convert.ToInt32(listDb["IDEMPRESAPAC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE1"].ToString()))
                        {
                            item.DiaSerie1 = Convert.ToDateTime(listDb["DIASERIE1"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE2"].ToString()))
                        {
                            item.DiaSerie2 = Convert.ToDateTime(listDb["DIASERIE2"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE3"].ToString()))
                        {
                            item.DiaSerie3 = Convert.ToDateTime(listDb["DIASERIE3"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE4"].ToString()))
                        {
                            item.DiaSerie4 = Convert.ToDateTime(listDb["DIASERIE4"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE5"].ToString()))
                        {
                            item.DiaSerie5 = Convert.ToDateTime(listDb["DIASERIE5"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE6"].ToString()))
                        {
                            item.DiaSerie6 = Convert.ToDateTime(listDb["DIASERIE6"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE7"].ToString()))
                        {
                            item.DiaSerie7 = Convert.ToDateTime(listDb["DIASERIE7"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE8"].ToString()))
                        {
                            item.DiaSerie8 = Convert.ToDateTime(listDb["DIASERIE8"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE9"].ToString()))
                        {
                            item.DiaSerie9 = Convert.ToDateTime(listDb["DIASERIE9"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE10"].ToString()))
                        {
                            item.DiaSerie10 = Convert.ToDateTime(listDb["DIASERIE10"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRFOLHASALA"].ToString()))
                        {
                            item.DataEntrFolhaSala = Convert.ToDateTime(listDb["DATAENTRFOLHASALA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRDESCCIR"].ToString()))
                        {
                            item.DataEntrDescCir = Convert.ToDateTime(listDb["DATAENTRDESCCIR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRBOLANEST"].ToString()))
                        {
                            item.DataEntrBolAnest = Convert.ToDateTime(listDb["DATAENTRBOLANEST"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRCDFILME"].ToString()))
                        {
                            item.DataEntrCDFilme = Convert.ToDateTime(listDb["DATAENTRCDFILME"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSEMAUTORIZACAO"].ToString()))
                        {
                            item.IsSemAutorizacao = Convert.ToBoolean(listDb["ISSEMAUTORIZACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAUTORIZADO"].ToString()))
                        {
                            item.IsAutorizado = Convert.ToBoolean(listDb["ISAUTORIZADO"]);
                        }
                        item.IndicacaoClinica = Convert.ToString(listDb["INDICACAOCLINICA"]);
                        item.TrilhaCartao = Convert.ToString(listDb["TRILHACARTAO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAVALIDADESENHA"].ToString()))
                        {
                            item.DataValidadeSenha = Convert.ToDateTime(listDb["DATAVALIDADESENHA"]);
                        }
                        item.GuiaOperadora = Convert.ToString(listDb["GUIAOPERADORA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORESPONSAVEL"].ToString()))
                        {
                            item.IDUsuarioResponsavel = Convert.ToInt32(listDb["IDUSUARIORESPONSAVEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAUSUARIORESPONSAVEL"].ToString()))
                        {
                            item.DataUsuarioResponsavel = Convert.ToDateTime(listDb["DATAUSUARIORESPONSAVEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTERACAO"].ToString()))
                        {
                            item.DataAlteracao = Convert.ToDateTime(listDb["DATAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ORDEM"].ToString()))
                        {
                            item.Ordem = Convert.ToInt32(listDb["ORDEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISIMPRIMEGUIA"].ToString()))
                        {
                            item.IsImprimeGuia = Convert.ToBoolean(listDb["ISIMPRIMEGUIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPLEMENTAR"].ToString()))
                        {
                            item.IsComplementar = Convert.ToBoolean(listDb["ISCOMPLEMENTAR"]);
                        }
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return item;
                }
            }

            public async Task<Sis_ContaMedica> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDCONTAMEDICA, IDATENDIMENTO, IDCONVENIO, IDPLANO, IDGUIA, IDFORMATOMATRICULA, MATRICULA, ");
                strSql.AppendFormat("CODDEPENDENTE, NUMEROGUIA, TITULAR, DTPAGAMENTO, VALCARTEIRA, IDENTACOMPANHANTE, ");
                strSql.AppendFormat("STATUSENTREGA, SENHAAUTORIZACAO, DIASAUTORIZADOS, OBSERVACAO, IDPENDENCIAMOTIVO, ");
                strSql.AppendFormat("DATAULTIMACONFERENCIA, IDUSUARIOCONFERENCIA, ISSINCRONIZADO, ISALTERADO, ");
                strSql.AppendFormat("IDFILIALSIN, NUMEROSEQ, IDMEDICO, GUIAPRINCIPAL, IDLEITOTIPO, DATAINICIO, DATAFIM, IDALTA, ");
                strSql.AppendFormat("ISAUTORIZADOR, DATAAUTORIZACAO, TIPOATENDIMENTO, IDEMPRESAPAC, DIASERIE1, DIASERIE2, ");
                strSql.AppendFormat("DIASERIE3, DIASERIE4, DIASERIE5, DIASERIE6, DIASERIE7, DIASERIE8, DIASERIE9, DIASERIE10, ");
                strSql.AppendFormat("DATAENTRFOLHASALA, DATAENTRDESCCIR, DATAENTRBOLANEST, DATAENTRCDFILME, ISSEMAUTORIZACAO, ");
                strSql.AppendFormat("ISAUTORIZADO, INDICACAOCLINICA, TRILHACARTAO, DATAVALIDADESENHA, GUIAOPERADORA, ");
                strSql.AppendFormat("IDUSUARIORESPONSAVEL, DATAUSUARIORESPONSAVEL, IDUSUARIOALTERACAO, DATAALTERACAO, ORDEM, ");
                strSql.AppendFormat("ISIMPRIMEGUIA, IDSW "); //, ISCOMPLEMENTAR ");
                strSql.AppendFormat("FROM SIS_CONTAMEDICA ");
                strSql.AppendFormat("WHERE IDSW={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_ContaMedica();

                    if (listDb.HasRows)
                    {
                        listDb.Read();

                        item.IDSW = Convert.ToInt32(listDb["IDSW"]);

                        item.IDContaMedica = Convert.ToInt32(listDb["IDCONTAMEDICA"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDIMENTO"].ToString()))
                        {
                            item.IDAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCONVENIO"].ToString()))
                        {
                            item.IDConvenio = Convert.ToInt32(listDb["IDCONVENIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPLANO"].ToString()))
                        {
                            item.IDPlano = Convert.ToInt32(listDb["IDPLANO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDGUIA"].ToString()))
                        {
                            item.IDGuia = Convert.ToInt32(listDb["IDGUIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDFORMATOMATRICULA"].ToString()))
                        {
                            item.IDFormatoMatricula = Convert.ToInt32(listDb["IDFORMATOMATRICULA"]);
                        }
                        item.Matricula = listDb["MATRICULA"].ToString();
                        item.CodDependente = listDb["CODDEPENDENTE"].ToString();
                        item.NumeroGuia = listDb["NUMEROGUIA"].ToString();
                        item.Titular = listDb["TITULAR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["DTPAGAMENTO"].ToString()))
                        {
                            item.DtPagamento = Convert.ToDateTime(listDb["DTPAGAMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["VALCARTEIRA"].ToString()))
                        {
                            item.ValCarteira = Convert.ToDateTime(listDb["VALCARTEIRA"]);
                        }
                        item.IdentAcompanhante = listDb["IDENTACOMPANHANTE"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSENTREGA"].ToString()))
                        {
                            item.StatusEntrega = Convert.ToInt32(listDb["STATUSENTREGA"]);
                        }
                        item.SenhaAutorizacao = listDb["SENHAAUTORIZACAO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["DIASAUTORIZADOS"].ToString()))
                        {
                            item.DiasAutorizados = Convert.ToInt32(listDb["DIASAUTORIZADOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["OBSERVACAO"].ToString()))
                        {
                            item.Observacao = (byte[])listDb["OBSERVACAO"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPENDENCIAMOTIVO"].ToString()))
                        {
                            item.IDPendenciaMotivo = Convert.ToInt32(listDb["IDPENDENCIAMOTIVO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMACONFERENCIA"].ToString()))
                        {
                            item.DataUltimaConferencia = Convert.ToDateTime(listDb["DATAULTIMACONFERENCIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOCONFERENCIA"].ToString()))
                        {
                            item.IDUsuarioConferencia = Convert.ToInt32(listDb["IDUSUARIOCONFERENCIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSINCRONIZADO"].ToString()))
                        {
                            item.IsSincronizado = Convert.ToBoolean(listDb["ISSINCRONIZADO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTERADO"].ToString()))
                        {
                            item.IsAlterado = Convert.ToBoolean(listDb["ISALTERADO"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.IDImportado = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["IDFILIALSIN"].ToString()))
                        {
                            item.IDFilialSin = Convert.ToInt32(listDb["IDFILIALSIN"]);
                        }
                        item.NumeroSeq = listDb["NUMEROSEQ"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICO"].ToString()))
                        {
                            item.IDMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        }
                        item.GuiaPrincipal = listDb["GUIAPRINCIPAL"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITOTIPO"].ToString()))
                        {
                            item.IDLeitoTipo = Convert.ToInt32(listDb["IDLEITOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINICIO"].ToString()))
                        {
                            item.DataInicio = Convert.ToDateTime(listDb["DATAINICIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAFIM"].ToString()))
                        {
                            item.DataFim = Convert.ToDateTime(listDb["DATAFIM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAUTORIZADOR"].ToString()))
                        {
                            item.IsAutorizador = Convert.ToBoolean(listDb["ISAUTORIZADOR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAAUTORIZACAO"].ToString()))
                        {
                            item.DataAutorizacao = Convert.ToDateTime(listDb["DATAAUTORIZACAO"]);
                        }
                        item.TipoAtendimento = listDb["TIPOATENDIMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                        {
                            item.IDEmpresaPac = Convert.ToInt32(listDb["IDEMPRESAPAC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE1"].ToString()))
                        {
                            item.DiaSerie1 = Convert.ToDateTime(listDb["DIASERIE1"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE2"].ToString()))
                        {
                            item.DiaSerie2 = Convert.ToDateTime(listDb["DIASERIE2"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE3"].ToString()))
                        {
                            item.DiaSerie3 = Convert.ToDateTime(listDb["DIASERIE3"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE4"].ToString()))
                        {
                            item.DiaSerie4 = Convert.ToDateTime(listDb["DIASERIE4"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE5"].ToString()))
                        {
                            item.DiaSerie5 = Convert.ToDateTime(listDb["DIASERIE5"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE6"].ToString()))
                        {
                            item.DiaSerie6 = Convert.ToDateTime(listDb["DIASERIE6"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE7"].ToString()))
                        {
                            item.DiaSerie7 = Convert.ToDateTime(listDb["DIASERIE7"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE8"].ToString()))
                        {
                            item.DiaSerie8 = Convert.ToDateTime(listDb["DIASERIE8"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE9"].ToString()))
                        {
                            item.DiaSerie9 = Convert.ToDateTime(listDb["DIASERIE9"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DIASERIE10"].ToString()))
                        {
                            item.DiaSerie10 = Convert.ToDateTime(listDb["DIASERIE10"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRFOLHASALA"].ToString()))
                        {
                            item.DataEntrFolhaSala = Convert.ToDateTime(listDb["DATAENTRFOLHASALA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRDESCCIR"].ToString()))
                        {
                            item.DataEntrDescCir = Convert.ToDateTime(listDb["DATAENTRDESCCIR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRBOLANEST"].ToString()))
                        {
                            item.DataEntrBolAnest = Convert.ToDateTime(listDb["DATAENTRBOLANEST"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAENTRCDFILME"].ToString()))
                        {
                            item.DataEntrCDFilme = Convert.ToDateTime(listDb["DATAENTRCDFILME"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSEMAUTORIZACAO"].ToString()))
                        {
                            item.IsSemAutorizacao = Convert.ToBoolean(listDb["ISSEMAUTORIZACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISAUTORIZADO"].ToString()))
                        {
                            item.IsAutorizado = Convert.ToBoolean(listDb["ISAUTORIZADO"]);
                        }
                        item.IndicacaoClinica = Convert.ToString(listDb["INDICACAOCLINICA"]);
                        item.TrilhaCartao = Convert.ToString(listDb["TRILHACARTAO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAVALIDADESENHA"].ToString()))
                        {
                            item.DataValidadeSenha = Convert.ToDateTime(listDb["DATAVALIDADESENHA"]);
                        }
                        item.GuiaOperadora = Convert.ToString(listDb["GUIAOPERADORA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIORESPONSAVEL"].ToString()))
                        {
                            item.IDUsuarioResponsavel = Convert.ToInt32(listDb["IDUSUARIORESPONSAVEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAUSUARIORESPONSAVEL"].ToString()))
                        {
                            item.DataUsuarioResponsavel = Convert.ToDateTime(listDb["DATAUSUARIORESPONSAVEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTERACAO"].ToString()))
                        {
                            item.IDUsuarioAlteracao = Convert.ToInt32(listDb["IDUSUARIOALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTERACAO"].ToString()))
                        {
                            item.DataAlteracao = Convert.ToDateTime(listDb["DATAALTERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ORDEM"].ToString()))
                        {
                            item.Ordem = Convert.ToInt32(listDb["ORDEM"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISIMPRIMEGUIA"].ToString()))
                        {
                            item.IsImprimeGuia = Convert.ToBoolean(listDb["ISIMPRIMEGUIA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ISCOMPLEMENTAR"].ToString()))
                        //{
                        //    item.IsComplementar = Convert.ToBoolean(listDb["ISCOMPLEMENTAR"]);
                        //}
                    }

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class SisInternacaoRepositorio : IRepositorio<Sis_Internacao>
        {
            private string _cnAsa;

            public SisInternacaoRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }

            public async Task Alterar(Sis_Internacao input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_INTERNACAO SET ");
                    //strSql.AppendFormat("IDINTERNACAO={0}", input.IDInternacao);
                    //strSql.AppendFormat(",CODINTERNACAO={0}", input.CodInternacao);
                    strSql.AppendFormat("IDLEITO={0}", input.IDLeito.HasValue ? input.IDLeito.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDLEITOTIPO={0}", input.IDLeitoTipo.HasValue ? input.IDLeitoTipo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAALTA={0}{1}{0}", input.DataAlta.HasValue ? "'" : string.Empty, input.DataAlta.HasValue ? input.DataAlta.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDALTA={0}", input.IDAlta.HasValue ? input.IDAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",TEMACOMPANHANTE={0}", input.TemAcompanhante.HasValue ? Convert.ToInt32(input.TemAcompanhante) : 0);
                    strSql.AppendFormat(",RESPONSAVEL={0}", string.IsNullOrWhiteSpace(input.Responsavel) ? "NULL" : input.Responsavel);
                    strSql.AppendFormat(",ENDRESPONSA={0}", string.IsNullOrWhiteSpace(input.EndResponsa) ? "NULL" : input.EndResponsa);
                    strSql.AppendFormat(",COMPRESPONSA={0}", string.IsNullOrWhiteSpace(input.CompResponsa) ? "NULL" : input.CompResponsa);
                    strSql.AppendFormat(",CEPRESPONSA={0}", string.IsNullOrWhiteSpace(input.CEPResponsa) ? "NULL" : input.CEPResponsa);
                    strSql.AppendFormat(",IDBAIRRORESPONSA={0}", input.IDBairroResponsa.HasValue ? input.IDBairroResponsa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDCIDADERESPONSA={0}", input.IDCidadeResponsa.HasValue ? input.IDCidadeResponsa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDESTADORESPONSA={0}", input.IDEstadoResponsa.HasValue ? input.IDEstadoResponsa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",PAISRESPONSA={0}", string.IsNullOrWhiteSpace(input.PaisResponsa) ? "NULL" : input.PaisResponsa);
                    strSql.AppendFormat(",IDTRESPONSA={0}", string.IsNullOrWhiteSpace(input.IdtResponsa) ? "NULL" : input.IdtResponsa);
                    strSql.AppendFormat(",ORGEMISRESPONSA={0}", string.IsNullOrWhiteSpace(input.OrgEmisResponsa) ? "NULL" : input.OrgEmisResponsa);
                    strSql.AppendFormat(",EMISIDTRESPONSA={0}{1}{0}", input.EmisIdtResponsa.HasValue ? "'" : string.Empty, input.EmisIdtResponsa.HasValue ? input.EmisIdtResponsa.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",CPFRESPONSA={0}", string.IsNullOrWhiteSpace(input.CPFResponsa) ? "NULL" : input.CPFResponsa);
                    strSql.AppendFormat(",CGCRESPONSA={0}", string.IsNullOrWhiteSpace(input.CGCResponsa) ? "NULL" : input.CGCResponsa);
                    strSql.AppendFormat(",IDESTADOPAC={0}", input.IDEstadoPac.HasValue ? input.IDEstadoPac.Value.ToString() : "NULL");
                    strSql.AppendFormat(",STATUSPRONT={0}", input.StatusPront.HasValue ? input.StatusPront.Value : 0);
                    strSql.AppendFormat(",IDUSUARIOPRONT={0}", input.IDUsuarioPront.HasValue ? input.IDUsuarioPront.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAPRONT={0}{1}{0}", input.DataPront.HasValue ? "'" : string.Empty, input.DataPront.HasValue ? input.DataPront.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",NUMOBITO={0}", string.IsNullOrWhiteSpace(input.NumObito) ? "NULL" : input.NumObito);
                    strSql.AppendFormat(",IDUSUARIOALTAINC={0}", input.IDUsuarioAltaInc.HasValue ? input.IDUsuarioAltaInc.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAALTAINC={0}{1}{0}", input.DataAltaInc.HasValue ? "'" : string.Empty, input.DataAltaInc.HasValue ? input.DataAltaInc.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOALTAALT={0}", input.IDUsuarioAltaAlt.HasValue ? input.IDUsuarioAltaAlt.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAALTAALT={0}{1}{0}", input.DataAltaAlt.HasValue ? "'" : string.Empty, input.DataAltaAlt.HasValue ? input.DataAltaAlt.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOALTADEL={0}", input.IDUsuarioAltaDel.HasValue ? input.IDUsuarioAltaDel.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAALTADEL={0}{1}{0}", input.DataAltaDel.HasValue ? "'" : string.Empty, input.DataAltaDel.HasValue ? input.DataAltaDel.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",ISELETIVA={0}", input.IsEletiva.HasValue ? Convert.ToInt32(input.IsEletiva) : 0);
                    strSql.AppendFormat(",IDCIDOBITO={0}", input.IDCIDObito.HasValue ? input.IDCIDObito.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISGESTACAO={0}", input.IsGestacao.HasValue ? Convert.ToInt32(input.IsGestacao) : 0);
                    strSql.AppendFormat(",ISABORTO={0}", input.IsAborto.HasValue ? Convert.ToInt32(input.IsAborto) : 0);
                    strSql.AppendFormat(",ISTRANSMAT={0}", input.IsTransMat.HasValue ? Convert.ToInt32(input.IsTransMat) : 0);
                    strSql.AppendFormat(",ISCOMPPUERPERIO={0}", input.IsCompPuerperio.HasValue ? Convert.ToInt32(input.IsCompPuerperio) : 0);
                    strSql.AppendFormat(",ISATENDRNSALAPARTO={0}", input.IsAtendRNSalaParto.HasValue ? Convert.ToInt32(input.IsAtendRNSalaParto) : 0);
                    strSql.AppendFormat(",ISCOMPNEONATAL={0}", input.IsCompNeoNatal.HasValue ? Convert.ToInt32(input.IsCompNeoNatal) : 0);
                    strSql.AppendFormat(",ISBXPESO={0}", input.IsBxPeso.HasValue ? Convert.ToInt32(input.IsBxPeso) : 0);
                    strSql.AppendFormat(",ISCESAREA={0}", input.IsCesarea.HasValue ? Convert.ToInt32(input.IsCesarea) : 0);
                    strSql.AppendFormat(",ISNORMAL={0}", input.IsNormal.HasValue ? Convert.ToInt32(input.IsNormal) : 0);
                    strSql.AppendFormat(",ISINTERNACAOOBSTETRICA={0}", input.IsInternacaoObstetrica.HasValue ? Convert.ToInt32(input.IsInternacaoObstetrica) : 0);
                    strSql.AppendFormat(",ISOBITONEONATAL={0}", input.IsObitoNeoNatal.HasValue ? Convert.ToInt32(input.IsObitoNeoNatal) : 0);
                    strSql.AppendFormat(",SEOBITOMULHER={0}", input.SeObitoMulher.HasValue ? input.SeObitoMulher.Value.ToString() : "NULL");
                    strSql.AppendFormat(",QTDEOBITONEONATALPRECOCE={0}", input.QtdeObitoNeonatalPrecoce.HasValue ? input.QtdeObitoNeonatalPrecoce.Value.ToString() : "NULL");
                    strSql.AppendFormat(",QTDEOBITONEONATALTARDIO={0}", input.QtdeObitoNeonatalTardio.HasValue ? input.QtdeObitoNeonatalTardio.Value.ToString() : "NULL");
                    strSql.AppendFormat(",NUMDECLNASCVIVOS1={0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivos1) ? "NULL" : input.NumDeclNascVivos1);
                    strSql.AppendFormat(",QTDENASCVIVOSTERMO={0}", input.QtdeNascVivosTermo.HasValue ? input.QtdeNascVivosTermo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",QTDENASCMORTOS={0}", input.QtdeNascMortos.HasValue ? input.QtdeNascMortos.Value.ToString() : "NULL");
                    strSql.AppendFormat(",QTDENASCVIVOSPREMATURO={0}", input.QtdeNascVivosPrematuro.HasValue ? input.QtdeNascVivosPrematuro.Value.ToString() : "NULL");
                    strSql.AppendFormat(",NUMDECLNASCVIVOS2={0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivos2) ? "NULL" : input.NumDeclNascVivos2);
                    strSql.AppendFormat(",NUMDECLNASCVIVOS3={0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivos3) ? "NULL" : input.NumDeclNascVivos3);
                    strSql.AppendFormat(",NUMDECLNASCVIVOS4={0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivos4) ? "NULL" : input.NumDeclNascVivos4);
                    strSql.AppendFormat(",NUMDECLNASCVIVOS5={0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivos5) ? "NULL" : input.NumDeclNascVivos5);
                    strSql.AppendFormat(",TVTELEFONE={0}", input.TvTelefone.HasValue ? input.TvTelefone.Value.ToString() : "NULL");
                    strSql.AppendFormat(",QTDEALTA={0}", input.QtdeAlta.HasValue ? input.QtdeAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",QTDETRANSF={0}", input.QtdeTransf.HasValue ? input.QtdeTransf.Value.ToString() : "NULL");
                    strSql.AppendFormat(",SISPRENATAL={0}", string.IsNullOrWhiteSpace(input.SisPreNatal) ? "NULL" : input.SisPreNatal);
                    //strSql.AppendFormat(",JUSTIFICATIVASUS20={0}", input.JustificativaSUS20); // verificar os campos Marcus
                    //strSql.AppendFormat(",JUSTIFICATIVASUS21={0}", input.JustificativaSUS21);
                    //strSql.AppendFormat(",JUSTIFICATIVASUS22={0}", input.JustificativaSUS22);
                    strSql.AppendFormat(",IDACOMPANHANTE={0}", input.IDAcompanhante.HasValue ? input.IDAcompanhante.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISALERGIASZN={0}", input.IsAlergiaSzn.HasValue ? Convert.ToInt32(input.IsAlergiaSzn) : 0);
                    strSql.AppendFormat(",QUALALERGIASZN={0}", string.IsNullOrWhiteSpace(input.QualAlergiaSzn) ? "NULL" : input.QualAlergiaSzn);
                    strSql.AppendFormat(",TEMCAFE={0}", input.TemCafe.HasValue ? Convert.ToInt32(input.TemCafe) : 0);
                    strSql.AppendFormat(",TEMFRALDA={0}", input.TemFralda.HasValue ? Convert.ToInt32(input.TemFralda) : 0);
                    strSql.AppendFormat(",TEMREFEICAO={0}", input.TemRefeicao.HasValue ? Convert.ToInt32(input.TemRefeicao) : 0);
                    strSql.AppendFormat(",TEMPERNOITE={0}", input.TemPernoite.HasValue ? Convert.ToInt32(input.TemPernoite) : 0);
                    strSql.AppendFormat(",TEMREFEICAOACOMPANHANTE={0}", input.TemRefeicaoAcompanhante.HasValue ? Convert.ToInt32(input.TemRefeicaoAcompanhante) : 0);
                    strSql.AppendFormat(",COBERTURA={0}{1}{0}", input.Cobertura.HasValue ? "'" : string.Empty, input.Cobertura.HasValue ? input.Cobertura.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIETAATUAL={0}", string.IsNullOrWhiteSpace(input.DietaAtual) ? "NULL" : input.DietaAtual);
                    strSql.AppendFormat(",QUANTFRALDA={0}{1}{0}", input.QuantFralda.HasValue ? "'" : string.Empty, input.QuantFralda.HasValue ? input.QuantFralda.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",DATAPREVISAOALTA={0}{1}{0}", input.DataPrevisaoAlta.HasValue ? "'" : string.Empty, input.DataPrevisaoAlta.HasValue ? input.DataPrevisaoAlta.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",IDUSUARIOPREVALTAINC={0}", input.IDUsuarioAltaInc.HasValue ? Convert.ToInt32(input.IDUsuarioAltaInc) : 0);
                    //strSql.AppendFormat(",DATAPREVALTAINC={0}{1}{0}", input.DataPrevAltaInc.HasValue ? "'" : string.Empty, input.DataPrevAltaInc.HasValue ? input.DataPrevAltaInc.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",IDUSUARIOPREVALTAALT={0}", input.IDUsuarioPrevAltaAlt.HasValue ? Convert.ToInt32(input.IDUsuarioPrevAltaAlt) : 0);
                    //strSql.AppendFormat(",DATAPREVALTAALT={0}{1}{0}", input.DataPrevAltaAlt.HasValue ? "'" : string.Empty, input.DataPrevAltaAlt.HasValue ? input.DataPrevAltaAlt.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",IDUSUARIOPREVALTADEL={0}", input.IDUsuarioPrevAltaDel.HasValue ? Convert.ToInt32(input.IDUsuarioPrevAltaDel) : 0);
                    //strSql.AppendFormat(",DATAPREVALTADEL={0}{1}{0} ", input.DataPrevAltaDel.HasValue ? "'" : string.Empty, input.DataPrevAltaDel.HasValue ? input.DataPrevAltaDel.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(" WHERE IDINTERNACAO={0}", input.IDInternacao);
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_INTERNACAO SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDINTERNACAO=@IDINTERNACAO ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDINTERNACAO", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Sis_Internacao input)
            {
                strSql = new StringBuilder();
                try
                {
                    if (string.IsNullOrWhiteSpace(input.CodInternacao) && (string.IsNullOrWhiteSpace(input.CodInternacao) && input.CodInternacao != "0"))
                    {
                        var cod = await ObterCodigo("SIS_INTERNACAO", _cnAsa); // await cmd.ExecuteScalarAsync();
                        input.CodInternacao = cod.ToString();
                    }
                    strSql.AppendFormat("INSERT INTO SIS_INTERNACAO(");
                    strSql.AppendFormat("IDINTERNACAO, CODINTERNACAO, IDLEITO, IDLEITOTIPO, DATAALTA, IDALTA, TEMACOMPANHANTE, ");
                    strSql.AppendFormat("RESPONSAVEL, ENDRESPONSA, COMPRESPONSA, CEPRESPONSA, IDBAIRRORESPONSA, IDCIDADERESPONSA, ");
                    strSql.AppendFormat("IDESTADORESPONSA, PAISRESPONSA, IDTRESPONSA, ORGEMISRESPONSA, EMISIDTRESPONSA, CPFRESPONSA, ");
                    strSql.AppendFormat("CGCRESPONSA, IDESTADOPAC, STATUSPRONT, IDUSUARIOPRONT, DATAPRONT, NUMOBITO, IDUSUARIOALTAINC, ");
                    strSql.AppendFormat("DATAALTAINC, IDUSUARIOALTAALT, DATAALTAALT, IDUSUARIOALTADEL, DATAALTADEL, ISELETIVA, ");
                    strSql.AppendFormat("IDCIDOBITO, ISGESTACAO, ISABORTO, ISTRANSMAT, ISCOMPPUERPERIO, ISATENDRNSALAPARTO, ");
                    strSql.AppendFormat("ISCOMPNEONATAL, ISBXPESO, ISCESAREA, ISNORMAL, ISINTERNACAOOBSTETRICA, ISOBITONEONATAL, ");
                    strSql.AppendFormat("SEOBITOMULHER, QTDEOBITONEONATALPRECOCE, QTDEOBITONEONATALTARDIO, NUMDECLNASCVIVOS1, ");
                    strSql.AppendFormat("QTDENASCVIVOSTERMO, QTDENASCMORTOS, QTDENASCVIVOSPREMATURO, NUMDECLNASCVIVOS2, ");
                    strSql.AppendFormat("NUMDECLNASCVIVOS3, NUMDECLNASCVIVOS4, NUMDECLNASCVIVOS5, TVTELEFONE, QTDEALTA, QTDETRANSF, ");
                    strSql.AppendFormat("SISPRENATAL, JUSTIFICATIVASUS20, JUSTIFICATIVASUS21, JUSTIFICATIVASUS22, IDACOMPANHANTE, ");
                    strSql.AppendFormat("ISALERGIASZN, QUALALERGIASZN, TEMCAFE, TEMFRALDA, TEMREFEICAO, TEMPERNOITE, ");
                    strSql.AppendFormat("TEMREFEICAOACOMPANHANTE, COBERTURA, DIETAATUAL, QUANTFRALDA,IDSW"); // DATAPREVISAOALTA, ");
                                                                                                             //strSql.AppendFormat("IDUSUARIOPREVALTAINC, DATAPREVALTAINC, IDUSUARIOPREVALTAALT, DATAPREVALTAALT, ");
                                                                                                             //strSql.AppendFormat("IDUSUARIOPREVALTADEL, DATAPREVALTADEL");
                    strSql.AppendFormat(") ");
                    //strSql.AppendFormat("output INSERTED.IDINTERNACAO ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.IDInternacao);
                    strSql.AppendFormat(",{0}", input.CodInternacao);
                    strSql.AppendFormat(",{0}", input.IDLeito.HasValue ? input.IDLeito.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDLeitoTipo.HasValue ? input.IDLeitoTipo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAlta.HasValue ? "'" : string.Empty, input.DataAlta.HasValue ? input.DataAlta.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAlta.HasValue ? input.IDAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.TemAcompanhante.HasValue ? Convert.ToInt32(input.TemAcompanhante) : 0);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.Responsavel) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.Responsavel) ? "NULL" : input.Responsavel);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.EndResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.EndResponsa) ? "NULL" : input.EndResponsa);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.CompResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.CompResponsa) ? "NULL" : input.CompResponsa);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.CEPResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.CEPResponsa) ? "NULL" : input.CEPResponsa);
                    strSql.AppendFormat(",{0}", input.IDCidadeResponsa.HasValue ? input.IDCidadeResponsa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDEstadoResponsa.HasValue ? input.IDEstadoResponsa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDBairroResponsa.HasValue ? input.IDBairroResponsa.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.PaisResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.PaisResponsa) ? "NULL" : input.PaisResponsa);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.IdtResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.IdtResponsa) ? "NULL" : input.IdtResponsa);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.OrgEmisResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.OrgEmisResponsa) ? "NULL" : input.OrgEmisResponsa);
                    strSql.AppendFormat(",{0}{1}{0}", input.EmisIdtResponsa.HasValue ? "'" : string.Empty, input.EmisIdtResponsa.HasValue ? input.EmisIdtResponsa.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.CPFResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.CPFResponsa) ? "NULL" : input.CPFResponsa);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.CGCResponsa) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.CGCResponsa) ? "NULL" : input.CGCResponsa);
                    strSql.AppendFormat(",{0}", input.IDEstadoPac.HasValue ? input.IDEstadoPac.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.StatusPront.HasValue ? input.StatusPront.Value : 0);
                    strSql.AppendFormat(",{0}", input.IDUsuarioPront.HasValue ? input.IDUsuarioPront.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataPront.HasValue ? "'" : string.Empty, input.DataPront.HasValue ? input.DataPront.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.NumObito) ? "NULL" : input.NumObito);
                    strSql.AppendFormat(",{0}", input.IDUsuarioAltaInc.HasValue ? input.IDUsuarioAltaInc.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAltaInc.HasValue ? "'" : string.Empty, input.DataAltaInc.HasValue ? input.DataAltaInc.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioAltaAlt.HasValue ? input.IDUsuarioAltaAlt.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAltaAlt.HasValue ? "'" : string.Empty, input.DataAltaAlt.HasValue ? input.DataAltaAlt.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioAltaDel.HasValue ? input.IDUsuarioAltaDel.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAltaDel.HasValue ? "'" : string.Empty, input.DataAltaDel.HasValue ? input.DataAltaDel.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IsEletiva.HasValue ? Convert.ToInt32(input.IsEletiva) : 0);
                    strSql.AppendFormat(",{0}", input.IDCIDObito.HasValue ? input.IDCIDObito.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsGestacao.HasValue ? Convert.ToInt32(input.IsGestacao) : 0);
                    strSql.AppendFormat(",{0}", input.IsAborto.HasValue ? Convert.ToInt32(input.IsAborto) : 0);
                    strSql.AppendFormat(",{0}", input.IsTransMat.HasValue ? Convert.ToInt32(input.IsTransMat) : 0);
                    strSql.AppendFormat(",{0}", input.IsCompPuerperio.HasValue ? Convert.ToInt32(input.IsCompPuerperio) : 0);
                    strSql.AppendFormat(",{0}", input.IsAtendRNSalaParto.HasValue ? Convert.ToInt32(input.IsAtendRNSalaParto) : 0);
                    strSql.AppendFormat(",{0}", input.IsCompNeoNatal.HasValue ? Convert.ToInt32(input.IsCompNeoNatal) : 0);
                    strSql.AppendFormat(",{0}", input.IsBxPeso.HasValue ? Convert.ToInt32(input.IsBxPeso) : 0);
                    strSql.AppendFormat(",{0}", input.IsCesarea.HasValue ? Convert.ToInt32(input.IsCesarea) : 0);
                    strSql.AppendFormat(",{0}", input.IsNormal.HasValue ? Convert.ToInt32(input.IsNormal) : 0);
                    strSql.AppendFormat(",{0}", input.IsInternacaoObstetrica.HasValue ? Convert.ToInt32(input.IsInternacaoObstetrica) : 0);
                    strSql.AppendFormat(",{0}", input.IsObitoNeoNatal.HasValue ? Convert.ToInt32(input.IsObitoNeoNatal) : 0);
                    strSql.AppendFormat(",{0}", input.SeObitoMulher.HasValue ? input.SeObitoMulher.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.QtdeObitoNeonatalPrecoce.HasValue ? input.QtdeObitoNeonatalPrecoce.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.QtdeObitoNeonatalTardio.HasValue ? input.QtdeObitoNeonatalTardio.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.NumDeclNascVivos1) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.NumDeclNascVivos1) ? "NULL" : input.NumDeclNascVivos1);
                    strSql.AppendFormat(",{0}", input.QtdeNascVivosTermo.HasValue ? input.QtdeNascVivosTermo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.QtdeNascMortos.HasValue ? input.QtdeNascMortos.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.QtdeNascVivosPrematuro.HasValue ? input.QtdeNascVivosPrematuro.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.NumDeclNascVivos2) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.NumDeclNascVivos2) ? "NULL" : input.NumDeclNascVivos2);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.NumDeclNascVivos3) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.NumDeclNascVivos3) ? "NULL" : input.NumDeclNascVivos3);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.NumDeclNascVivos4) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.NumDeclNascVivos4) ? "NULL" : input.NumDeclNascVivos4);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.NumDeclNascVivos5) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.NumDeclNascVivos5) ? "NULL" : input.NumDeclNascVivos5);
                    strSql.AppendFormat(",{0}", input.TvTelefone.HasValue ? input.TvTelefone.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.QtdeAlta.HasValue ? input.QtdeAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.QtdeTransf.HasValue ? input.QtdeTransf.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.SisPreNatal) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.SisPreNatal) ? "NULL" : input.SisPreNatal);
                    strSql.AppendFormat(",'{0}'", input.JustificativaSUS20); // verificar os campos Marcus
                    strSql.AppendFormat(",'{0}'", input.JustificativaSUS21);
                    strSql.AppendFormat(",'{0}'", input.JustificativaSUS22);
                    strSql.AppendFormat(",{0}", input.IDAcompanhante.HasValue ? input.IDAcompanhante.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsAlergiaSzn.HasValue ? Convert.ToInt32(input.IsAlergiaSzn) : 0);
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.QualAlergiaSzn) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.QualAlergiaSzn) ? "NULL" : input.QualAlergiaSzn);
                    strSql.AppendFormat(",{0}", input.TemCafe.HasValue ? Convert.ToInt32(input.TemCafe) : 0);
                    strSql.AppendFormat(",{0}", input.TemFralda.HasValue ? Convert.ToInt32(input.TemFralda) : 0);
                    strSql.AppendFormat(",{0}", input.TemRefeicao.HasValue ? Convert.ToInt32(input.TemRefeicao) : 0);
                    strSql.AppendFormat(",{0}", input.TemPernoite.HasValue ? Convert.ToInt32(input.TemPernoite) : 0);
                    strSql.AppendFormat(",{0}", input.TemRefeicaoAcompanhante.HasValue ? Convert.ToInt32(input.TemRefeicaoAcompanhante) : 0);
                    strSql.AppendFormat(",{0}{1}{0}", input.Cobertura.HasValue ? "'" : string.Empty, input.Cobertura.HasValue ? input.Cobertura.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", !string.IsNullOrWhiteSpace(input.DietaAtual) ? "'" : string.Empty, string.IsNullOrWhiteSpace(input.DietaAtual) ? "NULL" : input.DietaAtual);
                    strSql.AppendFormat(",{0}{1}{0}", input.QuantFralda.HasValue ? "'" : string.Empty, input.QuantFralda.HasValue ? input.QuantFralda.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",{0}{1}{0}", input.DataPrevisaoAlta.HasValue ? "'" : string.Empty, input.DataPrevisaoAlta.HasValue ? input.DataPrevisaoAlta.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",{0}", input.IDUsuarioAltaInc.HasValue ? Convert.ToInt32(input.IDUsuarioAltaInc) : 0);
                    //strSql.AppendFormat(",{0}{1}{0}", input.DataPrevAltaInc.HasValue ? "'" : string.Empty, input.DataPrevAltaInc.HasValue ? input.DataPrevAltaInc.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",{0}", input.IDUsuarioPrevAltaAlt.HasValue ? Convert.ToInt32(input.IDUsuarioPrevAltaAlt) : 0);
                    //strSql.AppendFormat(",{0}{1}{0}", input.DataPrevAltaAlt.HasValue ? "'" : string.Empty, input.DataPrevAltaAlt.HasValue ? input.DataPrevAltaAlt.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    //strSql.AppendFormat(",{0}", input.IDUsuarioPrevAltaDel.HasValue ? Convert.ToInt32(input.IDUsuarioPrevAltaDel) : 0);
                    //strSql.AppendFormat(",{0}{1}{0} ", input.DataPrevAltaDel.HasValue ? "'" : string.Empty, input.DataPrevAltaDel.HasValue ? input.DataPrevAltaDel.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDSW.Value);
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();
                        cmd.Dispose();

                        strSql = new StringBuilder();
                        strSql.AppendFormat("UPDATE SIS_ATENDIMENTO SET CODATENDIMENTO='{0}' WHERE IDATENDIMENTO={1}", input.CodInternacao, input.IDInternacao);

                        cmd = new SqlCommand(strSql.ToString(), conn);

                        await cmd.ExecuteNonQueryAsync();

                        cmd.Dispose();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    //}
                    //var record = await Obter(input.IDSW.ToString());
                    var result = input.IDInternacao.Value;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Sis_Internacao>> Listar()
            {
                var result = new List<Sis_Internacao>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("INTERN.IDINTERNACAO,INTERN.CODINTERNACAO,INTERN.IDLEITO,INTERN.IDLEITOTIPO,INTERN.DATAALTA,INTERN.IDALTA,INTERN.TEMACOMPANHANTE ");
                strSql.AppendFormat(",INTERN.RESPONSAVEL,INTERN.ENDRESPONSA,INTERN.COMPRESPONSA,INTERN.CEPRESPONSA,INTERN.IDBAIRRORESPONSA,INTERN.IDCIDADERESPONSA ");
                strSql.AppendFormat(",INTERN.IDESTADORESPONSA,INTERN.PAISRESPONSA,INTERN.IDTRESPONSA,INTERN.ORGEMISRESPONSA,INTERN.EMISIDTRESPONSA,INTERN.CPFRESPONSA ");
                strSql.AppendFormat(",INTERN.CGCRESPONSA,INTERN.IDESTADOPAC,INTERN.STATUSPRONT,INTERN.IDUSUARIOPRONT,INTERN.DATAPRONT,INTERN.NUMOBITO,INTERN.IDUSUARIOALTAINC ");
                strSql.AppendFormat(",INTERN.DATAALTAINC,INTERN.IDUSUARIOALTAALT,INTERN.DATAALTAALT,INTERN.IDUSUARIOALTADEL,INTERN.DATAALTADEL,INTERN.ISELETIVA ");
                strSql.AppendFormat(",INTERN.IDCIDOBITO,INTERN.ISGESTACAO,INTERN.ISABORTO,INTERN.ISTRANSMAT,INTERN.ISCOMPPUERPERIO,INTERN.ISATENDRNSALAPARTO ");
                strSql.AppendFormat(",INTERN.ISCOMPNEONATAL,INTERN.ISBXPESO,INTERN.ISCESAREA,INTERN.ISNORMAL,INTERN.ISINTERNACAOOBSTETRICA,INTERN.ISOBITONEONATAL ");
                strSql.AppendFormat(",INTERN.SEOBITOMULHER,INTERN.QTDEOBITONEONATALPRECOCE,INTERN.QTDEOBITONEONATALTARDIO,INTERN.NUMDECLNASCVIVOS1 ");
                strSql.AppendFormat(",INTERN.QTDENASCVIVOSTERMO,INTERN.QTDENASCMORTOS,INTERN.QTDENASCVIVOSPREMATURO,INTERN.NUMDECLNASCVIVOS2 ");
                strSql.AppendFormat(",INTERN.NUMDECLNASCVIVOS3,INTERN.NUMDECLNASCVIVOS4,INTERN.NUMDECLNASCVIVOS5,INTERN.TVTELEFONE,INTERN.QTDEALTA,INTERN.QTDETRANSF ");
                strSql.AppendFormat(",INTERN.SISPRENATAL,INTERN.JUSTIFICATIVASUS20,INTERN.JUSTIFICATIVASUS21,INTERN.JUSTIFICATIVASUS22,INTERN.IDACOMPANHANTE ");
                strSql.AppendFormat(",INTERN.ISALERGIASZN,INTERN.QUALALERGIASZN,INTERN.TEMCAFE,INTERN.TEMFRALDA,INTERN.TEMREFEICAO,INTERN.TEMPERNOITE ");
                strSql.AppendFormat(",INTERN.TEMREFEICAOACOMPANHANTE,INTERN.COBERTURA,INTERN.DIETAATUAL,INTERN.QUANTFRALDA "); //,INTERN.DATAPREVISAOALTA ");
                strSql.AppendFormat("FROM SIS_INTERNACAO INTERN");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Sis_Internacao();

                        item.IDInternacao = Convert.ToInt32(listDb["IDINTERNACAO"]);
                        item.CodInternacao = Convert.ToString(listDb["CODINTERNACAO"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITO"].ToString()))
                        {
                            item.IDLeito = Convert.ToInt32(listDb["IDLEITO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITOTIPO"].ToString()))
                        {
                            item.IDLeitoTipo = Convert.ToInt32(listDb["IDLEITOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTA"].ToString()))
                        {
                            item.DataAlta = Convert.ToDateTime(listDb["DATAALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMACOMPANHANTE"].ToString()))
                        {
                            item.TemAcompanhante = Convert.ToBoolean(listDb["TEMACOMPANHANTE"]);
                        }
                        item.Responsavel = listDb["CODDEPENDENTE"].ToString();
                        item.EndResponsa = listDb["NUMEROGUIA"].ToString();
                        item.CompResponsa = listDb["TITULAR"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBAIRRORESPONSA"].ToString()))
                        {
                            item.IDBairroResponsa = Convert.ToInt32(listDb["IDBAIRRORESPONSA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDADERESPONSA"].ToString()))
                        {
                            item.IDCidadeResponsa = Convert.ToInt32(listDb["IDCIDADERESPONSA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADORESPONSA"].ToString()))
                        {
                            item.IDEstadoResponsa = Convert.ToInt32(listDb["IDESTADORESPONSA"]);
                        }
                        item.PaisResponsa = Convert.ToString(listDb["PaisResponsa"]);
                        item.IdtResponsa = listDb["OBSERVACAO"].ToString();
                        item.OrgEmisResponsa = Convert.ToString(listDb["IDPENDENCIAMOTIVO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["EMISIDTRESPONSA"].ToString()))
                        {
                            item.EmisIdtResponsa = Convert.ToDateTime(listDb["EMISIDTRESPONSA"]);
                        }
                        item.CPFResponsa = Convert.ToString(listDb["IDUSUARIOCONFERENCIA"]);
                        item.CGCResponsa = Convert.ToString(listDb["ISSINCRONIZADO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADOPAC"].ToString()))
                        {
                            item.IDEstadoPac = Convert.ToInt32(listDb["IDESTADOPAC"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["ID"].ToString()))
                        //{
                        //    item.StatusPront = Convert.ToInt32(listDb["ID"]);
                        //}
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPRONT"].ToString()))
                        {
                            item.IDUsuarioPront = Convert.ToInt32(listDb["IDUSUARIOPRONT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPRONT"].ToString()))
                        {
                            item.DataPront = Convert.ToDateTime(listDb["DATAPRONT"]);
                        }
                        item.NumObito = Convert.ToString(listDb["IDMEDICO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAINC"].ToString()))
                        {
                            item.IDUsuarioAltaInc = Convert.ToInt32(listDb["IDUSUARIOALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAINC"].ToString()))
                        {
                            item.DataAltaInc = Convert.ToDateTime(listDb["DATAALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAALT"].ToString()))
                        {
                            item.IDUsuarioAltaAlt = Convert.ToInt32(listDb["IDUSUARIOALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAALT"].ToString()))
                        {
                            item.DataAltaAlt = Convert.ToDateTime(listDb["DATAALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTADEL"].ToString()))
                        {
                            item.IDUsuarioAltaDel = Convert.ToInt32(listDb["IDUSUARIOALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTADEL"].ToString()))
                        {
                            item.DataAltaDel = Convert.ToDateTime(listDb["DATAALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISELETIVA"].ToString()))
                        {
                            item.IsEletiva = Convert.ToBoolean(listDb["ISELETIVA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDOBITO"].ToString()))
                        {
                            item.IDCIDObito = Convert.ToInt32(listDb["IDCIDOBITO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISGESTACAO"].ToString()))
                        {
                            item.IsGestacao = Convert.ToBoolean(listDb["ISGESTACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISABORTO"].ToString()))
                        {
                            item.IsAborto = Convert.ToBoolean(listDb["ISABORTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISTRANSMAT"].ToString()))
                        {
                            item.IsTransMat = Convert.ToBoolean(listDb["ISTRANSMAT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPPUERPERIO"].ToString()))
                        {
                            item.IsCompPuerperio = Convert.ToBoolean(listDb["ISCOMPPUERPERIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISATENDRNSALAPARTO"].ToString()))
                        {
                            item.IsAtendRNSalaParto = Convert.ToBoolean(listDb["ISATENDRNSALAPARTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPNEONATAL"].ToString()))
                        {
                            item.IsCompNeoNatal = Convert.ToBoolean(listDb["ISCOMPNEONATAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISBXPESO"].ToString()))
                        {
                            item.IsBxPeso = Convert.ToBoolean(listDb["ISBXPESO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCESAREA"].ToString()))
                        {
                            item.IsCesarea = Convert.ToBoolean(listDb["ISCESAREA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISNORMAL"].ToString()))
                        {
                            item.IsNormal = Convert.ToBoolean(listDb["ISNORMAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISINTERNACAOOBSTETRICA"].ToString()))
                        {
                            item.IsInternacaoObstetrica = Convert.ToBoolean(listDb["ISINTERNACAOOBSTETRICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISOBITONEONATAL"].ToString()))
                        {
                            item.IsObitoNeoNatal = Convert.ToBoolean(listDb["ISOBITONEONATAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SEOBITOMULHER"].ToString()))
                        {
                            item.SeObitoMulher = Convert.ToInt32(listDb["SEOBITOMULHER"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEOBITONEONATALPRECOCE"].ToString()))
                        {
                            item.QtdeObitoNeonatalPrecoce = Convert.ToInt32(listDb["QTDEOBITONEONATALPRECOCE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEOBITONEONATALTARDIO"].ToString()))
                        {
                            item.QtdeObitoNeonatalTardio = Convert.ToInt32(listDb["QTDEOBITONEONATALTARDIO"]);
                        }
                        item.NumDeclNascVivos1 = Convert.ToString(listDb["NUMDECLNASCVIVOS1"]);
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCVIVOSTERMO"].ToString()))
                        {
                            item.QtdeNascVivosTermo = Convert.ToInt32(listDb["QTDENASCVIVOSTERMO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCMORTOS"].ToString()))
                        {
                            item.QtdeNascMortos = Convert.ToInt32(listDb["QTDENASCMORTOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCVIVOSPREMATURO"].ToString()))
                        {
                            item.QtdeNascVivosPrematuro = Convert.ToInt32(listDb["QTDENASCVIVOSPREMATURO"]);
                        }
                        item.NumDeclNascVivos2 = Convert.ToString(listDb["NUMDECLNASCVIVOS2"]);
                        item.NumDeclNascVivos3 = Convert.ToString(listDb["NUMDECLNASCVIVOS3"]);
                        item.NumDeclNascVivos4 = Convert.ToString(listDb["NUMDECLNASCVIVOS4"]);
                        item.NumDeclNascVivos5 = Convert.ToString(listDb["NUMDECLNASCVIVOS5"]);
                        if (!string.IsNullOrWhiteSpace(listDb["TVTELEFONE"].ToString()))
                        {
                            item.TvTelefone = Convert.ToInt32(listDb["TVTELEFONE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEALTA"].ToString()))
                        {
                            item.QtdeAlta = Convert.ToInt32(listDb["QTDEALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDETRANSF"].ToString()))
                        {
                            item.QtdeTransf = Convert.ToInt32(listDb["QTDETRANSF"]);
                        }
                        item.SisPreNatal = Convert.ToString(listDb["SISPRENATAL"]);
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS20"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS20"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS21"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS21"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS22"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS22"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDACOMPANHANTE"].ToString()))
                        {
                            item.IDAcompanhante = Convert.ToInt32(listDb["IDACOMPANHANTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALERGIASZN"].ToString()))
                        {
                            item.IsAlergiaSzn = Convert.ToBoolean(listDb["ISALERGIASZN"]);
                        }
                        item.QualAlergiaSzn = Convert.ToString(listDb["QUALALERGIASZN"]);
                        if (!string.IsNullOrWhiteSpace(listDb["TEMCAFE"].ToString()))
                        {
                            item.TemCafe = Convert.ToBoolean(listDb["TEMCAFE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMFRALDA"].ToString()))
                        {
                            item.TemFralda = Convert.ToBoolean(listDb["TEMFRALDA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMREFEICAO"].ToString()))
                        {
                            item.TemRefeicao = Convert.ToBoolean(listDb["TEMREFEICAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMPERNOITE"].ToString()))
                        {
                            item.TemPernoite = Convert.ToBoolean(listDb["TEMPERNOITE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMREFEICAOACOMPANHANTE"].ToString()))
                        {
                            item.TemRefeicaoAcompanhante = Convert.ToBoolean(listDb["TEMREFEICAOACOMPANHANTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["COBERTURA"].ToString()))
                        {
                            item.Cobertura = Convert.ToDateTime(listDb["COBERTURA"]);
                        }
                        item.DietaAtual = Convert.ToString(listDb["DIETAATUAL"]);
                        if (!string.IsNullOrWhiteSpace(listDb["QUANTFRALDA"].ToString()))
                        {
                            item.QuantFralda = Convert.ToInt32(listDb["QUANTFRALDA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREVISAOALTA"].ToString()))
                        {
                            item.DataPrevisaoAlta = Convert.ToDateTime(listDb["DATAPREVISAOALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTAINC"].ToString()))
                        {
                            item.IDUsuarioPrevAltaInc = Convert.ToInt32(listDb["IDUSUARIOPREVALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTAINC"].ToString()))
                        {
                            item.DataPrevAltaInc = Convert.ToDateTime(listDb["DATAPREVALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTAALT"].ToString()))
                        {
                            item.IDUsuarioPrevAltaAlt = Convert.ToInt32(listDb["IDUSUARIOPREVALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTAALT"].ToString()))
                        {
                            item.DataPrevAltaAlt = Convert.ToDateTime(listDb["DATAPREVALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTADEL"].ToString()))
                        {
                            item.IDUsuarioPrevAltaDel = Convert.ToInt32(listDb["IDUSUARIOPREVALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTADEL"].ToString()))
                        {
                            item.DataPrevAltaDel = Convert.ToDateTime(listDb["DATAPREVALTADEL"]);
                        }

                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Sis_Internacao> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("INTERN.IDINTERNACAO,INTERN.CODINTERNACAO,INTERN.IDLEITO,INTERN.IDLEITOTIPO,INTERN.DATAALTA,INTERN.IDALTA,INTERN.TEMACOMPANHANTE ");
                strSql.AppendFormat(",INTERN.RESPONSAVEL,INTERN.ENDRESPONSA,INTERN.COMPRESPONSA,INTERN.CEPRESPONSA,INTERN.IDBAIRRORESPONSA,INTERN.IDCIDADERESPONSA ");
                strSql.AppendFormat(",INTERN.IDESTADORESPONSA,INTERN.PAISRESPONSA,INTERN.IDTRESPONSA,INTERN.ORGEMISRESPONSA,INTERN.EMISIDTRESPONSA,INTERN.CPFRESPONSA ");
                strSql.AppendFormat(",INTERN.CGCRESPONSA,INTERN.IDESTADOPAC,INTERN.STATUSPRONT,INTERN.IDUSUARIOPRONT,INTERN.DATAPRONT,INTERN.NUMOBITO,INTERN.IDUSUARIOALTAINC ");
                strSql.AppendFormat(",INTERN.DATAALTAINC,INTERN.IDUSUARIOALTAALT,INTERN.DATAALTAALT,INTERN.IDUSUARIOALTADEL,INTERN.DATAALTADEL,INTERN.ISELETIVA ");
                strSql.AppendFormat(",INTERN.IDCIDOBITO,INTERN.ISGESTACAO,INTERN.ISABORTO,INTERN.ISTRANSMAT,INTERN.ISCOMPPUERPERIO,INTERN.ISATENDRNSALAPARTO ");
                strSql.AppendFormat(",INTERN.ISCOMPNEONATAL,INTERN.ISBXPESO,INTERN.ISCESAREA,INTERN.ISNORMAL,INTERN.ISINTERNACAOOBSTETRICA,INTERN.ISOBITONEONATAL ");
                strSql.AppendFormat(",INTERN.SEOBITOMULHER,INTERN.QTDEOBITONEONATALPRECOCE,INTERN.QTDEOBITONEONATALTARDIO,INTERN.NUMDECLNASCVIVOS1 ");
                strSql.AppendFormat(",INTERN.QTDENASCVIVOSTERMO,INTERN.QTDENASCMORTOS,INTERN.QTDENASCVIVOSPREMATURO,INTERN.NUMDECLNASCVIVOS2 ");
                strSql.AppendFormat(",INTERN.NUMDECLNASCVIVOS3,INTERN.NUMDECLNASCVIVOS4,INTERN.NUMDECLNASCVIVOS5,INTERN.TVTELEFONE,INTERN.QTDEALTA,INTERN.QTDETRANSF ");
                strSql.AppendFormat(",INTERN.SISPRENATAL,INTERN.JUSTIFICATIVASUS20,INTERN.JUSTIFICATIVASUS21,INTERN.JUSTIFICATIVASUS22,INTERN.IDACOMPANHANTE ");
                strSql.AppendFormat(",INTERN.ISALERGIASZN,INTERN.QUALALERGIASZN,INTERN.TEMCAFE,INTERN.TEMFRALDA,INTERN.TEMREFEICAO,INTERN.TEMPERNOITE ");
                strSql.AppendFormat(",INTERN.TEMREFEICAOACOMPANHANTE,INTERN.COBERTURA,INTERN.DIETAATUAL,INTERN.QUANTFRALDA "); //,INTERN.DATAPREVISAOALTA ");
                //strSql.AppendFormat(",INTERN.IDUSUARIOPREVALTAINC ");
                //,INTERN.TECNICO,INTERN.IDINTERNA,INTERN.IDCID,INTERN.RELIGIAOSZN,INTERN.TEMPOJEJUMSZN,INTERN.DATAALTAPREVISAO");
                //strSql.AppendFormat(",INTERN.DATAALTAPREVISAODEL,INTERN.IDUSUARIOALTAPREVISAODEL ");
                //INTERN.DATAPREVALTAINC,INTERN.IDUSUARIOPREVALTAALT,INTERN.DATAPREVALTAALT ");
                //strSql.AppendFormat(",INTERN.IDUSUARIOPREVALTADEL,INTERN.DATAPREVALTADEL ");
                strSql.AppendFormat("FROM SIS_INTERNACAO INTERN, SIS_ATENDIMENTO ATE ");
                strSql.AppendFormat("WHERE ATE.IDATENDIMENTO=INTERN.IDINTERNACAO ");
                strSql.AppendFormat("AND IDINTERNACAO={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Internacao();
                    if (listDb.HasRows)
                    {
                        listDb.Read();


                        item.IDInternacao = Convert.ToInt32(listDb["IDINTERNACAO"]);
                        item.CodInternacao = Convert.ToString(listDb["CODINTERNACAO"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITO"].ToString()))
                        {
                            item.IDLeito = Convert.ToInt32(listDb["IDLEITO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITOTIPO"].ToString()))
                        {
                            item.IDLeitoTipo = Convert.ToInt32(listDb["IDLEITOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTA"].ToString()))
                        {
                            item.DataAlta = Convert.ToDateTime(listDb["DATAALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMACOMPANHANTE"].ToString()))
                        {
                            item.TemAcompanhante = Convert.ToBoolean(listDb["TEMACOMPANHANTE"]);
                        }
                        item.Responsavel = listDb["RESPONSAVEL"].ToString();
                        item.EndResponsa = listDb["ENDRESPONSA"].ToString();
                        item.CompResponsa = listDb["COMPRESPONSA"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBAIRRORESPONSA"].ToString()))
                        {
                            item.IDBairroResponsa = Convert.ToInt32(listDb["IDBAIRRORESPONSA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDADERESPONSA"].ToString()))
                        {
                            item.IDCidadeResponsa = Convert.ToInt32(listDb["IDCIDADERESPONSA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADORESPONSA"].ToString()))
                        {
                            item.IDEstadoResponsa = Convert.ToInt32(listDb["IDESTADORESPONSA"]);
                        }
                        item.PaisResponsa = Convert.ToString(listDb["PAISRESPONSA"]);
                        item.IdtResponsa = listDb["IDTRESPONSA"].ToString();
                        item.OrgEmisResponsa = Convert.ToString(listDb["ORGEMISRESPONSA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["EMISIDTRESPONSA"].ToString()))
                        {
                            item.EmisIdtResponsa = Convert.ToDateTime(listDb["EMISIDTRESPONSA"]);
                        }
                        item.CPFResponsa = Convert.ToString(listDb["CPFRESPONSA"]);
                        item.CGCResponsa = Convert.ToString(listDb["CGCRESPONSA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADOPAC"].ToString()))
                        {
                            item.IDEstadoPac = Convert.ToInt32(listDb["IDESTADOPAC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSPRONT"].ToString()))
                        {
                            item.StatusPront = Convert.ToInt32(listDb["STATUSPRONT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPRONT"].ToString()))
                        {
                            item.IDUsuarioPront = Convert.ToInt32(listDb["IDUSUARIOPRONT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPRONT"].ToString()))
                        {
                            item.DataPront = Convert.ToDateTime(listDb["DATAPRONT"]);
                        }
                        item.NumObito = Convert.ToString(listDb["NUMOBITO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAINC"].ToString()))
                        {
                            item.IDUsuarioAltaInc = Convert.ToInt32(listDb["IDUSUARIOALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAINC"].ToString()))
                        {
                            item.DataAltaInc = Convert.ToDateTime(listDb["DATAALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAALT"].ToString()))
                        {
                            item.IDUsuarioAltaAlt = Convert.ToInt32(listDb["IDUSUARIOALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAALT"].ToString()))
                        {
                            item.DataAltaAlt = Convert.ToDateTime(listDb["DATAALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTADEL"].ToString()))
                        {
                            item.IDUsuarioAltaDel = Convert.ToInt32(listDb["IDUSUARIOALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTADEL"].ToString()))
                        {
                            item.DataAltaDel = Convert.ToDateTime(listDb["DATAALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISELETIVA"].ToString()))
                        {
                            item.IsEletiva = Convert.ToBoolean(listDb["ISELETIVA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDOBITO"].ToString()))
                        {
                            item.IDCIDObito = Convert.ToInt32(listDb["IDCIDOBITO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISGESTACAO"].ToString()))
                        {
                            item.IsGestacao = Convert.ToBoolean(listDb["ISGESTACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISABORTO"].ToString()))
                        {
                            item.IsAborto = Convert.ToBoolean(listDb["ISABORTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISTRANSMAT"].ToString()))
                        {
                            item.IsTransMat = Convert.ToBoolean(listDb["ISTRANSMAT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPPUERPERIO"].ToString()))
                        {
                            item.IsCompPuerperio = Convert.ToBoolean(listDb["ISCOMPPUERPERIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISATENDRNSALAPARTO"].ToString()))
                        {
                            item.IsAtendRNSalaParto = Convert.ToBoolean(listDb["ISATENDRNSALAPARTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPNEONATAL"].ToString()))
                        {
                            item.IsCompNeoNatal = Convert.ToBoolean(listDb["ISCOMPNEONATAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISBXPESO"].ToString()))
                        {
                            item.IsBxPeso = Convert.ToBoolean(listDb["ISBXPESO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCESAREA"].ToString()))
                        {
                            item.IsCesarea = Convert.ToBoolean(listDb["ISCESAREA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISNORMAL"].ToString()))
                        {
                            item.IsNormal = Convert.ToBoolean(listDb["ISNORMAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISINTERNACAOOBSTETRICA"].ToString()))
                        {
                            item.IsInternacaoObstetrica = Convert.ToBoolean(listDb["ISINTERNACAOOBSTETRICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISOBITONEONATAL"].ToString()))
                        {
                            item.IsObitoNeoNatal = Convert.ToBoolean(listDb["ISOBITONEONATAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SEOBITOMULHER"].ToString()))
                        {
                            item.SeObitoMulher = Convert.ToInt32(listDb["SEOBITOMULHER"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEOBITONEONATALPRECOCE"].ToString()))
                        {
                            item.QtdeObitoNeonatalPrecoce = Convert.ToInt32(listDb["QTDEOBITONEONATALPRECOCE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEOBITONEONATALTARDIO"].ToString()))
                        {
                            item.QtdeObitoNeonatalTardio = Convert.ToInt32(listDb["QTDEOBITONEONATALTARDIO"]);
                        }
                        item.NumDeclNascVivos1 = Convert.ToString(listDb["NUMDECLNASCVIVOS1"]);
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCVIVOSTERMO"].ToString()))
                        {
                            item.QtdeNascVivosTermo = Convert.ToInt32(listDb["QTDENASCVIVOSTERMO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCMORTOS"].ToString()))
                        {
                            item.QtdeNascMortos = Convert.ToInt32(listDb["QTDENASCMORTOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCVIVOSPREMATURO"].ToString()))
                        {
                            item.QtdeNascVivosPrematuro = Convert.ToInt32(listDb["QTDENASCVIVOSPREMATURO"]);
                        }
                        item.NumDeclNascVivos2 = Convert.ToString(listDb["NUMDECLNASCVIVOS2"]);
                        item.NumDeclNascVivos3 = Convert.ToString(listDb["NUMDECLNASCVIVOS3"]);
                        item.NumDeclNascVivos4 = Convert.ToString(listDb["NUMDECLNASCVIVOS4"]);
                        item.NumDeclNascVivos5 = Convert.ToString(listDb["NUMDECLNASCVIVOS5"]);
                        if (!string.IsNullOrWhiteSpace(listDb["TVTELEFONE"].ToString()))
                        {
                            item.TvTelefone = Convert.ToInt32(listDb["TVTELEFONE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEALTA"].ToString()))
                        {
                            item.QtdeAlta = Convert.ToInt32(listDb["QTDEALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDETRANSF"].ToString()))
                        {
                            item.QtdeTransf = Convert.ToInt32(listDb["QTDETRANSF"]);
                        }
                        item.SisPreNatal = Convert.ToString(listDb["SISPRENATAL"]);
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS20"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS20"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS21"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS21"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS22"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS22"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDACOMPANHANTE"].ToString()))
                        {
                            item.IDAcompanhante = Convert.ToInt32(listDb["IDACOMPANHANTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALERGIASZN"].ToString()))
                        {
                            item.IsAlergiaSzn = Convert.ToBoolean(listDb["ISALERGIASZN"]);
                        }
                        item.QualAlergiaSzn = Convert.ToString(listDb["QUALALERGIASZN"]);
                        if (!string.IsNullOrWhiteSpace(listDb["TEMCAFE"].ToString()))
                        {
                            item.TemCafe = Convert.ToBoolean(listDb["TEMCAFE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMFRALDA"].ToString()))
                        {
                            item.TemFralda = Convert.ToBoolean(listDb["TEMFRALDA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMREFEICAO"].ToString()))
                        {
                            item.TemRefeicao = Convert.ToBoolean(listDb["TEMREFEICAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMPERNOITE"].ToString()))
                        {
                            item.TemPernoite = Convert.ToBoolean(listDb["TEMPERNOITE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMREFEICAOACOMPANHANTE"].ToString()))
                        {
                            item.TemRefeicaoAcompanhante = Convert.ToBoolean(listDb["TEMREFEICAOACOMPANHANTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["COBERTURA"].ToString()))
                        {
                            item.Cobertura = Convert.ToDateTime(listDb["COBERTURA"]);
                        }
                        item.DietaAtual = Convert.ToString(listDb["DIETAATUAL"]);
                        if (!string.IsNullOrWhiteSpace(listDb["QUANTFRALDA"].ToString()))
                        {
                            item.QuantFralda = Convert.ToInt32(listDb["QUANTFRALDA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVISAOALTA"].ToString()))
                        //{
                        //    item.DataPrevisaoAlta = Convert.ToDateTime(listDb["DATAPREVISAOALTA"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTAINC"].ToString()))
                        //{
                        //    item.IDUsuarioPrevAltaInc = Convert.ToInt32(listDb["IDUSUARIOPREVALTAINC"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTAINC"].ToString()))
                        //{
                        //    item.DataPrevAltaInc = Convert.ToDateTime(listDb["DATAPREVALTAINC"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTAALT"].ToString()))
                        //{
                        //    item.IDUsuarioPrevAltaAlt = Convert.ToInt32(listDb["IDUSUARIOPREVALTAALT"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTAALT"].ToString()))
                        //{
                        //    item.DataPrevAltaAlt = Convert.ToDateTime(listDb["DATAPREVALTAALT"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTADEL"].ToString()))
                        //{
                        //    item.IDUsuarioPrevAltaDel = Convert.ToInt32(listDb["IDUSUARIOPREVALTADEL"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTADEL"].ToString()))
                        //{
                        //    item.DataPrevAltaDel = Convert.ToDateTime(listDb["DATAPREVALTADEL"]);
                        //}
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public async Task<Sis_Internacao> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("INTERN.IDINTERNACAO,INTERN.CODINTERNACAO,INTERN.IDLEITO,INTERN.IDLEITOTIPO,INTERN.DATAALTA,INTERN.IDALTA,INTERN.TEMACOMPANHANTE ");
                strSql.AppendFormat(",INTERN.RESPONSAVEL,INTERN.ENDRESPONSA,INTERN.COMPRESPONSA,INTERN.CEPRESPONSA,INTERN.IDBAIRRORESPONSA,INTERN.IDCIDADERESPONSA ");
                strSql.AppendFormat(",INTERN.IDESTADORESPONSA,INTERN.PAISRESPONSA,INTERN.IDTRESPONSA,INTERN.ORGEMISRESPONSA,INTERN.EMISIDTRESPONSA,INTERN.CPFRESPONSA ");
                strSql.AppendFormat(",INTERN.CGCRESPONSA,INTERN.IDESTADOPAC,INTERN.STATUSPRONT,INTERN.IDUSUARIOPRONT,INTERN.DATAPRONT,INTERN.NUMOBITO,INTERN.IDUSUARIOALTAINC ");
                strSql.AppendFormat(",INTERN.DATAALTAINC,INTERN.IDUSUARIOALTAALT,INTERN.DATAALTAALT,INTERN.IDUSUARIOALTADEL,INTERN.DATAALTADEL,INTERN.ISELETIVA ");
                strSql.AppendFormat(",INTERN.IDCIDOBITO,INTERN.ISGESTACAO,INTERN.ISABORTO,INTERN.ISTRANSMAT,INTERN.ISCOMPPUERPERIO,INTERN.ISATENDRNSALAPARTO ");
                strSql.AppendFormat(",INTERN.ISCOMPNEONATAL,INTERN.ISBXPESO,INTERN.ISCESAREA,INTERN.ISNORMAL,INTERN.ISINTERNACAOOBSTETRICA,INTERN.ISOBITONEONATAL ");
                strSql.AppendFormat(",INTERN.SEOBITOMULHER,INTERN.QTDEOBITONEONATALPRECOCE,INTERN.QTDEOBITONEONATALTARDIO,INTERN.NUMDECLNASCVIVOS1 ");
                strSql.AppendFormat(",INTERN.QTDENASCVIVOSTERMO,INTERN.QTDENASCMORTOS,INTERN.QTDENASCVIVOSPREMATURO,INTERN.NUMDECLNASCVIVOS2 ");
                strSql.AppendFormat(",INTERN.NUMDECLNASCVIVOS3,INTERN.NUMDECLNASCVIVOS4,INTERN.NUMDECLNASCVIVOS5,INTERN.TVTELEFONE,INTERN.QTDEALTA,INTERN.QTDETRANSF ");
                strSql.AppendFormat(",INTERN.SISPRENATAL,INTERN.JUSTIFICATIVASUS20,INTERN.JUSTIFICATIVASUS21,INTERN.JUSTIFICATIVASUS22,INTERN.IDACOMPANHANTE ");
                strSql.AppendFormat(",INTERN.ISALERGIASZN,INTERN.QUALALERGIASZN,INTERN.TEMCAFE,INTERN.TEMFRALDA,INTERN.TEMREFEICAO,INTERN.TEMPERNOITE ");
                strSql.AppendFormat(",INTERN.TEMREFEICAOACOMPANHANTE,INTERN.COBERTURA,INTERN.DIETAATUAL,INTERN.QUANTFRALDA "); //,INTERN.DATAPREVISAOALTA ");
                //strSql.AppendFormat(",INTERN.IDUSUARIOPREVALTAINC ");
                //,INTERN.TECNICO,INTERN.IDINTERNA,INTERN.IDCID,INTERN.RELIGIAOSZN,INTERN.TEMPOJEJUMSZN,INTERN.DATAALTAPREVISAO");
                //strSql.AppendFormat(",INTERN.DATAALTAPREVISAODEL,INTERN.IDUSUARIOALTAPREVISAODEL ");
                //INTERN.DATAPREVALTAINC,INTERN.IDUSUARIOPREVALTAALT,INTERN.DATAPREVALTAALT ");
                //strSql.AppendFormat(",INTERN.IDUSUARIOPREVALTADEL,INTERN.DATAPREVALTADEL ");
                strSql.AppendFormat(" FROM SIS_ATENDIMENTO ATE ");
                strSql.AppendFormat(" JOIN SIS_INTERNACAO INTERN ON ATE.IDATENDIMENTO=INTERN.IDINTERNACAO ");
                strSql.AppendFormat("AND ATE.IDSW={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Internacao();
                    if (listDb.HasRows)
                    {
                        listDb.Read();

                        item.IDInternacao = Convert.ToInt32(listDb["IDINTERNACAO"]);
                        item.CodInternacao = Convert.ToString(listDb["CODINTERNACAO"]);

                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITO"].ToString()))
                        {
                            item.IDLeito = Convert.ToInt32(listDb["IDLEITO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDLEITOTIPO"].ToString()))
                        {
                            item.IDLeitoTipo = Convert.ToInt32(listDb["IDLEITOTIPO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTA"].ToString()))
                        {
                            item.DataAlta = Convert.ToDateTime(listDb["DATAALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMACOMPANHANTE"].ToString()))
                        {
                            item.TemAcompanhante = Convert.ToBoolean(listDb["TEMACOMPANHANTE"]);
                        }
                        item.Responsavel = listDb["RESPONSAVEL"].ToString();
                        item.EndResponsa = listDb["ENDRESPONSA"].ToString();
                        item.CompResponsa = listDb["COMPRESPONSA"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDBAIRRORESPONSA"].ToString()))
                        {
                            item.IDBairroResponsa = Convert.ToInt32(listDb["IDBAIRRORESPONSA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDADERESPONSA"].ToString()))
                        {
                            item.IDCidadeResponsa = Convert.ToInt32(listDb["IDCIDADERESPONSA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADORESPONSA"].ToString()))
                        {
                            item.IDEstadoResponsa = Convert.ToInt32(listDb["IDESTADORESPONSA"]);
                        }
                        item.PaisResponsa = Convert.ToString(listDb["PAISRESPONSA"]);
                        item.IdtResponsa = listDb["IDTRESPONSA"].ToString();
                        item.OrgEmisResponsa = Convert.ToString(listDb["ORGEMISRESPONSA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["EMISIDTRESPONSA"].ToString()))
                        {
                            item.EmisIdtResponsa = Convert.ToDateTime(listDb["EMISIDTRESPONSA"]);
                        }
                        item.CPFResponsa = Convert.ToString(listDb["CPFRESPONSA"]);
                        item.CGCResponsa = Convert.ToString(listDb["CGCRESPONSA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDESTADOPAC"].ToString()))
                        {
                            item.IDEstadoPac = Convert.ToInt32(listDb["IDESTADOPAC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSPRONT"].ToString()))
                        {
                            item.StatusPront = Convert.ToInt32(listDb["STATUSPRONT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPRONT"].ToString()))
                        {
                            item.IDUsuarioPront = Convert.ToInt32(listDb["IDUSUARIOPRONT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPRONT"].ToString()))
                        {
                            item.DataPront = Convert.ToDateTime(listDb["DATAPRONT"]);
                        }
                        item.NumObito = Convert.ToString(listDb["NUMOBITO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAINC"].ToString()))
                        {
                            item.IDUsuarioAltaInc = Convert.ToInt32(listDb["IDUSUARIOALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAINC"].ToString()))
                        {
                            item.DataAltaInc = Convert.ToDateTime(listDb["DATAALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAALT"].ToString()))
                        {
                            item.IDUsuarioAltaAlt = Convert.ToInt32(listDb["IDUSUARIOALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAALT"].ToString()))
                        {
                            item.DataAltaAlt = Convert.ToDateTime(listDb["DATAALTAALT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTADEL"].ToString()))
                        {
                            item.IDUsuarioAltaDel = Convert.ToInt32(listDb["IDUSUARIOALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTADEL"].ToString()))
                        {
                            item.DataAltaDel = Convert.ToDateTime(listDb["DATAALTADEL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISELETIVA"].ToString()))
                        {
                            item.IsEletiva = Convert.ToBoolean(listDb["ISELETIVA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDCIDOBITO"].ToString()))
                        {
                            item.IDCIDObito = Convert.ToInt32(listDb["IDCIDOBITO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISGESTACAO"].ToString()))
                        {
                            item.IsGestacao = Convert.ToBoolean(listDb["ISGESTACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISABORTO"].ToString()))
                        {
                            item.IsAborto = Convert.ToBoolean(listDb["ISABORTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISTRANSMAT"].ToString()))
                        {
                            item.IsTransMat = Convert.ToBoolean(listDb["ISTRANSMAT"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPPUERPERIO"].ToString()))
                        {
                            item.IsCompPuerperio = Convert.ToBoolean(listDb["ISCOMPPUERPERIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISATENDRNSALAPARTO"].ToString()))
                        {
                            item.IsAtendRNSalaParto = Convert.ToBoolean(listDb["ISATENDRNSALAPARTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCOMPNEONATAL"].ToString()))
                        {
                            item.IsCompNeoNatal = Convert.ToBoolean(listDb["ISCOMPNEONATAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISBXPESO"].ToString()))
                        {
                            item.IsBxPeso = Convert.ToBoolean(listDb["ISBXPESO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCESAREA"].ToString()))
                        {
                            item.IsCesarea = Convert.ToBoolean(listDb["ISCESAREA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISNORMAL"].ToString()))
                        {
                            item.IsNormal = Convert.ToBoolean(listDb["ISNORMAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISINTERNACAOOBSTETRICA"].ToString()))
                        {
                            item.IsInternacaoObstetrica = Convert.ToBoolean(listDb["ISINTERNACAOOBSTETRICA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISOBITONEONATAL"].ToString()))
                        {
                            item.IsObitoNeoNatal = Convert.ToBoolean(listDb["ISOBITONEONATAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["SEOBITOMULHER"].ToString()))
                        {
                            item.SeObitoMulher = Convert.ToInt32(listDb["SEOBITOMULHER"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEOBITONEONATALPRECOCE"].ToString()))
                        {
                            item.QtdeObitoNeonatalPrecoce = Convert.ToInt32(listDb["QTDEOBITONEONATALPRECOCE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEOBITONEONATALTARDIO"].ToString()))
                        {
                            item.QtdeObitoNeonatalTardio = Convert.ToInt32(listDb["QTDEOBITONEONATALTARDIO"]);
                        }
                        item.NumDeclNascVivos1 = Convert.ToString(listDb["NUMDECLNASCVIVOS1"]);
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCVIVOSTERMO"].ToString()))
                        {
                            item.QtdeNascVivosTermo = Convert.ToInt32(listDb["QTDENASCVIVOSTERMO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCMORTOS"].ToString()))
                        {
                            item.QtdeNascMortos = Convert.ToInt32(listDb["QTDENASCMORTOS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDENASCVIVOSPREMATURO"].ToString()))
                        {
                            item.QtdeNascVivosPrematuro = Convert.ToInt32(listDb["QTDENASCVIVOSPREMATURO"]);
                        }
                        item.NumDeclNascVivos2 = Convert.ToString(listDb["NUMDECLNASCVIVOS2"]);
                        item.NumDeclNascVivos3 = Convert.ToString(listDb["NUMDECLNASCVIVOS3"]);
                        item.NumDeclNascVivos4 = Convert.ToString(listDb["NUMDECLNASCVIVOS4"]);
                        item.NumDeclNascVivos5 = Convert.ToString(listDb["NUMDECLNASCVIVOS5"]);
                        if (!string.IsNullOrWhiteSpace(listDb["TVTELEFONE"].ToString()))
                        {
                            item.TvTelefone = Convert.ToInt32(listDb["TVTELEFONE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDEALTA"].ToString()))
                        {
                            item.QtdeAlta = Convert.ToInt32(listDb["QTDEALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["QTDETRANSF"].ToString()))
                        {
                            item.QtdeTransf = Convert.ToInt32(listDb["QTDETRANSF"]);
                        }
                        item.SisPreNatal = Convert.ToString(listDb["SISPRENATAL"]);
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS20"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS20"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS21"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS21"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["JUSTIFICATIVASUS22"].ToString()))
                        {
                            item.JustificativaSUS20 = (byte[])listDb["JUSTIFICATIVASUS22"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDACOMPANHANTE"].ToString()))
                        {
                            item.IDAcompanhante = Convert.ToInt32(listDb["IDACOMPANHANTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALERGIASZN"].ToString()))
                        {
                            item.IsAlergiaSzn = Convert.ToBoolean(listDb["ISALERGIASZN"]);
                        }
                        item.QualAlergiaSzn = Convert.ToString(listDb["QUALALERGIASZN"]);
                        if (!string.IsNullOrWhiteSpace(listDb["TEMCAFE"].ToString()))
                        {
                            item.TemCafe = Convert.ToBoolean(listDb["TEMCAFE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMFRALDA"].ToString()))
                        {
                            item.TemFralda = Convert.ToBoolean(listDb["TEMFRALDA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMREFEICAO"].ToString()))
                        {
                            item.TemRefeicao = Convert.ToBoolean(listDb["TEMREFEICAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMPERNOITE"].ToString()))
                        {
                            item.TemPernoite = Convert.ToBoolean(listDb["TEMPERNOITE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["TEMREFEICAOACOMPANHANTE"].ToString()))
                        {
                            item.TemRefeicaoAcompanhante = Convert.ToBoolean(listDb["TEMREFEICAOACOMPANHANTE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["COBERTURA"].ToString()))
                        {
                            item.Cobertura = Convert.ToDateTime(listDb["COBERTURA"]);
                        }
                        item.DietaAtual = Convert.ToString(listDb["DIETAATUAL"]);
                        if (!string.IsNullOrWhiteSpace(listDb["QUANTFRALDA"].ToString()))
                        {
                            item.QuantFralda = Convert.ToInt32(listDb["QUANTFRALDA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVISAOALTA"].ToString()))
                        //{
                        //    item.DataPrevisaoAlta = Convert.ToDateTime(listDb["DATAPREVISAOALTA"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTAINC"].ToString()))
                        //{
                        //    item.IDUsuarioPrevAltaInc = Convert.ToInt32(listDb["IDUSUARIOPREVALTAINC"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTAINC"].ToString()))
                        //{
                        //    item.DataPrevAltaInc = Convert.ToDateTime(listDb["DATAPREVALTAINC"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTAALT"].ToString()))
                        //{
                        //    item.IDUsuarioPrevAltaAlt = Convert.ToInt32(listDb["IDUSUARIOPREVALTAALT"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTAALT"].ToString()))
                        //{
                        //    item.DataPrevAltaAlt = Convert.ToDateTime(listDb["DATAPREVALTAALT"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOPREVALTADEL"].ToString()))
                        //{
                        //    item.IDUsuarioPrevAltaDel = Convert.ToInt32(listDb["IDUSUARIOPREVALTADEL"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAPREVALTADEL"].ToString()))
                        //{
                        //    item.DataPrevAltaDel = Convert.ToDateTime(listDb["DATAPREVALTADEL"]);
                        //}
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class SisAmbulatorioRepositorio : IRepositorio<Sis_Ambulatorio>
        {
            private string _cnAsa;

            public SisAmbulatorioRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }

            public async Task Alterar(Sis_Ambulatorio input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_AMBULATORIO SET ");
                    //strSql.AppendFormat("CODAMBULATORIO='{0}'", input.CodAmbulatorio);
                    strSql.AppendFormat("DATAINICIO={0}", input.DataInicio.HasValue ? input.DataInicio.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",ISREVISAO={0}", input.IsRevisao.HasValue ? Convert.ToInt32(input.IsRevisao) : 0);
                    strSql.AppendFormat(",ISHORAMARCADA={0}", input.IsHoraMarcada.HasValue ? Convert.ToInt32(input.IsHoraMarcada) : 0);
                    strSql.AppendFormat(",DATARETORNO={0}{1}{0}", input.DataRetorno.HasValue ? "'" : string.Empty, input.DataRetorno.HasValue ? input.DataRetorno.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDATENDREVISAO={0}", input.IDAtendRevisao.HasValue ? input.IDAtendRevisao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDALTA={0}", input.IDAlta.HasValue ? input.IDAlta.Value.ToString() : "NULL");
                    strSql.AppendFormat(",STATUSPRONTOATEND={0}", input.StatusProntoAtend.HasValue ? input.StatusProntoAtend.Value.ToString() : "NULL");
                    strSql.AppendFormat(",NUMEROSEQ={0}", string.IsNullOrWhiteSpace(input.NumeroSeq) ? "NULL" : input.NumeroSeq);
                    strSql.AppendFormat(",TIPOCONSULTA={0}", string.IsNullOrWhiteSpace(input.TipoConsulta) ? "NULL" : input.TipoConsulta);
                    strSql.AppendFormat(",ISVACINA={0}", input.IsVacina.HasValue ? Convert.ToInt32(input.IsVacina) : 0);
                    strSql.AppendFormat(",DATAEXAME={0}{1}{0}", input.DataExame.HasValue ? "'" : string.Empty, input.DataExame.HasValue ? input.DataExame.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDUSUARIOLIBERACAO={0}", input.IDUsuarioLiberacao.HasValue ? input.IDUsuarioLiberacao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATALIBERACAO={0}{1}{0}", input.DataLiberacao.HasValue ? "'" : string.Empty, input.DataLiberacao.HasValue ? input.DataPreAtend.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDPRIORIDADEATENDIMENTO={0}", input.IDPrioridadeAtendimento.HasValue ? input.IDPrioridadeAtendimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISALTAREVELIA={0}", input.IsAltaRevelia.HasValue ? Convert.ToInt32(input.IsAltaRevelia) : 0);
                    strSql.AppendFormat(",IDUSUARIOREVELIA={0}", input.IDUsuarioRevelia.HasValue ? input.IDUsuarioRevelia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATASOLICITACAO={0}{1}{0}", input.DataSolicitacao.HasValue ? "'" : string.Empty, input.DataSolicitacao.HasValue ? input.DataSolicitacao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",DIAGNOSTICO={0}", string.IsNullOrWhiteSpace(input.Diagnostico) ? "NULL" : input.Diagnostico);
                    strSql.AppendFormat(",TRATAMENTO={0}", string.IsNullOrWhiteSpace(input.Tratamento) ? "NULL" : input.Tratamento);
                    strSql.AppendFormat(",DADOSCLINICOS={0}", string.IsNullOrWhiteSpace(input.DadosClinicos) ? "NULL" : input.DadosClinicos);
                    strSql.AppendFormat(",PRIMCONSULTA={0}", string.IsNullOrWhiteSpace(input.PrimConsulta) ? "NULL" : input.PrimConsulta);
                    strSql.AppendFormat(",ISALERGIASZN={0}", input.IsAlergiaSzn.HasValue ? Convert.ToInt32(input.IsAlergiaSzn) : 0);
                    strSql.AppendFormat(",QUALALERGIASZN={0}", string.IsNullOrWhiteSpace(input.QualAlergiaSzn) ? "NULL" : input.QualAlergiaSzn);
                    strSql.AppendFormat(",DATAPREATEND={0}{1}{0}", input.DataPreAtend.HasValue ? "'" : string.Empty, input.DataPreAtend.HasValue ? input.DataPreAtend.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",CODAMBULATORIOSUS={0}", input.CodAmbulatorioSUS.HasValue ? input.CodAmbulatorioSUS.Value : 0);
                    strSql.AppendFormat(",IDSETOR={0}", input.IDSetor.HasValue ? input.IDSetor.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAALTAAMBULATORIAL={0}{1}{0}", input.DataAltaAmbulatorial.HasValue ? "'" : string.Empty, input.DataAltaAmbulatorial.HasValue ? input.DataAltaAmbulatorial.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDALTAAMBULATORIAL={0}", input.IDAltaAmbulatorial.HasValue ? input.IDAltaAmbulatorial.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDUSUARIOALTAINC={0}", input.IDUsuarioAltaInc.HasValue ? input.IDUsuarioAltaInc.Value.ToString() : "NULL");
                    strSql.AppendFormat(",IDMEDPREATEND={0}", input.IDMedPreAtend.HasValue ? input.IDMedPreAtend.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISATENDENDO={0}", input.IsAtendendo.HasValue ? Convert.ToInt32(input.IsAtendendo) : 0);
                    strSql.AppendFormat(",IDMEDICOATENDENDO={0}", input.IDMedicoAtendendo.HasValue ? input.IDMedicoAtendendo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",DATAATENDAMBULATORIAL={0}{1}{0}", input.DataAtendAmbulatorial.HasValue ? "'" : string.Empty, input.DataAtendAmbulatorial.HasValue ? input.DataAtendAmbulatorial.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",IDPROTOCOLOEMERGENCIA={0}", input.IDProtocoloEmergencia.HasValue ? input.IDProtocoloEmergencia.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",DATAINIPREATEND='{0}'", input.DataIniPreAtend.HasValue ? input.DataIniPreAtend.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",DATAINIINFOCLINICAS='{0}'", input.DataIniInfoClinicas.HasValue ? input.DataIniInfoClinicas.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",DATAFIMPREATEND={0}", input.DataFimPreAtend.HasValue ? input.DataFimPreAtend.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",DATAFIMINFOCLINICAS={0}", input.DataFimInfoClinicas.HasValue ? input.DataFimInfoClinicas.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",DATAALTAMEDICA={0}", input.DataAltaMedica.HasValue ? input.DataAltaMedica.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",DATAALTAADMINISTRATIVA={0}", input.DataAltaAdministrativa.HasValue ? input.DataAltaAdministrativa.Value.ToString() : "NULL");
                    strSql.AppendFormat(" WHERE IDAMBULATORIO={0}", input.IDAmbulatorio);
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_AMBULATORIO SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDAMBULATORIO=@IDAMBULATORIO ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDAMBULATORIO", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Sis_Ambulatorio input)
            {
                strSql = new StringBuilder();
                try
                {
                    if (string.IsNullOrWhiteSpace(input.CodAmbulatorio) && (string.IsNullOrWhiteSpace(input.CodAmbulatorio) && input.CodAmbulatorio != "0"))
                    {
                        var cod = await ObterCodigo("SIS_AMBULATORIO", _cnAsa); //await cmd.ExecuteScalarAsync();
                        input.CodAmbulatorio = cod.ToString();
                    }
                    strSql.AppendFormat("INSERT INTO SIS_AMBULATORIO(");
                    strSql.AppendFormat("IDAMBULATORIO, CODAMBULATORIO, DATAINICIO, ISREVISAO, ISHORAMARCADA, DATARETORNO, ");
                    strSql.AppendFormat("IDATENDREVISAO, IDALTA, STATUSPRONTOATEND, NUMEROSEQ, TIPOCONSULTA, ISVACINA, DATAEXAME, ");
                    strSql.AppendFormat("IDUSUARIOLIBERACAO, DATALIBERACAO, IDPRIORIDADEATENDIMENTO, ISALTAREVELIA, IDUSUARIOREVELIA, ");
                    strSql.AppendFormat("DATASOLICITACAO, DIAGNOSTICO, TRATAMENTO, DADOSCLINICOS, PRIMCONSULTA, ISALERGIASZN, ");
                    strSql.AppendFormat("QUALALERGIASZN, DATAPREATEND, CODAMBULATORIOSUS, IDSETOR, DATAALTAAMBULATORIAL, ");
                    strSql.AppendFormat("IDALTAAMBULATORIAL, IDUSUARIOALTAINC, IDMEDPREATEND, ISATENDENDO, IDMEDICOATENDENDO, ");
                    strSql.AppendFormat("DATAATENDAMBULATORIAL, IDPROTOCOLOEMERGENCIA,IDSW"); //, DATAINIPREATEND, DATAINIINFOCLINICAS, ");
                                                                                              //strSql.AppendFormat("DATAFIMPREATEND, DATAFIMINFOCLINICAS, DATAALTAMEDICA, DATAALTAADMINISTRATIVA");
                    strSql.AppendFormat(") ");
                    //strSql.AppendFormat("output INSERTED.IDAMBULATORIO ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.IDAmbulatorio);
                    strSql.AppendFormat(",'{0}'", input.CodAmbulatorio);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataInicio.HasValue ? "'" : "", input.DataInicio.HasValue ? input.DataInicio.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IsRevisao.HasValue ? Convert.ToInt32(input.IsRevisao) : 0);
                    strSql.AppendFormat(",{0}", input.IsHoraMarcada.HasValue ? Convert.ToInt32(input.IsHoraMarcada).ToString() : "0");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataRetorno.HasValue ? "'" : string.Empty, input.DataRetorno.HasValue ? input.DataRetorno.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAtendRevisao.HasValue ? input.IDAtendRevisao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAlta.HasValue ? input.IDAlta.Value.ToString() : "5");
                    strSql.AppendFormat(",{0}", input.StatusProntoAtend.HasValue ? input.StatusProntoAtend.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.NumeroSeq) ? "NULL" : input.NumeroSeq);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.TipoConsulta) ? "NULL" : input.TipoConsulta);
                    strSql.AppendFormat(",{0}", input.IsVacina.HasValue ? Convert.ToInt32(input.IsVacina) : 0);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataExame.HasValue ? "'" : string.Empty, input.DataExame.HasValue ? input.DataExame.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioLiberacao.HasValue ? input.IDUsuarioLiberacao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataLiberacao.HasValue ? "'" : string.Empty, input.DataLiberacao.HasValue ? input.DataLiberacao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDPrioridadeAtendimento.HasValue ? input.IDPrioridadeAtendimento.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsAltaRevelia.HasValue ? Convert.ToInt32(input.IsAltaRevelia) : 0);
                    strSql.AppendFormat(",{0}", input.IDUsuarioRevelia.HasValue ? input.IDUsuarioRevelia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataSolicitacao.HasValue ? "'" : string.Empty, input.DataSolicitacao.HasValue ? input.DataSolicitacao.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Diagnostico) ? "NULL" : input.Diagnostico);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Tratamento) ? "NULL" : input.Tratamento);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.DadosClinicos) ? "NULL" : input.DadosClinicos);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.PrimConsulta) ? "NULL" : input.PrimConsulta);
                    strSql.AppendFormat(",{0}", input.IsAlergiaSzn.HasValue ? Convert.ToInt32(input.IsAlergiaSzn) : 0);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.QualAlergiaSzn) ? "NULL" : input.QualAlergiaSzn);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataPreAtend.HasValue ? "'" : string.Empty, input.DataPreAtend.HasValue ? input.DataPreAtend.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.CodAmbulatorioSUS.HasValue ? input.CodAmbulatorioSUS.Value : 0);
                    strSql.AppendFormat(",{0}", input.IDSetor.HasValue ? input.IDSetor.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAltaAmbulatorial.HasValue ? "'" : string.Empty, input.DataAltaAmbulatorial.HasValue ? input.DataAltaAmbulatorial.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDAltaAmbulatorial.HasValue ? input.IDAltaAmbulatorial.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDUsuarioAltaInc.HasValue ? input.IDUsuarioAltaInc.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDMedPreAtend.HasValue ? input.IDMedPreAtend.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsAtendendo.HasValue ? Convert.ToInt32(input.IsAtendendo) : 0);
                    strSql.AppendFormat(",{0}", input.IDMedicoAtendendo.HasValue ? input.IDMedicoAtendendo.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}{1}{0}", input.DataAtendAmbulatorial.HasValue ? "'" : string.Empty, input.DataAtendAmbulatorial.HasValue ? input.DataAtendAmbulatorial.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", input.IDProtocoloEmergencia.HasValue ? input.IDProtocoloEmergencia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IDSW.HasValue ? input.IDSW.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",'{0}'", input.DataIniPreAtend.HasValue ? input.DataIniPreAtend.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",'{0}'", input.DataIniInfoClinicas.HasValue ? input.DataIniInfoClinicas.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}", input.DataFimPreAtend.HasValue ? input.DataFimPreAtend.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}", input.DataFimInfoClinicas.HasValue ? input.DataFimInfoClinicas.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}", input.DataAltaMedica.HasValue ? input.DataAltaMedica.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}", input.DataAltaAdministrativa.HasValue ? input.DataAltaAdministrativa.Value.ToString() : "NULL");
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();
                        cmd.Dispose();

                        strSql = new StringBuilder();
                        strSql.AppendFormat("UPDATE SIS_ATENDIMENTO SET CODATENDIMENTO='{0}' WHERE IDATENDIMENTO={1}", input.CodAmbulatorio, input.IDAmbulatorio);

                        cmd = new SqlCommand(strSql.ToString(), conn);

                        await cmd.ExecuteNonQueryAsync();

                        cmd.Dispose();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    var result = input.IDAmbulatorio.Value;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Sis_Ambulatorio>> Listar()
            {
                var result = new List<Sis_Ambulatorio>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDAMBULATORIO, CODAMBULATORIO, DATAINICIO, ISREVISAO, ISHORAMARCADA, DATARETORNO, ");
                strSql.AppendFormat("IDATENDREVISAO, IDALTA, STATUSPRONTOATEND, NUMEROSEQ, TIPOCONSULTA, ISVACINA, DATAEXAME, ");
                strSql.AppendFormat("IDUSUARIOLIBERACAO, DATALIBERACAO, IDPRIORIDADEATENDIMENTO, ISALTAREVELIA, IDUSUARIOREVELIA, ");
                strSql.AppendFormat("DATASOLICITACAO, DIAGNOSTICO, TRATAMENTO, DADOSCLINICOS, PRIMCONSULTA, ISALERGIASZN, ");
                strSql.AppendFormat("QUALALERGIASZN, DATAPREATEND, CODAMBULATORIOSUS, IDSETOR, DATAALTAAMBULATORIAL, ");
                strSql.AppendFormat("IDALTAAMBULATORIAL, IDUSUARIOALTAINC, IDMEDPREATEND, ISATENDENDO, IDMEDICOATENDENDO, ");
                strSql.AppendFormat("DATAATENDAMBULATORIAL, IDPROTOCOLOEMERGENCIA "); //, DATAINIPREATEND, DATAINIINFOCLINICAS, ");
                //strSql.AppendFormat("DATAFIMPREATEND, DATAFIMINFOCLINICAS, DATAALTAMEDICA, DATAALTAADMINISTRATIVA ");
                strSql.AppendFormat("FROM SIS_INTERNACAO ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    while (listDb.Read())
                    {
                        var item = new Sis_Ambulatorio();

                        item.IDAmbulatorio = Convert.ToInt32(listDb["IDAMBULATORIO"]);
                        item.CodAmbulatorio = Convert.ToString(listDb["CODAMBULATORIO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINICIO"].ToString()))
                        {
                            item.DataInicio = Convert.ToDateTime(listDb["DATAINICIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISREVISAO"].ToString()))
                        {
                            item.IsRevisao = Convert.ToBoolean(listDb["ISREVISAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISHORAMARCADA"].ToString()))
                        {
                            item.IsHoraMarcada = Convert.ToBoolean(listDb["ISHORAMARCADA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATARETORNO"].ToString()))
                        {
                            item.DataRetorno = Convert.ToDateTime(listDb["DATARETORNO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDREVISAO"].ToString()))
                        {
                            item.IDAtendRevisao = Convert.ToInt32(listDb["IDATENDREVISAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSPRONTOATEND"].ToString()))
                        {
                            item.StatusProntoAtend = Convert.ToInt32(listDb["STATUSPRONTOATEND"]);
                        }
                        item.NumeroSeq = Convert.ToString(listDb["NUMEROSEQ"]);
                        item.TipoConsulta = Convert.ToString(listDb["TIPOCONSULTA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ISVACINA"].ToString()))
                        {
                            item.IsVacina = Convert.ToBoolean(listDb["ISVACINA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAEXAME"].ToString()))
                        {
                            item.DataExame = Convert.ToDateTime(listDb["DATAEXAME"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOLIBERACAO"].ToString()))
                        {
                            item.IDUsuarioLiberacao = Convert.ToInt32(listDb["IDUSUARIOLIBERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATALIBERACAO"].ToString()))
                        {
                            item.DataLiberacao = Convert.ToDateTime(listDb["DATALIBERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPRIORIDADEATENDIMENTO"].ToString()))
                        {
                            item.IDPrioridadeAtendimento = Convert.ToInt32(listDb["IDPRIORIDADEATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTAREVELIA"].ToString()))
                        {
                            item.IsAltaRevelia = Convert.ToBoolean(listDb["ISALTAREVELIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOREVELIA"].ToString()))
                        {
                            item.IDUsuarioRevelia = Convert.ToInt32(listDb["IDUSUARIOREVELIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATASOLICITACAO"].ToString()))
                        {
                            item.DataSolicitacao = Convert.ToDateTime(listDb["DATASOLICITACAO"]);
                        }
                        item.Diagnostico = Convert.ToString(listDb["DIAGNOSTICO"]);
                        item.Tratamento = Convert.ToString(listDb["TRATAMENTO"]);
                        item.DadosClinicos = Convert.ToString(listDb["DADOSCLINICOS"]);
                        item.PrimConsulta = Convert.ToString(listDb["PRIMCONSULTA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ISALERGIASZN"].ToString()))
                        {
                            item.IsAlergiaSzn = Convert.ToBoolean(listDb["ISALERGIASZN"]);
                        }
                        item.QualAlergiaSzn = Convert.ToString(listDb["QUALALERGIASZN"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREATEND"].ToString()))
                        {
                            item.DataPreAtend = Convert.ToDateTime(listDb["DATAPREATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["CODAMBULATORIOSUS"].ToString()))
                        {
                            item.CodAmbulatorioSUS = Convert.ToInt32(listDb["CODAMBULATORIOSUS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDSETOR"].ToString()))
                        {
                            item.IDSetor = Convert.ToInt32(listDb["IDSETOR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAAMBULATORIAL"].ToString()))
                        {
                            item.DataAltaAmbulatorial = Convert.ToDateTime(listDb["DATAALTAAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTAAMBULATORIAL"].ToString()))
                        {
                            item.IDAltaAmbulatorial = Convert.ToInt32(listDb["IDALTAAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAINC"].ToString()))
                        {
                            item.IDUsuarioAltaInc = Convert.ToInt32(listDb["IDUSUARIOALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDPREATEND"].ToString()))
                        {
                            item.IDMedPreAtend = Convert.ToInt32(listDb["IDMEDPREATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISATENDENDO"].ToString()))
                        {
                            item.IsAtendendo = Convert.ToBoolean(listDb["ISATENDENDO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOATENDENDO"].ToString()))
                        {
                            item.IDMedicoAtendendo = Convert.ToInt32(listDb["IDMEDICOATENDENDO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAATENDAMBULATORIAL"].ToString()))
                        {
                            item.DataAtendAmbulatorial = Convert.ToDateTime(listDb["DATAATENDAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPROTOCOLOEMERGENCIA"].ToString()))
                        {
                            item.IDProtocoloEmergencia = Convert.ToInt32(listDb["IDPROTOCOLOEMERGENCIA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAINIPREATEND"].ToString()))
                        //{
                        //    item.DataIniPreAtend = Convert.ToDateTime(listDb["DATAINIPREATEND"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAINIINFOCLINICAS"].ToString()))
                        //{
                        //    item.DataIniInfoClinicas = Convert.ToDateTime(listDb["DATAINIINFOCLINICAS"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAFIMPREATEND"].ToString()))
                        //{
                        //    item.DataFimPreAtend = Convert.ToDateTime(listDb["DATAFIMPREATEND"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAFIMINFOCLINICAS"].ToString()))
                        //{
                        //    item.DataFimInfoClinicas = Convert.ToDateTime(listDb["DATAFIMINFOCLINICAS"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAALTAMEDICA"].ToString()))
                        //{
                        //    item.DataAltaMedica = Convert.ToDateTime(listDb["DATAALTAMEDICA"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAALTAADMINISTRATIVA"].ToString()))
                        //{
                        //    item.DataAltaAdministrativa = Convert.ToDateTime(listDb["DATAALTAADMINISTRATIVA"]);
                        //}

                        result.Add(item);
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return result;
                }
            }

            public async Task<Sis_Ambulatorio> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDAMBULATORIO, CODAMBULATORIO, DATAINICIO, ISREVISAO, ISHORAMARCADA, DATARETORNO, ");
                strSql.AppendFormat("IDATENDREVISAO, IDALTA, STATUSPRONTOATEND, NUMEROSEQ, TIPOCONSULTA, ISVACINA, DATAEXAME, ");
                strSql.AppendFormat("IDUSUARIOLIBERACAO, DATALIBERACAO, IDPRIORIDADEATENDIMENTO, ISALTAREVELIA, IDUSUARIOREVELIA, ");
                strSql.AppendFormat("DATASOLICITACAO, DIAGNOSTICO, TRATAMENTO, DADOSCLINICOS, PRIMCONSULTA, ISALERGIASZN, ");
                strSql.AppendFormat("QUALALERGIASZN, DATAPREATEND, CODAMBULATORIOSUS, IDSETOR, DATAALTAAMBULATORIAL, ");
                strSql.AppendFormat("IDALTAAMBULATORIAL, IDUSUARIOALTAINC, IDMEDPREATEND, ISATENDENDO, IDMEDICOATENDENDO, ");
                strSql.AppendFormat("DATAATENDAMBULATORIAL, IDPROTOCOLOEMERGENCIA"); //, DATAINIPREATEND, DATAINIINFOCLINICAS, ");
                //strSql.AppendFormat("DATAFIMPREATEND, DATAFIMINFOCLINICAS, DATAALTAMEDICA, DATAALTAADMINISTRATIVA ");
                strSql.AppendFormat(" FROM SIS_INTERNACAO");
                strSql.AppendFormat(" WHERE IDAMBULATORIO={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    var item = new Sis_Ambulatorio();
                    if (listDb.HasRows)
                    {
                        listDb.Read();

                        item.IDAmbulatorio = Convert.ToInt32(listDb["IDAMBULATORIO"]);
                        item.CodAmbulatorio = Convert.ToString(listDb["CODAMBULATORIO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINICIO"].ToString()))
                        {
                            item.DataInicio = Convert.ToDateTime(listDb["DATAINICIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISREVISAO"].ToString()))
                        {
                            item.IsRevisao = Convert.ToBoolean(listDb["ISREVISAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISHORAMARCADA"].ToString()))
                        {
                            item.IsHoraMarcada = Convert.ToBoolean(listDb["ISHORAMARCADA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATARETORNO"].ToString()))
                        {
                            item.DataRetorno = Convert.ToDateTime(listDb["DATARETORNO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDREVISAO"].ToString()))
                        {
                            item.IDAtendRevisao = Convert.ToInt32(listDb["IDATENDREVISAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSPRONTOATEND"].ToString()))
                        {
                            item.StatusProntoAtend = Convert.ToInt32(listDb["STATUSPRONTOATEND"]);
                        }
                        item.NumeroSeq = Convert.ToString(listDb["NUMEROSEQ"]);
                        item.TipoConsulta = Convert.ToString(listDb["TIPOCONSULTA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ISVACINA"].ToString()))
                        {
                            item.IsVacina = Convert.ToBoolean(listDb["ISVACINA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAEXAME"].ToString()))
                        {
                            item.DataExame = Convert.ToDateTime(listDb["DATAEXAME"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOLIBERACAO"].ToString()))
                        {
                            item.IDUsuarioLiberacao = Convert.ToInt32(listDb["IDUSUARIOLIBERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATALIBERACAO"].ToString()))
                        {
                            item.DataLiberacao = Convert.ToDateTime(listDb["DATALIBERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPRIORIDADEATENDIMENTO"].ToString()))
                        {
                            item.IDPrioridadeAtendimento = Convert.ToInt32(listDb["IDPRIORIDADEATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTAREVELIA"].ToString()))
                        {
                            item.IsAltaRevelia = Convert.ToBoolean(listDb["ISALTAREVELIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOREVELIA"].ToString()))
                        {
                            item.IDUsuarioRevelia = Convert.ToInt32(listDb["IDUSUARIOREVELIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATASOLICITACAO"].ToString()))
                        {
                            item.DataSolicitacao = Convert.ToDateTime(listDb["DATASOLICITACAO"]);
                        }
                        item.Diagnostico = Convert.ToString(listDb["DIAGNOSTICO"]);
                        item.Tratamento = Convert.ToString(listDb["TRATAMENTO"]);
                        item.DadosClinicos = Convert.ToString(listDb["DADOSCLINICOS"]);
                        item.PrimConsulta = Convert.ToString(listDb["PRIMCONSULTA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ISALERGIASZN"].ToString()))
                        {
                            item.IsAlergiaSzn = Convert.ToBoolean(listDb["ISALERGIASZN"]);
                        }
                        item.QualAlergiaSzn = Convert.ToString(listDb["QUALALERGIASZN"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREATEND"].ToString()))
                        {
                            item.DataPreAtend = Convert.ToDateTime(listDb["DATAPREATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["CODAMBULATORIOSUS"].ToString()))
                        {
                            item.CodAmbulatorioSUS = Convert.ToInt32(listDb["CODAMBULATORIOSUS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDSETOR"].ToString()))
                        {
                            item.IDSetor = Convert.ToInt32(listDb["IDSETOR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAAMBULATORIAL"].ToString()))
                        {
                            item.DataAltaAmbulatorial = Convert.ToDateTime(listDb["DATAALTAAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTAAMBULATORIAL"].ToString()))
                        {
                            item.IDAltaAmbulatorial = Convert.ToInt32(listDb["IDALTAAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAINC"].ToString()))
                        {
                            item.IDUsuarioAltaInc = Convert.ToInt32(listDb["IDUSUARIOALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDPREATEND"].ToString()))
                        {
                            item.IDMedPreAtend = Convert.ToInt32(listDb["IDMEDPREATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISATENDENDO"].ToString()))
                        {
                            item.IsAtendendo = Convert.ToBoolean(listDb["ISATENDENDO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOATENDENDO"].ToString()))
                        {
                            item.IDMedicoAtendendo = Convert.ToInt32(listDb["IDMEDICOATENDENDO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAATENDAMBULATORIAL"].ToString()))
                        {
                            item.DataAtendAmbulatorial = Convert.ToDateTime(listDb["DATAATENDAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPROTOCOLOEMERGENCIA"].ToString()))
                        {
                            item.IDProtocoloEmergencia = Convert.ToInt32(listDb["IDPROTOCOLOEMERGENCIA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAINIPREATEND"].ToString()))
                        //{
                        //    item.DataIniPreAtend = Convert.ToDateTime(listDb["DATAINIPREATEND"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAINIINFOCLINICAS"].ToString()))
                        //{
                        //    item.DataIniInfoClinicas = Convert.ToDateTime(listDb["DATAINIINFOCLINICAS"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAFIMPREATEND"].ToString()))
                        //{
                        //    item.DataFimPreAtend = Convert.ToDateTime(listDb["DATAFIMPREATEND"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAFIMINFOCLINICAS"].ToString()))
                        //{
                        //    item.DataFimInfoClinicas = Convert.ToDateTime(listDb["DATAFIMINFOCLINICAS"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAALTAMEDICA"].ToString()))
                        //{
                        //    item.DataAltaMedica = Convert.ToDateTime(listDb["DATAALTAMEDICA"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAALTAADMINISTRATIVA"].ToString()))
                        //{
                        //    item.DataAltaAdministrativa = Convert.ToDateTime(listDb["DATAALTAADMINISTRATIVA"]);
                        //}
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public async Task<Sis_Ambulatorio> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("AMB.IDAMBULATORIO,AMB.CODAMBULATORIO,AMB.DATAINICIO,AMB.ISREVISAO,AMB.ISHORAMARCADA,AMB.DATARETORNO ");
                strSql.AppendFormat(",AMB.IDATENDREVISAO,AMB.IDALTA,AMB.STATUSPRONTOATEND,AMB.NUMEROSEQ,AMB.TIPOCONSULTA,AMB.ISVACINA,AMB.DATAEXAME ");
                strSql.AppendFormat(",AMB.IDUSUARIOLIBERACAO,AMB.DATALIBERACAO,AMB.IDPRIORIDADEATENDIMENTO,AMB.ISALTAREVELIA,AMB.IDUSUARIOREVELIA ");
                strSql.AppendFormat(",AMB.DATASOLICITACAO,AMB.DIAGNOSTICO,AMB.TRATAMENTO,AMB.DADOSCLINICOS,AMB.PRIMCONSULTA,AMB.ISALERGIASZN ");
                strSql.AppendFormat(",AMB.QUALALERGIASZN,AMB.DATAPREATEND,AMB.CODAMBULATORIOSUS,AMB.IDSETOR,AMB.DATAALTAAMBULATORIAL ");
                strSql.AppendFormat(",AMB.IDALTAAMBULATORIAL,AMB.IDUSUARIOALTAINC,AMB.IDMEDPREATEND,AMB.ISATENDENDO,AMB.IDMEDICOATENDENDO ");
                strSql.AppendFormat(",AMB.DATAATENDAMBULATORIAL,AMB.IDPROTOCOLOEMERGENCIA,ATE.IDSW IDSW"); //,AMB.DATAINIPREATEND,AMB.DATAINIINFOCLINICAS ");
                //strSql.AppendFormat(",AMB.DATAFIMPREATEND,AMB.DATAFIMINFOCLINICAS,AMB.DATAALTAMEDICA,AMB.DATAALTAADMINISTRATIVA ");
                strSql.AppendFormat(" FROM SIS_ATENDIMENTO ATE ");
                strSql.AppendFormat(" JOIN SIS_AMBULATORIO AMB ON ATE.IDATENDIMENTO=AMB.IDAMBULATORIO ");
                strSql.AppendFormat(" AND ATE.IDSW={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Ambulatorio();
                    if (listDb.HasRows)
                    {
                        listDb.Read();

                        item.IDSW = Convert.ToInt32(listDb["IDSW"]);
                        item.IDAmbulatorio = Convert.ToInt32(listDb["IDAMBULATORIO"]);
                        item.CodAmbulatorio = Convert.ToString(listDb["CODAMBULATORIO"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAINICIO"].ToString()))
                        {
                            item.DataInicio = Convert.ToDateTime(listDb["DATAINICIO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISREVISAO"].ToString()))
                        {
                            item.IsRevisao = Convert.ToBoolean(listDb["ISREVISAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISHORAMARCADA"].ToString()))
                        {
                            item.IsHoraMarcada = Convert.ToBoolean(listDb["ISHORAMARCADA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATARETORNO"].ToString()))
                        {
                            item.DataRetorno = Convert.ToDateTime(listDb["DATARETORNO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDATENDREVISAO"].ToString()))
                        {
                            item.IDAtendRevisao = Convert.ToInt32(listDb["IDATENDREVISAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTA"].ToString()))
                        {
                            item.IDAlta = Convert.ToInt32(listDb["IDALTA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["STATUSPRONTOATEND"].ToString()))
                        {
                            item.StatusProntoAtend = Convert.ToInt32(listDb["STATUSPRONTOATEND"]);
                        }
                        item.NumeroSeq = Convert.ToString(listDb["NUMEROSEQ"]);
                        item.TipoConsulta = Convert.ToString(listDb["TIPOCONSULTA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ISVACINA"].ToString()))
                        {
                            item.IsVacina = Convert.ToBoolean(listDb["ISVACINA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAEXAME"].ToString()))
                        {
                            item.DataExame = Convert.ToDateTime(listDb["DATAEXAME"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOLIBERACAO"].ToString()))
                        {
                            item.IDUsuarioLiberacao = Convert.ToInt32(listDb["IDUSUARIOLIBERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATALIBERACAO"].ToString()))
                        {
                            item.DataLiberacao = Convert.ToDateTime(listDb["DATALIBERACAO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPRIORIDADEATENDIMENTO"].ToString()))
                        {
                            item.IDPrioridadeAtendimento = Convert.ToInt32(listDb["IDPRIORIDADEATENDIMENTO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISALTAREVELIA"].ToString()))
                        {
                            item.IsAltaRevelia = Convert.ToBoolean(listDb["ISALTAREVELIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOREVELIA"].ToString()))
                        {
                            item.IDUsuarioRevelia = Convert.ToInt32(listDb["IDUSUARIOREVELIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATASOLICITACAO"].ToString()))
                        {
                            item.DataSolicitacao = Convert.ToDateTime(listDb["DATASOLICITACAO"]);
                        }
                        item.Diagnostico = Convert.ToString(listDb["DIAGNOSTICO"]);
                        item.Tratamento = Convert.ToString(listDb["TRATAMENTO"]);
                        item.DadosClinicos = Convert.ToString(listDb["DADOSCLINICOS"]);
                        item.PrimConsulta = Convert.ToString(listDb["PRIMCONSULTA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ISALERGIASZN"].ToString()))
                        {
                            item.IsAlergiaSzn = Convert.ToBoolean(listDb["ISALERGIASZN"]);
                        }
                        item.QualAlergiaSzn = Convert.ToString(listDb["QUALALERGIASZN"]);
                        if (!string.IsNullOrWhiteSpace(listDb["DATAPREATEND"].ToString()))
                        {
                            item.DataPreAtend = Convert.ToDateTime(listDb["DATAPREATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["CODAMBULATORIOSUS"].ToString()))
                        {
                            item.CodAmbulatorioSUS = Convert.ToInt32(listDb["CODAMBULATORIOSUS"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDSETOR"].ToString()))
                        {
                            item.IDSetor = Convert.ToInt32(listDb["IDSETOR"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAALTAAMBULATORIAL"].ToString()))
                        {
                            item.DataAltaAmbulatorial = Convert.ToDateTime(listDb["DATAALTAAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDALTAAMBULATORIAL"].ToString()))
                        {
                            item.IDAltaAmbulatorial = Convert.ToInt32(listDb["IDALTAAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDUSUARIOALTAINC"].ToString()))
                        {
                            item.IDUsuarioAltaInc = Convert.ToInt32(listDb["IDUSUARIOALTAINC"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDPREATEND"].ToString()))
                        {
                            item.IDMedPreAtend = Convert.ToInt32(listDb["IDMEDPREATEND"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISATENDENDO"].ToString()))
                        {
                            item.IsAtendendo = Convert.ToBoolean(listDb["ISATENDENDO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDMEDICOATENDENDO"].ToString()))
                        {
                            item.IDMedicoAtendendo = Convert.ToInt32(listDb["IDMEDICOATENDENDO"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAATENDAMBULATORIAL"].ToString()))
                        {
                            item.DataAtendAmbulatorial = Convert.ToDateTime(listDb["DATAATENDAMBULATORIAL"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["IDPROTOCOLOEMERGENCIA"].ToString()))
                        {
                            item.IDProtocoloEmergencia = Convert.ToInt32(listDb["IDPROTOCOLOEMERGENCIA"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAINIPREATEND"].ToString()))
                        //{
                        //    item.DataIniPreAtend = Convert.ToDateTime(listDb["DATAINIPREATEND"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAINIINFOCLINICAS"].ToString()))
                        //{
                        //    item.DataIniInfoClinicas = Convert.ToDateTime(listDb["DATAINIINFOCLINICAS"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAFIMPREATEND"].ToString()))
                        //{
                        //    item.DataFimPreAtend = Convert.ToDateTime(listDb["DATAFIMPREATEND"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAFIMINFOCLINICAS"].ToString()))
                        //{
                        //    item.DataFimInfoClinicas = Convert.ToDateTime(listDb["DATAFIMINFOCLINICAS"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAALTAMEDICA"].ToString()))
                        //{
                        //    item.DataAltaMedica = Convert.ToDateTime(listDb["DATAALTAMEDICA"]);
                        //}
                        //if (!string.IsNullOrWhiteSpace(listDb["DATAALTAADMINISTRATIVA"].ToString()))
                        //{
                        //    item.DataAltaAdministrativa = Convert.ToDateTime(listDb["DATAALTAADMINISTRATIVA"]);
                        //}
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class SisPacienteRepositorio : IRepositorio<Sis_Paciente>
        {
            private string _cnAsa;

            public SisPacienteRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }
            public async Task Alterar(Sis_Paciente input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_PACIENTE SET ");
                    //strSql.AppendFormat("CODPACIENTE='{0}'", input.CodPaciente);
                    strSql.AppendFormat("PAI={0}", string.IsNullOrWhiteSpace(input.Pai) ? "NULL" : "'" + input.Pai + "'");
                    strSql.AppendFormat(",MAE={0}", string.IsNullOrWhiteSpace(input.Mae) ? "NULL" : "'" + input.Mae + "'");
                    strSql.AppendFormat(",CNS={0}", string.IsNullOrWhiteSpace(input.CNS) ? "NULL" : "'" + input.CNS + "'");
                    strSql.AppendFormat(",OBSERVACAO='{0}'", input.Observacao);
                    strSql.AppendFormat(",DATAULTIMAMALADIR={0}", input.DataUltimaMalaDir.HasValue ? input.DataUltimaMalaDir.Value.ToString() : "NULL");
                    strSql.AppendFormat(",RACACOR={0}", string.IsNullOrWhiteSpace(input.RacaCor) ? "NULL" : "'" + input.RacaCor + "'");
                    strSql.AppendFormat(",IDETNIA={0}", input.IDEtnia.HasValue ? input.IDEtnia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISSUS={0}", input.IsSUS.HasValue ? (object)input.IsSUS.Value : 0);
                    strSql.AppendFormat(",USUARIOWEB={0}", string.IsNullOrWhiteSpace(input.UsuarioWeb) ? "NULL" : "'" + input.UsuarioWeb + "'");
                    strSql.AppendFormat(",SENHAWEB={0}", string.IsNullOrWhiteSpace(input.SenhaWeb) ? "NULL" : "'" + input.SenhaWeb + "'");
                    strSql.AppendFormat(",ISRECEBEEMAIL={0}", input.IsRecebeEmail.HasValue ? (object)input.IsRecebeEmail.Value : 0);
                    strSql.AppendFormat(",VALORESCALA={0}", string.IsNullOrWhiteSpace(input.ValorEscala) ? "NULL" : "'" + input.ValorEscala + "'");
                    strSql.AppendFormat(",IDRELIGIAO={0}", input.IDReligiao.HasValue ? input.IDReligiao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",MATRICULA={0}", string.IsNullOrWhiteSpace(input.Matricula) ? "NULL" : "'" + input.Matricula + "'");
                    strSql.AppendFormat(",CATEGORIA={0}", string.IsNullOrWhiteSpace(input.Categoria) ? "NULL" : "'" + input.Categoria + "'");
                    strSql.AppendFormat(",GRAUDEPENDENTE={0}", string.IsNullOrWhiteSpace(input.GrauDependente) ? "NULL" : "'" + input.GrauDependente + "'");
                    strSql.AppendFormat(",ESCOLARIDADE={0}", input.Escolaridade.HasValue ? input.Escolaridade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",ISCARTAO={0}", input.IsCartao.HasValue ? (object)input.IsCartao.Value : 0);
                    strSql.AppendFormat(",NUMDECLNASCVIVO={0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivo) ? "NULL" : "'" + input.NumDeclNascVivo + "'");
                    strSql.AppendFormat(",JUSTIFICATIVANUMDECLNASCVIVO={0}", string.IsNullOrWhiteSpace(input.JustificativaNumDeclNascVivo) ? "NULL" : "'" + input.JustificativaNumDeclNascVivo + "'");
                    strSql.AppendFormat(",USUARIOAGENDAWEB={0}", string.IsNullOrWhiteSpace(input.UsuarioAgendaWeb) ? "NULL" : "'" + input.UsuarioAgendaWeb + "'");
                    strSql.AppendFormat(",SENHAAGENDAWEB={0}", string.IsNullOrWhiteSpace(input.SenhaAgendaWeb) ? "NULL" : "'" + input.SenhaAgendaWeb + "'");
                    strSql.AppendFormat(",IDEMPRESAPAC={0}", input.IDEmpresaPac.HasValue ? input.IDEmpresaPac.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",IDEXTERNO={0}", input.IDExterno.HasValue ? input.IDExterno.Value.ToString() : "NULL");
                    strSql.AppendFormat(" WHERE IDPACIENTE={0}", input.IDPaciente);
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteScalarAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE SIS_PACIENTE SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDPACIENTE=@IDPACIENTE ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDPACIENTE", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Sis_Paciente input)
            {
                strSql = new StringBuilder();
                try
                {
                    if (string.IsNullOrWhiteSpace(input.CodPaciente) && (string.IsNullOrWhiteSpace(input.CodPaciente) && input.CodPaciente != "0"))
                    {
                        var cod = await ObterCodigo("SIS_PACIENTE", _cnAsa); // await cmd.ExecuteScalarAsync();
                        input.CodPaciente = cod.ToString();
                    }
                    strSql.AppendFormat("INSERT INTO SIS_PACIENTE(");
                    strSql.AppendFormat("IDPACIENTE,CODPACIENTE,PAI,MAE,CNS,OBSERVACAO,DATAULTIMAMALADIR,RACACOR,IDETNIA,");
                    strSql.AppendFormat("ISSUS,USUARIOWEB,SENHAWEB,ISRECEBEEMAIL,VALORESCALA,IDRELIGIAO,MATRICULA,CATEGORIA,");
                    strSql.AppendFormat("GRAUDEPENDENTE,ESCOLARIDADE,ISCARTAO,NUMDECLNASCVIVO,JUSTIFICATIVANUMDECLNASCVIVO,");
                    strSql.AppendFormat("USUARIOAGENDAWEB,SENHAAGENDAWEB,IDEMPRESAPAC"); //,IDEXTERNO");
                    strSql.AppendFormat(") ");
                    //strSql.AppendFormat("output INSERTED.IDPACIENTE ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.IDPaciente);
                    strSql.AppendFormat(",'{0}'", input.CodPaciente);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Pai) ? "NULL" : "'" + input.Pai + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Mae) ? "NULL" : "'" + input.Mae + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.CNS) ? "NULL" : "'" + input.CNS + "'");
                    strSql.AppendFormat(",'{0}'", input.Observacao);
                    strSql.AppendFormat(",{0}{1}{0}", input.DataUltimaMalaDir.HasValue ? "'" : string.Empty, input.DataUltimaMalaDir.HasValue ? input.DataUltimaMalaDir.Value.ToString("yyyyMMdd HH:mm") : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.RacaCor) ? "NULL" : "'" + input.RacaCor + "'");
                    strSql.AppendFormat(",{0}", input.IDEtnia.HasValue ? input.IDEtnia.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsSUS.HasValue ? (object)input.IsSUS.Value : 0);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.UsuarioWeb) ? "NULL" : "'" + input.UsuarioWeb + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.SenhaWeb) ? "NULL" : "'" + input.SenhaWeb + "'");
                    strSql.AppendFormat(",{0}", input.IsRecebeEmail.HasValue ? (object)input.IsRecebeEmail.Value : 0);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.ValorEscala) ? "NULL" : "'" + input.ValorEscala + "'");
                    strSql.AppendFormat(",{0}", input.IDReligiao.HasValue ? input.IDReligiao.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Matricula) ? "NULL" : "'" + input.Matricula + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.Categoria) ? "NULL" : "'" + input.Categoria + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.GrauDependente) ? "NULL" : "'" + input.GrauDependente + "'");
                    strSql.AppendFormat(",{0}", input.Escolaridade.HasValue ? input.Escolaridade.Value.ToString() : "NULL");
                    strSql.AppendFormat(",{0}", input.IsCartao.HasValue ? (object)input.IsCartao.Value : 0);
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.NumDeclNascVivo) ? "NULL" : "'" + input.NumDeclNascVivo + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.JustificativaNumDeclNascVivo) ? "NULL" : "'" + input.JustificativaNumDeclNascVivo + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.UsuarioAgendaWeb) ? "NULL" : "'" + input.UsuarioAgendaWeb + "'");
                    strSql.AppendFormat(",{0}", string.IsNullOrWhiteSpace(input.SenhaAgendaWeb) ? "NULL" : "'" + input.SenhaAgendaWeb + "'");
                    strSql.AppendFormat(",{0}", input.IDEmpresaPac.HasValue ? input.IDEmpresaPac.Value.ToString() : "NULL");
                    //strSql.AppendFormat(",{0}", input.IDExterno.HasValue ? input.IDExterno.Value.ToString() : "NULL");
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    var result = input.IDPaciente.Value; //record.IDPaciente.Value;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Sis_Paciente>> Listar()
            {
                var result = new List<Sis_Paciente>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDPACIENTE,CODPACIENTE,PAI,MAE,CNS,OBSERVACAO,DATAULTIMAMALADIR,RACACOR,IDETNIA,");
                strSql.AppendFormat("ISSUS,USUARIOWEB,SENHAWEB,ISRECEBEEMAIL,VALORESCALA,IDRELIGIAO,MATRICULA,CATEGORIA,");
                strSql.AppendFormat("GRAUDEPENDENTE,ESCOLARIDADE,ISCARTAO,NUMDECLNASCVIVO,JUSTIFICATIVANUMDECLNASCVIVO,");
                strSql.AppendFormat("USUARIOAGENDAWEB,SENHAAGENDAWEB,IDEMPRESAPAC"); //,IDEXTERNO ");
                strSql.AppendFormat("FROM SIS_PACIENTE ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Sis_Paciente();
                        item.IDPaciente = Convert.ToInt32(listDb["IDPESSOATIPO"]);
                        item.CodPaciente = listDb["CODPESSOA"].ToString();
                        item.Pai = listDb["PESSOA"].ToString();
                        item.Mae = listDb["ENDERECO"].ToString();
                        item.CNS = listDb["COMPLEMENTO"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["OBSERVACAO"].ToString()))
                        {
                            item.Observacao = (byte[])listDb["OBSERVACAO"];
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMAMALADIR"].ToString()))
                        {
                            item.DataUltimaMalaDir = Convert.ToDateTime(listDb["DATAULTIMAMALADIR"]);
                        }
                        item.RacaCor = Convert.ToString(listDb["RACACOR"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDETNIA"].ToString()))
                        {
                            item.IDEtnia = Convert.ToInt32(listDb["IDETNIA"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISSUS"].ToString()))
                        {
                            item.IsSUS = Convert.ToBoolean(listDb["ISSUS"]);
                        }
                        item.UsuarioWeb = Convert.ToString(listDb["USUARIOWEB"]);
                        item.SenhaWeb = listDb["SENHAWEB"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["ISRECEBEEMAIL"].ToString()))
                        {
                            item.IsRecebeEmail = Convert.ToBoolean(listDb["ISRECEBEEMAIL"]);
                        }
                        item.ValorEscala = Convert.ToString(listDb["VALORESCALA"]);
                        if (!string.IsNullOrWhiteSpace(listDb["IDRELIGIAO"].ToString()))
                        {
                            item.IDReligiao = Convert.ToInt32(listDb["IDRELIGIAO"]);
                        }
                        item.Matricula = Convert.ToString(listDb["MATRICULA"]);
                        item.Categoria = Convert.ToString(listDb["CATEGORIA"]);
                        item.GrauDependente = Convert.ToString(listDb["GRAUDEPENDENTE"]);
                        if (!string.IsNullOrWhiteSpace(listDb["ESCOLARIDADE"].ToString()))
                        {
                            item.Escolaridade = Convert.ToInt32(listDb["ESCOLARIDADE"]);
                        }
                        if (!string.IsNullOrWhiteSpace(listDb["ISCARTAO"].ToString()))
                        {
                            item.IsCartao = Convert.ToBoolean(listDb["ISCARTAO"]);
                        }
                        item.NumDeclNascVivo = Convert.ToString(listDb["NUMDECLNASCVIVO"]);
                        item.JustificativaNumDeclNascVivo = listDb["JUSTIFICATIVANUMDECLNASCVIVO"].ToString();
                        item.UsuarioAgendaWeb = listDb["USUARIOAGENDAWEB"].ToString();
                        item.SenhaAgendaWeb = listDb["SENHAAGENDAWEB"].ToString();
                        if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                        {
                            item.IDEmpresaPac = Convert.ToInt32(listDb["IDEMPRESAPAC"]);
                        }
                        //if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                        //{
                        //    item.IDExterno = Convert.ToInt32(listDb["IDEXTERNO"]);
                        //}
                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Sis_Paciente> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDPACIENTE,CODPACIENTE,PAI,MAE,CNS,OBSERVACAO,DATAULTIMAMALADIR,RACACOR,IDETNIA,");
                strSql.AppendFormat("ISSUS,USUARIOWEB,SENHAWEB,ISRECEBEEMAIL,VALORESCALA,IDRELIGIAO,MATRICULA,CATEGORIA,");
                strSql.AppendFormat("GRAUDEPENDENTE,ESCOLARIDADE,ISCARTAO,NUMDECLNASCVIVO,JUSTIFICATIVANUMDECLNASCVIVO,");
                strSql.AppendFormat("USUARIOAGENDAWEB,SENHAAGENDAWEB,IDEMPRESAPAC "); //,IDEXTERNO ");
                strSql.AppendFormat(" FROM SIS_PACIENTE ");
                strSql.AppendFormat(" WHERE IDPACIENTE={0}", id);

                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Paciente();
                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPACIENTE"].ToString()))
                        {
                            item.IDPaciente = Convert.ToInt32(listDb["IDPACIENTE"]);
                            item.CodPaciente = listDb["CODPACIENTE"].ToString();
                            item.Pai = listDb["PAI"].ToString();
                            item.Mae = listDb["MAE"].ToString();
                            item.CNS = listDb["CNS"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["OBSERVACAO"].ToString()))
                            {
                                item.Observacao = (byte[])listDb["OBSERVACAO"];
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMAMALADIR"].ToString()))
                            {
                                item.DataUltimaMalaDir = Convert.ToDateTime(listDb["DATAULTIMAMALADIR"]);
                            }
                            item.RacaCor = Convert.ToString(listDb["RACACOR"]);
                            if (!string.IsNullOrWhiteSpace(listDb["IDETNIA"].ToString()))
                            {
                                item.IDEtnia = Convert.ToInt32(listDb["IDETNIA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISSUS"].ToString()))
                            {
                                item.IsSUS = Convert.ToBoolean(listDb["ISSUS"]);
                            }
                            item.UsuarioWeb = Convert.ToString(listDb["USUARIOWEB"]);
                            item.SenhaWeb = listDb["SENHAWEB"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["ISRECEBEEMAIL"].ToString()))
                            {
                                item.IsRecebeEmail = Convert.ToBoolean(listDb["ISRECEBEEMAIL"]);
                            }
                            item.ValorEscala = Convert.ToString(listDb["VALORESCALA"]);
                            if (!string.IsNullOrWhiteSpace(listDb["IDRELIGIAO"].ToString()))
                            {
                                item.IDReligiao = Convert.ToInt32(listDb["IDRELIGIAO"]);
                            }
                            item.Matricula = Convert.ToString(listDb["MATRICULA"]);
                            item.Categoria = Convert.ToString(listDb["CATEGORIA"]);
                            item.GrauDependente = Convert.ToString(listDb["GRAUDEPENDENTE"]);
                            if (!string.IsNullOrWhiteSpace(listDb["ESCOLARIDADE"].ToString()))
                            {
                                item.Escolaridade = Convert.ToInt32(listDb["ESCOLARIDADE"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISCARTAO"].ToString()))
                            {
                                item.IsCartao = Convert.ToBoolean(listDb["ISCARTAO"]);
                            }
                            item.NumDeclNascVivo = Convert.ToString(listDb["NUMDECLNASCVIVO"]);
                            item.JustificativaNumDeclNascVivo = listDb["JUSTIFICATIVANUMDECLNASCVIVO"].ToString();
                            item.UsuarioAgendaWeb = listDb["USUARIOAGENDAWEB"].ToString();
                            item.SenhaAgendaWeb = listDb["SENHAAGENDAWEB"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                            {
                                item.IDEmpresaPac = Convert.ToInt32(listDb["IDEMPRESAPAC"]);
                            }
                            //if (!string.IsNullOrWhiteSpace(listDb["IDEXTERNO"].ToString()))
                            //{
                            //    item.IDExterno = Convert.ToInt32(listDb["IDEXTERNO"]);
                            //}
                        }
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    //}
                    return item;
                }
            }

            public async Task<Sis_Paciente> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("PA.IDPACIENTE,PA.CODPACIENTE,PA.PAI,PA.MAE,PA.CNS,PA.OBSERVACAO,PA.DATAULTIMAMALADIR,PA.RACACOR,PA.IDETNIA");
                strSql.AppendFormat(",PA.ISSUS,PA.USUARIOWEB,PA.SENHAWEB,PA.ISRECEBEEMAIL,PA.VALORESCALA,PA.IDRELIGIAO,PA.MATRICULA,PA.CATEGORIA");
                strSql.AppendFormat(",PA.GRAUDEPENDENTE,PA.ESCOLARIDADE,PA.ISCARTAO,PA.NUMDECLNASCVIVO,PA.JUSTIFICATIVANUMDECLNASCVIVO");
                strSql.AppendFormat(",PA.USUARIOAGENDAWEB,PA.SENHAAGENDAWEB,PA.IDEMPRESAPAC ");
                strSql.AppendFormat("FROM SIS_PESSOA PE, SIS_PACIENTE PA ");
                strSql.AppendFormat("WHERE PE.IDPESSOA = PA.IDPACIENTE ");
                strSql.AppendFormat("AND PE.IDSW={0}", id);
                //strSql.AppendFormat("FROM SIS_PESSOA PE ");
                //strSql.AppendFormat("LEFT JOIN SIS_PACIENTE PA ON PE.IDPESSOA = PA.IDPACIENTE ");
                //strSql.AppendFormat("WHERE  PE.ID={0}", id);

                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Sis_Paciente();

                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        if (!string.IsNullOrWhiteSpace(listDb["IDPACIENTE"].ToString()))
                        {
                            item.IDPaciente = Convert.ToInt32(listDb["IDPACIENTE"]);
                            item.CodPaciente = listDb["CODPACIENTE"].ToString();
                            item.Pai = listDb["PAI"].ToString();
                            item.Mae = listDb["MAE"].ToString();
                            item.CNS = listDb["CNS"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["OBSERVACAO"].ToString()))
                            {
                                item.Observacao = (byte[])listDb["OBSERVACAO"];
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["DATAULTIMAMALADIR"].ToString()))
                            {
                                item.DataUltimaMalaDir = Convert.ToDateTime(listDb["DATAULTIMAMALADIR"]);
                            }
                            item.RacaCor = Convert.ToString(listDb["RACACOR"]);
                            if (!string.IsNullOrWhiteSpace(listDb["IDETNIA"].ToString()))
                            {
                                item.IDEtnia = Convert.ToInt32(listDb["IDETNIA"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISSUS"].ToString()))
                            {
                                item.IsSUS = Convert.ToBoolean(listDb["ISSUS"]);
                            }
                            item.UsuarioWeb = Convert.ToString(listDb["USUARIOWEB"]);
                            item.SenhaWeb = listDb["SENHAWEB"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["ISRECEBEEMAIL"].ToString()))
                            {
                                item.IsRecebeEmail = Convert.ToBoolean(listDb["ISRECEBEEMAIL"]);
                            }
                            item.ValorEscala = Convert.ToString(listDb["VALORESCALA"]);
                            if (!string.IsNullOrWhiteSpace(listDb["IDRELIGIAO"].ToString()))
                            {
                                item.IDReligiao = Convert.ToInt32(listDb["IDRELIGIAO"]);
                            }
                            item.Matricula = Convert.ToString(listDb["MATRICULA"]);
                            item.Categoria = Convert.ToString(listDb["CATEGORIA"]);
                            item.GrauDependente = Convert.ToString(listDb["GRAUDEPENDENTE"]);
                            if (!string.IsNullOrWhiteSpace(listDb["ESCOLARIDADE"].ToString()))
                            {
                                item.Escolaridade = Convert.ToInt32(listDb["ESCOLARIDADE"]);
                            }
                            if (!string.IsNullOrWhiteSpace(listDb["ISCARTAO"].ToString()))
                            {
                                item.IsCartao = Convert.ToBoolean(listDb["ISCARTAO"]);
                            }
                            item.NumDeclNascVivo = Convert.ToString(listDb["NUMDECLNASCVIVO"]);
                            item.JustificativaNumDeclNascVivo = listDb["JUSTIFICATIVANUMDECLNASCVIVO"].ToString();
                            item.UsuarioAgendaWeb = listDb["USUARIOAGENDAWEB"].ToString();
                            item.SenhaAgendaWeb = listDb["SENHAAGENDAWEB"].ToString();
                            if (!string.IsNullOrWhiteSpace(listDb["IDEMPRESAPAC"].ToString()))
                            {
                                item.IDEmpresaPac = Convert.ToInt32(listDb["IDEMPRESAPAC"]);
                            }
                            //if (!string.IsNullOrWhiteSpace(listDb["IDEXTERNO"].ToString()))
                            //{
                            //    item.IDExterno = Convert.ToInt32(listDb["IDEXTERNO"]);
                            //}
                        }
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class ProReqExameMovRepositorio : IRepositorio<Pro_ReqExameMov>
        {
            private string _cnAsa;
            public ProReqExameMovRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }
            public async Task Alterar(Pro_ReqExameMov input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE PRO_REQEXAMEMOV SET ");
                    strSql.AppendFormat("IDCCREQUISITADO={0}", input.IdCcRequisitado);
                    strSql.AppendFormat(",IDMEDICO={0}", input.IdMedico);
                    strSql.AppendFormat(",DATAREQUISICAO='{0}'", input.DataRequisicao.ToString("yyyyMMdd hh:mm"));
                    strSql.AppendFormat(",ISENCERRADA={0}", Convert.ToInt32(input.IsEncerrada));
                    strSql.AppendFormat(",ISSEMANAL={0}", Convert.ToInt32(input.IsSemanal));
                    strSql.AppendFormat(",IDUSUARIO={0}", input.IdUsuario);
                    strSql.AppendFormat(",IDATENDIMENTO={0}", input.IdAtendimento);
                    strSql.AppendFormat(",IDLOCALUTILIZACAO={0}", input.IdLocalUtilizacao);
                    strSql.AppendFormat(",IDTERCEIRIZADO={0}", input.IdTerceirizado);
                    strSql.AppendFormat(",IDCENTROCUSTO={0}", input.IdCentroCusto);
                    strSql.AppendFormat(",IDREQEXAMESTATUS={0}", input.IdReqExameStatus);
                    strSql.AppendFormat(",IDSW={0}", input.IDSW);
                    //FIM DOS OBRIGATÓRIOS
                    strSql.AppendFormat(" WHERE IDREQUISICAOMOV={0}", input.IdRequisicaoMov);

                    conn = new SqlConnection();
                    //conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE PRO_REQEXAMEMOV SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDREQUISICAOMOV=@IDREQUISICAOMOV ");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@IDREQUISICAOMOV", id);

                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Pro_ReqExameMov input)
            {
                strSql = new StringBuilder();
                try
                {
                    int result;
                    //if (string.IsNullOrWhiteSpace(input.CodPessoa) && (string.IsNullOrWhiteSpace(input.CodPessoa) && input.CodPessoa != "0"))
                    //{
                    //    var cod = await ObterCodigo("SIS_PESSOA", _cnAsa); // await cmd.ExecuteScalarAsync();
                    //    input.CodPessoa = cod.ToString();
                    //}
                    strSql.AppendFormat("INSERT INTO PRO_REQEXAMEMOV(");
                    strSql.AppendFormat("IDCCREQUISITADO,IDMEDICO,DATAREQUISICAO,ISENCERRADA,ISSEMANAL,IDUSUARIO,IDATENDIMENTO,");
                    strSql.AppendFormat("IDLOCALUTILIZACAO,IDTERCEIRIZADO,IDCENTROCUSTO,IDREQEXAMESTATUS,IDSW,DATAATUALIZACAO,TIPOREQUISICAO");
                    strSql.AppendFormat(") ");
                    //strSql.AppendFormat("output INSERTED.IDPESSOA ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.IdCcRequisitado);
                    strSql.AppendFormat(",{0}", input.IdMedico);
                    strSql.AppendFormat(",'{0}'", input.DataRequisicao.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",{0}", Convert.ToInt32(input.IsEncerrada));
                    strSql.AppendFormat(",{0}", Convert.ToInt32(input.IsSemanal));
                    strSql.AppendFormat(",{0}", input.IdUsuario);
                    strSql.AppendFormat(",{0}", input.IdAtendimento);
                    strSql.AppendFormat(",{0}", input.IdLocalUtilizacao);
                    strSql.AppendFormat(",{0}", input.IdTerceirizado);
                    strSql.AppendFormat(",{0}", input.IdCentroCusto);
                    strSql.AppendFormat(",{0}", input.IdReqExameStatus);
                    strSql.AppendFormat(",{0}", input.IDSW);
                    strSql.AppendFormat(",'{0}'", input.DataAutorizacao.Value.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",{0}", input.TipoRequisicao);
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }
                    var record = await Obter(input.IDSW.Value.ToString());
                    result = record.IdRequisicaoMov;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Pro_ReqExameMov>> Listar()
            {
                var result = new List<Pro_ReqExameMov>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDCCREQUISITADO,IDMEDICO,DATAREQUISICAO,ISENCERRADA,ISSEMANAL,IDUSUARIO,IDATENDIMENTO,");
                strSql.AppendFormat("IDLOCALUTILIZACAO,IDTERCEIRIZADO,IDCENTROCUSTO,IDREQEXAMESTATUS ");
                strSql.AppendFormat("FROM PRO_REQEXAMEMOV ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Pro_ReqExameMov();

                        item.IdCcRequisitado = Convert.ToInt32(listDb["IDCCREQUISITADO"]);
                        item.IdMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        item.DataRequisicao = Convert.ToDateTime(listDb["DATAREQUISICAO"]);
                        item.IsEncerrada = Convert.ToBoolean(listDb["ISENCERRADA"]);
                        item.IsSemanal = Convert.ToBoolean(listDb["ISSEMANAL"]);
                        item.IdUsuario = Convert.ToInt32(listDb["IDUSUARIO"]);
                        item.IdAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        item.IdLocalUtilizacao = Convert.ToInt32(listDb["IDLOCALUTILIZACAO"]);
                        item.IdTerceirizado = Convert.ToInt32(listDb["IDTERCEIRIZADO"]);
                        item.IdCentroCusto = Convert.ToInt32(listDb["IDCENTROCUSTO"]);
                        item.IdReqExameStatus = Convert.ToInt32(listDb["IDREQEXAMESTATUS"]);

                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Pro_ReqExameMov> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDREQUISICAOMOV,IDCCREQUISITADO,IDMEDICO,DATAREQUISICAO,ISENCERRADA,ISSEMANAL,IDUSUARIO,IDATENDIMENTO,");
                strSql.AppendFormat("IDLOCALUTILIZACAO,IDTERCEIRIZADO,IDCENTROCUSTO,IDREQEXAMESTATUS ");
                strSql.AppendFormat("FROM PRO_REQEXAMEMOV ");
                strSql.AppendFormat("WHERE IdRequisicaoMov=@IDREQUISICAOMOV ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    cmd.Parameters.AddWithValue("@IDREQUISICAOMOV", id);
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    var item = new Pro_ReqExameMov();
                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        item.IdRequisicaoMov = Convert.ToInt32(listDb["IDREQUISICAOMOV"]);
                        item.IdCcRequisitado = Convert.ToInt32(listDb["IDCCREQUISITADO"]);
                        item.IdMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        item.DataRequisicao = Convert.ToDateTime(listDb["DATAREQUISICAO"]);
                        item.IsEncerrada = Convert.ToBoolean(listDb["ISENCERRADA"]);
                        item.IsSemanal = Convert.ToBoolean(listDb["ISSEMANAL"]);
                        item.IdUsuario = Convert.ToInt32(listDb["IDUSUARIO"]);
                        item.IdAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        item.IdLocalUtilizacao = Convert.ToInt32(listDb["IDLOCALUTILIZACAO"]);
                        item.IdTerceirizado = Convert.ToInt32(listDb["IDTERCEIRIZADO"]);
                        item.IdCentroCusto = Convert.ToInt32(listDb["IDCENTROCUSTO"]);
                        item.IdReqExameStatus = Convert.ToInt32(listDb["IDREQEXAMESTATUS"]);
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public async Task<Pro_ReqExameMov> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("IDREQUISICAOMOV,IDCCREQUISITADO,IDMEDICO,DATAREQUISICAO,ISENCERRADA,ISSEMANAL,IDUSUARIO,IDATENDIMENTO,");
                strSql.AppendFormat("IDLOCALUTILIZACAO,IDTERCEIRIZADO,IDCENTROCUSTO,IDREQEXAMESTATUS ");
                strSql.AppendFormat("FROM PRO_REQEXAMEMOV ");
                strSql.AppendFormat("WHERE IDSW={0}", id);
                strSql.AppendFormat(" ORDER BY DATAREQUISICAO DESC ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Pro_ReqExameMov();

                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        item.IdRequisicaoMov = Convert.ToInt32(listDb["IDREQUISICAOMOV"]);
                        item.IdCcRequisitado = Convert.ToInt32(listDb["IDCCREQUISITADO"]);
                        item.IdMedico = Convert.ToInt32(listDb["IDMEDICO"]);
                        item.DataRequisicao = Convert.ToDateTime(listDb["DATAREQUISICAO"]);
                        item.IsEncerrada = Convert.ToBoolean(listDb["ISENCERRADA"]);
                        item.IsSemanal = Convert.ToBoolean(listDb["ISSEMANAL"]);
                        item.IdUsuario = Convert.ToInt32(listDb["IDUSUARIO"]);
                        item.IdAtendimento = Convert.ToInt32(listDb["IDATENDIMENTO"]);
                        item.IdLocalUtilizacao = Convert.ToInt32(listDb["IDLOCALUTILIZACAO"]);
                        item.IdTerceirizado = Convert.ToInt32(listDb["IDTERCEIRIZADO"]);
                        item.IdCentroCusto = Convert.ToInt32(listDb["IDCENTROCUSTO"]);
                        item.IdReqExameStatus = Convert.ToInt32(listDb["IDREQEXAMESTATUS"]);
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        public class ProReqExameMovItemRepositorio : IRepositorio<Pro_ReqExameMovItem>
        {
            private string _cnAsa;
            public ProReqExameMovItemRepositorio(string cnAsa)
            {
                _cnAsa = cnAsa;
            }
            public async Task Alterar(Pro_ReqExameMovItem input)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE PRO_REQEXAMEMOVITEM SET ");
                    strSql.AppendFormat("QTDEREQUISITADA={0}", input.QtdeRequisitada);
                    strSql.AppendFormat(",DATAATUALIZACAO='{0}'", input.DataAtualizacao.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",ISENCERRADA={0}", Convert.ToInt32(input.IsEncerrada));
                    strSql.AppendFormat(",ISATENDIDA={0}", Convert.ToInt32(input.IsAtendida));
                    strSql.AppendFormat(",IDUSUARIO={0}", input.IdUsuario);
                    strSql.AppendFormat(",IDITEMREQUISITADO={0}", input.IdItemRequisitado);
                    strSql.AppendFormat(",IDITEM={0}", input.IdItem);
                    //FIM DOS OBRIGATÓRIOS
                    strSql.AppendFormat(" WHERE IDREQUISICAOMOVITEM={0}", input.IdRequisicaoMovItem);

                    conn = new SqlConnection();
                    //conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;

                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task Excluir(long id)
            {
                strSql = new StringBuilder();
                try
                {
                    strSql.AppendFormat("UPDATE PRO_REQEXAMEMOVITEM SET DESATIVADO=1 ");
                    strSql.AppendFormat("WHERE IDREQUISICAOMOVITEM={0}", id);
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        int x = await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<int> Inserir(Pro_ReqExameMovItem input)
            {
                strSql = new StringBuilder();
                try
                {
                    int result;
                    //if (string.IsNullOrWhiteSpace(input.CodPessoa) && (string.IsNullOrWhiteSpace(input.CodPessoa) && input.CodPessoa != "0"))
                    //{
                    //    var cod = await ObterCodigo("SIS_PESSOA", _cnAsa); // await cmd.ExecuteScalarAsync();
                    //    input.CodPessoa = cod.ToString();
                    //}
                    strSql.AppendFormat("INSERT INTO PRO_REQEXAMEMOVITEM(");
                    strSql.AppendFormat("QTDEREQUISITADA,DATAATUALIZACAO,ISENCERRADA,ISATENDIDA,IDUSUARIO,IDITEMREQUISITADO,IDITEM,IDSW,IDREQUISICAOMOV");
                    strSql.AppendFormat(") ");
                    //strSql.AppendFormat("output INSERTED.IDPESSOA ");
                    strSql.AppendFormat("VALUES(");
                    strSql.AppendFormat("{0}", input.QtdeRequisitada);
                    strSql.AppendFormat(",'{0}'", input.DataAtualizacao.ToString("yyyyMMdd HH:mm"));
                    strSql.AppendFormat(",{0}", Convert.ToInt32(input.IsEncerrada));
                    strSql.AppendFormat(",{0}", Convert.ToInt32(input.IsAtendida));
                    strSql.AppendFormat(",{0}", input.IdUsuario);
                    strSql.AppendFormat(",{0}", input.IdItemRequisitado);
                    strSql.AppendFormat(",{0}", input.IdItem);
                    strSql.AppendFormat(",{0}", input.IDSW);
                    strSql.AppendFormat(",{0}", input.IdRequisicaoMov);
                    strSql.AppendFormat(")");
                    conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                    using (cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        conn.Open();

                        await cmd.ExecuteNonQueryAsync();

                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }
                    var record = await Obter(input.IDSW.Value.ToString());
                    result = record.IdRequisicaoMovItem;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - SQL: {1}", ex.Message.ToString(), strSql.ToString()), ex);
                }
            }

            public async Task<ICollection<Pro_ReqExameMovItem>> Listar()
            {
                var result = new List<Pro_ReqExameMovItem>();
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("QTDEREQUISITADA,DATAATUALIZACAO,ISENCERRADA,ISATENDIDA,IDUSUARIO,IDITEMREQUISITADO,IDITEM,IDSW,IDREQUISICAOMOVITEM, IDREQUISICAOMOV ");
                strSql.AppendFormat("FROM PRO_REQEXAMEMOVITEM ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    while (listDb.Read())
                    {
                        var item = new Pro_ReqExameMovItem();

                        item.QtdeRequisitada = Convert.ToInt32(listDb["QTDEREQUISITADA"]);
                        item.DataAtualizacao = Convert.ToDateTime(listDb["DATAATUALIZACAO"]);
                        item.IsEncerrada = Convert.ToBoolean(listDb["ISENCERRADA"]);
                        item.IsAtendida = Convert.ToBoolean(listDb["ISATENDIDA"]);
                        item.IdUsuario = Convert.ToInt32(listDb["IDUSUARIO"]);
                        item.IdItemRequisitado = Convert.ToInt32(listDb["IDITEMREQUISITADO"]);
                        item.IdItem = Convert.ToInt32(listDb["IDITEM"]);
                        item.IdRequisicaoMovItem = Convert.ToInt32(listDb["IDREQUISICAOMOVITEM"]);
                        item.IdRequisicaoMov = Convert.ToInt32(listDb["IDREQUISICAOMOV"]);
                        result.Add(item);
                    }
                    return result;
                }
            }

            public async Task<Pro_ReqExameMovItem> Obter(long id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("QTDEREQUISITADA,DATAATUALIZACAO,ISENCERRADA,ISATENDIDA,IDUSUARIO,IDITEMREQUISITADO,IDITEM,IDSW,IDREQUISICAOMOVITEM, IDREQUISICAOMOV ");
                strSql.AppendFormat("FROM PRO_REQEXAMEMOVITEM ");
                strSql.AppendFormat("WHERE IDREQUISICAOMOVITEM={0}", id);
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();
                    var item = new Pro_ReqExameMovItem();
                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        item.QtdeRequisitada = Convert.ToInt32(listDb["QTDEREQUISITADA"]);
                        item.DataAtualizacao = Convert.ToDateTime(listDb["DATAATUALIZACAO"]);
                        item.IsEncerrada = Convert.ToBoolean(listDb["ISENCERRADA"]);
                        item.IsAtendida = Convert.ToBoolean(listDb["ISATENDIDA"]);
                        item.IdUsuario = Convert.ToInt32(listDb["IDUSUARIO"]);
                        item.IdItemRequisitado = Convert.ToInt32(listDb["IDITEMREQUISITADO"]);
                        item.IdItem = Convert.ToInt32(listDb["IDITEM"]);
                        item.IdRequisicaoMovItem = Convert.ToInt32(listDb["IDREQUISICAOMOVITEM"]);
                        item.IdRequisicaoMov = Convert.ToInt32(listDb["IDREQUISICAOMOV"]);
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public async Task<Pro_ReqExameMovItem> Obter(string id)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("SELECT ");
                strSql.AppendFormat("QTDEREQUISITADA,DATAATUALIZACAO,ISENCERRADA,ISATENDIDA,IDUSUARIO,IDITEMREQUISITADO,IDITEM,IDSW,IDREQUISICAOMOVITEM, IDREQUISICAOMOV ");
                strSql.AppendFormat("FROM PRO_REQEXAMEMOVITEM ");
                strSql.AppendFormat("WHERE IDSW={0}", id);
                //strSql.AppendFormat(" ORDER BY DATAREQUISICAO DESC ");
                conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[_cnAsa].ConnectionString;
                using (cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    conn.Open();

                    var listDb = await cmd.ExecuteReaderAsync();

                    var item = new Pro_ReqExameMovItem();

                    if (listDb.HasRows)
                    {
                        listDb.Read();
                        item.QtdeRequisitada = Convert.ToInt32(listDb["QTDEREQUISITADA"]);
                        item.DataAtualizacao = Convert.ToDateTime(listDb["DATAATUALIZACAO"]);
                        item.IsEncerrada = Convert.ToBoolean(listDb["ISENCERRADA"]);
                        item.IsAtendida = Convert.ToBoolean(listDb["ISATENDIDA"]);
                        item.IdUsuario = Convert.ToInt32(listDb["IDUSUARIO"]);
                        item.IdItemRequisitado = Convert.ToInt32(listDb["IDITEMREQUISITADO"]);
                        item.IdItem = Convert.ToInt32(listDb["IDITEM"]);
                        item.IdRequisicaoMovItem = Convert.ToInt32(listDb["IDREQUISICAOMOVITEM"]);
                        item.IdRequisicaoMov = Convert.ToInt32(listDb["IDREQUISICAOMOV"]);
                    }
                    else
                    {
                        item = null;
                    }
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return item;
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }


        private class Sis_MedEsp
        {
            public int IdMedico { get; set; }
            public int IdEspecialidade { get; set; }
        }

        private static Sis_MedEsp MedicoEspecialidade(int idEmp, int idMed, int IdEsp, string cnAsa)
        {
            var cMedEsp = new SqlConnection();
            cMedEsp.ConnectionString = ConfigurationManager.ConnectionStrings[cnAsa].ConnectionString;
            cMedEsp.Open();
            var result = new Sis_MedEsp();
            result.IdMedico = idMed;
            result.IdEspecialidade = IdEsp;
            //verificar medico/especialidade
            strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 IDMEDICO,IDESPECIALIDADE FROM SIS_MEDICOESPECIALIDADE ");
            strSql.AppendFormat("WHERE IDMEDICO={0} AND IDESPECIALIDADE={1} AND IDEMPRESA={2}", idMed, IdEsp, idEmp);

            cmd = new SqlCommand(strSql.ToString(), cMedEsp);
            var medEsp = cmd.ExecuteReader();
            cmd.Dispose();
            if (medEsp.HasRows)
            {
                medEsp.Close();
                cMedEsp.Close();
                cMedEsp.Dispose();
                return result;
            }
            medEsp.Close();
            cMedEsp.Close();
            cMedEsp.Dispose();
            var cMedEsp1 = new SqlConnection();
            cMedEsp1.ConnectionString = ConfigurationManager.ConnectionStrings[cnAsa].ConnectionString;
            cMedEsp1.Open();
            strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 IDMEDICO,IDESPECIALIDADE FROM SIS_MEDICOESPECIALIDADE ");
            strSql.AppendFormat("WHERE IDMEDICO={0} AND IDEMPRESA={1}", idMed, idEmp);
            var cmd1 = new SqlCommand(strSql.ToString(), cMedEsp1);
            var medEsp1 = cmd1.ExecuteReader();
            cmd1.Dispose();
            if (medEsp1.HasRows)
            {
                medEsp1.Read();
                result.IdMedico = Convert.ToInt32(medEsp1["IDMEDICO"]);
                result.IdEspecialidade = Convert.ToInt32(medEsp1["IDESPECIALIDADE"]);
            }
            else
            {
                medEsp1.Close();
                cMedEsp1.Close();

                var cMedEsp2 = new SqlConnection();
                cMedEsp2.ConnectionString = ConfigurationManager.ConnectionStrings[cnAsa].ConnectionString;
                cMedEsp2.Open();

                strSql = new StringBuilder();
                strSql.Append("SELECT TOP 1 IDMEDICO,IDESPECIALIDADE FROM SIS_MEDICOESPECIALIDADE ");
                strSql.AppendFormat("WHERE IDEMPRESA={0}", idEmp);
                var cmd2 = new SqlCommand(strSql.ToString(), cMedEsp2);
                var medEsp2 = cmd2.ExecuteReader();
                cmd2.Dispose();
                if (medEsp2.HasRows)
                {
                    medEsp2.Read();
                    result.IdMedico = Convert.ToInt32(medEsp2["IDMEDICO"]);
                    result.IdEspecialidade = Convert.ToInt32(medEsp2["IDESPECIALIDADE"]);
                }
                else
                {
                    result.IdMedico = 0;
                    result.IdEspecialidade = 0;
                }
                medEsp2.Close();
            }
            medEsp1.Close();
            medEsp1.Close();
            cMedEsp1.Close();
            cMedEsp1.Dispose();
            return result;
        }
    }
}
