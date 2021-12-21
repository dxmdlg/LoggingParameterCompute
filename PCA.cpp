//PCA方法

#include <iostream>
#include <fstream>
#include <Eigen/Dense>
#include <string>

using namespace std;
using namespace Eigen;

//读取矩阵
MatrixXd read_date(string filepath) {
	//打开文件
	ifstream ifs;
	ifs.open(filepath, ios::in);
	cout << "Reading XYZ File: " << filepath << endl;
	if (!ifs.is_open())
	{
		cout << "File Cannot Open" << endl;
		exit(1);
	}
	cout << "打开成功" << endl;
	string tmp;
	int rows = 0;
	while (getline(ifs, tmp, '\n')) { //计算文件有多少行
		if (tmp.size() > 0)
			rows++;
	}
	ifs.close();
	ifs.open(filepath, ios::in);
	int cols = 6;
	MatrixXd mat(rows, cols);
	for (int i = 0; i < rows; i++) {
		for (int j = 0; j < cols; j++) {
			ifs >> mat(i, j);
		}
	}
	ifs.close();
	return mat;
}

//z-score归一化
void data_normalization(MatrixXd& X)
{
	RowVectorXd meanvecRow = X.colwise().mean();
	X.rowwise() -= meanvecRow;
	RowVectorXd variance = (X.array() * X.array()).colwise().sum() / X.rows();
	variance = variance.array().sqrt();
	for (int i = 0; i < X.cols(); i++) {
		for (int j = 0; j < X.rows(); j++) {
			X(j, i) /= variance.array()[i];
		}
	}
}

//计算协方差矩阵
void computeCov(MatrixXd& X, MatrixXd& C)
{
	//计算每一维度均值
	RowVectorXd meanvecRow = X.colwise().mean();
	//样本均值化为0
	X.rowwise() -= meanvecRow;
	//计算协方差矩阵C = XTX / n-1;
	C = X.transpose() * X;
	C = C.array() / (X.rows() - 1);
}

//计算特征值特征向量
void computeEig(MatrixXd& C, MatrixXd& vec, MatrixXd& val)
{
	//计算特征值和特征向量，使用selfadjont按照对阵矩阵的算法去计算，可以让产生的vec和val按照有序排列
	SelfAdjointEigenSolver<MatrixXd> eig(C);
	vec = eig.eigenvectors();
	val = eig.eigenvalues();
	cout << "特征值" << val << endl << "特征向量" << vec << endl;
	//上面算出的是从小到大排，现在把特征值按照从大到小排
	for (int i = 0, j = 4; i < j; i++, j--) {
		vec.col(i).swap(vec.col(j));
		val.row(i).swap(val.row(j));
	}
}


MatrixXd computeResult(MatrixXd& mat, MatrixXd& vec, int dim)
{
	ofstream fout;
	fout.open("matrix_data.txt", ios::app);
	MatrixXd m = vec.leftCols(3);
	MatrixXd res = mat * m;
	fout << "使用的变换矩阵为:" << endl << vec.leftCols(5) << endl;
	fout.close();
	return res;
}

//PCA方法
MatrixXd pca(MatrixXd mat, int dim)
{
	//C存协方差矩阵
	MatrixXd C;
	ofstream fout;
	fout.open("output5.txt", ios::out);

	data_normalization(mat);
	MatrixXd mat1 = mat.leftCols(5);
	//计算协方差矩阵
	computeCov(mat1, C);

	//计算特征值特征向量
	MatrixXd vec, val;
	computeEig(C, vec, val);

	//按照指定特征值数降维数据
	MatrixXd res = computeResult(mat1, vec, dim);
	fout << res;
	fout.close();
	return res;
}

int main()
{
	string filepath = "C:\\Users\\19870\\Desktop\\测井数据.txt";

	ofstream fout;
	FILE* fp1;
	fp1 = fopen("降维的数据为.txt", "w");
	MatrixXd mat = read_date(filepath);
	MatrixXd res = pca(mat, 3);
	for (int i = 0; i < res.rows(); i++) {
		for (int j = 0; j < res.cols(); j++) {
			double a = res(i, j);
			fprintf(fp1, "%10.6f", a);
		}
		fprintf(fp1, "%c", '\n');
	}
	fclose(fp1);
	system("pause");
	return 0;
}
