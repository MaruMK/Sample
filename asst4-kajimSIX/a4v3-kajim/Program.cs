using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4v3kajim
{ 
    class Program
    {
        const int LSIZE = 5;         //actual number of infix strings in the data array
        const int NOPNDS = 10;       //number of operand symbols in the operand array

        static int IDX;              //index used to implement conversion stub



        static void Main()
        {
            /*************************************************************************
                                      KEY DECLARATIONS            
            *************************************************************************/
            string[] infix = new string[LSIZE] { "C$A$E",    //array of infix strings
                             "(A+B)*(C-D)",
                             "A$B*C-D+E/F/(G+H)",
                             "((A+B)*C-(D-E))$(F+G)",
                             "A-B/(C*D$E)"  };

            string[] postfix = new string[LSIZE] { "CAE$$",  //array of postfix strings
                             "AB+CD-*",
                             "AB$C*D-EF/GH+/+",
                             "AB+C*DE--FG+$",
                             "ABCDE$*/-"  };
            
            char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operands symbols
            double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };           //operand values

            List<string> WKinfix = infix.ToList<string>();      //array of infix strings to work with
            List<string> WKpostfix = postfix.ToList<string>();  //array of postfix strings to work with

            List<char> opndStk = opnd.ToList<char>();               //stack of operand symbols
            List<double> opndvalStk = opndval.ToList<double>();     //stack of opreand values

            string ifx;                                     //infix string to be converted in conversion stub
            string pfx = "";                                //converted postfix string result of conversion stub

            /*************************************************************************
                                DISPLAY INFIX AND POSTFIX TEST EXPRESSIONS            
            **************************************************************************/
            Console.WriteLine("INFIX AND POSTFIX TEST EXPRESSIONS".PadLeft(60));   // master title
            Console.WriteLine("\n\n");
            Console.Write("INFIX".PadLeft(30));
            Console.WriteLine("POSTFIX".PadLeft(30));                              //column headings
            OutLine(65, '=');                       //print a line of '=' for pretties
            for (int i = 0; i < LSIZE; i++)
                Console.WriteLine(infix[i].PadLeft(30) + postfix[i].PadLeft(30));
            OutLine(65, '=');                       //another line of '=' signs
            Console.Write("\n\n\n\n");


            /*************************************************************************
                   PRINT OUT THE OPERANDS AND THEIR VALUES            
             *************************************************************************/
            Console.WriteLine("\nOPERAND SYMBOLS USED:\n");   //title
            for (int i = 0; i < NOPNDS; i++)
                Console.Write(opnd[i].ToString().PadLeft(5));

            Console.WriteLine("\n\n\nCORRESPONDING OPERAND VALUES:\n");   //title
            for (int i = 0; i < NOPNDS; i++)
                Console.Write(opndval[i].ToString().PadLeft(5));

            Console.WriteLine();
            Console.WriteLine();

            /*************************************************************************
                                        V2 Main Block
            *************************************************************************/


            Console.WriteLine("{0} {1} {2}", "Infix String".PadRight(23), "Postfix String".PadRight(23), "Double Value");
            Console.WriteLine("______________________________________________________________________");
            for (IDX = 0;IDX < LSIZE; IDX++)
            {
                ifx = WKinfix[IDX];
                Convert(ref ifx, ref pfx);
                double answer = Evaluate(WKpostfix[IDX]);

                Output(ifx, pfx, answer);
            }

            Console.Write("Version 2 complete: Press any key to continue");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();


            /*************************************************************************
                                       V3 Main Block
            *************************************************************************/

            dumpOPNDstack(opndvalStk);              /************************/
            double pop = OPNDpop(opndvalStk);       /*                      */
            Console.WriteLine(pop);                 /*                      */
            dumpOPNDstack(opndvalStk);              /*        Test          */
            OPNDpush(opndvalStk, 21);               /*                      */
            dumpOPNDstack(opndvalStk);              /************************/

            Console.WriteLine();
            Console.WriteLine();

            int ptest = 0;

            for (int cnt = 0; cnt < 5; cnt++)       
            {
                OPNDpush(opndvalStk, ++ptest);         //push ptest onto stack 5 times
                dumpOPNDstack(opndvalStk);              //output stack
            }
            while (opndvalStk.Count > 0)                  //until stack has 1 item left
            {
                OPNDpop(opndvalStk);                    //pop the stack
                dumpOPNDstack(opndvalStk);              //output stack
            }
            Console.WriteLine("Stack is now empty");

            Console.Write("Version 3 complete: Press any key to continue");
            Console.ReadKey();

        }



        /***************************************************************************************** 
                FUNCTION OutLine:   formatting function to print n repetitions of char ch
        ******************************************************************************************/
        static void OutLine(int n, char ch)
        {   for (int q = 0; q < n; q++)
                Console.Write(ch.ToString());
            Console.WriteLine("\n");
        }

        /***************************************************************************************** 
                FUNCTION Convert:   Stub. Later will be used to convert infix to postfix
        ******************************************************************************************/
        static void Convert(ref string ifx, ref string pfx)
        {
            string[] infix = new string[LSIZE] { "C$A$E",    //array of infix strings
                             "(A+B)*(C-D)",
                             "A$B*C-D+E/F/(G+H)",
                             "((A+B)*C-(D-E))$(F+G)",
                             "A-B/(C*D$E)"  };

            string[] postfix = new string[LSIZE] { "CAE$$",  //array of postfix strings
                             "AB+CD-*",
                             "AB$C*D-EF/GH+/+",
                             "AB+C*DE--FG+$",
                             "ABCDE$*/-"  };

            char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operands symbols
            double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };

            List<string> WKinfix = infix.ToList<string>();      //array of infix strings to work with
            List<string> WKpostfix = postfix.ToList<string>();  //array of postfix strings to work with

            List<char> opndStk = opnd.ToList<char>();               //stack of operand symbols
            List<double> opndvalStk = opndval.ToList<double>();     //stack of opreand values

            pfx = WKpostfix[IDX];
        }

        /***************************************************************************************** 
                FUNCTION Evaluate:   Stub. Later will be used to evaluate the double value of postfix
        ******************************************************************************************/
        static double Evaluate(string pfx)
        {
            return 187.73;
        }

        /***************************************************************************************** 
                FUNCTION Output:   Outputs postfix string and its double value
        ******************************************************************************************/
        static void Output(string ifx, string pfx, double val)
        {
            Console.WriteLine();
            Console.WriteLine("{0} {1} {2}", ifx.PadRight(23), pfx.PadRight(23), val);
            Console.WriteLine();
        }

        /***************************************************************************************** 
                FUNCTION dumpOPNDstack:   Prints out contents of the operand stack
        ******************************************************************************************/
        static void dumpOPNDstack(List<double> opndvalStk)
        {
            foreach (double operand in opndvalStk)
                Console.Write(operand+", ");
            Console.WriteLine();
        }

        /***************************************************************************************** 
                FUNCTION OPNDpop:   Takes out the top stack value and returns it
        ******************************************************************************************/
        static double OPNDpop(List<double> opndval)
        {
            double x = opndval[opndval.Count - 1];              //value to be returned
            opndval.RemoveAt(opndval.Count - 1);                //popping top value

            return x;
        }

        /***************************************************************************************** 
                FUNCTION OPNDpush:   Puts a new value on top of the stack
        ******************************************************************************************/
        static void OPNDpush(List<double> opndval, double val)
        {
            opndval.Add(val);                                //push new value on stack
        }

    }
}
