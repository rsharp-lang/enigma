require(enigma);

imports "bootstrapping" from "enigma";

let m = bootstrapping::node2vec(file.path(@dir, "karate.edgelist"));