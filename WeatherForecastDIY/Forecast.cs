using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace WeatherForecastDIY
{
    /// <summary>
    /// 天气预报类
    /// </summary>
    class Forecast:INotifyPropertyChanged
    {
        private string city;//城市名
        private string currentTemperature;//实时的温度
        private bool forecastSuccess;//是否预测成功
        private Visibility vis;
        public string City
        {
            set
            {
                if (city != value)
                {
                    city = value;
                    OnpropertyChanged("City");
                }
            }
            get
            {
                return city;
            }
        }
        public string CurrentTemperature
        {
            set
            {
                if (currentTemperature != value)
                {
                    currentTemperature = value;
                    OnpropertyChanged("CurrentTemperature");
                }
            }
            get
            {
                return currentTemperature;
            }
        }
        public ForecastPeriod TodayForecast//当天的天气
        {
            set;
            get;
        }
        public Visibility Vis
        {
            set
            {
                if(value!=vis)
                {
                    vis = value;
                    OnpropertyChanged("Vis");
                }
            }
            get
            {
                return vis;
            }
        }
        public ObservableCollection<ForecastPeriod> ForecastList { set; get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CityName"></param>
        public Forecast(string CityName)
        {
            City = CityName;
            ForecastList = new ObservableCollection<ForecastPeriod>();
            TodayForecast=new ForecastPeriod();
        }
        public Forecast()
        {
            City = "成都";
            ForecastList = new ObservableCollection<ForecastPeriod>();
            TodayForecast = new ForecastPeriod();
        }
        /// <summary>
        /// 异步获取天气预报
        /// </summary>
        /// <returns></returns>
        private async void getForecastAsync()
        {
            Vis = Visibility.Visible;

            ObservableCollection<ForecastPeriod> newForecastList = new ObservableCollection<ForecastPeriod>();

            Uri uri = new Uri("http://api.map.baidu.com/telematics/v3/weather?location=" + City + "&output=xml&ak=HyXsPscHkQOfR0nxyUmYrV8l&date=" + System.DateTime.Now.ToString());
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();

            XElement now = XElement.Load(stream);
            httpClient.Dispose();

            if(now.Element("status").Value.Equals("success"))
            {
                forecastSuccess = true;
                now = now.Element("results").Element("weather_data").Element("date");
                ForecastPeriod newForecastPeriod = new ForecastPeriod();
                while (now != null) 
                {
                    string nowName=now.Name.LocalName;
                    if (nowName.Equals("date"))
                    {
                        string tmp = now.Value;
                        if (tmp.Length > 2)
                        {
                            newForecastPeriod.Date = tmp.Substring(0, 2);
                            int p = -1;
                            for (int i = 0; i < tmp.Length; i++) if (tmp[i] == '：') { p = i; break; }
                            CurrentTemperature = tmp.Substring(p + 1, tmp.Length - p - 2);
                        }
                        else
                            newForecastPeriod.Date = now.Value;
                    }
                    else if (nowName.Equals("dayPictureUrl"))
                        newForecastPeriod.DayPictureUrl = now.Value;
                    else if (nowName.Equals("weather"))
                        newForecastPeriod.Weather = now.Value;
                    else if (nowName.Equals("wind"))
                        newForecastPeriod.Wind = now.Value;
                    else if (nowName.Equals("temperature"))
                    {
                        newForecastPeriod.Temperature = now.Value;
                        newForecastList.Add(newForecastPeriod);
                        newForecastPeriod = new ForecastPeriod();
                    }
                    now = (XElement)now.NextNode;
                }
                TodayForecast.Weather = newForecastList[0].Weather;
                TodayForecast.Wind = newForecastList[0].Wind;
                TodayForecast.MaxTemperature = newForecastList[0].MaxTemperature;
                TodayForecast.MinTemperature = newForecastList[0].MinTemperature;
                
                foreach (ForecastPeriod forecastPeriod in newForecastList)
                    ForecastList.Add(forecastPeriod);
            }
            else
            {
                forecastSuccess = false;
            }
            Vis = Visibility.Collapsed;
        }
        public bool getForecast()
        {
            getForecastAsync();
            return forecastSuccess;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnpropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
