using System;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ExXAMLate.Common;
using ExXAMLate.Models;
using Okra.Core;
using Okra.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ExXAMLate.ViewModels
{
    public class AppXViewModel : NotifyPropertyChangedBase
    {
        public AppXModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        private readonly INavigationManager _navigation;
        private AppXModel _model;
        public ICommand GoBack { get; set; }
        public AppXViewModel(INavigationManager navigation)
        {
            _navigation = navigation;
            GoBack = new DelegateCommand(() => _navigation.GoBack());
            Load();
        }

        public async Task Load()
        {
            try
            {
                var f = new FileOpenPicker()
                    {
                        FileTypeFilter = { ".appx" }
                    };

                var file = await f.PickSingleFileAsync();
                if (file == null)
                    return;


                var model = new AppXModel();
                var zip = new ZipArchive((await file.OpenReadAsync()).AsStream(), ZipArchiveMode.Read);
                var manifest = zip.GetEntry("AppxManifest.xml");

                XNamespace ns = "http://schemas.microsoft.com/appx/2010/manifest";
                var doc = XDocument.Load(manifest.Open());
                var package = doc.Element(ns + "Package");
                var application = package.Element(ns + "Applications").Element(ns + "Application");
                var visuals = application.Element(ns + "VisualElements");

                model.DisplayName = visuals.Attribute("DisplayName").Value;
                model.Logo = visuals.Attribute("Logo").Value;
                model.SmallLogo = visuals.Attribute("SmallLogo").Value;

                var logoFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(model.DisplayName + ".png", CreationCollisionOption.ReplaceExisting);
                await zip.GetEntry(model.Logo.Replace('\\', '/')).SaveToFile(logoFile);
                model.LogoImage = new BitmapImage(new Uri(logoFile.Path));

                var smalllogoFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(model.DisplayName + "small.png", CreationCollisionOption.ReplaceExisting);
                await zip.GetEntry(model.SmallLogo.Replace('\\', '/')).SaveToFile(smalllogoFile);
                model.SmallLogoImage = new BitmapImage(new Uri(smalllogoFile.Path));

               
                model.Background = new SolidColorBrush(ColorExtensions.FromString(visuals.Attribute("BackgroundColor").Value));

                Model = model;
            }
            catch (Exception o_0)
            {

            }
        }
    }
}
