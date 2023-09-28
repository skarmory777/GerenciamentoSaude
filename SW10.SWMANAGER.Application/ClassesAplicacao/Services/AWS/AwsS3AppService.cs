using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.AWS.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3.Dto;
using System;
using System.Threading.Tasks;
using System.Web;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AWS
{
    public class AwsS3AppService : SWMANAGERAppServiceBase, IAwsS3AppService
    {        
        private RegionEndpoint bucketRegion;
        private IAmazonS3 s3Client;
        private readonly AwsS3ConfiguracaoAppService _awsS3ConfiguracaoAppService;
        private AwsS3ConfiguracaoDto configuracao;

        public AwsS3AppService(AwsS3ConfiguracaoAppService awsS3ConfiguracaoAppService)
        {
            _awsS3ConfiguracaoAppService = awsS3ConfiguracaoAppService;
        }

        private async Task InitializeS3Client()
        {
            configuracao = await _awsS3ConfiguracaoAppService.ObterConfiguracao(1);
            
            if (configuracao == null)
                throw new Exception("Serviço não disponível.");

            bucketRegion = RegionEndpoint.GetBySystemName(configuracao.BucketRegion);
            s3Client = new AmazonS3Client(configuracao.AcessKey, configuracao.SecretKey, bucketRegion);
        }

        public async Task<SaveObjectResponseDto> SaveObjectAsync(HttpPostedFileBase file, string key)
        {
            var saveObjectResponseDto = new SaveObjectResponseDto();
            await InitializeS3Client();

            var request = new PutObjectRequest
            {
                BucketName = configuracao.BucketName,
                Key = key,
                InputStream = file.InputStream,
                ContentType = file.ContentType
            };

            var response = await s3Client.PutObjectAsync(request);

            saveObjectResponseDto = Mapper.Map<SaveObjectResponseDto>(response);
            saveObjectResponseDto.BucketName = configuracao.BucketName;

            return saveObjectResponseDto;
        }

        public async Task<DeleteObjectResponse> DeleteObjectAsync(string objectKey)
        {
            await InitializeS3Client();

            var response = await s3Client.DeleteObjectAsync(configuracao.BucketName, objectKey);

            return response;
        }
    }
}
