IF EXISTS (SELECT sysobjects.id
           FROM sysobjects
		   WHERE sysobjects.name = 'FnRptEstAcuracia')
  DROP FUNCTION FnRptEstAcuracia
go

CREATE FUNCTION FnRptEstAcuracia
(
  @estoqueid int null,
  @DATAINICIO DATETIME2,
  @DATAFINAL DATETIME2
)
RETURNS TABLE 
AS
RETURN
Select EstInventario.Codigo,
EstInventario.DataInventario,
Est_Estoque.ID,
Est_Estoque.Descricao DescricaoEstoque,
--EstStatusInventario.ID,
EstStatusInventario.Descricao DescricaoStatusInventario,
Est_Produto.Descricao DescricaoProduto,
sum(isnull(QuantidadeContagem,0)) QtdContagem,
sum(isnull(QuantidadeEstoque,0)) QtdEstoque,
Case When sum(isnull(QuantidadeContagem,0)) >
sum(isnull(QuantidadeEstoque,0)) then sum(isnull(QuantidadeContagem,0))-sum(isnull(QuantidadeEstoque,0))
else 0 end QtdContagemMaior,
Case When sum(isnull(QuantidadeContagem,0)) <
sum(isnull(QuantidadeEstoque,0)) then sum(isnull(QuantidadeEstoque,0))-sum(isnull(QuantidadeContagem,0))
else 0 end QtdContagemMenor,
Case When sum(isnull(QuantidadeContagem,0)) =
sum(isnull(QuantidadeEstoque,0)) then 1
else 0 end IsContagemCorreta,
Case When sum(isnull(QuantidadeContagem,0)) >
sum(isnull(QuantidadeEstoque,0)) then 1
else 0 end IsContagemMaior,
Case When sum(isnull(QuantidadeContagem,0)) <
sum(isnull(QuantidadeEstoque,0)) then 1
else 0 end IsContagemMenor,
Case When sum(isnull(QuantidadeContagem,0)) <>
sum(isnull(QuantidadeEstoque,0)) then 1
else 0 end IsContagemIncorreta,
Custo.Documento,
Custo.Movimento,
round(isnull(Custo.CustoUnitario,0.00),4) UltimoValorCompra,
(Case When sum(isnull(QuantidadeContagem,0)) >
sum(isnull(QuantidadeEstoque,0)) then sum(isnull(QuantidadeContagem,0))-sum(isnull(QuantidadeEstoque,0))
else 0 end)*isnull(round(Custo.CustoUnitario,4),0.00) ValorTotalMaior,
(Case When sum(isnull(QuantidadeContagem,0)) <
sum(isnull(QuantidadeEstoque,0)) then sum(isnull(QuantidadeEstoque,0))-sum(isnull(QuantidadeContagem,0))
else 0 end)*isnull(round(Custo.CustoUnitario,4),0.00) ValorTotalMenor
from EstInventario (nolock) 
inner join Est_Estoque        (nolock) on EstInventario.EstoqueID = Est_Estoque.ID
inner join EstInventarioItem (nolock) on EstInventario.ID = EstInventarioItem.InventarioId
inner join Est_Produto       (nolock) on EstInventarioItem.ProdutoId = Est_Produto.ID
inner join EstStatusInventario (nolock) on EstInventario.StatusInventarioId = EstStatusInventario.ID
left join (select EstoqueMovimento.Documento,EstoqueMovimento.Movimento,
           EstoqueMovimentoItem.ID,
           EstoqueMovimentoItem.ProdutoId,
           Est_Produto.Descricao,
           EstoqueMovimentoItem.CustoUnitario
           from EstoqueMovimentoItem (nolock)
           inner join EstoqueMovimento (nolock) on EstoqueMovimentoItem.MovimentoId = EstoqueMovimento.ID
           inner join Est_Produto      (nolock) on EstoqueMovimentoItem.ProdutoId = Est_Produto.ID
           inner join (select EstoqueMovimentoItem.ProdutoId,max(EstoqueMovimentoItem.ID) IDMax
                       from EstoqueMovimento,EstoqueMovimentoItem
                       Where EstoqueMovimento.ID = EstoqueMovimentoItem.MovimentoId
                       and EstoqueMovimento.EstTipoOperacaoId = 1 -- Entradas
                       and EstoqueMovimento.EstTipoMovimentoId = 1 -- NotaFiscal
                       and EstoqueMovimento.IsDeleted = 0
                       and isnull(EstoqueMovimentoItem.CustoUnitario,0) <> 0
                       group by EstoqueMovimentoItem.ProdutoId) Ultimo on EstoqueMovimentoItem.ID = Ultimo.IDMax) Custo on Custo.ProdutoId = EstInventarioItem.ProdutoId
Where EstInventarioItem.QuantidadeContagem is not null
and EstInventario.StatusInventarioId = 4 -- Fechado
and EstInventario.DataInventario BETWEEN @DATAINICIO AND @DATAFINAL --'20220101'
--and EstInventario.DataInventario < Getdate()+1
and EstInventario.EstoqueID = isnull(null,@estoqueid)

group by EstInventario.Codigo,EstInventario.DataInventario,EstStatusInventario.ID,EstStatusInventario.Descricao,
Est_Estoque.ID,Est_Estoque.Descricao,
Est_Produto.Descricao,Custo.CustoUnitario,
Custo.Documento,
Custo.Movimento
go