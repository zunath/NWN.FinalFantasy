namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXItem
    {
        private const string NWNX_Item = "NWNX_Item";

        // Set oItem's weight. Will not persist through saving.
        public static void SetWeight(NWGameObject oItem, int w)
        {
            string sFunc = "SetWeight";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, w);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
        }

        // Set oItem's base value in gold pieces (Total cost = base_value +
        // additional_value). Will not persist through saving.
        // NOTE: Equivalent to SetGoldPieceValue NWNX2 function
        public static void SetBaseGoldPieceValue(NWGameObject oItem, int g)
        {
            string sFunc = "SetBaseGoldPieceValue";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, g);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
        }

        // Set oItem's additional value in gold pieces (Total cost = base_value +
        // additional_value). Will persist through saving.
        public static void SetAddGoldPieceValue(NWGameObject oItem, int g)
        {
            string sFunc = "SetAddGoldPieceValue";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, g);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
        }

        // Get oItem's base value in gold pieces.
        public static int GetBaseGoldPieceValue(NWGameObject oItem)
        {
            string sFunc = "GetBaseGoldPieceValue";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Item, sFunc);
        }

        // Get oItem's additional value in gold pieces.
        public static int GetAddGoldPieceValue(NWGameObject oItem)
        {
            string sFunc = "GetAddGoldPieceValue";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Item, sFunc);
        }

        // Set oItem's base item type. This will not be visible until the
        // item is refreshed (e.g. drop and take the item, or logging out
        // and back in).
        public static void SetBaseItemType(NWGameObject oItem, int nBaseItem)
        {
            string sFunc = "SetBaseItemType";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, nBaseItem);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
        }

        // Make a single change to the appearance of an item. This will not be visible to PCs until
        // the item is refreshed for them (e.g. by logging out and back in).
        // Helmet models and simple items ignore iIndex.
        // nType                            nIndex                              nValue
        // ITEM_APPR_TYPE_SIMPLE_MODEL      [Ignored]                           Model #
        // ITEM_APPR_TYPE_WEAPON_COLOR      ITEM_APPR_WEAPON_COLOR_*            0-255
        // ITEM_APPR_TYPE_WEAPON_MODEL      ITEM_APPR_WEAPON_MODEL_*            Model #
        // ITEM_APPR_TYPE_ARMOR_MODEL       ITEM_APPR_ARMOR_MODEL_*             Model #
        // ITEM_APPR_TYPE_ARMOR_COLOR       ITEM_APPR_ARMOR_COLOR_* [0]         0-255 [1]
        //
        // [0] Alternatively, where ITEM_APPR_TYPE_ARMOR_COLOR is specified, if per-part coloring is
        // desired, the following equation can be used for nIndex to achieve that:
        //
        // ITEM_APPR_ARMOR_NUM_COLORS + (ITEM_APPR_ARMOR_MODEL_ * ITEM_APPR_ARMOR_NUM_COLORS) + ITEM_APPR_ARMOR_COLOR_
        //
        // For example, to change the CLOTH1 channel of the torso, nIndex would be:
        //
        //     6 + (7 * 6) + 2 = 50
        //
        // [1] When specifying per-part coloring, the value 255 corresponds with the logical
        // function 'clear colour override', which clears the per-part override for that part.
        public static void SetItemAppearance(NWGameObject oItem, int nType, int nIndex, int nValue)
        {
            string sFunc = "SetItemAppearance";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, nValue);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, nIndex);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Item, sFunc, nType);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);

        }

        // Return a String containing the entire appearance for oItem which can later be
        // passed to RestoreItemAppearance().
        public static string GetEntireItemAppearance(NWGameObject oItem)
        {
            string sFunc = "GetEntireItemAppearance";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Item, sFunc);
        }

        // Restore an item's appearance with the value returned by GetEntireItemAppearance().
        public static void RestoreItemAppearance(NWGameObject oItem, string sApp)
        {
            string sFunc = "RestoreItemAppearance";

            NWNXCore.NWNX_PushArgumentString(NWNX_Item, sFunc, sApp);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
        }


        public static int GetBaseArmorClass(NWGameObject oItem)
        {
            string sFunc = "GetBaseArmorClass";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Item, sFunc, oItem);

            NWNXCore.NWNX_CallFunction(NWNX_Item, sFunc);
            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Item, sFunc);
        }
    }
}
