using System;
using System.Collections.Generic;
using System.Threading;

public abstract class MindfulnessActivity
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public MindfulnessActivity(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void StartActivity(int duration)
    {
        Console.WriteLine($"Starting {Name} Activity");
        Console.WriteLine(Description);
        Console.WriteLine($"Duration: {duration} seconds");
        PrepareToStart();

        RunActivity(duration);

        EndActivity(duration);
    }

    public void PrepareToStart()
    {
        Console.WriteLine("Prepare to begin...");
        ShowSpinner(3);
    }

    public void ShowSpinner(int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    public void EndActivity(int duration)
    {
        Console.WriteLine($"Good job! You have completed the {Name} Activity.");
        Console.WriteLine($"Duration: {duration} seconds");
        ShowSpinner(3);
    }

    public abstract void RunActivity(int duration);
}

public class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity() 
        : base("Breathing", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    public override void RunActivity(int duration)
    {
        int secondsPerBreath = 4; // Adjust as necessary
        int breaths = duration / secondsPerBreath;
        for (int i = 0; i < breaths; i++)
        {
            Console.WriteLine("Breathe in...");
            ShowSpinner(2);
            Console.WriteLine("Breathe out...");
            ShowSpinner(2);
        }
    }
}

public class ReflectionActivity : MindfulnessActivity
{
    private List<string> Prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private List<string> Questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() 
        : base("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience.")
    {
    }

    public override void RunActivity(int duration)
    {
        Random random = new Random();
        string prompt = Prompts[random.Next(Prompts.Count)];
        Console.WriteLine(prompt);
        ShowSpinner(3);

        int timePerQuestion = duration / Questions.Count;
        foreach (string question in Questions)
        {
            Console.WriteLine(question);
            ShowSpinner(timePerQuestion);
        }
    }
}

public class ListingActivity : MindfulnessActivity
{
    private List<string> Prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity() 
        : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    public override void RunActivity(int duration)
    {
        Random random = new Random();
        string prompt = Prompts[random.Next(Prompts.Count)];
        Console.WriteLine(prompt);
        ShowSpinner(3);

        DateTime startTime = DateTime.Now;
        int count = 0;
        Console.WriteLine("Start listing items (press Enter after each item):");
        while ((DateTime.Now - startTime).TotalSeconds < duration)
        {
            Console.ReadLine();
            count++;
        }
        Console.WriteLine($"You listed {count} items.");
    }
}

public class Program
{
    public static void Main()
    {
        Dictionary<string, MindfulnessActivity> activities = new Dictionary<string, MindfulnessActivity>
        {
            { "1", new BreathingActivity() },
            { "2", new ReflectionActivity() },
            { "3", new ListingActivity() }
        };

        while (true)
        {
            Console.WriteLine("\nChoose an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            string choice = Console.ReadLine();

            if (choice == "4")
            {
                break;
            }
            else if (activities.ContainsKey(choice))
            {
                Console.Write("Enter duration in seconds: ");
                if (int.TryParse(Console.ReadLine(), out int duration))
                {
                    activities[choice].StartActivity(duration);
                }
                else
                {
                    Console.WriteLine("Invalid duration. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }
}
