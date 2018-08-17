using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TollFeeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime[] date = { new DateTime(2018, 08, 17, 6, 25, 0), new DateTime(2018, 08, 17, 7, 40, 0), new DateTime(2018, 08, 17, 15, 40, 0), new DateTime(2018, 08, 17, 15, 55, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Console.WriteLine("Today's total toll fee collected is " + tollFee);
            Console.ReadLine();
        }
    }
}
