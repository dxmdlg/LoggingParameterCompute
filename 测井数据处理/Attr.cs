using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace 测井数据处理
{
    /// <summary>
    /// 用来数据绑定的类
    /// </summary>
    public class Attr 
    {
        private SolidColorBrush backgroundColor = new SolidColorBrush(Color.FromRgb(204, 213, 240));

        private LoggingParameter _loggingParameter = null;

        public LoggingParameter LoggingParameters
        {
            get {
                return _loggingParameter; }
            set {
                _loggingParameter = value;
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
    }
}
