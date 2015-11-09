using CommunicatingBetweenControls.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CommunicatingBetweenControls.UserControls
{
    public partial class Jobs : UserControl
    {
        List<Job> _Jobs = new List<Job>
        {
            new Job { ID = 1, Title = "Area 1 Maintenance", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) },
            new Job { ID = 2, Title = "Edge Park", StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(5)  },
            new Job { ID = 3, Title = "Paint Benches", StartDate = DateTime.Now.AddDays(4), EndDate = DateTime.Now.AddDays(10)  },
            new Job { ID = 4, Title = "Build New Wall", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(15)  }
        };

        public Jobs()
        {
            InitializeComponent();
            BindData();
        }

        private void BindData()
        {
            JobsComboBox.ItemsSource = _Jobs;
        }

        private void JobsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mediator.GetInstance().OnJobChanged(this, (Job)JobsComboBox.SelectedItem);
        }
    }
}
