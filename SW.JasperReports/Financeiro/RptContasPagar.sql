USE [AMERICAN]
GO
/****** Object:  UserDefinedFunction [dbo].[FnRptFinanceiroLancamento]    Script Date: 29/09/2021 11:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[FnRptFinanceiroLancamento](@IsCredito int null,@PessoaId int null, @EmpresaId int null, @SituacaoLancamentoId int null,@DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
SELECT Lanc.ID as ID,
Lanc.IsCredito As IsCredito,
Case When Lanc.IsCredito = 0 then 'Pagar' else 'Receber' end as TipoCredito,
Lanc.SituacaoLancamentoId as SituacaoLancamentoId,
Situacao.Descricao as SituacaoLancamento,
Documento.EmpresaId as EmpresaID,
Empresa.RazaoSocial as Empresa,
Empresa.NomeFantasia as EmpresaNomeFantasia,
Documento.Numero as NumeroDocumento,
Lanc.Parcela as Parcela,
Lanc.NossoNumero as NossoNumero,
Lanc.DataLancamento AS Emissao, 
Lanc.DataVencimento AS DataVencimento, 
Lanc.MesCompetencia AS MesCompetencia, 
Lanc.AnoCompetencia AS AnoCompetencia, 
Documento.PessoaId AS PessoaID,
Pessoa.NomeFantasia,
Pessoa.RazaoSocial,
Documento.ValorDocumento as ValorDocumento,
Lanc.ValorLancamento AS ValorLancamento,
isnull(QuitacaoTotal.ValorQuitacao,0.00) ValorQuitacao,
UltimaQuitacao
from FinLancamento as Lanc (nolock)
inner join FinDocumento Documento (nolock) on Lanc.DocumentoId = Documento.ID
inner join SisPessoa Pessoa (nolock) on Documento.PessoaId = Pessoa.ID
left join SisEmpresa Empresa (nolock) on Documento.EmpresaId = Empresa.ID
left join FinSituacaoLancamento Situacao (nolock) on Situacao.ID = Lanc.SituacaoLancamentoId
left join (Select LancQuitacao.LancamentoId,max(Quitacao.DataMovimento) UltimaQuitacao,sum(LancQuitacao.ValorQuitacao) as ValorQuitacao
           from FinLancamentoQuitacao (nolock) LancQuitacao, FinQuitacao Quitacao
           WHERE (LancQuitacao.QuitacaoId = Quitacao.Id)
		   and (Quitacao.IsDeleted = 0)
		   and (LancQuitacao.IsDeleted = 0)
		   group by LancQuitacao.LancamentoId) QuitacaoTotal on Lanc.Id = QuitacaoTotal.LancamentoId
Where Lanc.IsDeleted = 0
AND ( Lanc.DataVencimento >= @DATAINICIAL AND Lanc.DataVencimento <= @DATAFINAL )
and (Documento.PessoaID = isnull(@PessoaId,Documento.PessoaID))
and (Documento.EmpresaID = isnull(@EmpresaId,Documento.EmpresaID))
and (Lanc.SituacaoLancamentoId = isnull(@SituacaoLancamentoId,Lanc.SituacaoLancamentoId))
and (Lanc.IsCredito = isnull(@IsCredito,Lanc.IsCredito))