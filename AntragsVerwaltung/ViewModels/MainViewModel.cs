using System.Configuration;
using System.Reflection;
using System.Windows;
using AntragsVerwaltung.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using AntragsVerwaltungCommonLibrary;

namespace AntragsVerwaltung.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<AntragItem> _Liste;
        private ICommand _OkCommand;
        private int _SelectedAntragId = 0;
        //private Visibility _ButtonVisibility;
        private string _Titel;
        //private string _Version = "";

        public Visibility ButtonVisibility
        {
            get { return SelectedAntragId > 0 ? Visibility.Visible : Visibility.Hidden; }
        }

        public int SelectedAntragId
        {
            get { return _SelectedAntragId; }
            set
            {
                _SelectedAntragId = value;
                AntragItem selectedAntrag = null;
                foreach (var x in _Liste)
                {
                    if (x.AntragId == _SelectedAntragId)
                    {
                        selectedAntrag = x;
                        break;
                    }
                }
                var n1 = selectedAntrag.Data.users;
                var n2 = n1;
                if (selectedAntrag != null)
                {
                    foreach (var item in selectedAntrag.Data.items)
                    {
                        if (item.status == "added")
                        {
                            n2++;
                        }
                        if (item.status == "deleted")
                        {
                            n2--;
                        }
                    }
                }
                Titel = string.Format("[{0}] >> Beantragte Änderungen durchführen >> [{1}]", n1, n2);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedAntragId"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ButtonVisibility"));
                }
            }
        }

        public string Titel
        {
            get { return _Titel; }
            set
            {
                _Titel = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Titel"));
                }
            }
        }

        public ObservableCollection<AntragItem> Liste
        {
            get { return _Liste; }
            set
            {
                _Liste = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Liste"));
                }
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                {
                    _OkCommand = new RelayCommand((parameter) => Confirm((int)parameter));
                }
                return _OkCommand;
            }
            set
            {
                _OkCommand = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Liste"));
                }
            }
        }

        public string Version
        {
            get
            {
                var v = this.GetType().Assembly.GetName().Version;
                return string.Format("Version {0}.{1}", v.Major, v.Minor);
            }
        }

        public MainViewModel()
        {
            bool inDesigner = DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (inDesigner)
            {
                LoadDataForDesigner();
            }
            else
            {
                LoadData();
            }
        }

        private void LoadDataForDesigner()
        {
            var liste = new List<AntragItem>()
            {
                new AntragItem() { AntragId=12, FormData="{\"mandantId\":101,\"datenbankId\":1,\"module\":\"AE\",\"datum\":\"17.12.2014 11:28:40\",\"absender\":\"41796827023\", \"mandantBezeichnung\":\"Gemeinde X\",\"datenbankBezeichnung\":\"Unique\", \"users\":3, \"items\":[{\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"ags\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":true,\"module\":\"\"}, {\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":false,\"module\":\"\"}]}" },
                new AntragItem() { AntragId=13, FormData="{\"mandantId\":102,\"datenbankId\":1,\"module\":\"\",\"datum\":\"19.12.2014 12:19:30\",\"absender\":\"41792270683\", \"mandantBezeichnung\":\"Gemeinde A\",\"datenbankBezeichnung\":\"HHRR\", \"users\":2, \"items\":[{\"status\":\"added\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":false,\"module\":\"AEGV\"}]}" },
                new AntragItem() { AntragId=14, FormData="{\"mandantId\":103,\"datenbankId\":2,\"module\":\"AGS\",\"datum\":\"21.12.2014 09:07:06\",\"absender\":\"41760682231\", \"mandantBezeichnung\":\"Gemeinde F\",\"datenbankBezeichnung\":\"DB2\", \"users\":6, \"items\":[{\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"dzz\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":false,\"module\":\"\"}, {\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":false,\"module\":\"A\"}, {\"status\":\"modified\",\"handyNummer\":\"41793353202\",\"shortName\":\"stfe2\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":true,\"module\":\"AEGK\"}, {\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":true,\"module\":\"\"}]}" },
                new AntragItem() { AntragId=15, FormData="{\"mandantId\":104,\"datenbankId\":1,\"module\":\"V\",\"datum\":\"21.12.2014 15:29:53\",\"absender\":\"41786823270\", \"mandantBezeichnung\":\"Gemeinde B\",\"datenbankBezeichnung\":\"Datenbank342\", \"users\":4, \"items\":[{\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"ioo\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":false,\"isGesperrt\":true,\"module\":\"\"}, {\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":false,\"module\":\"EV\"}, {\"status\":\"modified\",\"handyNummer\":\"41793353202\",\"shortName\":\"stfe2\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":true,\"module\":\"\"}, {\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":true,\"module\":\"\"}, {\"status\":\"modified\",\"handyNummer\":\"41792706824\",\"shortName\":\"stfe\",\"vorname\":\"Peter\",\"name\":\"Henrici\",\"isAdmin\":true,\"isGesperrt\":true,\"module\":\"\"}]}" }
            };
            Liste = new ObservableCollection<AntragItem>(liste);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void LoadData()
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("user-agent",
                    "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                try
                {
                    var response =
                        await httpClient.GetAsync(new Uri(ConfigurationManager.AppSettings["DialogPortal"] + "/Admin/GetAntraege", UriKind.Absolute));
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    var bytes = Encoding.Unicode.GetBytes(result);
                    using (var stream = new MemoryStream(bytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<AntragItem>));
                        var liste = (List<AntragItem>)serializer.ReadObject(stream);
                        Liste = new ObservableCollection<AntragItem>(liste);
                        if (_Liste.Count > 0)
                        {
                            _SelectedAntragId = _Liste[0].AntragId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        private async void Confirm(int antragId)
        {
            var dr = MessageBox.Show("Wollen sie den Antrag (" + antragId + ") wirklich abschicken?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dr != MessageBoxResult.Yes)
            {
                return;
            }
            bool inDesigner = DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (!inDesigner)
            {
                var httpClient = new HttpClient();
                using (httpClient)
                {
                    var ignoreList = new List<string>();
                    foreach (var antrag in _Liste)
                    {
                        foreach (var item in antrag.Data.items)
                        {
                            if (item.isIgnoriert)
                            {
                                ignoreList.Add(item.handyNummer);
                            }
                        }
                    }
                    var ignore = "";
                    if (ignoreList.Count > 0)
                    {
                        ignore = string.Join(",", ignoreList.ToArray());
                    }
                    httpClient.DefaultRequestHeaders.Add("user-agent",
                        "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    var uri = ConfigurationManager.AppSettings["DialogPortal"] + "/Admin/ConfirmAntrag?antragId=" + antragId + "&ignore=" + ignore;
                    var response =
                        await httpClient.GetAsync(new Uri(uri, UriKind.Absolute));
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    AntragItem itemToRemove = null;
                    if (result == "ok")
                    {
                        foreach (var item in _Liste)
                        {
                            if (item.AntragId == antragId)
                            {
                                itemToRemove = item;
                            }
                        }
                        if (itemToRemove != null)
                        {
                            _Liste.Remove(itemToRemove);
                        }
                    }
                    else
                    {
                        MessageBox.Show(result);
                        // Buhhh
                    }
                }
            }
            else
            {
                AntragItem itemToRemove = null;
                foreach (var item in _Liste)
                {
                    if (item.AntragId == antragId)
                    {
                        itemToRemove = item;
                    }
                }
                if (itemToRemove != null)
                {
                    _Liste.Remove(itemToRemove);
                }
            }
            if (_Liste.Count > 0)
            {
                SelectedAntragId = _Liste[0].AntragId;
            }
        }
    }
}
