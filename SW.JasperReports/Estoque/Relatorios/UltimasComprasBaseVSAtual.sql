USE [AMERICAN]
GO
/** Object:  UserDefinedFunction [dbo].[RetornaUltimasComprasNoPeriodo]    Script Date: 22/09/2021 10:58:30 **/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER   FUNCTION [dbo].[RetornaUltimasComprasNoPeriodo] (@dataInicial datetime, @dataFinal datetime) 
RETURNS TABLE
AS
RETURN 
WITH CTE_ULTIMAS_COMPRAS AS (
SELECT EstoqueMovimentoItem.ProdutoId AS ProdutoId, MAX(EstoqueMovimento.Movimento) UltimaCompra 
FROM 
EstoqueMovimento 
INNER JOIN EstoqueMovimentoItem ON EstoqueMovimentoItem.MovimentoId = EstoqueMovimento.Id
WHERE IsEntrada = 1 AND EstoqueMovimento.IsDeleted = 0 AND EstoqueMovimentoItem.IsDeleted = 0
AND EstoqueMovimento.Movimento BETWEEN @dataInicial AND @dataFinal AND IsEntrada = 1
AND EstoqueMovimento.IsDeleted = 0 AND EstoqueMovimento.IsEntrada = 1
AND EstoqueMovimento.EstTipoOperacaoId = 1
GROUP BY  EstoqueMovimentoItem.ProdutoId
)
SELECT EstoqueMovimento.Id, EstoqueMovimento.Movimento, EstoqueMovimentoItem.ProdutoId, EstoqueMovimentoItem.CustoUnitario 
FROM EstoqueMovimento 
INNER JOIN EstoqueMovimentoItem ON EstoqueMovimentoItem.MovimentoId = EstoqueMovimento.Id
INNER JOIN  CTE_ULTIMAS_COMPRAS ON EstoqueMovimentoItem.ProdutoId = CTE_ULTIMAS_COMPRAS.ProdutoId AND EstoqueMovimento.Movimento = CTE_ULTIMAS_COMPRAS.UltimaCompra
WHERE EstoqueMovimento.IsDeleted = 0 AND EstoqueMovimento.IsEntrada = 1
AND EstoqueMovimento.EstTipoOperacaoId = 1