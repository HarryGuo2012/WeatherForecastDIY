using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastDIY
{
    /// <summary>
    /// 天气预报数据类
    /// </summary>
    class ForecastPeriod
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
                    weather = value;
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
                    wind = value;
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
                    maxTemperature = value;
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
                    minTemperature = value;
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
                    date = value;
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
                    dayPictureUrl = value;
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
                    temperature = value;
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
                minTemperature = Temperature.Substring(0, p) + "℃";
                maxTemperature = Temperature.Substring(p + 1, Temperature.Length - p - 1);
            }
        }
    }
}
