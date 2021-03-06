﻿using System;
using Google.Protobuf.WellKnownTypes;
using HOLMS.Platform.Client;
using Quartz;

namespace HOLMS.Scheduler.Jobs {
    class SupplyHistorySnapshotJob : QuartzJobBase {
        public const string JobGroupString = "Supply";
        public const string JobNameString = "HistorySnapshotter";
        public static TimeSpan JobPeriod => new TimeSpan(0, 5, 0);

        public override string JobGroup => JobGroupString;
        public override string JobName => JobNameString;
        
        protected override void ExecuteLogged(IJobExecutionContext context, ApplicationClient ac) {
            ac.SupplyHistorySnapshotSvc.TakeSnapshotIfDue(new Empty());
        }
    }
}
