require(ggplot);

setwd(@dir);

# test errors of SVR
data = read.csv("./svr_test.csv", row.names = 1);
data[, "errors"] = abs(data$z.predicts. - data$z);

print(data, max.print =6);

bitmap(file = "./svr_test.png", size = [2700, 1800]) {
    ggplot(data, aes(x = "errors"), padding = "padding:250px 400px 200px 250px;")
	 + geom_histogram(bins = 250,  color = "steelblue")
	 + ggtitle("Frequency of SVR Prediction Errors")
	 + scale_x_continuous(labels = "F2")
     + scale_y_continuous(labels = "F0")
	 + theme_default()
	 ;
}