require(ggplot);

setwd(@dir);

let plot_data = function(data, savefile, mapping = aes(x="x",y="y", z="z")) {
    bitmap(file = savefile, size = [2400, 1800]) {


        ggplot(data, mapping, padding = [150, 650, 100, 100])
        + geom_point(aes(color = "class"), 
        color = "paper", shape = "circle", size = 10, show.legend = FALSE)
        + view_camera(angle = [31.5,65,125], fov = 100000)
        + ggtitle("Scatter UMAP 3D")
        # + theme_default()
        ;
    }
}

plot_data(read.csv("./protein_umap.csv", row.names = 1, check.names = FALSE), savefile = "./protein_umap.png");
plot_data(read.csv("./protein_pca.csv", row.names = 1, check.names = FALSE), savefile = "./protein_pca.png", mapping = aes(x="dim1",y="dim2", z="dim3"));

