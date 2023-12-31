IF EXISTS (SELECT sysobjects.id
           FROM sysobjects
		   WHERE sysobjects.name = 'FnRptComprasPorLote')
  DROP FUNCTION FnRptComprasPorLote
go

CREATE FUNCTION FnRptComprasPorLote
(
  @DataInicial datetime2,
  @DataFinal datetime2,
  @ProdutoId bigint,
  @ProdutoDescricao nvarchar,
  @NumeroNota nvarchar,
  @Estoque bigint,
  @NomeFornecedor nvarchar
)
RETURNS TABLE 
AS
RETURN 
(
  SELECT		
			CONVERT(Varchar(10), M.Emissao,103)						AS Emissao,
			F.Descricao												AS Fornecedor, 
			D.Numero												AS NumeroDocumento,
			D.ValorDocumento										AS ValorNota,
			M.ValorFrete											AS Frete,
			M.ValorICMS												AS ICMS,
			P.Descricao												AS Produto,
			I.Quantidade											AS QtdeProduto,
			CONVERT(DECIMAL(10,2), I.CustoUnitario)					AS CustoUnitario,
			CONVERT(DECIMAL(10,2),(I.Quantidade * I.CustoUnitario)) AS Total,
			U.Descricao												AS Unidade,
			LVi.Lote												AS Lote,
			LVI.Validade											AS Validade,
			D.CreationTime											AS EntradaSistema,
			M.Chave													AS ChaveNFe,
			Est.Descricao											AS Estoque
	FROM EstoquePreMovimento M
	INNER JOIN EstoquePreMovimentoItem I ON M.Id = I.PreMovimentoId
	INNER JOIN Est_Produto P ON I.ProdutoId = P.Id
	INNER JOIN Est_Unidade U ON I.ProdutoUnidadeId = U.Id
	INNER JOIN FinDocumento D ON M.Documento = D.Numero
	INNER JOIN SisFornecedor F ON M.SisFornecedorId = F.Id
	INNER JOIN EstoquePreMovimentoLoteValidade LV ON I.Id = LV.EstoquePreMovimentoItemId
	INNER JOIN LoteValidade LVI ON LV.LoteValidadeId = LVI.Id
	INNER JOIN Est_Estoque Est ON M.EstoqueId = Est.Id
	WHERE LVI.Validade BETWEEN @DataInicial AND @DataFinal
	--	AND M.IsEntrada = 1
	--	AND I.ProdutoId = 25588
		AND M.IsDeleted = 0
		AND I.IsDeleted = 0
		AND D.IsDeleted = 0
		AND F.IsDeleted = 0
		AND LV.IsDeleted = 0
		AND LVI.IsDeleted = 0 
  AND   ((@ProdutoId IS NULL) OR (P.Id = @ProdutoId))
  AND   ((@ProdutoDescricao IS NULL) OR (P.Descricao LIKE '%' + @ProdutoDescricao + '%'))
  AND   ((@Estoque IS NULL) OR (Est.Id = @Estoque))
  AND   ((@NumeroNota IS NULL) OR (D.Numero = @NumeroNota))
  AND   ((@NomeFornecedor IS NULL) OR (F.Descricao = @NomeFornecedor))
)
go