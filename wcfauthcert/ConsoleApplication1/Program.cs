using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.Service1Client();
            var value = client.GetData(77);
            Console.WriteLine(value);
            Console.ReadLine();
        }
    }
}
