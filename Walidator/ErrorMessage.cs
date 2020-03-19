using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walidator
{
    /* Klasa do tworzenia wiadomosci jak cos bedzie nie za dobrze */
    public static class ErrorMessage
    {
        public static string errorMsg(int line, int cnum, string reason)
        {
            return "Failed in line:" + line + " char:" + cnum + "! " + reason
                + "\n" + getLine(line);
        }

        public static string errorMsg(int line, string reason)
        {
            return "Failed in line:" + line + " ! " + reason
                + "\n" + getLine(line);
        }

        public static string errorMsg(Token t, string expected, int i)
        {
            return "Failed in line:" + t.GetLine() + " on char:"
                    + t.GetCnumber() + "\nGot:'" + t.ToString()
                    + "' Expected: '" + expected + "' \n" + getLine(i);
        }

        private static string getLine(int lineNo)
        {
            string[] lines = scanner.jsonf.Replace("\r", "").Split('\n');
            string value = "";
            value = "Error in line:\n";
            value += lines.Length >= lineNo ? lines[lineNo - 1] : null;
            return value;
        }
    }
}