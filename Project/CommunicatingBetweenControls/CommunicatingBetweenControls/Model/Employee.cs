using System.Collections.Generic;

namespace CommunicatingBetweenControls.Model
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Job> Jobs { get; set; }
    }
}