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
using Sync.BL.Entities;
using Sync.BL.Infrastructure; 

namespace Sync_WPFBlend
{
	/// <summary>
	/// Interaction logic for AddWindow.xaml
	/// </summary>
	public partial class AddWindow : Window
	{
        private List<Sync.BL.Entities.List> cmList;
        private List<Sync.BL.Entities.List> crmList;
        List<CrmCmConnection> crmCmConnections;

        public AddWindow()
		{
            this.InitializeComponent(); 
		}

        public AddWindow(List<Sync.BL.Entities.List> cmList, List<Sync.BL.Entities.List> crmList, List<CrmCmConnection> crmCmConnections)
        {
            this.InitializeComponent();
            this.cmList = cmList;
            this.crmList = crmList;
            this.crmCmConnections = crmCmConnections; 
            InitializeComboBox(); 
        }
    
    
        private void InitializeComboBox(){
        
         Binding cmListBinding = new Binding();
                cmListBinding.Source = cmList;
                CmListComboBox.DisplayMemberPath = "Name";
                CmListComboBox.SelectedValuePath = "ListId";
                CmListComboBox.SetBinding(ComboBox.ItemsSourceProperty, cmListBinding);

                Binding crmListBinding = new Binding();
                crmListBinding.Source = crmList;
                CrmListComboBox.DisplayMemberPath = "Name";
                CrmListComboBox.SelectedValuePath = "ListId";
                CrmListComboBox.SetBinding(ComboBox.ItemsSourceProperty, crmListBinding);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Sync.BL.Entities.List crmList = (Sync.BL.Entities.List)CrmListComboBox.SelectedItem;
            Sync.BL.Entities.List cmList = (Sync.BL.Entities.List)CmListComboBox.SelectedItem;

            CrmCmConnection newConnection = new CrmCmConnection()
            {
                CmListName = cmList.Name,
                CmListId = cmList.ListId,
                CrmListName = crmList.Name,
                CrmListId = crmList.ListId,
                LastSync = new DateTime(1910, 1, 1)
            };
            crmCmConnections.Add(newConnection);
            ManageMapTable.SaveMapTable(crmCmConnections);
            this.Close(); 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}