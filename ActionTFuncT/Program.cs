using System;

namespace ActionTFuncT
{
    internal class Program
    {
        // toLower delegate
        private static Func<string, string> toLower = x => x.ToLower();

        private static Func<string, string> addString = x => x + "-added string";

        // This method takes in simpleFunc delegate that prints out that methods action
        // ie. ToLower()

        // Compare two strings <string, string> as input
        // This is a "FORMULA" only = outside of the method
        // and MyFunc2 is the actual data being computed
        private static Func<string, string, bool> checkStringTrue = (x, y) => x == y;

        // Here is IMPLEMENTATION of what is being Done with data
        private static Func<string, string, bool> checkStringFalse = (x, y) => x != y;

        private static Func<string, string, string> Add2Strings = (x, y) => { return x + y; };

        private static void Main(string[] args)
        {
            // points to toLower method below
            StringFunc("UPPER STRING", toLower);

            // doesn't point to any method but uses lambda
            StringFunc("LAMBDA HERE", x => x.ToLower());

            // doesn't point to any method but uses lambda => sending IMPLEMENTATION
            StringFunc("lower string", x => x.ToUpper());

            // points to addString Func
            StringFunc("start string", addString);

            // Bool
            // Here I'm pointing to checkStringTrue formula
            BoolFunc(checkStringTrue);

            // Here I'm pointing to checkStringFalse formula
            BoolFunc(checkStringFalse);

            // Bool test
            BoolFunc2("test", "test", checkStringTrue);

            // Here I'm passing actual implementation to function
            // (x, y) will be executed and not checkStringTrue
            BoolFunc2("test", "test", (x, y) => x == y);

            // Use the Func<> delegate to point to Add. => POINTS TO ADD!
            Func<int, int, int> funcTarget = new Func<int, int, int>(Add);
            int result = funcTarget.Invoke(2, 3);

            Action<string, string> myAction = AddStringAction;
            myAction("string1", "string2");

            // Define and Implement here. Doesn't point to any method => but implements at the same time
            Action<string, string> myActionString = (x, y) => Console.WriteLine(string.Concat(x, y));
            myActionString.Invoke("string1", "string2");

            // Define and point to a Method that will process this func
            string res = Add2Strings("test", "added");

            res = AddStringMethod("test1", "test2", Add2Strings);
        }

        private static void StringFunc(string sampleString, Func<string, string> simpleFunc)
        {
            Console.WriteLine(simpleFunc(sampleString));
        }

        private static void BoolFunc(Func<string, string, bool> simpleFunc)
        {
            string a = "a";
            string b = "b";

            bool result = simpleFunc(a, b);
            Console.WriteLine(result);
        }

        // simpleFunc is only a POINTER to the actual implementation
        private static void BoolFunc2(string str1, string str2, Func<string, string, bool> simpleFunc)
        {
            Console.WriteLine(simpleFunc(str1, str2));
        }

        private static int Add(int x, int y)
        {
            return x + y;
        }

        private static void AddStringAction(string msg, string msg2)
        {
            Console.WriteLine(msg + msg2);
        }

        // Using Lambda Implementation
        private static string AddStringMethod(string msg1, string msg2, Func<string, string, string> myFunc)
        {
            Console.WriteLine(myFunc(msg1, msg2));
            return myFunc(msg1, msg2);
        }
    }
}