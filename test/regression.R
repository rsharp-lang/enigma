require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

data("mtcars");

data = mtcars;

for(i in ["disp", "hp", "drat", "wt", "qsec"]) {
    data[, i] = data[, i] / max(data[, i]);
}

print(data, max.print = 6);

tensor(model = ANN)
|> feed(data, features = ["disp", "hp", "drat", "wt", "qsec"])
|> hidden_layer([13, 51, 5], activate = activateFunction::qlinear(truncate = 100000000.0))
|> output_layer(labels = "mpg", activate = activateFunction::qlinear(truncate = 100000000.0))
|> learn(truncate = -1, threshold = 3)
# |> snapshot(file = "./model.hds")
# ;
# tensor(model = "./model.hds")
|> solve(data)
|> print()
;

