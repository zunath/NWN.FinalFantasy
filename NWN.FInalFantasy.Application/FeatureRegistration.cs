using System;
using System.Collections.Generic;
using NWN.FinalFantasy.AI;
using NWN.FinalFantasy.Auditing;
using NWN.FinalFantasy.Authorization;
using NWN.FinalFantasy.Chat;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Death;
using NWN.FinalFantasy.Item;
using NWN.FinalFantasy.Item.Storage;
using NWN.FinalFantasy.Job;
using NWN.FinalFantasy.Location;
using NWN.FinalFantasy.Loot;
using NWN.FinalFantasy.Map;
using NWN.FinalFantasy.Menu;
using NWN.FinalFantasy.Migration;
using NWN.FinalFantasy.Quest;
using NWN.FinalFantasy.Roleplay;
using Serilog;

namespace NWN.FinalFantasy.Application
{
    internal class FeatureRegistration
    {
        private static readonly List<IFeatureRegistration> _registrations = new List<IFeatureRegistration>();

        public static void Register()
        {
            Log.Information("Registering Features...");
            _registrations.Add(new AIRegistration());
            _registrations.Add(new AuditingRegistration());
            _registrations.Add(new AuthorizationRegistration());
            _registrations.Add(new ChatRegistration());
            _registrations.Add(new DataRegistration());
            _registrations.Add(new DeathRegistration());
            _registrations.Add(new ItemRegistration());
            _registrations.Add(new ItemStorageRegistration());
            _registrations.Add(new JobRegistration());
            _registrations.Add(new LocationRegistration());
            _registrations.Add(new LootRegistration());
            _registrations.Add(new MapRegistration());
            _registrations.Add(new MenuRegistration());
            _registrations.Add(new MigrationRegistration());
            _registrations.Add(new QuestRegistration());
            _registrations.Add(new RoleplayRegistration());

            bool hasError = false;
            foreach (var registration in _registrations)
            {
                try
                {
                    registration.Register();
                }
                catch (Exception ex)
                {
                    Audit.Write(AuditGroup.Error, "Feature Registration Error: " + ex.ToMessageAndCompleteStacktrace());
                    hasError = true;
                }
            }

            if(!hasError)
                Log.Information("All features registered successfully!");
            else 
                Log.Warning("One or more features failed to register. Refer to the Error audit log for more information.");
        }
    }
}
