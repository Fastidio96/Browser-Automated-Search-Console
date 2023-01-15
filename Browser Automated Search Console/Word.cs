using System;
using System.IO;

namespace Browser_Automated_Search_Console
{
    public class Word
    {
        private bool _isRandom { get; set; }
        private string _result { get; set; }

        public Word(bool isRandom)
        {
            _isRandom = isRandom;
            if (_isRandom)
            {
                Generate();
            }
            else
            {
                Select();
            }
        }
        public override string ToString()
        {
            return _result;
        }

        /// <summary>
        /// Generate a random string of [5-10] characters to use as a search query
        /// </summary>
        private void Generate()
        {
            Random randNum = new Random();
            string charset = "abcdefghijklmnopqrstuvwxyz";
            string wordQuery = string.Empty; // Start with an empty string
            Random randLength = new Random();
            int length = randLength.Next(5, 15); // Set a random string length (from 5 to 15 chars)

            for (int i = 1; i <= length; i++)
            {
                int index = randNum.Next(charset.Length);
                wordQuery += charset[index];

            }

            _result = wordQuery;
        }

        /// <summary>
        /// Select a random query from a list to use as a search query.
        /// </summary>
        private void Select()
        {
            try
            {
                //string filePath = Assembly.GetEntryAssembly().Location + "\\wordlist.txt";
                string filePath = @"C:\wordlist.txt";
                string[] wordList = File.ReadAllLines(filePath);

                Random rand = new Random();
                int randNum = rand.Next(wordList.Length);
                
                _result = wordList[randNum];
            }
            catch (Exception ex)
            {
                Messages.Error("An error was encountered while opening the file.", ex);
            }
        }

    }
}
