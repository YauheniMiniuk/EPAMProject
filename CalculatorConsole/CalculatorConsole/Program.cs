using System;
using System.Numerics;

namespace CalculatorConsole
{
    class Program
    {
        static string Add(string num1, string num2)
        {
            try
            {
                return (BigInteger.Parse(num1) + BigInteger.Parse(num2)).ToString();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        static string Remove(string num1, string num2)
        {
            try
            {
                return (BigInteger.Parse(num1) - BigInteger.Parse(num2)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        static string Multiply(string num1, string num2)
        {
            try
            {
                return (BigInteger.Parse(num1) * BigInteger.Parse(num2)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        static string Divide(string num1, string num2)
        {
            try
            {
                return (BigInteger.Parse(num1) / BigInteger.Parse(num2)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        static void Main(string[] args)
        {
            string answer = null;
            string number1, number2, result;
            while (answer != "5")
            {
                Console.WriteLine("Actions:\n" +
                    "1 - Add\n" +
                    "2 - Remove\n" +
                    "3 - Multiply\n" +
                    "4 - Divide\n" +
                    "5 - Exit");
                Console.WriteLine("Select an Action:");
                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        Console.WriteLine("You selected Add.");
                        Console.WriteLine("Write first number:");
                        number1 = Console.ReadLine();
                        Console.WriteLine("Write second number:");
                        number2 = Console.ReadLine();
                        result = Add(number1, number2);
                        Console.WriteLine("result: " + result);
                        break;
                    case "2":
                        Console.WriteLine("You selected Remove.");
                        Console.WriteLine("Write first number:");
                        number1 = Console.ReadLine();
                        Console.WriteLine("Write second number:");
                        number2 = Console.ReadLine();
                        result = Remove(number1, number2);
                        Console.WriteLine("result: " + result);

                        break;
                    case "3":
                        Console.WriteLine("You selected Multiply.");
                        Console.WriteLine("Write first number:");
                        number1 = Console.ReadLine();
                        Console.WriteLine("Write second number:");
                        number2 = Console.ReadLine();
                        result = Multiply(number1, number2);
                        Console.WriteLine("result: " + result);

                        break;
                    case "4":
                        Console.WriteLine("You selected Divide.");
                        Console.WriteLine("Write first number:");
                        number1 = Console.ReadLine();
                        Console.WriteLine("Write second number:");
                        number2 = Console.ReadLine();
                        result = Divide(number1, number2);
                        Console.WriteLine("result: " + result);

                        break;
                    case "5":
                        break;
                    default:
                        Console.WriteLine("Select right action!");
                        break;
                }
            }
        }
    }
}
