using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System.Threading.Tasks;

namespace BackManager.Application.Sys
{
    public interface ISysUserSecretInsuranceCardService
    {
        Task<bool> DeleteByUserID(long UserID);
        Task<ApiResult<long>> InsertAsync(SysUserSecretInsuranceCard model);
        Task<bool> MatrixCardValidate(int[] Row, int[] Col, int[] CellData, long UserID);
        Task<(int[] Row, int[] Col, string PromptingLanguage)> PickRandomCells(long UserID);

        Task<SysUserSecretInsuranceCard> LoadSysUserSecretInsuranceCardByUserID(long UserID);
    }
}
