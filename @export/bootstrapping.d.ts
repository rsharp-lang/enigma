// export R# package module type define for javascript/typescript language
//
//    imports "bootstrapping" from "enigma";
//
// ref=enigma.bootstrapping@enigma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace bootstrapping {
   /**
   */
   function arguments(fnTrainTriples: string, fnValidTriples: string, fnTestTriples: string, fnAllTriples: string): object;
   /**
     * @param algorithm default value Is ``["ComplEx","complex_NNE","complex_NNE_AER","complex_R"]``.
     * @param rules default value Is ``null``.
     * @param iterations default value Is ``1000``.
     * @param env default value Is ``null``.
   */
   function complex(args: object, algorithm?: any, rules?: string, iterations?: object, env?: object): any;
   /**
   */
   function graph2vec(graph: any): any;
   /**
     * @param dims default value Is ``10``.
   */
   function node2vec(graph: any, dims?: object): any;
}
