using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Prescricoes
{
    [AutoMap(typeof(PrescricaoMedicaDto))]
    public class CriarOuEditarPrescricaoViewModel : PrescricaoMedicaDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public ICollection<DivisaoDto> Divisoes { get; set; }

        public ICollection<TipoRespostaDto> TiposRespostas { get; set; }

        public MedicoDto MedicoCorrente { get; set; }

        public DateTime? DataFuturaPrescricao { get; set; }
        
        public long? UnidadeOrganizacionalId { get; set; }

        public CriarOuEditarPrescricaoViewModel(PrescricaoMedicaDto output)
        {
            if(output == null)
            {
                return;
            }

            this.Id = output.Id;
            this.ImportaId = output.ImportaId;
            this.IsDeleted = output.IsDeleted;
            this.IsSistema = output.IsSistema;
            this.Descricao = output.Descricao;
            this.Codigo = output.Codigo;
            this.LastModificationTime = output.LastModificationTime;
            this.LastModifierUserId = output.LastModifierUserId;
            this.CreationTime = output.CreationTime;
            this.CreatorUserId = output.CreatorUserId;
            this.DeletionTime = output.DeletionTime;
            this.DeleterUserId = output.DeleterUserId;

            this.AtendimentoId = output.AtendimentoId;
            this.Atendimento = output.Atendimento;

            this.MedicoId = output.MedicoId;
            this.Medico = output.Medico;
            
            this.Observacao = output.Observacao;

            this.PrescricaoStatusId = output.PrescricaoStatusId;
            this.PrescricaoStatus = output.PrescricaoStatus;

            this.PrestadorId = output.PrestadorId;

            this.DivisaoId = output.DivisaoId;

            this.DataPrescricao = output.DataPrescricao;

            this.UnidadeOrganizacionalId = output.UnidadeOrganizacionalId;
            this.UnidadeOrganizacional = output.UnidadeOrganizacional;
            this.LeitoId = output.LeitoId;
            this.AtendimentoLeitoId = output.AtendimentoLeitoId;
        }
    }
}