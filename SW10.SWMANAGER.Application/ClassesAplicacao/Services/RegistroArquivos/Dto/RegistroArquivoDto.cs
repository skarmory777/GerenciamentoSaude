namespace SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;

    public class RegistroArquivoDto : CamposPadraoCRUDDto
    {
        public long RegistroId { get; set; }
        public long? RegistroTabelaId { get; set; }
        public string Campo { get; set; }
        public byte[] Arquivo { get; set; }

        /// <summary>
        /// Gets or sets the arquivo nome.
        /// </summary>
        public string ArquivoNome { get; set; }

        /// <summary>
        /// Gets or sets the arquivo tipo.
        /// </summary>
        public string ArquivoTipo { get; set; }

        public long? AtendimentoId { get; set; }

        public static RegistroArquivoDto Mapear(RegistroArquivo registroArquivo)
        {
            return new RegistroArquivoDto
            {
                Id = registroArquivo.Id,
                RegistroId = registroArquivo.RegistroId,
                RegistroTabelaId = registroArquivo.RegistroTabelaId,
                Campo = registroArquivo.Campo,
                Arquivo = registroArquivo.Arquivo,
                ArquivoNome = registroArquivo.ArquivoNome,
                ArquivoTipo = registroArquivo.ArquivoTipo,
                AtendimentoId = registroArquivo.AtendimentoId,
                Descricao = registroArquivo.Descricao,
                Codigo = registroArquivo.Codigo
            };
        }

        public static RegistroArquivo Mapear(RegistroArquivoDto registroArquivoDto)
        {
            return new RegistroArquivo
            {
                Id = registroArquivoDto.Id,
                RegistroId = registroArquivoDto.RegistroId,
                RegistroTabelaId = registroArquivoDto.RegistroTabelaId,
                Campo = registroArquivoDto.Campo,
                Arquivo = registroArquivoDto.Arquivo,
                AtendimentoId = registroArquivoDto.AtendimentoId,
                Descricao = registroArquivoDto.Descricao,
                Codigo = registroArquivoDto.Codigo
            };
        }

        public static long ObterTabelaRegistroFormularioDinamico(long operacaoId)
        {
            long registroTabelaId = 0;
            switch (operacaoId)
            {
                case 15:
                    registroTabelaId = (long)EnumArquivoTabela.PrescricaoEnfermagem;
                    break;

                case 369:
                case 186:
                    registroTabelaId = (long)EnumArquivoTabela.EvolucaoEnfermagem;
                    break;

                case 13:
                    registroTabelaId = (long)EnumArquivoTabela.AdmisaoEnfermagem;
                    break;

                case 14:
                    registroTabelaId = (long)EnumArquivoTabela.PassagemPlantao;
                    break;

                case 19:
                    registroTabelaId = (long)EnumArquivoTabela.AdmissaoMedica;
                    break;

                case 20:
                    registroTabelaId = (long)EnumArquivoTabela.AltaMedica;
                    break;

                case 21:
                    registroTabelaId = (long)EnumArquivoTabela.Anamnese;
                    break;

                case 22:
                    registroTabelaId = (long)EnumArquivoTabela.EvolucaoMedica;
                    break;
            }



            return registroTabelaId;
        }
    }
}
