require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

data("bezdekIris");

data = bezdekIris;

print(data, max.print = 6);

tensor(model = ANN)
|> feed(data, features = ["D1","D2","D3","D4"])
|> hidden_layer([10, 25, 15], activate = "Sigmoid(alpha = 2.0)")
|> output_layer(activate = activateFunction::func(
    forward = x -> log(x ^ 2), 
    derivative = x -> 1 / (2 * x)
))
|> learn()
|> snapshot(file = "./model.hds")
;

tensor(model = "./model.hds")
|> solve(bezdekIris)
|> print()
;

