﻿using Google.Protobuf.WellKnownTypes;
using HOLMS.Application.Client;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HOLMS.Scheduler.Jobs {
    class AuthorizationCleanupJob : QuartzJobBase {
        public const string JobGroupString = "AuthorizationCleanup";
        public const string JobNameString = "AuthorizationCleanupJob";

        public override string JobGroup => JobGroupString;
        public override string JobName => JobNameString;

        protected override void ExecuteLogged(IJobExecutionContext context, ApplicationClient ac) {
            ac.Logger.LogInformation($"Beginning card authorization cleanup job for tenancy {ac.SC.TenancyName}");
            ac.FolioAuthCleanupSvc.VoidAuthorizationsForDepartedReservations(new Empty());
        }
    }
}