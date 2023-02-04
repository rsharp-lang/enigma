require(enigma);
require(JSON);

imports ["learning", "model", "activateFunction"] from "enigma";
imports "dataset" from "MLkit";

setwd(@dir);

data("bezdekIris");

print(dataset::description(bezdekIris));

rownames(bezdekIris) = unique.names(bezdekIris$class);

data = bezdekIris 
|> toFeatureSet() 
|> dataset::encoding(class = to_factors) 
|> as.data.frame();

print(data, max.print = 6);

i = data[, "class.Iris-setosa"] == 1;
v = rep("", length(i));
v[i] = "Iris-setosa";
v[!i] = "Other";

data[, "class.Iris-setosa"] = v;

tensor(model = model::svm)
|> feed(data, features = ["D1","D2","D3","D4"])
|> output(labels = ["class.Iris-setosa"])
|> learn()
# |> snapshot(file = "./model.hds")
# ;

# tensor(model = "./model.hds")
|> solve(data)
|> write.csv(file = "./bezdekIris_svm_class.csv", row.names = TRUE)
;

