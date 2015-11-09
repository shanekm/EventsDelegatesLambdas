using System.Windows.Controls;

namespace CommunicatingBetweenControls.UserControls
{
    /// <summary>
    /// Interaction logic for JobDetails.xaml
    /// </summary>
    public partial class JobDetails : UserControl
    {
        public JobDetails()
        {
            InitializeComponent();

            // Same as writing a method yourself
            Mediator.GetInstance().JobChanged += (s, e) => { this.DataContext = e.Job; };
        }
    }
}