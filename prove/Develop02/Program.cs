using System;
using System.Collections.Generic;
using System.IO;

// Define the Entry class to represent a journal entry
public class Entry
{
    public string Prompt { get; set; }
    public string Response { get; set; }
    public DateTime Date { get; set; }

    // Constructor to initialize the Entry object
    public Entry(string prompt, string response)
    {
        Prompt = prompt;
        Response = response;
        Date = DateTime.Now;
    }

    // Method to format the entry for display
    public override string ToString()
    {
        return $"{Date.ToString("yyyy-MM-dd")} - {Prompt}: {Response}";
    }
}

// Define the Journal class to manage entries
public class Journal
{
    private List<Entry> entries = new List<Entry>();

    // Method to add a new entry to the journal
    public void AddEntry(Entry entry)
    {
        entries.Add(entry);
    }

    // Method to display all entries in the journal
    public void DisplayEntries()
    {
        foreach (var entry in entries)
        {
            Console.WriteLine(entry);
        }
    }

    // Method to save the journal to a file
    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var entry in entries)
            {
                writer.WriteLine($"{entry.Prompt}|{entry.Response}|{entry.Date}");
            }
        }
    }

    // Method to load entries from a file
    public void LoadFromFile(string filename)
    {
        entries.Clear(); // Clear existing entries

        if (File.Exists(filename))
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3)
                {
                    string prompt = parts[0];
                    string response = parts[1];
                    DateTime date = DateTime.Parse(parts[2]);
                    Entry entry = new Entry(prompt, response);
                    entry.Date = date;
                    entries.Add(entry);
                }
            }
        }
    }
}

// Define the Program class to manage user interactions
public class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    WriteNewEntry(journal);
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    Console.Write("Enter filename to save: ");
                    string saveFileName = Console.ReadLine();
                    journal.SaveToFile(saveFileName);
                    break;
                case "4":
                    Console.Write("Enter filename to load: ");
                    string loadFileName = Console.ReadLine();
                    journal.LoadFromFile(loadFileName);
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    // Method to write a new journal entry
    static void WriteNewEntry(Journal journal)
    {
        string[] prompts = {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?"
        };

        Random rand = new Random();
        int index = rand.Next(prompts.Length);
        string prompt = prompts[index];

        Console.WriteLine("Prompt: " + prompt);
        Console.Write("Enter your response: ");
        string response = Console.ReadLine();

        Entry entry = new Entry(prompt, response);
        journal.AddEntry(entry);
    }
}
