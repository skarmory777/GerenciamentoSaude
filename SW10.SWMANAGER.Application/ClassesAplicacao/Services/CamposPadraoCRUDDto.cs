using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(CamposPadraoCRUD))]
    public abstract class CamposPadraoCRUDDto : FullAuditedEntityDto<long>
    {
        public bool IsSistema { get; set; }

        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual int? ImportaId { get; set; }

        public static Td MapearBase<Td>(CamposPadraoCRUD to) where Td : CamposPadraoCRUDDto
        {
            if (to == null)
            {
                return null;
            }

            var td = (Td)Activator.CreateInstance(typeof(Td)); //new Td();

            td.Id = to.Id;
            td.Codigo = to.Codigo;
            td.Descricao = to.Descricao;

            td.IsDeleted = to.IsDeleted;
            td.DeleterUserId = to.DeleterUserId;
            td.DeletionTime = to.DeletionTime;
            td.CreatorUserId = to.CreatorUserId;
            td.CreationTime = to.CreationTime;
            td.IsSistema = to.IsSistema;
            td.LastModificationTime = to.LastModificationTime;
            td.LastModifierUserId = to.LastModifierUserId;
            td.ImportaId = to.ImportaId;

            return td;
        }

        public static IEnumerable<Td> MapearBase<Td>(List<CamposPadraoCRUD> tos) where Td: CamposPadraoCRUDDto
        {
            foreach(var to in tos)
            {
                yield return MapearBase<Td>(to);
            }
        }

        public static Td MapearBase<Td>(CamposPadraoCRUDDto to) where Td : CamposPadraoCRUD
        {
            if (to == null)
            {
                return null;
            }

            var td = (Td)Activator.CreateInstance(typeof(Td)); //new Td();

            td.Id = to.Id;
            td.Codigo = to.Codigo;
            td.Descricao = to.Descricao;

            td.IsDeleted = to.IsDeleted;
            td.DeleterUserId = to.DeleterUserId;
            td.DeletionTime = to.DeletionTime;
            td.CreatorUserId = to.CreatorUserId;
            td.CreationTime = to.CreationTime;
            td.IsSistema = to.IsSistema;
            td.LastModificationTime = to.LastModificationTime;
            td.LastModifierUserId = to.LastModifierUserId;
            td.ImportaId = to.ImportaId;

            return td;
        }

        public static CamposPadraoCRUDDto MapearBase(CamposPadraoCRUD to, CamposPadraoCRUDDto td)// where Td : CamposPadraoCRUDDto
        {
            if (to == null)
            {
                return null;
            }

            td.Id = to.Id;
            td.Codigo = to.Codigo;
            td.Descricao = to.Descricao;

            td.IsDeleted = to.IsDeleted;
            td.DeleterUserId = to.DeleterUserId;
            td.DeletionTime = to.DeletionTime;
            td.CreatorUserId = to.CreatorUserId;
            td.CreationTime = to.CreationTime;
            td.IsSistema = to.IsSistema;
            td.LastModificationTime = to.LastModificationTime;
            td.LastModifierUserId = to.LastModifierUserId;
            td.ImportaId = to.ImportaId;

            return td;
        }

    }
}
