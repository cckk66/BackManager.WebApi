using BackManager.Utility.MatrixCard;
using System.Collections.Generic;

namespace BackManager.Utility.MySecretInsuranceCard
{
    public interface ISecretInsuranceCard
    {
        /// <summary>
        /// 创建密保卡
        /// </summary>
        /// <returns></returns>
        (int Rows, int Cols, (string Head,string Body)) Create();
        /// <summary>
        /// 加载密保卡
        /// </summary>
        /// <param name="strMatrix"></param>
        /// <returns></returns>
        (int Rows, int Cols, (string Head, string Body)) Load(string strMatrix);
        /// <summary>
        /// 令牌随机
        /// </summary>
        /// <param name="strMatrix"></param>
        /// <param name="HowMany"></param>
        /// <returns></returns>
        (int[] Row, int[] Col, string PromptingLanguage) PickRandomCells(string strMatrix, int HowMany = 3);
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="Cells"></param>
        /// <returns></returns>
        bool Validate(string strMatrix, int[] Row, int[] Col, int[] CellData);
    }
}
