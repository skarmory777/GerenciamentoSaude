using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Enderecos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas
{
    public class SisPessoaAppService : SWMANAGERAppServiceBase, ISisPessoaAppService
    {
        private readonly IRepository<SisPessoa, long> _sisPessoaRepository;


        public SisPessoaAppService(IRepository<SisPessoa, long> sisPessoaRepository)
        {
            _sisPessoaRepository = sisPessoaRepository;
        }



        public async Task<SisPessoaDto> ObterPorCPF(string cpf)
        {
            try
            {
                var result = await _sisPessoaRepository
                    .GetAll()
                             .Include(m => m.Nacionalidade)
                             .Include(m => m.Naturalidade)
                             .Include(m => m.Profissao)
                             .Include(m => m.Enderecos)
                             .Include(m => m.Enderecos.Select(s => s.TipoLogradouro))
                             .Include(m => m.Enderecos.Select(s => s.Cidade))
                             .Include(m => m.Enderecos.Select(s => s.Estado))
                             .Include(m => m.Enderecos.Select(s => s.Pais))

                    .Where(m => m.Cpf == cpf)
                    .FirstOrDefaultAsync();


                SisPessoaDto pessoa = null;

                if (result != null)
                {

                    pessoa = result
                        .MapTo<SisPessoaDto>();
                    pessoa.Enderecos = new List<EnderecoDto>();
                    int idGrid = 0;
                    foreach (var item in result.Enderecos)
                    {
                        var enderecoDto = item.MapTo<EnderecoDto>();

                        enderecoDto.TipoLogradouroDescricao = item.TipoLogradouro?.Descricao;
                        enderecoDto.CidadeDescricao = item.Cidade != null ? item.Cidade.Nome : string.Empty;
                        enderecoDto.EstadoDescricao = item.Estado != null ? item.Estado.Nome : string.Empty;
                        enderecoDto.PaisDescricao = item.Pais != null ? item.Pais.Nome : string.Empty;

                        enderecoDto.IdGrid = idGrid++;

                        pessoa.Enderecos.Add(enderecoDto);
                    }
                    pessoa.EnderecosJson = JsonConvert.SerializeObject(pessoa.Enderecos);
                }



                return pessoa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownSisIsPagar(DropdownInput dropdownInput)
        {
            bool IsCredito = dropdownInput.filtro == "true";

            return await ListarDropdownLambda(dropdownInput
                                                     , _sisPessoaRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.NomeFantasia.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                    )

                                                    && m.TipoPessoa.IsPagar == !IsCredito
                                                    && m.TipoPessoa.IsReceber == IsCredito

                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.FisicaJuridica == "F" ? p.NomeCompleto : p.NomeFantasia) }
                                                    , o => o.NomeFantasia
                                                    );

        }

        public async Task<IResultDropdownList<long>> ListarDropdownPJ(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(dropdownInput
                                                     , _sisPessoaRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    && m.FisicaJuridica == "J"

                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.FisicaJuridica == "F" ? p.NomeCompleto : p.NomeFantasia) }
                                                    , o => o.NomeFantasia
                                                    );

        }

        public async Task<IResultDropdownList<long>> ListarDropdownPessoa(DropdownInput dropdownInput)
        {


            return await ListarDropdownLambda(dropdownInput
                                                     , _sisPessoaRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.NomeFantasia.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                    )

                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.FisicaJuridica == "F" ? p.NomeCompleto : p.NomeFantasia) }
                                                    , o => o.NomeFantasia
                                                    );

        }

        public async Task<SisPessoaDto> ObterPorCnpj(string cnpj)
        {
            try
            {
                var result = await _sisPessoaRepository
                    .GetAll()
                             .Include(m => m.Nacionalidade)
                             .Include(m => m.Naturalidade)
                             .Include(m => m.Profissao)
                             .Include(m => m.Enderecos.Select(s => s.TipoLogradouro))
                             .Include(m => m.Enderecos.Select(s => s.Cidade))
                             .Include(m => m.Enderecos.Select(s => s.Estado))
                             .Include(m => m.Enderecos.Select(s => s.Pais))
                    .Where(m => m.Cnpj == cnpj)
                    .FirstOrDefaultAsync();


                var pessoa = result
                       .MapTo<SisPessoaDto>();

                if (pessoa != null)
                {
                    pessoa.Enderecos = new List<EnderecoDto>();

                    int idGrid = 0;
                    foreach (var item in result.Enderecos)
                    {
                        var enderecoDto = item.MapTo<EnderecoDto>();
                        enderecoDto.TipoLogradouroDescricao = item.TipoLogradouro?.Descricao;
                        enderecoDto.CidadeDescricao = item.Cidade != null ? item.Cidade.Nome : string.Empty;
                        enderecoDto.EstadoDescricao = item.Estado != null ? item.Estado.Nome : string.Empty;
                        enderecoDto.PaisDescricao = item.Pais != null ? item.Pais.Nome : string.Empty;

                        enderecoDto.IdGrid = idGrid++;
                        pessoa.Enderecos.Add(enderecoDto);

                    }
                    pessoa.EnderecosJson = JsonConvert.SerializeObject(pessoa.Enderecos);


                }
                return pessoa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdownClinicas(DropdownInput dropdownInput)
        {
            IQueryable<DropdownItems> query;
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            
            try
            {
                var clinicas = _sisPessoaRepository.GetAll().Where(x => x.TipoPessoaId == 7);

                if (!dropdownInput.search.IsNullOrEmpty())
                {
                    clinicas = clinicas.Where(x => 
                        x.Codigo.Contains(dropdownInput.search) || x.NomeFantasia.Contains(dropdownInput.search));
                }
                
                query = from p in clinicas
                        orderby p.NomeFantasia ascending
                        select new DropdownItems 
                        { 
                            id = p.Id, 
                            text = p.NomeFantasia 
                        };
               
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                return new ResultDropdownList() { Items = await queryResultPage.ToListAsync(), TotalCount = await queryResultPage.CountAsync() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex.InnerException);
            }
        }
    }
}
