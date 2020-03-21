using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Walidator
{
    public partial class MainForm : Form
    {

        string fileContent = string.Empty;
        string filePath = string.Empty;
        string Result = string.Empty;

        //bool a()
        //{
        //    return true;
        //}
        //bool b()
        //{
        //    return true;
        //}
        //bool c()
        //{
        //    return true;
        //}

        public MainForm()
        {
            //if (A() && B() && C())
            //{
            //    int a = 1;
            //}
            InitializeComponent();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            Save(rtbInput.Text.ToString());
        }

        private void Save(string TextFile)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "bnf |*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string txt = TextFile;
                    File.WriteAllText(sfd.FileName, txt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania" + ex);
            }

        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            try
            {

                string path = Directory.GetCurrentDirectory();
                OpenFileDialog of = new OpenFileDialog();
                of.FileName = "Open Text File";
                of.Filter = "bnf (*.txt)|*.txt";
                of.InitialDirectory = path;// Directory.GetCurrentDirectory();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = of.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = of.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        rtbInput.Text = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas otwierania"+ex);
            }
           
        }



        private void btClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            rtbInput.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Waliduj();
        
        }

        /// <summary>
        /// Funkcja sprawdzająca zwraca wartości int dla kazdego tokena(w postaci string) - funkcja tymczsowa
        /// 
        /// </summary>
        private void Waliduj_1()
        {
            
            List<Token> TokensList = new List<Token>();
            scanner l = new scanner();
            //Parser p = new Parser();
            bool failed = false;
            StringBuilder Tmp = new StringBuilder();
            failed = l.lexer(rtbInput.Text, ref TokensList, ref Result);
            foreach (var item in TokensList)
            {
                Tmp.Append(item.GetToken().ToString());
                Tmp.Append("||");
                if (item.GetToken() == Token.NewLine)
                {
                    Tmp.Append('\n');
                }
            }

            rtbResult.Text = Tmp.ToString();
            Tmp.Length = 0;

        }
            private void Waliduj()
        {
            List<Token> TokensList = new List<Token>();
            scanner l = new scanner();
            
            bool failed = false;
            StringBuilder Tmp = new StringBuilder();
            failed = l.lexer(rtbInput.Text, ref TokensList, ref Result);

          
            //p.start();
            
            Parser p = new Parser(TokensList);
            Result=Result+p.start();
            rtbResult.Text = Result;//+ Tmp.ToString();
            //foreach (var item in TokensList)
            //{
            //    Tmp.Append(item.GetToken().ToString());
            //    Tmp.Append("||");
            //    if (item.GetToken() == Token.NewLine)
            //    {
            //        Tmp.Append('\n');
            //    }
            //}
            //p.start();
            // Tmp.Length = 0;
            

            if (!failed)
            {
                rtbResult.Text = "Json is valid.";
            }
            failed = false;
        }
    }
}
