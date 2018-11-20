using System;
using System.Text.RegularExpressions;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter unit price:");
                string price = Console.ReadLine();

                Regex reg = new Regex(@"^\d+(\.\d\d)?$");
                if (reg.IsMatch(price))
                //if (Regex.IsMatch(price, @"^(-)?\d+(\.\d\d)?$"))
                {
                    Console.WriteLine("Valid price!");
                }
                else
                {
                    Console.WriteLine("Invalid");
                }
            }



        }
    }
}
