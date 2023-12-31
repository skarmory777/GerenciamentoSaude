USE [AMERICAN]
GO
/****** Object:  UserDefinedFunction [dbo].[FnRptFinanceiroQuitacao]    Script Date: 21/03/2022 10:42:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[FnRptFinanceiroQuitacao](@IsCredito int null,@PessoaId int null, @EmpresaId int null, @DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
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
isnull(LancQuitacao.ValorQuitacao,0.00) ValorQuitacao,
LancQuitacao.CreationTime AS DataQuitacao
from FinLancamento as Lanc (nolock)
inner join FinDocumento Documento (nolock) on Lanc.DocumentoId = Documento.ID
inner join SisPessoa Pessoa (nolock) on Documento.PessoaId = Pessoa.ID
left join SisEmpresa Empresa (nolock) on Documento.EmpresaId = Empresa.ID
left join FinSituacaoLancamento Situacao (nolock) on Situacao.ID = Lanc.SituacaoLancamentoId
left join FinLancamentoQuitacao LancQuitacao (nolock) on Lanc.Id = LancQuitacao.LancamentoId
left join FinQuitacao Quitacao (nolock) on LancQuitacao.QuitacaoId = Quitacao.Id
/*(Select LancQuitacao.LancamentoId,max(Quitacao.DataMovimento) UltimaQuitacao,sum(LancQuitacao.ValorQuitacao)  as ValorQuitacao, max(LancQuitacao.CreationTime) as DataQuitacao
           from FinLancamentoQuitacao (nolock) LancQuitacao, FinQuitacao Quitacao
           WHERE (LancQuitacao.QuitacaoId = Quitacao.Id)
		   and (Quitacao.IsDeleted = 0)
		   and (LancQuitacao.IsDeleted = 0)
		   group by LancQuitacao.LancamentoId) QuitacaoTotal on Lanc.Id = QuitacaoTotal.LancamentoId */
Where Lanc.IsDeleted = 0
AND ( LancQuitacao.CreationTime >= @DATAINICIAL AND LancQuitacao.CreationTime <= @DATAFINAL )
and (@PessoaId = 0 OR Documento.PessoaID = isnull(@PessoaId,Documento.PessoaID))
and (@EmpresaId = 0 OR Documento.EmpresaID = isnull(@EmpresaId,Documento.EmpresaID))
and (@IsCredito = 0 OR Lanc.IsCredito = isnull(@IsCredito,Lanc.IsCredito))