using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WeatherForecastDIY
{
    /// <summary>
    /// 天气预报数据类
    /// </summary>
    class ForecastPeriod:INotifyPropertyChanged
    {
        private string weather;//天气
        private string wind;//风力
        private string maxTemperature;//最高温
        private string minTemperature;//最低温
        private string date;//日期
        private string dayPictureUrl;//天气图片
        private string temperature;//温度
        public string Weather
        {
            set
            {
                if (value != weather)
                {
                    weather = value;
                    OnpropertyChanged("Weather");
                }
            }
            get
            {
                return weather;
            }
        }
        public string Wind
        {
            set
            {
                if (value != wind)
                {
                    wind = value;
                    OnpropertyChanged("Wind");
                }
            }
            get
            {
                return wind;
            }
        }
        public string MaxTemperature
        {
            set
            {
                if (value != maxTemperature)
                {
                    maxTemperature = value;
                    OnpropertyChanged("MaxTemperature");
                }
            }
            get
            {
                analysisTemperature();
                return maxTemperature;
            }
        }
        public string MinTemperature
        {
            set
            {
                if (value != minTemperature)
                {
                    minTemperature = value;
                    OnpropertyChanged("MinTemperature");
                }
            }
            get
            {
                analysisTemperature();
                return minTemperature;
            }
        }
        public string Date
        {
            set
            {
                if (value != date)
                {
                    date = value;
                    OnpropertyChanged("Date");
                }
            }
            get
            {
                return date;
            }
        }
        public string DayPictureUrl
        {
            set
            {
                if (dayPictureUrl != value)
                {
                    dayPictureUrl = value;
                    OnpropertyChanged("DayPictureUrl");
                }
            }
            get
            {
                return dayPictureUrl;
            }
        }
        public string Temperature
        {
            set
            {
                if (value != temperature)
                {
                    temperature = value;
                    OnpropertyChanged("Temperature");
                }
            }
            get
            {
                return temperature;
            }
        }
        /// <summary>
        /// 分析温度字符串，得到最高最低温
        /// </summary>
        private void analysisTemperature()
        {
            if (Temperature == null)
                return;
            int p = -1;//波浪号的位置
            for (int i = 0; i < Temperature.Length; i++) 
                if (Temperature[i] == '~') { p = i; break; }
            if(p==-1)//没有波浪号
            {
                maxTemperature = Temperature;
                minTemperature = Temperature;
            }
            else
            {
                maxTemperature = Temperature.Substring(0, p) + "℃";
                minTemperature = Temperature.Substring(p + 1, Temperature.Length - p - 1);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnpropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler!=null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
