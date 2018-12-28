﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for Factory_Manager_Dashboard.xaml
    /// </summary>
    public partial class Factory_Manager_Dashboard : Window
    {
        public Factory_Manager_Dashboard()
        {
            InitializeComponent();
        }

        private void btn_recordReceivedItem_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Record_Delivered_Item_Window(Login.LocID));
        }

        private void Btn_makeFactoryDecision_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Factory_Item_Decision_Window());
        }
    }
}