using System;
using CommunicatingBetweenControls.Model;

namespace CommunicatingBetweenControls
{
    public class JobChangedEventArgs : EventArgs
    {
        public Job Job { get; set; }
    }
}