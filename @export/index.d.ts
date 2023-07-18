// export R# source type define for javascript/typescript language
//
// package_source=enigma

declare namespace enigma {
   module _ {
      /**
      */
      function aishell3_pinyin(pinyin_lib: any, si: any): object;
      /**
      */
      function onLoad(): object;
      /**
      */
      function pinyin(pinyin_lib: any, si: any): object;
   }
   /**
   */
   function data_aishell3(text: any): object;
   /**
   */
   function transcript_pinyin(text: any): object;
}
