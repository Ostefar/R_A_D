using Serilog;

namespace Monitorings { 
    public static class MonitorService 
    {
        public static Serilog.ILogger Log => Serilog.Log.Logger;
        static MonitorService() 
        {
            var seqUrl = "http://localhost:5341";
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.Seq(seqUrl)
                .CreateLogger();

            /*
                 var log = new LoggerConfiguration()
                .WriteTo.Seq(seqUrl)
                .CreateLogger();

                log.Information("Hello, Seq!");
             */

        }
    }
}