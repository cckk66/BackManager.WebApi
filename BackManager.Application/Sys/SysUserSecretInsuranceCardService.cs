using System;
using System.Threading.Tasks;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using BackManager.Utility;
using BackManager.Utility.MySecretInsuranceCard;

namespace BackManager.Application.Sys
{
    public class SysUserSecretInsuranceCardService : ISysUserSecretInsuranceCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysUserSecretInsuranceCard> _sysUserSecretInsuranceCardRepository;
        private readonly ISecretInsuranceCard _secretInsuranceCard;
        public SysUserSecretInsuranceCardService(
            IUnitOfWork unitOfWork
            , IRepository<SysUserSecretInsuranceCard> sysUserSecretInsuranceCardRepository
            , ISecretInsuranceCard secretInsuranceCard

            )
        {
            _sysUserSecretInsuranceCardRepository = sysUserSecretInsuranceCardRepository;
            _secretInsuranceCard = secretInsuranceCard;

            _unitOfWork = unitOfWork;
        }
        public async Task<bool> DeleteByUserID(long UserID)
        {
            await _sysUserSecretInsuranceCardRepository.DeleteAsync(m => m.UserID == UserID);
            return true;

        }

        public async Task<SysUserSecretInsuranceCard> LoadSysUserSecretInsuranceCardByUserID(long UserID)
        {
            return await _sysUserSecretInsuranceCardRepository.FirstOrDefaultAsync(m => m.UserID == UserID);

        }

        public async Task<(int[] Row, int[] Col, string PromptingLanguage)> PickRandomCells(long UserID)
        {
            SysUserSecretInsuranceCard sysUserSecretInsuranceCard = await this.LoadSysUserSecretInsuranceCardByUserID(UserID);
            if (sysUserSecretInsuranceCard != null)
            {
                return _secretInsuranceCard.PickRandomCells(sysUserSecretInsuranceCard.SecretInsuranceBody);
            }
            return (null, null, "");
        }
        public async Task<bool> MatrixCardValidate(int[] Row, int[] Col, int[] CellData, long UserID)
        {
            SysUserSecretInsuranceCard sysUserSecretInsuranceCard = await this.LoadSysUserSecretInsuranceCardByUserID(UserID);
            if (sysUserSecretInsuranceCard==null)
            {
                return true;
            }
            return _secretInsuranceCard.Validate(sysUserSecretInsuranceCard.SecretInsuranceBody, Row,Col,CellData);

        }
        public async Task<ApiResult<long>> CreateAsync(long UserID)
        {
            (int Rows, int Cols, (string Head, string Body) CellData) MatrixCard = _secretInsuranceCard.Create();
            SysUserSecretInsuranceCard sysUserSecretInsuranceCard = new SysUserSecretInsuranceCard
            {
                Rows = MatrixCard.Rows,
                Cols = MatrixCard.Cols,
                SecretInsuranceHead = MatrixCard.CellData.Head,
                SecretInsuranceBody = MatrixCard.CellData.Body,
                UserID = UserID,
                CreatedAt = DateTime.Now,
                CreatedUserId = UserID
            };
            return await this.InsertAsync(sysUserSecretInsuranceCard);
        }

        public async Task<ApiResult<long>> InsertAsync(SysUserSecretInsuranceCard model)
        {
            SysUserSecretInsuranceCard sysUserSecretInsuranceCard = AutoMapperHelper.MapTo<SysUserSecretInsuranceCard>(model);
            sysUserSecretInsuranceCard = await _sysUserSecretInsuranceCardRepository.InsertAsync(sysUserSecretInsuranceCard);
            _unitOfWork.SaveChanges();
            return await Task.FromResult(ApiResult<long>.Ok(sysUserSecretInsuranceCard.ID));
        }
    }
}
