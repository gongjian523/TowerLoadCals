
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Internal;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.PdfViewer;
using DevExpress.XtraPrinting;

namespace TowerLoad
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //ExceptionHelper.Initialize();
            DevExpress.Images.ImagesAssemblyLoader.Load();
            PdfViewerLocalizer.Active = new CustomPdfViewerLocalizer();
            ApplicationThemeHelper.ApplicationThemeName = Theme.Office2013Name;
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //ServiceContainer.Default.RegisterService(new ApplicationJumpListService());
            base.OnStartup(e);
        }
    }

    public class CustomPdfViewerLocalizer : PdfViewerLocalizer
    {
        public override string GetLocalizedString(PdfViewerStringId id)
        {
            switch (id)
            {
                case PdfViewerStringId.BarCaption: return "PDF VIEWER";
                case PdfViewerStringId.BarCommentCaption: return "COMMENT";
                case PdfViewerStringId.BarFormDataCaption: return "FORM DATA";
                default: return base.GetLocalizedString(id);
            }
        }
    }
}
