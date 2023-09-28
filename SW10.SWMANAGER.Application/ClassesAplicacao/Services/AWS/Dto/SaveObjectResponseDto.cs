using Abp.AutoMapper;
using Amazon.S3.Model;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AWS.Dto
{
    [AutoMapTo(typeof(PutObjectResponse))]
    public class SaveObjectResponseDto : PutObjectResponse
    {
        public string BucketName { get; set; }
    }
}
