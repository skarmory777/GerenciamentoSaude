USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[FnEstUltimasCompras]    Script Date: 03/08/2021 16:45:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnEstUltimasCompras](@DATAINICIAL DATETIME2, @DATAFINAL DATETIME2, @RANK INT, @produtoId bigint, @estoqueId bigint)
RETURNS TABLE 
AS
RETURN 
(
	WITH cte(Id, DataCompra, ProdutoId, Descricao, Fornecedor, RankProduto,Quantidade,Unidade,CustoUnitario,Laboratorio) AS (
SELECT 
	EstoqueMovimentoItem.Id,
	EstoqueMovimentoItem.CreationTime,
	EstoqueMovimentoItem.ProdutoId,
	Est_Produto.Descricao,
	SisPessoa.NomeFantasia,
	ROW_NUMBER() OVER (PARTITION BY EstoqueMovimentoItem.ProdutoId ORDER BY EstoqueMovimentoItem.CreationTime DESC) AS RankProduto,
	EstoqueMovimentoItem.Quantidade,
	Est_Unidade.Descricao,
	EstoqueMovimentoItem.CustoUnitario,
	EstLaboratorio.Descricao
	
FROM EstoqueMovimento 
INNER JOIN EstoqueMovimentoItem ON EstoqueMovimentoItem.MovimentoId = EstoqueMovimento.Id 
LEFT JOIN EstoqueMovimentoLoteValidade ON EstoqueMovimentoItem.Id = EstoqueMovimentoLoteValidade.EstoqueMovimentoItemId
LEFT JOIN LoteValidade ON EstoqueMovimentoLoteValidade.LoteValidadeId = LoteValidade.Id
LEFT JOIN EstLaboratorio ON LoteValidade.EstEstoqueLaboratorioId = EstLaboratorio.Id
INNER JOIN Est_Produto ON Est_Produto.Id = EstoqueMovimentoItem.ProdutoId
LEFT JOIN Est_Unidade ON Est_Unidade.Id = EstoqueMovimentoItem.ProdutoUnidadeId
RIGHT JOIN SisFornecedor ON EstoqueMovimento.SisFornecedorId = SisFornecedor.Id
Right JOIN SisPessoa ON SisFornecedor.SisPessoaId = SisPessoa.Id

WHERE IsEntrada = 1 AND EstoqueMovimento.IsDeleted = 0 AND EstoqueMovimentoItem.IsDeleted = 0

AND EstoqueMovimento.Movimento BETWEEN @DATAINICIAL AND @DATAFINAL
AND (@estoqueId = 0 OR EstoqueMovimento.EstoqueId = @estoqueId)
AND (@produtoId = 0 OR EstoqueMovimentoItem.ProdutoId = @estoqueId)
)
SELECT Id,ProdutoId,DataCompra,Descricao, Fornecedor, RankProduto,Quantidade, Unidade, CustoUnitario, Laboratorio from cte where RankProduto <= @RANK
)



GO


