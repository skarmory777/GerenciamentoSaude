using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class ClassificacaoAtendimentoDto : CamposPadraoCRUDDto
    {
        public string Cor { get; set; }
        public int Prioridade { get; set; }
        public int PrazoAtendimento { get; set; }
        public bool Ativo { get; set; }

        public static ClassificacaoAtendimentoDto Mapear(ClassificacaoAtendimento classificacaoAtendimento)
        {
            if (classificacaoAtendimento == null)
            {
                return null;
            }

            var classificacaoAtendimentoDto = MapearBase<ClassificacaoAtendimentoDto>(classificacaoAtendimento);

            classificacaoAtendimentoDto.Id = classificacaoAtendimento.Id;
            classificacaoAtendimentoDto.Codigo = classificacaoAtendimento.Codigo;
            classificacaoAtendimentoDto.Descricao = classificacaoAtendimento.Descricao;
            classificacaoAtendimentoDto.Cor = classificacaoAtendimento.Cor;
            classificacaoAtendimentoDto.Prioridade = classificacaoAtendimento.Prioridade;
            classificacaoAtendimentoDto.PrazoAtendimento = classificacaoAtendimento.PrazoAtendimento;
            classificacaoAtendimentoDto.Ativo = classificacaoAtendimento.Ativo;

            return classificacaoAtendimentoDto;
        }
    }
}
