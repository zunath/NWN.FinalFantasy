using System;
using System.Collections.Generic;
using System.Text;
using Internal = NWN.FinalFantasy.Core.Internal;

// ReSharper disable once CheckNamespace
namespace NWN.FinalFantasy.Core.NWScript
{
    public partial class NWScript
    {
        /// <summary>
        ///   Destroy oObject (irrevocably).
        ///   This will not work on modules and areas.
        /// </summary>
        public static void DestroyObject(uint oDestroy, float fDelay = 0.0f)
        {
            Internal.NativeFunctions.StackPushFloat(fDelay);
            Internal.NativeFunctions.StackPushObject(oDestroy);
            Internal.NativeFunctions.CallBuiltIn(241);

            ExecuteScript("object_destroyed", oDestroy);
        }
    }
}
