using System;

namespace BackManager.Domain.Model.Sys
{
    public class SysUserSecretInsuranceCard : AggregateRoot, ICreated
    {
        public long UserID { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public string SecretInsuranceHead { get; set; }
        public string SecretInsuranceBody { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedUserId { get; set; }
    }
}
