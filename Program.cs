using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace CybersecurityChatbot
{
    class Program
    {
        static string userName;
        static string favoriteTopic;
        static ResponseManager responseManager;

        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.ForegroundColor = ConsoleColor.Cyan;

            
            AudioPlayer.PlaySound("C:\\Users\\RC_Student_lab\\source\\repos\\CyberSecurityAwarenessBotPart2\\bin\\Debug\\net6.0\\welcome_message.wav");

            
            DisplayAsciiLogo();

            
            Console.Write("Enter your name: ");
            userName = Console.ReadLine();
            Console.WriteLine($"\nHi {userName}, I’m here to help with online safety!");

            responseManager = new ResponseManager();
            StartChat();
        }

        static void DisplayAsciiLogo()
        {
            Console.WriteLine(@"
 ██████╗██╗   ██╗██████╗ ███████╗███████╗
██╔════╝██║   ██║██╔══██╗██╔════╝██╔════╝
██║     ██║   ██║██████╔╝█████╗  ███████╗
██║     ██║   ██║██╔═══╝ ██╔══╝  ╚════██║
╚██████╗╚██████╔╝██║     ███████╗███████║
 ╚═════╝ ╚═════╝ ╚═╝     ╚══════╝╚══════╝
");
            Console.WriteLine("Welcome to the Cybersecurity Awareness Bot!\n");
        }

        static void StartChat()
        {
            while (true)
            {
                Console.Write("\nAsk a cybersecurity question or type 'exit': ");
                string input = Console.ReadLine().ToLower();

                if (input == "exit") break;

                responseManager.CheckSentiment(input);

                if (input.Contains("favourite topic") || input.Contains("favorite topic"))
                {
                    if (!string.IsNullOrEmpty(favoriteTopic))
                        Console.WriteLine($"You mentioned your favorite topic is {favoriteTopic}.");
                    else
                        Console.WriteLine("You haven’t told me your favorite topic yet.");
                    continue;
                }

                var responder = responseManager.GetResponder(input);
                if (responder != null)
                {
                    responder.Invoke();
                    favoriteTopic = responseManager.GetLastTopic();
                }
                else
                {
                    Console.WriteLine("I'm not sure I understand. Can you try rephrasing?");
                }
            }

            Console.WriteLine($"\nGoodbye, {userName}! Stay safe online.");
        }
    }

    class ResponseManager
    {
        public delegate void ResponseDelegate();
        private Dictionary<string, List<string>> keywordResponses;
        private List<string> sentiments;
        private string lastKeyword;
        private Random rand;

        public ResponseManager()
        {
            rand = new Random();
            lastKeyword = "";
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            keywordResponses = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "Use strong, unique passwords for each account.",
                    "Avoid personal info like birthdays in your passwords.",
                    "Use a password manager to store your credentials securely."
                }},
                { "phishing", new List<string> {
                    "Be cautious of emails asking for your login info.",
                    "Never click on suspicious links in messages.",
                    "Verify sender email addresses carefully."
                }},
                { "privacy", new List<string> {
                    "Limit what you post on social media.",
                    "Use privacy settings on your apps.",
                    "Be mindful of what permissions you grant to apps."
                }}
            };

            sentiments = new List<string> { "worried", "scared", "confused", "frustrated", "curious" };
        }

        public void CheckSentiment(string input)
        {
            foreach (var word in sentiments)
            {
                if (input.Contains(word))
                {
                    Console.WriteLine("It’s okay to feel that way. Cybersecurity can be tricky, but I’ve got your back!");
                    break;
                }
            }
        }

        public ResponseDelegate GetResponder(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastKeyword = keyword;
                    return () =>
                    {
                        var responses = keywordResponses[keyword];
                        string selected = responses[rand.Next(responses.Count)];
                        Console.WriteLine(selected);
                    };
                }
            }
            return null;
        }

        public string GetLastTopic()
        {
            return lastKeyword;
        }
    }

    static class AudioPlayer
    {
        public static void PlaySound(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    SoundPlayer player = new SoundPlayer(filePath);
                    player.PlaySync();
                }
                else
                {
                    Console.WriteLine("Audio file not found: " + Path.GetFullPath(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing sound: " + ex.Message);
            }
        }
    }
}
