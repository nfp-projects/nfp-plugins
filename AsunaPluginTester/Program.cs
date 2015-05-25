using System;
using System.Collections.Generic;
using System.Linq;
using AsunaPlugin;
using System.Text;
using System.Threading.Tasks;

namespace AsunaPluginTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var plugin = new AsunaPlugin.AsunaPlugin(null);
            Console.Write("Success");
            Console.ReadKey();
        }
    }
}
