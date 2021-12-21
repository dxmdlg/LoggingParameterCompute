using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 测井数据处理
{
    internal class Zscore
    {
        private double[] mean = new double[5];
        public double[] Mean
        {
            get { return mean; }
        }

        private double[] std = new double[5];
        public double[] Std
        {
            get { return std; }
        }

        public Zscore()
        {
            mean = new double[5] {75.492063, -3.777778, 206.650794, 2.515714, 12.714286};
            std = new double[5] { 12.349247, 9.106852, 8.573045, 0.164488, 4.613399};
        }
    }
}
