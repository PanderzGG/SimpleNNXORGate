using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNN
{
    public class Layer
    {
        public List<Node> Nodes { get; set; } = new List<Node>();

        public Layer(int width)
        {
            for(int i = 0; i < width; i++)
            {
                var newNode = new Node(
                    (double x) =>
                    {
                        // Math Sigmoid function ranges from 5 to -5. Auf Desmos nachschauen.
                        // Kann mit bedingungen (if) versehen werden und werte können angepasst werden.

                        return (10 / (1 + Math.Pow(Math.E, -1 * x))) - 5;
                    }
                    );

                Nodes.Add(newNode);
            }
        }
        
        public void CalcLayer()
        {
            foreach(var node in Nodes)
            {
                node.CalcNode();
            }
        }

        // Verbindungen zwischen den Node Ebenen.
        public void LinkLayer(Layer nextLayer)
        {
            foreach(var nextNode in nextLayer.Nodes)
            {
                foreach(var node in Nodes)
                {
                    // Momentane Nodes werden als InputNodes für die nächste Ebene benutzt
                    nextNode.AddInputNode(node);
                }
            }
        }

    }
}
