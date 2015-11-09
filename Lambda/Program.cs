using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lambda
{
    public class Program
    {
        // define delegate - with parameters
        public delegate int AddDelegate(int a, int b);

        // delegate without parameters
        public delegate bool LogDelegate();

        // pipeline, takes in two ints and returns an int
        public delegate int BizRulesDelegate(int x, int y);

        // Action<T> - no need to define it
        // public Action<string> DelAction;
        // public delegate void DelAction(string message); // Same as Action<T> above

        // Func<T,TResult> - no need to define it
        // public Func<string, bool> logFunc;
        // public delegate bool logFunc(string message);

        private static bool LogToEventLog(string message)
        {
            // log
            return true;
        }

        private static bool LogToScreen(string message) { return true; }

        private static void Main(string[] args)
        {
            // without lambda
            AddDelegate del1 = new AddDelegate(Add);
            del1(1, 2);

            // lambda expression can be assigned to any delegate
            // method body { a + b } without writing the method
            AddDelegate del2 = (a, b) => a + b;

            // invoke delegate
            int result = del2(2, 1);


            // Lambda without params
            LogDelegate logDel = () =>
                {
                    Console.WriteLine("do work");
                    return true;
                };

            // invoke delegate
            bool status = logDel();

            
            // Method signature same as definition
            BizRulesDelegate addDel = (x, y) => x + y; // no need for 'return' keyword
            BizRulesDelegate multiplyDel = (x, y) => x * y; // same as name of external method injected to delegate constructor
            Console.WriteLine(addDel(2, 3)); // Invoke delegate

            // Dynamic
            var data = new ProcessData();
            data.Process(2, 3, addDel);
            data.Process(2, 5, multiplyDel);


            // Action<T> - void return, takes in one param
            Action<string> delAction = new Action<string>(Display);
            delAction("Invoking action");

            // Dynamic
            if (1 == 1)
                delAction = Display;
            else
                delAction = Console.WriteLine; // WriteLine takes in string overload


            // Action<T> ex2
            Action<int, int> myAddAction = (x, y) => Console.WriteLine(x + y);
            Action<int, int> myMultiplyAction = (x, y) => Console.WriteLine(x * y);

            var data1 = new ProcessData();
            data1.ProcessAction(2, 3, myAddAction);
            data1.ProcessAction(2, 3, myMultiplyAction);

            // Func<T,TResult>
            Func<int, int, int, string> myAddFunc = (x, y, z) =>
                {
                    var res = x + y + z; 
                    //Console.WriteLine(res.ToString()); // Need to return a string
                    return res.ToString();
                };

            myAddFunc(1,2,3);

            // Or pass it into a method
            var data2 = new ProcessData();
            data2.ProcessFunc(2, 3, 5, myAddFunc);

            // Dynamic Func<T,TResult>
            Func<string, bool> myLoggingFunc;
            if (1 == 1)
                myLoggingFunc = LogToScreen;
            else
                myLoggingFunc = LogToEventLog;

            bool message = myLoggingFunc("testing logging");


            // Lambda Func<Customer, bool> - in Customer, returns bool
            var custs = new List<Customer>
                            {
                                new Customer { Name = "John", City = "New York" },
                                new Customer { Name = "Bob", City = "Chicago" }
                            };

            // Func<Customer, bool> - returns true 
            // c == (c) -> parameter || method body { c.City == "Chicago" }
            var customers = custs.Where((c) => c.City == "Chicago");
        }

        private static int Add(int a, int b)
        {
            return a + b;
        }

        private static void Display(string message)
        {
            MessageBox.Show(message);
        }
    }
}