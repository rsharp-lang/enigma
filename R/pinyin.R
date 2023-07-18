imports "clr" from "devkit";

#' Transcript chinese text to pinyin text
#' 
const transcript_pinyin = function(text) {
    const pinyin_lib = clr::open("Microsoft.VisualBasic.Text.GB2312");

    if (length(text) == 1) {
        .pinyin(pinyin_lib, text);
    } else {
        lapply(text, si -> .pinyin(pinyin_lib, si));
    }
}

const data_aishell3 = function(text) {
    const pinyin_lib = clr::open("Microsoft.VisualBasic.Text.GB2312");

    if (length(text) == 1) {
        .aishell3_pinyin(pinyin_lib, text);
    } else {
        lapply(text, si -> .aishell3_pinyin(pinyin_lib, si));
    }
}

const .aishell3_pinyin = function(pinyin_lib, si) {
    let pinyin = clr::call_clr(pinyin_lib, "TranscriptPinYin", str = si, sep = " | ");
    let split2 = clr::call_clr(pinyin_lib, "SplitZhChars", str = si, sep = " | ");
    let input2 = si;

    split2 = split2[nchar(split2) > 0];
    pinyin = pinyin 
    |> strsplit(" | ", fixed = TRUE) 
    |> tolower()
    ;

    si = `${split2} ${pinyin}`;
    si = paste(si, " ");

    {
        text: input2,
        chars: data.frame(pinyin, chs = split2),
        data_aishell3: si 
    }
}

const .pinyin = function(pinyin_lib, si) {
    let pinyin = clr::call_clr(pinyin_lib, "TranscriptPinYin", str = si, sep = " | ");
    
    pinyin = pinyin 
    |> strsplit(" | ", fixed = TRUE) 
    |> tolower()
    ;
    pinyin;
}


