using Microsoft.Extensions.Logging;

namespace BackManager.Infrastructure.MyEFLogger
{
    /// <summary>
    /// 自定义ILoggerProvider实现类
    /// </summary>
    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);
        public void Dispose() { }
    }
}
