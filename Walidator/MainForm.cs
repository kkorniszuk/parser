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

        private void Waliduj()
        {
            List<Token> TokensList = new List<Token>();
            scanner l = new scanner();
            Parser p = new Parser();
            bool failed = false;
            try
            {
                failed = l.lexer(rtbInput.Text, ref TokensList);
                p.parse(TokensList);
            }
            catch (JSONException pje)
            {
                failed = true;
                //panel4.BackColor = Color.Red;
                rtbResult.Text = pje.Message + "\n" + "Validation failed.";
            }

            if (!failed)
            {
                rtbResult.Text = "Json is valid.";
                //panel4.BackColor = Color.Green;
            }
            failed = false;
#if DEBUG
            foreach (Token t in TokensList)
            {
                Console.Write(t);
            }
#endif
            //////////\\\
            //Parser Walid = new Parser(tbInput.Text.ToString());
            //Walid.stateBeginValue();
            //StringBuilder AllError = new StringBuilder();
            //foreach (var item in Walid.error)
            //{
            //    AllError.Append(String.Format("Error(line {0}): {1}\n",item.getKodSymbolu(),item.getOpis()));
            //}
            //MessageBox.Show(String.Format("Liczba błędów: {0} \n{1} ",Walid.error.Count,AllError.ToString()));//; Walid.stateBeginValue().ToString());
        }


    }
}
