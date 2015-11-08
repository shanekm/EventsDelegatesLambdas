using System;

namespace DelegatesAndEvents
{
    public class WorkPerformedEventArgs : EventArgs
    {
        public WorkPerformedEventArgs(int hours, WorkType workType)
        {
            this.Hours = hours;
            this.WorkType = workType;
        }

        public int Hours { get; set; }

        public WorkType WorkType { get; set; }
    }
}