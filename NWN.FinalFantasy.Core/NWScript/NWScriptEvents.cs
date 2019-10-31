// ReSharper disable once CheckNamespace
namespace NWN
{
    public class Events
    {
        public delegate void MainLoopDelegate(ulong frame);
        public static event MainLoopDelegate MainLoopTick;
        public static void OnMainLoopTick(ulong frame)
        {
            MainLoopTick?.Invoke(frame);
        }
    }
}