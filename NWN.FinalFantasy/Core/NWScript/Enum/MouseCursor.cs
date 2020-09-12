﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NWN.FinalFantasy.Core.NWScript.Enum
{
    public enum MouseCursor
    {
        Invalid = -1,
        Default = 1,
        DefaultDown = 2,
        Walk = 3,
        WalkDown = 4,
        NoWalk = 5,
        NoWalkDown = 6,
        Attack = 7,
        AttackDown = 8,
        NoAttack = 9,
        NoAttackDown = 10,
        Talk = 11,
        TalkDown = 12,
        NoTalk = 13,
        NoTalkDown = 14,
        Follow = 15,
        FollowDown = 16,
        Examine = 17,
        ExamineDown = 18,
        NoExamine = 19,
        NoExamineDown = 20,
        Transition = 21,
        TransitionDown = 22,
        Door = 23,
        DoorDown = 24,
        Use = 25,
        UseDown = 26,
        Mouse = 27,
        MouseDown = 28,
        Magic = 29,
        MagicDown = 30,
        NoMagic = 31,
        NoMagicDown = 32,
        Disarm = 33,
        DisarmDown = 34,
        NoDisarm = 35,
        NoDisarmDown = 36,
        Action = 37,
        ActionDown = 38,
        NoAction = 39,
        NoActionDown = 40,
        Lock = 41,
        LockDown = 42,
        NoLock= 43,
        NoLockDown = 44,
        PushPin = 45,
        PushPinDown = 46,
        Create = 47,
        CreateDown = 48,
        NoCreate = 49,
        NoCreateDown = 50,
        Kill = 51,
        KillDown = 52,
        NoKill = 53,
        NoKillDown = 54,
        Heal = 55,
        HealDown = 56,
        NoHeal = 57,
        NohealDown = 58,
        RunArrow = 59,
        WalkArrow = 75,
        PickUp = 91,
        PickUpDown = 92,
        Custom00 = 93,       // gui_mp_custom00u
        Custom00Down = 94,  // gui_mp_custom00d

        // More custom mouse cursors can be added here.

        Custom99 = 291,      // gui_mp_custom99u
        Custom99Down = 292, // gui_mp_custom99d
    }
}
