using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 测井数据处理
{
	public class Model
	{

		static double Relu(double input)
		{
			return input > 0 ? input : 0.1 * input;
		}
		static double Tanh(double input)
		{
			return Math.Tanh(input);
		}
		static double Sigmoid(double input)
		{
			return 1.0 / (1 + Math.Exp(-input));
		}
		static double Softmax(double[] z, int len, int index)
		{
			double sum = 0;
			for (int i = 0; i < len; i++)
			{
				sum += Math.Exp(z[i]);
			}
			return Math.Exp(z[index]) / sum;
		}
		static double Line(double input)
		{
			return input;
		}
		class Dense
		{
			public int inLen, NeuroNum, actType;//输入长度
			public double[,] W;  //本层权值
			public double[] bias;
			public double[] Z, DenseOut; //每个神经元对应的偏置

			public Dense(int NeuroNum, int actType, int inlen)
			{
				this.NeuroNum = NeuroNum;
				this.actType = actType;
				this.inLen = inlen;
				W = new double[inLen, NeuroNum];
				Z = new double[NeuroNum];
				DenseOut = new double[NeuroNum];
				bias = new double[NeuroNum];
			}
			public int getOutLen()
			{
				return this.NeuroNum;
			}
			public double[] getDenseOut()
			{
				return this.DenseOut;
			}

			public void forward(double[] inData)
			{

				for (int outNeuro = 0; outNeuro < this.NeuroNum; outNeuro++)
				{
					double sum = 0;
					for (int i = 0; i < this.inLen; i++)
					{
						sum += inData[i] * this.W[i, outNeuro];
					}
					sum += this.bias[outNeuro];
					this.Z[outNeuro] = sum;
				}
				for (int outNeuro = 0; outNeuro < this.NeuroNum; outNeuro++)
				{
					if (this.actType == 1)
					{ //relu
						this.DenseOut[outNeuro] = Relu(this.Z[outNeuro]);
					}
					else if (this.actType == 2)
					{ //tanh
						this.DenseOut[outNeuro] = Tanh(this.Z[outNeuro]);
					}
					else if (this.actType == 3)
					{ //sigmoid
						this.DenseOut[outNeuro] = Sigmoid(this.Z[outNeuro]);
					}
					else if (this.actType == 4)
					{ //softmax
						this.DenseOut[outNeuro] = Softmax(this.Z, this.NeuroNum, outNeuro);
					}
					else if (this.actType == 5)
					{ //line
						this.DenseOut[outNeuro] = Line(this.Z[outNeuro]);
					}
					else
					{
						//printf("未知的激活函数类型！\n");
						//exit(0);
					}
				}
			}
			//释放空间
			//void FreeSpace()
			//{
			//	for (int i = 0; i < this->inLen; i++)
			//	{
			//		free(this->W[i]);
			//	}
			//	free(this->W);
			//	free(this->bias);
			//	free(this->Z);
			//	free(this->DenseOut);
			//}
			public void LoadTXT(string WTxT, string BiasTxT)
			{
				//FILE* fpW,*fpB;
				//fpW = fopen(WTxT, "r");
				FileStream fs = new FileStream(WTxT, FileMode.Open);
				StreamReader sr = new StreamReader(fs);
				string line;
				int i = 0;

				while ((line = sr.ReadLine()) != null)
				{
					string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

					for (int j = 0; j < data.Length; j++)
					{
						W[i, j] = Convert.ToDouble(data[j]);
					}
					i++;
				}
				fs.Close();
				sr.Close();
				
				FileStream fs1 = new FileStream(BiasTxT, FileMode.Open);
				StreamReader sr1 = new StreamReader(fs1);
				string line1;
				while ((line1 = sr1.ReadLine()) != null)
				{
					string[] data = line1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < data.Length; j++)
					{
						bias[j] = Convert.ToDouble(data[j]);
					}
				}
				fs1.Close();
				sr1.Close();
			}
		};

		public static double predict(double[] input)
		{
			/*1.定义网络结构*/
			Dense layer1 = new Dense(20, 3, 3);
			Dense layer2 = new Dense(1, 3, 20);

			/*3.加载已经训练好的网络参数*/
			layer1.LoadTXT("layer1-W.txt", "layer1-bias.txt");
			layer2.LoadTXT("layer2-W.txt", "layer2-bias.txt");

			layer1.forward(input);
			layer2.forward(layer1.getDenseOut());

			double pred = layer2.DenseOut[0];

			return pred;
		}
	}
}
