IF EXISTS (SELECT sysobjects.id
           FROM sysobjects
		   WHERE sysobjects.name = 'FnRptCompReqSimples')
  DROP FUNCTION FnRptCompReqSimples
go

CREATE FUNCTION FnRptCompReqSimples
(
  @DataInicial datetime2,
  @DataFinal datetime2,
  @ProdutoDescricao nvarchar,
  @EmpresaId int,
  @RequisicaoId bigint
)
RETURNS TABLE 
AS
RETURN 
(
  SELECT			R.Id						AS RequisicaoId, 
					P.Descricao					AS Produto, 
					RI.Quantidade				AS Quantidade, 
					U.Descricao					AS UnidadeTipo, 
					E.NomeFantasia				AS Empresa, 
					M.Descricao					AS MotivoPedido, 
					T.Descricao					AS RequisicaoTipo, 
					QuantidadeAprovacao			AS QtdAprovada,
					R.DataEmissao				AS Emissao,
					S.Descricao					AS Status
					
	FROM CmpRequisicao R
		INNER JOIN CmpRequisicaoItem RI (NOLOCK) ON R.Id = RI.CmpRequisicaoId
		INNER JOIN Est_Produto P (NOLOCK) ON RI.EstProdutoId = P.Id
		INNER JOIN Est_Unidade U (NOLOCK) ON RI.UnidadeId = U.Id
		INNER JOIN CmpMotivoPedido M (NOLOCK) ON R.CmpMotivoPedidoId = M.Id
		INNER JOIN CmpRequisicaoTipo T (NOLOCK) ON R.CmpTipoRequisicaoId = T.Id
		INNER JOIN CmpCotacaoStatus S (NOLOCK) ON R.CmpCotacaoStatusId = S.Id
		INNER JOIN SisEmpresa E (NOLOCK) ON R.CmpEmpresaId = E.Id
	WHERE R.DataEmissao BETWEEN @DataInicial AND @DataFinal
	--	AND M.IsEntrada = 1
	--	AND I.ProdutoId = 25588
		AND M.IsDeleted = 0
		AND T.IsDeleted = 0
		AND S.IsDeleted = 0
		AND R.IsDeleted = 0

  AND   ((@ProdutoDescricao IS NULL) OR (P.Descricao LIKE '%' + @ProdutoDescricao + '%'))
  AND   ((@EmpresaId IS NULL) OR (E.Id = @EmpresaId))
  AND   ((@RequisicaoId IS NULL) OR (R.Id = @EmpresaId))
)
go