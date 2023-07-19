using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Linq;

namespace ServiceManager
{
    public partial class MainWindow : Window
    {

        public List<string> servs;

        public MainWindow()
        {
            InitializeComponent();
            servs = new List<string>();
            servBox.ItemsSource = ServiceLogics.getServiceList("");
        }

        private void GetButt_Click(object? sender, RoutedEventArgs e)
        {
            servBox.ItemsSource=ServiceLogics.getServiceList(Inp.Text);
            Inp.Text="";
        }

        private void ReqSwitch(string action)
        {
            if (servBox.SelectedItem != null)
            {
            	OutputLog.Text = ServiceLogics.requestHandler(servBox.SelectedItem.ToString(), action);
            	servBox.ItemsSource=ServiceLogics.getServiceList(servBox.SelectedItem.ToString().Split(".")[0]);
            }
            else
            { OutputLog.Text = "Service is not chosen"; }
        }

        private void ResButt_Click(object? sender, RoutedEventArgs e)
        {
            ReqSwitch("restart");
        }

        private void StarButt_Click(object? sender, RoutedEventArgs e)
        {
            ReqSwitch("start");
        }

        private void StopButt_Click(object? sender, RoutedEventArgs e)
        {
            ReqSwitch("stop");
        }
    }
}