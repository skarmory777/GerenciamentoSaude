using Amazon.S3.Model;
using SW10.SWMANAGER.ClassesAplicacao.Services.AWS.Dto;
using System.Threading.Tasks;
using System.Web;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AWS
{
    public interface IAwsS3AppService
    {
        Task<SaveObjectResponseDto> SaveObjectAsync(HttpPostedFileBase file, string key);
        Task<DeleteObjectResponse> DeleteObjectAsync(string objectKey);
    }
}
