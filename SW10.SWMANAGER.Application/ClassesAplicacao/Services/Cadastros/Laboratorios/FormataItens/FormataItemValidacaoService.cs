using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItens
{
    public class FormataItemValidacaoService
    {
        private readonly IRepository<FormataItem, long> _formataItemRepository;
        public FormataItemValidacaoService(IRepository<FormataItem, long> formataItemRepository)
        {
            _formataItemRepository = formataItemRepository;
        }


        List<ErroDto> Lista = new List<ErroDto>();
        public List<ErroDto> Validar(FormataItemDto formataItem)
        {
            ValidarDuplicidadeOrdem(formataItem);
            ValidarDuplicidadeCodigo(formataItem);

            return Lista;
        }

        private void ValidarDuplicidadeOrdem(FormataItemDto formataItem)
        {
            var formatacoesItens = _formataItemRepository.GetAll()
                                                         .Where(w => w.FormataId == formataItem.FormataId
                                                                 && w.Ordem == formataItem.Ordem
                                                                 && w.Id != formataItem.Id)
                                                         .Count();

            if (formatacoesItens != 0)
            {
                Lista.Add(new ErroDto { Descricao = "Não é permitido ordem repetida para uma mesma formatação." });
            }
        }

        private void ValidarDuplicidadeCodigo(FormataItemDto formataItem)
        {
            var formatacoesItens = _formataItemRepository.GetAll()
                                                         .Where(w => w.FormataId == formataItem.FormataId
                                                                 && w.ItemResultadoId == formataItem.ItemResultadoId
                                                                 && w.Id != formataItem.Id)
                                                         .Count();

            if (formatacoesItens != 0)
            {
                Lista.Add(new ErroDto { Descricao = "Não é permitido item de resultado repetido para uma mesma formatação." });
            }
        }
    }
}
