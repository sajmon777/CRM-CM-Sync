using Sync.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sync_WPFBlend
{
	/// <summary>
	/// Interaction logic for LogWindow.xaml
	/// </summary>
	public partial class ErrorLogWindow : Window
	{
        EventType eventType;

        public ErrorLogWindow(EventType eventType)
		{
			this.InitializeComponent();
            this.eventType = eventType; 
            List<Message> eventList = EventLogManager.ReadMessages(eventType);
            EventDataGrid.ItemsSource = eventList; 
		}

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            EventLogManager.ClearList(eventType);
            List<Message> eventList = EventLogManager.ReadMessages(eventType);
            EventDataGrid.ItemsSource = eventList; 
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    
        
    
    
    }
}