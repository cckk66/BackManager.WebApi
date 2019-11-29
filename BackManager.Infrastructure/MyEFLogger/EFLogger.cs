using Microsoft.Extensions.Logging;
using System;

namespace BackManager.Infrastructure.MyEFLogger
{
    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logContent = formatter(state, exception);
            //TODO: 拿到日志内容想怎么玩就怎么玩吧
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(logContent);
            Console.ResetColor();
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
