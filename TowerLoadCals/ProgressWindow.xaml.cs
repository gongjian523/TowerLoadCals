using DevExpress.Xpf.Core;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductsDemo {
    
    
    
    public partial class ProgressWindow : Window, ISplashScreen {
        Storyboard Storyboard { get { return (Storyboard)FindResource("storyboard"); } }

        public ProgressWindow() {
            InitializeComponent();
        }

        void ISplashScreen.CloseSplashScreen() {
            if (Storyboard != null) {
                Storyboard.Completed += OnAnimationCompleted;
                Storyboard.Begin();
            }
        }

        void ISplashScreen.Progress(double value) {
        }

        void ISplashScreen.SetProgressState(bool isIndeterminate) {
        }
        
        void OnAnimationCompleted(object sender, EventArgs e) {
            Storyboard.Completed -= OnAnimationCompleted;
            this.Close();
        }
    }
}
