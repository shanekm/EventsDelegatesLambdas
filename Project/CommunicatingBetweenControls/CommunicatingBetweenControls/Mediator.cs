using System;
using CommunicatingBetweenControls.Model;

namespace CommunicatingBetweenControls
{
    // Singleton functionality
    public sealed class Mediator
    {
        private static readonly Mediator _Instance = new Mediator();

        private Mediator()
        {
        }

        public static Mediator GetInstance()
        {
            return _Instance;
        }

        // Instance functionality
        // Public event to subscribe to
        public event EventHandler<JobChangedEventArgs> JobChanged;

        // Gets called whenever drop down is changed
        public void OnJobChanged(object sender, Job job)
        {
            var jobChangeDelegate = JobChanged as EventHandler<JobChangedEventArgs>;
            if (jobChangeDelegate != null)
            {
                // raise event
                jobChangeDelegate(sender, new JobChangedEventArgs { Job = job });
            }
        }
    }
}