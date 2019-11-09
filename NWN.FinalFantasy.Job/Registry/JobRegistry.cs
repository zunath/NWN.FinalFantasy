using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;
using NWN.FinalFantasy.Job.JobDefinition;

namespace NWN.FinalFantasy.Job.Registry
{
    internal static class JobRegistry
    {
        private static readonly Dictionary<ClassType, JobDefinitionBase> _jobs = new Dictionary<ClassType, JobDefinitionBase>();

        public static void Register()
        {
            _jobs[ClassType.Warrior] = new Warrior();
            _jobs[ClassType.Monk] = new Monk();
            _jobs[ClassType.Thief] = new Thief();
            _jobs[ClassType.Ranger] = new Ranger();
            _jobs[ClassType.WhiteMage] = new WhiteMage();
            _jobs[ClassType.BlackMage] = new BlackMage();
        }

        /// <summary>
        /// Retrieves a specific job definition by its class type.
        /// </summary>
        /// <param name="type">The type of class to retrieve</param>
        /// <returns>The job definition</returns>
        public static JobDefinitionBase Get(ClassType type)
        {
            if(!_jobs.ContainsKey(type))
                throw new Exception($"Job type {type} has not been registered.");

            return _jobs[type];
        }

        /// <summary>
        /// Retrieves all registered jobs.
        /// </summary>
        /// <returns>All registered jobs</returns>
        public static IEnumerable<JobDefinitionBase> GetAll()
        {
            return _jobs.Values.ToList();
        }
    }
}
