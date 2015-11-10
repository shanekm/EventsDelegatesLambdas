using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        private static bool LogToEventLog(string message) { return true; }
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

            // Dynamic - pass delegate to external method
            var data = new ProcessData();
            data.Process(2, 3, addDel);
            data.Process(2, 5, multiplyDel);


            // Action<T> - void return, takes in one or more params
            //public delegate void delAction(string message);
            Action<string> delAction = new Action<string>(Display);
            delAction("Invoking action");

            // Dynamic
            if (1 == 1)
                delAction = Display;
            else
                delAction = Console.WriteLine; // WriteLine takes in string overload

            Action<string> delAction2 = (s) =>
                {
                    // string passed into method
                    string msg = s;
                    Console.WriteLine(msg);
                };
            delAction2.Invoke("this is test");

            // Action<T> ex2
            //public delegate void delAction(int x, int y);
            Action<int, int> myAddAction = (x, y) => Console.WriteLine(x + y); // method body
            Action<int, int> myMultiplyAction = (x, y) => Console.WriteLine(x * y);
            myAddAction(2, 3); // Invoke

            // Dynamic
            var data1 = new ProcessData();
            data1.ProcessAction(2, 3, myAddAction);
            data1.ProcessAction(2, 3, myMultiplyAction);

            // Func<T,TResult>
            // Read: myAddFunc is a pointer to a function/method 
            // whenever invoked will take 3 params (x,y,z) and will execute method body returning string
            Func<int, int, int, string> myAddFunc = (x, y, z) =>
                {
                    var res = x + y + z; 
                    //Console.WriteLine(res.ToString()); // Need to return a string
                    return res.ToString();
                };

            // Invoke delegate
            myAddFunc(1, 2, 3);

            // Dynamic - pass it into a method
            var data2 = new ProcessData();
            data2.ProcessFunc(2, 3, 5, myAddFunc);

            // Dynamic Func<T,TResult>
            // point to a method that takes in string and returns a bool
            Func<string, bool> myLoggingFunc;
            if (1 == 1)
                myLoggingFunc = LogToScreen;
            else
                myLoggingFunc = LogToEventLog;

            // Invoke
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

            // 1 - Executing Func custom methods
            Func<Customer, string> doStuff = (c) => c.Name.ToLower();
            string stringResult = GetStuff(custs, doStuff); // Calling method 

            // 2 - Without calling external method
            stringResult = GetStuff(custs, c => c.City);

            // 3 - Without external method
            // Method performs operations on List<Customer>
            Func<IEnumerable<Customer>, string> doStuffWithoutMethod = (c) =>
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Customer customer in c)
                    {
                        sb.AppendLine(customer.City); 
                    }

                    return sb.ToString();
                };

            // Invoke
            stringResult = doStuffWithoutMethod(custs);

            // 4 - get/set property Func<T>
            var someClass = new SomeClass<IEnumerable<Customer>> { 
                funcProperty = (c) =>
                    {
                        return c.FirstOrDefault().Name.ToLower();
                    }
            };

            // Invoke
            stringResult = someClass.funcProperty.Invoke(custs);
            

            // Action<Customer, string>
            Action<Customer, string, int> doStuffAction = (a, b, c) =>
                {
                    Console.WriteLine(a.Name + "-" + b + (c / 2).ToString());
                };

            doStuffAction(custs.FirstOrDefault(), "-test-", 10);
        }

        private static int Add(int a, int b)
        {
            return a + b;
        }

        private static void Display(string message)
        {
            MessageBox.Show(message);
        }

        // Function that will return string of names or cities
        // T = Customer
        // Func<T => what goes in Invoke method
        private static string GetStuff<T>(IEnumerable<T> list, Func<T, string> func)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
            {
                sb.AppendLine(func.Invoke(item));
            }

            return sb.ToString();
        }
    }

    public class SomeClass<T>
    {
        public Func<T, string> funcProperty { get; set; }
    }
}