// export R# package module type define for javascript/typescript language
//
//    imports "gpu" from "enigma";
//
// ref=enigma.GPU@enigma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace gpu {
   /**
    * try to execute the expression on gpu
    * 
    * 
     * @param exp -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function exec(exp: object, env?: object): any;
}
