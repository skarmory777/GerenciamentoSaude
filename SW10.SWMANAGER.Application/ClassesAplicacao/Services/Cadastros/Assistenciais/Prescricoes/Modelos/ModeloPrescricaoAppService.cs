using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.ModelosPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Modelos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using Abp.Linq.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Modelos
{
    public class ModeloPrescricaoAppService : SWMANAGERAppServiceBase, IModeloPrescricaoAppService
    {
        public async Task<PagedResultDto<ModelePrescricaoIndex>> Listar(ModeloPrescricaoListarInput input)
        {
            using (var ModeloPrescricaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloPrescricao, long>>())
            {
                var query = ModeloPrescricaoRepository.Object
                                                      .GetAll();

                var count = await query.CountAsync();

                var result = query.AsQueryable()
                            .AsNoTracking()
                            .OrderBy(input.Sorting)
                            .PageBy(input)
                            .Select(s => new ModelePrescricaoIndex
                            {
                                Id = s.AtendimentoId,
                                Codigo= s.Codigo,
                                Descricao=s.Descricao
                            })
                            .ToList();
                return new PagedResultDto<ModelePrescricaoIndex> { TotalCount = count, Items = result };

            }
               
        }
    }
}
