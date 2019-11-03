using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXObject
    {
        private const string NWNX_Object = "NWNX_Object";

        /// <summary>
        /// Gets the count of all local variables on the provided object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetLocalVariableCount(NWGameObject obj)
        {
            string sFunc = "GetLocalVariableCount";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Returns a local variable at the specified index.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static LocalVariable GetLocalVariable(NWGameObject obj, int index)
        {
            string sFunc = "GetLocalVariable";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, index);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            LocalVariable var = new LocalVariable();
            var.Key = NWNXCore.NWNX_GetReturnValueString(NWNX_Object, sFunc);
            var.Type = (LocalVariableType)NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
            return var;
        }

        /// <summary>
        /// Returns an object from the provided object ID.
        /// This is the counterpart to ObjectToString.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NWGameObject StringToObject(string id)
        {
            string sFunc = "StringToObject";

            NWNXCore.NWNX_PushArgumentString(NWNX_Object, sFunc, id);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Set the provided object's position to the provided vector.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        public static void SetPosition(NWGameObject obj, Vector pos)
        {
            string sFunc = "SetPosition";

            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.X);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.Y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.Z);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

        }

        /// <summary>
        /// Sets the provided object's current hit points to the provided value.
        /// </summary>
        /// <param name="creature"></param>
        /// <param name="hp"></param>
        public static void SetCurrentHitPoints(NWGameObject creature, int hp)
        {
            string sFunc = "SetCurrentHitPoints";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, hp);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, creature);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Set object's maximum hit points; will not work on PCs.
        /// </summary>
        /// <param name="creature"></param>
        /// <param name="hp"></param>
        public static void SetMaxHitPoints(NWGameObject creature, int hp)
        {
            string sFunc = "SetMaxHitPoints";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, hp);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, creature);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Serialize the full object (including locals, inventory, etc) to base64 string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(NWGameObject obj)
        {
            string sFunc = "Serialize";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Deserialize the object. The object will be created outside of the world and
        /// needs to be manually positioned at a location/inventory.
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static NWGameObject Deserialize(string serialized)
        {
            string sFunc = "Deserialize";

            NWNXCore.NWNX_PushArgumentString(NWNX_Object, sFunc, serialized);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Returns the dialog resref of the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDialogResref(NWGameObject obj)
        {
            string sFunc = "GetDialogResref";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Sets the dialog resref of the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dialog"></param>
        public static void SetDialogResref(NWGameObject obj, string dialog)
        {
            string sFunc = "SetDialogResref";

            NWNXCore.NWNX_PushArgumentString(NWNX_Object, sFunc, dialog);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Set obj's appearance. Will not update for PCs until they
        /// re-enter the area.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="app"></param>
        public static void SetAppearance(NWGameObject obj, int app)
        {
            string sFunc = "SetAppearance";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, app);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Get obj's appearance
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetAppearance(NWGameObject obj)
        {
            string sFunc = "GetAppearance";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Return true if obj has visual effect nVFX applied to it
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nVFX"></param>
        /// <returns></returns>
        public static bool GetHasVisualEffect(NWGameObject obj, int nVFX)
        {
            string sFunc = "GetHasVisualEffect";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, nVFX);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc) == 1;
        }

        /// <summary>
        /// Return true if an item of baseitem type can fit in object's inventory
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="baseitem"></param>
        /// <returns></returns>
        public static bool CheckFit(NWGameObject item, int baseitem)
        {
            string sFunc = "CheckFit";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, baseitem);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, item);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc) == 1;
        }

        /// <summary>
        /// Return damage immunity (in percent) against given damage type
        /// Use DAMAGE_TYPE_* constants for damageType
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="damageType"></param>
        /// <returns></returns>
        public static int GetDamageImmunity(NWGameObject obj, int damageType)
        {
            string sFunc = "GetDamageImmunity";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, damageType);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Add or move obj to area at pos
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="area"></param>
        /// <param name="pos"></param>
        public static void AddToArea(NWGameObject obj, NWGameObject area, Vector pos)
        {
            string sFunc = "AddToArea";

            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.Z);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.Y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.X);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, area);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }


    }
}
