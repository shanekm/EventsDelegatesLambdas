using System;

namespace DelegatesAndEvents
{
    // Delegate - pipeline signature, takes in int and enum
    // specify what will be passed into delegate/pipeline
    public delegate int WorkPerformedHandlerDelEvent(int hours, WorkType workType);

    public class Worker
    {
        // Events - wiring with delegates
        // Find who is attached to this delegate and when
        // this event fires, find the method that delegate points to
        // Events are really syntactic sugar sitting on top of event
        //              delegate                    event name
        public event WorkPerformedHandlerDelEvent WorkPerformed;

        // Compiler defines/generates a delegate 
        // and writes the above delegate signature for us ie. (object sender, EventArgs e)
        // EventHandler<WorkPerformedEventArgs> == delegate void WorkPerformedHandler(object sender, WorkPerformedEventArgs e)
        public event EventHandler<WorkPerformedEventArgs> WorkPerfomedT; // delegate void WorkPerformedHandler(object sender, WorkPerformedEventArgs e)

        // Built in .net event handler
        // public delegate void EventHandler(object sender, EventArgs e); source
        public event EventHandler WorkCompleted;

        public void DoWork(int hours, WorkType workType)
        {
            for (int i = 0; i < hours; i++)
            {
                System.Threading.Thread.Sleep(1000); // two seconds

                // Take 1
                // Call method below - raise the event
                this.OnWorkPerformed(i + 1, workType);

                // Take 2 with Generic
                this.OnWorkPerformedT(i + 1, workType);
            }

            this.OnWorkCompleted();
        }

        // Virtual can be overwridden
        protected virtual void OnWorkPerformed(int hours, WorkType workType)
        {
            //if (WorkPerformed != null) // any listeners attached?
            //{
            //    // Delegate call
            //    WorkPerformed(hours, workType); // listeners will be notified
            //}

            // Same as
            var del = WorkPerformed as WorkPerformedHandlerDelEvent; // cast to delegate (since event is delegate in reallity)
            if (del != null) // anyone in invocation list?
            {
                del(hours, workType);
            }
        }

        protected virtual void OnWorkPerformedT(int hours, WorkType workType)
        {
            var del = WorkPerfomedT as EventHandler<WorkPerformedEventArgs>; // cast to delegate (since event is delegate in reallity)
            if (del != null) // anyone in invocation list?
            {
                del(this, new WorkPerformedEventArgs(hours, workType));
            }
        }

        private void OnWorkCompleted()
        {
            var del = this.WorkCompleted as EventHandler; // cast to delegate (since event is delegate in reallity)
            if (del != null) // anyone in invocation list?
            {
                // This class with empty args
                del(this, EventArgs.Empty);
            }
        }
    }
}