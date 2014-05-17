using System;
using System.Threading;

namespace Serilog.Generator.Model
{
    static class Fake
    {
        static int _next;
        static readonly Random _random = new Random();

        public static int Int() { return Interlocked.Increment(ref _next); }

        public static string String(string tag) { return tag + "__" + Int(); }

        static readonly Tuple<string, string>[] _names =
        {
            Tuple.Create("Alice", "Andrews"),
            Tuple.Create("Bob", "Bright"),
            Tuple.Create("Charles", "Cook"),
            Tuple.Create("Denise", "de Maro"),
            Tuple.Create("Elanore", "Errols"),
            Tuple.Create("Fay", "Fung"),
            Tuple.Create("Garry", "Garcia"),
            Tuple.Create("Hu", "Hwo"),
            Tuple.Create("Isolde", "Innes"),
            Tuple.Create("John", "Jones"),
            Tuple.Create("Kim", "Kelly-Scott"),
            Tuple.Create("Luke", "Lawson"),
            Tuple.Create("Mahesh", "Muktar"),
            Tuple.Create("Nita", "Ng"),
            Tuple.Create("Ostwald", "Orwell"),
            Tuple.Create("Paul", "Plum"),
            Tuple.Create("Quentin", "Quincy-Lee"),
        };

        public static string Name()
        {
            var first = _names[_random.Next(_names.Length)].Item1;
            var second = _names[_random.Next(_names.Length)].Item2;
            return first + " " + second;
        }
    }
}