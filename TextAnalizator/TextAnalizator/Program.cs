using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TextAnalizator
{
    public class TextInfo
    {
        public string filename = "";
        public string longestWord = "";
        public Dictionary<string, int> counts = new Dictionary<string, int>();
        public Dictionary<char, int> letters = new Dictionary<char, int>();
        public Dictionary<string, int> words = new Dictionary<string, int>();
    }
    class Program
    {
        static void Main(string[] args)
        {
            TextInfo info = new TextInfo();
            info.counts.Add("fileSize", 0);
            info.counts.Add("lettersCount", 0);
            info.counts.Add("wordsCount", 0);
            info.counts.Add("linesCount", 1);
            info.counts.Add("digitsCount", 0);
            info.counts.Add("numbersCount", 0);
            info.counts.Add("wordsWithHyphen", 0);
            info.counts.Add("punctuations", 0);

            Console.WriteLine("Укажите путь к файлу:");
            string path = Console.ReadLine();
            info.filename = Path.GetFileName(path);
            Console.WriteLine("Укажите путь для JSON файла:");
            string outputPath = Console.ReadLine();
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string str = sr.ReadLine();
                        info.counts["linesCount"]++;
                        str = HttpUtility.HtmlDecode(str);
                        info.counts["fileSize"] += str.Length;
                        string[] mass = str.Split(' ');
                        foreach (string word in mass)
                        {
                            string currentWord = "";
                            string currentNumber = "";
                            foreach (char sym in word)
                            {
                                // Буквы и слова
                                if (char.IsLetter(sym) || sym == '’' || sym == '-')
                                {
                                    currentWord += sym;
                                    if (char.IsLetter(sym))
                                    {
                                        info.counts["lettersCount"]++;
                                        //Добавляем букву в словарь
                                        if (!info.letters.ContainsKey(sym))
                                        {
                                            info.letters.Add(sym, 1);
                                        }
                                        else
                                        {
                                            info.letters[sym]++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (currentWord.Length > 0)
                                    {
                                        if (currentWord.Contains('-'))
                                        {
                                            info.counts["wordsWithHyphen"]++;
                                            if (currentWord.Length > info.longestWord.Length)
                                            {
                                                info.longestWord = currentWord;
                                            }
                                        }
                                        info.counts["wordsCount"]++;
                                        if (!info.words.ContainsKey(currentWord))
                                        {
                                            info.words.Add(currentWord, 1);
                                        }
                                        else
                                        {
                                            info.words[currentWord]++;
                                        }
                                    }
                                    currentWord = "";
                                }
                                // Цифры и числа
                                if (char.IsDigit(sym) || sym == ',' || sym == '.')
                                {
                                    currentNumber += sym;
                                    if (char.IsDigit(sym))
                                        info.counts["digitsCount"]++;
                                }
                                else
                                {
                                    currentNumber = currentNumber.Replace(",", string.Empty);
                                    currentNumber = currentNumber.Replace(".", string.Empty);
                                    if (currentNumber.Length > 1)
                                    {
                                        info.counts["numbersCount"]++;
                                    }
                                    currentNumber = "";
                                }
                                // Знаки препинания
                                if (char.IsPunctuation(sym))
                                {
                                    info.counts["punctuations"]++;
                                }
                            }

                            if (currentWord.Length > 0)
                            {
                                if (currentWord.Contains('-'))
                                {
                                    info.counts["wordsWithHyphen"]++;
                                    if (currentWord.Length > info.longestWord.Length)
                                    {
                                        info.longestWord = currentWord;
                                    }
                                }
                                info.counts["wordsCount"]++;
                                if (!info.words.ContainsKey(currentWord))
                                {
                                    info.words.Add(currentWord, 1);
                                }
                                else
                                {
                                    info.words[currentWord]++;
                                }
                            }
                            currentNumber = currentNumber.Replace(",", string.Empty);
                            currentNumber = currentNumber.Replace(".", string.Empty);
                            if (currentNumber.Length > 1)
                            {
                                info.counts["numbersCount"]++;
                            }
                        }
                    }
                }
                // Сортировка
                info.counts = (from c in info.counts orderby c.Value descending select c).ToDictionary(p => p.Key, p => p.Value);
                info.letters = (from l in info.letters orderby l.Value descending select l).ToDictionary(p => p.Key, p => p.Value);
                info.words = (from w in info.words orderby w.Value descending select w).ToDictionary(p => p.Key, p => p.Value);

                string json = JsonConvert.SerializeObject(info, Formatting.Indented);
                // Запись в файл
                using (StreamWriter sw = new StreamWriter(Path.Combine(outputPath,Path.GetFileNameWithoutExtension(path)) + ".json"))
                {
                    sw.Write(json);
                }
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
