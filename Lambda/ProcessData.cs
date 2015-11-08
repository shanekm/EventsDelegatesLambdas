using System;

namespace Lambda
{
    public class ProcessData
    {
        // Method takes in int(s) x and y
        // and also "Rules" for processing x and y
        // Passing in "what to do" with x and y. Keeps things loosely coupled
        public void Process(int x, int y, Program.BizRulesDelegate del)
        {
            var result = del(x, y); // Invoke delegate
            Console.WriteLine(result);
        }

        // Using Action<T>
        public void ProcessAction(int x, int y, Action<int, int> action)
        {
            action(x, y); // Invoke
            Console.WriteLine("Action has been processed");
        }

        public void ProcessFunc(int x, int y, int z, Func<int, int, int, string> func)
        {
            string result = func(x, y, z); // Invoke
            Console.WriteLine("Result of func: {0}", result);
        }
    }
}