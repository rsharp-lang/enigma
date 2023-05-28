// export R# package module type define for javascript/typescript language
//
//    imports "model" from "enigma";
//
// ref=enigma.models@enigma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * ### Create the machine learning model
 *  
 *  Performing machine learning involves creating a model, which is trained
 *  on some training data and then can process additional data to make predictions.
 *  Various types of models have been used and researched for machine 
 *  learning systems.
 * 
*/
declare namespace model {
   module ANN {
      /**
      */
      function regression(): object;
   }
   module randomForest {
      /**
      */
      function regression(): object;
   }
   /**
    * load saved machine learning model
    * 
    * 
     * @param file A file path to the model snapshot data file.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function readModelFile(file: any, env?: object): object;
   /**
    * make the snapshot of the machine learning model
    * 
    * 
     * @param model A machine learning model
     * @param file A file path value, the model snapshot will be save to this file.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function snapshot(model: object, file: string, env?: object): boolean;
   /**
    * ### Support-vector machines
    * 
    *  Support-vector machines (SVMs), also known as support-vector 
    *  networks, are a set of related supervised learning methods 
    *  used for classification And regression. Given a set of training 
    *  examples, each marked as belonging to one of two categories, 
    *  an SVM training algorithm builds a model that predicts whether 
    *  a New example falls into one category. An SVM training 
    *  algorithm Is a non-probabilistic, binary, linear classifier, 
    *  although methods such as Platt scaling exist to use SVM in a 
    *  probabilistic classification setting. In addition to performing
    *  linear classification, SVMs can efficiently perform a non-linear 
    *  classification using what Is called the kernel trick, 
    *  implicitly mapping their inputs into high-dimensional feature 
    *  spaces.
    * 
    * 
   */
   function svm(): object;
   /**
    * 
    * 
   */
   function svr(): object;
   /**
    * ### XGBoost (eXtreme Gradient Boosting)
    *  
    *  XGBoost is an optimized distributed gradient boosting library
    *  designed to be highly efficient, flexible and portable. 
    *  It implements machine learning algorithms under the Gradient
    *  Boosting framework. XGBoost provides a parallel tree boosting 
    *  (also known as GBDT, GBM) that solve many data science 
    *  problems in a fast and accurate way.
    * 
    * 
   */
   function xgboost(): object;
}
