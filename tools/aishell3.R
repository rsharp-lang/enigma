require(enigma);

const dir as string = ?"--dir" || stop("no inputs!");
const text as string = list.files(dir, pattern = "*.txt");
const data = lapply(text, function(file) {
    const wav = `${basename(file)}.wav`;
    const text = enigma::data_aishell3(readText(file));
    const content = `${wav} ${text$data_aishell3}`;
    const label = `${basename(file)}|${paste(text$chars$pinyin, " ")} $|${text$text}$`;

    {
        content: content,
        label: label
    }
});

str(data);

writeLines(data@content, con = `${dir}/content.txt`);
writeLines(data@label, con = `${dir}/label_train-set.txt`);