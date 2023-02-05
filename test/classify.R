require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";
imports "dataset" from "MLkit";

setwd(@dir);

data("bezdekIris");

data = bezdekIris 
|> toFeatureSet() 
|> dataset::encoding(class = to_factors) 
|> as.data.frame();

print(data, max.print = 6);

tensor(model = ANN)
|> feed(data, features = ["D1","D2","D3","D4"])
|> hidden_layer([50, 500, 5], activate = activateFunction::sigmoid(alpha = 2.0))
|> output(
    labels = ["class.Iris-setosa","class.Iris-versicolor","class.Iris-virginica"], 
    activate = activateFunction::sigmoid(alpha = 2.0)
)
|> learn(parallel = TRUE)
|> solve(data)
|> write.csv(file = "./bezdekIris_ANN_class.csv")
;

