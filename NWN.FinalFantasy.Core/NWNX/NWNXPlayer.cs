namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXPlayer
    {
        private const string NWNX_Player = "NWNX_Player";

        /// <summary> 
        /// Force display placeable examine window for player
        /// If used on a placeable in a different area than the player, the portait will not be shown.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="placeable"></param>
        public static void ForcePlaceableExamineWindow(NWGameObject player, NWGameObject placeable)
        {
            string sFunc = "ForcePlaceableExamineWindow";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Force opens the target object's inventory for the player.
        /// A few notes about this function:
        /// - If the placeable is in a different area than the player, the portrait will not be shown
        /// - The placeable's open/close animations will be played
        /// - Clicking the 'close' button will cause the player to walk to the placeable;
        ///     If the placeable is in a different area, the player will just walk to the edge
        ///     of the current area and stop. This action can be cancelled manually.
        /// - Walking will close the placeable automatically.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="placeable"></param>
        public static void ForcePlaceableInventoryWindow(NWGameObject player, NWGameObject placeable)
        {
            string sFunc = "ForcePlaceableInventoryWindow";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Starts displaying a timing bar.
        /// Will run a script at the end of the timing bar, if specified.
        /// </summary>
        /// <param name="creature">The creature who will see the timing bar.</param>
        /// <param name="seconds">How long the timing bar should come on screen.</param>
        /// <param name="script">The script to run at the end of the timing bar.</param>
        public static void StartGuiTimingBar(NWGameObject creature, float seconds, string script)
        {
            // only one timing bar at a time!
            if (_.GetLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ACTIVE") == 1)
                return;

            string sFunc = "StartGuiTimingBar";
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, seconds);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, creature);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);

            int id = _.GetLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ID") + 1;
            _.SetLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ACTIVE", id);
            _.SetLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ID", id);

            _.DelayCommand(seconds, () =>
            {
                StopGuiTimingBar(creature, script, -1);
            });
        }

        /// <summary>
        /// Stops displaying a timing bar.
        /// Runs a script if specified.
        /// </summary>
        /// <param name="creature">The creature's timing bar to stop.</param>
        /// <param name="script">The script to run once ended.</param>
        /// <param name="id">ID number of this timing bar.</param>
        public static void StopGuiTimingBar(NWGameObject creature, string script, int id)
        {
            int activeId = _.GetLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ACTIVE");
            // Either the timing event was never started, or it already finished.
            if (activeId == 0)
                return;

            // If id != -1, we ended up here through DelayCommand. Make sure it's for the right ID
            if (id != -1 && id != activeId)
                return;

            _.DeleteLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ACTIVE");

            string sFunc = "StopGuiTimingBar";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, creature);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);

            if (!string.IsNullOrWhiteSpace(script))
            {
                _.ExecuteScript(script, creature);
            }
        }


        /// <summary>
        /// Stops displaying a timing bar.
        /// Runs a script if specified.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="script"></param>
        public static void StopGuiTimingBar(NWGameObject player, string script)
        {
            StopGuiTimingBar(player, script, -1);
        }

        /// <summary>
        /// Sets whether the player should always walk when given movement commands.
        /// If true, clicking on the ground or using WASD will trigger walking instead of running.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="bWalk"></param>
        public static void SetAlwaysWalk(NWGameObject player, int bWalk)
        {
            string sFunc = "SetAlwaysWalk";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, bWalk);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Gets the player's quickbar slot info
        /// </summary>
        /// <param name="player"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static QuickBarSlot GetQuickBarSlot(NWGameObject player, int slot)
        {
            string sFunc = "GetQuickBarSlot";
            QuickBarSlot qbs = new QuickBarSlot();

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, slot);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);

            qbs.Associate = (NWNXCore.NWNX_GetReturnValueObject(NWNX_Player, sFunc));
            qbs.AssociateType = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.DomainLevel = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.MetaType = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.INTParam1 = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.ToolTip = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.CommandLine = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.CommandLabel = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.Resref = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.MultiClass = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.ObjectType = (QuickBarSlotType)NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.SecondaryItem = (NWNXCore.NWNX_GetReturnValueObject(NWNX_Player, sFunc));
            qbs.Item = (NWNXCore.NWNX_GetReturnValueObject(NWNX_Player, sFunc));

            return qbs;
        }

        /// <summary>
        /// Sets a player's quickbar slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="slot"></param>
        /// <param name="qbs"></param>
        public static void SetQuickBarSlot(NWGameObject player, int slot, QuickBarSlot qbs)
        {
            string sFunc = "SetQuickBarSlot";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, qbs.Item);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, qbs.SecondaryItem);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, (int)qbs.ObjectType);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.MultiClass);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.Resref);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.CommandLabel);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.CommandLine);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.ToolTip);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.INTParam1);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.MetaType);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.DomainLevel);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.AssociateType);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, qbs.Associate);

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, slot);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }


        /// <summary>
        /// Get the name of the .bic file associated with the player's character.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string GetBicFileName(NWGameObject player)
        {
            string sFunc = "GetBicFileName";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Plays the VFX at the target position in current area for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="effectId"></param>
        /// <param name="position"></param>
        public static void ShowVisualEffect(NWGameObject player, int effectId, Vector position)
        {
            string sFunc = "ShowVisualEffect";
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, position.X);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, position.Y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, position.Z);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, effectId);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Changes the daytime music track for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="track"></param>
        public static void MusicBackgroundChangeDay(NWGameObject player, int track)
        {
            string sFunc = "ChangeBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, track);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, 1); // bool day = TRUE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Changes the nighttime music track for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="track"></param>
        public static void MusicBackgroundChangeNight(NWGameObject player, int track)
        {
            string sFunc = "ChangeBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, track);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, 0); // bool day = FALSE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Starts the background music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBackgroundStart(NWGameObject player)
        {
            string sFunc = "PlayBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, 1); // bool play = TRUE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Stops the background music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBackgroundStop(NWGameObject player)
        {
            string sFunc = "PlayBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, 0); // bool play = FALSE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Changes the battle music track for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="track"></param>
        public static void MusicBattleChange(NWGameObject player, int track)
        {
            string sFunc = "ChangeBattleMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, track);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Starts the battle music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBattleStart(NWGameObject player)
        {
            string sFunc = "PlayBattleMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, 1); // bool play = TRUE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Stops the background music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBattleStop(NWGameObject player)
        {
            string sFunc = "PlayBattleMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, 0); // bool play = FALSE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Play a sound at the location of target for the given player only
        /// If target is OBJECT_INVALID the sound will play at the location of the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="target"></param>
        public static void PlaySound(NWGameObject player, string sound, NWGameObject target)
        {
            string sFunc = "PlaySound";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, sound);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Toggle a placeable's usable flag for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="placeable"></param>
        /// <param name="isUseable"></param>
        public static void SetPlaceableUseable(NWGameObject player, NWGameObject placeable, bool isUseable)
        {
            string sFunc = "SetPlaceableUsable";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, isUseable ? 1 : 0);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Override player's rest duration
        /// Duration is in milliseconds, 1000 = 1 second
        /// Minimum duration of 10ms
        /// -1 clears the override
        /// </summary>
        /// <param name="player"></param>
        /// <param name="duration"></param>
        public static void SetRestDuration(NWGameObject player, int duration)
        {
            string sFunc = "SetRestDuration";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, duration);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }


        /// <summary>
        /// Apply visualeffect to target that only player can see
        /// Note: Only works with instant effects: VFX_COM_*, VFX_FNF_*, VFX_IMP_*
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="visualeffect"></param>
        public static void ApplyInstantVisualEffectToObject(NWGameObject player, NWGameObject target, int visualeffect)
        {
            string sFunc = "ApplyInstantVisualEffectToObject";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, visualeffect);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// // Refreshes the players character sheet
        /// Note: You may need to use DelayCommand if you're manipulating values
        /// through nwnx and forcing a UI refresh, 0.5s seemed to be fine
        /// </summary>
        /// <param name="player"></param>
        public static void UpdateCharacterSheet(NWGameObject player)
        {
            string sFunc = "UpdateCharacterSheet";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Allows player to open target's inventory
        /// Target must be a creature or another player
        /// Note: only works if player and target are in the same area
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="open"></param>
        public static void OpenInventory(NWGameObject player, NWGameObject target, bool open = true)
        {
            string sFunc = "OpenInventory";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, open ? 1 : 0);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Get player's area exploration state
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="area">The area</param>
        /// <returns></returns>
        public static string GetAreaExplorationState(NWGameObject player, NWGameObject area)
        {
            string sFunc = "GetAreaExplorationState";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, area);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Set player's area exploration state (str is an encoded string obtained with NWNX_Player_GetAreaExplorationState)
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="area">The area</param>
        /// <param name="str">The encoded exploration state string</param>
        public static void SetAreaExplorationState(NWGameObject player, NWGameObject area, string str)
        {
            string sFunc = "SetAreaExplorationState";
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, str);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, area);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Override oPlayer's rest animation to nAnimation
        ///
        /// NOTE: nAnimation does not take ANIMATION_LOOPING_* or ANIMATION_FIREFORGET_* constants
        ///       Use NWNX_Consts_TranslateNWScriptAnimation() in nwnx_consts.nss to get their NWNX equivalent
        ///       -1 to clear the override
        /// </summary>
        /// <param name="oPlayer">The player object</param>
        /// <param name="nAnimation">The rest animation</param>
        public static void SetRestAnimation(NWGameObject oPlayer, int nAnimation)
        {
            string sFunc = "SetRestAnimation";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, nAnimation);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, oPlayer);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Override a visual transform on the given object that only oPlayer will see.
        /// - oObject can be any valid Creature, Placeable, Item or Door.
        /// - nTransform is one of OBJECT_VISUAL_TRANSFORM_* or -1 to remove the override
        /// - fValue depends on the transformation to apply.
        /// </summary>
        /// <param name="oPlayer">The player object</param>
        /// <param name="oObject">The object to transform</param>
        /// <param name="nTransform">The transformation type</param>
        /// <param name="fValue">The amount to transform by</param>
        public static void SetObjectVisualTransformOverride(NWGameObject oPlayer, NWGameObject oObject, int nTransform, float fValue)
        {
            string sFunc = "SetObjectVisualTransformOverride";

            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, fValue);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, nTransform);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, oObject);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, oPlayer);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Apply a looping visualeffect to target that only player can see
        /// visualeffect: VFX_DUR_*, call again to remove an applied effect
        ///               -1 to remove all effects
        ///
        /// Note: Only really works with looping effects: VFX_DUR_*
        ///       Other types *kind* of work, they'll play when reentering the area and the object is in view
        ///       or when they come back in view range.
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="target">The target to apply the visual effect to</param>
        /// <param name="visualeffect">The visual effect to use</param>
        public static void ApplyLoopingVisualEffectToObject(NWGameObject player, NWGameObject target, int visualeffect)
        {
            string sFunc = "ApplyLoopingVisualEffectToObject";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, visualeffect);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Override the name of placeable for player only
        /// "" to clear the override
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="placeable">The placeable object</param>
        /// <param name="name">The new name to use</param>
        public static void SetPlaceableNameOverride(NWGameObject player, NWGameObject placeable, string name)
        {
            string sFunc = "SetPlaceableNameOverride";

            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, name);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Gets whether a quest has been completed by a player
        /// Returns -1 if they don't have the journal entry
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="sQuestName">The name of the quest</param>
        /// <returns></returns>
        public static int GetQuestCompleted(NWGameObject player, string sQuestName)
        {
            string sFunc = "GetQuestCompleted";
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, sQuestName);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
        }
    }
}
