require(enigma);

imports "bootstrapping" from "enigma";

# Datasets we used are in the corresponding subfolder contained in datasets/ with the following formats:
# * _train.txt,_valid.txt,_test.txt; training, valid, test set with string id; format: **e1**\t**r**\t**e2**\n
# * _cons.txt; approximate entailment constraints; formant: **r1,r2**\t**confidence**\n, where '-' denotes the inversion

setwd(@dir);

let args = bootstrapping::arguments(
    fnTrainTriples = "DB100K\\_train.txt", 
    fnValidTriples = "DB100K\\_valid.txt", 
    fnTestTriples = "DB100K\\_test.txt", 
    fnAllTriples = NULL
);

bootstrapping::complex(args);