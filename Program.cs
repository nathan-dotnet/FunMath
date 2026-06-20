/*
    1.You need to create a game that consists of asking the player what's the result of a math question (i.e. 9 x 9 = ?),
    collecting the input and adding a point in case of a correct answer.
    2.A game needs to have at least 5 questions.
    3.The divisions should result on INTEGERS ONLY and dividends should go from 0 to 100. 
    Example: Your app shouldn't present the division 7/2 to the user, since it doesn't result in an integer.
    4.Users should be presented with a menu to choose an operation
    5.You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.
    6.You don't need to record results on a database. Once the program is closed the results will be deleted.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathGameLogic = FunMath.MathGameLogic;
using System.Diagnostics;

MathGameLogic mathGame = new MathGameLogic();
Random random = new Random();

int num1;
int num2;
int userMenuChoice;
int score = 0;
bool gameOver = false;


Difficulty difficulty = Difficulty.Easy;

while (!gameOver)
{
    userMenuChoice = GetUserMenuSelection(mathGame);

    num1 = random.Next(1, 101);
    num2 = random.Next(1, 101);

    switch (userMenuChoice)
    {
        case 1:
            score = await PerformOperation(mathGame, num1, num2, '+', score, difficulty);
            break;
        case 2:
            score = await PerformOperation(mathGame, num1, num2, '-', score, difficulty);
            break;
        case 3:
            score = await PerformOperation(mathGame, num1, num2, '*', score, difficulty);
            break;
        case 4:
            while (num1 % num2 != 0)
            {
                num1 = random.Next(1, 101);
                num2 = random.Next(1, 101);
            }
            score += await PerformOperation(mathGame, num1, num2, '/', score, difficulty);
            break;
        case 5:
            int numberOfQuestions = 99;
            Console.WriteLine("How many questions would you like to answer?");
            while (!int.TryParse(Console.ReadLine(), out numberOfQuestions))
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
            while(numberOfQuestions > 0)
            {
                int operation = random.Next(1, 5);

                if(operation == 1)
                {
                    num1 = random.Next(1, 101);
                    num2 = random.Next(1, 101);
                    score = await PerformOperation(mathGame, num1, num2, '+', score, difficulty);
                }
                else if(operation == 2)
                {
                    num1 = random.Next(1, 101);
                    num2 = random.Next(1, 101);
                    score = await PerformOperation(mathGame, num1, num2, '-', score, difficulty);
                }
                else if(operation == 3)
                {
                    num1 = random.Next(1, 101);
                    num2 = random.Next(1, 101);
                    score = await PerformOperation(mathGame, num1, num2, '*', score, difficulty);
                }
                else
                {
                    
                    num1 = random.Next(1, 101);
                    num2 = random.Next(1, 101);
                    while (num1 % num2 != 0)
                    {
                        num1 = random.Next(1, 101);
                        num2 = random.Next(1, 101);
                    }
                    score += await PerformOperation(mathGame, num1, num2, '/', score, difficulty);
                }
                numberOfQuestions--;
            }
            break;
        case 6:
            Console.WriteLine("Game History:");
            foreach (string entry in mathGame.GameHistory)
            {
                Console.WriteLine($"{entry}");
            }
            break;
        case 7:
            difficulty = ChangeDifficulty();
            Difficulty newDifficulty = (Difficulty)difficulty;
            Enum.IsDefined(typeof(Difficulty), newDifficulty);
            Console.WriteLine($"Difficulty changed to {newDifficulty}.");
            break;
        case 8:
            Console.WriteLine($"Thanks for playing! Your final score was {score} points.");
            gameOver = true;
            break;
    }
}


static Difficulty ChangeDifficulty()
{
    int userSelection = 0;

    Console.WriteLine("Please select a difficulty level:");
    Console.WriteLine("1. Easy");
    Console.WriteLine("2. Medium");
    Console.WriteLine("3. Hard");

    while (!int.TryParse (Console.ReadLine(), out userSelection) || userSelection < 1 || userSelection > 3)
    {
        Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
    }

    switch(userSelection)
    {
        case 1:
            return Difficulty.Easy;
        case 2:
            return Difficulty.Medium;
        case 3:
            return Difficulty.Hard;
        default:
            Console.WriteLine("Invalid selection. Defaulting to Easy difficulty.");
            return Difficulty.Easy;
    };
}   

static void DisplayMathGameQuestion(int num1, int num2, char operation)
{
    Console.WriteLine($"What is {num1} {operation} {num2}?");
}

static int GetUserMenuSelection(MathGameLogic mathGame)
{
    int selection = -1;
    mathGame.ShowMenu();
    while(selection < 1 || selection > 8)
    {
        while(!int.TryParse(Console.ReadLine(), out selection))
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 8.");
        }

        if(!(selection >= 1 && selection <= 8))
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 8.");
        }
    }

    return selection;
}

static async Task<int?> GetUserResponse(Difficulty difficulty)
{
    int response = 0;
    int timeout = (int)difficulty;

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task<string?> GetUserInputTask = Task.Run(() => Console.ReadLine());

    try
    {
        string? result = await Task.WhenAny(GetUserInputTask, Task.Delay(timeout * 1000)) == GetUserInputTask
            ? GetUserInputTask.Result
            : null;

        stopwatch.Stop();

        if (result != null && int.TryParse(result, out response))
        {
            Console.WriteLine($"You answered in {stopwatch.Elapsed.ToString(@"mm\:ss\.fff")} seconds.");
            return response;
        }
        else
        {
            throw new OperationCanceledException();
        }
    }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Time's up! You took too long to answer.");
            return null;
        }
}

static int ValidateResult(int result, int? userResponse, int score)
{
    if (result == userResponse)
    {
        Console.WriteLine("Correct! You earned 5 point.");
        score += 5;
    }
    else
    {
        Console.WriteLine("Incorrect! Try again.");
        Console.WriteLine($"Incorrect! The correct answer was {result}.");
    }
    return score;
}

static async Task<int> PerformOperation(MathGameLogic mathGame, int num1, int num2, char operation, int score, Difficulty difficulty)
{
    int result;
    int? userResponse;
    DisplayMathGameQuestion(num1, num2, operation);
    result = mathGame.MathOperation(num1, num2, operation);
    userResponse = await GetUserResponse(difficulty);
    score += ValidateResult(result, userResponse, score);
    return score;
}

public enum Difficulty
{
    Easy = 45,
    Medium = 30,
    Hard = 15
}