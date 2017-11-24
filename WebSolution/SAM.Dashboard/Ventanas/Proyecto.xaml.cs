using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;
using SAM.Dashboard.Resources;

namespace SAM.Dashboard.Ventanas
{
    public partial class Proyecto : UserControl
    {
        public Proyecto()
        {
            InitializeComponent();

            DataContext = new Recursos();
        }
    }
}
