using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(SolicitacaoExameItem))]
    public class SolicitacaoExameItemDto : CamposPadraoCRUDDto
    {
        public long SolicitacaoExameId { get; set; }
        public SolicitacaoExameDto Solicitacao { get; set; }
        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public string GuiaNumero { get; set; }
        public DateTime? DataValidade { get; set; }
        public string SenhaNumero { get; set; }
        public long? MaterialId { get; set; }
        public MaterialDto Material { get; set; }
        public string Justificativa { get; set; }
        public long? KitExameId { get; set; }
        public KitExameDto KitExame { get; set; }

        public string AccessNumber { get; set; }

        public long? PrescricaoItemRespostaId { get; set; }

        public PrescricaoItemRespostaDto PrescricaoItemResposta { get; set; }

        public long? IdGrid { get; set; }
        
        public bool IsPendencia { get; set; }
        public long? PendenciaUserId { get; set; }
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }
        
        

        #region Mapeamento

        public static SolicitacaoExameItemDto Mapear(SolicitacaoExameItem solicitacaoExameItem)
        {
            var solicitacaoExameItemDto = new SolicitacaoExameItemDto
            {
                Id = solicitacaoExameItem.Id,
                Codigo = solicitacaoExameItem.Codigo,
                Descricao = solicitacaoExameItem.Descricao,
                SolicitacaoExameId = solicitacaoExameItem.SolicitacaoExameId,
                FaturamentoItemId = solicitacaoExameItem.FaturamentoItemId,
                GuiaNumero = solicitacaoExameItem.GuiaNumero,
                DataValidade = solicitacaoExameItem.DataValidade,
                SenhaNumero = solicitacaoExameItem.SenhaNumero,
                MaterialId = solicitacaoExameItem.MaterialId,
                Justificativa = solicitacaoExameItem.Justificativa,
                KitExameId = solicitacaoExameItem.KitExameId,
                AccessNumber = solicitacaoExameItem.AccessNumber,
                IsPendencia = solicitacaoExameItem.IsPendencia,
                PendenciaUserId = solicitacaoExameItem.PendenciaUserId,
                PendenciaDateTime = solicitacaoExameItem.PendenciaDateTime,
                MotivoPendencia = solicitacaoExameItem.MotivoPendencia
            };


            solicitacaoExameItem.PrescricaoItemRespostaId = solicitacaoExameItem.PrescricaoItemRespostaId;

            if (solicitacaoExameItem.Solicitacao != null)
            {
                solicitacaoExameItemDto.Solicitacao = SolicitacaoExameDto.Mapear(solicitacaoExameItem.Solicitacao);
            }

            if (solicitacaoExameItem.FaturamentoItem != null)
            {
                solicitacaoExameItemDto.FaturamentoItem = FaturamentoItemDto.Mapear(solicitacaoExameItem.FaturamentoItem);
            }

            if(solicitacaoExameItem.Material != null)
            {
                solicitacaoExameItemDto.Material = MaterialDto.Mapear(solicitacaoExameItem.Material);
            }

            if (solicitacaoExameItem.PrescricaoItemResposta != null)
            {
                solicitacaoExameItemDto.PrescricaoItemResposta = PrescricaoItemRespostaDto.Mapear(solicitacaoExameItem.PrescricaoItemResposta);
            }

            //public SolicitacaoExameDto Solicitacao 
            //public FaturamentoItemDto FaturamentoItem 
            //public MaterialDto Material 
            //public KitExameDto KitExame 

            return solicitacaoExameItemDto;
        }

        public static SolicitacaoExameItem Mapear(SolicitacaoExameItemDto solicitacaoExameItemDto)
        {
            var solicitacaoExameItem = new SolicitacaoExameItem
            {
                Id = solicitacaoExameItemDto.Id,
                Codigo = solicitacaoExameItemDto.Codigo,
                Descricao = solicitacaoExameItemDto.Descricao,
                SolicitacaoExameId = solicitacaoExameItemDto.SolicitacaoExameId,
                FaturamentoItemId = solicitacaoExameItemDto.FaturamentoItemId,
                GuiaNumero = solicitacaoExameItemDto.GuiaNumero,
                DataValidade = solicitacaoExameItemDto.DataValidade,
                SenhaNumero = solicitacaoExameItemDto.SenhaNumero,
                MaterialId = solicitacaoExameItemDto.MaterialId,
                Justificativa = solicitacaoExameItemDto.Justificativa,
                KitExameId = solicitacaoExameItemDto.KitExameId,
                AccessNumber = solicitacaoExameItemDto.AccessNumber
            };


            //if(solicitacaoExameItemDto.Solicitacao!=null)
            //{
            //    solicitacaoExameItem.Solicitacao = SolicitacaoExameDto.Mapear(solicitacaoExameItemDto.Solicitacao);
            //}

            //if(solicitacaoExameItemDto.FaturamentoItem!=null)
            //{
            //    solicitacaoExameItem.FaturamentoItem = FaturamentoItemDto.Mapear(solicitacaoExameItemDto.FaturamentoItem);
            //}

            //public FaturamentoItemDto FaturamentoItem 
            //public MaterialDto Material 
            //public KitExameDto KitExame 

            return solicitacaoExameItem;
        }

        public static List<SolicitacaoExameItem> Mapear(List<SolicitacaoExameItemDto> listDto)
        {
            var list = new List<SolicitacaoExameItem>();

            foreach (var item in listDto)
            {
                list.Add(Mapear(item));
            }

            return list;
        }

        public static List<SolicitacaoExameItemDto> Mapear(List<SolicitacaoExameItem> list)
        {
            var listDto = new List<SolicitacaoExameItemDto>();

            foreach (var item in list)
            {
                listDto.Add(Mapear(item));
            }

            return listDto;
        }


        #endregion

    }
}
