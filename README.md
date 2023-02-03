## Regression Example

An example of solve a regression problem by use the xgboost regession from ``enigma`` package:

```r
require(enigma);

imports ["learning", "model", "activateFunction"] from "enigma";

# generate dataset
let x = 1:1000;
let y = x ^ 1.25 + runif(n = length(x));
let z = y / x;

data = data.frame(x, y, z, row.names = as.character(x));

print(data, max.print = 6);
cat("\n\n\n");

#                x        y        z
# -----------------------------------
# <mode> <integer> <double> <double>
# 1              1  1.99925  1.99925
# 2              2  3.34409  1.67205
# 3              3  4.40403  1.46801
# 4              4   5.8122  1.45305
# 5              5   7.9491  1.58982
# 6              6  10.1006  1.68343

#  [ reached 'max' / getOption("max.print") -- omitted 994 rows ]

test = tensor(model = model::xgboost)
|> feed(data, features = ["x", "y"])
|> output(labels = "z")
|> learn(loss = "squareloss", cost = "mse")
|> solve(data)
;

test[, "errors"] = abs(test$z - test[, "z(predicts)"]);
i = order(test$errors);
test = test[i, ];

cat("\n\n\n");
print(test, max.print = 6);
write.csv(test, file = "regression_test.csv", row.names = TRUE);

#                x        y        z z(predicts)       errors
# ------------------------------------------------------------
# <mode> <integer> <double> <double>    <double>     <double>
# 918          918  5053.48  5.50488     5.50487  3.06194E-06
# 696          696  3574.91  5.13637     5.13636  5.30553E-06
# 607          607  3012.96   4.9637     4.96369  7.10683E-06
# 826          826  4428.27  5.36111     5.36112  1.16761E-05
# 810          810  4322.19  5.33603     5.33604  1.23223E-05
# 879          879  4786.52  5.44542     5.44543  1.36714E-05

#  [ reached 'max' / getOption("max.print") -- omitted 994 rows ]
```