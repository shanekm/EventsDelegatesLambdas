using System;
using System.ComponentModel;

namespace DelegatesAndEvents
{
    // Delegate definition
    public delegate void WorkPerformedHandler(int hours, WorkType workType);

    // Returning value
    public delegate int WorkPerformedHandlerRet(int hours, WorkType workType);

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Pipeline that dumps data to WorkPerformed1
            WorkPerformedHandler del1 = new WorkPerformedHandler(WorkPerformed1);

            WorkPerformedHandler del2 = new WorkPerformedHandler(WorkPerformed2);

            WorkPerformedHandler del3 = new WorkPerformedHandler(WorkPerformed2);

            del1(5, WorkType.Golf);
            del2(10, WorkType.GenerateReports);

            // Pass in deletage - del1 calls WorkPerformed1. It will invoke whatever was passed ie. WorkPerformed1
            DoWorkTake2(del1);
            // Pass in deletage - del2 calls WorkPerformed2. It will invoke whatever was passed ie. WorkPerformed2
            DoWorkTake2(del2);

            // Adding handlers to Invocation list of Delegate1
            // Whenever del1 fires it will notify WorkPerformed2 and WorkPerformed3 as well
            del1 += del2;
            del1 += del3; // I could also do { del1 += del2 + del3 }


            // Events with Delegates 
            // Attach handlers with delegates
            // Worker class 3 public events to wire up
            var worker = new Worker();

            // Wire event
            // specifying parameter signature match/pipeline
            worker.WorkPerfomedT += new EventHandler<WorkPerformedEventArgs>(worker_WorkPerformed);
            worker.WorkCompleted += worker_WorkCompleted; // Delegate Inference

            // Fire event
            worker.DoWork(3, WorkType.GoToMeeting);

            Console.Read();
        }

        public static void worker_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            Console.WriteLine("Hours worked: {0}, {1}", e.Hours, e.WorkType);
        }

        public static void worker_WorkCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Work completed");
        }


        // Make WorkPerformed(x) method dynamic
        // Take 1
        static void DoWorkTake1()
        {
            // instead of hardocing i can pass in handler
            WorkPerformed1(5, WorkType.GoToMeeting);
        }

        // Make WorkPerformed(x) method dynamic
        // Take 2
        static void DoWorkTake2(WorkPerformedHandler del)
        {
            del(5, WorkType.GenerateReports);
        }

        static void WorkPerformed1(int hours, WorkType type)
        {
            Console.WriteLine("WorkPerformed 1 called: {0}:{1}", hours, type);
        }

        static void WorkPerformed2(int hours, WorkType type)
        {
            Console.WriteLine("WorkPerformed 2 called: {0}:{1}", hours, type);
        }

        static void WorkPerformed3(int hours, WorkType type)
        {
            Console.WriteLine("WorkPerformed 3 called: {0}:{1}", hours, type);
        }

        static int WorkPerformed1Ret(int hours, WorkType type)
        {
            Console.WriteLine("WorkPerformed 1 called: {0}:{1}", hours, type);
            return hours +1;
        }

        static int WorkPerformed2Ret(int hours, WorkType type)
        {
            Console.WriteLine("WorkPerformed 2 called: {0}:{1}", hours, type);
            return hours +2;
        }

        static int WorkPerformed3Ret(int hours, WorkType type)
        {
            Console.WriteLine("WorkPerformed 3 called: {0}:{1}", hours, type);
            return hours +3;
        }
    }

    public enum WorkType
    {
        GoToMeeting = 0, 
        Golf = 1, 
        GenerateReports = 2
    }
}