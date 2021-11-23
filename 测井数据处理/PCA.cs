using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 测井数据处理
{
    internal class PCA
    {
        private double[][] matrix = new double[3][];
        public double[][] Matrix
        {
            get { return matrix; }
        }
        
        public PCA()
        {
            matrix[0] = new double[5] { 0.456621, 0.341676, -0.500487, 0.426808, -0.49204};
            matrix[1] = new double[5] { -0.259325, 0.807352, 0.220581, -0.407282, -0.25768};
            matrix[2] = new double[5] { 0.533571, -0.323078, 0.123549, -0.648945, -0.417767};
        }
    }
}
