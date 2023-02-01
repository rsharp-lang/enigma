require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

data("mtcars");

data = mtcars;

print(data, max.print = 6);

tensor(model = ANN)
|> feed(data, features = ["cyl", "disp", "hp", "drat", "wt", "qsec", "vs", "am", "gear", "carb"])
|> hidden_layer([13, 25, 15], activate = activateFunction::sigmoid(alpha = 2.0))
|> output_layer(labels = "mpg", activate = activateFunction::func(
    forward    = x -> log(x ^ 2), 
    derivative = x -> 1 / (2 * x)
))
|> learn()
|> snapshot(file = "./model.hds")
;

tensor(model = "./model.hds")
|> solve(mtcars)
|> print()
;

