imports "clr" from "devkit";

const pinyin_lib = clr::open("Microsoft.VisualBasic.Text.GB2312");

#' Transcript chinese text to pinyin text
#' 
const transcript_pinyin = function(text) {
    if (length(text) == 1) {
        let pinyin = clr::call_clr(pinyin_lib, "TranscriptPinYin", str = text, sep = " | ");
        pinyin;
    } else {
        lapply(text, si -> transcript_pinyin(si));
    }
}


