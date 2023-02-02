require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

x = 1:1000;
y = x ^ 1.25;
z = y / x;

data = data.frame(x,y,z, row.names = as.character(x));

print(data, max.print = 6);

tensor(model = ANN)
|> feed(data, features = ["x", "y"])
|> hidden_layer([13, 51, 5], activate = activateFunction::qlinear(truncate = -1))
|> output_layer(labels = "z", activate = activateFunction::qlinear(truncate = -1))
|> learn(truncate = -1, threshold = 1)
# |> snapshot(file = "./model.hds")
# ;
# tensor(model = "./model.hds")
|> solve(data)
|> print()
;

