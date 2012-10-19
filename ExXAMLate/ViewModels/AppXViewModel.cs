using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ExXAMLate.Common;
using ExXAMLate.Models;
using Okra.Core;
using Okra.Navigation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ExXAMLate.ViewModels
{
    public class AppXViewModel : NotifyPropertyChangedBase
    {
        public ObservableCollection<IGroup> Groups { get; set; }
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
            Groups = new ObservableCollection<IGroup> { new AppXHeaderGroup() };
            GoBack = new DelegateCommand(() => _navigation.GoBack());
            Load();
        }

        public async Task Load()
        {
            var model = new AppXModel();
            try
            {
                Windows.UI.ViewManagement.ApplicationView.TryUnsnap();

                var f = new FileOpenPicker
                    {
                        FileTypeFilter = { ".appx" }
                    };

                var file = await f.PickSingleFileAsync();
                if (file == null)
                {
                    _navigation.GoBack();
                    return;
                }

                var zip = new ZipArchive((await file.OpenReadAsync()).AsStream(), ZipArchiveMode.Read);
                var entries = zip.Entries.ToList();
                var manifest = zip.GetEntry("AppxManifest.xml");

                XNamespace ns = "http://schemas.microsoft.com/appx/2010/manifest";
                var doc = XDocument.Load(manifest.Open());
                var package = doc.Element(ns + "Package");
                var application = package.Element(ns + "Applications").Element(ns + "Application");
                var visuals = application.Element(ns + "VisualElements");

                model.DisplayName = visuals.Attribute("DisplayName").Value;
                var c = ColorExtensions.FromString(visuals.Attribute("BackgroundColor").Value);
                model.Background = new SolidColorBrush(c);
                model.ForegroundText = visuals.Attribute("ForegroundText").Value;

                model.ShouldChangeForeground = false;
                var brightness = Hammer.Pants.ColorExtensions.GetBrightness(c);
                if (brightness > 0.5 && model.ForegroundText == "light")
                {
                    model.ShouldChangeForeground = true;
                }
                else if (brightness < 0.5 && model.ForegroundText == "dark")
                {
                    model.ShouldChangeForeground = true;
                }
                model.Description = visuals.Attribute("Description").Value;

                var logo = new AppXIcon { Description = "The \"regular\" logo is displayed as your square tile. It measures 150x150.", Title = "Logo" };
                if (visuals.Attribute("Logo") != null)
                    await GetImage(zip, model.DisplayName + ".png", visuals.Attribute("Logo").Value, logo, entries);

                var smallLogo = new AppXIcon { Description = "The small logo is displayed in search results, \"show all apps\", and in the search charm (vertical list). It measures 30x30.", Title = "Small Logo" };
                if (visuals.Attribute("SmallLogo") != null)
                    await GetImage(zip, model.DisplayName + "small.png", visuals.Attribute("SmallLogo").Value, smallLogo, entries);

                var storeLogo = new AppXTile { Description = "The store logo is visible on the Windows Store on the web and within the Windows Store app. It measures 50x50.", Title = "Store Logo", AppTitle = model.DisplayName };
                if (package.Element(ns + "Properties").Element(ns + "Logo") != null)
                    await GetImage(zip, model.DisplayName + "store.png", package.Element(ns + "Properties").Element(ns + "Logo").Value, storeLogo, entries);
                
                var wideLogo = new AppXIcon { Description = "The wide logo is an optional, double width tile. It measures 320x150.", Title = "Wide Logo" };
                if (visuals.Element(ns + "DefaultTile") != null && visuals.Element(ns + "DefaultTile").Attribute("WideLogo") != null)
                    await GetImage(zip, model.DisplayName + "wide.png", visuals.Element(ns + "DefaultTile").Attribute("WideLogo").Value, wideLogo, entries);

                var splash = new AppXSplash { Description = "", Title = "Splash Screen" };
                if (visuals.Element(ns + "SplashScreen").Attribute("Image") != null)
                    await GetImage(zip, model.DisplayName + "splash.png", visuals.Element(ns + "SplashScreen").Attribute("Image").Value, splash, entries);

                if (visuals.Element(ns + "SplashScreen").Attribute("BackgroundColor") != null)
                    splash.Background = new SolidColorBrush(ColorExtensions.FromString(visuals.Element(ns + "SplashScreen").Attribute("BackgroundColor").Value));
                else
                    splash.Background = model.Background;
                
                var icons = new AppXIconGroup("Icons");
                icons.Items.Add(smallLogo);
                icons.Items.Add(storeLogo);
                icons.Items.Add(logo);
                if (wideLogo.Image != null)
                    icons.Items.Add(wideLogo);
                icons.Items.Add(splash);

                Groups.Add(icons);
            }
            catch (Exception o_0)
            {
                MarkedUp.AnalyticClient.Info("Error parsing appx", o_0);
            }

            Model = model;
        }

        private static async Task GetImage(ZipArchive zip, string displayname, string path, AppXIcon icon, IEnumerable<ZipArchiveEntry> entries)
        {
            try
            {
                path = path.Replace('\\', '/');

                var item = entries.FirstOrDefault(z => z.FullName == path);
                if (item == null)
                {
                    var ext = Path.GetExtension(path);
                    path = string.Format("{0}/{1}.scale-{2}{3}", Path.GetDirectoryName(path).Replace('\\', '/'), Path.GetFileNameWithoutExtension(path), (int)DisplayProperties.ResolutionScale, ext);
                }

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(displayname, CreationCollisionOption.ReplaceExisting);
                await zip.GetEntry(path).SaveToFile(file);
                icon.Image = new BitmapImage(new Uri(file.Path));
            }
            catch (Exception o_0)
            {
                MarkedUp.AnalyticClient.Info("Error retrieving image from appx", o_0);
            }
        }
    }
}
