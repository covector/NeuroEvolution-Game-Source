using UnityEngine;
[System.Serializable]
public class NeuralNetwork
{
    #region normal code
    int input;

    public R2Tensor ih_weight;
    public R2Tensor ih_bias;

    int hidden;

    public R2Tensor ho_weight;
    public R2Tensor ho_bias;

    int output;

    public NeuralNetwork(int InputLayer, int HiddenLayer, int OutputLayer)
    {
        input = InputLayer;
        hidden = HiddenLayer;
        output = OutputLayer;
        ih_weight = new R2Tensor(HiddenLayer, InputLayer);
        ih_bias = new R2Tensor(HiddenLayer, 1);
        ho_weight = new R2Tensor(OutputLayer, HiddenLayer);
        ho_bias = new R2Tensor(OutputLayer, 1);
    }
    private static R2Tensor Sigmoid(R2Tensor Tensor)
    {
        float[,] tensor_out = new float[Tensor.row, Tensor.col];
        for (int i = 0; i < Tensor.row; i++)
        {
            tensor_out[i, 0] = Sigmoid(Tensor.tensor[i, 0]);
        }
        return new R2Tensor(tensor_out);
    }
    private static R2Tensor Tanh(R2Tensor Tensor)
    {
        float[,] tensor_out = new float[Tensor.row, Tensor.col];
        for (int i = 0; i < Tensor.row; i++)
        {
            tensor_out[i, 0] = Tanh(Tensor.tensor[i, 0]);
        }
        return new R2Tensor(tensor_out);
    }
    private static R2Tensor Sign(R2Tensor Tensor)
    {
        float[,] tensor_out = new float[Tensor.row, Tensor.col];
        for (int i = 0; i < Tensor.row; i++)
        {
            tensor_out[i, 0] = Mathf.Sign(Tensor.tensor[i, 0]);
        }
        return new R2Tensor(tensor_out);
    }
    private static float Sigmoid(float number) { Debug.Log(1 / (1 + Mathf.Exp(-1 * number))); return 1 / (1 + Mathf.Exp(-1 * number)); }
    private static float Tanh(float number)
    {
        float exp = Mathf.Exp(number);
        float invexp = Mathf.Exp(-1 * number);
        return (exp - invexp) / (exp + invexp);
    }
    private static R2Tensor Softmax(R2Tensor Tensor)
    {
        float[,] tensor_out = new float[Tensor.row, Tensor.col];
        float total = 0;
        for (int i = 0; i < Tensor.row; i++)
        {
            total += Mathf.Exp(Tensor.tensor[i, 0]);
            tensor_out[i, 0] = Mathf.Exp(Tensor.tensor[i, 0]);
        }
        return new R2Tensor(tensor_out).Scale(1 / total);
    }
    #endregion
    #region additional code
    public NeuralNetwork(R2Tensor ihWeight, R2Tensor ihBias, R2Tensor hoWeight, R2Tensor hoBias, int InputLayer, int HiddenLayer, int OutputLayer)
    {
        input = InputLayer;
        hidden = HiddenLayer;
        output = OutputLayer;
        ih_weight = ihWeight.Copy();
        ih_bias = ihBias.Copy();
        ho_weight = hoWeight.Copy();
        ho_bias = hoBias.Copy();
    }
    private int Max(R2Tensor tensor)
    {
        int intout = 0;
        float max = 0;
        for (int i = 0; i < tensor.row; i++)
        {
            int currin = intout;
            if (tensor.tensor[i, 0] > max) { currin = i; }
            intout = currin;
        }
        return intout;
    }
    public R2Tensor ForwardPropagation(R2Tensor Input, bool norm = true)
    {
        R2Tensor Hidden = Tanh(R2Tensor.Add(R2Tensor.MatMul(ih_weight, Input), ih_bias));
        if (!norm) { return Hidden; }
        R2Tensor Output = R2Tensor.Add(R2Tensor.MatMul(ho_weight, Hidden), ho_bias);
        return Sign(Output);
    }
    #endregion
}