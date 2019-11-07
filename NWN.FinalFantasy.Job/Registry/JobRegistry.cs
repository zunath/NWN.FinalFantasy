using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;
using NWN.FinalFantasy.Job.JobDefinition;

namespace NWN.FinalFantasy.Job.Registry
{
    internal static class JobRegistry
    {
        private static readonly Dictionary<JobType, JobDefinitionBase> _jobs = new Dictionary<JobType, JobDefinitionBase>();

        public static void Register()
        {
            _jobs[JobType.Warrior] = new Warrior();
        }

        public static JobDefinitionBase Get(JobType type)
        {
            if(!_jobs.ContainsKey(type))
                throw new Exception($"Job type {type} has not been registered.");

            return _jobs[type];
        }
    }
}
