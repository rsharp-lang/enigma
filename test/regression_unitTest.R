require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

let x = 1:1000;
let y = x ^ 1.25;
let z = y / x;

data = data.frame(x,y,z, row.names = as.character(x));

print(data, max.print = 6);

tensor(model = model::xgboost)
|> feed(data, features = ["x", "y"])
|> output(labels = "z")
|> learn()
|> solve(data)
|> print()
;

