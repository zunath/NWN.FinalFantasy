using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition
{
    internal interface IAbilityDefinition
    {
        /// <summary>
        /// The name of the ability.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The Feat it is linked to.
        /// </summary>
        Feat Feat { get; }

        /// <summary>
        /// The category under which it falls.
        /// </summary>
        AbilityCategory Category { get; }

        /// <summary>
        /// The group under which it falls.
        /// </summary>
        AbilityGroup Group { get; }

        /// <summary>
        /// If true, other jobs can equip this ability. If false, they cannot.
        /// </summary>
        bool IsEquippable { get; }

        /// <summary>
        /// The amount of AP needed to master this ability. Leave at 0 if you want it to
        /// always be available.
        /// </summary>
        int APRequired { get; }

        /// <summary>
        /// The job requirements needed to progress and master this ability.
        /// </summary>
        JobLevel[] JobRequirements { get; }

        /// <summary>
        /// The amount of MP needed to use this ability.
        /// </summary>
        /// <param name="user">The user of the ability.</param>
        /// <returns>The amount of MP necessary</returns>
        int MP(NWGameObject user);

        /// <summary>
        /// Add custom validation logic here to ensure the user may activate the ability.
        /// </summary>
        /// <param name="user">The user of the ability.</param>
        /// <param name="target">The target of the ability.</param>
        /// <returns>null or whitespace string means valid, anything else means invalid. User will be informed of this message.</returns>
        string CanUse(NWGameObject user, NWGameObject target);

        /// <summary>
        /// The amount of time, in seconds, it takes to use this ability.
        /// </summary>
        /// <param name="user">The user of the ability.</param>
        /// <returns>The amount of time, in seconds, it takes to use this ability.</returns>
        float CastingTime(NWGameObject user);

        /// <summary>
        /// The amount of time, in seconds, before this ability can be used again.
        /// </summary>
        /// <param name="user">The user of the ability.</param>
        /// <returns>The amount of time, in seconds, before this ability can be used again.</returns>
        float CooldownTime(NWGameObject user);

        /// <summary>
        /// Executes when the ability is given to the player.
        /// Typically used for passive abilities.
        /// </summary>
        /// <param name="user">The user receiving this ability.</param>
        void Apply(NWGameObject user);

        /// <summary>
        /// Executes when the ability is used.
        /// </summary>
        /// <param name="user">The user of this ability.</param>
        /// <param name="target">The target of this ability.</param>
        void Impact(NWGameObject user, NWGameObject target);
        

    }
}
