// export R# package module type define for javascript/typescript language
//
// ref=enigma.activateFunction@enigma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * #### the activate function handler
 *  
 *  An activation function is a function used in artificial neural
 *  networks which outputs a small value for small inputs, and a
 *  larger value if its inputs exceed a threshold. If the inputs
 *  are large enough, the activation function "fires", otherwise it
 *  does nothing. In other words, an activation function is like a
 *  gate that checks that an incoming value is greater than a 
 *  critical number.
 *  
 *  Activation functions are useful because they add non-linearities
 *  into neural networks, allowing the neural networks To learn 
 *  powerful operations. If the activation functions were To be removed
 *  from a feedforward neural network, the entire network could be 
 *  re-factored To a simple linear operation Or matrix transformation
 *  On its input, And it would no longer be capable Of performing 
 *  complex tasks such As image recognition.
 *  
 *  Well-known activation functions used in data science include the 
 *  rectified linear unit (ReLU) function, And the family of sigmoid 
 *  functions such as the logistic sigmoid function, the hyperbolic
 *  tangent, And the arctangent function.
 * 
 * > Activation functions in computer science are inspired by the 
 * >  action potential in neuroscience. If the electrical potential
 * >  between a neuron's interior and exterior exceeds a value called 
 * >  the action potential, the neuron undergoes a chain reaction 
 * >  which allows it to 'fire' and transmit a signal to neighboring 
 * >  neurons. The resultant sequence of activations, called a 'spike 
 * >  train', enables sensory neurons to transmit feeling from the 
 * >  fingers to the brain, and allows motor neurons to transmit 
 * >  instructions from the brain to the limbs.
*/
declare namespace activateFunction {
   /**
    * Logistic Sigmoid Function Formula
    * 
    * 
     * @param alpha -
     * 
     * + default value Is ``0.2``.
     * @param truncate 
     * + default value Is ``-1``.
   */
   function sigmoid(alpha?:number, truncate?:number): object;
   /**
     * @param truncate default value Is ``-1``.
   */
   function identical(truncate?:number): object;
   /**
     * @param truncate default value Is ``-1``.
   */
   function qlinear(truncate?:number): object;
   /**
    * create a new custom activate function
    * 
    * 
     * @param forward -
     * @param derivative derivative formula of the ``**`forward`**`` formula
     * @param truncate 
     * + default value Is ``-1``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function func(forward:any, derivative:any, truncate?:number, env?:object): any;
}
