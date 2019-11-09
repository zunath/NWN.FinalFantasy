using System;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWScript.Enumerations;

// ReSharper disable once CheckNamespace
namespace NWN
{
    public static class _
    {
        //  Assign aActionToAssign to oActionSubject.
        //  * No return value, but if an error occurs, the log file will contain
        //    "AssignCommand failed."
        //    (If the object doesn't exist, nothing happens.)
        //  Delay aActionToDelay by fSeconds.
        //  * No return value, but if an error occurs, the log file will contain
        //    "DelayCommand failed.".
        //  It is suggested that functions which create effects should not be used
        //  as parameters to delayed actions.  Instead, the effect should be created in the
        //  script and then passed into the action.  For example:
        //  effect eDamage = EffectDamage(nDamage, DAMAGE_TYPE_MAGICAL);
        //  DelayCommand(fDelay, ApplyEffectToObject(DURATION_TYPE_INSTANT, eDamage, oTarget);
        //  Make oTarget run sScript and then return execution to the calling script.
        //  If sScript does not specify a compiled nothing happens.
        public static void ExecuteScript(string sScript, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushString(sScript);
            Internal.CallBuiltIn(8);
        }

        //  Clear all the actions of the caller.
        //  * No return value, but if an error occurs, the log file will contain
        //    "ClearAllActions failed.".
        //  - nClearCombatState: if true, this will immediately clear the combat state
        //    on a creature, which will stop the combat music and allow them to rest,
        //    engage in dialog, or other actions that they would normally have to wait for.
        public static void ClearAllActions(bool nClearCombatState = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nClearCombatState));
            Internal.CallBuiltIn(9);
        }

        //  Cause the caller to face fDirection.
        //  - fDirection is expressed as anticlockwise degrees from Due East.
        //    DIRECTION_EAST, DIRECTION_NORTH, DIRECTION_WEST and DIRECTION_SOUTH are
        //    predefined. (0.0f=East, 90.0f=North, 180.0f=West, 270.0f=South)
        public static void SetFacing(float fDirection)
        {
            Internal.StackPushFloat(fDirection);
            Internal.CallBuiltIn(10);
        }

        //  Set the calendar to the specified date.
        //  - nYear should be from 0 to 32000 inclusive
        //  - nMonth should be from 1 to 12 inclusive
        //  - nDay should be from 1 to 28 inclusive
        //  1) Time can only be advanced forwards; attempting to set the time backwards
        //     will result in no change to the calendar.
        //  2) If values larger than the month or day are specified, they will be wrapped
        //     around and the overflow will be used to advance the next field.
        //     e.g. Specifying a year of 1350, month of 33 and day of 10 will result in
        //     the calender being set to a year of 1352, a month of 9 and a day of 10.
        public static void SetCalendar(int nYear, int nMonth, int nDay)
        {
            Internal.StackPushInteger(nDay);
            Internal.StackPushInteger(nMonth);
            Internal.StackPushInteger(nYear);
            Internal.CallBuiltIn(11);
        }

        //  Set the time to the time specified.
        //  - nHour should be from 0 to 23 inclusive
        //  - nMinute should be from 0 to 59 inclusive
        //  - nSecond should be from 0 to 59 inclusive
        //  - nMillisecond should be from 0 to 999 inclusive
        //  1) Time can only be advanced forwards; attempting to set the time backwards
        //     will result in the day advancing and then the time being set to that
        //     specified, e.g. if the current hour is 15 and then the hour is set to 3,
        //     the day will be advanced by 1 and the hour will be set to 3.
        //  2) If values larger than the max hour, minute, second or millisecond are
        //     specified, they will be wrapped around and the overflow will be used to
        //     advance the next field, e.g. specifying 62 hours, 250 minutes, 10 seconds
        //     and 10 milliseconds will result in the calendar day being advanced by 2
        //     and the time being set to 18 hours, 10 minutes, 10 milliseconds.
        public static void SetTime(int nHour, int nMinute, int nSecond, int nMillisecond)
        {
            Internal.StackPushInteger(nMillisecond);
            Internal.StackPushInteger(nSecond);
            Internal.StackPushInteger(nMinute);
            Internal.StackPushInteger(nHour);
            Internal.CallBuiltIn(12);
        }

        //  Get the current calendar year.
        public static int GetCalendarYear()
        {
            Internal.CallBuiltIn(13);
            return Internal.StackPopInteger();
        }

        //  Get the current calendar month.
        public static int GetCalendarMonth()
        {
            Internal.CallBuiltIn(14);
            return Internal.StackPopInteger();
        }

        //  Get the current calendar day.
        public static int GetCalendarDay()
        {
            Internal.CallBuiltIn(15);
            return Internal.StackPopInteger();
        }

        //  Get the current hour.
        public static int GetTimeHour()
        {
            Internal.CallBuiltIn(16);
            return Internal.StackPopInteger();
        }

        //  Get the current minute
        public static int GetTimeMinute()
        {
            Internal.CallBuiltIn(17);
            return Internal.StackPopInteger();
        }

        //  Get the current second
        public static int GetTimeSecond()
        {
            Internal.CallBuiltIn(18);
            return Internal.StackPopInteger();
        }

        //  Get the current millisecond
        public static int GetTimeMillisecond()
        {
            Internal.CallBuiltIn(19);
            return Internal.StackPopInteger();
        }

        //  The action subject will generate a random location near its current location
        //  and pathfind to it.  ActionRandomwalk never ends, which means it is neccessary
        //  to call ClearAllActions in order to allow a creature to perform any other action
        //  once ActionRandomWalk has been called.
        //  * No return value, but if an error occurs the log file will contain
        //    "ActionRandomWalk failed."
        public static void ActionRandomWalk()
        {
            Internal.CallBuiltIn(20);
        }

        //  The action subject will move to lDestination.
        //  - lDestination: The object will move to this location.  If the location is
        //    invalid or a path cannot be found to it, the command does nothing.
        //  - bRun: If this is TRUE, the action subject will run rather than walk
        //  * No return value, but if an error occurs the log file will contain
        //    "MoveToPoint failed."
        public static void ActionMoveToLocation(Location lDestination, bool bRun = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bRun));
            Internal.StackPushLocation(lDestination);
            Internal.CallBuiltIn(21);
        }

        //  Cause the action subject to move to a certain distance from oMoveTo.
        //  If there is no path to oMoveTo, this command will do nothing.
        //  - oMoveTo: This is the object we wish the action subject to move to
        //  - bRun: If this is TRUE, the action subject will run rather than walk
        //  - fRange: This is the desired distance between the action subject and oMoveTo
        //  * No return value, but if an error occurs the log file will contain
        //    "ActionMoveToObject failed."
        public static void ActionMoveToObject(NWGameObject oMoveTo, bool bRun = false, float fRange = 1.0f)
        {
            Internal.StackPushFloat(fRange);
            Internal.StackPushInteger(Convert.ToInt32(bRun));
            Internal.StackPushObject(oMoveTo, false);
            Internal.CallBuiltIn(22);
        }

        //  Cause the action subject to move to a certain distance away from oFleeFrom.
        //  - oFleeFrom: This is the object we wish the action subject to move away from.
        //    If oFleeFrom is not in the same area as the action subject, nothing will
        //    happen.
        //  - bRun: If this is TRUE, the action subject will run rather than walk
        //  - fMoveAwayRange: This is the distance we wish the action subject to put
        //    between themselves and oFleeFrom
        //  * No return value, but if an error occurs the log file will contain
        //    "ActionMoveAwayFromObject failed."
        public static void ActionMoveAwayFromObject(NWGameObject oFleeFrom, bool bRun = false, float fMoveAwayRange = 40.0f)
        {
            Internal.StackPushFloat(fMoveAwayRange);
            Internal.StackPushInteger(Convert.ToInt32(bRun));
            Internal.StackPushObject(oFleeFrom, false);
            Internal.CallBuiltIn(23);
        }

        //  Get the area that oTarget is currently in
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetArea(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(24);
            return Internal.StackPopObject();
        }

        //  The value returned by this function depends on the object type of the caller:
        //  1) If the caller is a door it returns the object that last
        //     triggered it.
        //  2) If the caller is a trigger, area of effect, module, area or encounter it
        //     returns the object that last entered it.
        //  * Return value on error: OBJECT_INVALID
        //   When used for doors, this should only be called from the OnAreaTransitionClick
        //   event.  Otherwise, it should only be called in OnEnter scripts.
        public static NWGameObject GetEnteringObject()
        {
            Internal.CallBuiltIn(25);
            return Internal.StackPopObject();
        }

        //  Get the object that last left the caller.  This function works on triggers,
        //  areas of effect, modules, areas and encounters.
        //  * Return value on error: OBJECT_INVALID
        //  Should only be called in OnExit scripts.
        public static NWGameObject GetExitingObject()
        {
            Internal.CallBuiltIn(26);
            return Internal.StackPopObject();
        }

        //  Get the position of oTarget
        //  * Return value on error: vector (0.0f, 0.0f, 0.0f)
        public static Vector GetPosition(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(27);
            return Internal.StackPopVector();
        }

        //  Get the direction in which oTarget is facing, expressed as a float between
        //  0.0f and 360.0f
        //  * Return value on error: -1.0f
        public static float GetFacing(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(28);
            return Internal.StackPopFloat();
        }

        //  Get the possessor of oItem
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetItemPossessor(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(29);
            return Internal.StackPopObject();
        }

        //  Get the object possessed by oCreature with the tag sItemTag
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetItemPossessedBy(NWGameObject oCreature, string sItemTag)
        {
            Internal.StackPushString(sItemTag);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(30);
            return Internal.StackPopObject();
        }

        //  Create an item with the template sItemTemplate in oTarget's inventory.
        //  - nStackSize: This is the stack size of the item to be created
        //  - sNewTag: If this string is not empty, it will replace the default tag from the template
        //  * Return value: The object that has been created.  On error, this returns
        //    OBJECT_INVALID.
        //  If the item created was merged into an existing stack of similar items,
        //  the function will return the merged stack object. If the merged stack
        //  overflowed, the function will return the overflowed stack that was created.
        public static NWGameObject CreateItemOnObject(string sItemTemplate, NWGameObject oTarget = null, int nStackSize = 1, string sNewTag = "")
        {
            Internal.StackPushString(sNewTag);
            Internal.StackPushInteger(nStackSize);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushString(sItemTemplate);
            Internal.CallBuiltIn(31);
            return Internal.StackPopObject();
        }

        //  Equip oItem into nInventorySlot.
        //  - nInventorySlot: INVENTORY_SLOT_*
        //  * No return value, but if an error occurs the log file will contain
        //    "ActionEquipItem failed."
        // 
        //  Note: 
        //        If the creature already has an item equipped in the slot specified, it will be 
        //        unequipped automatically by the call to ActionEquipItem.
        //      
        //        In order for ActionEquipItem to succeed the creature must be able to equip the
        //        item oItem normally. This means that:
        //        1) The item is in the creature's inventory.
        //        2) The item must already be identified (if magical). 
        //        3) The creature has the level required to equip the item (if magical and ILR is on).
        //        4) The creature possesses the required feats to equip the item (such as weapon proficiencies).
        public static void ActionEquipItem(NWGameObject oItem, int nInventorySlot)
        {
            Internal.StackPushInteger(nInventorySlot);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(32);
        }

        //  Unequip oItem from whatever slot it is currently in.
        public static void ActionUnequipItem(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(33);
        }

        //  Pick up oItem from the ground.
        //  * No return value, but if an error occurs the log file will contain
        //    "ActionPickUpItem failed."
        public static void ActionPickUpItem(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(34);
        }

        //  Put down oItem on the ground.
        //  * No return value, but if an error occurs the log file will contain
        //    "ActionPutDownItem failed."
        public static void ActionPutDownItem(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(35);
        }

        //  Get the last attacker of oAttackee.  This should only be used ONLY in the
        //  OnAttacked events for creatures, placeables and doors.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetLastAttacker(NWGameObject oAttackee = null)
        {
            Internal.StackPushObject(oAttackee, false);
            Internal.CallBuiltIn(36);
            return Internal.StackPopObject();
        }

        //  Attack oAttackee.
        //  - bPassive: If this is TRUE, attack is in passive mode.
        public static void ActionAttack(NWGameObject oAttackee, bool bPassive = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bPassive));
            Internal.StackPushObject(oAttackee, false);
            Internal.CallBuiltIn(37);
        }

        //  Get the creature nearest to oTarget, subject to all the criteria specified.
        //  - nFirstCriteriaType: CREATURE_TYPE_*
        //  - nFirstCriteriaValue:
        //    -> CLASS_TYPE_* if nFirstCriteriaType was CREATURE_TYPE_CLASS
        //    -> SPELL_* if nFirstCriteriaType was CREATURE_TYPE_DOES_NOT_HAVE_SPELL_EFFECT
        //       or CREATURE_TYPE_HAS_SPELL_EFFECT
        //    -> TRUE or FALSE if nFirstCriteriaType was CREATURE_TYPE_IS_ALIVE
        //    -> PERCEPTION_* if nFirstCriteriaType was CREATURE_TYPE_PERCEPTION
        //    -> PLAYER_CHAR_IS_PC or PLAYER_CHAR_NOT_PC if nFirstCriteriaType was
        //       CREATURE_TYPE_PLAYER_CHAR
        //    -> RACIAL_TYPE_* if nFirstCriteriaType was CREATURE_TYPE_RACIAL_TYPE
        //    -> REPUTATION_TYPE_* if nFirstCriteriaType was CREATURE_TYPE_REPUTATION
        //    For example, to get the nearest PC, use:
        //    (CREATURE_TYPE_PLAYER_CHAR, PLAYER_CHAR_IS_PC)
        //  - oTarget: We're trying to find the creature of the specified type that is
        //    nearest to oTarget
        //  - nNth: We don't have to find the first nearest: we can find the Nth nearest...
        //  - nSecondCriteriaType: This is used in the same way as nFirstCriteriaType to
        //    further specify the type of creature that we are looking for.
        //  - nSecondCriteriaValue: This is used in the same way as nFirstCriteriaValue
        //    to further specify the type of creature that we are looking for.
        //  - nThirdCriteriaType: This is used in the same way as nFirstCriteriaType to
        //    further specify the type of creature that we are looking for.
        //  - nThirdCriteriaValue: This is used in the same way as nFirstCriteriaValue to
        //    further specify the type of creature that we are looking for.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetNearestCreature(int nFirstCriteriaType, int nFirstCriteriaValue, NWGameObject oTarget = null, int nNth = 1, int nSecondCriteriaType = -1, int nSecondCriteriaValue = -1, int nThirdCriteriaType = -1, int nThirdCriteriaValue = -1)
        {
            Internal.StackPushInteger(nThirdCriteriaValue);
            Internal.StackPushInteger(nThirdCriteriaType);
            Internal.StackPushInteger(nSecondCriteriaValue);
            Internal.StackPushInteger(nSecondCriteriaType);
            Internal.StackPushInteger(nNth);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger(nFirstCriteriaValue);
            Internal.StackPushInteger(nFirstCriteriaType);
            Internal.CallBuiltIn(38);
            return Internal.StackPopObject();
        }

        //  Add a speak action to the action subject.
        //  - sStringToSpeak: String to be spoken
        //  - nTalkVolume: TALKVOLUME_*
        public static void ActionSpeakString(string sStringToSpeak, TalkVolume nTalkVolume = TalkVolume.Talk)
        {
            Internal.StackPushInteger((int)nTalkVolume);
            Internal.StackPushString(sStringToSpeak);
            Internal.CallBuiltIn(39);
        }

        /// <summary>
        /// Cause the action subject to play an animation
        ///  - nAnimation: ANIMATION_*
        ///  - fSpeed: Speed of the animation
        ///  - fDurationSeconds: Duration of the animation (this is not used for Fire and
        ///    Forget animations)
        /// </summary>
        /// <param name="nAnimation"></param>
        /// <param name="fSpeed"></param>
        public static void ActionPlayAnimation(AnimationFireForget nAnimation, float fSpeed = 1.0f)
        {
            ActionPlayAnimation((int)nAnimation, fSpeed, 0.0f);
        }

        /// <summary>
        /// Cause the action subject to play an animation
        ///  - nAnimation: ANIMATION_*
        ///  - fSpeed: Speed of the animation
        ///  - fDurationSeconds: Duration of the animation (this is not used for Fire and
        ///    Forget animations)
        /// </summary>
        /// <param name="nAnimation"></param>
        /// <param name="fSpeed"></param>
        /// <param name="fDurationSeconds"></param>
        public static void ActionPlayAnimation(AnimationLooping nAnimation, float fSpeed = 1.0f, float fDurationSeconds = 0.0f)
        {
            ActionPlayAnimation((int)nAnimation, fSpeed, fDurationSeconds);
        }

        private static void ActionPlayAnimation(int nAnimation, float fSpeed = 1.0f, float fDurationSeconds = 0.0f)
        {
            Internal.StackPushFloat(fDurationSeconds);
            Internal.StackPushFloat(fSpeed);
            Internal.StackPushInteger((int)nAnimation);
            Internal.CallBuiltIn(40);
        }

        //  Get the distance from the caller to oObject in metres.
        //  * Return value on error: -1.0f
        public static float GetDistanceToObject(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(41);
            return Internal.StackPopFloat();
        }

        //  * Returns TRUE if oObject is a valid object.
        public static bool GetIsObjectValid(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(42);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Cause the action subject to open oDoor
        public static void ActionOpenDoor(NWGameObject oDoor)
        {
            Internal.StackPushObject(oDoor, false);
            Internal.CallBuiltIn(43);
        }

        //  Cause the action subject to close oDoor
        public static void ActionCloseDoor(NWGameObject oDoor)
        {
            Internal.StackPushObject(oDoor, false);
            Internal.CallBuiltIn(44);
        }

        //  Change the direction in which the camera is facing
        //  - fDirection is expressed as anticlockwise degrees from Due East.
        //    (0.0f=East, 90.0f=North, 180.0f=West, 270.0f=South)
        //  A value of -1.0f for any parameter will be ignored and instead it will
        //  use the current camera value.
        //  This can be used to change the way the camera is facing after the player
        //  emerges from an area transition.
        //  - nTransitionType: CAMERA_TRANSITION_TYPE_*  SNAP will immediately move the
        //    camera to the new position, while the other types will result in the camera moving gradually into position
        //  Pitch and distance are limited to valid values for the current camera mode:
        //  Top Down: Distance = 5-20, Pitch = 1-50
        //  Driving camera: Distance = 6 (can't be changed), Pitch = 1-62
        //  Chase: Distance = 5-20, Pitch = 1-50
        //  *** NOTE *** In NWN:Hordes of the Underdark the camera limits have been relaxed to the following:
        //  Distance 1-25
        //  Pitch 1-89
        public static void SetCameraFacing(float fDirection, float fDistance = -1.0f, float fPitch = -1.0f, CameraTransitionType nTransitionType = CameraTransitionType.Snap)
        {
            Internal.StackPushInteger((int)nTransitionType);
            Internal.StackPushFloat(fPitch);
            Internal.StackPushFloat(fDistance);
            Internal.StackPushFloat(fDirection);
            Internal.CallBuiltIn(45);
        }

        //  Play sSoundName
        //  - sSoundName: TBD - SS
        //  This will play a mono sound from the location of the object running the command.
        public static void PlaySound(string sSoundName)
        {
            Internal.StackPushString(sSoundName);
            Internal.CallBuiltIn(46);
        }

        //  Get the object at which the caller last cast a spell
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetSpellTargetObject()
        {
            Internal.CallBuiltIn(47);
            return Internal.StackPopObject();
        }

        //  This action casts a spell at oTarget.
        //  - nSpell: SPELL_*
        //  - oTarget: Target for the spell
        //  - nMetamagic: METAMAGIC_*
        //  - bCheat: If this is TRUE, then the executor of the action doesn't have to be
        //    able to cast the spell.
        //  - nDomainLevel: TBD - SS
        //  - nProjectilePathType: PROJECTILE_PATH_TYPE_*
        //  - bInstantSpell: If this is TRUE, the spell is cast immediately. This allows
        //    the end-user to simulate a high-level magic-user having lots of advance
        //    warning of impending trouble
        public static void ActionCastSpellAtObject(Spell nSpell, NWGameObject oTarget, MetaMagic nMetaMagic = MetaMagic.Any, bool bCheat = false, int nDomainLevel = 0, ProjectilePathType nProjectilePathType = ProjectilePathType.Default, bool bInstantSpell = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bInstantSpell));
            Internal.StackPushInteger((int)nProjectilePathType);
            Internal.StackPushInteger(nDomainLevel);
            Internal.StackPushInteger(Convert.ToInt32(bCheat));
            Internal.StackPushInteger((int)nMetaMagic);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(48);
        }

        //  Get the current hitpoints of oObject
        //  * Return value on error: 0
        public static int GetCurrentHitPoints(NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(49);
            return Internal.StackPopInteger();
        }

        //  Get the maximum hitpoints of oObject
        //  * Return value on error: 0
        public static int GetMaxHitPoints(NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(50);
            return Internal.StackPopInteger();
        }

        //  Get oObject's local integer variable sVarName
        //  * Return value on error: 0
        public static int GetLocalInt(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(51);
            return Internal.StackPopInteger();
        }

        //  Get oObject's local float variable sVarName
        //  * Return value on error: 0.0f
        public static float GetLocalFloat(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(52);
            return Internal.StackPopFloat();
        }

        //  Get oObject's local string variable sVarName
        //  * Return value on error: ""
        public static string GetLocalString(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(53);
            return Internal.StackPopString();
        }

        //  Get oObject's local object variable sVarName
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetLocalObject(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(54);
            return Internal.StackPopObject();
        }

        //  Set oObject's local integer variable sVarName to nValue
        public static void SetLocalInt(NWGameObject oObject, string sVarName, int nValue)
        {
            Internal.StackPushInteger(nValue);
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(55);
        }

        //  Set oObject's local float variable sVarName to nValue
        public static void SetLocalFloat(NWGameObject oObject, string sVarName, float fValue)
        {
            Internal.StackPushFloat(fValue);
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(56);
        }

        //  Set oObject's local string variable sVarName to nValue
        public static void SetLocalString(NWGameObject oObject, string sVarName, string sValue)
        {
            Internal.StackPushString(sValue);
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(57);
        }

        //  Set oObject's local object variable sVarName to nValue
        public static void SetLocalObject(NWGameObject oObject, string sVarName, NWGameObject oValue)
        {
            Internal.StackPushObject(oValue, false);
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(58);
        }

        //  Create a Heal effect. This should be applied as an instantaneous effect.
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nDamageToHeal < 0.
        public static Effect EffectHeal(int nDamageToHeal)
        {
            Internal.StackPushInteger(nDamageToHeal);
            Internal.CallBuiltIn(78);
            return Internal.StackPopEffect();
        }

        //  Create a Damage effect
        //  - nDamageAmount: amount of damage to be dealt. This should be applied as an
        //    instantaneous effect.
        //  - nDamageType: DAMAGE_TYPE_*
        //  - nDamagePower: DAMAGE_POWER_*
        public static Effect EffectDamage(int nDamageAmount, DamageType nDamageType = DamageType.Magical, DamagePower nDamagePower = DamagePower.Normal)
        {
            Internal.StackPushInteger((int)nDamagePower);
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger(nDamageAmount);
            Internal.CallBuiltIn(79);
            return Internal.StackPopEffect();
        }

        //  Create an Ability Increase effect
        //  - bAbilityToIncrease: ABILITY_*
        public static Effect EffectAbilityIncrease(Ability nAbilityToIncrease, int nModifyBy)
        {
            Internal.StackPushInteger(nModifyBy);
            Internal.StackPushInteger((int)nAbilityToIncrease);
            Internal.CallBuiltIn(80);
            return Internal.StackPopEffect();
        }

        //  Create a Damage Resistance effect that removes the first nAmount points of
        //  damage of type nDamageType, up to nLimit (or infinite if nLimit is 0)
        //  - nDamageType: DAMAGE_TYPE_*
        //  - nAmount
        //  - nLimit
        public static Effect EffectDamageResistance(DamageType nDamageType, int nAmount, int nLimit = 0)
        {
            Internal.StackPushInteger(nLimit);
            Internal.StackPushInteger(nAmount);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(81);
            return Internal.StackPopEffect();
        }

        //  Create a Resurrection effect. This should be applied as an instantaneous effect.
        public static Effect EffectResurrection()
        {
            Internal.CallBuiltIn(82);
            return Internal.StackPopEffect();
        }

        //  Create a Summon Creature effect.  The creature is created and placed into the
        //  caller's party/faction.
        //  - sCreatureResref: Identifies the creature to be summoned
        //  - nVisualEffectId: VFX_*
        //  - fDelaySeconds: There can be delay between the visual effect being played, and the
        //    creature being added to the area
        //  - nUseAppearAnimation: should this creature play it's "appear" animation when it is
        //    summoned. If zero, it will just fade in somewhere near the target.  If the value is 1
        //    it will use the appear animation, and if it's 2 it will use appear2 (which doesn't exist for most creatures)
        public static Effect EffectSummonCreature(string sCreatureResref, Vfx nVisualEffectId = Vfx.None, float fDelaySeconds = 0.0f, int nUseAppearAnimation = 0)
        {
            Internal.StackPushInteger(nUseAppearAnimation);
            Internal.StackPushFloat(fDelaySeconds);
            Internal.StackPushInteger((int)nVisualEffectId);
            Internal.StackPushString(sCreatureResref);
            Internal.CallBuiltIn(83);
            return Internal.StackPopEffect();
        }

        //  Get the level at which this creature cast it's last spell (or spell-like ability)
        //  * Return value on error, or if oCreature has not yet cast a spell: 0;
        public static int GetCasterLevel(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(84);
            return Internal.StackPopInteger();
        }

        //  Get the first in-game effect on oCreature.
        public static Effect GetFirstEffect(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(85);
            return Internal.StackPopEffect();
        }

        //  Get the next in-game effect on oCreature.
        public static Effect GetNextEffect(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(86);
            return Internal.StackPopEffect();
        }

        //  Remove eEffect from oCreature.
        //  * No return value
        public static void RemoveEffect(NWGameObject oCreature, Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(87);
        }

        //  * Returns TRUE if eEffect is a valid effect. The effect must have been applied to
        //  * an object or else it will return FALSE
        public static bool GetIsEffectValid(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(88);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the duration type (DURATION_TYPE_*) of eEffect.
        //  * Return value if eEffect is not valid: -1
        public static int GetEffectDurationType(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(89);
            return Internal.StackPopInteger();
        }

        //  Get the subtype (SUBTYPE_*) of eEffect.
        //  * Return value on error: 0
        public static int GetEffectSubType(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(90);
            return Internal.StackPopInteger();
        }

        //  Get the object that created eEffect.
        //  * Returns OBJECT_INVALID if eEffect is not a valid effect.
        public static NWGameObject GetEffectCreator(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(91);
            return Internal.StackPopObject();
        }

        //  Get the first object in oArea.
        //  If no valid area is specified, it will use the caller's area.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetFirstObjectInArea(NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(93);
            return Internal.StackPopObject();
        }

        //  Get the next object in oArea.
        //  If no valid area is specified, it will use the caller's area.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetNextObjectInArea(NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(94);
            return Internal.StackPopObject();
        }

        //  Get the total from rolling (nNumDice x d2 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d2(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(95);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d3 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d3(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(96);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d4 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d4(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(97);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d6 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d6(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(98);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d8 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d8(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(99);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d10 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d10(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(100);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d12 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d12(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(101);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d20 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d20(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(102);
            return Internal.StackPopInteger();
        }

        //  Get the total from rolling (nNumDice x d100 dice).
        //  - nNumDice: If this is less than 1, the value 1 will be used.
        public static int d100(int nNumDice = 1)
        {
            Internal.StackPushInteger(nNumDice);
            Internal.CallBuiltIn(103);
            return Internal.StackPopInteger();
        }

        //  Get the magnitude of vVector; this can be used to determine the
        //  distance between two points.
        //  * Return value on error: 0.0f
        public static float VectorMagnitude(Vector? vVector)
        {
            Internal.StackPushVector(vVector);
            Internal.CallBuiltIn(104);
            return Internal.StackPopFloat();
        }

        //  Get the metamagic type (METAMAGIC_*) of the last spell cast by the caller
        //  * Return value if the caster is not a valid object: -1
        public static MetaMagic GetMetaMagicFeat()
        {
            Internal.CallBuiltIn(105);
            return (MetaMagic)Internal.StackPopInteger();
        }

        //  Get the object type (OBJECT_TYPE_*) of oTarget
        //  * Return value if oTarget is not a valid object: -1
        public static ObjectType GetObjectType(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(106);
            return (ObjectType)Internal.StackPopInteger();
        }

        //  Get the racial type (RACIAL_TYPE_*) of oCreature
        //  * Return value if oCreature is not a valid creature: RACIAL_TYPE_INVALID
        public static RacialType GetRacialType(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(107);
            return (RacialType)Internal.StackPopInteger();
        }

        //  Do a Fortitude Save check for the given DC
        //  - oCreature
        //  - nDC: Difficulty check
        //  - nSaveType: SAVING_THROW_TYPE_*
        //  - oSaveVersus
        //  Returns: 0 if the saving throw roll failed
        //  Returns: 1 if the saving throw roll succeeded
        //  Returns: 2 if the target was immune to the save type specified
        //  Note: If used within an Area of Effect Object Script (On Enter, OnExit, OnHeartbeat), you MUST pass
        //  GetAreaOfEffectCreator() into oSaveVersus!!
        public static int FortitudeSave(NWGameObject oCreature, int nDC, SavingThrowType nSaveType = SavingThrowType.None, NWGameObject oSaveVersus = null)
        {
            Internal.StackPushObject(oSaveVersus, false);
            Internal.StackPushInteger((int)nSaveType);
            Internal.StackPushInteger(nDC);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(108);
            return Internal.StackPopInteger();
        }

        /// <summary>
        ///   Does a Reflex Save check for the given DC
        ///  - oCreature
        ///  - nDC: Difficulty check
        ///  - nSaveType: SAVING_THROW_TYPE_*
        ///  - oSaveVersus
        ///  Returns: 0 if the saving throw roll failed
        ///  Returns: 1 if the saving throw roll succeeded
        ///  Returns: 2 if the target was immune to the save type specified
        ///  Note: If used within an Area of Effect Object Script (On Enter, OnExit, OnHeartbeat), you MUST pass
        ///  GetAreaOfEffectCreator() into oSaveVersus!!
        /// </summary>
        /// <param name="oCreature">The creature being checked</param>
        /// <param name="nDC">The difficulty check</param>
        /// <param name="nSaveType">The type of saving throw</param>
        /// <param name="oSaveVersus">The creature being used for the check</param>
        /// <returns>A reflex result containing the status of the call</returns>
        public static int ReflexSave(NWGameObject oCreature, int nDC, SavingThrowType nSaveType = SavingThrowType.None, NWGameObject oSaveVersus = null)
        {
            Internal.StackPushObject(oSaveVersus, false);
            Internal.StackPushInteger((int)nSaveType);
            Internal.StackPushInteger(nDC);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(109);
            return Internal.StackPopInteger();
        }

        //  Does a Will Save check for the given DC
        //  - oCreature
        //  - nDC: Difficulty check
        //  - nSaveType: SAVING_THROW_TYPE_*
        //  - oSaveVersus
        //  Returns: 0 if the saving throw roll failed
        //  Returns: 1 if the saving throw roll succeeded
        //  Returns: 2 if the target was immune to the save type specified
        //  Note: If used within an Area of Effect Object Script (On Enter, OnExit, OnHeartbeat), you MUST pass
        //  GetAreaOfEffectCreator() into oSaveVersus!!
        public static int WillSave(NWGameObject oCreature, int nDC, SavingThrowType nSaveType = SavingThrowType.None, NWGameObject oSaveVersus = null)
        {
            Internal.StackPushObject(oSaveVersus, false);
            Internal.StackPushInteger((int)nSaveType);
            Internal.StackPushInteger(nDC);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(110);
            return Internal.StackPopInteger();
        }

        //  Get the DC to save against for a spell (10 + spell level + relevant ability
        //  bonus).  This can be called by a creature or by an Area of Effect object.
        public static int GetSpellSaveDC()
        {
            Internal.CallBuiltIn(111);
            return Internal.StackPopInteger();
        }

        //  Set the subtype of eEffect to Magical and return eEffect.
        //  (Effects default to magical if the subtype is not set)
        //  Magical effects are removed by resting, and by dispel magic
        public static Effect MagicalEffect(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(112);
            return Internal.StackPopEffect();
        }

        //  Set the subtype of eEffect to Supernatural and return eEffect.
        //  (Effects default to magical if the subtype is not set)
        //  Permanent supernatural effects are not removed by resting
        public static Effect SupernaturalEffect(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(113);
            return Internal.StackPopEffect();
        }

        //  Set the subtype of eEffect to Extraordinary and return eEffect.
        //  (Effects default to magical if the subtype is not set)
        //  Extraordinary effects are removed by resting, but not by dispel magic
        public static Effect ExtraordinaryEffect(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(114);
            return Internal.StackPopEffect();
        }

        //  Create an AC Increase effect
        //  - nValue: size of AC increase
        //  - nModifyType: AC_*_BONUS
        //  - nDamageType: DAMAGE_TYPE_*
        //    * Default value for nDamageType should only ever be used in this function prototype.
        public static Effect EffectACIncrease(int nValue, AC nModifyType = AC.DodgeBonus, DamageType nDamageType = DamageType.ACVsDamageTypeAll)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger((int)nModifyType);
            Internal.StackPushInteger(nValue);
            Internal.CallBuiltIn(115);
            return Internal.StackPopEffect();
        }

        //  If oObject is a creature, this will return that creature's armour class
        //  If oObject is an item, door or placeable, this will return zero.
        //  - nForFutureUse: this parameter is not currently used
        //  * Return value if oObject is not a creature, item, door or placeable: -1
        public static int GetAC(NWGameObject oObject, int nForFutureUse = 0)
        {
            Internal.StackPushInteger(nForFutureUse);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(116);
            return Internal.StackPopInteger();
        }

        //  Create a Saving Throw Increase effect
        //  - nSave: SAVING_THROW_* (not SAVING_THROW_TYPE_*)
        //           SAVING_THROW_ALL
        //           SAVING_THROW_FORT
        //           SAVING_THROW_REFLEX
        //           SAVING_THROW_WILL 
        //  - nValue: size of the Saving Throw increase
        //  - nSaveType: SAVING_THROW_TYPE_* (e.g. SAVING_THROW_TYPE_ACID )
        public static Effect EffectSavingThrowIncrease(SavingThrow nSave, int nValue, SavingThrowType nSaveType = SavingThrowType.All)
        {
            Internal.StackPushInteger((int)nSaveType);
            Internal.StackPushInteger(nValue);
            Internal.StackPushInteger((int)nSave);
            Internal.CallBuiltIn(117);
            return Internal.StackPopEffect();
        }

        //  Create an Attack Increase effect
        //  - nBonus: size of attack bonus
        //  - nModifierType: ATTACK_BONUS_*
        public static Effect EffectAttackIncrease(int nBonus, AttackBonus nModifierType = AttackBonus.Misc)
        {
            Internal.StackPushInteger((int)nModifierType);
            Internal.StackPushInteger(nBonus);
            Internal.CallBuiltIn(118);
            return Internal.StackPopEffect();
        }

        //  Create a Damage Reduction effect
        //  - nAmount: amount of damage reduction
        //  - nDamagePower: DAMAGE_POWER_*
        //  - nLimit: How much damage the effect can absorb before disappearing.
        //    Set to zero for infinite
        public static Effect EffectDamageReduction(int nAmount, DamagePower nDamagePower, int nLimit = 0)
        {
            Internal.StackPushInteger(nLimit);
            Internal.StackPushInteger((int)nDamagePower);
            Internal.StackPushInteger(nAmount);
            Internal.CallBuiltIn(119);
            return Internal.StackPopEffect();
        }

        //  Create a Damage Increase effect
        //  - nBonus: DAMAGE_BONUS_*
        //  - nDamageType: DAMAGE_TYPE_*
        //  NOTE! You *must* use the DAMAGE_BONUS_* constants! Using other values may
        //        result in odd behaviour.
        public static Effect EffectDamageIncrease(int nBonus, DamageType nDamageType = DamageType.Magical)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger(nBonus);
            Internal.CallBuiltIn(120);
            return Internal.StackPopEffect();
        }

        //  Convert nRounds into a number of seconds
        //  A round is always 6.0 seconds
        public static float RoundsToSeconds(int nRounds)
        {
            Internal.StackPushInteger(nRounds);
            Internal.CallBuiltIn(121);
            return Internal.StackPopFloat();
        }

        //  Convert nHours into a number of seconds
        //  The result will depend on how many minutes there are per hour (game-time)
        public static float HoursToSeconds(int nHours)
        {
            Internal.StackPushInteger(nHours);
            Internal.CallBuiltIn(122);
            return Internal.StackPopFloat();
        }

        //  Convert nTurns into a number of seconds
        //  A turn is always 60.0 seconds
        public static float TurnsToSeconds(int nTurns)
        {
            Internal.StackPushInteger(nTurns);
            Internal.CallBuiltIn(123);
            return Internal.StackPopFloat();
        }

        //  Get an integer between 0 and 100 (inclusive) to represent oCreature's
        //  Law/Chaos alignment
        //  (100=law, 0=chaos)
        //  * Return value if oCreature is not a valid creature: -1
        public static int GetLawChaosValue(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(124);
            return Internal.StackPopInteger();
        }

        //  Get an integer between 0 and 100 (inclusive) to represent oCreature's
        //  Good/Evil alignment
        //  (100=good, 0=evil)
        //  * Return value if oCreature is not a valid creature: -1
        public static int GetGoodEvilValue(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(125);
            return Internal.StackPopInteger();
        }

        //  Return an ALIGNMENT_* constant to represent oCreature's law/chaos alignment
        //  * Return value if oCreature is not a valid creature: -1
        public static Alignment GetAlignmentLawChaos(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(126);
            return (Alignment)Internal.StackPopInteger();
        }

        //  Return an ALIGNMENT_* constant to represent oCreature's good/evil alignment
        //  * Return value if oCreature is not a valid creature: -1
        public static Alignment GetAlignmentGoodEvil(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(127);
            return (Alignment)Internal.StackPopInteger();
        }

        //  Get the first object in nShape
        //  - nShape: SHAPE_*
        //  - fSize:
        //    -> If nShape == SHAPE_SPHERE, this is the radius of the sphere
        //    -> If nShape == SHAPE_SPELLCYLINDER, this is the length of the cylinder
        //       Spell Cylinder's always have a radius of 1.5m.
        //    -> If nShape == SHAPE_CONE, this is the widest radius of the cone
        //    -> If nShape == SHAPE_SPELLCONE, this is the length of the cone in the
        //       direction of lTarget. Spell cones are always 60 degrees with the origin
        //       at OBJECT_SELF.
        //    -> If nShape == SHAPE_CUBE, this is half the length of one of the sides of
        //       the cube
        //  - lTarget: This is the centre of the effect, usually GetSpellTargetLocation(),
        //    or the end of a cylinder or cone.
        //  - bLineOfSight: This controls whether to do a line-of-sight check on the
        //    object returned. Line of sight check is done from origin to target object
        //    at a height 1m above the ground
        //    (This can be used to ensure that spell effects do not go through walls.)
        //  - nObjectFilter: This allows you to filter out undesired object types, using
        //    bitwise "or".
        //    For example, to return only creatures and doors, the value for this
        //    parameter would be OBJECT_TYPE_CREATURE | OBJECT_TYPE_DOOR
        //  - vOrigin: This is only used for cylinders and cones, and specifies the
        //    origin of the effect(normally the spell-caster's position).
        //  Return value on error: OBJECT_INVALID
        public static NWGameObject GetFirstObjectInShape(Shape nShape, float fSize, Location lTarget, bool bLineOfSight = false, ObjectType nObjectFilter = ObjectType.Creature, Vector? vOrigin = null)
        {
            Internal.StackPushVector(vOrigin);
            Internal.StackPushInteger((int)nObjectFilter);
            Internal.StackPushInteger(Convert.ToInt32(bLineOfSight));
            Internal.StackPushLocation(lTarget);
            Internal.StackPushFloat(fSize);
            Internal.StackPushInteger((int)nShape);
            Internal.CallBuiltIn(128);
            return Internal.StackPopObject();
        }

        //  Get the next object in nShape
        //  - nShape: SHAPE_*
        //  - fSize:
        //    -> If nShape == SHAPE_SPHERE, this is the radius of the sphere
        //    -> If nShape == SHAPE_SPELLCYLINDER, this is the length of the cylinder.
        //       Spell Cylinder's always have a radius of 1.5m.
        //    -> If nShape == SHAPE_CONE, this is the widest radius of the cone
        //    -> If nShape == SHAPE_SPELLCONE, this is the length of the cone in the
        //       direction of lTarget. Spell cones are always 60 degrees with the origin
        //       at OBJECT_SELF.
        //    -> If nShape == SHAPE_CUBE, this is half the length of one of the sides of
        //       the cube
        //  - lTarget: This is the centre of the effect, usually GetSpellTargetLocation(),
        //    or the end of a cylinder or cone.
        //  - bLineOfSight: This controls whether to do a line-of-sight check on the
        //    object returned. (This can be used to ensure that spell effects do not go
        //    through walls.) Line of sight check is done from origin to target object
        //    at a height 1m above the ground
        //  - nObjectFilter: This allows you to filter out undesired object types, using
        //    bitwise "or". For example, to return only creatures and doors, the value for
        //    this parameter would be OBJECT_TYPE_CREATURE | OBJECT_TYPE_DOOR
        //  - vOrigin: This is only used for cylinders and cones, and specifies the origin
        //    of the effect (normally the spell-caster's position).
        //  Return value on error: OBJECT_INVALID
        public static NWGameObject GetNextObjectInShape(Shape nShape, float fSize, Location lTarget, bool bLineOfSight = false, ObjectType nObjectFilter = ObjectType.Creature, Vector? vOrigin = null)
        {
            Internal.StackPushVector(vOrigin);
            Internal.StackPushInteger((int)nObjectFilter);
            Internal.StackPushInteger(Convert.ToInt32(bLineOfSight));
            Internal.StackPushLocation(lTarget);
            Internal.StackPushFloat(fSize);
            Internal.StackPushInteger((int)nShape);
            Internal.CallBuiltIn(129);
            return Internal.StackPopObject();
        }

        //  Create an Entangle effect
        //  When applied, this effect will restrict the creature's movement and apply a
        //  (-2) to all attacks and a -4 to AC.
        public static Effect EffectEntangle()
        {
            Internal.CallBuiltIn(130);
            return Internal.StackPopEffect();
        }

        //  Causes object oObject to run the event evToRun. The script on the object that is
        //  associated with the event specified will run.
        //  Events can be created using the following event functions:
        //     EventActivateItem() - This creates an OnActivateItem module event. The script for handling
        //                           this event can be set in Module Properties on the Event Tab.
        //     EventConversation() - This creates on OnConversation creature event. The script for handling
        //                           this event can be set by viewing the Creature Properties on a 
        //                           creature and then clicking on the Scripts Tab.
        //     EventSpellCastAt()  - This creates an OnSpellCastAt event. The script for handling this
        //                           event can be set in the Scripts Tab of the Properties menu 
        //                           for the object.
        //     EventUserDefined()  - This creates on OnUserDefined event. The script for handling this event
        //                           can be set in the Scripts Tab of the Properties menu for the object/area/module.
        public static void SignalEvent(NWGameObject oObject, Event evToRun)
        {
            Internal.StackPushEvent(evToRun);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(131);
        }

        //  Create an event of the type nUserDefinedEventNumber
        //  Note: This only creates the event. The event wont actually trigger until SignalEvent()
        //  is called using this created UserDefined event as an argument.
        //  For example:
        //      SignalEvent(oObject, EventUserDefined(9999));
        //  Once the event has been signaled. The script associated with the OnUserDefined event will
        //  run on the object oObject.
        // 
        //  To specify the OnUserDefined script that should run, view the object's Properties
        //  and click on the Scripts Tab. Then specify a script for the OnUserDefined event.
        //  From inside the OnUserDefined script call:
        //     GetUserDefinedEventNumber() to retrieve the value of nUserDefinedEventNumber
        //     that was used when the event was signaled.
        public static Event EventUserDefined(int nUserDefinedEventNumber)
        {
            Internal.StackPushInteger(nUserDefinedEventNumber);
            Internal.CallBuiltIn(132);
            return Internal.StackPopEvent();
        }

        //  Create a Death effect
        //  - nSpectacularDeath: if this is TRUE, the creature to which this effect is
        //    applied will die in an extraordinary fashion
        //  - nDisplayFeedback
        public static Effect EffectDeath(bool nSpectacularDeath = false, bool nDisplayFeedback = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nDisplayFeedback));
            Internal.StackPushInteger(Convert.ToInt32(nSpectacularDeath));
            Internal.CallBuiltIn(133);
            return Internal.StackPopEffect();
        }

        //  Create a Knockdown effect
        //  This effect knocks creatures off their feet, they will sit until the effect
        //  is removed. This should be applied as a temporary effect with a 3 second
        //  duration minimum (1 second to fall, 1 second sitting, 1 second to get up).
        public static Effect EffectKnockdown()
        {
            Internal.CallBuiltIn(134);
            return Internal.StackPopEffect();
        }

        //  Give oItem to oGiveTo
        //  If oItem is not a valid item, or oGiveTo is not a valid object, nothing will
        //  happen.
        public static void ActionGiveItem(NWGameObject oItem, NWGameObject oGiveTo)
        {
            Internal.StackPushObject(oGiveTo, false);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(135);
        }

        //  Take oItem from oTakeFrom
        //  If oItem is not a valid item, or oTakeFrom is not a valid object, nothing
        //  will happen.
        public static void ActionTakeItem(NWGameObject oItem, NWGameObject oTakeFrom)
        {
            Internal.StackPushObject(oTakeFrom, false);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(136);
        }

        //  Normalize vVector
        public static Vector VectorNormalize(Vector? vVector)
        {
            Internal.StackPushVector(vVector);
            Internal.CallBuiltIn(137);
            return Internal.StackPopVector();
        }

        //  Create a Curse effect.
        //  - nStrMod: strength modifier
        //  - nDexMod: dexterity modifier
        //  - nConMod: constitution modifier
        //  - nIntMod: intelligence modifier
        //  - nWisMod: wisdom modifier
        //  - nChaMod: charisma modifier
        public static Effect EffectCurse(int nStrMod = 1, int nDexMod = 1, int nConMod = 1, int nIntMod = 1, int nWisMod = 1, int nChaMod = 1)
        {
            Internal.StackPushInteger(nChaMod);
            Internal.StackPushInteger(nWisMod);
            Internal.StackPushInteger(nIntMod);
            Internal.StackPushInteger(nConMod);
            Internal.StackPushInteger(nDexMod);
            Internal.StackPushInteger(nStrMod);
            Internal.CallBuiltIn(138);
            return Internal.StackPopEffect();
        }

        //  Get the ability score of type nAbility for a creature (otherwise 0)
        //  - oCreature: the creature whose ability score we wish to find out
        //  - nAbilityType: ABILITY_*
        //  - nBaseAbilityScore: if set to true will return the base ability score without
        //                       bonuses (e.g. ability bonuses granted from equipped items).
        //  Return value on error: 0
        public static int GetAbilityScore(NWGameObject oCreature, Ability nAbilityType, bool nBaseAbilityScore = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nBaseAbilityScore));
            Internal.StackPushInteger((int)nAbilityType);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(139);
            return Internal.StackPopInteger();
        }

        //  * Returns TRUE if oCreature is a dead NPC, dead PC or a dying PC.
        public static bool GetIsDead(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(140);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Create a vector with the specified values for x, y and z
        public static Vector Vector(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            Internal.StackPushFloat(z);
            Internal.StackPushFloat(y);
            Internal.StackPushFloat(x);
            Internal.CallBuiltIn(142);
            return Internal.StackPopVector();
        }

        //  Cause the caller to face vTarget
        public static void SetFacingPoint(Vector? vTarget)
        {
            Internal.StackPushVector(vTarget);
            Internal.CallBuiltIn(143);
        }

        //  Convert fAngle to a vector
        public static Vector AngleToVector(float fAngle)
        {
            Internal.StackPushFloat(fAngle);
            Internal.CallBuiltIn(144);
            return Internal.StackPopVector();
        }

        //  Convert vVector to an angle
        public static float VectorToAngle(Vector? vVector)
        {
            Internal.StackPushVector(vVector);
            Internal.CallBuiltIn(145);
            return Internal.StackPopFloat();
        }

        //  The caller will perform a Melee Touch Attack on oTarget
        //  This is not an action, and it assumes the caller is already within range of
        //  oTarget
        //  * Returns 0 on a miss, 1 on a hit and 2 on a critical hit
        public static int TouchAttackMelee(NWGameObject oTarget, bool bDisplayFeedback = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bDisplayFeedback));
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(146);
            return Internal.StackPopInteger();
        }

        //  The caller will perform a Ranged Touch Attack on oTarget
        //  * Returns 0 on a miss, 1 on a hit and 2 on a critical hit
        public static int TouchAttackRanged(NWGameObject oTarget, bool bDisplayFeedback = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bDisplayFeedback));
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(147);
            return Internal.StackPopInteger();
        }

        //  Create a Paralyze effect
        public static Effect EffectParalyze()
        {
            Internal.CallBuiltIn(148);
            return Internal.StackPopEffect();
        }

        //  Create a Spell Immunity effect.
        //  There is a known bug with this function. There *must* be a parameter specified
        //  when this is called (even if the desired parameter is SPELL_ALL_SPELLS),
        //  otherwise an effect of type EFFECT_TYPE_INVALIDEFFECT will be returned.
        //  - nImmunityToSpell: SPELL_*
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nImmunityToSpell is
        //    invalid.
        public static Effect EffectSpellImmunity(Spell nImmunityToSpell = Spell.AllSpells)
        {
            Internal.StackPushInteger((int)nImmunityToSpell);
            Internal.CallBuiltIn(149);
            return Internal.StackPopEffect();
        }

        //  Create a Deaf effect
        public static Effect EffectDeaf()
        {
            Internal.CallBuiltIn(150);
            return Internal.StackPopEffect();
        }

        //  Get the distance in metres between oObjectA and oObjectB.
        //  * Return value if either object is invalid: 0.0f
        public static float GetDistanceBetween(NWGameObject oObjectA, NWGameObject oObjectB)
        {
            Internal.StackPushObject(oObjectB, false);
            Internal.StackPushObject(oObjectA, false);
            Internal.CallBuiltIn(151);
            return Internal.StackPopFloat();
        }

        //  Set oObject's local location variable sVarname to lValue
        public static void SetLocalLocation(NWGameObject oObject, string sVarName, Location lValue)
        {
            Internal.StackPushLocation(lValue);
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(152);
        }

        //  Get oObject's local location variable sVarname
        public static Location GetLocalLocation(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(153);
            return Internal.StackPopLocation();
        }

        //  Create a Sleep effect
        public static Effect EffectSleep()
        {
            Internal.CallBuiltIn(154);
            return Internal.StackPopEffect();
        }

        //  Get the object which is in oCreature's specified inventory slot
        //  - nInventorySlot: INVENTORY_SLOT_*
        //  - oCreature
        //  * Returns OBJECT_INVALID if oCreature is not a valid creature or there is no
        //    item in nInventorySlot.
        public static NWGameObject GetItemInSlot(InventorySlot nInventorySlot, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nInventorySlot);
            Internal.CallBuiltIn(155);
            return Internal.StackPopObject();
        }

        //  Create a Charm effect
        public static Effect EffectCharmed()
        {
            Internal.CallBuiltIn(156);
            return Internal.StackPopEffect();
        }

        //  Create a Confuse effect
        public static Effect EffectConfused()
        {
            Internal.CallBuiltIn(157);
            return Internal.StackPopEffect();
        }

        //  Create a Frighten effect
        public static Effect EffectFrightened()
        {
            Internal.CallBuiltIn(158);
            return Internal.StackPopEffect();
        }

        //  Create a Dominate effect
        public static Effect EffectDominated()
        {
            Internal.CallBuiltIn(159);
            return Internal.StackPopEffect();
        }

        //  Create a Daze effect
        public static Effect EffectDazed()
        {
            Internal.CallBuiltIn(160);
            return Internal.StackPopEffect();
        }

        //  Create a Stun effect
        public static Effect EffectStunned()
        {
            Internal.CallBuiltIn(161);
            return Internal.StackPopEffect();
        }

        //  Set whether oTarget's action stack can be modified
        public static void SetCommandable(bool bCommandable, NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger(Convert.ToInt32(bCommandable));
            Internal.CallBuiltIn(162);
        }

        //  Determine whether oTarget's action stack can be modified.
        public static bool GetCommandable(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(163);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Create a Regenerate effect.
        //  - nAmount: amount of damage to be regenerated per time interval
        //  - fIntervalSeconds: length of interval in seconds
        public static Effect EffectRegenerate(int nAmount, float fIntervalSeconds)
        {
            Internal.StackPushFloat(fIntervalSeconds);
            Internal.StackPushInteger(nAmount);
            Internal.CallBuiltIn(164);
            return Internal.StackPopEffect();
        }

        //  Create a Movement Speed Increase effect.
        //  - nPercentChange - range 0 through 99
        //  eg.
        //     0 = no change in speed
        //    50 = 50% faster
        //    99 = almost twice as fast
        public static Effect EffectMovementSpeedIncrease(int nPercentChange)
        {
            Internal.StackPushInteger(nPercentChange);
            Internal.CallBuiltIn(165);
            return Internal.StackPopEffect();
        }

        //  Get the number of hitdice for oCreature.
        //  * Return value if oCreature is not a valid creature: 0
        public static int GetHitDice(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(166);
            return Internal.StackPopInteger();
        }

        //  The action subject will follow oFollow until a ClearAllActions() is called.
        //  - oFollow: this is the object to be followed
        //  - fFollowDistance: follow distance in metres
        //  * No return value
        public static void ActionForceFollowObject(NWGameObject oFollow, float fFollowDistance = 0.0f)
        {
            Internal.StackPushFloat(fFollowDistance);
            Internal.StackPushObject(oFollow, false);
            Internal.CallBuiltIn(167);
        }

        //  Get the Tag of oObject
        //  * Return value if oObject is not a valid object: ""
        public static string GetTag(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(168);
            return Internal.StackPopString();
        }

        //  Do a Spell Resistance check between oCaster and oTarget, returning TRUE if
        //  the spell was resisted.
        //  * Return value if oCaster or oTarget is an invalid object: FALSE
        //  * Return value if spell cast is not a player spell: - 1
        //  * Return value if spell resisted: 1
        //  * Return value if spell resisted via magic immunity: 2
        //  * Return value if spell resisted via spell absorption: 3
        public static int ResistSpell(NWGameObject oCaster, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oCaster, false);
            Internal.CallBuiltIn(169);
            return Internal.StackPopInteger();
        }

        //  Get the effect type (EFFECT_TYPE_*) of eEffect.
        //  * Return value if eEffect is invalid: EFFECT_INVALIDEFFECT
        public static int GetEffectType(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(170);
            return Internal.StackPopInteger();
        }

        //  Create an Area Of Effect effect in the area of the creature it is applied to.
        //  If the scripts are not specified, default ones will be used.
        public static Effect EffectAreaOfEffect(int nAreaEffectId, string sOnEnterScript = "", string sHeartbeatScript = "", string sOnExitScript = "")
        {
            Internal.StackPushString(sOnExitScript);
            Internal.StackPushString(sHeartbeatScript);
            Internal.StackPushString(sOnEnterScript);
            Internal.StackPushInteger(nAreaEffectId);
            Internal.CallBuiltIn(171);
            return Internal.StackPopEffect();
        }

        //  * Returns TRUE if the Faction Ids of the two objects are the same
        public static bool GetFactionEqual(NWGameObject oFirstObject, NWGameObject oSecondObject = null)
        {
            Internal.StackPushObject(oSecondObject, false);
            Internal.StackPushObject(oFirstObject, false);
            Internal.CallBuiltIn(172);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Make oObjectToChangeFaction join the faction of oMemberOfFactionToJoin.
        //  NB. ** This will only work for two NPCs **
        public static void ChangeFaction(NWGameObject oObjectToChangeFaction, NWGameObject oMemberOfFactionToJoin)
        {
            Internal.StackPushObject(oMemberOfFactionToJoin, false);
            Internal.StackPushObject(oObjectToChangeFaction, false);
            Internal.CallBuiltIn(173);
        }

        //  * Returns TRUE if oObject is listening for something
        public static bool GetIsListening(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(174);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Set whether oObject is listening.
        public static void SetListening(NWGameObject oObject, bool bValue)
        {
            Internal.StackPushInteger(Convert.ToInt32(bValue));
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(175);
        }

        //  Set the string for oObject to listen for.
        //  Note: this does not set oObject to be listening.
        public static void SetListenPattern(NWGameObject oObject, string sPattern, int nNumber = 0)
        {
            Internal.StackPushInteger(nNumber);
            Internal.StackPushString(sPattern);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(176);
        }

        //  * Returns TRUE if sStringToTest matches sPattern.
        public static bool TestStringAgainstPattern(string sPattern, string sStringToTest)
        {
            Internal.StackPushString(sStringToTest);
            Internal.StackPushString(sPattern);
            Internal.CallBuiltIn(177);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the appropriate matched string (this should only be used in
        //  OnConversation scripts).
        //  * Returns the appropriate matched string, otherwise returns ""
        public static string GetMatchedSubstring(int nString)
        {
            Internal.StackPushInteger(nString);
            Internal.CallBuiltIn(178);
            return Internal.StackPopString();
        }

        //  Get the number of string parameters available.
        //  * Returns -1 if no string matched (this could be because of a dialogue event)
        public static int GetMatchedSubstringsCount()
        {
            Internal.CallBuiltIn(179);
            return Internal.StackPopInteger();
        }

        //  * Create a Visual Effect that can be applied to an object.
        //  - nVisualEffectId
        //  - nMissEffect: if this is TRUE, a random vector near or past the target will
        //    be generated, on which to play the effect
        public static Effect EffectVisualEffect(Vfx nVisualEffectId, bool nMissEffect = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nMissEffect));
            Internal.StackPushInteger((int)nVisualEffectId);
            Internal.CallBuiltIn(180);
            return Internal.StackPopEffect();
        }

        //  Get the weakest member of oFactionMember's faction.
        //  * Returns OBJECT_INVALID if oFactionMember's faction is invalid.
        public static NWGameObject GetFactionWeakestMember(NWGameObject oFactionMember = null, bool bMustBeVisible = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMustBeVisible));
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(181);
            return Internal.StackPopObject();
        }

        //  Get the strongest member of oFactionMember's faction.
        //  * Returns OBJECT_INVALID if oFactionMember's faction is invalid.
        public static NWGameObject GetFactionStrongestMember(NWGameObject oFactionMember = null, bool bMustBeVisible = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMustBeVisible));
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(182);
            return Internal.StackPopObject();
        }

        //  Get the member of oFactionMember's faction that has taken the most hit points
        //  of damage.
        //  * Returns OBJECT_INVALID if oFactionMember's faction is invalid.
        public static NWGameObject GetFactionMostDamagedMember(NWGameObject oFactionMember = null, bool bMustBeVisible = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMustBeVisible));
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(183);
            return Internal.StackPopObject();
        }

        //  Get the member of oFactionMember's faction that has taken the fewest hit
        //  points of damage.
        //  * Returns OBJECT_INVALID if oFactionMember's faction is invalid.
        public static NWGameObject GetFactionLeastDamagedMember(NWGameObject oFactionMember = null, bool bMustBeVisible = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMustBeVisible));
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(184);
            return Internal.StackPopObject();
        }

        //  Get the amount of gold held by oFactionMember's faction.
        //  * Returns -1 if oFactionMember's faction is invalid.
        public static int GetFactionGold(NWGameObject oFactionMember)
        {
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(185);
            return Internal.StackPopInteger();
        }

        //  Get an integer between 0 and 100 (inclusive) that represents how
        //  oSourceFactionMember's faction feels about oTarget.
        //  * Return value on error: -1
        public static int GetFactionAverageReputation(NWGameObject oSourceFactionMember, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oSourceFactionMember, false);
            Internal.CallBuiltIn(186);
            return Internal.StackPopInteger();
        }

        //  Get an integer between 0 and 100 (inclusive) that represents the average
        //  good/evil alignment of oFactionMember's faction.
        //  * Return value on error: -1
        public static int GetFactionAverageGoodEvilAlignment(NWGameObject oFactionMember)
        {
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(187);
            return Internal.StackPopInteger();
        }

        //  Get an integer between 0 and 100 (inclusive) that represents the average
        //  law/chaos alignment of oFactionMember's faction.
        //  * Return value on error: -1
        public static int GetFactionAverageLawChaosAlignment(NWGameObject oFactionMember)
        {
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(188);
            return Internal.StackPopInteger();
        }

        //  Get the average level of the members of the faction.
        //  * Return value on error: -1
        public static int GetFactionAverageLevel(NWGameObject oFactionMember)
        {
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(189);
            return Internal.StackPopInteger();
        }

        //  Get the average XP of the members of the faction.
        //  * Return value on error: -1
        public static int GetFactionAverageXP(NWGameObject oFactionMember)
        {
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(190);
            return Internal.StackPopInteger();
        }

        //  Get the most frequent class in the faction - this can be compared with the
        //  constants CLASS_TYPE_*.
        //  * Return value on error: -1
        public static int GetFactionMostFrequentClass(NWGameObject oFactionMember)
        {
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(191);
            return Internal.StackPopInteger();
        }

        //  Get the object faction member with the lowest armour class.
        //  * Returns OBJECT_INVALID if oFactionMember's faction is invalid.
        public static NWGameObject GetFactionWorstAC(NWGameObject oFactionMember = null, bool bMustBeVisible = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMustBeVisible));
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(192);
            return Internal.StackPopObject();
        }

        //  Get the object faction member with the highest armour class.
        //  * Returns OBJECT_INVALID if oFactionMember's faction is invalid.
        public static NWGameObject GetFactionBestAC(NWGameObject oFactionMember = null, bool bMustBeVisible = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMustBeVisible));
            Internal.StackPushObject(oFactionMember, false);
            Internal.CallBuiltIn(193);
            return Internal.StackPopObject();
        }

        //  Sit in oChair.
        //  Note: Not all creatures will be able to sit and not all
        //        objects can be sat on.
        //        The object oChair must also be marked as usable in the toolset.
        // 
        //  For Example: To get a player to sit in oChair when they click on it,
        //  place the following script in the OnUsed event for the object oChair.
        //  void main()
        //  {
        //     object oChair = OBJECT_SELF;
        //     AssignCommand(GetLastUsedBy(),ActionSit(oChair));
        //  }
        public static void ActionSit(NWGameObject oChair)
        {
            Internal.StackPushObject(oChair, false);
            Internal.CallBuiltIn(194);
        }

        //  In an onConversation script this gets the number of the string pattern
        //  matched (the one that triggered the script).
        //  * Returns -1 if no string matched
        public static int GetListenPatternNumber()
        {
            Internal.CallBuiltIn(195);
            return Internal.StackPopInteger();
        }

        //  Jump to an object ID, or as near to it as possible.
        public static void ActionJumpToObject(NWGameObject oToJumpTo, bool bWalkStraightLineToPoint = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bWalkStraightLineToPoint));
            Internal.StackPushObject(oToJumpTo, false);
            Internal.CallBuiltIn(196);
        }

        //  Get the first waypoint with the specified tag.
        //  * Returns OBJECT_INVALID if the waypoint cannot be found.
        public static NWGameObject GetWaypointByTag(string sWaypointTag)
        {
            Internal.StackPushString(sWaypointTag);
            Internal.CallBuiltIn(197);
            return Internal.StackPopObject();
        }

        //  Get the destination object for the given object.
        // 
        //  All objects can hold a transition target, but only Doors and Triggers
        //  will be made clickable by the game engine (This may change in the
        //  future). You can set and query transition targets on other objects for
        //  your own scripted purposes.
        // 
        //  * Returns OBJECT_INVALID if oTransition does not hold a target.
        public static NWGameObject GetTransitionTarget(NWGameObject oTransition)
        {
            Internal.StackPushObject(oTransition, false);
            Internal.CallBuiltIn(198);
            return Internal.StackPopObject();
        }

        //  Link the two supplied effects, returning eChildEffect as a child of
        //  eParentEffect.
        //  Note: When applying linked effects if the target is immune to all valid
        //  effects all other effects will be removed as well. This means that if you
        //  apply a visual effect and a silence effect (in a link) and the target is
        //  immune to the silence effect that the visual effect will get removed as well.
        //  Visual Effects are not considered "valid" effects for the purposes of
        //  determining if an effect will be removed or not and as such should never be
        //  packaged *only* with other visual effects in a link.
        public static Effect EffectLinkEffects(Effect eChildEffect, Effect eParentEffect)
        {
            Internal.StackPushEffect(eParentEffect);
            Internal.StackPushEffect(eChildEffect);
            Internal.CallBuiltIn(199);
            return Internal.StackPopEffect();
        }

        //  Get the nNth object with the specified tag.
        //  - sTag
        //  - nNth: the nth object with this tag may be requested
        //  * Returns OBJECT_INVALID if the object cannot be found.
        //  Note: The module cannot be retrieved by GetObjectByTag(), use GetModule() instead.
        public static NWGameObject GetObjectByTag(string sTag, int nNth = 0)
        {
            Internal.StackPushInteger(nNth);
            Internal.StackPushString(sTag);
            Internal.CallBuiltIn(200);
            return Internal.StackPopObject();
        }

        //  Adjust the alignment of oSubject.
        //  - oSubject
        //  - nAlignment:
        //    -> ALIGNMENT_LAWFUL/ALIGNMENT_CHAOTIC/ALIGNMENT_GOOD/ALIGNMENT_EVIL: oSubject's
        //       alignment will be shifted in the direction specified
        //    -> ALIGNMENT_ALL: nShift will be added to oSubject's law/chaos and
        //       good/evil alignment values
        //    -> ALIGNMENT_NEUTRAL: nShift is applied to oSubject's law/chaos and
        //       good/evil alignment values in the direction which is towards neutrality.
        //      e.g. If oSubject has a law/chaos value of 10 (i.e. chaotic) and a
        //           good/evil value of 80 (i.e. good) then if nShift is 15, the
        //           law/chaos value will become (10+15)=25 and the good/evil value will
        //           become (80-25)=55
        //      Furthermore, the shift will at most take the alignment value to 50 and
        //      not beyond.
        //      e.g. If oSubject has a law/chaos value of 40 and a good/evil value of 70,
        //           then if nShift is 15, the law/chaos value will become 50 and the
        //           good/evil value will become 55
        //  - nShift: this is the desired shift in alignment
        //  - bAllPartyMembers: when TRUE the alignment shift of oSubject also has a 
        //                      diminished affect all members of oSubject's party (if oSubject is a Player).
        //                      When FALSE the shift only affects oSubject.
        //  * No return value
        public static void AdjustAlignment(NWGameObject oSubject, Alignment nAlignment, int nShift, bool bAllPartyMembers = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bAllPartyMembers));
            Internal.StackPushInteger(nShift);
            Internal.StackPushInteger((int)nAlignment);
            Internal.StackPushObject(oSubject, false);
            Internal.CallBuiltIn(201);
        }

        //  Do nothing for fSeconds seconds.
        public static void ActionWait(float fSeconds)
        {
            Internal.StackPushFloat(fSeconds);
            Internal.CallBuiltIn(202);
        }

        //  Set the transition bitmap of a player; this should only be called in area
        //  transition scripts. This action should be run by the person "clicking" the
        //  area transition via AssignCommand.
        //  - nPredefinedAreaTransition:
        //    -> To use a predefined area transition bitmap, use one of AREA_TRANSITION_*
        //    -> To use a custom, user-defined area transition bitmap, use
        //       AREA_TRANSITION_USER_DEFINED and specify the filename in the second
        //       parameter
        //  - sCustomAreaTransitionBMP: this is the filename of a custom, user-defined
        //    area transition bitmap
        public static void SetAreaTransitionBMP(AreaTransition nPredefinedAreaTransition, string sCustomAreaTransitionBMP = "")
        {
            Internal.StackPushString(sCustomAreaTransitionBMP);
            Internal.StackPushInteger((int)nPredefinedAreaTransition);
            Internal.CallBuiltIn(203);
        }

        //  Starts a conversation with oObjectToConverseWith - this will cause their
        //  OnDialog event to fire.
        //  - oObjectToConverseWith
        //  - sDialogResRef: If this is blank, the creature's own dialogue file will be used
        //  - bPrivateConversation
        //  Turn off bPlayHello if you don't want the initial greeting to play
        public static void ActionStartConversation(NWGameObject oObjectToConverseWith, string sDialogResRef = "", bool bPrivateConversation = false, bool bPlayHello = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bPlayHello));
            Internal.StackPushInteger(Convert.ToInt32(bPrivateConversation));
            Internal.StackPushString(sDialogResRef);
            Internal.StackPushObject(oObjectToConverseWith, false);
            Internal.CallBuiltIn(204);
        }

        //  Pause the current conversation.
        public static void ActionPauseConversation()
        {
            Internal.CallBuiltIn(205);
        }

        //  Resume a conversation after it has been paused.
        public static void ActionResumeConversation()
        {
            Internal.CallBuiltIn(206);
        }

        //  Create a Beam effect.
        //  - nBeamVisualEffect: VFX_BEAM_*
        //  - oEffector: the beam is emitted from this creature
        //  - nBodyPart: BODY_NODE_*
        //  - bMissEffect: If this is TRUE, the beam will fire to a random vector near or
        //    past the target
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nBeamVisualEffect is
        //    not valid.
        public static Effect EffectBeam(Vfx nBeamVisualEffect, NWGameObject oEffector, int nBodyPart, bool bMissEffect = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bMissEffect));
            Internal.StackPushInteger(nBodyPart);
            Internal.StackPushObject(oEffector, false);
            Internal.StackPushInteger((int)nBeamVisualEffect);
            Internal.CallBuiltIn(207);
            return Internal.StackPopEffect();
        }

        //  Get an integer between 0 and 100 (inclusive) that represents how oSource
        //  feels about oTarget.
        //  -> 0-10 means oSource is hostile to oTarget
        //  -> 11-89 means oSource is neutral to oTarget
        //  -> 90-100 means oSource is friendly to oTarget
        //  * Returns -1 if oSource or oTarget does not identify a valid object
        public static int GetReputation(NWGameObject oSource, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oSource, false);
            Internal.CallBuiltIn(208);
            return Internal.StackPopInteger();
        }

        //  Adjust how oSourceFactionMember's faction feels about oTarget by the
        //  specified amount.
        //  Note: This adjusts Faction Reputation, how the entire faction that
        //  oSourceFactionMember is in, feels about oTarget.
        //  * No return value
        //  Note: You can't adjust a player character's (PC) faction towards
        //        NPCs, so attempting to make an NPC hostile by passing in a PC object
        //        as oSourceFactionMember in the following call will fail:
        //        AdjustReputation(oNPC,oPC,-100);
        //        Instead you should pass in the PC object as the first
        //        parameter as in the following call which should succeed: 
        //        AdjustReputation(oPC,oNPC,-100);
        //  Note: Will fail if oSourceFactionMember is a plot object.
        public static void AdjustReputation(NWGameObject oTarget, NWGameObject oSourceFactionMember, int nAdjustment)
        {
            Internal.StackPushInteger(nAdjustment);
            Internal.StackPushObject(oSourceFactionMember, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(209);
        }

        //  Get the creature that is currently sitting on the specified object.
        //  - oChair
        //  * Returns OBJECT_INVALID if oChair is not a valid placeable.
        public static NWGameObject GetSittingCreature(NWGameObject oChair)
        {
            Internal.StackPushObject(oChair, false);
            Internal.CallBuiltIn(210);
            return Internal.StackPopObject();
        }

        //  Get the creature that is going to attack oTarget.
        //  Note: This value is cleared out at the end of every combat round and should
        //  not be used in any case except when getting a "going to be attacked" shout
        //  from the master creature (and this creature is a henchman)
        //  * Returns OBJECT_INVALID if oTarget is not a valid creature.
        public static NWGameObject GetGoingToBeAttackedBy(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(211);
            return Internal.StackPopObject();
        }

        //  Create a Spell Resistance Increase effect.
        //  - nValue: size of spell resistance increase
        public static Effect EffectSpellResistanceIncrease(int nValue)
        {
            Internal.StackPushInteger(nValue);
            Internal.CallBuiltIn(212);
            return Internal.StackPopEffect();
        }

        //  Get the location of oObject.
        public static Location GetLocation(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(213);
            return Internal.StackPopLocation();
        }

        //  The subject will jump to lLocation instantly (even between areas).
        //  If lLocation is invalid, nothing will happen.
        public static void ActionJumpToLocation(Location lLocation)
        {
            Internal.StackPushLocation(lLocation);
            Internal.CallBuiltIn(214);
        }

        //  Create a location.
        public static Location Location(NWGameObject oArea, Vector? vPosition, float fOrientation)
        {
            Internal.StackPushFloat(fOrientation);
            Internal.StackPushVector(vPosition);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(215);
            return Internal.StackPopLocation();
        }

        //  Apply eEffect at lLocation.
        public static void ApplyEffectAtLocation(DurationType nDurationType, Effect eEffect, Location lLocation, float fDuration = 0.0f)
        {
            Internal.StackPushFloat(fDuration);
            Internal.StackPushLocation(lLocation);
            Internal.StackPushEffect(eEffect);
            Internal.StackPushInteger((int)nDurationType);
            Internal.CallBuiltIn(216);
        }

        //  * Returns TRUE if oCreature is a Player Controlled character.
        private static bool GetIsPC(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(217);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Convert fFeet into a number of meters.
        public static float FeetToMeters(float fFeet)
        {
            Internal.StackPushFloat(fFeet);
            Internal.CallBuiltIn(218);
            return Internal.StackPopFloat();
        }

        //  Convert fYards into a number of meters.
        public static float YardsToMeters(float fYards)
        {
            Internal.StackPushFloat(fYards);
            Internal.CallBuiltIn(219);
            return Internal.StackPopFloat();
        }

        //  Apply eEffect to oTarget.
        public static void ApplyEffectToObject(DurationType nDurationType, Effect eEffect, NWGameObject oTarget, float fDuration = 0.0f)
        {
            Internal.StackPushFloat(fDuration);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushEffect(eEffect);
            Internal.StackPushInteger((int)nDurationType);
            Internal.CallBuiltIn(220);
        }

        //  The caller will immediately speak sStringToSpeak (this is different from
        //  ActionSpeakString)
        //  - sStringToSpeak
        //  - nTalkVolume: TALKVOLUME_*
        public static void SpeakString(string sStringToSpeak, TalkVolume nTalkVolume = TalkVolume.Talk)
        {
            Internal.StackPushInteger((int)nTalkVolume);
            Internal.StackPushString(sStringToSpeak);
            Internal.CallBuiltIn(221);
        }

        //  Get the location of the caller's last spell target.
        public static Location GetSpellTargetLocation()
        {
            Internal.CallBuiltIn(222);
            return Internal.StackPopLocation();
        }

        //  Get the position vector from lLocation.
        public static Vector GetPositionFromLocation(Location lLocation)
        {
            Internal.StackPushLocation(lLocation);
            Internal.CallBuiltIn(223);
            return Internal.StackPopVector();
        }

        //  Get the area's object ID from lLocation.
        public static NWGameObject GetAreaFromLocation(Location lLocation)
        {
            Internal.StackPushLocation(lLocation);
            Internal.CallBuiltIn(224);
            return Internal.StackPopObject();
        }

        //  Get the orientation value from lLocation.
        public static float GetFacingFromLocation(Location lLocation)
        {
            Internal.StackPushLocation(lLocation);
            Internal.CallBuiltIn(225);
            return Internal.StackPopFloat();
        }

        //  Get the creature nearest to lLocation, subject to all the criteria specified.
        //  - nFirstCriteriaType: CREATURE_TYPE_*
        //  - nFirstCriteriaValue:
        //    -> CLASS_TYPE_* if nFirstCriteriaType was CREATURE_TYPE_CLASS
        //    -> SPELL_* if nFirstCriteriaType was CREATURE_TYPE_DOES_NOT_HAVE_SPELL_EFFECT
        //       or CREATURE_TYPE_HAS_SPELL_EFFECT
        //    -> TRUE or FALSE if nFirstCriteriaType was CREATURE_TYPE_IS_ALIVE
        //    -> PERCEPTION_* if nFirstCriteriaType was CREATURE_TYPE_PERCEPTION
        //    -> PLAYER_CHAR_IS_PC or PLAYER_CHAR_NOT_PC if nFirstCriteriaType was
        //       CREATURE_TYPE_PLAYER_CHAR
        //    -> RACIAL_TYPE_* if nFirstCriteriaType was CREATURE_TYPE_RACIAL_TYPE
        //    -> REPUTATION_TYPE_* if nFirstCriteriaType was CREATURE_TYPE_REPUTATION
        //    For example, to get the nearest PC, use
        //    (CREATURE_TYPE_PLAYER_CHAR, PLAYER_CHAR_IS_PC)
        //  - lLocation: We're trying to find the creature of the specified type that is
        //    nearest to lLocation
        //  - nNth: We don't have to find the first nearest: we can find the Nth nearest....
        //  - nSecondCriteriaType: This is used in the same way as nFirstCriteriaType to
        //    further specify the type of creature that we are looking for.
        //  - nSecondCriteriaValue: This is used in the same way as nFirstCriteriaValue
        //    to further specify the type of creature that we are looking for.
        //  - nThirdCriteriaType: This is used in the same way as nFirstCriteriaType to
        //    further specify the type of creature that we are looking for.
        //  - nThirdCriteriaValue: This is used in the same way as nFirstCriteriaValue to
        //    further specify the type of creature that we are looking for.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetNearestCreatureToLocation(int nFirstCriteriaType, int nFirstCriteriaValue, Location lLocation, int nNth = 1, int nSecondCriteriaType = -1, int nSecondCriteriaValue = -1, int nThirdCriteriaType = -1, int nThirdCriteriaValue = -1)
        {
            Internal.StackPushInteger(nThirdCriteriaValue);
            Internal.StackPushInteger(nThirdCriteriaType);
            Internal.StackPushInteger(nSecondCriteriaValue);
            Internal.StackPushInteger(nSecondCriteriaType);
            Internal.StackPushInteger(nNth);
            Internal.StackPushLocation(lLocation);
            Internal.StackPushInteger(nFirstCriteriaValue);
            Internal.StackPushInteger(nFirstCriteriaType);
            Internal.CallBuiltIn(226);
            return Internal.StackPopObject();
        }

        //  Get the Nth object nearest to oTarget that is of the specified type.
        //  - nObjectType: OBJECT_TYPE_*
        //  - oTarget
        //  - nNth
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetNearestObject(ObjectType nObjectType = ObjectType.All, NWGameObject oTarget = null, int nNth = 1)
        {
            Internal.StackPushInteger(nNth);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nObjectType);
            Internal.CallBuiltIn(227);
            return Internal.StackPopObject();
        }

        //  Get the nNth object nearest to lLocation that is of the specified type.
        //  - nObjectType: OBJECT_TYPE_*
        //  - lLocation
        //  - nNth
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetNearestObjectToLocation(ObjectType nObjectType, Location lLocation, int nNth = 1)
        {
            Internal.StackPushInteger(nNth);
            Internal.StackPushLocation(lLocation);
            Internal.StackPushInteger((int)nObjectType);
            Internal.CallBuiltIn(228);
            return Internal.StackPopObject();
        }

        //  Get the nth Object nearest to oTarget that has sTag as its tag.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetNearestObjectByTag(string sTag, NWGameObject oTarget = null, int nNth = 1)
        {
            Internal.StackPushInteger(nNth);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushString(sTag);
            Internal.CallBuiltIn(229);
            return Internal.StackPopObject();
        }

        //  Cast spell nSpell at lTargetLocation.
        //  - nSpell: SPELL_*
        //  - lTargetLocation
        //  - nMetaMagic: METAMAGIC_*
        //  - bCheat: If this is TRUE, then the executor of the action doesn't have to be
        //    able to cast the spell.
        //  - nProjectilePathType: PROJECTILE_PATH_TYPE_*
        //  - bInstantSpell: If this is TRUE, the spell is cast immediately; this allows
        //    the end-user to simulate
        //    a high-level magic user having lots of advance warning of impending trouble.
        public static void ActionCastSpellAtLocation(Spell nSpell, Location lTargetLocation, MetaMagic nMetaMagic = MetaMagic.Any, bool bCheat = false, ProjectilePathType nProjectilePathType = ProjectilePathType.Default, bool bInstantSpell = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bInstantSpell));
            Internal.StackPushInteger((int)nProjectilePathType);
            Internal.StackPushInteger(Convert.ToInt32(bCheat));
            Internal.StackPushInteger((int)nMetaMagic);
            Internal.StackPushLocation(lTargetLocation);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(234);
        }

        //  * Returns TRUE if oSource considers oTarget as an enemy.
        public static bool GetIsEnemy(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(235);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  * Returns TRUE if oSource considers oTarget as a friend.
        public static bool GetIsFriend(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(236);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  * Returns TRUE if oSource considers oTarget as neutral.
        public static bool GetIsNeutral(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(237);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the PC that is involved in the conversation.
        //  * Returns OBJECT_INVALID on error.
        public static NWGameObject GetPCSpeaker()
        {
            Internal.CallBuiltIn(238);
            return Internal.StackPopObject();
        }

        //  Get a string from the talk table using nStrRef.
        public static string GetStringByStrRef(int nStrRef, Gender nGender = Gender.Male)
        {
            Internal.StackPushInteger((int)nGender);
            Internal.StackPushInteger(nStrRef);
            Internal.CallBuiltIn(239);
            return Internal.StackPopString();
        }

        //  Causes the creature to speak a translated string.
        //  - nStrRef: Reference of the string in the talk table
        //  - nTalkVolume: TALKVOLUME_*
        public static void ActionSpeakStringByStrRef(int nStrRef, TalkVolume nTalkVolume = TalkVolume.Talk)
        {
            Internal.StackPushInteger((int)nTalkVolume);
            Internal.StackPushInteger(nStrRef);
            Internal.CallBuiltIn(240);
        }

        //  Destroy oObject (irrevocably).
        //  This will not work on modules and areas.
        public static void DestroyObject(NWGameObject oDestroy, float fDelay = 0.0f)
        {
            Internal.StackPushFloat(fDelay);
            Internal.StackPushObject(oDestroy, false);
            Internal.CallBuiltIn(241);
        }

        //  Get the module.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetModule()
        {
            Internal.CallBuiltIn(242);
            return Internal.StackPopObject();
        }

        //  Create an object of the specified type at lLocation.
        //  - nObjectType: OBJECT_TYPE_ITEM, OBJECT_TYPE_CREATURE, OBJECT_TYPE_PLACEABLE,
        //    OBJECT_TYPE_STORE, OBJECT_TYPE_WAYPOINT
        //  - sTemplate
        //  - lLocation
        //  - bUseAppearAnimation
        //  - sNewTag - if this string is not empty, it will replace the default tag from the template
        public static NWGameObject CreateObject(ObjectType nObjectType, string sTemplate, Location lLocation, bool bUseAppearAnimation = false, string sNewTag = "")
        {
            Internal.StackPushString(sNewTag);
            Internal.StackPushInteger(Convert.ToInt32(bUseAppearAnimation));
            Internal.StackPushLocation(lLocation);
            Internal.StackPushString(sTemplate);
            Internal.StackPushInteger((int)nObjectType);
            Internal.CallBuiltIn(243);
            var result = Internal.StackPopObject();

            MessageHub.Instance.Publish(new ObjectCreated(result));

            return result;
        }

        //  Create an event which triggers the "SpellCastAt" script
        //  Note: This only creates the event. The event wont actually trigger until SignalEvent()
        //  is called using this created SpellCastAt event as an argument.
        //  For example:
        //      SignalEvent(oCreature, EventSpellCastAt(oCaster, SPELL_MAGIC_MISSILE, TRUE));
        //  This function doesn't cast the spell specified, it only creates an event so that
        //  when the event is signaled on an object, the object will use its OnSpellCastAt script
        //  to react to the spell being cast.
        // 
        //  To specify the OnSpellCastAt script that should run, view the Object's Properties 
        //  and click on the Scripts Tab. Then specify a script for the OnSpellCastAt event.
        //  From inside the OnSpellCastAt script call:
        //      GetLastSpellCaster() to get the object that cast the spell (oCaster).
        //      GetLastSpell() to get the type of spell cast (nSpell)
        //      GetLastSpellHarmful() to determine if the spell cast at the object was harmful.
        public static Event EventSpellCastAt(NWGameObject oCaster, Spell nSpell, bool bHarmful = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bHarmful));
            Internal.StackPushInteger((int)nSpell);
            Internal.StackPushObject(oCaster, false);
            Internal.CallBuiltIn(244);
            return Internal.StackPopEvent();
        }

        //  This is for use in a "Spell Cast" it gets who cast the spell.
        //  The spell could have been cast by a creature, placeable or door.
        //  * Returns OBJECT_INVALID if the caller is not a creature, placeable or door.
        public static NWGameObject GetLastSpellCaster()
        {
            Internal.CallBuiltIn(245);
            return Internal.StackPopObject();
        }

        //  This is for use in a "Spell Cast" it gets the ID of the spell that
        //  was cast.
        public static Spell GetLastSpell()
        {
            Internal.CallBuiltIn(246);
            return (Spell)Internal.StackPopInteger();
        }

        //  This is for use in a user-defined it gets the event number.
        public static int GetUserDefinedEventNumber()
        {
            Internal.CallBuiltIn(247);
            return Internal.StackPopInteger();
        }

        //  This is for use in a Spell it gets the ID of the spell that is being
        //  cast (SPELL_*).
        public static int GetSpellId()
        {
            Internal.CallBuiltIn(248);
            return Internal.StackPopInteger();
        }

        //  Generate a random name.
        //  nNameType: The type of random name to be generated (NAME_*)
        public static string RandomName(Name nNameType = Name.FirstGenericMale)
        {
            Internal.StackPushInteger((int)nNameType);
            Internal.CallBuiltIn(249);
            return Internal.StackPopString();
        }

        //  Create a Poison effect.
        //  - nPoisonType: POISON_*
        public static Effect EffectPoison(Poison nPoisonType)
        {
            Internal.StackPushInteger((int)nPoisonType);
            Internal.CallBuiltIn(250);
            return Internal.StackPopEffect();
        }

        //  Create a Disease effect.
        //  - nDiseaseType: DISEASE_*
        public static Effect EffectDisease(Disease nDiseaseType)
        {
            Internal.StackPushInteger((int)nDiseaseType);
            Internal.CallBuiltIn(251);
            return Internal.StackPopEffect();
        }

        //  Create a Silence effect.
        public static Effect EffectSilence()
        {
            Internal.CallBuiltIn(252);
            return Internal.StackPopEffect();
        }

        //  Set the name of oObject.
        // 
        //  - oObject: the object for which you are changing the name (area, creature, placeable, item, or door).
        //  - sNewName: the new name that the object will use.
        //  Note: SetName() does not work on player objects.
        //        Setting an object's name to "" will make the object
        //        revert to using the name it had originally before any
        //        SetName() calls were made on the object.
        public static string GetName(NWGameObject oObject, bool bOriginalName = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bOriginalName));
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(253);
            return Internal.StackPopString();
        }

        //  Use this in a conversation script to get the person with whom you are conversing.
        //  * Returns OBJECT_INVALID if the caller is not a valid creature.
        public static NWGameObject GetLastSpeaker()
        {
            Internal.CallBuiltIn(254);
            return Internal.StackPopObject();
        }

        //  Use this in an OnDialog script to start up the dialog tree.
        //  - sResRef: if this is not specified, the default dialog file will be used
        //  - oObjectToDialog: if this is not specified the person that triggered the
        //    event will be used
        public static int BeginConversation(string sResRef = "", NWGameObject oObjectToDialog = null)
        {
            Internal.StackPushObject(oObjectToDialog, false);
            Internal.StackPushString(sResRef);
            Internal.CallBuiltIn(255);
            return Internal.StackPopInteger();
        }

        //  Use this in an OnPerception script to get the object that was perceived.
        //  * Returns OBJECT_INVALID if the caller is not a valid creature.
        public static NWGameObject GetLastPerceived()
        {
            Internal.CallBuiltIn(256);
            return Internal.StackPopObject();
        }

        //  Use this in an OnPerception script to determine whether the object that was
        //  perceived was heard.
        public static bool GetLastPerceptionHeard()
        {
            Internal.CallBuiltIn(257);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in an OnPerception script to determine whether the object that was
        //  perceived has become inaudible.
        public static bool GetLastPerceptionInaudible()
        {
            Internal.CallBuiltIn(258);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in an OnPerception script to determine whether the object that was
        //  perceived was seen.
        public static bool GetLastPerceptionSeen()
        {
            Internal.CallBuiltIn(259);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in an OnClosed script to get the object that closed the door or placeable.
        //  * Returns OBJECT_INVALID if the caller is not a valid door or placeable.
        public static NWGameObject GetLastClosedBy()
        {
            Internal.CallBuiltIn(260);
            return Internal.StackPopObject();
        }

        //  Use this in an OnPerception script to determine whether the object that was
        //  perceived has vanished.
        public static bool GetLastPerceptionVanished()
        {
            Internal.CallBuiltIn(261);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the first object within oPersistentObject.
        //  - oPersistentObject
        //  - nResidentObjectType: OBJECT_TYPE_*
        //  - nPersistentZone: PERSISTENT_ZONE_ACTIVE. [This could also take the value
        //    PERSISTENT_ZONE_FOLLOW, but this is no longer used.]
        //  * Returns OBJECT_INVALID if no object is found.
        public static NWGameObject GetFirstInPersistentObject(NWGameObject oPersistentObject = null, ObjectType nResidentObjectType = ObjectType.Creature, PersistentZone nPersistentZone = PersistentZone.Active)
        {
            Internal.StackPushInteger((int)nPersistentZone);
            Internal.StackPushInteger((int)nResidentObjectType);
            Internal.StackPushObject(oPersistentObject, false);
            Internal.CallBuiltIn(262);
            return Internal.StackPopObject();
        }

        //  Get the next object within oPersistentObject.
        //  - oPersistentObject
        //  - nResidentObjectType: OBJECT_TYPE_*
        //  - nPersistentZone: PERSISTENT_ZONE_ACTIVE. [This could also take the value
        //    PERSISTENT_ZONE_FOLLOW, but this is no longer used.]
        //  * Returns OBJECT_INVALID if no object is found.
        public static NWGameObject GetNextInPersistentObject(NWGameObject oPersistentObject = null, ObjectType nResidentObjectType = ObjectType.Creature, PersistentZone nPersistentZone = PersistentZone.Active)
        {
            Internal.StackPushInteger((int)nPersistentZone);
            Internal.StackPushInteger((int)nResidentObjectType);
            Internal.StackPushObject(oPersistentObject, false);
            Internal.CallBuiltIn(263);
            return Internal.StackPopObject();
        }

        //  This returns the creator of oAreaOfEffectObject.
        //  * Returns OBJECT_INVALID if oAreaOfEffectObject is not a valid Area of Effect object.
        public static NWGameObject GetAreaOfEffectCreator(NWGameObject oAreaOfEffectObject = null)
        {
            Internal.StackPushObject(oAreaOfEffectObject, false);
            Internal.CallBuiltIn(264);
            return Internal.StackPopObject();
        }

        //  Delete oObject's local integer variable sVarName
        public static void DeleteLocalInt(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(265);
        }

        //  Delete oObject's local float variable sVarName
        public static void DeleteLocalFloat(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(266);
        }

        //  Delete oObject's local string variable sVarName
        public static void DeleteLocalString(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(267);
        }

        //  Delete oObject's local object variable sVarName
        public static void DeleteLocalObject(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(268);
        }

        //  Delete oObject's local location variable sVarName
        public static void DeleteLocalLocation(NWGameObject oObject, string sVarName)
        {
            Internal.StackPushString(sVarName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(269);
        }

        //  Create a Haste effect.
        public static Effect EffectHaste()
        {
            Internal.CallBuiltIn(270);
            return Internal.StackPopEffect();
        }

        //  Create a Slow effect.
        public static Effect EffectSlow()
        {
            Internal.CallBuiltIn(271);
            return Internal.StackPopEffect();
        }

        //  Convert oObject into a hexadecimal string.
        public static string ObjectToString(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(272);
            return Internal.StackPopString();
        }

        //  Create an Immunity effect.
        //  - nImmunityType: IMMUNITY_TYPE_*
        public static Effect EffectImmunity(ImmunityType nImmunityType)
        {
            Internal.StackPushInteger((int)nImmunityType);
            Internal.CallBuiltIn(273);
            return Internal.StackPopEffect();
        }

        //  - oCreature
        //  - nImmunityType: IMMUNITY_TYPE_*
        //  - oVersus: if this is specified, then we also check for the race and
        //    alignment of oVersus
        //  * Returns TRUE if oCreature has immunity of type nImmunity versus oVersus.
        public static bool GetIsImmune(NWGameObject oCreature, ImmunityType nImmunityType, NWGameObject oVersus = null)
        {
            Internal.StackPushObject(oVersus, false);
            Internal.StackPushInteger((int)nImmunityType);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(274);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Creates a Damage Immunity Increase effect.
        //  - nDamageType: DAMAGE_TYPE_*
        //  - nPercentImmunity
        public static Effect EffectDamageImmunityIncrease(DamageType nDamageType, int nPercentImmunity)
        {
            Internal.StackPushInteger(nPercentImmunity);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(275);
            return Internal.StackPopEffect();
        }

        //  Determine whether oEncounter is active.
        public static bool GetEncounterActive(NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.CallBuiltIn(276);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Set oEncounter's active state to nNewValue.
        //  - nNewValue: TRUE/FALSE
        //  - oEncounter
        public static void SetEncounterActive(int nNewValue, NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.StackPushInteger(nNewValue);
            Internal.CallBuiltIn(277);
        }

        //  Get the maximum number of times that oEncounter will spawn.
        public static int GetEncounterSpawnsMax(NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.CallBuiltIn(278);
            return Internal.StackPopInteger();
        }

        //  Set the maximum number of times that oEncounter can spawn
        public static void SetEncounterSpawnsMax(int nNewValue, NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.StackPushInteger(nNewValue);
            Internal.CallBuiltIn(279);
        }

        //  Get the number of times that oEncounter has spawned so far
        public static int GetEncounterSpawnsCurrent(NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.CallBuiltIn(280);
            return Internal.StackPopInteger();
        }

        //  Set the number of times that oEncounter has spawned so far
        public static void SetEncounterSpawnsCurrent(int nNewValue, NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.StackPushInteger(nNewValue);
            Internal.CallBuiltIn(281);
        }

        //  Use this in an OnItemAcquired script to get the item that was acquired.
        //  * Returns OBJECT_INVALID if the module is not valid.
        public static NWGameObject GetModuleItemAcquired()
        {
            Internal.CallBuiltIn(282);
            return Internal.StackPopObject();
        }

        //  Use this in an OnItemAcquired script to get the creatre that previously
        //  possessed the item.
        //  * Returns OBJECT_INVALID if the item was picked up from the ground.
        public static NWGameObject GetModuleItemAcquiredFrom()
        {
            Internal.CallBuiltIn(283);
            return Internal.StackPopObject();
        }

        //  Set the value for a custom token.
        public static void SetCustomToken(int nCustomTokenNumber, string sTokenValue)
        {
            Internal.StackPushString(sTokenValue);
            Internal.StackPushInteger(nCustomTokenNumber);
            Internal.CallBuiltIn(284);
        }

        //  Determine whether oCreature has nFeat, and nFeat is useable.
        //  - nFeat: FEAT_*
        //  - oCreature
        public static bool GetHasFeat(Feat nFeat, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nFeat);
            Internal.CallBuiltIn(285);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Determine whether oCreature has nSkill, and nSkill is useable.
        //  - nSkill: SKILL_*
        //  - oCreature
        public static bool GetHasSkill(Skill nSkill, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(286);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use nFeat on oTarget.
        //  - nFeat: FEAT_*
        //  - oTarget
        public static void ActionUseFeat(Feat nFeat, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nFeat);
            Internal.CallBuiltIn(287);
        }

        //  Runs the action "UseSkill" on the current creature
        //  Use nSkill on oTarget.
        //  - nSkill: SKILL_*
        //  - oTarget
        //  - nSubSkill: SUBSKILL_*
        //  - oItemUsed: Item to use in conjunction with the skill
        public static void ActionUseSkill(Skill nSkill, NWGameObject oTarget, int nSubSkill = 0, NWGameObject oItemUsed = null)
        {
            Internal.StackPushObject(oItemUsed, false);
            Internal.StackPushInteger(nSubSkill);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(288);
        }

        //  Determine whether oSource sees oTarget.
        //  NOTE: This *only* works on creatures, as visibility lists are not
        //        maintained for non-creature objects.
        public static bool GetObjectSeen(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(289);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Determine whether oSource hears oTarget.
        //  NOTE: This *only* works on creatures, as visibility lists are not
        //        maintained for non-creature objects.
        public static bool GetObjectHeard(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(290);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in an OnPlayerDeath module script to get the last player that died.
        public static NWGameObject GetLastPlayerDied()
        {
            Internal.CallBuiltIn(291);
            return Internal.StackPopObject();
        }

        //  Use this in an OnItemLost script to get the item that was lost/dropped.
        //  * Returns OBJECT_INVALID if the module is not valid.
        public static NWGameObject GetModuleItemLost()
        {
            Internal.CallBuiltIn(292);
            return Internal.StackPopObject();
        }

        //  Use this in an OnItemLost script to get the creature that lost the item.
        //  * Returns OBJECT_INVALID if the module is not valid.
        public static NWGameObject GetModuleItemLostBy()
        {
            Internal.CallBuiltIn(293);
            return Internal.StackPopObject();
        }

        //  Do aActionToDo.
        //  Creates a conversation event.
        //  Note: This only creates the event. The event wont actually trigger until SignalEvent()
        //  is called using this created conversation event as an argument.
        //  For example:
        //      SignalEvent(oCreature, EventConversation());
        //  Once the event has been signaled. The script associated with the OnConversation event will
        //  run on the creature oCreature.
        // 
        //  To specify the OnConversation script that should run, view the Creature Properties on
        //  the creature and click on the Scripts Tab. Then specify a script for the OnConversation event.
        public static Event EventConversation()
        {
            Internal.CallBuiltIn(295);
            return Internal.StackPopEvent();
        }

        //  Set the difficulty level of oEncounter.
        //  - nEncounterDifficulty: ENCOUNTER_DIFFICULTY_*
        //  - oEncounter
        public static void SetEncounterDifficulty(EncounterDifficulty nEncounterDifficulty, NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.StackPushInteger((int)nEncounterDifficulty);
            Internal.CallBuiltIn(296);
        }

        //  Get the difficulty level of oEncounter.
        public static EncounterDifficulty GetEncounterDifficulty(NWGameObject oEncounter = null)
        {
            Internal.StackPushObject(oEncounter, false);
            Internal.CallBuiltIn(297);
            return (EncounterDifficulty)Internal.StackPopInteger();
        }

        //  Get the distance between lLocationA and lLocationB.
        public static float GetDistanceBetweenLocations(Location lLocationA, Location lLocationB)
        {
            Internal.StackPushLocation(lLocationB);
            Internal.StackPushLocation(lLocationA);
            Internal.CallBuiltIn(298);
            return Internal.StackPopFloat();
        }

        //  Use this in spell scripts to get nDamage adjusted by oTarget's reflex and
        //  evasion saves.
        //  - nDamage
        //  - oTarget
        //  - nDC: Difficulty check
        //  - nSaveType: SAVING_THROW_TYPE_*
        //  - oSaveVersus
        public static int GetReflexAdjustedDamage(int nDamage, NWGameObject oTarget, int nDC, SavingThrowType nSaveType = SavingThrowType.None, NWGameObject oSaveVersus = null)
        {
            Internal.StackPushObject(oSaveVersus, false);
            Internal.StackPushInteger((int)nSaveType);
            Internal.StackPushInteger(nDC);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger(nDamage);
            Internal.CallBuiltIn(299);
            return Internal.StackPopInteger();
        }

        //  Play nAnimation immediately.
        //  - nAnimation: ANIMATION_*
        //  - fSpeed
        //  - fSeconds
        public static void PlayAnimation(Animation nAnimation, float fSpeed = 1.0f, float fSeconds = 0.0f)
        {
            Internal.StackPushFloat(fSeconds);
            Internal.StackPushFloat(fSpeed);
            Internal.StackPushInteger((int)nAnimation);
            Internal.CallBuiltIn(300);
        }

        //  Create a Spell Talent.
        //  - nSpell: SPELL_*
        public static Talent TalentSpell(Spell nSpell)
        {
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(301);
            return Internal.StackPopTalent();
        }

        //  Create a Feat Talent.
        //  - nFeat: FEAT_*
        public static Talent TalentFeat(Feat nFeat)
        {
            Internal.StackPushInteger((int)nFeat);
            Internal.CallBuiltIn(302);
            return Internal.StackPopTalent();
        }

        //  Create a Skill Talent.
        //  - nSkill: SKILL_*
        public static Talent TalentSkill(Skill nSkill)
        {
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(303);
            return Internal.StackPopTalent();
        }

        //  Determines whether oObject has any effects applied by nSpell
        //  - nSpell: SPELL_*
        //  - oObject
        //  * The spell id on effects is only valid if the effect is created
        //    when the spell script runs. If it is created in a delayed command
        //    then the spell id on the effect will be invalid.
        public static bool GetHasSpellEffect(Spell nSpell, NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(304);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the spell (SPELL_*) that applied eSpellEffect.
        //  * Returns -1 if eSpellEffect was applied outside a spell script.
        public static Spell GetEffectSpellId(Effect eSpellEffect)
        {
            Internal.StackPushEffect(eSpellEffect);
            Internal.CallBuiltIn(305);
            return (Spell)Internal.StackPopInteger();
        }

        //  Determine whether oCreature has tTalent.
        public static bool GetCreatureHasTalent(Talent tTalent, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushTalent(tTalent);
            Internal.CallBuiltIn(306);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get a random talent of oCreature, within nCategory.
        //  - nCategory: TALENT_CATEGORY_*
        //  - oCreature
        public static Talent GetCreatureTalentRandom(TalentCategory nCategory, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nCategory);
            Internal.CallBuiltIn(307);
            return Internal.StackPopTalent();
        }

        //  Get the best talent (i.e. closest to nCRMax without going over) of oCreature,
        //  within nCategory.
        //  - nCategory: TALENT_CATEGORY_*
        //  - nCRMax: Challenge Rating of the talent
        //  - oCreature
        public static Talent GetCreatureTalentBest(TalentCategory nCategory, int nCRMax, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger(nCRMax);
            Internal.StackPushInteger((int)nCategory);
            Internal.CallBuiltIn(308);
            return Internal.StackPopTalent();
        }

        //  Use tChosenTalent on oTarget.
        public static void ActionUseTalentOnObject(Talent tChosenTalent, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushTalent(tChosenTalent);
            Internal.CallBuiltIn(309);
        }

        //  Use tChosenTalent at lTargetLocation.
        public static void ActionUseTalentAtLocation(Talent tChosenTalent, Location lTargetLocation)
        {
            Internal.StackPushLocation(lTargetLocation);
            Internal.StackPushTalent(tChosenTalent);
            Internal.CallBuiltIn(310);
        }

        //  Get the gold piece value of oItem.
        //  * Returns 0 if oItem is not a valid item.
        public static int GetGoldPieceValue(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(311);
            return Internal.StackPopInteger();
        }

        //  * Returns TRUE if oCreature is of a playable racial type.
        public static bool GetIsPlayableRacialType(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(312);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Jump to lDestination.  The action is added to the TOP of the action queue.
        public static void JumpToLocation(Location lDestination)
        {
            Internal.StackPushLocation(lDestination);
            Internal.CallBuiltIn(313);
        }

        //  Create a Temporary Hitpoints effect.
        //  - nHitPoints: a positive integer
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nHitPoints < 0.
        public static Effect EffectTemporaryHitpoints(int nHitPoints)
        {
            Internal.StackPushInteger(nHitPoints);
            Internal.CallBuiltIn(314);
            return Internal.StackPopEffect();
        }

        //  Get the number of ranks that oTarget has in nSkill.
        //  - nSkill: SKILL_*
        //  - oTarget
        //  - nBaseSkillRank: if set to true returns the number of base skill ranks the target
        //                    has (i.e. not including any bonuses from ability scores, feats, etc).
        //  * Returns -1 if oTarget doesn't have nSkill.
        //  * Returns 0 if nSkill is untrained.
        public static int GetSkillRank(Skill nSkill, NWGameObject oTarget = null, bool nBaseSkillRank = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nBaseSkillRank));
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(315);
            return Internal.StackPopInteger();
        }

        //  Get the attack target of oCreature.
        //  This only works when oCreature is in combat.
        public static NWGameObject GetAttackTarget(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(316);
            return Internal.StackPopObject();
        }

        //  Get the attack type (SPECIAL_ATTACK_*) of oCreature's last attack.
        //  This only works when oCreature is in combat.
        public static SpecialAttack GetLastAttackType(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(317);
            return (SpecialAttack)Internal.StackPopInteger();
        }

        //  Get the attack mode (COMBAT_MODE_*) of oCreature's last attack.
        //  This only works when oCreature is in combat.
        public static CombatMode GetLastAttackMode(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(318);
            return (CombatMode)Internal.StackPopInteger();
        }

        //  Get the master of oAssociate.
        public static NWGameObject GetMaster(NWGameObject oAssociate = null)
        {
            Internal.StackPushObject(oAssociate, false);
            Internal.CallBuiltIn(319);
            return Internal.StackPopObject();
        }

        //  * Returns TRUE if oCreature is in combat.
        public static bool GetIsInCombat(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(320);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the last command (ASSOCIATE_COMMAND_*) issued to oAssociate.
        public static AssociateCommand GetLastAssociateCommand(NWGameObject oAssociate = null)
        {
            Internal.StackPushObject(oAssociate, false);
            Internal.CallBuiltIn(321);
            return (AssociateCommand)Internal.StackPopInteger();
        }

        //  Give nGP gold to oCreature.
        public static void GiveGoldToCreature(NWGameObject oCreature, int nGP)
        {
            Internal.StackPushInteger(nGP);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(322);
        }

        //  Set the destroyable status of the caller.
        //  - bDestroyable: If this is FALSE, the caller does not fade out on death, but
        //    sticks around as a corpse.
        //  - bRaiseable: If this is TRUE, the caller can be raised via resurrection.
        //  - bSelectableWhenDead: If this is TRUE, the caller is selectable after death.
        public static void SetIsDestroyable(bool bDestroyable, bool bRaiseable = true, bool bSelectableWhenDead = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bSelectableWhenDead));
            Internal.StackPushInteger(Convert.ToInt32(bRaiseable));
            Internal.StackPushInteger(Convert.ToInt32(bDestroyable));
            Internal.CallBuiltIn(323);
        }

        //  Set the locked state of oTarget, which can be a door or a placeable object.
        public static void SetLocked(NWGameObject oTarget, bool bLocked)
        {
            Internal.StackPushInteger(Convert.ToInt32(bLocked));
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(324);
        }

        //  Get the locked state of oTarget, which can be a door or a placeable object.
        public static bool GetLocked(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(325);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in a trigger's OnClick event script to get the object that last
        //  clicked on it.
        //  This is identical to GetEnteringObject.
        //  GetClickingObject() should not be called from a placeable's OnClick event,
        //  instead use GetPlaceableLastClickedBy();
        public static NWGameObject GetClickingObject()
        {
            Internal.CallBuiltIn(326);
            return Internal.StackPopObject();
        }

        //  Initialise oTarget to listen for the standard Associates commands.
        public static void SetAssociateListenPatterns(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(327);
        }

        //  Get the last weapon that oCreature used in an attack.
        //  * Returns OBJECT_INVALID if oCreature did not attack, or has no weapon equipped.
        public static NWGameObject GetLastWeaponUsed(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(328);
            return Internal.StackPopObject();
        }

        //  Use oPlaceable.
        public static void ActionInteractObject(NWGameObject oPlaceable)
        {
            Internal.StackPushObject(oPlaceable, false);
            Internal.CallBuiltIn(329);
        }

        //  Get the last object that used the placeable object that is calling this function.
        //  * Returns OBJECT_INVALID if it is called by something other than a placeable or
        //    a door.
        public static NWGameObject GetLastUsedBy()
        {
            Internal.CallBuiltIn(330);
            return Internal.StackPopObject();
        }

        //  Returns the ability modifier for the specified ability
        //  Get oCreature's ability modifier for nAbility.
        //  - nAbility: ABILITY_*
        //  - oCreature
        public static int GetAbilityModifier(Ability nAbility, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nAbility);
            Internal.CallBuiltIn(331);
            return Internal.StackPopInteger();
        }

        //  Determined whether oItem has been identified.
        public static bool GetIdentified(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(332);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Set whether oItem has been identified.
        public static void SetIdentified(NWGameObject oItem, bool bIdentified)
        {
            Internal.StackPushInteger(Convert.ToInt32(bIdentified));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(333);
        }

        //  Summon an Animal Companion
        public static void SummonAnimalCompanion(NWGameObject oMaster = null)
        {
            Internal.StackPushObject(oMaster, false);
            Internal.CallBuiltIn(334);
        }

        //  Summon a Familiar
        public static void SummonFamiliar(NWGameObject oMaster = null)
        {
            Internal.StackPushObject(oMaster, false);
            Internal.CallBuiltIn(335);
        }

        //  Get the last blocking door encountered by the caller of this function.
        //  * Returns OBJECT_INVALID if the caller is not a valid creature.
        public static NWGameObject GetBlockingDoor()
        {
            Internal.CallBuiltIn(336);
            return Internal.StackPopObject();
        }

        //  - oTargetDoor
        //  - nDoorAction: DOOR_ACTION_*
        //  * Returns TRUE if nDoorAction can be performed on oTargetDoor.
        public static bool GetIsDoorActionPossible(NWGameObject oTargetDoor, DoorAction nDoorAction)
        {
            Internal.StackPushInteger((int)nDoorAction);
            Internal.StackPushObject(oTargetDoor, false);
            Internal.CallBuiltIn(337);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Perform nDoorAction on oTargetDoor.
        public static void DoDoorAction(NWGameObject oTargetDoor, DoorAction nDoorAction)
        {
            Internal.StackPushInteger((int)nDoorAction);
            Internal.StackPushObject(oTargetDoor, false);
            Internal.CallBuiltIn(338);
        }

        //  Get the first item in oTarget's inventory (start to cycle through oTarget's
        //  inventory).
        //  * Returns OBJECT_INVALID if the caller is not a creature, item, placeable or store,
        //    or if no item is found.
        public static NWGameObject GetFirstItemInInventory(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(339);
            return Internal.StackPopObject();
        }

        //  Get the next item in oTarget's inventory (continue to cycle through oTarget's
        //  inventory).
        //  * Returns OBJECT_INVALID if the caller is not a creature, item, placeable or store,
        //    or if no item is found.
        public static NWGameObject GetNextItemInInventory(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(340);
            return Internal.StackPopObject();
        }

        /// <summary>
        /// A creature can have up to three classes.  This function determines the
        ///  creature's class (CLASS_TYPE_*) based on nClassPosition.
        ///  - nClassPosition: 1, 2 or 3
        ///  - oCreature
        ///  * Returns CLASS_TYPE_INVALID if the oCreature does not have a class in
        ///    nClassPosition (i.e. a single-class creature will only have a value in
        ///    nClassLocation=1) or if oCreature is not a valid creature.
        /// </summary>
        /// <param name="nClassPosition">The position of the class</param>
        /// <param name="oCreature">The creature to use</param>
        /// <returns></returns>
        public static ClassType GetClassByPosition(ClassPosition nClassPosition, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nClassPosition);
            Internal.CallBuiltIn(341);
            return (ClassType)Internal.StackPopInteger();
        }

        //  A creature can have up to three classes.  This function determines the
        //  creature's class level based on nClass Position.
        //  - nClassPosition: 1, 2 or 3
        //  - oCreature
        //  * Returns 0 if oCreature does not have a class in nClassPosition
        //    (i.e. a single-class creature will only have a value in nClassLocation=1)
        //    or if oCreature is not a valid creature.
        public static int GetLevelByPosition(ClassPosition nClassPosition, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nClassPosition);
            Internal.CallBuiltIn(342);
            return Internal.StackPopInteger();
        }

        /// <summary>
        /// Returns the total level of a creature by adding up to three of their classes together.
        /// Returns 0 if there's an error.
        /// </summary>
        /// <param name="creature">The creature to sum levels up for</param>
        /// <returns></returns>
        public static int GetTotalLevel(NWGameObject creature)
        {
            return GetLevelByPosition(ClassPosition.First, creature) +
                GetLevelByPosition(ClassPosition.Second, creature) +
                GetLevelByPosition(ClassPosition.Third, creature);
        }

        //  Determine the levels that oCreature holds in nClassType.
        //  - nClassType: CLASS_TYPE_*
        //  - oCreature
        public static int GetLevelByClass(ClassType nClassType, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nClassType);
            Internal.CallBuiltIn(343);
            return Internal.StackPopInteger();
        }

        //  Get the amount of damage of type nDamageType that has been dealt to the caller.
        //  - nDamageType: DAMAGE_TYPE_*
        public static int GetDamageDealtByType(DamageType nDamageType)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(344);
            return Internal.StackPopInteger();
        }

        //  Get the total amount of damage that has been dealt to the caller.
        public static int GetTotalDamageDealt()
        {
            Internal.CallBuiltIn(345);
            return Internal.StackPopInteger();
        }

        //  Get the last object that damaged oObject
        //  * Returns OBJECT_INVALID if the passed in object is not a valid object.
        public static NWGameObject GetLastDamager(NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(346);
            return Internal.StackPopObject();
        }

        //  Get the last object that disarmed the trap on the caller.
        //  * Returns OBJECT_INVALID if the caller is not a valid placeable, trigger or
        //    door.
        public static NWGameObject GetLastDisarmed()
        {
            Internal.CallBuiltIn(347);
            return Internal.StackPopObject();
        }

        //  Get the last object that disturbed the inventory of the caller.
        //  * Returns OBJECT_INVALID if the caller is not a valid creature or placeable.
        public static NWGameObject GetLastDisturbed()
        {
            Internal.CallBuiltIn(348);
            return Internal.StackPopObject();
        }

        //  Get the last object that locked the caller.
        //  * Returns OBJECT_INVALID if the caller is not a valid door or placeable.
        public static NWGameObject GetLastLocked()
        {
            Internal.CallBuiltIn(349);
            return Internal.StackPopObject();
        }

        //  Get the last object that unlocked the caller.
        //  * Returns OBJECT_INVALID if the caller is not a valid door or placeable.
        public static NWGameObject GetLastUnlocked()
        {
            Internal.CallBuiltIn(350);
            return Internal.StackPopObject();
        }

        //  Create a Skill Increase effect.
        //  - nSkill: SKILL_*
        //  - nValue
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nSkill is invalid.
        public static Effect EffectSkillIncrease(Skill nSkill, int nValue)
        {
            Internal.StackPushInteger(nValue);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(351);
            return Internal.StackPopEffect();
        }

        //  Get the type of disturbance (INVENTORY_DISTURB_*) that caused the caller's
        //  OnInventoryDisturbed script to fire.  This will only work for creatures and
        //  placeables.
        public static InventoryDisturbType GetInventoryDisturbType()
        {
            Internal.CallBuiltIn(352);
            return (InventoryDisturbType)Internal.StackPopInteger();
        }

        //  get the item that caused the caller's OnInventoryDisturbed script to fire.
        //  * Returns OBJECT_INVALID if the caller is not a valid object.
        public static NWGameObject GetInventoryDisturbItem()
        {
            Internal.CallBuiltIn(353);
            return Internal.StackPopObject();
        }

        //  Get the henchman belonging to oMaster.
        //  * Return OBJECT_INVALID if oMaster does not have a henchman.
        //  -nNth: Which henchman to return.
        public static NWGameObject GetHenchman(NWGameObject oMaster = null, int nNth = 1)
        {
            Internal.StackPushInteger(nNth);
            Internal.StackPushObject(oMaster, false);
            Internal.CallBuiltIn(354);
            return Internal.StackPopObject();
        }

        //  Set eEffect to be versus a specific alignment.
        //  - eEffect
        //  - nLawChaos: ALIGNMENT_LAWFUL/ALIGNMENT_CHAOTIC/ALIGNMENT_ALL
        //  - nGoodEvil: ALIGNMENT_GOOD/ALIGNMENT_EVIL/ALIGNMENT_ALL
        public static Effect VersusAlignmentEffect(Effect eEffect, Alignment nLawChaos = Alignment.All, Alignment nGoodEvil = Alignment.All)
        {
            Internal.StackPushInteger((int)nGoodEvil);
            Internal.StackPushInteger((int)nLawChaos);
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(355);
            return Internal.StackPopEffect();
        }

        //  Set eEffect to be versus nRacialType.
        //  - eEffect
        //  - nRacialType: RACIAL_TYPE_*
        public static Effect VersusRacialTypeEffect(Effect eEffect, RacialType nRacialType)
        {
            Internal.StackPushInteger((int)nRacialType);
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(356);
            return Internal.StackPopEffect();
        }

        //  Set eEffect to be versus traps.
        public static Effect VersusTrapEffect(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(357);
            return Internal.StackPopEffect();
        }

        //  Get the gender of oCreature.
        public static Gender GetGender(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(358);
            return (Gender)Internal.StackPopInteger();
        }

        //  * Returns TRUE if tTalent is valid.
        public static bool GetIsTalentValid(Talent tTalent)
        {
            Internal.StackPushTalent(tTalent);
            Internal.CallBuiltIn(359);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Causes the action subject to move away from lMoveAwayFrom.
        public static void ActionMoveAwayFromLocation(Location lMoveAwayFrom, bool bRun = false, float fMoveAwayRange = 40.0f)
        {
            Internal.StackPushFloat(fMoveAwayRange);
            Internal.StackPushInteger(Convert.ToInt32(bRun));
            Internal.StackPushLocation(lMoveAwayFrom);
            Internal.CallBuiltIn(360);
        }

        //  Get the target that the caller attempted to attack - this should be used in
        //  conjunction with GetAttackTarget(). This value is set every time an attack is
        //  made, and is reset at the end of combat.
        //  * Returns OBJECT_INVALID if the caller is not a valid creature.
        public static NWGameObject GetAttemptedAttackTarget()
        {
            Internal.CallBuiltIn(361);
            return Internal.StackPopObject();
        }

        //  Get the type (TALENT_TYPE_*) of tTalent.
        public static TalentType GetTypeFromTalent(Talent tTalent)
        {
            Internal.StackPushTalent(tTalent);
            Internal.CallBuiltIn(362);
            return (TalentType)Internal.StackPopInteger();
        }

        //  Get the ID of tTalent.  This could be a SPELL_*, FEAT_* or SKILL_*.
        public static int GetIdFromTalent(Talent tTalent)
        {
            Internal.StackPushTalent(tTalent);
            Internal.CallBuiltIn(363);
            return Internal.StackPopInteger();
        }

        //  Get the associate of type nAssociateType belonging to oMaster.
        //  - nAssociateType: ASSOCIATE_TYPE_*
        //  - nMaster
        //  - nTh: Which associate of the specified type to return
        //  * Returns OBJECT_INVALID if no such associate exists.
        public static NWGameObject GetAssociate(AssociateType nAssociateType, NWGameObject oMaster = null, int nTh = 1)
        {
            Internal.StackPushInteger(nTh);
            Internal.StackPushObject(oMaster, false);
            Internal.StackPushInteger((int)nAssociateType);
            Internal.CallBuiltIn(364);
            return Internal.StackPopObject();
        }

        //  Add oHenchman as a henchman to oMaster
        //  If oHenchman is either a DM or a player character, this will have no effect.
        public static void AddHenchman(NWGameObject oMaster, NWGameObject oHenchman = null)
        {
            Internal.StackPushObject(oHenchman, false);
            Internal.StackPushObject(oMaster, false);
            Internal.CallBuiltIn(365);
        }

        //  Remove oHenchman from the service of oMaster, returning them to their original faction.
        public static void RemoveHenchman(NWGameObject oMaster, NWGameObject oHenchman = null)
        {
            Internal.StackPushObject(oHenchman, false);
            Internal.StackPushObject(oMaster, false);
            Internal.CallBuiltIn(366);
        }

        //  Add a journal quest entry to oCreature.
        //  - szPlotID: the plot identifier used in the toolset's Journal Editor
        //  - nState: the state of the plot as seen in the toolset's Journal Editor
        //  - oCreature
        //  - bAllPartyMembers: If this is TRUE, the entry will show up in the journal of
        //    everyone in the party
        //  - bAllPlayers: If this is TRUE, the entry will show up in the journal of
        //    everyone in the world
        //  - bAllowOverrideHigher: If this is TRUE, you can set the state to a lower
        //    number than the one it is currently on
        public static void AddJournalQuestEntry(string szPlotID, int nState, NWGameObject oCreature, bool bAllPartyMembers = true, bool bAllPlayers = false, bool bAllowOverrideHigher = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bAllowOverrideHigher));
            Internal.StackPushInteger(Convert.ToInt32(bAllPlayers));
            Internal.StackPushInteger(Convert.ToInt32(bAllPartyMembers));
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger(nState);
            Internal.StackPushString(szPlotID);
            Internal.CallBuiltIn(367);
        }

        //  Remove a journal quest entry from oCreature.
        //  - szPlotID: the plot identifier used in the toolset's Journal Editor
        //  - oCreature
        //  - bAllPartyMembers: If this is TRUE, the entry will be removed from the
        //    journal of everyone in the party
        //  - bAllPlayers: If this is TRUE, the entry will be removed from the journal of
        //    everyone in the world
        public static void RemoveJournalQuestEntry(string szPlotID, NWGameObject oCreature, bool bAllPartyMembers = true, bool bAllPlayers = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bAllPlayers));
            Internal.StackPushInteger(Convert.ToInt32(bAllPartyMembers));
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushString(szPlotID);
            Internal.CallBuiltIn(368);
        }

        //  Get the public part of the CD Key that oPlayer used when logging in.
        //  - nSinglePlayerCDKey: If set to TRUE, the player's public CD Key will 
        //    be returned when the player is playing in single player mode 
        //    (otherwise returns an empty string in single player mode).
        public static string GetPCPublicCDKey(NWGameObject oPlayer, bool nSinglePlayerCDKey = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nSinglePlayerCDKey));
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(369);
            return Internal.StackPopString();
        }

        //  Get the IP address from which oPlayer has connected.
        public static string GetPCIPAddress(NWGameObject oPlayer)
        {
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(370);
            return Internal.StackPopString();
        }

        //  Get the name of oPlayer.
        public static string GetPCPlayerName(NWGameObject oPlayer)
        {
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(371);
            return Internal.StackPopString();
        }

        //  Sets oPlayer and oTarget to like each other.
        public static void SetPCLike(NWGameObject oPlayer, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(372);
        }

        //  Sets oPlayer and oTarget to dislike each other.
        public static void SetPCDislike(NWGameObject oPlayer, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(373);
        }

        //  Send a server message (szMessage) to the oPlayer.
        public static void SendMessageToPC(NWGameObject oPlayer, string szMessage)
        {
            Internal.StackPushString(szMessage);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(374);
        }

        //  Get the target at which the caller attempted to cast a spell.
        //  This value is set every time a spell is cast and is reset at the end of
        //  combat.
        //  * Returns OBJECT_INVALID if the caller is not a valid creature.
        public static NWGameObject GetAttemptedSpellTarget()
        {
            Internal.CallBuiltIn(375);
            return Internal.StackPopObject();
        }

        //  Get the last creature that opened the caller.
        //  * Returns OBJECT_INVALID if the caller is not a valid door, placeable or store.
        public static NWGameObject GetLastOpenedBy()
        {
            Internal.CallBuiltIn(376);
            return Internal.StackPopObject();
        }

        //  Determines the number of times that oCreature has nSpell memorised.
        //  - nSpell: SPELL_*
        //  - oCreature
        public static bool GetHasSpell(Spell nSpell, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(377);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Open oStore for oPC.
        //  - nBonusMarkUp is added to the stores default mark up percentage on items sold (-100 to 100)
        //  - nBonusMarkDown is added to the stores default mark down percentage on items bought (-100 to 100)
        public static void OpenStore(NWGameObject oStore, NWGameObject oPC, int nBonusMarkUp = 0, int nBonusMarkDown = 0)
        {
            Internal.StackPushInteger(nBonusMarkDown);
            Internal.StackPushInteger(nBonusMarkUp);
            Internal.StackPushObject(oPC, false);
            Internal.StackPushObject(oStore, false);
            Internal.CallBuiltIn(378);
        }

        //  Create a Turned effect.
        //  Turned effects are supernatural by default.
        public static Effect EffectTurned()
        {
            Internal.CallBuiltIn(379);
            return Internal.StackPopEffect();
        }

        //  Get the first member of oMemberOfFaction's faction (start to cycle through
        //  oMemberOfFaction's faction).
        //  * Returns OBJECT_INVALID if oMemberOfFaction's faction is invalid.
        public static NWGameObject GetFirstFactionMember(NWGameObject oMemberOfFaction, bool bPCOnly = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bPCOnly));
            Internal.StackPushObject(oMemberOfFaction, false);
            Internal.CallBuiltIn(380);
            return Internal.StackPopObject();
        }

        //  Get the next member of oMemberOfFaction's faction (continue to cycle through
        //  oMemberOfFaction's faction).
        //  * Returns OBJECT_INVALID if oMemberOfFaction's faction is invalid.
        public static NWGameObject GetNextFactionMember(NWGameObject oMemberOfFaction, bool bPCOnly = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bPCOnly));
            Internal.StackPushObject(oMemberOfFaction, false);
            Internal.CallBuiltIn(381);
            return Internal.StackPopObject();
        }

        //  Force the action subject to move to lDestination.
        public static void ActionForceMoveToLocation(Location lDestination, bool bRun = false, float fTimeout = 30.0f)
        {
            Internal.StackPushFloat(fTimeout);
            Internal.StackPushInteger(Convert.ToInt32(bRun));
            Internal.StackPushLocation(lDestination);
            Internal.CallBuiltIn(382);
        }

        //  Force the action subject to move to oMoveTo.
        public static void ActionForceMoveToObject(NWGameObject oMoveTo, bool bRun = false, float fRange = 1.0f, float fTimeout = 30.0f)
        {
            Internal.StackPushFloat(fTimeout);
            Internal.StackPushFloat(fRange);
            Internal.StackPushInteger(Convert.ToInt32(bRun));
            Internal.StackPushObject(oMoveTo, false);
            Internal.CallBuiltIn(383);
        }

        //  Get the experience assigned in the journal editor for szPlotID.
        public static int GetJournalQuestExperience(string szPlotID)
        {
            Internal.StackPushString(szPlotID);
            Internal.CallBuiltIn(384);
            return Internal.StackPopInteger();
        }

        //  Jump to oToJumpTo (the action is added to the top of the action queue).
        public static void JumpToObject(NWGameObject oToJumpTo, int nWalkStraightLineToPoint = 1)
        {
            Internal.StackPushInteger(nWalkStraightLineToPoint);
            Internal.StackPushObject(oToJumpTo, false);
            Internal.CallBuiltIn(385);
        }

        //  Set whether oMapPin is enabled.
        //  - oMapPin
        //  - nEnabled: 0=Off, 1=On
        public static void SetMapPinEnabled(NWGameObject oMapPin, bool nEnabled)
        {
            Internal.StackPushInteger(Convert.ToInt32(nEnabled));
            Internal.StackPushObject(oMapPin, false);
            Internal.CallBuiltIn(386);
        }

        //  Create a Hit Point Change When Dying effect.
        //  - fHitPointChangePerRound: this can be positive or negative, but not zero.
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if fHitPointChangePerRound is 0.
        public static Effect EffectHitPointChangeWhenDying(float fHitPointChangePerRound)
        {
            Internal.StackPushFloat(fHitPointChangePerRound);
            Internal.CallBuiltIn(387);
            return Internal.StackPopEffect();
        }

        //  Spawn a GUI panel for the client that controls oPC.
        //  - oPC
        //  - nGUIPanel: GUI_PANEL_*
        //  * Nothing happens if oPC is not a player character or if an invalid value is
        //    used for nGUIPanel.
        public static void PopUpGUIPanel(NWGameObject oPC, GuiPanel nGUIPanel)
        {
            Internal.StackPushInteger((int)nGUIPanel);
            Internal.StackPushObject(oPC, false);
            Internal.CallBuiltIn(388);
        }

        //  Clear all personal feelings that oSource has about oTarget.
        public static void ClearPersonalReputation(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(389);
        }

        //  oSource will temporarily be friends towards oTarget.
        //  bDecays determines whether the personal reputation value decays over time
        //  fDurationInSeconds is the length of time that the temporary friendship lasts
        //  Make oSource into a temporary friend of oTarget using personal reputation.
        //  - oTarget
        //  - oSource
        //  - bDecays: If this is TRUE, the friendship decays over fDurationInSeconds;
        //    otherwise it is indefinite.
        //  - fDurationInSeconds: This is only used if bDecays is TRUE, it is how long
        //    the friendship lasts.
        //  Note: If bDecays is TRUE, the personal reputation amount decreases in size
        //  over fDurationInSeconds. Friendship will only be in effect as long as
        //  (faction reputation + total personal reputation) >= REPUTATION_TYPE_FRIEND.
        public static void SetIsTemporaryFriend(NWGameObject oTarget, NWGameObject oSource = null, bool bDecays = false, float fDurationInSeconds = 180.0f)
        {
            Internal.StackPushFloat(fDurationInSeconds);
            Internal.StackPushInteger(Convert.ToInt32(bDecays));
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(390);
        }

        //  Make oSource into a temporary enemy of oTarget using personal reputation.
        //  - oTarget
        //  - oSource
        //  - bDecays: If this is TRUE, the enmity decays over fDurationInSeconds;
        //    otherwise it is indefinite.
        //  - fDurationInSeconds: This is only used if bDecays is TRUE, it is how long
        //    the enmity lasts.
        //  Note: If bDecays is TRUE, the personal reputation amount decreases in size
        //  over fDurationInSeconds. Enmity will only be in effect as long as
        //  (faction reputation + total personal reputation) <= REPUTATION_TYPE_ENEMY.
        public static void SetIsTemporaryEnemy(NWGameObject oTarget, NWGameObject oSource = null, bool bDecays = false, float fDurationInSeconds = 180.0f)
        {
            Internal.StackPushFloat(fDurationInSeconds);
            Internal.StackPushInteger(Convert.ToInt32(bDecays));
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(391);
        }

        //  Make oSource temporarily neutral to oTarget using personal reputation.
        //  - oTarget
        //  - oSource
        //  - bDecays: If this is TRUE, the neutrality decays over fDurationInSeconds;
        //    otherwise it is indefinite.
        //  - fDurationInSeconds: This is only used if bDecays is TRUE, it is how long
        //    the neutrality lasts.
        //  Note: If bDecays is TRUE, the personal reputation amount decreases in size
        //  over fDurationInSeconds. Neutrality will only be in effect as long as
        //  (faction reputation + total personal reputation) > REPUTATION_TYPE_ENEMY and
        //  (faction reputation + total personal reputation) < REPUTATION_TYPE_FRIEND.
        public static void SetIsTemporaryNeutral(NWGameObject oTarget, NWGameObject oSource = null, bool bDecays = false, float fDurationInSeconds = 180.0f)
        {
            Internal.StackPushFloat(fDurationInSeconds);
            Internal.StackPushInteger(Convert.ToInt32(bDecays));
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(392);
        }

        //  Gives nXpAmount to oCreature.
        public static void GiveXPToCreature(NWGameObject oCreature, int nXpAmount)
        {
            Internal.StackPushInteger(nXpAmount);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(393);
        }

        //  Sets oCreature's experience to nXpAmount.
        public static void SetXP(NWGameObject oCreature, int nXpAmount)
        {
            Internal.StackPushInteger(nXpAmount);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(394);
        }

        //  Get oCreature's experience.
        public static int GetXP(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(395);
            return Internal.StackPopInteger();
        }

        //  Get the base item type (BASE_ITEM_*) of oItem.
        //  * Returns BASE_ITEM_INVALID if oItem is an invalid item.
        public static BaseItemType GetBaseItemType(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(397);
            return (BaseItemType)Internal.StackPopInteger();
        }

        //  Determines whether oItem has nProperty.
        //  - oItem
        //  - nProperty: ITEM_PROPERTY_*
        //  * Returns FALSE if oItem is not a valid item, or if oItem does not have
        //    nProperty.
        public static bool GetItemHasItemProperty(NWGameObject oItem, ItemPropertyType nProperty)
        {
            Internal.StackPushInteger((int)nProperty);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(398);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  The creature will equip the melee weapon in its possession that can do the
        //  most damage. If no valid melee weapon is found, it will equip the most
        //  damaging range weapon. This function should only ever be called in the
        //  EndOfCombatRound scripts, because otherwise it would have to stop the combat
        //  round to run simulation.
        //  - oVersus: You can try to get the most damaging weapon against oVersus
        //  - bOffHand
        public static void ActionEquipMostDamagingMelee(NWGameObject oVersus = null, bool bOffHand = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bOffHand));
            Internal.StackPushObject(oVersus, false);
            Internal.CallBuiltIn(399);
        }

        //  The creature will equip the range weapon in its possession that can do the
        //  most damage.
        //  If no valid range weapon can be found, it will equip the most damaging melee
        //  weapon.
        //  - oVersus: You can try to get the most damaging weapon against oVersus
        public static void ActionEquipMostDamagingRanged(NWGameObject oVersus = null)
        {
            Internal.StackPushObject(oVersus, false);
            Internal.CallBuiltIn(400);
        }

        //  Get the Armour Class of oItem.
        //  * Return 0 if the oItem is not a valid item, or if oItem has no armour value.
        public static int GetItemACValue(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(401);
            return Internal.StackPopInteger();
        }

        //  The creature will rest if not in combat and no enemies are nearby.
        //  - bCreatureToEnemyLineOfSightCheck: TRUE to allow the creature to rest if enemies
        //                                      are nearby, but the creature can't see the enemy.
        //                                      FALSE the creature will not rest if enemies are
        //                                      nearby regardless of whether or not the creature
        //                                      can see them, such as if an enemy is close by,
        //                                      but is in a different room behind a closed door.
        public static void ActionRest(bool bCreatureToEnemyLineOfSightCheck = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bCreatureToEnemyLineOfSightCheck));
            Internal.CallBuiltIn(402);
        }

        //  Expose/Hide the entire map of oArea for oPlayer.
        //  - oArea: The area that the map will be exposed/hidden for.
        //  - oPlayer: The player the map will be exposed/hidden for.
        //  - bExplored: TRUE/FALSE. Whether the map should be completely explored or hidden.
        public static void ExploreAreaForPlayer(NWGameObject oArea, NWGameObject oPlayer, bool bExplored = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bExplored));
            Internal.StackPushObject(oPlayer, false);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(403);
        }

        //  The creature will equip the armour in its possession that has the highest
        //  armour class.
        public static void ActionEquipMostEffectiveArmor()
        {
            Internal.CallBuiltIn(404);
        }

        //  * Returns TRUE if it is currently day.
        public static bool GetIsDay()
        {
            Internal.CallBuiltIn(405);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  * Returns TRUE if it is currently night.
        public static bool GetIsNight()
        {
            Internal.CallBuiltIn(406);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  * Returns TRUE if it is currently dawn.
        public static bool GetIsDawn()
        {
            Internal.CallBuiltIn(407);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  * Returns TRUE if it is currently dusk.
        public static bool GetIsDusk()
        {
            Internal.CallBuiltIn(408);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  * Returns TRUE if oCreature was spawned from an encounter.
        public static bool GetIsEncounterCreature(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(409);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in an OnPlayerDying module script to get the last player who is dying.
        public static NWGameObject GetLastPlayerDying()
        {
            Internal.CallBuiltIn(410);
            return Internal.StackPopObject();
        }

        //  Get the starting location of the module.
        public static Location GetStartingLocation()
        {
            Internal.CallBuiltIn(411);
            return Internal.StackPopLocation();
        }

        //  Make oCreatureToChange join one of the standard factions.
        //  ** This will only work on an NPC **
        //  - nStandardFaction: STANDARD_FACTION_*
        public static void ChangeToStandardFaction(NWGameObject oCreatureToChange, StandardFaction nStandardFaction)
        {
            Internal.StackPushInteger((int)nStandardFaction);
            Internal.StackPushObject(oCreatureToChange, false);
            Internal.CallBuiltIn(412);
        }

        //  Play oSound.
        public static void SoundObjectPlay(NWGameObject oSound)
        {
            Internal.StackPushObject(oSound, false);
            Internal.CallBuiltIn(413);
        }

        //  Stop playing oSound.
        public static void SoundObjectStop(NWGameObject oSound)
        {
            Internal.StackPushObject(oSound, false);
            Internal.CallBuiltIn(414);
        }

        //  Set the volume of oSound.
        //  - oSound
        //  - nVolume: 0-127
        public static void SoundObjectSetVolume(NWGameObject oSound, int nVolume)
        {
            Internal.StackPushInteger(nVolume);
            Internal.StackPushObject(oSound, false);
            Internal.CallBuiltIn(415);
        }

        //  Set the position of oSound.
        public static void SoundObjectSetPosition(NWGameObject oSound, Vector? vPosition)
        {
            Internal.StackPushVector(vPosition);
            Internal.StackPushObject(oSound, false);
            Internal.CallBuiltIn(416);
        }

        //  Immediately speak a conversation one-liner.
        //  - sDialogResRef
        //  - oTokenTarget: This must be specified if there are creature-specific tokens
        //    in the string.
        public static void SpeakOneLinerConversation(string sDialogResRef = "", NWGameObject oTokenTarget = null)
        {
            Internal.StackPushObject(oTokenTarget, false);
            Internal.StackPushString(sDialogResRef);
            Internal.CallBuiltIn(417);
        }

        //  Get the amount of gold possessed by oTarget.
        public static int GetGold(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(418);
            return Internal.StackPopInteger();
        }

        //  Use this in an OnRespawnButtonPressed module script to get the object id of
        //  the player who last pressed the respawn button.
        public static NWGameObject GetLastRespawnButtonPresser()
        {
            Internal.CallBuiltIn(419);
            return Internal.StackPopObject();
        }

        //  * Returns TRUE if oCreature is the Dungeon Master.
        //  Note: This will return FALSE if oCreature is a DM Possessed creature.
        //  To determine if oCreature is a DM Possessed creature, use GetIsDMPossessed()
        private static bool GetIsDM(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(420);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Play a voice chat.
        //  - nVoiceChatID: VOICE_CHAT_*
        //  - oTarget
        public static void PlayVoiceChat(VoiceChat nVoiceChatID, NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nVoiceChatID);
            Internal.CallBuiltIn(421);
        }

        //  * Returns TRUE if the weapon equipped is capable of damaging oVersus.
        public static bool GetIsWeaponEffective(NWGameObject oVersus = null, bool bOffHand = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bOffHand));
            Internal.StackPushObject(oVersus, false);
            Internal.CallBuiltIn(422);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Use this in a SpellCast script to determine whether the spell was considered
        //  harmful.
        //  * Returns TRUE if the last spell cast was harmful.
        public static bool GetLastSpellHarmful()
        {
            Internal.CallBuiltIn(423);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Activate oItem.
        public static Event EventActivateItem(NWGameObject oItem, Location lTarget, NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushLocation(lTarget);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(424);
            return Internal.StackPopEvent();
        }

        //  Play the background music for oArea.
        public static void MusicBackgroundPlay(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(425);
        }

        //  Stop the background music for oArea.
        public static void MusicBackgroundStop(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(426);
        }

        //  Set the delay for the background music for oArea.
        //  - oArea
        //  - nDelay: delay in milliseconds
        public static void MusicBackgroundSetDelay(NWGameObject oArea, int nDelay)
        {
            Internal.StackPushInteger(nDelay);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(427);
        }

        //  Change the background day track for oArea to nTrack.
        //  - oArea
        //  - nTrack
        public static void MusicBackgroundChangeDay(NWGameObject oArea, int nTrack)
        {
            Internal.StackPushInteger(nTrack);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(428);
        }

        //  Change the background night track for oArea to nTrack.
        //  - oArea
        //  - nTrack
        public static void MusicBackgroundChangeNight(NWGameObject oArea, int nTrack)
        {
            Internal.StackPushInteger(nTrack);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(429);
        }

        //  Play the battle music for oArea.
        public static void MusicBattlePlay(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(430);
        }

        //  Stop the battle music for oArea.
        public static void MusicBattleStop(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(431);
        }

        //  Change the battle track for oArea.
        //  - oArea
        //  - nTrack
        public static void MusicBattleChange(NWGameObject oArea, int nTrack)
        {
            Internal.StackPushInteger(nTrack);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(432);
        }

        //  Play the ambient sound for oArea.
        public static void AmbientSoundPlay(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(433);
        }

        //  Stop the ambient sound for oArea.
        public static void AmbientSoundStop(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(434);
        }

        //  Change the ambient day track for oArea to nTrack.
        //  - oArea
        //  - nTrack
        public static void AmbientSoundChangeDay(NWGameObject oArea, int nTrack)
        {
            Internal.StackPushInteger(nTrack);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(435);
        }

        //  Change the ambient night track for oArea to nTrack.
        //  - oArea
        //  - nTrack
        public static void AmbientSoundChangeNight(NWGameObject oArea, int nTrack)
        {
            Internal.StackPushInteger(nTrack);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(436);
        }

        //  Get the object that killed the caller.
        public static NWGameObject GetLastKiller()
        {
            Internal.CallBuiltIn(437);
            return Internal.StackPopObject();
        }

        //  Use this in a spell script to get the item used to cast the spell.
        public static NWGameObject GetSpellCastItem()
        {
            Internal.CallBuiltIn(438);
            return Internal.StackPopObject();
        }

        //  Use this in an OnItemActivated module script to get the item that was activated.
        public static NWGameObject GetItemActivated()
        {
            Internal.CallBuiltIn(439);
            return Internal.StackPopObject();
        }

        //  Use this in an OnItemActivated module script to get the creature that
        //  activated the item.
        public static NWGameObject GetItemActivator()
        {
            Internal.CallBuiltIn(440);
            return Internal.StackPopObject();
        }

        //  Use this in an OnItemActivated module script to get the location of the item's
        //  target.
        public static Location GetItemActivatedTargetLocation()
        {
            Internal.CallBuiltIn(441);
            return Internal.StackPopLocation();
        }

        //  Use this in an OnItemActivated module script to get the item's target.
        public static NWGameObject GetItemActivatedTarget()
        {
            Internal.CallBuiltIn(442);
            return Internal.StackPopObject();
        }

        //  * Returns TRUE if oObject (which is a placeable or a door) is currently open.
        public static bool GetIsOpen(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(443);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Take nAmount of gold from oCreatureToTakeFrom.
        //  - nAmount
        //  - oCreatureToTakeFrom: If this is not a valid creature, nothing will happen.
        //  - bDestroy: If this is TRUE, the caller will not get the gold.  Instead, the
        //    gold will be destroyed and will vanish from the game.
        public static void TakeGoldFromCreature(int nAmount, NWGameObject oCreatureToTakeFrom, bool bDestroy = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bDestroy));
            Internal.StackPushObject(oCreatureToTakeFrom, false);
            Internal.StackPushInteger(nAmount);
            Internal.CallBuiltIn(444);
        }

        //  Determine whether oObject is in conversation.
        public static bool IsInConversation(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(445);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Create an Ability Decrease effect.
        //  - nAbility: ABILITY_*
        //  - nModifyBy: This is the amount by which to decrement the ability
        public static Effect EffectAbilityDecrease(Ability nAbility, int nModifyBy)
        {
            Internal.StackPushInteger(nModifyBy);
            Internal.StackPushInteger((int)nAbility);
            Internal.CallBuiltIn(446);
            return Internal.StackPopEffect();
        }

        //  Create an Attack Decrease effect.
        //  - nPenalty
        //  - nModifierType: ATTACK_BONUS_*
        public static Effect EffectAttackDecrease(int nPenalty, AttackBonus nModifierType = AttackBonus.Misc)
        {
            Internal.StackPushInteger((int)nModifierType);
            Internal.StackPushInteger(nPenalty);
            Internal.CallBuiltIn(447);
            return Internal.StackPopEffect();
        }

        //  Create a Damage Decrease effect.
        //  - nPenalty
        //  - nDamageType: DAMAGE_TYPE_*
        public static Effect EffectDamageDecrease(int nPenalty, DamageType nDamageType = DamageType.Magical)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger(nPenalty);
            Internal.CallBuiltIn(448);
            return Internal.StackPopEffect();
        }

        //  Create a Damage Immunity Decrease effect.
        //  - nDamageType: DAMAGE_TYPE_*
        //  - nPercentImmunity
        public static Effect EffectDamageImmunityDecrease(DamageType nDamageType, int nPercentImmunity)
        {
            Internal.StackPushInteger(nPercentImmunity);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(449);
            return Internal.StackPopEffect();
        }

        //  Create an AC Decrease effect.
        //  - nValue
        //  - nModifyType: AC_*
        //  - nDamageType: DAMAGE_TYPE_*
        //    * Default value for nDamageType should only ever be used in this function prototype.
        public static Effect EffectACDecrease(int nValue, AC nModifyType = AC.DodgeBonus, DamageType nDamageType = DamageType.ACVsDamageTypeAll)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger((int)nModifyType);
            Internal.StackPushInteger(nValue);
            Internal.CallBuiltIn(450);
            return Internal.StackPopEffect();
        }

        //  Create a Movement Speed Decrease effect.
        //  - nPercentChange - range 0 through 99
        //  eg.
        //     0 = no change in speed
        //    50 = 50% slower
        //    99 = almost immobile
        public static Effect EffectMovementSpeedDecrease(int nPercentChange)
        {
            Internal.StackPushInteger(nPercentChange);
            Internal.CallBuiltIn(451);
            return Internal.StackPopEffect();
        }

        //  Create a Saving Throw Decrease effect.
        //  - nSave: SAVING_THROW_* (not SAVING_THROW_TYPE_*)
        //           SAVING_THROW_ALL
        //           SAVING_THROW_FORT
        //           SAVING_THROW_REFLEX
        //           SAVING_THROW_WILL 
        //  - nValue: size of the Saving Throw decrease
        //  - nSaveType: SAVING_THROW_TYPE_* (e.g. SAVING_THROW_TYPE_ACID )
        public static Effect EffectSavingThrowDecrease(SavingThrow nSave, int nValue, SavingThrowType nSaveType = SavingThrowType.All)
        {
            Internal.StackPushInteger((int)nSaveType);
            Internal.StackPushInteger(nValue);
            Internal.StackPushInteger((int)nSave);
            Internal.CallBuiltIn(452);
            return Internal.StackPopEffect();
        }

        //  Create a Skill Decrease effect.
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nSkill is invalid.
        public static Effect EffectSkillDecrease(Skill nSkill, int nValue)
        {
            Internal.StackPushInteger(nValue);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(453);
            return Internal.StackPopEffect();
        }

        //  Create a Spell Resistance Decrease effect.
        public static Effect EffectSpellResistanceDecrease(int nValue)
        {
            Internal.StackPushInteger(nValue);
            Internal.CallBuiltIn(454);
            return Internal.StackPopEffect();
        }

        //  Determine whether oTarget is a plot object.
        public static bool GetPlotFlag(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(455);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Set oTarget's plot object status.
        public static void SetPlotFlag(NWGameObject oTarget, bool nPlotFlag)
        {
            Internal.StackPushInteger(Convert.ToInt32(nPlotFlag));
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(456);
        }

        //  Create an Invisibility effect.
        //  - nInvisibilityType: INVISIBILITY_TYPE_*
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nInvisibilityType
        //    is invalid.
        public static Effect EffectInvisibility(InvisibilityType nInvisibilityType)
        {
            Internal.StackPushInteger((int)nInvisibilityType);
            Internal.CallBuiltIn(457);
            return Internal.StackPopEffect();
        }

        //  Create a Concealment effect.
        //  - nPercentage: 1-100 inclusive
        //  - nMissChanceType: MISS_CHANCE_TYPE_*
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nPercentage < 1 or
        //    nPercentage > 100.
        public static Effect EffectConcealment(int nPercentage, MissChanceType nMissType = MissChanceType.Normal)
        {
            Internal.StackPushInteger((int)nMissType);
            Internal.StackPushInteger(nPercentage);
            Internal.CallBuiltIn(458);
            return Internal.StackPopEffect();
        }

        //  Create a Darkness effect.
        public static Effect EffectDarkness()
        {
            Internal.CallBuiltIn(459);
            return Internal.StackPopEffect();
        }

        //  Create a Dispel Magic All effect.
        //  If no parameter is specified, USE_CREATURE_LEVEL will be used. This will
        //  cause the dispel effect to use the level of the creature that created the
        //  effect.
        public static Effect EffectDispelMagicAll(int nCasterLevel = NWNConstants.UseCreatureLevel)
        {
            Internal.StackPushInteger(nCasterLevel);
            Internal.CallBuiltIn(460);
            return Internal.StackPopEffect();
        }

        //  Create an Ultravision effect.
        public static Effect EffectUltravision()
        {
            Internal.CallBuiltIn(461);
            return Internal.StackPopEffect();
        }

        //  Create a Negative Level effect.
        //  - nNumLevels: the number of negative levels to apply.
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nNumLevels > 100.
        public static Effect EffectNegativeLevel(int nNumLevels, bool bHPBonus = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bHPBonus));
            Internal.StackPushInteger(nNumLevels);
            Internal.CallBuiltIn(462);
            return Internal.StackPopEffect();
        }

        //  Create a Polymorph effect.
        public static Effect EffectPolymorph(int nPolymorphSelection, bool nLocked = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nLocked));
            Internal.StackPushInteger(nPolymorphSelection);
            Internal.CallBuiltIn(463);
            return Internal.StackPopEffect();
        }

        //  Create a Sanctuary effect.
        //  - nDifficultyClass: must be a non-zero, positive number
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nDifficultyClass <= 0.
        public static Effect EffectSanctuary(int nDifficultyClass)
        {
            Internal.StackPushInteger(nDifficultyClass);
            Internal.CallBuiltIn(464);
            return Internal.StackPopEffect();
        }

        //  Create a True Seeing effect.
        public static Effect EffectTrueSeeing()
        {
            Internal.CallBuiltIn(465);
            return Internal.StackPopEffect();
        }

        //  Create a See Invisible effect.
        public static Effect EffectSeeInvisible()
        {
            Internal.CallBuiltIn(466);
            return Internal.StackPopEffect();
        }

        //  Create a Time Stop effect.
        public static Effect EffectTimeStop()
        {
            Internal.CallBuiltIn(467);
            return Internal.StackPopEffect();
        }

        //  Create a Blindness effect.
        public static Effect EffectBlindness()
        {
            Internal.CallBuiltIn(468);
            return Internal.StackPopEffect();
        }

        //  Determine whether oSource has a friendly reaction towards oTarget, depending
        //  on the reputation, PVP setting and (if both oSource and oTarget are PCs),
        //  oSource's Like/Dislike setting for oTarget.
        //  Note: If you just want to know how two objects feel about each other in terms
        //  of faction and personal reputation, use GetIsFriend() instead.
        //  * Returns TRUE if oSource has a friendly reaction towards oTarget
        public static bool GetIsReactionTypeFriendly(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(469);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Determine whether oSource has a neutral reaction towards oTarget, depending
        //  on the reputation, PVP setting and (if both oSource and oTarget are PCs),
        //  oSource's Like/Dislike setting for oTarget.
        //  Note: If you just want to know how two objects feel about each other in terms
        //  of faction and personal reputation, use GetIsNeutral() instead.
        //  * Returns TRUE if oSource has a neutral reaction towards oTarget
        public static bool GetIsReactionTypeNeutral(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(470);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Determine whether oSource has a Hostile reaction towards oTarget, depending
        //  on the reputation, PVP setting and (if both oSource and oTarget are PCs),
        //  oSource's Like/Dislike setting for oTarget.
        //  Note: If you just want to know how two objects feel about each other in terms
        //  of faction and personal reputation, use GetIsEnemy() instead.
        //  * Returns TRUE if oSource has a hostile reaction towards oTarget
        public static bool GetIsReactionTypeHostile(NWGameObject oTarget, NWGameObject oSource = null)
        {
            Internal.StackPushObject(oSource, false);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(471);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Create a Spell Level Absorption effect.
        //  - nMaxSpellLevelAbsorbed: maximum spell level that will be absorbed by the
        //    effect
        //  - nTotalSpellLevelsAbsorbed: maximum number of spell levels that will be
        //    absorbed by the effect
        //  - nSpellSchool: SPELL_SCHOOL_*
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if:
        //    nMaxSpellLevelAbsorbed is not between -1 and 9 inclusive, or nSpellSchool
        //    is invalid.
        public static Effect EffectSpellLevelAbsorption(int nMaxSpellLevelAbsorbed, int nTotalSpellLevelsAbsorbed = 0, SpellSchool nSpellSchool = SpellSchool.General)
        {
            Internal.StackPushInteger((int)nSpellSchool);
            Internal.StackPushInteger(nTotalSpellLevelsAbsorbed);
            Internal.StackPushInteger(nMaxSpellLevelAbsorbed);
            Internal.CallBuiltIn(472);
            return Internal.StackPopEffect();
        }

        //  Create a Dispel Magic Best effect.
        //  If no parameter is specified, USE_CREATURE_LEVEL will be used. This will
        //  cause the dispel effect to use the level of the creature that created the
        //  effect.
        public static Effect EffectDispelMagicBest(int nCasterLevel = NWNConstants.UseCreatureLevel)
        {
            Internal.StackPushInteger(nCasterLevel);
            Internal.CallBuiltIn(473);
            return Internal.StackPopEffect();
        }

        //  Try to send oTarget to a new server defined by sIPaddress.
        //  - oTarget
        //  - sIPaddress: this can be numerical "192.168.0.84" or alphanumeric
        //    "www.bioware.com". It can also contain a port "192.168.0.84:5121" or
        //    "www.bioware.com:5121"; if the port is not specified, it will default to
        //    5121.
        //  - sPassword: login password for the destination server
        //  - sWaypointTag: if this is set, after portalling the character will be moved
        //    to this waypoint if it exists
        //  - bSeamless: if this is set, the client wil not be prompted with the
        //    information window telling them about the server, and they will not be
        //    allowed to save a copy of their character if they are using a local vault
        //    character.
        public static void ActivatePortal(NWGameObject oTarget, string sIPaddress = "", string sPassword = "", string sWaypointTag = "", bool bSeemless = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bSeemless));
            Internal.StackPushString(sWaypointTag);
            Internal.StackPushString(sPassword);
            Internal.StackPushString(sIPaddress);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(474);
        }

        //  Get the number of stacked items that oItem comprises.
        public static int GetNumStackedItems(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(475);
            return Internal.StackPopInteger();
        }

        //  Use this on an NPC to cause all creatures within a 10-metre radius to stop
        //  what they are doing and sets the NPC's enemies within this range to be
        //  neutral towards the NPC for roughly 3 minutes. If this command is run on a PC
        //  or an object that is not a creature, nothing will happen.
        public static void SurrenderToEnemies()
        {
            Internal.CallBuiltIn(476);
        }

        //  Create a Miss Chance effect.
        //  - nPercentage: 1-100 inclusive
        //  - nMissChanceType: MISS_CHANCE_TYPE_*
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nPercentage < 1 or
        //    nPercentage > 100.
        public static Effect EffectMissChance(int nPercentage, MissChanceType nMissChanceType = MissChanceType.Normal)
        {
            Internal.StackPushInteger((int)nMissChanceType);
            Internal.StackPushInteger(nPercentage);
            Internal.CallBuiltIn(477);
            return Internal.StackPopEffect();
        }

        //  Get the number of Hitdice worth of Turn Resistance that oUndead may have.
        //  This will only work on undead creatures.
        public static int GetTurnResistanceHD(NWGameObject oUndead = null)
        {
            Internal.StackPushObject(oUndead, false);
            Internal.CallBuiltIn(478);
            return Internal.StackPopInteger();
        }

        //  Get the size (CREATURE_SIZE_*) of oCreature.
        public static int GetCreatureSize(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(479);
            return Internal.StackPopInteger();
        }

        //  Create a Disappear/Appear effect.
        //  The object will "fly away" for the duration of the effect and will reappear
        //  at lLocation.
        //  - nAnimation determines which appear and disappear animations to use. Most creatures
        //  only have animation 1, although a few have 2 (like beholders)
        public static Effect EffectDisappearAppear(Location lLocation, int nAnimation = 1)
        {
            Internal.StackPushInteger(nAnimation);
            Internal.StackPushLocation(lLocation);
            Internal.CallBuiltIn(480);
            return Internal.StackPopEffect();
        }

        //  Create a Disappear effect to make the object "fly away" and then destroy
        //  itself.
        //  - nAnimation determines which appear and disappear animations to use. Most creatures
        //  only have animation 1, although a few have 2 (like beholders)
        public static Effect EffectDisappear(int nAnimation = 1)
        {
            Internal.StackPushInteger(nAnimation);
            Internal.CallBuiltIn(481);
            return Internal.StackPopEffect();
        }

        //  Create an Appear effect to make the object "fly in".
        //  - nAnimation determines which appear and disappear animations to use. Most creatures
        //  only have animation 1, although a few have 2 (like beholders)
        public static Effect EffectAppear(int nAnimation = 1)
        {
            Internal.StackPushInteger(nAnimation);
            Internal.CallBuiltIn(482);
            return Internal.StackPopEffect();
        }

        //  The action subject will unlock oTarget, which can be a door or a placeable
        //  object.
        public static void ActionUnlockObject(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(483);
        }

        //  The action subject will lock oTarget, which can be a door or a placeable
        //  object.
        public static void ActionLockObject(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(484);
        }

        //  Create a Modify Attacks effect to add attacks.
        //  - nAttacks: maximum is 5, even with the effect stacked
        //  * Returns an effect of type EFFECT_TYPE_INVALIDEFFECT if nAttacks > 5.
        public static Effect EffectModifyAttacks(int nAttacks)
        {
            Internal.StackPushInteger(nAttacks);
            Internal.CallBuiltIn(485);
            return Internal.StackPopEffect();
        }

        //  Get the last trap detected by oTarget.
        //  * Return value on error: OBJECT_INVALID
        public static NWGameObject GetLastTrapDetected(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(486);
            return Internal.StackPopObject();
        }

        //  Create a Damage Shield effect which does (nDamageAmount + nRandomAmount)
        //  damage to any melee attacker on a successful attack of damage type nDamageType.
        //  - nDamageAmount: an integer value
        //  - nRandomAmount: DAMAGE_BONUS_*
        //  - nDamageType: DAMAGE_TYPE_*
        //  NOTE! You *must* use the DAMAGE_BONUS_* constants! Using other values may
        //        result in odd behaviour.
        public static Effect EffectDamageShield(DamageBonus nDamageAmount, int nRandomAmount, DamageType nDamageType)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger(nRandomAmount);
            Internal.StackPushInteger((int)nDamageAmount);
            Internal.CallBuiltIn(487);
            return Internal.StackPopEffect();
        }

        //  Get the trap nearest to oTarget.
        //  Note : "trap objects" are actually any trigger, placeable or door that is
        //  trapped in oTarget's area.
        //  - oTarget
        //  - nTrapDetected: if this is TRUE, the trap returned has to have been detected
        //    by oTarget.
        public static NWGameObject GetNearestTrapToObject(NWGameObject oTarget = null, bool nTrapDetected = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nTrapDetected));
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(488);
            return Internal.StackPopObject();
        }

        //  Get the name of oCreature's deity.
        //  * Returns "" if oCreature is invalid (or if the deity name is blank for
        //    oCreature).
        public static string GetDeity(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(489);
            return Internal.StackPopString();
        }

        //  Get the name of oCreature's sub race.
        //  * Returns "" if oCreature is invalid (or if sub race is blank for oCreature).
        public static string GetSubRace(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(490);
            return Internal.StackPopString();
        }

        //  Get oTarget's base fortitude saving throw value (this will only work for
        //  creatures, doors, and placeables).
        //  * Returns 0 if oTarget is invalid.
        public static int GetFortitudeSavingThrow(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(491);
            return Internal.StackPopInteger();
        }

        //  Get oTarget's base will saving throw value (this will only work for creatures,
        //  doors, and placeables).
        //  * Returns 0 if oTarget is invalid.
        public static int GetWillSavingThrow(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(492);
            return Internal.StackPopInteger();
        }

        //  Get oTarget's base reflex saving throw value (this will only work for
        //  creatures, doors, and placeables).
        //  * Returns 0 if oTarget is invalid.
        public static int GetReflexSavingThrow(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(493);
            return Internal.StackPopInteger();
        }

        //  Get oCreature's challenge rating.
        //  * Returns 0.0 if oCreature is invalid.
        public static float GetChallengeRating(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(494);
            return Internal.StackPopFloat();
        }

        //  Get oCreature's age.
        //  * Returns 0 if oCreature is invalid.
        public static int GetAge(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(495);
            return Internal.StackPopInteger();
        }

        //  Get oCreature's movement rate.
        //  * Returns 0 if oCreature is invalid.
        public static int GetMovementRate(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(496);
            return Internal.StackPopInteger();
        }

        //  Get oCreature's familiar creature type (FAMILIAR_CREATURE_TYPE_*).
        //  * Returns FAMILIAR_CREATURE_TYPE_NONE if oCreature is invalid or does not
        //    currently have a familiar.
        public static FamiliarCreatureType GetFamiliarCreatureType(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(497);
            return (FamiliarCreatureType)Internal.StackPopInteger();
        }

        //  Get oCreature's animal companion creature type
        //  (ANIMAL_COMPANION_CREATURE_TYPE_*).
        //  * Returns ANIMAL_COMPANION_CREATURE_TYPE_NONE if oCreature is invalid or does
        //    not currently have an animal companion.
        public static AnimalCompanionCreatureType GetAnimalCompanionCreatureType(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(498);
            return (AnimalCompanionCreatureType)Internal.StackPopInteger();
        }

        //  Get oCreature's familiar's name.
        //  * Returns "" if oCreature is invalid, does not currently
        //  have a familiar or if the familiar's name is blank.
        public static string GetFamiliarName(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(499);
            return Internal.StackPopString();
        }

        //  Get oCreature's animal companion's name.
        //  * Returns "" if oCreature is invalid, does not currently
        //  have an animal companion or if the animal companion's name is blank.
        public static string GetAnimalCompanionName(NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(500);
            return Internal.StackPopString();
        }

        //  The action subject will fake casting a spell at oTarget; the conjure and cast
        //  animations and visuals will occur, nothing else.
        //  - nSpell
        //  - oTarget
        //  - nProjectilePathType: PROJECTILE_PATH_TYPE_*
        public static void ActionCastFakeSpellAtObject(Spell nSpell, NWGameObject oTarget, ProjectilePathType nProjectilePathType = ProjectilePathType.Default)
        {
            Internal.StackPushInteger((int)nProjectilePathType);
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(501);
        }

        //  The action subject will fake casting a spell at lLocation; the conjure and
        //  cast animations and visuals will occur, nothing else.
        //  - nSpell
        //  - lTarget
        //  - nProjectilePathType: PROJECTILE_PATH_TYPE_*
        public static void ActionCastFakeSpellAtLocation(Spell nSpell, Location lTarget, ProjectilePathType nProjectilePathType = ProjectilePathType.Default)
        {
            Internal.StackPushInteger((int)nProjectilePathType);
            Internal.StackPushLocation(lTarget);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(502);
        }

        //  Removes oAssociate from the service of oMaster, returning them to their
        //  original faction.
        public static void RemoveSummonedAssociate(NWGameObject oMaster, NWGameObject oAssociate = null)
        {
            Internal.StackPushObject(oAssociate, false);
            Internal.StackPushObject(oMaster, false);
            Internal.CallBuiltIn(503);
        }

        //  Set the camera mode for oPlayer.
        //  - oPlayer
        //  - nCameraMode: CAMERA_MODE_*
        //  * If oPlayer is not player-controlled or nCameraMode is invalid, nothing
        //    happens.
        public static void SetCameraMode(NWGameObject oPlayer, CameraMode nCameraMode)
        {
            Internal.StackPushInteger((int)nCameraMode);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(504);
        }

        //  * Returns TRUE if oCreature is resting.
        public static bool GetIsResting(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(505);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the last PC that has rested in the module.
        public static NWGameObject GetLastPCRested()
        {
            Internal.CallBuiltIn(506);
            return Internal.StackPopObject();
        }

        //  Set the weather for oTarget.
        //  - oTarget: if this is GetModule(), all outdoor areas will be modified by the
        //    weather constant. If it is an area, oTarget will play the weather only if
        //    it is an outdoor area.
        //  - nWeather: WEATHER_*
        //    -> WEATHER_USER_AREA_SETTINGS will set the area back to random weather.
        //    -> WEATHER_CLEAR, WEATHER_RAIN, WEATHER_SNOW will make the weather go to
        //       the appropriate precipitation *without stopping*.
        public static void SetWeather(NWGameObject oTarget, Weather nWeather)
        {
            Internal.StackPushInteger((int)nWeather);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(507);
        }

        //  Determine the type (REST_EVENTTYPE_REST_*) of the last rest event (as
        //  returned from the OnPCRested module event).
        public static RestEventType GetLastRestEventType()
        {
            Internal.CallBuiltIn(508);
            return (RestEventType)Internal.StackPopInteger();
        }

        //  Shut down the currently loaded module and start a new one (moving all
        //  currently-connected players to the starting point.
        public static void StartNewModule(string sModuleName)
        {
            Internal.StackPushString(sModuleName);
            Internal.CallBuiltIn(509);
        }

        //  Create a Swarm effect.
        //  - nLooping: If this is TRUE, for the duration of the effect when one creature
        //    created by this effect dies, the next one in the list will be created.  If
        //    the last creature in the list dies, we loop back to the beginning and
        //    sCreatureTemplate1 will be created, and so on...
        //  - sCreatureTemplate1
        //  - sCreatureTemplate2
        //  - sCreatureTemplate3
        //  - sCreatureTemplate4
        public static Effect EffectSwarm(bool nLooping, string sCreatureTemplate1, string sCreatureTemplate2 = "", string sCreatureTemplate3 = "", string sCreatureTemplate4 = "")
        {
            Internal.StackPushString(sCreatureTemplate4);
            Internal.StackPushString(sCreatureTemplate3);
            Internal.StackPushString(sCreatureTemplate2);
            Internal.StackPushString(sCreatureTemplate1);
            Internal.StackPushInteger(Convert.ToInt32(nLooping));
            Internal.CallBuiltIn(510);
            return Internal.StackPopEffect();
        }

        //  * Returns TRUE if oItem is a ranged weapon.
        public static bool GetWeaponRanged(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(511);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Only if we are in a single player game, AutoSave the game.
        public static void DoSinglePlayerAutoSave()
        {
            Internal.CallBuiltIn(512);
        }

        //  Get the game difficulty (GAME_DIFFICULTY_*).
        public static GameDifficulty GetGameDifficulty()
        {
            Internal.CallBuiltIn(513);
            return (GameDifficulty)Internal.StackPopInteger();
        }

        //  Set the main light color on the tile at lTileLocation.
        //  - lTileLocation: the vector part of this is the tile grid (x,y) coordinate of
        //    the tile.
        //  - nMainLight1Color: TILE_MAIN_LIGHT_COLOR_*
        //  - nMainLight2Color: TILE_MAIN_LIGHT_COLOR_*
        public static void SetTileMainLightColor(Location lTileLocation, TileMainLightColor nMainLight1Color, TileMainLightColor nMainLight2Color)
        {
            Internal.StackPushInteger((int)nMainLight2Color);
            Internal.StackPushInteger((int)nMainLight1Color);
            Internal.StackPushLocation(lTileLocation);
            Internal.CallBuiltIn(514);
        }

        //  Set the source light color on the tile at lTileLocation.
        //  - lTileLocation: the vector part of this is the tile grid (x,y) coordinate of
        //    the tile.
        //  - nSourceLight1Color: TILE_SOURCE_LIGHT_COLOR_*
        //  - nSourceLight2Color: TILE_SOURCE_LIGHT_COLOR_*
        public static void SetTileSourceLightColor(Location lTileLocation, TileSourceLightColor nSourceLight1Color, TileSourceLightColor nSourceLight2Color)
        {
            Internal.StackPushInteger((int)nSourceLight2Color);
            Internal.StackPushInteger((int)nSourceLight1Color);
            Internal.StackPushLocation(lTileLocation);
            Internal.CallBuiltIn(515);
        }

        //  All clients in oArea will recompute the static lighting.
        //  This can be used to update the lighting after changing any tile lights or if
        //  placeables with lights have been added/deleted.
        public static void RecomputeStaticLighting(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(516);
        }

        //  Get the color (TILE_MAIN_LIGHT_COLOR_*) for the main light 1 of the tile at
        //  lTile.
        //  - lTile: the vector part of this is the tile grid (x,y) coordinate of the tile.
        public static TileMainLightColor GetTileMainLight1Color(Location lTile)
        {
            Internal.StackPushLocation(lTile);
            Internal.CallBuiltIn(517);
            return (TileMainLightColor)Internal.StackPopInteger();
        }

        //  Get the color (TILE_MAIN_LIGHT_COLOR_*) for the main light 2 of the tile at
        //  lTile.
        //  - lTile: the vector part of this is the tile grid (x,y) coordinate of the
        //    tile.
        public static TileMainLightColor GetTileMainLight2Color(Location lTile)
        {
            Internal.StackPushLocation(lTile);
            Internal.CallBuiltIn(518);
            return (TileMainLightColor)Internal.StackPopInteger();
        }

        //  Get the color (TILE_SOURCE_LIGHT_COLOR_*) for the source light 1 of the tile
        //  at lTile.
        //  - lTile: the vector part of this is the tile grid (x,y) coordinate of the
        //    tile.
        public static TileSourceLightColor GetTileSourceLight1Color(Location lTile)
        {
            Internal.StackPushLocation(lTile);
            Internal.CallBuiltIn(519);
            return (TileSourceLightColor)Internal.StackPopInteger();
        }

        //  Get the color (TILE_SOURCE_LIGHT_COLOR_*) for the source light 2 of the tile
        //  at lTile.
        //  - lTile: the vector part of this is the tile grid (x,y) coordinate of the
        //    tile.
        public static TileSourceLightColor GetTileSourceLight2Color(Location lTile)
        {
            Internal.StackPushLocation(lTile);
            Internal.CallBuiltIn(520);
            return (TileSourceLightColor)Internal.StackPopInteger();
        }

        //  Make the corresponding panel button on the player's client start or stop
        //  flashing.
        //  - oPlayer
        //  - nButton: PANEL_BUTTON_*
        //  - nEnableFlash: if this is TRUE nButton will start flashing.  It if is FALSE,
        //    nButton will stop flashing.
        public static void SetPanelButtonFlash(NWGameObject oPlayer, PanelButton nButton, bool nEnableFlash)
        {
            Internal.StackPushInteger(Convert.ToInt32(nEnableFlash));
            Internal.StackPushInteger((int)nButton);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(521);
        }

        //  Get the current action (ACTION_*) that oObject is executing.
        public static ActionType GetCurrentAction(NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(522);
            return (ActionType)Internal.StackPopInteger();
        }

        //  Set how nStandardFaction feels about oCreature.
        //  - nStandardFaction: STANDARD_FACTION_*
        //  - nNewReputation: 0-100 (inclusive)
        //  - oCreature
        public static void SetStandardFactionReputation(StandardFaction nStandardFaction, int nNewReputation, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger(nNewReputation);
            Internal.StackPushInteger((int)nStandardFaction);
            Internal.CallBuiltIn(523);
        }

        //  Find out how nStandardFaction feels about oCreature.
        //  - nStandardFaction: STANDARD_FACTION_*
        //  - oCreature
        //  Returns -1 on an error.
        //  Returns 0-100 based on the standing of oCreature within the faction nStandardFaction.
        //  0-10   :  Hostile.
        //  11-89  :  Neutral.
        //  90-100 :  Friendly.
        public static int GetStandardFactionReputation(StandardFaction nStandardFaction, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nStandardFaction);
            Internal.CallBuiltIn(524);
            return Internal.StackPopInteger();
        }

        //  Display floaty text above the specified creature.
        //  The text will also appear in the chat buffer of each player that receives the
        //  floaty text.
        //  - nStrRefToDisplay: String ref (therefore text is translated)
        //  - oCreatureToFloatAbove
        //  - bBroadcastToFaction: If this is TRUE then only creatures in the same faction
        //    as oCreatureToFloatAbove
        //    will see the floaty text, and only if they are within range (30 metres).
        public static void FloatingTextStrRefOnCreature(int nStrRefToDisplay, NWGameObject oCreatureToFloatAbove, bool bBroadcastToFaction = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bBroadcastToFaction));
            Internal.StackPushObject(oCreatureToFloatAbove, false);
            Internal.StackPushInteger(nStrRefToDisplay);
            Internal.CallBuiltIn(525);
        }

        //  Display floaty text above the specified creature.
        //  The text will also appear in the chat buffer of each player that receives the
        //  floaty text.
        //  - sStringToDisplay: String
        //  - oCreatureToFloatAbove
        //  - bBroadcastToFaction: If this is TRUE then only creatures in the same faction
        //    as oCreatureToFloatAbove
        //    will see the floaty text, and only if they are within range (30 metres).
        public static void FloatingTextStringOnCreature(NWGameObject oCreatureToFloatAbove, string sStringToDisplay, bool bBroadcastToFaction = false)
        {
            // Note: this method's parameters have been moved around to make the API easier to use. The order in which they are pushed to NWN have not been modified.
            Internal.StackPushInteger(Convert.ToInt32(bBroadcastToFaction));
            Internal.StackPushObject(oCreatureToFloatAbove, false);
            Internal.StackPushString(sStringToDisplay);
            Internal.CallBuiltIn(526);
        }

        //  - oTrapObject: a placeable, door or trigger
        //  * Returns TRUE if oTrapObject is disarmable.
        public static bool GetTrapDisarmable(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(527);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  - oTrapObject: a placeable, door or trigger
        //  * Returns TRUE if oTrapObject is detectable.
        public static bool GetTrapDetectable(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(528);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  - oTrapObject: a placeable, door or trigger
        //  - oCreature
        //  * Returns TRUE if oCreature has detected oTrapObject
        public static bool GetTrapDetectedBy(NWGameObject oTrapObject, NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(529);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  - oTrapObject: a placeable, door or trigger
        //  * Returns TRUE if oTrapObject has been flagged as visible to all creatures.
        public static bool GetTrapFlagged(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(530);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the trap base type (TRAP_BASE_TYPE_*) of oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        public static int GetTrapBaseType(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(531);
            return Internal.StackPopInteger();
        }

        //  - oTrapObject: a placeable, door or trigger
        //  * Returns TRUE if oTrapObject is one-shot (i.e. it does not reset itself
        //    after firing.
        public static bool GetTrapOneShot(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(532);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the creator of oTrapObject, the creature that set the trap.
        //  - oTrapObject: a placeable, door or trigger
        //  * Returns OBJECT_INVALID if oTrapObject was created in the toolset.
        public static NWGameObject GetTrapCreator(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(533);
            return Internal.StackPopObject();
        }

        //  Get the tag of the key that will disarm oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        public static string GetTrapKeyTag(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(534);
            return Internal.StackPopString();
        }

        //  Get the DC for disarming oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        public static int GetTrapDisarmDC(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(535);
            return Internal.StackPopInteger();
        }

        //  Get the DC for detecting oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        public static int GetTrapDetectDC(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(536);
            return Internal.StackPopInteger();
        }

        //  * Returns TRUE if a specific key is required to open the lock on oObject.
        public static bool GetLockKeyRequired(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(537);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the tag of the key that will open the lock on oObject.
        public static string GetLockKeyTag(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(538);
            return Internal.StackPopString();
        }

        //  * Returns TRUE if the lock on oObject is lockable.
        public static bool GetLockLockable(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(539);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the DC for unlocking oObject.
        public static int GetLockUnlockDC(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(540);
            return Internal.StackPopInteger();
        }

        //  Get the DC for locking oObject.
        public static int GetLockLockDC(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(541);
            return Internal.StackPopInteger();
        }

        //  Get the last PC that levelled up.
        public static NWGameObject GetPCLevellingUp()
        {
            Internal.CallBuiltIn(542);
            return Internal.StackPopObject();
        }

        //  - nFeat: FEAT_*
        //  - oObject
        //  * Returns TRUE if oObject has effects on it originating from nFeat.
        public static bool GetHasFeatEffect(Feat nFeat, NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.StackPushInteger((int)nFeat);
            Internal.CallBuiltIn(543);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Set the status of the illumination for oPlaceable.
        //  - oPlaceable
        //  - bIlluminate: if this is TRUE, oPlaceable's illumination will be turned on.
        //    If this is FALSE, oPlaceable's illumination will be turned off.
        //  Note: You must call RecomputeStaticLighting() after calling this function in
        //  order for the changes to occur visually for the players.
        //  SetPlaceableIllumination() buffers the illumination changes, which are then
        //  sent out to the players once RecomputeStaticLighting() is called.  As such,
        //  it is best to call SetPlaceableIllumination() for all the placeables you wish
        //  to set the illumination on, and then call RecomputeStaticLighting() once after
        //  all the placeable illumination has been set.
        //  * If oPlaceable is not a placeable object, or oPlaceable is a placeable that
        //    doesn't have a light, nothing will happen.
        public static void SetPlaceableIllumination(NWGameObject oPlaceable = null, bool bIlluminate = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bIlluminate));
            Internal.StackPushObject(oPlaceable, false);
            Internal.CallBuiltIn(544);
        }

        //  * Returns TRUE if the illumination for oPlaceable is on
        public static bool GetPlaceableIllumination(NWGameObject oPlaceable = null)
        {
            Internal.StackPushObject(oPlaceable, false);
            Internal.CallBuiltIn(545);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  - oPlaceable
        //  - nPlaceableAction: PLACEABLE_ACTION_*
        //  * Returns TRUE if nPlacebleAction is valid for oPlaceable.
        public static bool GetIsPlaceableObjectActionPossible(NWGameObject oPlaceable, PlaceableAction nPlaceableAction)
        {
            Internal.StackPushInteger((int)nPlaceableAction);
            Internal.StackPushObject(oPlaceable, false);
            Internal.CallBuiltIn(546);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  The caller performs nPlaceableAction on oPlaceable.
        //  - oPlaceable
        //  - nPlaceableAction: PLACEABLE_ACTION_*
        public static void DoPlaceableObjectAction(NWGameObject oPlaceable, PlaceableAction nPlaceableAction)
        {
            Internal.StackPushInteger((int)nPlaceableAction);
            Internal.StackPushObject(oPlaceable, false);
            Internal.CallBuiltIn(547);
        }

        //  Get the first PC in the player list.
        //  This resets the position in the player list for GetNextPC().
        public static NWGameObject GetFirstPC()
        {
            Internal.CallBuiltIn(548);
            return Internal.StackPopObject();
        }

        //  Get the next PC in the player list.
        //  This picks up where the last GetFirstPC() or GetNextPC() left off.
        public static NWGameObject GetNextPC()
        {
            Internal.CallBuiltIn(549);
            return Internal.StackPopObject();
        }

        //  Set whether or not the creature oDetector has detected the trapped object oTrap.
        //  - oTrap: A trapped trigger, placeable or door object.
        //  - oDetector: This is the creature that the detected status of the trap is being adjusted for.
        //  - bDetected: A Boolean that sets whether the trapped object has been detected or not.
        public static int SetTrapDetectedBy(NWGameObject oTrap, NWGameObject oDetector, bool bDetected = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bDetected));
            Internal.StackPushObject(oDetector, false);
            Internal.StackPushObject(oTrap, false);
            Internal.CallBuiltIn(550);
            return Internal.StackPopInteger();
        }

        //  Note: Only placeables, doors and triggers can be trapped.
        //  * Returns TRUE if oObject is trapped.
        public static bool GetIsTrapped(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(551);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Create a Turn Resistance Decrease effect.
        //  - nHitDice: a positive number representing the number of hit dice for the
        // /  decrease
        public static Effect EffectTurnResistanceDecrease(int nHitDice)
        {
            Internal.StackPushInteger(nHitDice);
            Internal.CallBuiltIn(552);
            return Internal.StackPopEffect();
        }

        //  Create a Turn Resistance Increase effect.
        //  - nHitDice: a positive number representing the number of hit dice for the
        //    increase
        public static Effect EffectTurnResistanceIncrease(int nHitDice)
        {
            Internal.StackPushInteger(nHitDice);
            Internal.CallBuiltIn(553);
            return Internal.StackPopEffect();
        }

        //  Spawn in the Death GUI.
        //  The default (as defined by BioWare) can be spawned in by PopUpGUIPanel, but
        //  if you want to turn off the "Respawn" or "Wait for Help" buttons, this is the
        //  function to use.
        //  - oPC
        //  - bRespawnButtonEnabled: if this is TRUE, the "Respawn" button will be enabled
        //    on the Death GUI.
        //  - bWaitForHelpButtonEnabled: if this is TRUE, the "Wait For Help" button will
        //    be enabled on the Death GUI (Note: This button will not appear in single player games).
        //  - nHelpStringReference
        //  - sHelpString
        public static void PopUpDeathGUIPanel(NWGameObject oPC, bool bRespawnButtonEnabled = true, bool bWaitForHelpButtonEnabled = true, int nHelpStringReference = 0, string sHelpString = "")
        {
            Internal.StackPushString(sHelpString);
            Internal.StackPushInteger(nHelpStringReference);
            Internal.StackPushInteger(Convert.ToInt32(bWaitForHelpButtonEnabled));
            Internal.StackPushInteger(Convert.ToInt32(bRespawnButtonEnabled));
            Internal.StackPushObject(oPC, false);
            Internal.CallBuiltIn(554);
        }

        //  Disable oTrap.
        //  - oTrap: a placeable, door or trigger.
        public static void SetTrapDisabled(NWGameObject oTrap)
        {
            Internal.StackPushObject(oTrap, false);
            Internal.CallBuiltIn(555);
        }

        //  Get the last object that was sent as a GetLastAttacker(), GetLastDamager(),
        //  GetLastSpellCaster() (for a hostile spell), or GetLastDisturbed() (when a
        //  creature is pickpocketed).
        //  Note: Return values may only ever be:
        //  1) A Creature
        //  2) Plot Characters will never have this value set
        //  3) Area of Effect Objects will return the AOE creator if they are registered
        //     as this value, otherwise they will return INVALID_OBJECT_ID
        //  4) Traps will not return the creature that set the trap.
        //  5) This value will never be overwritten by another non-creature object.
        //  6) This value will never be a dead/destroyed creature
        public static NWGameObject GetLastHostileActor(NWGameObject oVictim = null)
        {
            Internal.StackPushObject(oVictim, false);
            Internal.CallBuiltIn(556);
            return Internal.StackPopObject();
        }

        //  Force all the characters of the players who are currently in the game to
        //  be exported to their respective directories i.e. LocalVault/ServerVault/ etc.
        public static void ExportAllCharacters()
        {
            Internal.CallBuiltIn(557);
        }

        //  Get the Day Track for oArea.
        public static int MusicBackgroundGetDayTrack(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(558);
            return Internal.StackPopInteger();
        }

        //  Get the Night Track for oArea.
        public static int MusicBackgroundGetNightTrack(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(559);
            return Internal.StackPopInteger();
        }

        //  Write sLogEntry as a timestamped entry into the log file
        public static void WriteTimestampedLogEntry(string sLogEntry)
        {
            Internal.StackPushString(sLogEntry);
            Internal.CallBuiltIn(560);
        }

        //  Get the module's name in the language of the server that's running it.
        //  * If there is no entry for the language of the server, it will return an
        //    empty string
        public static string GetModuleName()
        {
            Internal.CallBuiltIn(561);
            return Internal.StackPopString();
        }

        //  Get the player leader of the faction of which oMemberOfFaction is a member.
        //  * Returns OBJECT_INVALID if oMemberOfFaction is not a valid creature,
        //    or oMemberOfFaction is a member of a NPC faction.
        public static NWGameObject GetFactionLeader(NWGameObject oMemberOfFaction)
        {
            Internal.StackPushObject(oMemberOfFaction, false);
            Internal.CallBuiltIn(562);
            return Internal.StackPopObject();
        }

        //  Sends szMessage to all the Dungeon Masters currently on the server.
        public static void SendMessageToAllDMs(string szMessage)
        {
            Internal.StackPushString(szMessage);
            Internal.CallBuiltIn(563);
        }

        //  End the currently running game, play sEndMovie then return all players to the
        //  game's main menu.
        public static void EndGame(string sEndMovie)
        {
            Internal.StackPushString(sEndMovie);
            Internal.CallBuiltIn(564);
        }

        //  Remove oPlayer from the server.
        //  You can optionally specify a reason to override the text shown to the player.
        public static void BootPC(NWGameObject oPlayer, string sReason = "")
        {
            Internal.StackPushString(sReason);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(565);
        }

        //  Counterspell oCounterSpellTarget.
        public static void ActionCounterSpell(NWGameObject oCounterSpellTarget)
        {
            Internal.StackPushObject(oCounterSpellTarget, false);
            Internal.CallBuiltIn(566);
        }

        //  Set the ambient day volume for oArea to nVolume.
        //  - oArea
        //  - nVolume: 0 - 100
        public static void AmbientSoundSetDayVolume(NWGameObject oArea, int nVolume)
        {
            Internal.StackPushInteger(nVolume);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(567);
        }

        //  Set the ambient night volume for oArea to nVolume.
        //  - oArea
        //  - nVolume: 0 - 100
        public static void AmbientSoundSetNightVolume(NWGameObject oArea, int nVolume)
        {
            Internal.StackPushInteger(nVolume);
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(568);
        }

        //  Get the Battle Track for oArea.
        public static int MusicBackgroundGetBattleTrack(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(569);
            return Internal.StackPopInteger();
        }

        //  Determine whether oObject has an inventory.
        //  * Returns TRUE for creatures and stores, and checks to see if an item or placeable object is a container.
        //  * Returns FALSE for all other object types.
        public static bool GetHasInventory(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(570);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the duration (in seconds) of the sound attached to nStrRef
        //  * Returns 0.0f if no duration is stored or if no sound is attached
        public static float GetStrRefSoundDuration(int nStrRef)
        {
            Internal.StackPushInteger(nStrRef);
            Internal.CallBuiltIn(571);
            return Internal.StackPopFloat();
        }

        //  Add oPC to oPartyLeader's party.  This will only work on two PCs.
        //  - oPC: player to add to a party
        //  - oPartyLeader: player already in the party
        public static void AddToParty(NWGameObject oPC, NWGameObject oPartyLeader)
        {
            Internal.StackPushObject(oPartyLeader, false);
            Internal.StackPushObject(oPC, false);
            Internal.CallBuiltIn(572);
        }

        //  Remove oPC from their current party. This will only work on a PC.
        //  - oPC: removes this player from whatever party they're currently in.
        public static void RemoveFromParty(NWGameObject oPC)
        {
            Internal.StackPushObject(oPC, false);
            Internal.CallBuiltIn(573);
        }

        //  Returns the stealth mode of the specified creature.
        //  - oCreature
        //  * Returns a constant STEALTH_MODE_*
        public static StealthMode GetStealthMode(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(574);
            return (StealthMode)Internal.StackPopInteger();
        }

        //  Returns the detection mode of the specified creature.
        //  - oCreature
        //  * Returns a constant DETECT_MODE_*
        public static DetectMode GetDetectMode(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(575);
            return (DetectMode)Internal.StackPopInteger();
        }

        //  Returns the defensive casting mode of the specified creature.
        //  - oCreature
        //  * Returns a constant DEFENSIVE_CASTING_MODE_*
        public static DefensiveCastingMode GetDefensiveCastingMode(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(576);
            return (DefensiveCastingMode)Internal.StackPopInteger();
        }

        //  returns the appearance type of the specified creature.
        //  * returns a constant APPEARANCE_TYPE_* for valid creatures
        //  * returns APPEARANCE_TYPE_INVALID for non creatures/invalid creatures
        public static AppearanceType GetAppearanceType(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(577);
            return (AppearanceType)Internal.StackPopInteger();
        }

        //  in an onItemAcquired returns the size of the stack of the item
        //  that was just acquired.
        //  * returns the stack size of the item acquired
        public static int GetModuleItemAcquiredStackSize()
        {
            Internal.CallBuiltIn(579);
            return Internal.StackPopInteger();
        }

        //  Decrement the remaining uses per day for this creature by one.
        //  - oCreature: creature to modify
        //  - nFeat: constant FEAT_*
        public static void DecrementRemainingFeatUses(NWGameObject oCreature, Feat nFeat)
        {
            Internal.StackPushInteger((int)nFeat);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(580);
        }

        //  Decrement the remaining uses per day for this creature by one.
        //  - oCreature: creature to modify
        //  - nSpell: constant SPELL_*
        public static void DecrementRemainingSpellUses(NWGameObject oCreature, Spell nSpell)
        {
            Internal.StackPushInteger((int)nSpell);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(581);
        }

        //  returns the template used to create this object (if appropriate)
        //  * returns an empty string when no template found
        public static string GetResRef(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(582);
            return Internal.StackPopString();
        }

        //  returns an effect that will petrify the target
        //  * currently applies EffectParalyze and the stoneskin visual effect.
        public static Effect EffectPetrify()
        {
            Internal.CallBuiltIn(583);
            return Internal.StackPopEffect();
        }

        //  duplicates the item and returns a new object
        //  oItem - item to copy
        //  oTargetInventory - create item in this object's inventory. If this parameter
        //                     is not valid, the item will be created in oItem's location
        //  bCopyVars - copy the local variables from the old item to the new one
        //  * returns the new item
        //  * returns OBJECT_INVALID for non-items.
        //  * can only copy empty item containers. will return OBJECT_INVALID if oItem contains
        //    other items.
        //  * if it is possible to merge this item with any others in the target location,
        //    then it will do so and return the merged object.
        public static NWGameObject CopyItem(NWGameObject oItem, NWGameObject oTargetInventory = null, bool bCopyVars = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bCopyVars));
            Internal.StackPushObject(oTargetInventory, false);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(584);
            return Internal.StackPopObject();
        }

        //  returns an effect that is guaranteed to paralyze a creature.
        //  this effect is identical to EffectParalyze except that it cannot be resisted.
        public static Effect EffectCutsceneParalyze()
        {
            Internal.CallBuiltIn(585);
            return Internal.StackPopEffect();
        }

        //  returns TRUE if the item CAN be dropped
        //  Droppable items will appear on a creature's remains when the creature is killed.
        public static bool GetDroppableFlag(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(586);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  returns TRUE if the placeable object is usable
        public static bool GetUseableFlag(NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(587);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  returns TRUE if the item is stolen
        public static bool GetStolenFlag(NWGameObject oStolen)
        {
            Internal.StackPushObject(oStolen, false);
            Internal.CallBuiltIn(588);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Duplicates the object specified by oSource.
        //  ONLY creatures and items can be specified.
        //  If an owner is specified and the object is an item, it will be put into their inventory
        //  If the object is a creature, they will be created at the location.
        //  If a new tag is specified, it will be assigned to the new object.
        public static NWGameObject CopyObject(NWGameObject oSource, Location locLocation, NWGameObject oOwner = null, string sNewTag = "")
        {
            Internal.StackPushString(sNewTag);
            Internal.StackPushObject(oOwner, false);
            Internal.StackPushLocation(locLocation);
            Internal.StackPushObject(oSource, false);
            Internal.CallBuiltIn(600);
            var result = Internal.StackPopObject();

            MessageHub.Instance.Publish(new ObjectCreated(result));

            return result;
        }

        //  Returns an effect that is guaranteed to dominate a creature
        //  Like EffectDominated but cannot be resisted
        public static Effect EffectCutsceneDominated()
        {
            Internal.CallBuiltIn(604);
            return Internal.StackPopEffect();
        }

        //  Returns stack size of an item
        //  - oItem: item to query
        public static int GetItemStackSize(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(605);
            return Internal.StackPopInteger();
        }

        //  Sets stack size of an item.
        //  - oItem: item to change
        //  - nSize: new size of stack.  Will be restricted to be between 1 and the
        //    maximum stack size for the item type.  If a value less than 1 is passed it
        //    will set the stack to 1.  If a value greater than the max is passed
        //    then it will set the stack to the maximum size
        public static void SetItemStackSize(NWGameObject oItem, int nSize)
        {
            Internal.StackPushInteger(nSize);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(606);
        }

        //  Returns charges left on an item
        //  - oItem: item to query
        public static int GetItemCharges(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(607);
            return Internal.StackPopInteger();
        }

        //  Sets charges left on an item.
        //  - oItem: item to change
        //  - nCharges: number of charges.  If value below 0 is passed, # charges will
        //    be set to 0.  If value greater than maximum is passed, # charges will
        //    be set to maximum.  If the # charges drops to 0 the item
        //    will be destroyed.
        public static void SetItemCharges(NWGameObject oItem, int nCharges)
        {
            Internal.StackPushInteger(nCharges);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(608);
        }

        //  ***********************  START OF ITEM PROPERTY FUNCTIONS  **********************
        //  adds an item property to the specified item
        //  Only temporary and permanent duration types are allowed.
        public static void AddItemProperty(DurationType nDurationType, ItemProperty ipProperty, NWGameObject oItem, float fDuration = 0.0f)
        {
            Internal.StackPushFloat(fDuration);
            Internal.StackPushObject(oItem, false);
            Internal.StackPushItemProperty(ipProperty);
            Internal.StackPushInteger((int)nDurationType);
            Internal.CallBuiltIn(609);
        }

        //  removes an item property from the specified item
        public static void RemoveItemProperty(NWGameObject oItem, ItemProperty ipProperty)
        {
            Internal.StackPushItemProperty(ipProperty);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(610);
        }

        //  if the item property is valid this will return true
        public static bool GetIsItemPropertyValid(ItemProperty ipProperty)
        {
            Internal.StackPushItemProperty(ipProperty);
            Internal.CallBuiltIn(611);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Gets the first item property on an item
        public static ItemProperty GetFirstItemProperty(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(612);
            return Internal.StackPopItemProperty();
        }

        //  Will keep retrieving the next and the next item property on an Item,
        //  will return an invalid item property when the list is empty.
        public static ItemProperty GetNextItemProperty(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(613);
            return Internal.StackPopItemProperty();
        }

        //  will return the item property type (ie. holy avenger)
        public static ItemPropertyType GetItemPropertyType(ItemProperty ip)
        {
            Internal.StackPushItemProperty(ip);
            Internal.CallBuiltIn(614);
            return (ItemPropertyType)Internal.StackPopInteger();
        }

        //  will return the duration type of the item property
        public static DurationType GetItemPropertyDurationType(ItemProperty ip)
        {
            Internal.StackPushItemProperty(ip);
            Internal.CallBuiltIn(615);
            return (DurationType)Internal.StackPopInteger();
        }

        //  Returns Item property ability bonus.  You need to specify an
        //  ability constant(IP_CONST_ABILITY_*) and the bonus.  The bonus should
        //  be a positive integer between 1 and 12.
        public static ItemProperty ItemPropertyAbilityBonus(Ability nAbility, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nAbility);
            Internal.CallBuiltIn(616);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property AC bonus.  You need to specify the bonus.
        //  The bonus should be a positive integer between 1 and 20. The modifier
        //  type depends on the item it is being applied to.
        public static ItemProperty ItemPropertyACBonus(int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.CallBuiltIn(617);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property AC bonus vs. alignment group.  An example of
        //  an alignment group is Chaotic, or Good.  You need to specify the
        //  alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the AC bonus.
        //  The AC bonus should be an integer between 1 and 20.  The modifier
        //  type depends on the item it is being applied to.
        public static ItemProperty ItemPropertyACBonusVsAlign(IPConst nAlignGroup, int nACBonus)
        {
            Internal.StackPushInteger(nACBonus);
            Internal.StackPushInteger((int)nAlignGroup);
            Internal.CallBuiltIn(618);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property AC bonus vs. Damage type (ie. piercing).  You
        //  need to specify the damage type constant(IP_CONST_DAMAGETYPE_*) and the
        //  AC bonus.  The AC bonus should be an integer between 1 and 20.  The
        //  modifier type depends on the item it is being applied to.
        //  NOTE: Only the first 3 damage types may be used here, the 3 basic
        //        physical types.
        public static ItemProperty ItemPropertyACBonusVsDmgType(IPConst nDamageType, int nACBonus)
        {
            Internal.StackPushInteger(nACBonus);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(619);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property AC bonus vs. Racial group.  You need to specify
        //  the racial group constant(IP_CONST_RACIALTYPE_*) and the AC bonus.  The AC
        //  bonus should be an integer between 1 and 20.  The modifier type depends
        //  on the item it is being applied to.
        public static ItemProperty ItemPropertyACBonusVsRace(IPConst nRace, int nACBonus)
        {
            Internal.StackPushInteger(nACBonus);
            Internal.StackPushInteger((int)nRace);
            Internal.CallBuiltIn(620);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property AC bonus vs. Specific alignment.  You need to
        //  specify the specific alignment constant(IP_CONST_ALIGNMENT_*) and the AC
        //  bonus.  The AC bonus should be an integer between 1 and 20.  The
        //  modifier type depends on the item it is being applied to.
        public static ItemProperty ItemPropertyACBonusVsSAlign(IPConst nAlign, int nACBonus)
        {
            Internal.StackPushInteger(nACBonus);
            Internal.StackPushInteger((int)nAlign);
            Internal.CallBuiltIn(621);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Enhancement bonus.  You need to specify the
        //  enhancement bonus.  The Enhancement bonus should be an integer between
        //  1 and 20.
        public static ItemProperty ItemPropertyEnhancementBonus(int nEnhancementBonus)
        {
            Internal.StackPushInteger(nEnhancementBonus);
            Internal.CallBuiltIn(622);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Enhancement bonus vs. an Alignment group.  You
        //  need to specify the alignment group constant(IP_CONST_ALIGNMENTGROUP_*)
        //  and the enhancement bonus.  The Enhancement bonus should be an integer
        //  between 1 and 20.
        public static ItemProperty ItemPropertyEnhancementBonusVsAlign(IPConst nAlignGroup, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nAlignGroup);
            Internal.CallBuiltIn(623);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Enhancement bonus vs. Racial group.  You need
        //  to specify the racial group constant(IP_CONST_RACIALTYPE_*) and the
        //  enhancement bonus.  The enhancement bonus should be an integer between
        //  1 and 20.
        public static ItemProperty ItemPropertyEnhancementBonusVsRace(IPConst nRace, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nRace);
            Internal.CallBuiltIn(624);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Enhancement bonus vs. a specific alignment.  You
        //  need to specify the alignment constant(IP_CONST_ALIGNMENT_*) and the
        //  enhancement bonus.  The enhancement bonus should be an integer between
        //  1 and 20.
        public static ItemProperty ItemPropertyEnhancementBonusVsSAlign(IPConst nAlign, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nAlign);
            Internal.CallBuiltIn(625);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Enhancment penalty.  You need to specify the
        //  enhancement penalty.  The enhancement penalty should be a POSITIVE
        //  integer between 1 and 5 (ie. 1 = -1).
        public static ItemProperty ItemPropertyEnhancementPenalty(int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.CallBuiltIn(626);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property weight reduction.  You need to specify the weight
        //  reduction constant(IP_CONST_REDUCEDWEIGHT_*).
        public static ItemProperty ItemPropertyWeightReduction(IPConst nReduction)
        {
            Internal.StackPushInteger((int)nReduction);
            Internal.CallBuiltIn(627);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Bonus Feat.  You need to specify the the feat
        //  constant(IP_CONST_FEAT_*).
        public static ItemProperty ItemPropertyBonusFeat(IPConst nFeat)
        {
            Internal.StackPushInteger((int)nFeat);
            Internal.CallBuiltIn(628);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Bonus level spell (Bonus spell of level).  You must
        //  specify the class constant(IP_CONST_CLASS_*) of the bonus spell(MUST BE a
        //  spell casting class) and the level of the bonus spell.  The level of the
        //  bonus spell should be an integer between 0 and 9.
        public static ItemProperty ItemPropertyBonusLevelSpell(IPConst nClass, int nSpellLevel)
        {
            Internal.StackPushInteger(nSpellLevel);
            Internal.StackPushInteger((int)nClass);
            Internal.CallBuiltIn(629);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Cast spell.  You must specify the spell constant
        //  (IP_CONST_CASTSPELL_*) and the number of uses constant(IP_CONST_CASTSPELL_NUMUSES_*).
        //  NOTE: The number after the name of the spell in the constant is the level
        //        at which the spell will be cast.  Sometimes there are multiple copies
        //        of the same spell but they each are cast at a different level.  The higher
        //        the level, the more cost will be added to the item.
        //  NOTE: The list of spells that can be applied to an item will depend on the
        //        item type.  For instance there are spells that can be applied to a wand
        //        that cannot be applied to a potion.  Below is a list of the types and the
        //        spells that are allowed to be placed on them.  If you try to put a cast
        //        spell effect on an item that is not allowed to have that effect it will
        //        not work.
        //  NOTE: Even if spells have multiple versions of different levels they are only
        //        listed below once.
        // 
        //  WANDS:
        //           Acid_Splash
        //           Activate_Item
        //           Aid
        //           Amplify
        //           Animate_Dead
        //           AuraOfGlory
        //           BalagarnsIronHorn
        //           Bane
        //           Banishment
        //           Barkskin
        //           Bestow_Curse
        //           Bigbys_Clenched_Fist
        //           Bigbys_Crushing_Hand
        //           Bigbys_Forceful_Hand
        //           Bigbys_Grasping_Hand
        //           Bigbys_Interposing_Hand
        //           Bless
        //           Bless_Weapon
        //           Blindness/Deafness
        //           Blood_Frenzy
        //           Bombardment
        //           Bulls_Strength
        //           Burning_Hands
        //           Call_Lightning
        //           Camoflage
        //           Cats_Grace
        //           Charm_Monster
        //           Charm_Person
        //           Charm_Person_or_Animal
        //           Clairaudience/Clairvoyance
        //           Clarity
        //           Color_Spray
        //           Confusion
        //           Continual_Flame
        //           Cure_Critical_Wounds
        //           Cure_Light_Wounds
        //           Cure_Minor_Wounds
        //           Cure_Moderate_Wounds
        //           Cure_Serious_Wounds
        //           Darkness
        //           Darkvision
        //           Daze
        //           Death_Ward
        //           Dirge
        //           Dismissal
        //           Dispel_Magic
        //           Displacement
        //           Divine_Favor
        //           Divine_Might
        //           Divine_Power
        //           Divine_Shield
        //           Dominate_Animal
        //           Dominate_Person
        //           Doom
        //           Dragon_Breath_Acid
        //           Dragon_Breath_Cold
        //           Dragon_Breath_Fear
        //           Dragon_Breath_Fire
        //           Dragon_Breath_Gas
        //           Dragon_Breath_Lightning
        //           Dragon_Breath_Paralyze
        //           Dragon_Breath_Sleep
        //           Dragon_Breath_Slow
        //           Dragon_Breath_Weaken
        //           Drown
        //           Eagle_Spledor
        //           Earthquake
        //           Electric_Jolt
        //           Elemental_Shield
        //           Endurance
        //           Endure_Elements
        //           Enervation
        //           Entangle
        //           Entropic_Shield
        //           Etherealness
        //           Expeditious_Retreat
        //           Fear
        //           Find_Traps
        //           Fireball
        //           Firebrand
        //           Flame_Arrow
        //           Flame_Lash
        //           Flame_Strike
        //           Flare
        //           Foxs_Cunning
        //           Freedom_of_Movement
        //           Ghostly_Visage
        //           Ghoul_Touch
        //           Grease
        //           Greater_Magic_Fang
        //           Greater_Magic_Weapon
        //           Grenade_Acid
        //           Grenade_Caltrops
        //           Grenade_Chicken
        //           Grenade_Choking
        //           Grenade_Fire
        //           Grenade_Holy
        //           Grenade_Tangle
        //           Grenade_Thunderstone
        //           Gust_of_wind
        //           Hammer_of_the_Gods
        //           Haste
        //           Hold_Animal
        //           Hold_Monster
        //           Hold_Person
        //           Ice_Storm
        //           Identify
        //           Improved_Invisibility
        //           Inferno
        //           Inflict_Critical_Wounds
        //           Inflict_Light_Wounds
        //           Inflict_Minor_Wounds
        //           Inflict_Moderate_Wounds
        //           Inflict_Serious_Wounds
        //           Invisibility
        //           Invisibility_Purge
        //           Invisibility_Sphere
        //           Isaacs_Greater_Missile_Storm
        //           Isaacs_Lesser_Missile_Storm
        //           Knock
        //           Lesser_Dispel
        //           Lesser_Restoration
        //           Lesser_Spell_Breach
        //           Light
        //           Lightning_Bolt
        //           Mage_Armor
        //           Magic_Circle_against_Alignment
        //           Magic_Fang
        //           Magic_Missile
        //           Manipulate_Portal_Stone
        //           Mass_Camoflage
        //           Melfs_Acid_Arrow
        //           Meteor_Swarm
        //           Mind_Blank
        //           Mind_Fog
        //           Negative_Energy_Burst
        //           Negative_Energy_Protection
        //           Negative_Energy_Ray
        //           Neutralize_Poison
        //           One_With_The_Land
        //           Owls_Insight
        //           Owls_Wisdom
        //           Phantasmal_Killer
        //           Planar_Ally
        //           Poison
        //           Polymorph_Self
        //           Prayer
        //           Protection_from_Alignment
        //           Protection_From_Elements
        //           Quillfire
        //           Ray_of_Enfeeblement
        //           Ray_of_Frost
        //           Remove_Blindness/Deafness
        //           Remove_Curse
        //           Remove_Disease
        //           Remove_Fear
        //           Remove_Paralysis
        //           Resist_Elements
        //           Resistance
        //           Restoration
        //           Sanctuary
        //           Scare
        //           Searing_Light
        //           See_Invisibility
        //           Shadow_Conjuration
        //           Shield
        //           Shield_of_Faith
        //           Silence
        //           Sleep
        //           Slow
        //           Sound_Burst
        //           Spike_Growth
        //           Stinking_Cloud
        //           Stoneskin
        //           Summon_Creature_I
        //           Summon_Creature_I
        //           Summon_Creature_II
        //           Summon_Creature_III
        //           Summon_Creature_IV
        //           Sunburst
        //           Tashas_Hideous_Laughter
        //           True_Strike
        //           Undeaths_Eternal_Foe
        //           Unique_Power
        //           Unique_Power_Self_Only
        //           Vampiric_Touch
        //           Virtue
        //           Wall_of_Fire
        //           Web
        //           Wounding_Whispers
        // 
        //  POTIONS:
        //           Activate_Item
        //           Aid
        //           Amplify
        //           AuraOfGlory
        //           Bane
        //           Barkskin
        //           Barkskin
        //           Barkskin
        //           Bless
        //           Bless_Weapon
        //           Bless_Weapon
        //           Blood_Frenzy
        //           Bulls_Strength
        //           Bulls_Strength
        //           Bulls_Strength
        //           Camoflage
        //           Cats_Grace
        //           Cats_Grace
        //           Cats_Grace
        //           Clairaudience/Clairvoyance
        //           Clairaudience/Clairvoyance
        //           Clairaudience/Clairvoyance
        //           Clarity
        //           Continual_Flame
        //           Cure_Critical_Wounds
        //           Cure_Critical_Wounds
        //           Cure_Critical_Wounds
        //           Cure_Light_Wounds
        //           Cure_Light_Wounds
        //           Cure_Minor_Wounds
        //           Cure_Moderate_Wounds
        //           Cure_Moderate_Wounds
        //           Cure_Moderate_Wounds
        //           Cure_Serious_Wounds
        //           Cure_Serious_Wounds
        //           Cure_Serious_Wounds
        //           Darkness
        //           Darkvision
        //           Darkvision
        //           Death_Ward
        //           Dispel_Magic
        //           Dispel_Magic
        //           Displacement
        //           Divine_Favor
        //           Divine_Might
        //           Divine_Power
        //           Divine_Shield
        //           Dragon_Breath_Acid
        //           Dragon_Breath_Cold
        //           Dragon_Breath_Fear
        //           Dragon_Breath_Fire
        //           Dragon_Breath_Gas
        //           Dragon_Breath_Lightning
        //           Dragon_Breath_Paralyze
        //           Dragon_Breath_Sleep
        //           Dragon_Breath_Slow
        //           Dragon_Breath_Weaken
        //           Eagle_Spledor
        //           Eagle_Spledor
        //           Eagle_Spledor
        //           Elemental_Shield
        //           Elemental_Shield
        //           Endurance
        //           Endurance
        //           Endurance
        //           Endure_Elements
        //           Entropic_Shield
        //           Ethereal_Visage
        //           Ethereal_Visage
        //           Etherealness
        //           Expeditious_Retreat
        //           Find_Traps
        //           Foxs_Cunning
        //           Foxs_Cunning
        //           Foxs_Cunning
        //           Freedom_of_Movement
        //           Ghostly_Visage
        //           Ghostly_Visage
        //           Ghostly_Visage
        //           Globe_of_Invulnerability
        //           Greater_Bulls_Strength
        //           Greater_Cats_Grace
        //           Greater_Dispelling
        //           Greater_Dispelling
        //           Greater_Eagles_Splendor
        //           Greater_Endurance
        //           Greater_Foxs_Cunning
        //           Greater_Magic_Weapon
        //           Greater_Owls_Wisdom
        //           Greater_Restoration
        //           Greater_Spell_Mantle
        //           Greater_Stoneskin
        //           Grenade_Acid
        //           Grenade_Caltrops
        //           Grenade_Chicken
        //           Grenade_Choking
        //           Grenade_Fire
        //           Grenade_Holy
        //           Grenade_Tangle
        //           Grenade_Thunderstone
        //           Haste
        //           Haste
        //           Heal
        //           Hold_Animal
        //           Hold_Monster
        //           Hold_Person
        //           Identify
        //           Invisibility
        //           Lesser_Dispel
        //           Lesser_Dispel
        //           Lesser_Mind_Blank
        //           Lesser_Restoration
        //           Lesser_Spell_Mantle
        //           Light
        //           Light
        //           Mage_Armor
        //           Manipulate_Portal_Stone
        //           Mass_Camoflage
        //           Mind_Blank
        //           Minor_Globe_of_Invulnerability
        //           Minor_Globe_of_Invulnerability
        //           Mordenkainens_Disjunction
        //           Negative_Energy_Protection
        //           Negative_Energy_Protection
        //           Negative_Energy_Protection
        //           Neutralize_Poison
        //           One_With_The_Land
        //           Owls_Insight
        //           Owls_Wisdom
        //           Owls_Wisdom
        //           Owls_Wisdom
        //           Polymorph_Self
        //           Prayer
        //           Premonition
        //           Protection_From_Elements
        //           Protection_From_Elements
        //           Protection_from_Spells
        //           Protection_from_Spells
        //           Raise_Dead
        //           Remove_Blindness/Deafness
        //           Remove_Curse
        //           Remove_Disease
        //           Remove_Fear
        //           Remove_Paralysis
        //           Resist_Elements
        //           Resist_Elements
        //           Resistance
        //           Resistance
        //           Restoration
        //           Resurrection
        //           Rogues_Cunning
        //           See_Invisibility
        //           Shadow_Shield
        //           Shapechange
        //           Shield
        //           Shield_of_Faith
        //           Special_Alcohol_Beer
        //           Special_Alcohol_Spirits
        //           Special_Alcohol_Wine
        //           Special_Herb_Belladonna
        //           Special_Herb_Garlic
        //           Spell_Mantle
        //           Spell_Resistance
        //           Spell_Resistance
        //           Stoneskin
        //           Tensers_Transformation
        //           True_Seeing
        //           True_Strike
        //           Unique_Power
        //           Unique_Power_Self_Only
        //           Virtue
        // 
        //  GENERAL USE (ie. everything else):
        //           Just about every spell is useable by all the general use items so instead we
        //           will only list the ones that are not allowed:
        //           Special_Alcohol_Beer
        //           Special_Alcohol_Spirits
        //           Special_Alcohol_Wine
        // 
        public static ItemProperty ItemPropertyCastSpell(IPConst nSpell, int nNumUses)
        {
            Internal.StackPushInteger(nNumUses);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(630);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage bonus.  You must specify the damage type constant
        //  (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
        //  NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
        //        Cold, Electrical, Fire, Piercing, Slashing, Sonic.
        public static ItemProperty ItemPropertyDamageBonus(IPConst nDamageType, int nDamage)
        {
            Internal.StackPushInteger(nDamage);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(631);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage bonus vs. Alignment groups.  You must specify the
        //  alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the damage type constant
        //  (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
        //  NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
        //        Cold, Electrical, Fire, Piercing, Slashing, Sonic.
        public static ItemProperty ItemPropertyDamageBonusVsAlign(IPConst nAlignGroup, IPConst nDamageType, int nDamage)
        {
            Internal.StackPushInteger(nDamage);
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger((int)nAlignGroup);
            Internal.CallBuiltIn(632);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage bonus vs. specific race.  You must specify the
        //  racial group constant(IP_CONST_RACIALTYPE_*) and the damage type constant
        //  (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
        //  NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
        //        Cold, Electrical, Fire, Piercing, Slashing, Sonic.
        public static ItemProperty ItemPropertyDamageBonusVsRace(IPConst nRace, IPConst nDamageType, int nDamage)
        {
            Internal.StackPushInteger(nDamage);
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger((int)nRace);
            Internal.CallBuiltIn(633);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage bonus vs. specific alignment.  You must specify the
        //  specific alignment constant(IP_CONST_ALIGNMENT_*) and the damage type constant
        //  (IP_CONST_DAMAGETYPE_*) and the amount of damage constant(IP_CONST_DAMAGEBONUS_*).
        //  NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
        //        Cold, Electrical, Fire, Piercing, Slashing, Sonic.
        public static ItemProperty ItemPropertyDamageBonusVsSAlign(IPConst nAlign, IPConst nDamageType, int nDamage)
        {
            Internal.StackPushInteger(nDamage);
            Internal.StackPushInteger((int)nDamageType);
            Internal.StackPushInteger((int)nAlign);
            Internal.CallBuiltIn(634);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage immunity.  You must specify the damage type constant
        //  (IP_CONST_DAMAGETYPE_*) that you want to be immune to and the immune bonus percentage
        //  constant(IP_CONST_DAMAGEIMMUNITY_*).
        //  NOTE: not all the damage types will work, use only the following: Acid, Bludgeoning,
        //        Cold, Electrical, Fire, Piercing, Slashing, Sonic.
        public static ItemProperty ItemPropertyDamageImmunity(IPConst nDamageType, IPConst nImmuneBonus)
        {
            Internal.StackPushInteger((int)nImmuneBonus);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(635);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage penalty.  You must specify the damage penalty.
        //  The damage penalty should be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
        public static ItemProperty ItemPropertyDamagePenalty(int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.CallBuiltIn(636);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage reduction.  You must specify the enhancment level
        //  (IP_CONST_DAMAGEREDUCTION_*) that is required to get past the damage reduction
        //  and the amount of HP of damage constant(IP_CONST_DAMAGESOAK_*) will be soaked
        //  up if your weapon is not of high enough enhancement.
        public static ItemProperty ItemPropertyDamageReduction(IPConst nEnhancement, IPConst nHPSoak)
        {
            Internal.StackPushInteger((int)nHPSoak);
            Internal.StackPushInteger((int)nEnhancement);
            Internal.CallBuiltIn(637);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage resistance.  You must specify the damage type
        //  constant(IP_CONST_DAMAGETYPE_*) and the amount of HP of damage constant
        //  (IP_CONST_DAMAGERESIST_*) that will be resisted against each round.
        public static ItemProperty ItemPropertyDamageResistance(IPConst nDamageType, IPConst nHPResist)
        {
            Internal.StackPushInteger((int)nHPResist);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(638);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property damage vulnerability.  You must specify the damage type
        //  constant(IP_CONST_DAMAGETYPE_*) that you want the user to be extra vulnerable to
        //  and the percentage vulnerability constant(IP_CONST_DAMAGEVULNERABILITY_*).
        public static ItemProperty ItemPropertyDamageVulnerability(IPConst nDamageType, IPConst nVulnerability)
        {
            Internal.StackPushInteger((int)nVulnerability);
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(639);
            return Internal.StackPopItemProperty();
        }

        //  Return Item property Darkvision.
        public static ItemProperty ItemPropertyDarkvision()
        {
            Internal.CallBuiltIn(640);
            return Internal.StackPopItemProperty();
        }

        //  Return Item property decrease ability score.  You must specify the ability
        //  constant(IP_CONST_ABILITY_*) and the modifier constant.  The modifier must be
        //  a POSITIVE integer between 1 and 10 (ie. 1 = -1).
        public static ItemProperty ItemPropertyDecreaseAbility(IPConst nAbility, int nModifier)
        {
            Internal.StackPushInteger(nModifier);
            Internal.StackPushInteger((int)nAbility);
            Internal.CallBuiltIn(641);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property decrease Armor Class.  You must specify the armor
        //  modifier type constant(IP_CONST_ACMODIFIERTYPE_*) and the armor class penalty.
        //  The penalty must be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
        public static ItemProperty ItemPropertyDecreaseAC(IPConst nModifierType, int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.StackPushInteger((int)nModifierType);
            Internal.CallBuiltIn(642);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property decrease skill.  You must specify the constant for the
        //  skill to be decreased(SKILL_*) and the amount of the penalty.  The penalty
        //  must be a POSITIVE integer between 1 and 10 (ie. 1 = -1).
        public static ItemProperty ItemPropertyDecreaseSkill(Skill nSkill, int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(643);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property container reduced weight.  This is used for special
        //  containers that reduce the weight of the objects inside them.  You must
        //  specify the container weight reduction type constant(IP_CONST_CONTAINERWEIGHTRED_*).
        public static ItemProperty ItemPropertyContainerReducedWeight(IPConst nContainerType)
        {
            Internal.StackPushInteger((int)nContainerType);
            Internal.CallBuiltIn(644);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property extra melee damage type.  You must specify the extra
        //  melee base damage type that you want applied.  It is a constant(IP_CONST_DAMAGETYPE_*).
        //  NOTE: only the first 3 base types (piercing, slashing, & bludgeoning are applicable
        //        here.
        //  NOTE: It is also only applicable to melee weapons.
        public static ItemProperty ItemPropertyExtraMeleeDamageType(IPConst nDamageType)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(645);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property extra ranged damage type.  You must specify the extra
        //  melee base damage type that you want applied.  It is a constant(IP_CONST_DAMAGETYPE_*).
        //  NOTE: only the first 3 base types (piercing, slashing, & bludgeoning are applicable
        //        here.
        //  NOTE: It is also only applicable to ranged weapons.
        public static ItemProperty ItemPropertyExtraRangeDamageType(IPConst nDamageType)
        {
            Internal.StackPushInteger((int)nDamageType);
            Internal.CallBuiltIn(646);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property haste.
        public static ItemProperty ItemPropertyHaste()
        {
            Internal.CallBuiltIn(647);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Holy Avenger.
        public static ItemProperty ItemPropertyHolyAvenger()
        {
            Internal.CallBuiltIn(648);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property immunity to miscellaneous effects.  You must specify the
        //  effect to which the user is immune, it is a constant(IP_CONST_IMMUNITYMISC_*).
        public static ItemProperty ItemPropertyImmunityMisc(IPConst nImmunityType)
        {
            Internal.StackPushInteger((int)nImmunityType);
            Internal.CallBuiltIn(649);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property improved evasion.
        public static ItemProperty ItemPropertyImprovedEvasion()
        {
            Internal.CallBuiltIn(650);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property bonus spell resistance.  You must specify the bonus spell
        //  resistance constant(IP_CONST_SPELLRESISTANCEBONUS_*).
        public static ItemProperty ItemPropertyBonusSpellResistance(IPConst nBonus)
        {
            Internal.StackPushInteger((int)nBonus);
            Internal.CallBuiltIn(651);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property saving throw bonus vs. a specific effect or damage type.
        //  You must specify the save type constant(IP_CONST_SAVEVS_*) that the bonus is
        //  applied to and the bonus that is be applied.  The bonus must be an integer
        //  between 1 and 20.
        public static ItemProperty ItemPropertyBonusSavingThrowVsX(IPConst nBonusType, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nBonusType);
            Internal.CallBuiltIn(652);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property saving throw bonus to the base type (ie. will, reflex,
        //  fortitude).  You must specify the base type constant(IP_CONST_SAVEBASETYPE_*)
        //  to which the user gets the bonus and the bonus that he/she will get.  The
        //  bonus must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyBonusSavingThrow(IPConst nBaseSaveType, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nBaseSaveType);
            Internal.CallBuiltIn(653);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property keen.  This means a critical threat range of 19-20 on a
        //  weapon will be increased to 17-20 etc.
        public static ItemProperty ItemPropertyKeen()
        {
            Internal.CallBuiltIn(654);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property light.  You must specify the intesity constant of the
        //  light(IP_CONST_LIGHTBRIGHTNESS_*) and the color constant of the light
        //  (IP_CONST_LIGHTCOLOR_*).
        public static ItemProperty ItemPropertyLight(IPConst nBrightness, IPConst nColor)
        {
            Internal.StackPushInteger((int)nColor);
            Internal.StackPushInteger((int)nBrightness);
            Internal.CallBuiltIn(655);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Max range strength modification (ie. mighty).  You must
        //  specify the maximum modifier for strength that is allowed on a ranged weapon.
        //  The modifier must be a positive integer between 1 and 20.
        public static ItemProperty ItemPropertyMaxRangeStrengthMod(int nModifier)
        {
            Internal.StackPushInteger(nModifier);
            Internal.CallBuiltIn(656);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property no damage.  This means the weapon will do no damage in
        //  combat.
        public static ItemProperty ItemPropertyNoDamage()
        {
            Internal.CallBuiltIn(657);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property on hit -> do effect property.  You must specify the on
        //  hit property constant(IP_CONST_ONHIT_*) and the save DC constant(IP_CONST_ONHIT_SAVEDC_*).
        //  Some of the item properties require a special parameter as well.  If the
        //  property does not require one you may leave out the last one.  The list of
        //  the ones with 3 parameters and what they are are as follows:
        //       ABILITYDRAIN      :nSpecial is the ability it is to drain.
        //                          constant(IP_CONST_ABILITY_*)
        //       BLINDNESS         :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       CONFUSION         :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       DAZE              :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       DEAFNESS          :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       DISEASE           :nSpecial is the type of desease that will effect the victim.
        //                          constant(DISEASE_*)
        //       DOOM              :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       FEAR              :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       HOLD              :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       ITEMPOISON        :nSpecial is the type of poison that will effect the victim.
        //                          constant(IP_CONST_POISON_*)
        //       SILENCE           :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       SLAYRACE          :nSpecial is the race that will be slain.
        //                          constant(IP_CONST_RACIALTYPE_*)
        //       SLAYALIGNMENTGROUP:nSpecial is the alignment group that will be slain(ie. chaotic).
        //                          constant(IP_CONST_ALIGNMENTGROUP_*)
        //       SLAYALIGNMENT     :nSpecial is the specific alignment that will be slain.
        //                          constant(IP_CONST_ALIGNMENT_*)
        //       SLEEP             :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       SLOW              :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        //       STUN              :nSpecial is the duration/percentage of effecting victim.
        //                          constant(IP_CONST_ONHIT_DURATION_*)
        public static ItemProperty ItemPropertyOnHitProps(IPConst nProperty, IPConst nSaveDC, int nSpecial = 0)
        {
            Internal.StackPushInteger(nSpecial);
            Internal.StackPushInteger((int)nSaveDC);
            Internal.StackPushInteger((int)nProperty);
            Internal.CallBuiltIn(658);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property reduced saving throw vs. an effect or damage type.  You must
        //  specify the constant to which the penalty applies(IP_CONST_SAVEVS_*) and the
        //  penalty to be applied.  The penalty must be a POSITIVE integer between 1 and 20
        //  (ie. 1 = -1).
        public static ItemProperty ItemPropertyReducedSavingThrowVsX(IPConst nBaseSaveType, int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.StackPushInteger((int)nBaseSaveType);
            Internal.CallBuiltIn(659);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property reduced saving to base type.  You must specify the base
        //  type to which the penalty applies (ie. will, reflex, or fortitude) and the penalty
        //  to be applied.  The constant for the base type starts with (IP_CONST_SAVEBASETYPE_*).
        //  The penalty must be a POSITIVE integer between 1 and 20 (ie. 1 = -1).
        public static ItemProperty ItemPropertyReducedSavingThrow(IPConst nBonusType, int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.StackPushInteger((int)nBonusType);
            Internal.CallBuiltIn(660);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property regeneration.  You must specify the regeneration amount.
        //  The amount must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyRegeneration(int nRegenAmount)
        {
            Internal.StackPushInteger(nRegenAmount);
            Internal.CallBuiltIn(661);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property skill bonus.  You must specify the skill to which the user
        //  will get a bonus(SKILL_*) and the amount of the bonus.  The bonus amount must
        //  be an integer between 1 and 50.
        public static ItemProperty ItemPropertySkillBonus(Skill nSkill, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nSkill);
            Internal.CallBuiltIn(662);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property spell immunity vs. specific spell.  You must specify the
        //  spell to which the user will be immune(IP_CONST_IMMUNITYSPELL_*).
        public static ItemProperty ItemPropertySpellImmunitySpecific(Spell nSpell)
        {
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(663);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property spell immunity vs. spell school.  You must specify the
        //  school to which the user will be immune(IP_CONST_SPELLSCHOOL_*).
        public static ItemProperty ItemPropertySpellImmunitySchool(IPConst nSchool)
        {
            Internal.StackPushInteger((int)nSchool);
            Internal.CallBuiltIn(664);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Thieves tools.  You must specify the modifier you wish
        //  the tools to have.  The modifier must be an integer between 1 and 12.
        public static ItemProperty ItemPropertyThievesTools(int nModifier)
        {
            Internal.StackPushInteger(nModifier);
            Internal.CallBuiltIn(665);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Attack bonus.  You must specify an attack bonus.  The bonus
        //  must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyAttackBonus(int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.CallBuiltIn(666);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Attack bonus vs. alignment group.  You must specify the
        //  alignment group constant(IP_CONST_ALIGNMENTGROUP_*) and the attack bonus.  The
        //  bonus must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyAttackBonusVsAlign(IPConst nAlignGroup, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nAlignGroup);
            Internal.CallBuiltIn(667);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property attack bonus vs. racial group.  You must specify the
        //  racial group constant(IP_CONST_RACIALTYPE_*) and the attack bonus.  The bonus
        //  must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyAttackBonusVsRace(IPConst nRace, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nRace);
            Internal.CallBuiltIn(668);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property attack bonus vs. a specific alignment.  You must specify
        //  the alignment you want the bonus to work against(IP_CONST_ALIGNMENT_*) and the
        //  attack bonus.  The bonus must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyAttackBonusVsSAlign(IPConst nAlignment, int nBonus)
        {
            Internal.StackPushInteger(nBonus);
            Internal.StackPushInteger((int)nAlignment);
            Internal.CallBuiltIn(669);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property attack penalty.  You must specify the attack penalty.
        //  The penalty must be a POSITIVE integer between 1 and 5 (ie. 1 = -1).
        public static ItemProperty ItemPropertyAttackPenalty(int nPenalty)
        {
            Internal.StackPushInteger(nPenalty);
            Internal.CallBuiltIn(670);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property unlimited ammo.  If you leave the parameter field blank
        //  it will be just a normal bolt, arrow, or bullet.  However you may specify that
        //  you want the ammunition to do special damage (ie. +1d6 Fire, or +1 enhancement
        //  bonus).  For this parmeter you use the constants beginning with:
        //       (IP_CONST_UNLIMITEDAMMO_*).
        public static ItemProperty ItemPropertyUnlimitedAmmo(IPConst nAmmoDamage = IPConst.UnlimitedAmmo_Basic)
        {
            Internal.StackPushInteger((int)nAmmoDamage);
            Internal.CallBuiltIn(671);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property limit use by alignment group.  You must specify the
        //  alignment group(s) that you want to be able to use this item(IP_CONST_ALIGNMENTGROUP_*).
        public static ItemProperty ItemPropertyLimitUseByAlign(IPConst nAlignGroup)
        {
            Internal.StackPushInteger((int)nAlignGroup);
            Internal.CallBuiltIn(672);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property limit use by class.  You must specify the class(es) who
        //  are able to use this item(IP_CONST_CLASS_*).
        public static ItemProperty ItemPropertyLimitUseByClass(IPConst nClass)
        {
            Internal.StackPushInteger((int)nClass);
            Internal.CallBuiltIn(673);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property limit use by race.  You must specify the race(s) who are
        //  allowed to use this item(IP_CONST_RACIALTYPE_*).
        public static ItemProperty ItemPropertyLimitUseByRace(IPConst nRace)
        {
            Internal.StackPushInteger((int)nRace);
            Internal.CallBuiltIn(674);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property limit use by specific alignment.  You must specify the
        //  alignment(s) of those allowed to use the item(IP_CONST_ALIGNMENT_*).
        public static ItemProperty ItemPropertyLimitUseBySAlign(IPConst nAlignment)
        {
            Internal.StackPushInteger((int)nAlignment);
            Internal.CallBuiltIn(675);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property vampiric regeneration.  You must specify the amount of
        //  regeneration.  The regen amount must be an integer between 1 and 20.
        public static ItemProperty ItemPropertyVampiricRegeneration(int nRegenAmount)
        {
            Internal.StackPushInteger(nRegenAmount);
            Internal.CallBuiltIn(677);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Trap.  You must specify the trap level constant
        //  (IP_CONST_TRAPSTRENGTH_*) and the trap type constant(IP_CONST_TRAPTYPE_*).
        public static ItemProperty ItemPropertyTrap(IPConst nTrapLevel, IPConst nTrapType)
        {
            Internal.StackPushInteger((int)nTrapType);
            Internal.StackPushInteger((int)nTrapLevel);
            Internal.CallBuiltIn(678);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property true seeing.
        public static ItemProperty ItemPropertyTrueSeeing()
        {
            Internal.CallBuiltIn(679);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Monster on hit apply effect property.  You must specify
        //  the property that you want applied on hit.  There are some properties that
        //  require an additional special parameter to be specified.  The others that
        //  don't require any additional parameter you may just put in the one.  The
        //  special cases are as follows:
        //       ABILITYDRAIN:nSpecial is the ability to drain.
        //                    constant(IP_CONST_ABILITY_*)
        //       DISEASE     :nSpecial is the disease that you want applied.
        //                    constant(DISEASE_*)
        //       LEVELDRAIN  :nSpecial is the number of levels that you want drained.
        //                    integer between 1 and 5.
        //       POISON      :nSpecial is the type of poison that will effect the victim.
        //                    constant(IP_CONST_POISON_*)
        //       WOUNDING    :nSpecial is the amount of wounding.
        //                    integer between 1 and 5.
        //  NOTE: Any that do not appear in the above list do not require the second
        //        parameter.
        //  NOTE: These can only be applied to monster NATURAL weapons (ie. bite, claw,
        //        gore, and slam).  IT WILL NOT WORK ON NORMAL WEAPONS.
        public static ItemProperty ItemPropertyOnMonsterHitProperties(int nProperty, int nSpecial = 0)
        {
            Internal.StackPushInteger(nSpecial);
            Internal.StackPushInteger(nProperty);
            Internal.CallBuiltIn(680);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property turn resistance.  You must specify the resistance bonus.
        //  The bonus must be an integer between 1 and 50.
        public static ItemProperty ItemPropertyTurnResistance(int nModifier)
        {
            Internal.StackPushInteger(nModifier);
            Internal.CallBuiltIn(681);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Massive Critical.  You must specify the extra damage
        //  constant(IP_CONST_DAMAGEBONUS_*) of the criticals.
        public static ItemProperty ItemPropertyMassiveCritical(IPConst nDamage)
        {
            Internal.StackPushInteger((int)nDamage);
            Internal.CallBuiltIn(682);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property free action.
        public static ItemProperty ItemPropertyFreeAction()
        {
            Internal.CallBuiltIn(683);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property monster damage.  You must specify the amount of damage
        //  the monster's attack will do(IP_CONST_MONSTERDAMAGE_*).
        //  NOTE: These can only be applied to monster NATURAL weapons (ie. bite, claw,
        //        gore, and slam).  IT WILL NOT WORK ON NORMAL WEAPONS.
        public static ItemProperty ItemPropertyMonsterDamage(IPConst nDamage)
        {
            Internal.StackPushInteger((int)nDamage);
            Internal.CallBuiltIn(684);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property immunity to spell level.  You must specify the level of
        //  which that and below the user will be immune.  The level must be an integer
        //  between 1 and 9.  By putting in a 3 it will mean the user is immune to all
        //  3rd level and lower spells.
        public static ItemProperty ItemPropertyImmunityToSpellLevel(int nLevel)
        {
            Internal.StackPushInteger(nLevel);
            Internal.CallBuiltIn(685);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property special walk.  If no parameters are specified it will
        //  automatically use the zombie walk.  This will apply the special walk animation
        //  to the user.
        public static ItemProperty ItemPropertySpecialWalk(int nWalkType = 0)
        {
            Internal.StackPushInteger(nWalkType);
            Internal.CallBuiltIn(686);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property healers kit.  You must specify the level of the kit.
        //  The modifier must be an integer between 1 and 12.
        public static ItemProperty ItemPropertyHealersKit(int nModifier)
        {
            Internal.StackPushInteger(nModifier);
            Internal.CallBuiltIn(687);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property weight increase.  You must specify the weight increase
        //  constant(IP_CONST_WEIGHTINCREASE_*).
        public static ItemProperty ItemPropertyWeightIncrease(IPConst nWeight)
        {
            Internal.StackPushInteger((int)nWeight);
            Internal.CallBuiltIn(688);
            return Internal.StackPopItemProperty();
        }

        //  ***********************  END OF ITEM PROPERTY FUNCTIONS  **************************
        //  Returns true if 1d20 roll + skill rank is greater than or equal to difficulty
        //  - oTarget: the creature using the skill
        //  - nSkill: the skill being used
        //  - nDifficulty: Difficulty class of skill
        public static bool GetIsSkillSuccessful(NWGameObject oTarget, Skill nSkill, int nDifficulty)
        {
            Internal.StackPushInteger(nDifficulty);
            Internal.StackPushInteger((int)nSkill);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(689);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Creates an effect that inhibits spells
        //  - nPercent - percentage of failure
        //  - nSpellSchool - the school of spells affected.
        public static Effect EffectSpellFailure(int nPercent = 100, SpellSchool nSpellSchool = SpellSchool.General)
        {
            Internal.StackPushInteger((int)nSpellSchool);
            Internal.StackPushInteger(nPercent);
            Internal.CallBuiltIn(690);
            return Internal.StackPopEffect();
        }

        //  Causes the object to instantly speak a translated string.
        //  (not an action, not blocked when uncommandable)
        //  - nStrRef: Reference of the string in the talk table
        //  - nTalkVolume: TALKVOLUME_*
        public static void SpeakStringByStrRef(int nStrRef, TalkVolume nTalkVolume = TalkVolume.Talk)
        {
            Internal.StackPushInteger((int)nTalkVolume);
            Internal.StackPushInteger(nStrRef);
            Internal.CallBuiltIn(691);
        }

        //  Sets the given creature into cutscene mode.  This prevents the player from
        //  using the GUI and camera controls.
        //  - oCreature: creature in a cutscene
        //  - nInCutscene: TRUE to move them into cutscene, FALSE to remove cutscene mode
        //  - nLeftClickingEnabled: TRUE to allow the user to interact with the game world using the left mouse button only.
        //                          FALSE to stop the user from interacting with the game world.
        //  Note: SetCutsceneMode(oPlayer, TRUE) will also make the player 'plot' (unkillable).
        //  SetCutsceneMode(oPlayer, FALSE) will restore the player's plot flag to what it
        //  was when SetCutsceneMode(oPlayer, TRUE) was called.
        public static void SetCutsceneMode(NWGameObject oCreature, bool nInCutscene = true, bool nLeftClickingEnabled = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(nLeftClickingEnabled));
            Internal.StackPushInteger(Convert.ToInt32(nInCutscene));
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(692);
        }

        //  Gets the last player character to cancel from a cutscene.
        public static NWGameObject GetLastPCToCancelCutscene()
        {
            Internal.CallBuiltIn(693);
            return Internal.StackPopObject();
        }

        //  Gets the length of the specified wavefile, in seconds
        //  Only works for sounds used for dialog.
        public static float GetDialogSoundLength(int nStrRef)
        {
            Internal.StackPushInteger(nStrRef);
            Internal.CallBuiltIn(694);
            return Internal.StackPopFloat();
        }

        //  Fades the screen for the given creature/player from black to regular screen
        //  - oCreature: creature controlled by player that should fade from black
        public static void FadeFromBlack(NWGameObject oCreature, float fSpeed = FadeSpeed.Medium)
        {
            Internal.StackPushFloat(fSpeed);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(695);
        }

        //  Fades the screen for the given creature/player from regular screen to black
        //  - oCreature: creature controlled by player that should fade to black
        public static void FadeToBlack(NWGameObject oCreature, float fSpeed = FadeSpeed.Medium)
        {
            Internal.StackPushFloat(fSpeed);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(696);
        }

        //  Removes any fading or black screen.
        //  - oCreature: creature controlled by player that should be cleared
        public static void StopFade(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(697);
        }

        //  Sets the screen to black.  Can be used in preparation for a fade-in (FadeFromBlack)
        //  Can be cleared by either doing a FadeFromBlack, or by calling StopFade.
        //  - oCreature: creature controlled by player that should see black screen
        public static void BlackScreen(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(698);
        }

        //  Returns the base attach bonus for the given creature.
        public static int GetBaseAttackBonus(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(699);
            return Internal.StackPopInteger();
        }

        //  Set a creature's immortality flag.
        //  -oCreature: creature affected
        //  -bImmortal: TRUE = creature is immortal and cannot be killed (but still takes damage)
        //              FALSE = creature is not immortal and is damaged normally.
        //  This scripting command only works on Creature objects.
        public static void SetImmortal(NWGameObject oCreature, bool bImmortal)
        {
            Internal.StackPushInteger(Convert.ToInt32(bImmortal));
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(700);
        }

        //  Open's this creature's inventory panel for this player
        //  - oCreature: creature to view
        //  - oPlayer: the owner of this creature will see the panel pop up
        //  * DM's can view any creature's inventory
        //  * Players can view their own inventory, or that of their henchman, familiar or animal companion
        public static void OpenInventory(NWGameObject oCreature, NWGameObject oPlayer)
        {
            Internal.StackPushObject(oPlayer, false);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(701);
        }

        //  Stores the current camera mode and position so that it can be restored (using
        //  RestoreCameraFacing())
        public static void StoreCameraFacing()
        {
            Internal.CallBuiltIn(702);
        }

        //  Restores the camera mode and position to what they were last time StoreCameraFacing
        //  was called.  RestoreCameraFacing can only be called once, and must correspond to a
        //  previous call to StoreCameraFacing.
        public static void RestoreCameraFacing()
        {
            Internal.CallBuiltIn(703);
        }

        //  Levels up a creature using default settings.
        //  If successfull it returns the level the creature now is, or 0 if it fails.
        //  If you want to give them a different level (ie: Give a Fighter a level of Wizard)
        //     you can specify that in the nClass.
        //  However, if you specify a class to which the creature no package specified,
        //    they will use the default package for that class for their levelup choices.
        //    (ie: no Barbarian Savage/Wizard Divination combinations)
        //  If you turn on bReadyAllSpells, all memorized spells will be ready to cast without resting.
        //  if nPackage is PACKAGE_INVALID then it will use the starting package assigned to that class or just the class package
        public static int LevelUpHenchman(NWGameObject oCreature, ClassType nClass = ClassType.Invalid, bool bReadyAllSpells = false, Package nPackage = Package.Invalid)
        {
            Internal.StackPushInteger((int)nPackage);
            Internal.StackPushInteger(Convert.ToInt32(bReadyAllSpells));
            Internal.StackPushInteger((int)nClass);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(704);
            return Internal.StackPopInteger();
        }

        //  Sets the droppable flag on an item
        //  - oItem: the item to change
        //  - bDroppable: TRUE or FALSE, whether the item should be droppable
        //  Droppable items will appear on a creature's remains when the creature is killed.
        public static void SetDroppableFlag(NWGameObject oItem, bool bDroppable)
        {
            Internal.StackPushInteger(Convert.ToInt32(bDroppable));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(705);
        }

        //  Gets the weight of an item, or the total carried weight of a creature in tenths
        //  of pounds (as per the baseitems.2da).
        //  - oTarget: the item or creature for which the weight is needed
        public static int GetWeight(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(706);
            return Internal.StackPopInteger();
        }

        //  Gets the object that acquired the module item.  May be a creature, item, or placeable
        public static NWGameObject GetModuleItemAcquiredBy()
        {
            Internal.CallBuiltIn(707);
            return Internal.StackPopObject();
        }

        //  Get the immortal flag on a creature
        public static bool GetImmortal(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(708);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Does a single attack on every hostile creature within 10ft. of the attacker
        //  and determines damage accordingly.  If the attacker has a ranged weapon
        //  equipped, this will have no effect.
        //  ** NOTE ** This is meant to be called inside the spell script for whirlwind
        //  attack, it is not meant to be used to queue up a new whirlwind attack.  To do
        //  that you need to call ActionUseFeat(FEAT_WHIRLWIND_ATTACK, oEnemy)
        //  - int bDisplayFeedback: TRUE or FALSE, whether or not feedback should be
        //    displayed
        //  - int bImproved: If TRUE, the improved version of whirlwind is used
        public static void DoWhirlwindAttack(bool bDisplayFeedback = true, bool bImproved = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bImproved));
            Internal.StackPushInteger(Convert.ToInt32(bDisplayFeedback));
            Internal.CallBuiltIn(709);
        }

        //  Gets a value from a 2DA file on the server and returns it as a string
        //  avoid using this function in loops
        //  - s2DA: the name of the 2da file, 16 chars max
        //  - sColumn: the name of the column in the 2da
        //  - nRow: the row in the 2da
        //  * returns an empty string if file, row, or column not found
        public static string Get2DAString(string s2DA, string sColumn, int nRow)
        {
            Internal.StackPushInteger(nRow);
            Internal.StackPushString(sColumn);
            Internal.StackPushString(s2DA);
            Internal.CallBuiltIn(710);
            return Internal.StackPopString();
        }

        //  Returns an effect of type EFFECT_TYPE_ETHEREAL which works just like EffectSanctuary
        //  except that the observers get no saving throw
        public static Effect EffectEthereal()
        {
            Internal.CallBuiltIn(711);
            return Internal.StackPopEffect();
        }

        //  Gets the current AI Level that the creature is running at.
        //  Returns one of the following:
        //  AI_LEVEL_INVALID, AI_LEVEL_VERY_LOW, AI_LEVEL_LOW, AI_LEVEL_NORMAL, AI_LEVEL_HIGH, AI_LEVEL_VERY_HIGH
        public static AILevel GetAILevel(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(712);
            return (AILevel)Internal.StackPopInteger();
        }

        //  Sets the current AI Level of the creature to the value specified. Does not work on Players.
        //  The game by default will choose an appropriate AI level for
        //  creatures based on the circumstances that the creature is in.
        //  Explicitly setting an AI level will over ride the game AI settings.
        //  The new setting will last until SetAILevel is called again with the argument AI_LEVEL_DEFAULT.
        //  AI_LEVEL_DEFAULT  - Default setting. The game will take over seting the appropriate AI level when required.
        //  AI_LEVEL_VERY_LOW - Very Low priority, very stupid, but low CPU usage for AI. Typically used when no players are in the area.
        //  AI_LEVEL_LOW      - Low priority, mildly stupid, but slightly more CPU usage for AI. Typically used when not in combat, but a player is in the area.
        //  AI_LEVEL_NORMAL   - Normal priority, average AI, but more CPU usage required for AI. Typically used when creature is in combat.
        //  AI_LEVEL_HIGH     - High priority, smartest AI, but extremely high CPU usage required for AI. Avoid using this. It is most likely only ever needed for cutscenes.
        public static void SetAILevel(NWGameObject oTarget, AILevel nAILevel)
        {
            Internal.StackPushInteger((int)nAILevel);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(713);
        }

        //  This will return TRUE if the creature running the script is a familiar currently
        //  possessed by his master.
        //  returns FALSE if not or if the creature object is invalid
        public static bool GetIsPossessedFamiliar(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(714);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  This will cause a Player Creature to unpossess his/her familiar.  It will work if run
        //  on the player creature or the possessed familiar.  It does not work in conjunction with
        //  any DM possession.
        public static void UnpossessFamiliar(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(715);
        }

        //  This will return TRUE if the area is flagged as either interior or underground.
        public static bool GetIsAreaInterior(NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(716);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Send a server message (szMessage) to the oPlayer.
        public static void SendMessageToPCByStrRef(NWGameObject oPlayer, int nStrRef)
        {
            Internal.StackPushInteger(nStrRef);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(717);
        }

        //  Increment the remaining uses per day for this creature by one.
        //  Total number of feats per day can not exceed the maximum.
        //  - oCreature: creature to modify
        //  - nFeat: constant FEAT_*
        public static void IncrementRemainingFeatUses(NWGameObject oCreature, Feat nFeat)
        {
            Internal.StackPushInteger((int)nFeat);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(718);
        }

        //  Force the character of the player specified to be exported to its respective directory
        //  i.e. LocalVault/ServerVault/ etc.
        public static void ExportSingleCharacter(NWGameObject oPlayer)
        {
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(719);
        }

        //  This will play a sound that is associated with a stringRef, it will be a mono sound from the location of the object running the command.
        //  if nRunAsAction is off then the sound is forced to play intantly.
        public static void PlaySoundByStrRef(int nStrRef, bool nRunAsAction = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nRunAsAction));
            Internal.StackPushInteger(nStrRef);
            Internal.CallBuiltIn(720);
        }

        //  Set the name of oCreature's sub race to sSubRace.
        public static void SetSubRace(NWGameObject oCreature, string sSubRace)
        {
            Internal.StackPushString(sSubRace);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(721);
        }

        //  Set the name of oCreature's Deity to sDeity.
        public static void SetDeity(NWGameObject oCreature, string sDeity)
        {
            Internal.StackPushString(sDeity);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(722);
        }

        //  Returns TRUE if the creature oCreature is currently possessed by a DM character.
        //  Returns FALSE otherwise.
        //  Note: GetIsDMPossessed() will return FALSE if oCreature is the DM character.
        //  To determine if oCreature is a DM character use GetIsDM()
        public static bool GetIsDMPossessed(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(723);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Gets the current weather conditions for the area oArea.
        //    Returns: WEATHER_CLEAR, WEATHER_RAIN, WEATHER_SNOW, WEATHER_INVALID
        //    Note: If called on an Interior area, this will always return WEATHER_CLEAR.
        public static int GetWeather(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(724);
            return Internal.StackPopInteger();
        }

        //  Returns AREA_NATURAL if the area oArea is natural, AREA_ARTIFICIAL otherwise.
        //  Returns AREA_INVALID, on an error.
        public static int GetIsAreaNatural(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(725);
            return Internal.StackPopInteger();
        }

        //  Returns AREA_ABOVEGROUND if the area oArea is above ground, AREA_UNDERGROUND otherwise.
        //  Returns AREA_INVALID, on an error.
        public static int GetIsAreaAboveGround(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(726);
            return Internal.StackPopInteger();
        }

        //  Use this to get the item last equipped by a player character in OnPlayerEquipItem..
        public static NWGameObject GetPCItemLastEquipped()
        {
            Internal.CallBuiltIn(727);
            return Internal.StackPopObject();
        }

        //  Use this to get the player character who last equipped an item in OnPlayerEquipItem..
        public static NWGameObject GetPCItemLastEquippedBy()
        {
            Internal.CallBuiltIn(728);
            return Internal.StackPopObject();
        }

        //  Use this to get the item last unequipped by a player character in OnPlayerEquipItem..
        public static NWGameObject GetPCItemLastUnequipped()
        {
            Internal.CallBuiltIn(729);
            return Internal.StackPopObject();
        }

        //  Use this to get the player character who last unequipped an item in OnPlayerUnEquipItem..
        public static NWGameObject GetPCItemLastUnequippedBy()
        {
            Internal.CallBuiltIn(730);
            return Internal.StackPopObject();
        }

        //  Creates a new copy of an item, while making a single change to the appearance of the item.
        //  Helmet models and simple items ignore iIndex.
        //  iType                            iIndex                              iNewValue
        //  ITEM_APPR_TYPE_SIMPLE_MODEL      [Ignored]                           Model #
        //  ITEM_APPR_TYPE_WEAPON_COLOR      ITEM_APPR_WEAPON_COLOR_*            1-4
        //  ITEM_APPR_TYPE_WEAPON_MODEL      ITEM_APPR_WEAPON_MODEL_*            Model #
        //  ITEM_APPR_TYPE_ARMOR_MODEL       ITEM_APPR_ARMOR_MODEL_*             Model #
        //  ITEM_APPR_TYPE_ARMOR_COLOR       ITEM_APPR_ARMOR_COLOR_* [0]         0-175 [1]
        // 
        //  [0] Alternatively, where ITEM_APPR_TYPE_ARMOR_COLOR is specified, if per-part coloring is
        //  desired, the following equation can be used for nIndex to achieve that:
        // 
        //    ITEM_APPR_ARMOR_NUM_COLORS + (ITEM_APPR_ARMOR_MODEL_ * ITEM_APPR_ARMOR_NUM_COLORS) + ITEM_APPR_ARMOR_COLOR_
        // 
        //  For example, to change the CLOTH1 channel of the torso, nIndex would be:
        // 
        //    6 + (7 * 6) + 2 = 50
        // 
        //  [1] When specifying per-part coloring, the value 255 is allowed and corresponds with the logical
        //  function 'clear colour override', which clears the per-part override for that part.
        public static NWGameObject CopyItemAndModify(NWGameObject oItem, ItemApprType nType, int nIndex, int nNewValue, bool bCopyVars = false)
        {
            Internal.StackPushInteger(Convert.ToInt32(bCopyVars));
            Internal.StackPushInteger(nNewValue);
            Internal.StackPushInteger(nIndex);
            Internal.StackPushInteger((int)nType);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(731);
            return Internal.StackPopObject();
        }

        //  Queries the current value of the appearance settings on an item. The parameters are
        //  identical to those of CopyItemAndModify().
        public static int GetItemAppearance(NWGameObject oItem, ItemApprType nType, int nIndex)
        {
            Internal.StackPushInteger(nIndex);
            Internal.StackPushInteger((int)nType);
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(732);
            return Internal.StackPopInteger();
        }

        //  Creates an item property that (when applied to a weapon item) causes a spell to be cast
        //  when a successful strike is made, or (when applied to armor) is struck by an opponent.
        //  - nSpell uses the IP_CONST_ONHIT_CASTSPELL_* constants
        public static ItemProperty ItemPropertyOnHitCastSpell(IPConst nSpell, int nLevel)
        {
            Internal.StackPushInteger(nLevel);
            Internal.StackPushInteger((int)nSpell);
            Internal.CallBuiltIn(733);
            return Internal.StackPopItemProperty();
        }

        //  Returns the SubType number of the item property. See the 2DA files for value definitions.
        public static int GetItemPropertySubType(ItemProperty iProperty)
        {
            Internal.StackPushItemProperty(iProperty);
            Internal.CallBuiltIn(734);
            return Internal.StackPopInteger();
        }

        //  Gets the status of ACTION_MODE_* modes on a creature.
        public static ActionMode GetActionMode(NWGameObject oCreature, int nMode)
        {
            Internal.StackPushInteger(nMode);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(735);
            return (ActionMode)Internal.StackPopInteger();
        }

        //  Sets the status of modes ACTION_MODE_* on a creature.
        public static void SetActionMode(NWGameObject oCreature, ActionMode nMode, int nStatus)
        {
            Internal.StackPushInteger(nStatus);
            Internal.StackPushInteger((int)nMode);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(736);
        }

        //  Returns the current arcane spell failure factor of a creature
        public static int GetArcaneSpellFailure(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(737);
            return Internal.StackPopInteger();
        }

        //  Makes a player examine the object oExamine. This causes the examination
        //  pop-up box to appear for the object specified.
        public static void ActionExamine(NWGameObject oExamine)
        {
            Internal.StackPushObject(oExamine, false);
            Internal.CallBuiltIn(738);
        }

        //  Creates a visual effect (ITEM_VISUAL_*) that may be applied to
        //  melee weapons only.
        public static ItemProperty ItemPropertyVisualEffect(ItemVisual nEffect)
        {
            Internal.StackPushInteger((int)nEffect);
            Internal.CallBuiltIn(739);
            return Internal.StackPopItemProperty();
        }

        //  Sets the lootable state of a *living* NPC creature.
        //  This function will *not* work on players or dead creatures.
        public static void SetLootable(NWGameObject oCreature, bool bLootable)
        {
            Internal.StackPushInteger(Convert.ToInt32(bLootable));
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(740);
        }

        //  Returns the lootable state of a creature.
        public static bool GetLootable(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(741);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Returns the current movement rate factor
        //  of the cutscene 'camera man'.
        //  NOTE: This will be a value between 0.1, 2.0 (10%-200%)
        public static float GetCutsceneCameraMoveRate(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(742);
            return Internal.StackPopFloat();
        }

        //  Sets the current movement rate factor for the cutscene
        //  camera man.
        //  NOTE: You can only set values between 0.1, 2.0 (10%-200%)
        public static void SetCutsceneCameraMoveRate(NWGameObject oCreature, float fRate)
        {
            Internal.StackPushFloat(fRate);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(743);
        }

        //  Returns TRUE if the item is cursed and cannot be dropped
        public static bool GetItemCursedFlag(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(744);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  When cursed, items cannot be dropped
        public static void SetItemCursedFlag(NWGameObject oItem, bool nCursed)
        {
            Internal.StackPushInteger(Convert.ToInt32(nCursed));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(745);
        }

        //  Sets the maximum number of henchmen
        public static void SetMaxHenchmen(int nNumHenchmen)
        {
            Internal.StackPushInteger(nNumHenchmen);
            Internal.CallBuiltIn(746);
        }

        //  Gets the maximum number of henchmen
        public static int GetMaxHenchmen()
        {
            Internal.CallBuiltIn(747);
            return Internal.StackPopInteger();
        }

        //  Returns the associate type of the specified creature.
        //  - Returns ASSOCIATE_TYPE_NONE if the creature is not the associate of anyone.
        public static AssociateType GetAssociateType(NWGameObject oAssociate)
        {
            Internal.StackPushObject(oAssociate, false);
            Internal.CallBuiltIn(748);
            return (AssociateType)Internal.StackPopInteger();
        }

        //  Returns the spell resistance of the specified creature.
        //  - Returns 0 if the creature has no spell resistance or an invalid
        //    creature is passed in.
        public static int GetSpellResistance(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(749);
            return Internal.StackPopInteger();
        }

        //  Changes the current Day/Night cycle for this player to night
        //  - oPlayer: which player to change the lighting for
        //  - fTransitionTime: how long the transition should take
        public static void DayToNight(NWGameObject oPlayer, float fTransitionTime = 0.0f)
        {
            Internal.StackPushFloat(fTransitionTime);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(750);
        }

        //  Changes the current Day/Night cycle for this player to daylight
        //  - oPlayer: which player to change the lighting for
        //  - fTransitionTime: how long the transition should take
        public static void NightToDay(NWGameObject oPlayer, float fTransitionTime = 0.0f)
        {
            Internal.StackPushFloat(fTransitionTime);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(751);
        }

        //  Returns whether or not there is a direct line of sight
        //  between the two objects. (Not blocked by any geometry).
        // 
        //  PLEASE NOTE: This is an expensive function and may
        //               degrade performance if used frequently.
        public static bool LineOfSightObject(NWGameObject oSource, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oSource, false);
            Internal.CallBuiltIn(752);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Returns whether or not there is a direct line of sight
        //  between the two vectors. (Not blocked by any geometry).
        // 
        //  This function must be run on a valid object in the area
        //  it will not work on the module or area.
        // 
        //  PLEASE NOTE: This is an expensive function and may
        //               degrade performance if used frequently.
        public static bool LineOfSightVector(Vector? vSource, Vector? vTarget)
        {
            Internal.StackPushVector(vTarget);
            Internal.StackPushVector(vSource);
            Internal.CallBuiltIn(753);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Returns the class that the spellcaster cast the
        //  spell as.
        //  - Returns CLASS_TYPE_INVALID if the caster has
        //    no valid class (placeables, etc...)
        public static ClassType GetLastSpellCastClass()
        {
            Internal.CallBuiltIn(754);
            return (ClassType)Internal.StackPopInteger();
        }

        //  Sets the number of base attacks for the specified
        //  creatures. The range of values accepted are from
        //  1 to 6
        //  Note: This function does not work on Player Characters
        public static void SetBaseAttackBonus(int nBaseAttackBonus, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger(nBaseAttackBonus);
            Internal.CallBuiltIn(755);
        }

        //  Restores the number of base attacks back to it's
        //  original state.
        public static void RestoreBaseAttackBonus(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(756);
        }

        //  Creates a cutscene ghost effect, this will allow creatures
        //  to pathfind through other creatures without bumping into them
        //  for the duration of the effect.
        public static Effect EffectCutsceneGhost()
        {
            Internal.CallBuiltIn(757);
            return Internal.StackPopEffect();
        }

        //  Creates an item property that offsets the effect on arcane spell failure
        //  that a particular item has. Parameters come from the ITEM_PROP_ASF_* group.
        public static ItemProperty ItemPropertyArcaneSpellFailure(int nModLevel)
        {
            Internal.StackPushInteger(nModLevel);
            Internal.CallBuiltIn(758);
            return Internal.StackPopItemProperty();
        }

        //  Returns the amount of gold a store currently has. -1 indicates it is not using gold.
        //  -2 indicates the store could not be located.
        public static int GetStoreGold(NWGameObject oidStore)
        {
            Internal.StackPushObject(oidStore, false);
            Internal.CallBuiltIn(759);
            return Internal.StackPopInteger();
        }

        //  Sets the amount of gold a store has. -1 means the store does not use gold.
        public static void SetStoreGold(NWGameObject oidStore, int nGold)
        {
            Internal.StackPushInteger(nGold);
            Internal.StackPushObject(oidStore, false);
            Internal.CallBuiltIn(760);
        }

        //  Gets the maximum amount a store will pay for any item. -1 means price unlimited.
        //  -2 indicates the store could not be located.
        public static int GetStoreMaxBuyPrice(NWGameObject oidStore)
        {
            Internal.StackPushObject(oidStore, false);
            Internal.CallBuiltIn(761);
            return Internal.StackPopInteger();
        }

        //  Sets the maximum amount a store will pay for any item. -1 means price unlimited.
        public static void SetStoreMaxBuyPrice(NWGameObject oidStore, int nMaxBuy)
        {
            Internal.StackPushInteger(nMaxBuy);
            Internal.StackPushObject(oidStore, false);
            Internal.CallBuiltIn(762);
        }

        //  Gets the amount a store charges for identifying an item. Default is 100. -1 means
        //  the store will not identify items.
        //  -2 indicates the store could not be located.
        public static int GetStoreIdentifyCost(NWGameObject oidStore)
        {
            Internal.StackPushObject(oidStore, false);
            Internal.CallBuiltIn(763);
            return Internal.StackPopInteger();
        }

        //  Sets the amount a store charges for identifying an item. Default is 100. -1 means
        //  the store will not identify items.
        public static void SetStoreIdentifyCost(NWGameObject oidStore, int nCost)
        {
            Internal.StackPushInteger(nCost);
            Internal.StackPushObject(oidStore, false);
            Internal.CallBuiltIn(764);
        }

        //  Sets the creature's appearance type to the value specified (uses the APPEARANCE_TYPE_XXX constants)
        public static void SetCreatureAppearanceType(NWGameObject oCreature, AppearanceType nAppearanceType)
        {
            Internal.StackPushInteger((int)nAppearanceType);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(765);
        }

        //  Returns the default package selected for this creature to level up with
        //  - returns PACKAGE_INVALID if error occurs
        public static int GetCreatureStartingPackage(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(766);
            return Internal.StackPopInteger();
        }

        //  Returns an effect that when applied will paralyze the target's legs, rendering
        //  them unable to walk but otherwise unpenalized. This effect cannot be resisted.
        public static Effect EffectCutsceneImmobilize()
        {
            Internal.CallBuiltIn(767);
            return Internal.StackPopEffect();
        }

        //  Is this creature in the given subarea? (trigger, area of effect object, etc..)
        //  This function will tell you if the creature has triggered an onEnter event,
        //  not if it is physically within the space of the subarea
        public static bool GetIsInSubArea(NWGameObject oCreature, NWGameObject oSubArea = null)
        {
            Internal.StackPushObject(oSubArea, false);
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(768);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Returns the Cost Table number of the item property. See the 2DA files for value definitions.
        public static int GetItemPropertyCostTable(ItemProperty iProp)
        {
            Internal.StackPushItemProperty(iProp);
            Internal.CallBuiltIn(769);
            return Internal.StackPopInteger();
        }

        //  Returns the Cost Table value (index of the cost table) of the item property.
        //  See the 2DA files for value definitions.
        public static int GetItemPropertyCostTableValue(ItemProperty iProp)
        {
            Internal.StackPushItemProperty(iProp);
            Internal.CallBuiltIn(770);
            return Internal.StackPopInteger();
        }

        //  Returns the Param1 number of the item property. See the 2DA files for value definitions.
        public static int GetItemPropertyParam1(ItemProperty iProp)
        {
            Internal.StackPushItemProperty(iProp);
            Internal.CallBuiltIn(771);
            return Internal.StackPopInteger();
        }

        //  Returns the Param1 value of the item property. See the 2DA files for value definitions.
        public static int GetItemPropertyParam1Value(ItemProperty iProp)
        {
            Internal.StackPushItemProperty(iProp);
            Internal.CallBuiltIn(772);
            return Internal.StackPopInteger();
        }

        //  Is this creature able to be disarmed? (checks disarm flag on creature, and if
        //  the creature actually has a weapon equipped in their right hand that is droppable)
        public static bool GetIsCreatureDisarmable(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(773);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets whether this item is 'stolen' or not
        public static void SetStolenFlag(NWGameObject oItem, bool nStolenFlag)
        {
            Internal.StackPushInteger(Convert.ToInt32(nStolenFlag));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(774);
        }

        //  Instantly gives this creature the benefits of a rest (restored hitpoints, spells, feats, etc..)
        public static void ForceRest(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(775);
        }

        //  Forces this player's camera to be set to this height. Setting this value to zero will
        //  restore the camera to the racial default height.
        public static void SetCameraHeight(NWGameObject oPlayer, float fHeight = 0.0f)
        {
            Internal.StackPushFloat(fHeight);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(776);
        }

        //  Changes the sky that is displayed in the specified area.
        //  nSkyBox = SKYBOX_* constants (associated with skyboxes.2da)
        //  If no valid area (or object) is specified, it uses the area of caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static void SetSkyBox(Skybox nSkyBox, NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.StackPushInteger((int)nSkyBox);
            Internal.CallBuiltIn(777);
        }

        //  Returns the creature's currently set PhenoType (body type).
        public static PhenoType GetPhenoType(NWGameObject oCreature)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(778);
            return (PhenoType)Internal.StackPopInteger();
        }

        //  Sets the creature's PhenoType (body type) to the type specified.
        //  nPhenoType = PHENOTYPE_NORMAL
        //  nPhenoType = PHENOTYPE_BIG
        //  nPhenoType = PHENOTYPE_CUSTOM* - The custom PhenoTypes should only ever
        //  be used if you have specifically created your own custom content that
        //  requires the use of a new PhenoType and you have specified the appropriate
        //  custom PhenoType in your custom content.
        //  SetPhenoType will only work on part based creature (i.e. the starting
        //  default playable races).
        public static void SetPhenoType(PhenoType nPhenoType, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nPhenoType);
            Internal.CallBuiltIn(779);
        }

        //  Sets the fog color in the area specified.
        //  nFogType = FOG_TYPE_* specifies wether the Sun, Moon, or both fog types are set.
        //  nFogColor = FOG_COLOR_* specifies the color the fog is being set to.
        //  The fog color can also be represented as a hex RGB number if specific color shades
        //  are desired.
        //  The format of a hex specified color would be 0xFFEEDD where
        //  FF would represent the amount of red in the color
        //  EE would represent the amount of green in the color
        //  DD would represent the amount of blue in the color.
        //  If no valid area (or object) is specified, it uses the area of caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static void SetFogColor(FogType nFogType, FogColor nFogColor, NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.StackPushInteger((int)nFogColor);
            Internal.StackPushInteger((int)nFogType);
            Internal.CallBuiltIn(780);
        }

        //  Gets the current cutscene state of the player specified by oCreature.
        //  Returns TRUE if the player is in cutscene mode.
        //  Returns FALSE if the player is not in cutscene mode, or on an error
        //  (such as specifying a non creature object).
        public static bool GetCutsceneMode(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(781);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Gets the skybox that is currently displayed in the specified area.
        //  Returns:
        //      SKYBOX_* constant
        //  If no valid area (or object) is specified, it uses the area of caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static Skybox GetSkyBox(NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(782);
            return (Skybox)Internal.StackPopInteger();
        }

        //  Gets the fog color in the area specified.
        //  nFogType specifies wether the Sun, or Moon fog type is returned. 
        //     Valid values for nFogType are FOG_TYPE_SUN or FOG_TYPE_MOON.
        //  If no valid area (or object) is specified, it uses the area of caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static FogColor GetFogColor(FogType nFogType, NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.StackPushInteger((int)nFogType);
            Internal.CallBuiltIn(783);
            return (FogColor)Internal.StackPopInteger();
        }

        //  Sets the fog amount in the area specified.
        //  nFogType = FOG_TYPE_* specifies wether the Sun, Moon, or both fog types are set.
        //  nFogAmount = specifies the density that the fog is being set to.
        //  If no valid area (or object) is specified, it uses the area of caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static void SetFogAmount(FogType nFogType, int nFogAmount, NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.StackPushInteger(nFogAmount);
            Internal.StackPushInteger((int)nFogType);
            Internal.CallBuiltIn(784);
        }

        //  Gets the fog amount in the area specified.
        //  nFogType = nFogType specifies wether the Sun, or Moon fog type is returned. 
        //     Valid values for nFogType are FOG_TYPE_SUN or FOG_TYPE_MOON.
        //  If no valid area (or object) is specified, it uses the area of caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static int GetFogAmount(FogType nFogType, NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.StackPushInteger((int)nFogType);
            Internal.CallBuiltIn(785);
            return Internal.StackPopInteger();
        }

        //  returns TRUE if the item CAN be pickpocketed
        public static bool GetPickpocketableFlag(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(786);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets the Pickpocketable flag on an item
        //  - oItem: the item to change
        //  - bPickpocketable: TRUE or FALSE, whether the item can be pickpocketed.
        public static void SetPickpocketableFlag(NWGameObject oItem, bool bPickpocketable)
        {
            Internal.StackPushInteger(Convert.ToInt32(bPickpocketable));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(787);
        }

        //  returns the footstep type of the creature specified.
        //  The footstep type determines what the creature's footsteps sound
        //  like when ever they take a step.
        //  returns FOOTSTEP_TYPE_INVALID if used on a non-creature object, or if
        //  used on creature that has no footstep sounds by default (e.g. Will-O'-Wisp).
        public static FootstepType GetFootstepType(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(788);
            return (FootstepType)Internal.StackPopInteger();
        }

        //  Sets the footstep type of the creature specified.
        //  Changing a creature's footstep type will change the sound that
        //  its feet make when ever the creature makes takes a step.
        //  By default a creature's footsteps are detemined by the appearance
        //  type of the creature. SetFootstepType() allows you to make a
        //  creature use a difference footstep type than it would use by default
        //  for its given appearance.
        //  - nFootstepType (FOOTSTEP_TYPE_*):
        //       FOOTSTEP_TYPE_NORMAL
        //       FOOTSTEP_TYPE_LARGE
        //       FOOTSTEP_TYPE_DRAGON
        //       FOOTSTEP_TYPE_SoFT
        //       FOOTSTEP_TYPE_HOOF
        //       FOOTSTEP_TYPE_HOOF_LARGE
        //       FOOTSTEP_TYPE_BEETLE
        //       FOOTSTEP_TYPE_SPIDER
        //       FOOTSTEP_TYPE_SKELETON
        //       FOOTSTEP_TYPE_LEATHER_WING
        //       FOOTSTEP_TYPE_FEATHER_WING
        //       FOOTSTEP_TYPE_DEFAULT - Makes the creature use its original default footstep sounds.
        //       FOOTSTEP_TYPE_NONE
        //  - oCreature: the creature to change the footstep sound for.
        public static void SetFootstepType(FootstepType nFootstepType, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nFootstepType);
            Internal.CallBuiltIn(789);
        }

        //  returns the Wing type of the creature specified.
        //       CREATURE_WING_TYPE_NONE
        //       CREATURE_WING_TYPE_DEMON
        //       CREATURE_WING_TYPE_ANGEL
        //       CREATURE_WING_TYPE_BAT
        //       CREATURE_WING_TYPE_DRAGON
        //       CREATURE_WING_TYPE_BUTTERFLY
        //       CREATURE_WING_TYPE_BIRD
        //  returns CREATURE_WING_TYPE_NONE if used on a non-creature object,
        //  if the creature has no wings, or if the creature can not have its
        //  wing type changed in the toolset.
        public static CreatureWingType GetCreatureWingType(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(790);
            return (CreatureWingType)Internal.StackPopInteger();
        }

        //  Sets the Wing type of the creature specified.
        //  - nWingType (CREATURE_WING_TYPE_*)
        //       CREATURE_WING_TYPE_NONE
        //       CREATURE_WING_TYPE_DEMON
        //       CREATURE_WING_TYPE_ANGEL
        //       CREATURE_WING_TYPE_BAT
        //       CREATURE_WING_TYPE_DRAGON
        //       CREATURE_WING_TYPE_BUTTERFLY
        //       CREATURE_WING_TYPE_BIRD
        //  - oCreature: the creature to change the wing type for.
        //  Note: Only two creature model types will support wings. 
        //  The MODELTYPE for the part based (playable races) 'P' 
        //  and MODELTYPE 'W'in the appearance.2da
        public static void SetCreatureWingType(CreatureWingType nWingType, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nWingType);
            Internal.CallBuiltIn(791);
        }

        //  returns the model number being used for the body part and creature specified
        //  The model number returned is for the body part when the creature is not wearing
        //  armor (i.e. whether or not the creature is wearing armor does not affect
        //  the return value).
        //  Note: Only works on part based creatures, which is typically restricted to
        //  the playable races (unless some new part based custom content has been 
        //  added to the module).
        // 
        //  returns CREATURE_PART_INVALID if used on a non-creature object,
        //  or if the creature does not use a part based model.
        // 
        //  - nPart (CREATURE_PART_*)
        //       CREATURE_PART_RIGHT_FOOT
        //       CREATURE_PART_LEFT_FOOT
        //       CREATURE_PART_RIGHT_SHIN
        //       CREATURE_PART_LEFT_SHIN
        //       CREATURE_PART_RIGHT_THIGH
        //       CREATURE_PART_LEFT_THIGH
        //       CREATURE_PART_PELVIS
        //       CREATURE_PART_TORSO
        //       CREATURE_PART_BELT
        //       CREATURE_PART_NECK
        //       CREATURE_PART_RIGHT_FOREARM
        //       CREATURE_PART_LEFT_FOREARM
        //       CREATURE_PART_RIGHT_BICEP
        //       CREATURE_PART_LEFT_BICEP
        //       CREATURE_PART_RIGHT_SHOULDER
        //       CREATURE_PART_LEFT_SHOULDER
        //       CREATURE_PART_RIGHT_HAND
        //       CREATURE_PART_LEFT_HAND
        //       CREATURE_PART_HEAD
        public static int GetCreatureBodyPart(CreaturePart nPart, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nPart);
            Internal.CallBuiltIn(792);
            return Internal.StackPopInteger();
        }

        //  Sets the body part model to be used on the creature specified.
        //  The model names for parts need to be in the following format:
        //    p<m/f><race letter><phenotype>_<body part><model number>.mdl
        // 
        //  - nPart (CREATURE_PART_*)
        //       CREATURE_PART_RIGHT_FOOT
        //       CREATURE_PART_LEFT_FOOT
        //       CREATURE_PART_RIGHT_SHIN
        //       CREATURE_PART_LEFT_SHIN
        //       CREATURE_PART_RIGHT_THIGH
        //       CREATURE_PART_LEFT_THIGH
        //       CREATURE_PART_PELVIS
        //       CREATURE_PART_TORSO
        //       CREATURE_PART_BELT
        //       CREATURE_PART_NECK
        //       CREATURE_PART_RIGHT_FOREARM
        //       CREATURE_PART_LEFT_FOREARM
        //       CREATURE_PART_RIGHT_BICEP
        //       CREATURE_PART_LEFT_BICEP
        //       CREATURE_PART_RIGHT_SHOULDER
        //       CREATURE_PART_LEFT_SHOULDER
        //       CREATURE_PART_RIGHT_HAND
        //       CREATURE_PART_LEFT_HAND
        //       CREATURE_PART_HEAD
        //  - nModelNumber: CREATURE_MODEL_TYPE_*
        //       CREATURE_MODEL_TYPE_NONE
        //       CREATURE_MODEL_TYPE_SKIN (not for use on shoulders, pelvis or head).
        //       CREATURE_MODEL_TYPE_TATTOO (for body parts that support tattoos, i.e. not heads/feet/hands).
        //       CREATURE_MODEL_TYPE_UNDEAD (undead model only exists for the right arm parts).
        //  - oCreature: the creature to change the body part for.
        //  Note: Only part based creature appearance types are supported. 
        //  i.e. The model types for the playable races ('P') in the appearance.2da
        public static void SetCreatureBodyPart(CreaturePart nPart, CreatureModelType nModelNumber, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nModelNumber);
            Internal.StackPushInteger((int)nPart);
            Internal.CallBuiltIn(793);
        }

        //  returns the Tail type of the creature specified.
        //       CREATURE_TAIL_TYPE_NONE
        //       CREATURE_TAIL_TYPE_LIZARD
        //       CREATURE_TAIL_TYPE_BONE
        //       CREATURE_TAIL_TYPE_DEVIL
        //  returns CREATURE_TAIL_TYPE_NONE if used on a non-creature object,
        //  if the creature has no Tail, or if the creature can not have its
        //  Tail type changed in the toolset.
        public static CreatureTailType GetCreatureTailType(NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.CallBuiltIn(794);
            return (CreatureTailType)Internal.StackPopInteger();
        }

        //  Sets the Tail type of the creature specified.
        //  - nTailType (CREATURE_TAIL_TYPE_*)
        //       CREATURE_TAIL_TYPE_NONE
        //       CREATURE_TAIL_TYPE_LIZARD
        //       CREATURE_TAIL_TYPE_BONE
        //       CREATURE_TAIL_TYPE_DEVIL
        //  - oCreature: the creature to change the Tail type for.
        //  Note: Only two creature model types will support Tails. 
        //  The MODELTYPE for the part based (playable) races 'P' 
        //  and MODELTYPE 'T'in the appearance.2da
        public static void SetCreatureTailType(CreatureTailType nTailType, NWGameObject oCreature = null)
        {
            Internal.StackPushObject(oCreature, false);
            Internal.StackPushInteger((int)nTailType);
            Internal.CallBuiltIn(795);
        }

        //  returns the Hardness of a Door or Placeable object.
        //  - oObject: a door or placeable object.
        //  returns -1 on an error or if used on an object that is
        //  neither a door nor a placeable object.
        public static int GetHardness(NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(796);
            return Internal.StackPopInteger();
        }

        //  Sets the Hardness of a Door or Placeable object.
        //  - nHardness: must be between 0 and 250.
        //  - oObject: a door or placeable object.
        //  Does nothing if used on an object that is neither
        //  a door nor a placeable.
        public static void SetHardness(int nHardness, NWGameObject oObject = null)
        {
            Internal.StackPushObject(oObject, false);
            Internal.StackPushInteger(nHardness);
            Internal.CallBuiltIn(797);
        }

        //  When set the object can not be opened unless the
        //  opener possesses the required key. The key tag required
        //  can be specified either in the toolset, or by using
        //  the SetLockKeyTag() scripting command.
        //  - oObject: a door, or placeable.
        //  - nKeyRequired: TRUE/FALSE
        public static void SetLockKeyRequired(NWGameObject oObject, bool nKeyRequired = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nKeyRequired));
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(798);
        }

        //  Set the key tag required to open object oObject.
        //  This will only have an effect if the object is set to
        //  "Key required to unlock or lock" either in the toolset
        //  or by using the scripting command SetLockKeyRequired().
        //  - oObject: a door, placeable or trigger.
        //  - sNewKeyTag: the key tag required to open the locked object.
        public static void SetLockKeyTag(NWGameObject oObject, string sNewKeyTag)
        {
            Internal.StackPushString(sNewKeyTag);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(799);
        }

        //  Sets whether or not the object can be locked.
        //  - oObject: a door or placeable.
        //  - nLockable: TRUE/FALSE
        public static void SetLockLockable(NWGameObject oObject, bool nLockable = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nLockable));
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(800);
        }

        //  Sets the DC for unlocking the object.
        //  - oObject: a door or placeable object.
        //  - nNewUnlockDC: must be between 0 and 250.
        public static void SetLockUnlockDC(NWGameObject oObject, int nNewUnlockDC)
        {
            Internal.StackPushInteger(nNewUnlockDC);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(801);
        }

        //  Sets the DC for locking the object.
        //  - oObject: a door or placeable object.
        //  - nNewLockDC: must be between 0 and 250.
        public static void SetLockLockDC(NWGameObject oObject, int nNewLockDC)
        {
            Internal.StackPushInteger(nNewLockDC);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(802);
        }

        //  Sets whether or not the trapped object can be disarmed.
        //  - oTrapObject: a placeable, door or trigger
        //  - nDisarmable: TRUE/FALSE
        public static void SetTrapDisarmable(NWGameObject oTrapObject, bool nDisarmable = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nDisarmable));
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(803);
        }

        //  Sets whether or not the trapped object can be detected.
        //  - oTrapObject: a placeable, door or trigger
        //  - nDetectable: TRUE/FALSE
        //  Note: Setting a trapped object to not be detectable will
        //  not make the trap disappear if it has already been detected.
        public static void SetTrapDetectable(NWGameObject oTrapObject, bool nDetectable = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nDetectable));
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(804);
        }

        //  Sets whether or not the trap is a one-shot trap
        //  (i.e. whether or not the trap resets itself after firing).
        //  - oTrapObject: a placeable, door or trigger
        //  - nOneShot: TRUE/FALSE
        public static void SetTrapOneShot(NWGameObject oTrapObject, bool nOneShot = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nOneShot));
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(805);
        }

        //  Set the tag of the key that will disarm oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        public static void SetTrapKeyTag(NWGameObject oTrapObject, string sKeyTag)
        {
            Internal.StackPushString(sKeyTag);
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(806);
        }

        //  Set the DC for disarming oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        //  - nDisarmDC: must be between 0 and 250.
        public static void SetTrapDisarmDC(NWGameObject oTrapObject, int nDisarmDC)
        {
            Internal.StackPushInteger(nDisarmDC);
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(807);
        }

        //  Set the DC for detecting oTrapObject.
        //  - oTrapObject: a placeable, door or trigger
        //  - nDetectDC: must be between 0 and 250.
        public static void SetTrapDetectDC(NWGameObject oTrapObject, int nDetectDC)
        {
            Internal.StackPushInteger(nDetectDC);
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(808);
        }

        //  Creates a square Trap object.
        //  - nTrapType: The base type of trap (TRAP_BASE_TYPE_*)
        //  - lLocation: The location and orientation that the trap will be created at.
        //  - fSize: The size of the trap. Minimum size allowed is 1.0f.
        //  - sTag: The tag of the trap being created.
        //  - nFaction: The faction of the trap (STANDARD_FACTION_*).
        //  - sOnDisarmScript: The OnDisarm script that will fire when the trap is disarmed.
        //                     If "" no script will fire.
        //  - sOnTrapTriggeredScript: The OnTrapTriggered script that will fire when the
        //                            trap is triggered.
        //                            If "" the default OnTrapTriggered script for the trap
        //                            type specified will fire instead (as specified in the
        //                            traps.2da).
        public static NWGameObject CreateTrapAtLocation(int nTrapType, Location lLocation, float fSize = 2.0f, string sTag = "", StandardFaction nFaction = StandardFaction.Hostile, string sOnDisarmScript = "", string sOnTrapTriggeredScript = "")
        {
            Internal.StackPushString(sOnTrapTriggeredScript);
            Internal.StackPushString(sOnDisarmScript);
            Internal.StackPushInteger((int)nFaction);
            Internal.StackPushString(sTag);
            Internal.StackPushFloat(fSize);
            Internal.StackPushLocation(lLocation);
            Internal.StackPushInteger(nTrapType);
            Internal.CallBuiltIn(809);
            return Internal.StackPopObject();
        }

        //  Creates a Trap on the object specified.
        //  - nTrapType: The base type of trap (TRAP_BASE_TYPE_*)
        //  - oObject: The object that the trap will be created on. Works only on Doors and Placeables.
        //  - nFaction: The faction of the trap (STANDARD_FACTION_*).
        //  - sOnDisarmScript: The OnDisarm script that will fire when the trap is disarmed.
        //                     If "" no script will fire.
        //  - sOnTrapTriggeredScript: The OnTrapTriggered script that will fire when the
        //                            trap is triggered.
        //                            If "" the default OnTrapTriggered script for the trap
        //                            type specified will fire instead (as specified in the
        //                            traps.2da).
        //  Note: After creating a trap on an object, you can change the trap's properties
        //        using the various SetTrap* scripting commands by passing in the object
        //        that the trap was created on (i.e. oObject) to any subsequent SetTrap* commands.
        public static void CreateTrapOnObject(TrapBaseType nTrapType, NWGameObject oObject, StandardFaction nFaction = StandardFaction.Hostile, string sOnDisarmScript = "", string sOnTrapTriggeredScript = "")
        {
            Internal.StackPushString(sOnTrapTriggeredScript);
            Internal.StackPushString(sOnDisarmScript);
            Internal.StackPushInteger((int)nFaction);
            Internal.StackPushObject(oObject, false);
            Internal.StackPushInteger((int)nTrapType);
            Internal.CallBuiltIn(810);
        }

        //  Set the Will saving throw value of the Door or Placeable object oObject.
        //  - oObject: a door or placeable object.
        //  - nWillSave: must be between 0 and 250.
        public static void SetWillSavingThrow(NWGameObject oObject, int nWillSave)
        {
            Internal.StackPushInteger(nWillSave);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(811);
        }

        //  Set the Reflex saving throw value of the Door or Placeable object oObject.
        //  - oObject: a door or placeable object.
        //  - nReflexSave: must be between 0 and 250.
        public static void SetReflexSavingThrow(NWGameObject oObject, int nReflexSave)
        {
            Internal.StackPushInteger(nReflexSave);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(812);
        }

        //  Set the Fortitude saving throw value of the Door or Placeable object oObject.
        //  - oObject: a door or placeable object.
        //  - nFortitudeSave: must be between 0 and 250.
        public static void SetFortitudeSavingThrow(NWGameObject oObject, int nFortitudeSave)
        {
            Internal.StackPushInteger(nFortitudeSave);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(813);
        }

        //  returns the resref (TILESET_RESREF_*) of the tileset used to create area oArea.
        //       TILESET_RESREF_BEHOLDER_CAVES
        //       TILESET_RESREF_CASTLE_INTERIOR
        //       TILESET_RESREF_CITY_EXTERIOR
        //       TILESET_RESREF_CITY_INTERIOR
        //       TILESET_RESREF_CRYPT
        //       TILESET_RESREF_DESERT
        //       TILESET_RESREF_DROW_INTERIOR
        //       TILESET_RESREF_DUNGEON
        //       TILESET_RESREF_FOREST
        //       TILESET_RESREF_FROZEN_WASTES
        //       TILESET_RESREF_ILLITHID_INTERIOR
        //       TILESET_RESREF_MICROSET
        //       TILESET_RESREF_MINES_AND_CAVERNS
        //       TILESET_RESREF_RUINS
        //       TILESET_RESREF_RURAL
        //       TILESET_RESREF_RURAL_WINTER
        //       TILESET_RESREF_SEWERS
        //       TILESET_RESREF_UNDERDARK
        //  * returns an empty string on an error.
        public static string GetTilesetResRef(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(814);
            return Internal.StackPopString();
        }

        //  - oTrapObject: a placeable, door or trigger
        //  * Returns TRUE if oTrapObject can be recovered.
        public static bool GetTrapRecoverable(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(815);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets whether or not the trapped object can be recovered.
        //  - oTrapObject: a placeable, door or trigger
        public static void SetTrapRecoverable(NWGameObject oTrapObject, bool nRecoverable = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nRecoverable));
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(816);
        }

        //  Get the XP scale being used for the module.
        public static int GetModuleXPScale()
        {
            Internal.CallBuiltIn(817);
            return Internal.StackPopInteger();
        }

        //  Set the XP scale used by the module.
        //  - nXPScale: The XP scale to be used. Must be between 0 and 200.
        public static void SetModuleXPScale(int nXPScale)
        {
            Internal.StackPushInteger(nXPScale);
            Internal.CallBuiltIn(818);
        }

        //  Get the feedback message that will be displayed when trying to unlock the object oObject.
        //  - oObject: a door or placeable.
        //  Returns an empty string "" on an error or if the game's default feedback message is being used
        public static string GetKeyRequiredFeedback(NWGameObject oObject)
        {
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(819);
            return Internal.StackPopString();
        }

        //  Set the feedback message that is displayed when trying to unlock the object oObject.
        //  This will only have an effect if the object is set to
        //  "Key required to unlock or lock" either in the toolset
        //  or by using the scripting command SetLockKeyRequired().
        //  - oObject: a door or placeable.
        //  - sFeedbackMessage: the string to be displayed in the player's text window.
        //                      to use the game's default message, set sFeedbackMessage to ""
        public static void SetKeyRequiredFeedback(NWGameObject oObject, string sFeedbackMessage)
        {
            Internal.StackPushString(sFeedbackMessage);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(820);
        }

        //  - oTrapObject: a placeable, door or trigger
        //  * Returns TRUE if oTrapObject is active
        public static bool GetTrapActive(NWGameObject oTrapObject)
        {
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(821);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets whether or not the trap is an active trap
        //  - oTrapObject: a placeable, door or trigger
        //  - nActive: TRUE/FALSE
        //  Notes:
        //  Setting a trap as inactive will not make the
        //  trap disappear if it has already been detected.
        //  Call SetTrapDetectedBy() to make a detected trap disappear.
        //  To make an inactive trap not detectable call SetTrapDetectable()
        public static void SetTrapActive(NWGameObject oTrapObject, bool nActive = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(nActive));
            Internal.StackPushObject(oTrapObject, false);
            Internal.CallBuiltIn(822);
        }

        //  Locks the player's camera pitch to its current pitch setting,
        //  or unlocks the player's camera pitch.
        //  Stops the player from tilting their camera angle. 
        //  - oPlayer: A player object.
        //  - bLocked: TRUE/FALSE.
        public static void LockCameraPitch(NWGameObject oPlayer, bool bLocked = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bLocked));
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(823);
        }

        //  Locks the player's camera distance to its current distance setting,
        //  or unlocks the player's camera distance.
        //  Stops the player from being able to zoom in/out the camera.
        //  - oPlayer: A player object.
        //  - bLocked: TRUE/FALSE.
        public static void LockCameraDistance(NWGameObject oPlayer, bool bLocked = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bLocked));
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(824);
        }

        //  Locks the player's camera direction to its current direction,
        //  or unlocks the player's camera direction to enable it to move
        //  freely again.
        //  Stops the player from being able to rotate the camera direction.
        //  - oPlayer: A player object.
        //  - bLocked: TRUE/FALSE.
        public static void LockCameraDirection(NWGameObject oPlayer, bool bLocked = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bLocked));
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(825);
        }

        //  Get the last object that default clicked (left clicked) on the placeable object
        //  that is calling this function.
        //  Should only be called from a placeables OnClick event.
        //  * Returns OBJECT_INVALID if it is called by something other than a placeable.
        public static NWGameObject GetPlaceableLastClickedBy()
        {
            Internal.CallBuiltIn(826);
            return Internal.StackPopObject();
        }

        //  returns TRUE if the item is flagged as infinite.
        //  - oItem: an item.
        //  The infinite property affects the buying/selling behavior of the item in a store.
        //  An infinite item will still be available to purchase from a store after a player
        //  buys the item (non-infinite items will disappear from the store when purchased).
        public static bool GetInfiniteFlag(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(827);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets the Infinite flag on an item
        //  - oItem: the item to change
        //  - bInfinite: TRUE or FALSE, whether the item should be Infinite
        //  The infinite property affects the buying/selling behavior of the item in a store.
        //  An infinite item will still be available to purchase from a store after a player
        //  buys the item (non-infinite items will disappear from the store when purchased).
        public static void SetInfiniteFlag(NWGameObject oItem, bool bInfinite = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bInfinite));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(828);
        }

        //  Gets the size of the area.
        //  - nAreaDimension: The area dimension that you wish to determine.
        //       AREA_HEIGHT
        //       AREA_WIDTH
        //  - oArea: The area that you wish to get the size of.
        //  Returns: The number of tiles that the area is wide/high, or zero on an error.
        //  If no valid area (or object) is specified, it uses the area of the caller.
        //  If an object other than an area is specified, will use the area that the object is currently in.
        public static int GetAreaSize(AreaProperty nAreaDimension, NWGameObject oArea = null)
        {
            Internal.StackPushObject(oArea, false);
            Internal.StackPushInteger((int)nAreaDimension);
            Internal.CallBuiltIn(829);
            return Internal.StackPopInteger();
        }

        //  Set the name of oObject.
        //  - oObject: the object for which you are changing the name (a creature, placeable, item, or door).
        //  - sNewName: the new name that the object will use.
        //  Note: SetName() does not work on player objects.
        //        Setting an object's name to "" will make the object
        //        revert to using the name it had originally before any
        //        SetName() calls were made on the object.
        public static void SetName(NWGameObject oObject, string sNewName = "")
        {
            Internal.StackPushString(sNewName);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(830);
        }

        //  Get the PortraitId of oTarget.
        //  - oTarget: the object for which you are getting the portrait Id.
        //  Returns: The Portrait Id number being used for the object oTarget.
        //           The Portrait Id refers to the row number of the Portraits.2da
        //           that this portrait is from.
        //           If a custom portrait is being used, oTarget is a player object,
        //           or on an error returns PORTRAIT_INVALID. In these instances
        //           try using GetPortraitResRef() instead.
        public static int GetPortraitId(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(831);
            return Internal.StackPopInteger();
        }

        //  Change the portrait of oTarget to use the Portrait Id specified.
        //  - oTarget: the object for which you are changing the portrait.
        //  - nPortraitId: The Id of the new portrait to use. 
        //                 nPortraitId refers to a row in the Portraits.2da
        //  Note: Not all portrait Ids are suitable for use with all object types.
        //        Setting the portrait Id will also cause the portrait ResRef
        //        to be set to the appropriate portrait ResRef for the Id specified.
        public static void SetPortraitId(NWGameObject oTarget, int nPortraitId)
        {
            Internal.StackPushInteger(nPortraitId);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(832);
        }

        //  Get the Portrait ResRef of oTarget.
        //  - oTarget: the object for which you are getting the portrait ResRef.
        //  Returns: The Portrait ResRef being used for the object oTarget.
        //           The Portrait ResRef will not include a trailing size letter.
        public static string GetPortraitResRef(NWGameObject oTarget = null)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(833);
            return Internal.StackPopString();
        }

        //  Change the portrait of oTarget to use the Portrait ResRef specified.
        //  - oTarget: the object for which you are changing the portrait.
        //  - sPortraitResRef: The ResRef of the new portrait to use. 
        //                     The ResRef should not include any trailing size letter ( e.g. po_el_f_09_ ).
        //  Note: Not all portrait ResRefs are suitable for use with all object types.
        //        Setting the portrait ResRef will also cause the portrait Id
        //        to be set to PORTRAIT_INVALID.
        public static void SetPortraitResRef(NWGameObject oTarget, string sPortraitResRef)
        {
            Internal.StackPushString(sPortraitResRef);
            Internal.StackPushObject(oTarget, false);
            Internal.CallBuiltIn(834);
        }

        //  Set oPlaceable's useable object status.
        //  Note: Only works on non-static placeables.
        public static void SetUseableFlag(NWGameObject oPlaceable, bool nUseableFlag)
        {
            Internal.StackPushInteger(Convert.ToInt32(nUseableFlag));
            Internal.StackPushObject(oPlaceable, false);
            Internal.CallBuiltIn(835);
        }

        //  Get the description of oObject.
        //  - oObject: the object from which you are obtaining the description. 
        //             Can be a creature, item, placeable, door, trigger or module object.
        //  - bOriginalDescription:  if set to true any new description specified via a SetDescription scripting command
        //                    is ignored and the original object's description is returned instead.
        //  - bIdentified: If oObject is an item, setting this to TRUE will return the identified description,
        //                 setting this to FALSE will return the unidentified description. This flag has no
        //                 effect on objects other than items.
        public static string GetDescription(NWGameObject oObject, bool bOriginalDescription = false, bool bIdentifiedDescription = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bIdentifiedDescription));
            Internal.StackPushInteger(Convert.ToInt32(bOriginalDescription));
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(836);
            return Internal.StackPopString();
        }

        //  Set the description of oObject.
        //  - oObject: the object for which you are changing the description 
        //             Can be a creature, placeable, item, door, or trigger.
        //  - sNewDescription: the new description that the object will use.
        //  - bIdentified: If oObject is an item, setting this to TRUE will set the identified description,
        //                 setting this to FALSE will set the unidentified description. This flag has no
        //                 effect on objects other than items.
        //  Note: Setting an object's description to "" will make the object
        //        revert to using the description it originally had before any
        //        SetDescription() calls were made on the object.
        public static void SetDescription(NWGameObject oObject, string sNewDescription = "", bool bIdentifiedDescription = true)
        {
            Internal.StackPushInteger(Convert.ToInt32(bIdentifiedDescription));
            Internal.StackPushString(sNewDescription);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(837);
        }

        //  Get the PC that sent the last player chat(text) message.
        //  Should only be called from a module's OnPlayerChat event script.
        //  * Returns OBJECT_INVALID on error.
        //  Note: Private tells do not trigger a OnPlayerChat event.
        public static NWGameObject GetPCChatSpeaker()
        {
            Internal.CallBuiltIn(838);
            return Internal.StackPopObject();
        }

        //  Get the last player chat(text) message that was sent.
        //  Should only be called from a module's OnPlayerChat event script.
        //  * Returns empty string "" on error.
        //  Note: Private tells do not trigger a OnPlayerChat event.
        public static string GetPCChatMessage()
        {
            Internal.CallBuiltIn(839);
            return Internal.StackPopString();
        }

        //  Get the volume of the last player chat(text) message that was sent.
        //  Returns one of the following TALKVOLUME_* constants based on the volume setting
        //  that the player used to send the chat message.
        //                 TALKVOLUME_TALK
        //                 TALKVOLUME_WHISPER
        //                 TALKVOLUME_SHOUT
        //                 TALKVOLUME_SILENT_SHOUT (used for DM chat channel)
        //                 TALKVOLUME_PARTY
        //  Should only be called from a module's OnPlayerChat event script.
        //  * Returns -1 on error.
        //  Note: Private tells do not trigger a OnPlayerChat event.
        public static TalkVolume GetPCChatVolume()
        {
            Internal.CallBuiltIn(840);
            return (TalkVolume)Internal.StackPopInteger();
        }

        //  Set the last player chat(text) message before it gets sent to other players.
        //  - sNewChatMessage: The new chat text to be sent onto other players.
        //                     Setting the player chat message to an empty string "",
        //                     will cause the chat message to be discarded 
        //                     (i.e. it will not be sent to other players).
        //  Note: The new chat message gets sent after the OnPlayerChat script exits.
        public static void SetPCChatMessage(string sNewChatMessage = "")
        {
            Internal.StackPushString(sNewChatMessage);
            Internal.CallBuiltIn(841);
        }

        //  Set the last player chat(text) volume before it gets sent to other players.
        //  - nTalkVolume: The new volume of the chat text to be sent onto other players.
        //                 TALKVOLUME_TALK
        //                 TALKVOLUME_WHISPER
        //                 TALKVOLUME_SHOUT
        //                 TALKVOLUME_SILENT_SHOUT (used for DM chat channel)
        //                 TALKVOLUME_PARTY
        //                 TALKVOLUME_TELL (sends the chat message privately back to the original speaker)
        //  Note: The new chat message gets sent after the OnPlayerChat script exits.
        public static void SetPCChatVolume(TalkVolume nTalkVolume = TalkVolume.Talk)
        {
            Internal.StackPushInteger((int)nTalkVolume);
            Internal.CallBuiltIn(842);
        }

        //  Get the Color of oObject from the color channel specified.
        //  - oObject: the object from which you are obtaining the color. 
        //             Can be a creature that has color information (i.e. the playable races).
        //  - nColorChannel: The color channel that you want to get the color value of.
        //                    COLOR_CHANNEL_SKIN
        //                    COLOR_CHANNEL_HAIR
        //                    COLOR_CHANNEL_TATTOO_1
        //                    COLOR_CHANNEL_TATTOO_2
        //  * Returns -1 on error.
        public static int GetColor(NWGameObject oObject, ColorChannel nColorChannel)
        {
            Internal.StackPushInteger((int)nColorChannel);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(843);
            return Internal.StackPopInteger();
        }

        //  Set the color channel of oObject to the color specified.
        //  - oObject: the object for which you are changing the color.
        //             Can be a creature that has color information (i.e. the playable races).
        //  - nColorChannel: The color channel that you want to set the color value of.
        //                    COLOR_CHANNEL_SKIN
        //                    COLOR_CHANNEL_HAIR
        //                    COLOR_CHANNEL_TATTOO_1
        //                    COLOR_CHANNEL_TATTOO_2
        //  - nColorValue: The color you want to set (0-175).
        public static void SetColor(NWGameObject oObject, ColorChannel nColorChannel, int nColorValue)
        {
            Internal.StackPushInteger(nColorValue);
            Internal.StackPushInteger((int)nColorChannel);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(844);
        }

        //  Returns Item property Material.  You need to specify the Material Type.
        //  - nMasterialType: The Material Type should be a positive integer between 0 and 77 (see iprp_matcost.2da).
        //  Note: The Material Type property will only affect the cost of the item if you modify the cost in the iprp_matcost.2da.
        public static ItemProperty ItemPropertyMaterial(int nMaterialType)
        {
            Internal.StackPushInteger(nMaterialType);
            Internal.CallBuiltIn(845);
            return Internal.StackPopItemProperty();
        }

        //  Returns Item property Quality. You need to specify the Quality.
        //  - nQuality:  The Quality of the item property to create (see iprp_qualcost.2da).
        //               IP_CONST_QUALITY_*
        //  Note: The quality property will only affect the cost of the item if you modify the cost in the iprp_qualcost.2da.
        public static ItemProperty ItemPropertyQuality(IPConst nQuality)
        {
            Internal.StackPushInteger((int)nQuality);
            Internal.CallBuiltIn(846);
            return Internal.StackPopItemProperty();
        }

        //  Returns a generic Additional Item property. You need to specify the Additional property.
        //  - nProperty: The item property to create (see iprp_addcost.2da).
        //               IP_CONST_ADDITIONAL_*
        //  Note: The additional property only affects the cost of the item if you modify the cost in the iprp_addcost.2da.
        public static ItemProperty ItemPropertyAdditional(IPConst nAdditionalProperty)
        {
            Internal.StackPushInteger((int)nAdditionalProperty);
            Internal.CallBuiltIn(847);
            return Internal.StackPopItemProperty();
        }

        //  Sets a new tag for oObject.
        //  Will do nothing for invalid objects or the module object.
        // 
        //  Note: Care needs to be taken with this function.
        //        Changing the tag for creature with waypoints will make them stop walking them.
        //        Changing waypoint, door or trigger tags will break their area transitions.
        public static void SetTag(NWGameObject oObject, string sNewTag)
        {
            Internal.StackPushString(sNewTag);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(848);
        }

        //  Returns the string tag set for the provided effect.
        //  - If no tag has been set, returns an empty string.
        public static string GetEffectTag(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(849);
            return Internal.StackPopString();
        }

        //  Tags the effect with the provided string.
        //  - Any other tags in the link will be overwritten.
        public static Effect TagEffect(Effect eEffect, string sNewTag)
        {
            Internal.StackPushString(sNewTag);
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(850);
            return Internal.StackPopEffect();
        }

        //  Returns the caster level of the creature who created the effect.
        //  - If not created by a creature, returns 0.
        //  - If created by a spell-like ability, returns 0.
        public static int GetEffectCasterLevel(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(851);
            return Internal.StackPopInteger();
        }

        //  Returns the total duration of the effect in seconds.
        //  - Returns 0 if the duration type of the effect is not DURATION_TYPE_TEMPORARY.
        public static int GetEffectDuration(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(852);
            return Internal.StackPopInteger();
        }

        //  Returns the remaining duration of the effect in seconds.
        //  - Returns 0 if the duration type of the effect is not DURATION_TYPE_TEMPORARY.
        public static int GetEffectDurationRemaining(Effect eEffect)
        {
            Internal.StackPushEffect(eEffect);
            Internal.CallBuiltIn(853);
            return Internal.StackPopInteger();
        }

        //  Returns the string tag set for the provided item property.
        //  - If no tag has been set, returns an empty string.
        public static string GetItemPropertyTag(ItemProperty nProperty)
        {
            Internal.StackPushItemProperty(nProperty);
            Internal.CallBuiltIn(854);
            return Internal.StackPopString();
        }

        //  Tags the item property with the provided string.
        //  - Any tags currently set on the item property will be overwritten.
        public static ItemProperty TagItemProperty(ItemProperty nProperty, string sNewTag)
        {
            Internal.StackPushString(sNewTag);
            Internal.StackPushItemProperty(nProperty);
            Internal.CallBuiltIn(855);
            return Internal.StackPopItemProperty();
        }

        //  Returns the total duration of the item property in seconds.
        //  - Returns 0 if the duration type of the item property is not DURATION_TYPE_TEMPORARY.
        public static int GetItemPropertyDuration(ItemProperty nProperty)
        {
            Internal.StackPushItemProperty(nProperty);
            Internal.CallBuiltIn(856);
            return Internal.StackPopInteger();
        }

        //  Returns the remaining duration of the item property in seconds.
        //  - Returns 0 if the duration type of the item property is not DURATION_TYPE_TEMPORARY.
        public static int GetItemPropertyDurationRemaining(ItemProperty nProperty)
        {
            Internal.StackPushItemProperty(nProperty);
            Internal.CallBuiltIn(857);
            return Internal.StackPopInteger();
        }

        //  Instances a new area from the given resref, which needs to be a existing module area.
        //  Will optionally set a new area tag and displayed name. The new area is accessible
        //  immediately, but initialisation scripts for the area and all contained creatures will only
        //  run after the current script finishes (so you can clean up objects before returning).
        // 
        //  Returns the new area, or OBJECT_INVALID on failure.
        // 
        //  Note: When spawning a second instance of a existing area, you will have to manually
        //        adjust all transitions (doors, triggers) with the relevant script commands,
        //        or players might end up in the wrong area.
        public static NWGameObject CreateArea(string sResRef, string sNewTag = "", string sNewName = "")
        {
            Internal.StackPushString(sNewName);
            Internal.StackPushString(sNewTag);
            Internal.StackPushString(sResRef);
            Internal.CallBuiltIn(858);
            var result = Internal.StackPopObject();

            MessageHub.Instance.Publish(new AreaCreated(result));

            return result;
        }

        //  Destroys the given area object and everything in it.
        // 
        //  Return values:
        //     0: Object not an area or invalid.
        //    -1: Area contains spawn location and removal would leave module without entrypoint.
        //    -2: Players in area.
        //     1: Area destroyed successfully.
        public static int DestroyArea(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(859);
            return Internal.StackPopInteger();
        }

        //  Creates a copy of a existing area, including everything inside of it (except players).
        // 
        //  Returns the new area, or OBJECT_INVALID on error.
        // 
        //  Note: You will have to manually adjust all transitions (doors, triggers) with the
        //        relevant script commands, or players might end up in the wrong area.
        public static NWGameObject CopyArea(NWGameObject oArea)
        {
            Internal.StackPushObject(oArea, false);
            Internal.CallBuiltIn(860);
            var result = Internal.StackPopObject();

            MessageHub.Instance.Publish(new AreaCreated(result));

            return result;
        }

        //  Returns the first area in the module.
        public static NWGameObject GetFirstArea()
        {
            Internal.CallBuiltIn(861);
            return Internal.StackPopObject();
        }

        //  Returns the next area in the module (after GetFirstArea), or OBJECT_INVALID if no more
        //  areas are loaded.
        public static NWGameObject GetNextArea()
        {
            Internal.CallBuiltIn(862);
            return Internal.StackPopObject();
        }

        //  Sets the transition target for oTransition.
        // 
        //  Notes:
        //  - oTransition can be any valid game object, except areas.
        //  - oTarget can be any valid game object with a location, or OBJECT_INVALID (to unlink).
        //  - Rebinding a transition will NOT change the other end of the transition; for example,
        //    with normal doors you will have to do either end separately.
        //  - Any valid game object can hold a transition target, but only some are used by the game engine
        //    (doors and triggers). This might change in the future. You can still set and query them for
        //    other game objects from _.
        //  - Transition target objects are cached: The toolset-configured destination tag is
        //    used for a lookup only once, at first use. Thus, attempting to use SetTag() to change the
        //    destination for a transition will not work in a predictable fashion.
        public static void SetTransitionTarget(NWGameObject oTransition, NWGameObject oTarget)
        {
            Internal.StackPushObject(oTarget, false);
            Internal.StackPushObject(oTransition, false);
            Internal.CallBuiltIn(863);
        }

        //  Sets whether the provided item should be hidden when equipped.
        //  - The intended usage of this function is to provide an easy way to hide helmets, but it
        //    can be used equally for any slot which has creature mesh visibility when equipped,
        //    e.g.: armour, helm, cloak, left hand, and right hand.
        //  - nValue should be TRUE or FALSE.
        public static void SetHiddenWhenEquipped(NWGameObject oItem, bool nValue)
        {
            Internal.StackPushInteger(Convert.ToInt32(nValue));
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(864);
        }

        //  Returns whether the provided item is hidden when equipped.
        public static bool GetHiddenWhenEquipped(NWGameObject oItem)
        {
            Internal.StackPushObject(oItem, false);
            Internal.CallBuiltIn(865);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets if the given creature has explored tile at x, y of the given area.
        //  Note that creature needs to be a player- or player-possessed creature.
        // 
        //  Keep in mind that tile exploration also controls object visibility in areas
        //  and the fog of war for interior and underground areas.
        // 
        //  Return values:
        //   -1: Area or creature invalid.
        //    0: Tile was not explored before setting newState.
        //    1: Tile was explored before setting newState.
        public static int SetTileExplored(NWGameObject creature, NWGameObject area, int x, int y, bool newState)
        {
            Internal.StackPushInteger(Convert.ToInt32(newState));
            Internal.StackPushInteger(y);
            Internal.StackPushInteger(x);
            Internal.StackPushObject(area, false);
            Internal.StackPushObject(creature, false);
            Internal.CallBuiltIn(866);
            return Internal.StackPopInteger();
        }

        //  Returns whether the given tile at x, y, for the given creature in the stated
        //  area is visible on the map.
        //  Note that creature needs to be a player- or player-possessed creature.
        // 
        //  Keep in mind that tile exploration also controls object visibility in areas
        //  and the fog of war for interior and underground areas.
        // 
        //  Return values:
        //   -1: Area or creature invalid.
        //    0: Tile is not explored yet.
        //    1: Tile is explored.
        public static int GetTileExplored(NWGameObject creature, NWGameObject area, int x, int y)
        {
            Internal.StackPushInteger(y);
            Internal.StackPushInteger(x);
            Internal.StackPushObject(area, false);
            Internal.StackPushObject(creature, false);
            Internal.CallBuiltIn(867);
            return Internal.StackPopInteger();
        }

        //  Sets the creature to auto-explore the map as it walks around.
        // 
        //  Keep in mind that tile exploration also controls object visibility in areas
        //  and the fog of war for interior and underground areas.
        // 
        //  This means that if you turn off auto exploration, it falls to you to manage this
        //  through SetTileExplored(); otherwise, the player will not be able to see anything.
        // 
        //  Valid arguments: TRUE and FALSE.
        //  Does nothing for non-creatures.
        //  Returns the previous state (or -1 if non-creature).
        public static int SetCreatureExploresMinimap(NWGameObject creature, bool newState)
        {
            Internal.StackPushInteger(Convert.ToInt32(newState));
            Internal.StackPushObject(creature, false);
            Internal.CallBuiltIn(868);
            return Internal.StackPopInteger();
        }

        //  Returns TRUE if the creature is set to auto-explore the map as it walks around (on by default).
        //  Returns FALSE if creature is not actually a creature.
        public static bool GetCreatureExploresMinimap(NWGameObject creature)
        {
            Internal.StackPushObject(creature, false);
            Internal.CallBuiltIn(869);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Get the surface material at the given location. (This is
        //  equivalent to the walkmesh type).
        //  Returns 0 if the location is invalid or has no surface type.
        public static int GetSurfaceMaterial(Location at)
        {
            Internal.StackPushLocation(at);
            Internal.CallBuiltIn(870);
            return Internal.StackPopInteger();
        }

        //  Returns the z-offset at which the walkmesh is at the given location.
        //  Returns -6.0 for invalid locations.
        public static float GetGroundHeight(Location at)
        {
            Internal.StackPushLocation(at);
            Internal.CallBuiltIn(871);
            return Internal.StackPopFloat();
        }

        //  Gets the attack bonus limit.
        //  - The default value is 20.
        public static int GetAttackBonusLimit()
        {
            Internal.CallBuiltIn(872);
            return Internal.StackPopInteger();
        }

        //  Gets the damage bonus limit.
        //  - The default value is 100.
        public static int GetDamageBonusLimit()
        {
            Internal.CallBuiltIn(873);
            return Internal.StackPopInteger();
        }

        //  Gets the saving throw bonus limit.
        //  - The default value is 20.
        public static int GetSavingThrowBonusLimit()
        {
            Internal.CallBuiltIn(874);
            return Internal.StackPopInteger();
        }

        //  Gets the ability bonus limit.
        //  - The default value is 12.
        public static int GetAbilityBonusLimit()
        {
            Internal.CallBuiltIn(875);
            return Internal.StackPopInteger();
        }

        //  Gets the ability penalty limit.
        //  - The default value is 30.
        public static int GetAbilityPenaltyLimit()
        {
            Internal.CallBuiltIn(876);
            return Internal.StackPopInteger();
        }

        //  Gets the skill bonus limit.
        //  - The default value is 50.
        public static int GetSkillBonusLimit()
        {
            Internal.CallBuiltIn(877);
            return Internal.StackPopInteger();
        }

        //  Sets the attack bonus limit.
        //  - The minimum value is 0.
        public static void SetAttackBonusLimit(int nNewLimit)
        {
            Internal.StackPushInteger(nNewLimit);
            Internal.CallBuiltIn(878);
        }

        //  Sets the damage bonus limit.
        //  - The minimum value is 0.
        public static void SetDamageBonusLimit(int nNewLimit)
        {
            Internal.StackPushInteger(nNewLimit);
            Internal.CallBuiltIn(879);
        }

        //  Sets the saving throw bonus limit.
        //  - The minimum value is 0.
        public static void SetSavingThrowBonusLimit(int nNewLimit)
        {
            Internal.StackPushInteger(nNewLimit);
            Internal.CallBuiltIn(880);
        }

        //  Sets the ability bonus limit.
        //  - The minimum value is 0.
        public static void SetAbilityBonusLimit(int nNewLimit)
        {
            Internal.StackPushInteger(nNewLimit);
            Internal.CallBuiltIn(881);
        }

        //  Sets the ability penalty limit.
        //  - The minimum value is 0.
        public static void SetAbilityPenaltyLimit(int nNewLimit)
        {
            Internal.StackPushInteger(nNewLimit);
            Internal.CallBuiltIn(882);
        }

        //  Sets the skill bonus limit.
        //  - The minimum value is 0.
        public static void SetSkillBonusLimit(int nNewLimit)
        {
            Internal.StackPushInteger(nNewLimit);
            Internal.CallBuiltIn(883);
        }

        //  Get if oPlayer is currently connected over a relay (instead of directly).
        //  Returns FALSE for any other object, including OBJECT_INVALID.
        public static bool GetIsPlayerConnectionRelayed(NWGameObject oPlayer)
        {
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(884);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        private static string GetEventScriptInternal(NWGameObject oObject, int nHandler)
        {
            Internal.StackPushInteger(nHandler);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(885);
            return Internal.StackPopString();
        }

        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptArea nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptAreaOfEffect nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptCreature nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptDoor nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptEncounter nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptModule nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptPlaceable nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptStore nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }
        //  Returns the event script for the given object and handler.
        //  Will return "" if unset, the object is invalid, or the object cannot
        //  have the requested handler.
        public static string GetEventScript(NWGameObject oObject, EventScriptTrigger nHandler)
        {
            return GetEventScriptInternal(oObject, (int)nHandler);
        }

        private static bool SetEventScriptInternal(NWGameObject oObject, int nHandler, string sScript)
        {
            Internal.StackPushString(sScript);
            Internal.StackPushInteger(nHandler);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(886);
            return Convert.ToBoolean(Internal.StackPopInteger());
        }

        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptArea nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptAreaOfEffect nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptCreature nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptDoor nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptEncounter nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptModule nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptPlaceable nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptStore nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }
        //  Sets the given event script for the given object and handler.
        //  Returns 1 on success, 0 on failure.
        //  Will fail if oObject is invalid or does not have the requested handler.
        public static bool SetEventScript(NWGameObject oObject, EventScriptTrigger nHandler, string sScript)
        {
            return SetEventScriptInternal(oObject, (int)nHandler, sScript);
        }

        //  Gets a visual transform on the given object.
        //  - oObject can be any valid Creature, Placeable, Item or Door.
        //  - nTransform is one of OBJECT_VISUAL_TRANSFORM_*
        //  Returns the current (or default) value.
        public static float GetObjectVisualTransform(NWGameObject oObject, ObjectVisualTransform nTransform)
        {
            Internal.StackPushInteger((int)nTransform);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(887);
            return Internal.StackPopFloat();
        }

        //  Sets a visual transform on the given object.
        //  - oObject can be any valid Creature, Placeable, Item or Door.
        //  - nTransform is one of OBJECT_VISUAL_TRANSFORM_*
        //  - fValue depends on the transformation to apply.
        //  Returns the old/previous value.
        public static float SetObjectVisualTransform(NWGameObject oObject, ObjectVisualTransform nTransform, float fValue)
        {
            Internal.StackPushFloat(fValue);
            Internal.StackPushInteger((int)nTransform);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(888);
            return Internal.StackPopFloat();
        }

        //  Sets an integer material shader uniform override.
        //  - sMaterial needs to be a material on that object.
        //  - sParam needs to be a valid shader parameter already defined on the material.
        public static void SetMaterialShaderUniformInt(NWGameObject oObject, string sMaterial, string sParam, int nValue)
        {
            Internal.StackPushInteger(nValue);
            Internal.StackPushString(sParam);
            Internal.StackPushString(sMaterial);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(889);
        }

        //  Sets a vec4 material shader uniform override.
        //  - sMaterial needs to be a material on that object.
        //  - sParam needs to be a valid shader parameter already defined on the material.
        //  - You can specify a single float value to set just a float, instead of a vec4.
        public static void SetMaterialShaderUniformVec4(NWGameObject oObject, string sMaterial, string sParam, float fValue1, float fValue2 = 0.0f, float fValue3 = 0.0f, float fValue4 = 0.0f)
        {
            Internal.StackPushFloat(fValue4);
            Internal.StackPushFloat(fValue3);
            Internal.StackPushFloat(fValue2);
            Internal.StackPushFloat(fValue1);
            Internal.StackPushString(sParam);
            Internal.StackPushString(sMaterial);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(890);
        }

        //  Resets material shader parameters on the given object:
        //  - Supply a material to only reset shader uniforms for meshes with that material.
        //  - Supply a parameter to only reset shader uniforms of that name.
        //  - Supply both to only reset shader uniforms of that name on meshes with that material.
        public static void ResetMaterialShaderUniforms(NWGameObject oObject, string sMaterial = "", string sParam = "")
        {
            Internal.StackPushString(sParam);
            Internal.StackPushString(sMaterial);
            Internal.StackPushObject(oObject, false);
            Internal.CallBuiltIn(891);
        }

        //  Vibrate the player's device or controller. Does nothing if vibration is not supported.
        //  - nMotor is one of VIBRATOR_MOTOR_*
        //  - fStrength is between 0.0 and 1.0
        //  - fSeconds is the number of seconds to vibrate
        public static void Vibrate(NWGameObject oPlayer, VibratorMotor nMotor, float fStrength, float fSeconds)
        {
            Internal.StackPushFloat(fSeconds);
            Internal.StackPushFloat(fStrength);
            Internal.StackPushInteger((int)nMotor);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(892);
        }

        //  Unlock an achievement for the given player who must be logged in.
        //  - sId is the achievement ID on the remote server
        //  - nLastValue is the previous value of the associated achievement stat
        //  - nCurValue is the current value of the associated achievement stat
        //  - nMaxValue is the maximum value of the associate achievement stat
        public static void UnlockAchievement(NWGameObject oPlayer, string sId, int nLastValue = 0, int nCurValue = 0, int nMaxValue = 0)
        {
            Internal.StackPushInteger(nMaxValue);
            Internal.StackPushInteger(nCurValue);
            Internal.StackPushInteger(nLastValue);
            Internal.StackPushString(sId);
            Internal.StackPushObject(oPlayer, false);
            Internal.CallBuiltIn(893);
        }

        public static void AssignCommand(NWGameObject oActionSubject, ActionDelegate aActionToAssign)
        {
            Internal.ClosureAssignCommand(oActionSubject, aActionToAssign);
        }

        public static void DelayCommand(float fSeconds, ActionDelegate aActionToDelay)
        {
            Internal.ClosureDelayCommand(NWGameObject.OBJECT_SELF, fSeconds, aActionToDelay);
        }

        public static void ActionDoCommand(ActionDelegate aActionToDelay)
        {
            Internal.ClosureActionDoCommand(NWGameObject.OBJECT_SELF, aActionToDelay);
        }

        /// <summary>
        /// Returns true if obj is a player. If obj is a DM, DM-possessed, or any other type of object it will return false.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if player, false otherwise</returns>
        public static bool GetIsPlayer(NWGameObject obj)
        {
            return _.GetIsPC(obj) && !_.GetIsDM(obj) && !_.GetIsDMPossessed(obj);
        }

        /// <summary>
        /// Returns true if obj is a DM or DM-possessed. Players or any other type of object will return false.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if DM or DM-possessed, false otherwise</returns>
        public static bool GetIsDungeonMaster(NWGameObject obj)
        {
            return GetIsDM(obj) || GetIsDMPossessed(obj);
        }

        /// <summary>
        /// Returns true if obj is a non-player, non-DM, non-possessed creature. 
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if object is a NPC or false if not</returns>
        public static bool GetIsNPC(NWGameObject obj)
        {
            return !GetIsPlayer(obj) && 
                   !GetIsDungeonMaster(obj) && 
                   GetObjectType(obj) == ObjectType.Creature;
        }

        /// <summary>
        /// Retrieves a unique ID for a given object.
        /// Throws an exception if a player has not been assigned an ID yet.
        /// Assigns a new ID if a non-player has not been assigned an ID yet.
        /// </summary>
        /// <param name="obj">The object to retrieve the ID from</param>
        /// <returns>The ID of the object</returns>
        public static Guid GetGlobalID(NWGameObject obj)
        {
            if (GetIsPC(obj) && !GetIsDM(obj) && !GetIsDMPossessed(obj))
            {
                string tag = GetTag(obj);
                if (string.IsNullOrWhiteSpace(tag))
                    throw new Exception($"Player has not been assigned an ID yet. Player Name: {GetName(obj)}");

                return new Guid(tag);
            }
            else
            {
                var id = GetLocalString(obj, "GLOBAL_ID");
                if(string.IsNullOrWhiteSpace(id))
                {
                    id = Guid.NewGuid().ToString();
                    SetLocalString(obj, "GLOBAL_ID", id);
                }

                return new Guid(id);
            } 
        }

        /// <summary>
        /// Gets an area by its resref. Returns OBJECT_INVALID if no area with the given resref can be found.
        /// </summary>
        /// <param name="resRef">The resref to search for.</param>
        /// <returns>An area with the matching resref, or OBJECT_INVALID if no area could be found.</returns>
        public static NWGameObject GetAreaByResRef(string resRef)
        {
            NWGameObject area = GetFirstArea();

            while(GetIsObjectValid(area))
            {
                if (GetResRef(area) == resRef)
                    return area;

                area = GetNextArea();
            }

            return NWGameObject.OBJECT_INVALID;
        }

        /// <summary>
        /// Destroys all items inside an object's inventory.
        /// </summary>
        /// <param name="obj">The objects whose inventory will be wiped.</param>
        public static void DestroyAllInventoryItems(NWGameObject obj)
        {
            NWGameObject item = _.GetFirstItemInInventory(obj);
            while (GetIsObjectValid(item))
            {
                DestroyObject(item);
                item = GetNextItemInInventory(obj);
            }
        }

        /// <summary>
        /// Returns the number of items in an object's inventory.
        /// Returns -1 if target does not have an inventory
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>-1 if obj doesn't have an inventory, otherwise returns the number of items in the inventory</returns>
        public static int GetInventoryItemCount(NWGameObject obj)
        {
            if (!GetHasInventory(obj)) return -1;

            int count = 0;
            NWGameObject item = GetFirstItemInInventory(obj);
            while (GetIsObjectValid(item))
            {
                count++;
                item = GetNextItemInInventory(obj);
            }

            return count;
        }

        /// <summary>
        /// If creature is currently busy, returns true.
        /// Otherwise returns false.
        /// </summary>
        /// <param name="creature">The creature to check busy status of</param>
        /// <returns>true if busy, false otherwise</returns>
        public static bool GetIsBusy(NWGameObject creature)
        {
            return Convert.ToBoolean(GetLocalString(creature, "IS_BUSY"));
        }

        /// <summary>
        /// Sets whether creature is busy.
        /// </summary>
        /// <param name="creature">The creature to change the busy status of</param>
        /// <param name="isBusy">true or false</param>
        public static void SetIsBusy(NWGameObject creature, bool isBusy)
        {
            SetLocalInt(creature, "IS_BUSY", Convert.ToInt32(isBusy));
        }
    }
}