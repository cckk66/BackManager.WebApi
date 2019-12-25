using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.Login;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public interface ISysUserService : IDataEntityAsync<SysUserDto>
    {
        SysUser User();
        Task<ApiResult<SysUserDto>> Login(LoginUserDto loginUserDto);
        Task<ApiResult<SysUserSecretInsuranceCard>> GetMatrixCard(long userID);
        Task<ApiResult<bool>> MatrixCardValidate(int[] Row, int[] Col, int[] CellData, long UserID);
        Task<ApiResult<string>> ReUserPassword(ReUserPasswordDto userPasswordDto);
    }
}