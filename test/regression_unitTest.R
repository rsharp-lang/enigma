require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

let x = 1:1000;
let y = x ^ 1.25;
let z = y / x;

data = data.frame(x, y, z, row.names = as.character(x));

print(data, max.print = 6);

test = tensor(model = model::xgboost)
|> feed(data, features = ["x", "y"])
|> output(labels = "z")
|> learn(loss = "squareloss", cost = "mse")
|> solve(data)
;

test[, "errors"] = abs(test$z - test[, "z(predicts)"]);
i = order(test$errors);
test = test[i, ];

print(test);
write.csv(test, file = `${@dir}/regression_test.csv`, row.names = TRUE);

