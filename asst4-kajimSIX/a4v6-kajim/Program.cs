using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4v6kajim
{
    /****************************************************************************************
     *                                  ID Block                                            *
     * Name: Mamoru-Maru Kajifusa                                                           *
     * Student ID: 1411266                                                                  *
     * Assignment #4 Version 6                                                              *
     * Application Description:                                                             *
     * This program works with notations for infix aka traditional algebraic                *
     * expressions, with conversion into postfix expression, which has the operators        *
     * at the end of two operands instead of between them. The program then evaluates       *
     * the postfix expression and calculates then outputs its value.                        *
     * In the conversion section, the algorithm employs a stack of characters to store      *
     * the operators encountered thus far in the infix scanning process, the evaluation     * 
     * algorithm uses a stack of real numbers to store the values of the operands and       *
     * the numerical values of successive arithmetic operations.                            *
     * Each of these stacks is constructed using a C# list.                                 *                                                     
     ****************************************************************************************/                                   
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
            
            char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operands symbols
            double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };           //operand values

            string ifx;                                     //next infix string to be converted to postfix
            string pfx;                                     //converted postfix string

            /*************************************************************************
                   PRINT OUT THE OPERANDS AND THEIR VALUES            
             *************************************************************************/
            Console.WriteLine("\n\n\tCORRESPONDING OPERAND SYMBOLS:\n");   //title
            for (int i = 0; i < NOPNDS; i++)
                Console.Write(opnd[i].ToString().PadLeft(5));

            Console.WriteLine("\n\n\n\tCORRESPONDING OPERAND VALUES:\n");   //title
            for (int i = 0; i < NOPNDS; i++)
                Console.Write(opndval[i].ToString().PadLeft(5));

            Console.WriteLine();
            Console.WriteLine();

            /************************************************************************
             *                          Main Block                      
             ************************************************************************/
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ {0} {1} {2} ║", "Infix String".PadRight(23), "Postfix String".PadRight(23), "Double Value".PadRight(18));
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            for (IDX = 0; IDX < LSIZE; IDX++)
            {
                ifx = infix[IDX];
                Convert(ifx, out pfx);
                double answer = Evaluate(pfx); 

                Output(ifx, pfx, answer);

                if(IDX > LSIZE-1)
                    Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Version 6 complete: Press any key to continue");
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

        /********************************************************************************************* 
          FUNCTION Convert:   -Converts an infix expression string to postfix expression string.
                              -Operands are directly sent to the resulting postfix string
                              -Other symbols are evaluated depending on the condition of the operator
                                stack:
                                    -An empty stack gets the symbol s
                                    -Operators keep getting sent onto the stack as long as they have a 
                                        higher priority in the order of operations
                                    -Otherwise, they get sent to the resulting postfix string
                              -Parentheses get special rules:
                                -when '(' is encountered, it is automatically sent to the stack
                                -when ')' is encountered, the stack continuously gets popped to the 
                                    resulting postfix string until '(' is at the top of the stack
                                -'(' gets removed from the stack when ')' is encountered or when the
                                    stack is empty
                              -When there aren't anymore infix symbols to be evaluated, the stack
                                continuously gets popped to the resulting postfix string until it's
                                empty.

        **********************************************************************************************/
        static void Convert(string ifx, out string pfx)
        {
            List<char> infx = ifx.ToList<char>();   //convert string to a char list to allow evaluation of each char
            List<char> postfx = new List<char>();   //converted postfix string
            List<char> oprStk = new List<char>();   //operator stack
            char s;         //next symbol to evaluate
            char top;       //symbol on top of stack
            bool und;       //underflow. Is true if operator stack is empty 

            while(infx.Count != 0) //until no more infix symbols
            {
                s = infx[0];        //next infix symbol
                infx.RemoveAt(0);   //remove said symbol

                if (Char.IsLetter(s))    //if s is an operand
                    postfx.Add(s);       //join s to postfix string
                else        //s is an operator or a bracket
                {
                    OPRpopandtest(oprStk, out top, out und);    //get top symbol if it exists
                    
                    while(!und && Prcd(top,s))                      //stack not empty and top is a higher priority in  
                    {                                               //order of operations than s. Or s is ')' until top is '('
                        postfx.Add(top);                            //join top symbol from stack to postfix string
                        OPRpopandtest(oprStk, out top, out und);    //get next top symbol if it exists
                    }

                    if (!und)               //if stack not empty
                        oprStk.Add(top);    //put top back to stack

                    if (und || s != ')')        //if empty stack or next symbol not ')'
                        oprStk.Add(s);          //add s to operator stack
                    else                                            //s is ')' and stack not empty
                        OPRpopandtest(oprStk, out top, out und);    //pop the stack to eliminate '('    
                }
            }
            while (oprStk.Count != 0)                       //until operator stack is empty
            {
                OPRpopandtest(oprStk, out top, out und);    //pop operator stack
                postfx.Add(top);                            //and add it to postfix string
            }

            StringBuilder b = new StringBuilder();
            foreach (char sym in postfx)
                b.Append(sym);
            pfx = b.ToString();                     //convert char array to string
        }

        /***************************************************************************************** 
            FUNCTION Evaluate:   -Evaluates the double value of postfix
                                 -Operands are sent directly to the operand stack
                                 -When an operator is encountered, the top 2 values of the stack
                                    are popped as operands. The resulting value is pushed back
                                    onto the stack
        ******************************************************************************************/
        static double Evaluate(string pfxStr)
        {
            List<double> Stk = new List<double>();      //operand stack used to get double value of postfix string
            List<char> pfx = pfxStr.ToList<char>();     //change string to char list

            char s;             //next postfix symbol
            double sval;        //double value of s if it's a digit
            double op1;         //left-hand operand
            double op2;         //right-hand operand
            double val = 0;     //result of operation between op1 and op2. Also value to be returned after evaluation

            for(int i = 0;i < pfx.Count;i++)      //until no more postfix symbols
            {
                s = pfx[i];     //get next postfix symbol

                if (Char.IsLetter(s))           //if next postfix symbol is an operand
                {
                    sval = GetOpndVal(s);       //get double value of operand symbol
                    OPNDpush(Stk, sval);        //push it in stack
                }
                else                    //next postfix symbol is an operator
                {
                    op2 = OPNDpop(Stk);
                    op1 = OPNDpop(Stk);

                    switch(s)           //operation switch branch
                    {
                        case '+':           //addition
                            val = op1 + op2;break;
                        case '-':           //subtraction
                            val = op1 - op2;break;
                        case '*':           //multiplication
                            val = op1 * op2;break;
                        case '/':           //division
                            val = op1 / op2;break;
                        case '$':           //exponent
                            val = Math.Pow(op1, op2);break;
                    }

                    OPNDpush(Stk, val);         //push operation result into stack
                }
            }

            val = OPNDpop(Stk);     //get final double value
            return val;             //return it


        }

        /***************************************************************************************** 
                FUNCTION Output:   Outputs postfix string and its double value
        ******************************************************************************************/
        static void Output(string ifx, string pfx, double val)
        {
            Console.WriteLine("║ {0} {1} {2} ║", ifx.PadRight(23), pfx.PadRight(23), val.ToString().PadRight(18));
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
            opndval.Add(val);           //push new value on stack
        }

        /***************************************************************************************** 
                FUNCTION GetOpndVal:   Returns the double value of the opnd letter
        ******************************************************************************************/
        static double GetOpndVal(char s)
        {
            char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operands symbols
            double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };           //operand values

            return opndval[s - 'A'];    //return double at corresponding position of s in double array 
        }
        /***************************************************************************************** 
                FUNCTION OPRpush:   Puts a new operator on top of the stack
        ******************************************************************************************/
        static void OPRpush(List<char> oprStk, char opr)
        {
            oprStk.Add(opr);    //return double at corresponding position of s in double array 
        }
        /***************************************************************************************** 
                FUNCTION dumpOPRstack:   Prints out contents of the operator stack
        ******************************************************************************************/
        static void dumpOPRstack(List<char> oprStk)
        {
            foreach (char oprtr in oprStk)
                Console.Write(oprtr + ", ");
            Console.WriteLine();
        }
        /******************************************************************************************** 
            FUNCTION OPRpopandtest:   -Takes out the top stack value and returns it 
                                      -Also checks if stack is empty
                                      -The boolean und means underflow, it determines if the stack
                                        is empty.
                                      -The char top is the top symbol of the stack if it's not empty
        *********************************************************************************************/
        static void OPRpopandtest(List<char> oprStk, out char top, out bool und)
        {
            if (oprStk.Count == 0)                       //if empty stack
            {
                und = true;                              //und is true
                top = '\0';                              //top gets nothing
                return;
            }
            else                                    //if stack not empty
                und = false;                        //und is false

            top = oprStk[oprStk.Count - 1];              //value to be returned
            oprStk.RemoveAt(oprStk.Count - 1);           //popping top value
        }
        /**************************************************************************************************** 
            FUNCTION Prcd:  -Predence. Checks 2 symbols and returns true if the left-hand operator has
                                priority over the right-hand operator in the order of operations. 
                            -$ is the sign for exponents
                            -Exception rules:
                                -left is '(' = false
                                -right is '(' = false unless left is ')'
                                -right is ')' = true unless left is '('
                                -double $ is false because the right-most exponent expression gets evaluated
                                    first
        *****************************************************************************************************/
        static bool Prcd(char left, char right)
        {
            if (left == '(')
                return false;
            else if (right == '(')
                return false;
            else if (right == ')')
                return true;
            else if (left == '$' && right == '$')
                return false;
            else if (Priority(left) >= Priority(right)) //actual operators that aren't both $
                return true;
            else //data is assumed to be valid. Will never get here
                return false;
        }
        /************************************************************************************************ 
            FUNCTION Priority:   -Gets an operator and sets its priority in the order of operations
                                 -A higher number/priority means it gets evaluated first unless there
                                    are brackets.
                                 -$ is the sign for exponents
        *************************************************************************************************/
        static int Priority(char opr)
        {
            switch(opr)
            {
                case '+':     //if operator is addition
                case '-':     //or subtraction
                    return 1; //return lowest priority
                case '*':             //if operator is multiplication
                case '/':             //or division
                    return 2;         //return middle priority
                case '$':                 //if operator is exponent
                    return 3;             //return highest priority
                default:                        //unknown operator. Will never get here
                    return 0;                       
            }
        }
    }
}
