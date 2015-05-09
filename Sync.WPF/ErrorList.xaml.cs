using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sync.BL.Infrastructure; 

namespace Sync.WPF
{
    /// <summary>
    /// Interaction logic for ErrorList.xaml
    /// </summary>
    public partial class ErrorList : Window
    {
        public ErrorList()
        {
            InitializeComponent();
            List<Message> list = EventLogManager.ReadMessages(EventType.Error);
            ErrorGrid.ItemsSource = list; 
        }
    }
}
