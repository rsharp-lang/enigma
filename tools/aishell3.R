require(enigma);

const dir as string = ?"--dir" || stop("no inputs!");
const text as string = list.files(dir, pattern = "*.txt");
const data as string = sapply(text, function(file) {
    const wav = `${basename(file)}.wav`;
    const text = enigma::data_aishell3(readText(file));

    `${wav} ${text$data_aishell3}`;
});

print(data);

writeLines(data, con = `${dir}/content.txt`);
