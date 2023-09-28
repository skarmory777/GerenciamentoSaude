USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[FnRptConsumoSetor]    Script Date: 06/07/2021 16:38:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnRptConsumoSetor](@unidadeOrganizacionalId int, @DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
SELECT 
			   movitem.creationtime                       AS DataMovimento,
			   movitem.Id                                 AS IdItemMov,
			   unid.Descricao                             AS UnidadeOrganizacional,
			   prod.descricao                             AS Produto,
			   Lote.lote                                  AS Lote,
			   lote.Validade							  AS Validade,
			   MovItem.quantidade                         AS Quantidade,
			   ( abpusers.NAME + ' ' + abpusers.surname ) AS Usuario
		FROM   estoquemovimentoitem AS MovItem
			   LEFT JOIN estoquemovimento AS Mov
					  ON movitem.movimentoid = mov.id
			   LEFT JOIN est_produto AS Prod
					  ON movitem.produtoid = prod.id
			   LEFT JOIN estoquemovimentolotevalidade AS MovLote
					  ON movitem.id = movlote.estoquemovimentoitemid
			   LEFT JOIN lotevalidade AS Lote
					  ON movlote.lotevalidadeid = lote.id
			   LEFT JOIN abpusers
					  ON movitem.creatoruserid = abpusers.id
			   LEFT JOIN EstoquePreMovimento AS premov
					  ON mov.EstoquePreMovimentoId = premov.Id
			   LEFT JOIN EstoquePreMovimento AS premovOrigem
					  ON premov.EstoquePreMovimentoParentId = premovOrigem.Id
			   LEFT JOIN SisUnidadeOrganizacional AS Unid
					  ON premovOrigem.UnidadeOrganizacionalId = Unid.Id
		WHERE  
			( mov.esttipomovimentoid = 2 AND mov.esttipooperacaoid = 3 )
			AND ( MovItem.creationtime >= @DATAINICIAL AND movitem.creationtime <= @DATAFINAL )
			AND ( @unidadeOrganizacionalId = 0 OR premovOrigem.UnidadeOrganizacionalId = @unidadeOrganizacionalId)


GO


