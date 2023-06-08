require(ggplot);

setwd(@dir);

let data = read.csv("./protein_umap.csv", row.names = 1, check.names = FALSE);

bitmap(file = "./protein_umap.png", size = [2400, 1800]) {


    ggplot(data, aes(x="x",y="y", z="z"), padding = [150, 650, 100, 100])
    + geom_point(aes(color = "class"), 
    color = "paper", shape = "circle", size = 10, show.legend = FALSE)
	+ view_camera(angle = [31.5,65,125], fov = 100000)
	+ ggtitle("Scatter UMAP 3D")
	# + theme_default()
	;
}