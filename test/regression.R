require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

data("mtcars");

data = mtcars;

print(data, max.print = 6);

tensor(model = ANN)
|> feed(data, features = ["cyl", "disp", "hp", "drat", "wt", "qsec", "vs", "am", "gear", "carb"])
|> hidden_layer([13, 25, 15], activate = activateFunction::identical)
|> output_layer(labels = "mpg", activate = activateFunction::identical)
|> learn(truncate = 50)
# |> snapshot(file = "./model.hds")
# ;
# tensor(model = "./model.hds")
|> solve(mtcars)
|> print()
;

