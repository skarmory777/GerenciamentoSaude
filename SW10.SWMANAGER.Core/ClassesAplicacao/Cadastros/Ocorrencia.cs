using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias
{
    [Table("SisOcorrencias")]
    public class Ocorrencia : CamposPadraoCRUD
    {
        public DateTime Data { get; set; }
        public long TipoOcorrenciaId { get; set; }
        public TipoOcorrencia TipoOcorrencia { get; set; }
        
        public long? SubTipoOcorrenciaId { get; set; }
        
        public SubTipoOcorrencia SubTipoOcorrencia { get; set; }
        
        public string SourceModel { get; set; }
        
        public long? SourceId { get; set; }
        
        public string RelationModel { get; set; }
        
        public long? RelationId { get; set; }
        
        public string Texto { get; set; }


        public static Ocorrencia Criar<TSourceModel, TRelationModel>(DateTime data, string texto, long tipOcorrenciaId,
            long? subTipoOcorrenciaId, TSourceModel sourceModel, TRelationModel relationModel = null ) where TSourceModel : CamposPadraoCRUD  where TRelationModel : CamposPadraoCRUD
        {
            return new Ocorrencia
            {
                Data = data,
                Texto = texto,
                TipoOcorrenciaId = tipOcorrenciaId,
                SubTipoOcorrenciaId = subTipoOcorrenciaId,
                SourceModel = typeof(TSourceModel).FullName,
                SourceId = sourceModel.Id,
                RelationModel = relationModel != null?  typeof(TRelationModel).FullName: default(string),
                RelationId = relationModel != null?  relationModel.Id: default(long?)
            };
        }
        
        public static Ocorrencia Criar<TSourceModel, TRelationModel>(DateTime data, string texto, long tipOcorrenciaId,
            long? subTipoOcorrenciaId, TSourceModel sourceModel) where TSourceModel : CamposPadraoCRUD
        {
            return new Ocorrencia
            {
                Data = data,
                Texto = texto,
                TipoOcorrenciaId = tipOcorrenciaId,
                SubTipoOcorrenciaId = subTipoOcorrenciaId,
                SourceModel = typeof(TSourceModel).FullName,
                SourceId = sourceModel.Id
            };
        }
        
        public static Ocorrencia Criar(DateTime data, string texto, long tipOcorrenciaId,
            long? subTipoOcorrenciaId, string sourceModel, long? sourceId, string relationModel = null, long? relationId = null, bool IsSistema = true )
        {
            return new Ocorrencia
            {
                Data = data,
                Texto = texto,
                TipoOcorrenciaId = tipOcorrenciaId,
                SubTipoOcorrenciaId = subTipoOcorrenciaId,
                SourceModel = sourceModel,
                SourceId = sourceId,
                RelationModel = relationModel,
                RelationId = relationId,
                IsSistema = IsSistema
            };
        }

        public static string GetNameByEntityModel(string entityName)
        {
            if (entityName == null)
            {
                return null;
            }

            var sourceModel = GetModels().FirstOrDefault(x => x.EntityName == entityName);

            return sourceModel?.Name;
        }

        // Todo ver como isso irá performar
        private static IEnumerable<OcorrenciaSourceModel> GetModels()
        {
            return new List<OcorrenciaSourceModel>
            {
                OcorrenciaSourceModel.Criar("atendimento", "Atendimento", typeof(Atendimento).FullName),
                OcorrenciaSourceModel.Criar("contamedica", "Conta Médica", typeof(FaturamentoConta).FullName),
                OcorrenciaSourceModel.Criar("contamedicaitem", "Conta Médica Item",
                    typeof(FaturamentoContaItem).FullName),
            };
        }

        public class OcorrenciaSourceModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            
            public string EntityName { get; set; }

            public static OcorrenciaSourceModel Criar(string id, string name, string entityName)
            {
                return new OcorrenciaSourceModel
                {
                    Id = id,
                    Name = name,
                    EntityName = entityName,
                };
            }
        }

        public static string GetEntityModelById(string model)
        {
            if (model == null)
            {
                return null;
            }

            var sourceModel = GetModels().FirstOrDefault(x => x.Id == model.ToLower());

            return sourceModel?.EntityName;
        }
    }


    public static class OcorrenciaTexto
    {
        public static string KitIncluidoContaMedica(string fatKitDescricao, string contaMedicaCodigo, string userName)
        {
            return $@"Kit  {fatKitDescricao} Incluído na conta Médica {contaMedicaCodigo} - por {userName}";
        }
        
        public static string ContaMedicaItemCriada(string contaItemCodigo, string fatItemDescricao, string userName)
        {
            return $@"Conta Médica Item criado {contaItemCodigo} - {fatItemDescricao} por {userName}";
        }
        
        public static string ContaMedicaItemAlterado(string contaItemCodigo, string fatItemDescricao, string userName)
        {
            return $@"Conta Médica Item alterado {contaItemCodigo} - {fatItemDescricao} por {userName}";
        }

        public static string ContaMedicaCriada(string contaMedicaCodigo, string codigoAtendimento, string userName)
        {
            return $"Conta Médica criada {contaMedicaCodigo} no atendimento {codigoAtendimento} por {userName}";
        }
        
        public static string ContaMedicaAlterada(string contaMedicaCodigo, string codigoAtendimento, string userName)
        {
            return $"Conta Médica editada {contaMedicaCodigo} no atendimento {codigoAtendimento} por {userName}";
        }

        public static string ContaMedicaItemCriadaPorKit(string contaItemCodigo, string fatItemDescricao,
            string fatKitDescricao, string userName)
        {
            return $@"Conta Médica Item criado {contaItemCodigo} - {fatItemDescricao} Kit: {fatKitDescricao} por {userName}";
        }
    }
}