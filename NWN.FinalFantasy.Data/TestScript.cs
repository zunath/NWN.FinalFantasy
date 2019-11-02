using System;
using System.Diagnostics;

namespace NWN.FinalFantasy.Data
{
    public class TestScript
    {
        public static void Main()
        {
            Console.WriteLine("Running from data project");

            var sw = new Stopwatch();

            sw.Start();
            var entity = new TestEntity("my new entity");
            DB.Set(entity);
            entity.Name = "changed name";

            var result = DB.Get<TestEntity>(entity.ID);

            Console.WriteLine("name = " + result.Name);
            sw.Stop();

            Console.WriteLine("sw = " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
