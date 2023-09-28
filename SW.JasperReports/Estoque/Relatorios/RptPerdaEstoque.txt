USE [AMERICAN]
GO
/****** Object:  UserDefinedFunction [dbo].[FnRptPerdaEstoque]    Script Date: 29/09/2021 10:29:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER FUNCTION [dbo].[FnRptPerdaEstoque](@estoqueId int, @DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 
		   CONCAT(
			CONVERT(varchar,movitem.creationtime,103),
			' ',
			CONVERT(varchar,movitem.creationtime,108)
			)										  AS DataMovimento,
		   SisUnidadeOrganizacional.Descricao		  AS Setor,
		   Estoque.Descricao						  AS Estoque,
		   movitem.Id                                 AS IdItemMov,
		   isnull(perda.Descricao,perda2.Descricao)   AS MotivoPerda,
		   prod.id,
		   prod.descricao                             AS Produto,
		   Lote.lote                                  AS Lote,
		   CONCAT(
			CONVERT(varchar,lote.Validade,103),
			' ',
			CONVERT(varchar,lote.Validade,108)
			)													  AS Validade,
		   MovItem.quantidade									  AS Quantidade,
		   CONVERT(varchar,MovimentoCustoUnitario.movimento,103)  AS DataCustoUnitario,
		   MovimentoCustoUnitario.CustoUnitario,
		   MovimentoCustoUnitario.CustoUnitario * MovItem.quantidade AS CustoTotal,
		   CONCAT(' R$', CAST(ROUND(MovimentoCustoUnitario.CustoUnitario,2,1) AS DECIMAL(18,2))) AS CustoUnitarioFormatado,
		   ( abpusers.NAME + ' ' + abpusers.surname ) AS Usuario,
		   mov.Observacao                             AS Observacao
	FROM   estoquemovimentoitem AS MovItem
		   LEFT JOIN estoquemovimento AS Mov
				  ON movitem.movimentoid = mov.id
		   LEFT JOIN est_produto AS Prod
				  ON movitem.produtoid = prod.id
		   LEFT JOIN estoquemovimentolotevalidade AS MovLote
				  ON movitem.id = movlote.estoquemovimentoitemid
		   LEFT JOIN Est_Estoque AS Estoque 
				  ON  Estoque.Id = Mov.EstoqueId
		   LEFT JOIN lotevalidade AS Lote
				  ON movlote.lotevalidadeid = lote.id
		   LEFT JOIN abpusers
				  ON movitem.creatoruserid = abpusers.id
		   LEFT JOIN EstoquePreMovimento AS premov
				  ON mov.EstoquePreMovimentoId = premov.Id
		   LEFT JOIN EstoquePreMovimento AS premovOrigem
		          ON premov.EstoquePreMovimentoParentId = premovOrigem.Id
		   LEFT JOIN MotivoPerdaProduto AS Perda
				  ON premovOrigem.MotivoPerdaProdutoId = Perda.Id
		   LEFT JOIN MotivoPerdaProduto AS Perda2
		          ON premov.MotivoPerdaProdutoId = perda2.Id
		   LEFT JOIN SisUnidadeOrganizacional
				  ON SisUnidadeOrganizacional.Id = Mov.UnidadeOrganizacionalId
	LEFT JOIN (SELECT 
					Mov.Movimento, 
					movItem.ProdutoId, 
					movLoteValidade.LoteValidadeId,
					movItem.CustoUnitario
					FROM 
					EstoqueMovimento mov
					INNER JOIN EstoqueMovimentoItem movItem
					ON mov.Id = movItem.MovimentoId
					LEFT JOIN EstoqueMovimentoLoteValidade movLoteValidade 
					ON movLoteValidade.EstoqueMovimentoItemId = movItem.Id
					INNER JOIN (
					SELECT
					MAX(mov.Movimento) MaxMovimento,
					movItem.ProdutoId, 
					movLoteValidade.LoteValidadeId  
					FROM 
					EstoqueMovimento mov
					INNER JOIN EstoqueMovimentoItem movItem 
					ON mov.Id = movItem.MovimentoId
					LEFT JOIN EstoqueMovimentoLoteValidade movLoteValidade 
					ON movLoteValidade.EstoqueMovimentoItemId = movItem.Id
					where mov.IsEntrada = 1 AND mov.EstTipoOperacaoId =1
					AND mov.IsDeleted = 0 AND movItem.IsDeleted = 0 AND movLoteValidade.IsDeleted =0
					GROUP BY movItem.ProdutoId, 
					movLoteValidade.LoteValidadeId) AS MaxMovimento 
					ON MaxMovimento.LoteValidadeId = movLoteValidade.LoteValidadeId
					AND MaxMovimento.ProdutoId = movItem.ProdutoId
					AND MaxMovimento.MaxMovimento = mov.Movimento
					AND mov.IsEntrada = 1 AND mov.EstTipoOperacaoId =1
					AND mov.IsDeleted = 0 AND movItem.IsDeleted = 0 AND movLoteValidade.IsDeleted =0) AS MovimentoCustoUnitario
					ON MovimentoCustoUnitario.LoteValidadeId = MovLote.LoteValidadeId
					AND MovimentoCustoUnitario.ProdutoId = movItem.ProdutoId

	WHERE  ( mov.esttipomovimentoid = 4 AND mov.esttipooperacaoid = 3 )   
			 AND ( MovItem.creationtime >= @DATAINICIAL AND movitem.creationtime <= @DATAFINAL )
			 AND ( @estoqueId = 0 OR Mov.EstoqueId = @estoqueId)		   
)


