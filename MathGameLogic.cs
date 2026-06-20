using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunMath
{
    public class MathGameLogic
    {
        public List<string> GameHistory {get; set;} = new List<string>();

        public void ShowMenu()
        {
            Console.WriteLine("Welcome to the Fun Math Game!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Summation");
            Console.WriteLine("2. Subtraction");
            Console.WriteLine("3. Multiplication");
            Console.WriteLine("4. Division");
            Console.WriteLine("5. Random Mode");
            Console.WriteLine("6. Show Game History");
            Console.WriteLine("7. Change Difficulty");
            Console.WriteLine("8. Exit");
        }

        public int MathOperation(int num1, int num2, char operation)
        {
            switch (operation)
            {
                case '+':
                    GameHistory.Add($"{num1} + {num2} = {num1 + num2}");
                    return num1 + num2;
                case '-':
                    GameHistory.Add($"{num1} - {num2} = {num1 - num2}");
                    return num1 - num2;
                case '*':
                    GameHistory.Add($"{num1} * {num2} = {num1 * num2}");
                    return num1 * num2;
                case '/':
                    while(num1 < 0 || num1 >100)
                    {
                        try
                        {
                            Console.WriteLine("Please enter a dividend between 0 and 100:");
                            num1 = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer.");
                        }
                    }
                    GameHistory.Add($"{num1} / {num2} = {num1 / num2}");
                    return num1 / num2;
            }
            return 0;
        }
    }
}