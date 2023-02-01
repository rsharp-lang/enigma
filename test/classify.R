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

# stop();

tensor(model = ANN)
|> feed(data, features = ["D1","D2","D3","D4"])
|> hidden_layer([10, 50, 5], activate = activateFunction::sigmoid(alpha = 2.0))
|> output_layer(labels = ["class.Iris-setosa","class.Iris-versicolor","class.Iris-virginica"], activate = activateFunction::sigmoid(alpha = 2.0))
|> learn()
# |> snapshot(file = "./model.hds")
# ;

# tensor(model = "./model.hds")
|> solve(data)
|> write.csv(file = "./bezdekIris_class.csv")
;

