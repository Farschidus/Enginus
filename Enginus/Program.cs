using System;

namespace Enginus
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var enginus = new Enginus();
            enginus.Run();
        }
    }
}

//using var enginus = new Enginus();
//enginus.Run();

