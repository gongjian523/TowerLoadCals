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
using DevExpress.Xpf.Core;
using System.Windows.Media.Animation;

namespace TowerLoadCals
{
    
    public partial class SplashScreenWindow : Window, ISplashScreen {
        Storyboard Storyboard { get { return (Storyboard)FindResource("storyboard"); } }

        public SplashScreenWindow() {
            Copyright = AssemblyInfo.AssemblyCopyright;
            InitializeComponent();
        }

        public string Copyright { get; set; }

        #region ISplashScreen
        void ISplashScreen.Progress(double value) {
            progressBar.Value = value;
        }
        void ISplashScreen.CloseSplashScreen() {
            if (Storyboard != null) {
                Storyboard.Completed += OnAnimationCompleted;
                Storyboard.Begin();
            }
        }
        void ISplashScreen.SetProgressState(bool isIndeterminate) {
            progressBar.IsIndeterminate = isIndeterminate;
        }
        #endregion
        
        void OnAnimationCompleted(object sender, EventArgs e) {
            Storyboard.Completed -= OnAnimationCompleted;
            this.Close();
        }
    }
}
