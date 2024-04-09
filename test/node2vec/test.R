require(enigma);

imports "bootstrapping" from "enigma";
imports "NLP" from "MLkit";
imports "umap" from "MLkit";

let m = bootstrapping::node2vec(file.path(@dir, "karate.edgelist"));
let data = as.data.frame(m);

print(data);

let embed = umap(data, dimension = 3);

data = as.data.frame(embed$umap, labels = embed$labels);

print(data);

let dist(x) = sqrt(sum(x ^ 2));
let i = order(sapply( as.list(data, byrow = TRUE), v -> dist(as.numeric(unlist(v)) )));
let order_data = data[i, ];

print(i);
print(data);