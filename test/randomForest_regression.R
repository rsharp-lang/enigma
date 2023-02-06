require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

setwd(@dir);

let x = 1:1000;
let y = x ^ 1.25 + runif(n = length(x));
let a = (x ^ 2) * runif(n = length(x));
let b = a / 2 + y;
let c = 1 + runif(n = length(x));
let z = y / x + b;

data = data.frame(x, y, a, b, c, z, row.names = as.character(x));

print(data, max.print = 6);
cat("\n\n\n");

test = tensor(model = model::randomForest(regression = TRUE))
|> feed(data, features = ["x", "y","a","b","c"])
|> output(labels = "z")
|> learn()
|> solve(data)
;

print(test);

test[, "z"] = data$z;
test[, "errors"] = abs(test$z - test[, "z(predicts)"]);

i = order(test$errors);
test = test[i, ];

cat("\n\n\n");
print(test, max.print = 6);
write.csv(test, file = `${@dir}/randomForest_regression_test.csv`, row.names = TRUE);

