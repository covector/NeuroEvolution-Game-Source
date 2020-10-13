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
    /*public R2Tensor ForwardPropagation(R2Tensor Input)
    {
        R2Tensor Hidden = Sigmoid(R2Tensor.Add(R2Tensor.MatMul(ih_weight, Input), ih_bias));
        R2Tensor Output = Sigmoid(R2Tensor.Add(R2Tensor.MatMul(ho_weight, Hidden), ho_bias));
        return Output;
    }*/
    #endregion
    #region additional code
    string outtype;
    public NeuralNetwork(int InputLayer, int HiddenLayer, int OutputLayer, string Outtype)
    {
        input = InputLayer;
        hidden = HiddenLayer;
        output = OutputLayer;
        ih_weight = new R2Tensor(HiddenLayer, InputLayer);
        ih_bias = new R2Tensor(HiddenLayer, 1);
        ho_weight = new R2Tensor(OutputLayer, HiddenLayer);
        ho_bias = new R2Tensor(OutputLayer, 1);
        outtype = Outtype;
    }
    public NeuralNetwork(R2Tensor ihWeight, R2Tensor ihBias, R2Tensor hoWeight, R2Tensor hoBias, int InputLayer, int HiddenLayer, int OutputLayer, string Outtype)
    {
        input = InputLayer;
        hidden = HiddenLayer;
        output = OutputLayer;
        ih_weight = ihWeight.Copy();
        ih_bias = ihBias.Copy();
        ho_weight = hoWeight.Copy();
        ho_bias = hoBias.Copy();
        outtype = Outtype;
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
        switch (outtype)
        {
            case "Basisdirection":
                return Tanh(Output);
            case "Direction":
                int maxind = Max(Output);
                float[] tensor_out = new float[9];
                for (int i = 0; i < 9; i++)
                {
                    if (i == maxind) { tensor_out[i] = 1; continue; }
                    tensor_out[i] = 0;
                }
                return R2Tensor.ToMatrix(tensor_out, "col");
            case "Keystroke":
                return Sign(Output);
            case "Debug":
                return Output;
            default:
                return Output;
        }
    }
    public void Mutate(float mutationrate, float magnitude, float clamping)
    {
        Mutate(ih_weight, mutationrate, magnitude, clamping);
        Mutate(ho_weight, mutationrate, magnitude, clamping);
        Mutate(ih_bias, mutationrate, magnitude, clamping);
        Mutate(ho_bias, mutationrate, magnitude, clamping);
    }
    public void Mutate(R2Tensor Tensor, float mutationrate, float magnitude, float clamping)
    {
        for (int i = 0; i < Tensor.row; i++)
        {
            for (int j = 0; j < Tensor.col; j++)
            {
                if (mutationrate > Random.Range(0f, 1f))
                {
                    Tensor.tensor[i, j] += Random.Range(-1f * magnitude, magnitude);
                }
                if (Tensor.tensor[i, j] > clamping) { Tensor.tensor[i, j] = clamping; }
                if (Tensor.tensor[i, j] < -1 * clamping) { Tensor.tensor[i, j] = -1 * clamping; }
            }
        }
    }
    public NeuralNetwork Copy()
    {
        NeuralNetwork nn_out = new NeuralNetwork(input, hidden, output, outtype);
        nn_out.ih_weight = ih_weight.Copy();
        nn_out.ih_bias = ih_bias.Copy();
        nn_out.ho_weight = ho_weight.Copy();
        nn_out.ho_bias = ho_bias.Copy();
        return nn_out;
    }
    #endregion
}