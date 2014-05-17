using System;

namespace Serilog.Generator.Model
{
    static class Machine
    {
        static readonly Random _random = new Random();

        static readonly string[] Machines = { "XMPWEB-01", "XMPWEB-02", "XMPWEB-03" };

        public static string Choose()
        {
            return Machines[_random.Next(Machines.Length)];
        }
    }
}
