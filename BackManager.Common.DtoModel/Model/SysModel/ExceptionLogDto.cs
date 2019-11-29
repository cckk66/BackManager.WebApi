using BackManager.Domain.Model.Sys;
using System;

namespace BackManager.Common.DtoModel
{
    public class ExceptionLogDto
    {
        public long UserID { get; set; }

        public string Message { get; set; }
        public string InnerException { get; set; }
        public string OpMethod { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ESouceType SouceType { get; set; } = ESouceType.Backstage;
    }
}
