using System;
using System.Collections.Generic;
using BackManager.Utility.MatrixCard;
using System.Linq;
using System.Text;

namespace BackManager.Utility.MySecretInsuranceCard
{
    public class SecretInsuranceCard : ISecretInsuranceCard
    {
        public (int Rows, int Cols, (string Head, string Body)) Create()
        {
            Card thisCard = new Card().GenerateData();
            return (thisCard.Rows, thisCard.Cols, (thisCard.CellHead,thisCard.CellData));
        }

        public (int Rows, int Cols, (string Head, string Body)) Load(string strMatrix)
        {
            Card thisCard = new Card().LoadCellData(strMatrix);
            return (thisCard.Rows, thisCard.Cols, (thisCard.CellHead, thisCard.CellData));
        }

        public (int[] Row, int[] Col,string PromptingLanguage) PickRandomCells(string strMatrix, int HowMany = 3)
        {
            Card thisCard = new Card().LoadCellData(strMatrix);
            var cellsToValidate = thisCard.PickRandomCells(HowMany).ToList();
            var sb = new StringBuilder();
            foreach (var t in cellsToValidate)
            {
                sb.Append($"[{t.ColumnName}{t.RowIndex}] ");
            }
            return (
                cellsToValidate.Select(m => m.RowIndex).ToArray(),
                cellsToValidate.Select(m => m.ColIndex).ToArray(),
                sb.ToString()
                );
        }

        public bool Validate(string strMatrix,int[] Row, int[] Col, int[] CellData)
        {
            if (Row.Length != Col.Length || Col.Length != CellData.Length)
            {
                throw new ArgumentNullException("传入参数错误,各参赛长度不相等!");

            }
            List<Cell> Cells = new List<Cell>();
            for (int i = 0; i < Row.Length; i++)
            {
                Cells.Add(new Cell(Row[0], Col[0], CellData[0]));
            }
            return new Card().LoadCellData(strMatrix).Validate(Cells);
        }
    }
}
