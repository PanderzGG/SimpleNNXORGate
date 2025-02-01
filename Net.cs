using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNN
{
    public class Net
    {
        public List<Layer> Layers = new List<Layer>();

        public Net(int inputSize, int outputSize, int depth)
        {
            // Input Layer
            // +2 weil wir 2 verschiedene Bias Nodes zu einer InputNode hinzufügen.
            var layer1 = new Layer(inputSize + 2);
            Layers.Add(layer1);

            // Middle Layer
            for(int i = 0; i < depth; i++)
            {
                var middleLayer = new Layer(3);
                Layers.Add(middleLayer);
            }

            // output Layer

            var outputLayer = new Layer(outputSize);
            Layers.Add(outputLayer);

            // Link Layers
            // Started bei 1 weil wir uns 2 items in der Liste gleichzeitig anschauen
            for(int i = 1; i < Layers.Count; i++)
            {
                var lastLayer = Layers[i - 1];
                var nextLayer = Layers[i];
                lastLayer.LinkLayer(nextLayer);
            }
        }

        public List<double> GetResults(List<double> inputs)
        {
            // Load inputs
            var firstLayer = Layers[0];
            var localInputs = new List<double>(inputs);
            localInputs.Add(-1);
            localInputs.Add(1);

            for(int i = 0; i < localInputs.Count; i++)
            {
                firstLayer.Nodes[i].Result = localInputs[i];
            }

            for(int i = 0; i < Layers.Count; i++)
            {
                Layers[i].CalcLayer();    
            }

            var lastLayer = Layers.Last();

            var results = lastLayer.Nodes.Select(node => node.Result).ToList();

            // Resulte Clear(), muss gemacht werden
            foreach (var layer in Layers)
            {
                foreach (var node in layer.Nodes)
                {
                    node.Result = 0;
                }
            }

            return results;
        }

        public void Train(List<Simulant> trainingData, double acceptableScore)
        {
            Random r = new Random(17);

            while (true) 
            {
                var layers = Layers.Skip(1);

                foreach(var layer in layers)
                {
                    foreach(var node in layer.Nodes)
                    {
                        foreach(var key in new List<Node> (node.InputNodes.Keys))
                        {
                            var originalScore = GetScore(trainingData);
                            var originalValue = node.InputNodes[key];
                            
                            // Machen eine random Veränderung
                            node.InputNodes[key] += r.NextDouble() < 0.5 ? -10 * r.NextDouble() : r.NextDouble() * 10;

                            var newScore = GetScore(trainingData);
                            
                            // Weniger ist besser
                            if(newScore < originalScore)
                            {
                                Console.WriteLine("Verbesserte Node!");
                            }
                            else
                            {
                                {
                                    // Zuruecksetzen auf originalen wert
                                    node.InputNodes[key] = originalValue;
                                }
                            }
                        }
                    }
                }

                var score = GetScore(trainingData);
                Console.WriteLine($"Scored: {score}");

                if(score < acceptableScore)
                {
                    Console.WriteLine("Training passed");
                    return;
                }
            }
        }

        private double GetScore(List<Simulant> trainingData)
        {
            var score = 0.0;

            foreach (var simulant in trainingData)
            {
                var results = this.GetResults(simulant.Inputs);

                for (int i = 0; i < results.Count; i++)
                {
                    var wantedOutput = simulant.WantedOutputs[i];
                    var actualOutput = results[i];
                    
                    // Gibt uns immer eine positive nummer zurück. Selbst wenn das ergebnis negativ ist.
                    score += Math.Abs(wantedOutput - actualOutput);
                }
            }
            return score;
        }
    }
}
