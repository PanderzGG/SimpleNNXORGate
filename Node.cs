using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNN
{
    public class Node
    {

        public Dictionary<Node, double> InputNodes { get; set; } = new Dictionary<Node, double>();

        public double Result { get; set; }

        public Func<double, double> Sigmoid { get; set; }

        public Node(Func<double, double> sigmoid, double result = 0)
        {
            Sigmoid = sigmoid;
            Result = result;
        }

        public void AddInputNode(Node node)
        {
            InputNodes.Add(node, 0.00001);
        }

        public void CalcNode()
        {
            foreach(var valuePair in InputNodes)
            {
                var node = valuePair.Key;
                var weight = valuePair.Value;

                Result += node.Sigmoid(weight * node.Result);

                if(double.IsNaN(Result) || double.IsInfinity(Result) || double.IsNegativeInfinity(Result))
                {
                    throw new Exception("Bad result!");
                }
                
            }

            Result = Result / Math.Max(1, InputNodes.Count);
        }
    }
}
