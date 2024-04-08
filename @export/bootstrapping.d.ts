// export R# package module type define for javascript/typescript language
//
//    imports "bootstrapping" from "enigma";
//
// ref=enigma.bootstrapping@enigma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace bootstrapping {
   /**
   */
   function arguments(): object;
   /**
     * @param algorithm default value Is ``["ComplEx","complex_NNE","complex_NNE_AER","complex_R"]``.
     * @param rules default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function complex(args: object, algorithm?: any, rules?: string, env?: object): any;
}
