namespace NWN.FinalFantasy.Chat.Command
{
    public abstract class LoopingAnimationCommand: IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            float duration = 9999.0f;

            if (args.Length > 0)
            {
                if (!float.TryParse(args[0], out duration))
                {
                    duration = 9999.0f;
                }
            }

            DoAction(user, duration);
        }

        public bool RequiresTarget => false;

        protected abstract void DoAction(NWGameObject user, float duration);

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }
    }
}
