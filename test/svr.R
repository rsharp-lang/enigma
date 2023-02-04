require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

let x = 1:1000;
let y = x ^ 1.25 + runif(n = length(x));
let z = y / x;

data = data.frame(x, y, z, row.names = as.character(x));

print(data, max.print = 6);
cat("\n\n\n");

test = tensor(model = model::svr)
|> feed(data, features = ["x", "y"])
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
write.csv(test, file = `${@dir}/svr_test.csv`, row.names = TRUE);

