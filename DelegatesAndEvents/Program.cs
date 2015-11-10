using System;

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

            // Pass in deletage - del2 calls WorkPerformed2. It will invoke whatever was passed ie. WorkPerformed2
            DoWork(del2);

            // Adding handlers to Invocation list of Delegate1
            // Whenever del1 fires it will notify WorkPerformed2 and WorkPerformed3 as well
            del1 += del2;
            del1 += del3; // I could also do { del1 += del2 + del3 }


            // Events with Delegates 
            // Attach handlers with delegates
            // Worker class 3 public events to wire up
            var worker = new Worker();
            
            // Without using Generic<T>
            worker.WorkPerformed += new WorkPerformedHandlerDelEvent(worker_WorkPerformed);

            // Wire event
            // specifying parameter signature match/pipeline
            worker.WorkPerfomedT += new EventHandler<WorkPerformedEventArgs>(worker_WorkPerformed);
            worker.WorkCompleted += worker_WorkCompleted; // Delegate Inference

            // Worker performs some task that exposes it's events. Whenever event is fired
            // subscribers (worker_WorkPerformed, worker_WorkCompleted) will be notified
            // Start work
            worker.DoWork(3, WorkType.GoToMeeting);


            // Testing class
            MyWorker w = new MyWorker();
            w.AttachListenerToMe += MyWorkerNotified;
            

            // Using Func with Events
            // Can't do this
            //w.AttachListenerToMe += Action<object, EventArgs> = (o, e) =>
            //    {
            //        Console.WriteLine();
            //    };
            
            string stringResult = w.DoWork();

            Console.Read();
        }

        public static int worker_WorkPerformed(int hours, WorkType workType)
        {
            Console.WriteLine("Hours worked: {0}, {1}", hours, workType);
            return hours;
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
        static void DoWork(WorkPerformedHandler del)
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

        static void MyWorkerNotified(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString(), e.ToString());
        }
    }

    public enum WorkType
    {
        GoToMeeting = 0, 
        Golf = 1, 
        GenerateReports = 2
    }
}