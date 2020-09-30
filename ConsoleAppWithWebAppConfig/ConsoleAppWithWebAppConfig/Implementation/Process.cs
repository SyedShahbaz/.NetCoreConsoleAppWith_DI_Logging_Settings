using System;
using System.Collections.Generic;
using System.Text;
using ConsoleAppWithWebAppConfig.Abstraction;
using Microsoft.Extensions.Logging;

namespace ConsoleAppWithWebAppConfig.Implementation
{
    public class Process : IProcess
    {
        private readonly ILogger<Process> _log;

        public Process(ILogger<Process> log)
        {
            _log = log;
        }
        public void ProcessStarter()
        {
            Console.WriteLine(Environment.NewLine);
            _log.LogInformation("Process has started, All initializations are successful.. ");
        }
    }
}
