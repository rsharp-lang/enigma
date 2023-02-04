require(ggplot);

setwd(@dir);

plot_errors = function(data, title, file) {
    data[, "errors"] = abs(data$z.predicts. - data$z);

    print(data, max.print =6);

    bitmap(file = file, size = [2700, 1800]) {
        ggplot(data, aes(x = "errors"), padding = "padding:250px 400px 200px 250px;")
        + geom_histogram(bins = 250,  color = "steelblue", range = [0, 1])
        + ggtitle(title)
        + scale_x_continuous(labels = "F2")
        + scale_y_continuous(labels = "F0")
        + theme_default()
        ;
    }
}

# test errors of SVR
plot_errors(
    read.csv("./svr_test.csv", row.names = 1), 
    "Frequency of SVR Prediction Errors", 
    "./svr_test.png"
);

# test errors of xgboost regression
plot_errors(
    read.csv("./regression_test.csv", row.names = 1), 
    "Frequency of xgboost regression errors", 
    "./xgboost_regression_test.png"
);
