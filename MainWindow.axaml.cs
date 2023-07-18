using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Linq;

namespace ServiceManager
{
    public partial class MainWindow : Window
    {

        private List<string> servs;

        public MainWindow()
        {
            InitializeComponent();
            servs = new List<string>(); //{ "cat", "pet", "net", "cat", "camel", "cow", "chameleon", "mouse", "lion", "zebra" };
            servBox.ItemsSource = servs;
            //ServiceLogics.getServiceList("", ref servs, ref OutputLog);
        }

        private void GetButt_Click(object? sender, RoutedEventArgs e)
        {
            ServiceLogics.getServiceList(Inp.Text, ref servs, ref OutputLog);
        }

        private void ReqSwitch(string action)
        {
            if (servBox.SelectedItem != null)
            { OutputLog.Text = ServiceLogics.requestHandler(servBox.SelectedItem.ToString(), action); }
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