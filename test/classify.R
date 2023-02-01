require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";
imports "dataset" from "MLkit";

data("bezdekIris");

data = bezdekIris 
|> toFeatureSet() 
|> dataset::encoding() 
|> as.data.frame();

print(data, max.print = 6);

stop();

tensor(model = ANN)
|> feed(data, features = ["D1","D2","D3","D4"])
|> hidden_layer([10, 25, 15], activate = activateFunction::sigmoid(alpha = 2.0))
|> output_layer(labels = ["","","",""], activate = activateFunction::sigmoid(alpha = 2.0))
|> learn()
# |> snapshot(file = "./model.hds")
# ;

# tensor(model = "./model.hds")
|> solve(data)
|> print()
;

