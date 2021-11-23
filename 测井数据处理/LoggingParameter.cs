using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 测井数据处理
{
    /// <summary>
    /// 测井数据的参数类
    /// </summary>
    public class LoggingParameter
    {
        public double GR { get; set; }
        public double SP { get; set; }
        public double DT { get; set; }
        public double DEN { get; set; }
        public double CNL { get; set; }

        public double BI { get; set; }
        public LoggingParameter(double GR,double SP,double DT,double DEN,double CNL)
        {
            this.GR = GR;
            this.SP = SP;
            this.DT = DT;
            this.DEN = DEN;
            this.CNL = CNL;
        }

        public override string ToString()
        {
            return "参数为：GR"+GR+"--SP:"+SP + "--DT:" + DT + "--DEN:" + DEN+ "--CNL:" + CNL+"--BI:" + BI;
        }
    }
}
