﻿using System;
using HOLMS.Platform.Client;
using HOLMS.Scheduler.Support;
using HOLMS.Types.IAM.RPC;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace HOLMS.Scheduler {
    public static class Globals {
        private static ApplicationClient _ac;
        private static ILogger _logger;

        // NOTE(DA) I have no love for singletons, but sharing this across the tasks 
        // (with internal auto-refresh) did seem particularly compelling.
        public static ApplicationClient AC => _ac ?? (_ac = GetAppServiceClient());
        public static ILogger Logger => _logger ?? (_logger = GetLogger());

        private static ApplicationClient GetAppServiceClient() {
            var logger = GetLogger();

            try {
                var settings = new SchedulerServiceSettings(logger);
                var ac = new ApplicationClient(settings, logger, "BQ0HIJDQ91TKCKCBV8UGPO7M5OTNIOYA7L19A6QKBGF76S0L4D4CFV4GMM9D");
                var res = ac.StartSession(settings.GetServiceUsername(),
                    settings.GetServicePassword(), null).Result;
                ac.Logger.LogDebug("Created app service client");

                if (res != SessionSvcStartSessionResult.Success) {
                    logger.LogCritical($"Could not start session with username {settings.GetServiceUsername()}");
                    throw new Exception();
                }
                ac.Logger.LogDebug("Started app service session");

                return ac;
            } catch {
                logger.LogCritical("App service creation/session initiation failed");
                throw;
            }
        }

        private static ILogger GetLogger() {
            var lf = new LoggerFactory();
            var els = new EventLogSettings {
                LogName = "HOLMS",
                SourceName = "Scheduler"
            };
            lf.AddEventLog(els);

            return lf.CreateLogger("Scheduler");
        }
    }
}
