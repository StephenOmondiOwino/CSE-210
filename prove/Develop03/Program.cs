using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMemorizationProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list of scriptures
            List<Scripture> scriptures = new List<Scripture>
            {
                new Scripture("John 3:16", "For God so loved the world that he gave his only Son, that whoever believes in him should not perish but have eternal life.")
                // Add more scriptures here if desired
            };

            // Select a random scripture from the list
            Random random = new Random();
            Scripture selectedScripture = scriptures[random.Next(scriptures.Count)];

            // Display the complete scripture
            selectedScripture.Display();

            // Loop until all words are hidden
            while (!selectedScripture.AllWordsHidden)
            {
                Console.WriteLine("\nPress Enter to continue or type 'quit' to exit:");
                string input = Console.ReadLine();
                if (input.ToLower() == "quit")
                    break;

                // Hide a few random words
                selectedScripture.HideRandomWords();
                Console.Clear();
                selectedScripture.Display();
            }

            Console.WriteLine("\nAll words in the scripture are now hidden. Press any key to exit.");
            Console.ReadKey();
        }
    }

    class Scripture
    {
        private string reference;
        private List<string> words;
        private List<bool> hiddenWords;

        public bool AllWordsHidden => hiddenWords.All(h => h);

        public Scripture(string reference, string text)
        {
            this.reference = reference;
            words = text.Split(' ').ToList();
            hiddenWords = Enumerable.Repeat(false, words.Count).ToList();
        }

        public void Display()
        {
            Console.WriteLine($"Scripture: {reference}\n");

            for (int i = 0; i < words.Count; i++)
            {
                if (hiddenWords[i])
                    Console.Write("____ ");
                else
                    Console.Write(words[i] + " ");
            }

            Console.WriteLine();
        }

        public void HideRandomWords()
        {
            Random random = new Random();
            int wordsToHide = random.Next(1, 4); // Hide 1 to 3 words at a time

            for (int i = 0; i < wordsToHide; i++)
            {
                int index;
                do
                {
                    index = random.Next(words.Count);
                } while (hiddenWords[index]);

                hiddenWords[index] = true;
            }
        }
    }
}
