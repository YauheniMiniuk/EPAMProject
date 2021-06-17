using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace EPAMProject
{
    public partial class Form1 : Form
    {
        public string filename, filenameNoType, longestWord;
        public int fileSize, lettersCount, wordsCount, linesCount, digitsCount, numbersCount, wordsWithHyphen, punctuations;
        Dictionary<char, int> letters = new Dictionary<char, int>();
        Dictionary<string, int> words = new Dictionary<string, int>();
        char[] separators = new char[] { ',', '.', '?', ';', ':', '"', '!', '\'' };
        public Form1()
        {
            InitializeComponent();
        }
        private void openFileBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog.FileName;
                    filename = openFileDialog.SafeFileName;
                    filenameNoType = Path.GetFileNameWithoutExtension(textBox1.Text);
                }
            }

        }
        private void selectJSONPathBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog browserDialog = new FolderBrowserDialog())
            {
                if (browserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = browserDialog.SelectedPath;
                }
            }
        }
        string fileStr;
        private void calculateBtn_Click(object sender, EventArgs e)
        {
            using (StreamReader sr = new StreamReader(textBox1.Text))
            {
                fileSize = 0;
                lettersCount = 0;
                letters.Clear();
                wordsCount = 0;
                words.Clear();
                linesCount = 1;
                digitsCount = 0;
                numbersCount = 0;
                longestWord = "";
                wordsWithHyphen = 0;
                punctuations = 0;

                fileStr = sr.ReadToEnd();
                fileSize = fileStr.Length;
                foreach (char sym in fileStr)
                {
                    if (sym >= 'A' && sym <= 'z')
                    {
                        lettersCount++;
                        if (letters.ContainsKey(sym))
                        {
                            letters[sym]++;
                        }
                        else
                        {
                            letters.Add(sym, 1);
                        }
                    }
                    if (sym >= '0' && sym <= '9')
                    {
                        digitsCount++;
                    }
                    foreach (char punc in separators)
                    {
                        if (sym == punc)
                        {
                            punctuations++;
                        }
                    }
                    if (sym == '\n')
                    {
                        linesCount++;
                    }
                }
            }
            // Remove punctuation marks
            fileStr = Regex.Replace(fileStr, @"\s-\s", string.Empty);
            fileStr = Regex.Replace(fileStr, @"\r\n", " ");
            fileStr = Regex.Replace(fileStr, @"\d-\d", " ");
            fileStr = Regex.Replace(fileStr, "[@,./?!_\\'\"—;:]", string.Empty);
            string[] textArr = fileStr.Split(' ');
            wordsCount = textArr.Length;
            foreach (string str in textArr)
            {
                if ((str.Trim(separators)).Length > 0 && !Regex.IsMatch(str, @"^\d+$"))
                {
                    if (words.ContainsKey(str))
                    {
                        words[str]++;
                    }
                    else
                    {
                        words.Add(str, 1);
                    }
                }
                longestWord = str.Length > longestWord.Length ? str : longestWord;
                if (str.Contains("-"))
                {
                    wordsWithHyphen++;
                }
                if (int.TryParse(str, out _))
                {
                    numbersCount++;
                }
            }
            letters = (from l in letters orderby l.Value descending select l).ToDictionary(p => p.Key, p => p.Value);
            words = (from w in words orderby w.Value descending select w).ToDictionary(p => p.Key, p => p.Value);

            // Create anon object
            var jsonObj = new
            {
                filename = filename,
                fileSize = fileSize,
                lettersCount = lettersCount,
                letters = letters,
                wordsCount = wordsCount,
                words = words,
                linesCount = linesCount,
                digitsCount = digitsCount,
                numbersCount = numbersCount,
                longestWord = longestWord,
                wordsWithHyphen = wordsWithHyphen,
                punctuations = punctuations
            };
            
            var json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            // Create .json file
            string path = textBox2.Text + "/" + filenameNoType + ".json";
            using (StreamWriter sw = new StreamWriter(path))
            {
                if (File.Exists(path))
                {
                    sw.Write(json);
                }
                else
                {
                    File.Create(path);
                    sw.Write(json);
                }
            }
            MessageBox.Show("OK");
        }
    }
}
