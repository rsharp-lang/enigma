require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";
imports "dataset" from "MLkit";

setwd(@dir);

data("bezdekIris");

dataset::description(bezdekIris);

data = bezdekIris 
|> toFeatureSet() 
|> dataset::encoding(class = to_factors) 
|> as.data.frame();

print(data, max.print = 6);

stop();

tensor(model = model::svm)
|> feed(data, features = ["D1","D2","D3","D4"])
|> output(labels = ["class.Iris-setosa"])
|> learn()
# |> snapshot(file = "./model.hds")
# ;

# tensor(model = "./model.hds")
|> solve(data)
|> write.csv(file = "./bezdekIris_class.csv")
;

