imports "dataset" from "MLkit";
imports "umap" from "MLkit";

setwd(@dir);

let data = read.csv("../../data/sgt/protein_classification.csv", row.names = 1, check.names = FALSE);
let seqs = lapply(data$Sequence, i -> i, names = data$"Entry name");

# print(data);

str(seqs);

let sgt = SGT(alphabets = estimate_alphabets(seqs),
     kappa = 1,
                      length.sensitive = TRUE,
                      full = TRUE);

let graph = fit(sgt, seqs, df = TRUE);

print(graph);

let umap = umap(graph, dimension = 3, numberOfNeighbors = 64 );
let scatter = as.data.frame(umap$umap, labels = umap$labels, dimension = ["x","y","z"]);

scatter[, "class"] = data$"Protein families";

write.csv(scatter, file = "./protein_umap.csv", row.names = TRUE);

let pca = as.data.frame(prcomp(graph), npc = 3);

pca[, "class"] = data$"Protein families";

write.csv(pca, file = "./protein_pca.csv", row.names = TRUE);