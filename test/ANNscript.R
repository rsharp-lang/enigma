require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

data = read.csv("");

tensor(model = ANN)
|> feed(data, features = ["","",""])
|> hidden_layer([50, 30, 20], activate = "Sigmoid(alpha = 2.0)")
|> output_layer(activate = activateFunction::func(
    forward = x -> log(x ^ 2), 
    derivative = x -> 1 / (2 * x)
))
|> learn()
|> snapshot(file = "./model.hds")
;

validates = read.csv("");

tensor(model = "./model.hds")
|> solve(validates)
|> print()
;

