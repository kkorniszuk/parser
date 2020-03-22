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

        public MainForm()
        {
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
                MessageBox.Show("Błąd podczas otwierania" + ex);
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


        private void Waliduj()
        {
            try
            {
                List<Error> errList = new List<Error>();
                List<Token> TokensList = new List<Token>();
                scanner l = new scanner();

                bool failed = false;
                StringBuilder Tmp = new StringBuilder();
                errList = l.lexer(rtbInput.Text, ref TokensList);
                Tmp.AppendFormat("+Scanner: \n\t-Error({0})\n", errList.Count.ToString());

                if (errList.Count<1)
                {
                    Parser p = new Parser(TokensList);
                    Tmp.Append(p.start());
                }
                else
                {
                    Tmp.Append(ListToString(errList));
                }
                
                rtbResult.Text = Tmp.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string ListToString(List<Error> error)
        {
            string retval = "";
            StringBuilder errList = new StringBuilder();
            errList.AppendFormat("+Scaner: \n\t-Error({0}\n)", error.Count.ToString());

            foreach (var er in error)
            {
                errList.AppendFormat("\t-line{0} error:{1}", er.GetLine(), er.GetDescription());
            }
            return retval;
        }

    }
}
