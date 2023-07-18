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

const .pinyin = function(pinyin_lib, si) {
    let pinyin = clr::call_clr(pinyin_lib, "TranscriptPinYin", str = si, sep = " | ");
    
    pinyin = pinyin 
    |> strsplit(" | ", fixed = TRUE) 
    |> tolower()
    ;
    pinyin;
}


