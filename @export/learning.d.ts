// export R# package module type define for javascript/typescript language
//
// ref=enigma.learning@enigma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * ## machine learning toolkit
 *  
 *  Machine learning (ML) is a field of inquiry devoted to
 *  understanding and building methods that "learn" – that
 *  is, methods that leverage data to improve performance 
 *  on some set of tasks. It is seen as a part of artificial
 *  intelligence.
 * 
 *  Machine learning algorithms build a model based On sample
 *  data, known As training data, In order To make predictions
 *  Or decisions without being explicitly programmed To Do so.
 *  Machine learning algorithms are used In a wide variety Of 
 *  applications, such As In medicine, email filtering, speech 
 *  recognition, agriculture, And computer vision, where it Is
 *  difficult Or unfeasible To develop conventional algorithms 
 *  To perform the needed tasks.
 * 
 *  A subset Of machine learning Is closely related To computational 
 *  statistics, which focuses On making predictions Using computers,
 *  but Not all machine learning Is statistical learning. The 
 *  study Of mathematical optimization delivers methods, theory 
 *  And application domains To the field Of machine learning. Data
 *  mining Is a related field Of study, focusing On exploratory 
 *  data analysis through unsupervised learning.
 * 
 *  Some implementations Of machine learning use data And neural 
 *  networks In a way that mimics the working Of a biological 
 *  brain.
 * 
 *  In its application across business problems, machine learning 
 *  Is also referred to as predictive analytics.
 * 
*/
declare namespace learning {
   /**
    * ### create a new machine learning model
    *  
    *  In mathematics, a tensor is an algebraic object that describes
    *  a multilinear relationship between sets of algebraic objects 
    *  related to a vector space. Tensors may map between different 
    *  objects such as vectors, scalars, and even other tensors. There
    *  are many types of tensors, including scalars and vectors (which 
    *  are the simplest tensors), dual vectors, multilinear maps between 
    *  vector spaces, and even some operations such as the dot product.
    *  Tensors are defined independent of any basis, although they are 
    *  often referred to by their components in a basis related to a 
    *  particular coordinate system.
    * 
    * 
     * @param model the source of the machine learning model where it comes from:
     *  
     *  1. could be a file name to the trained model file
     *  2. could be a function name in ``models`` namespace to train a new model
     * @param env 
     * + default value Is ``null``.
   */
   function tensor(model:any, env?:object): object;
   /**
    * feed training data to the machine learning model
    * 
    * > Typically, machine learning models require a high quantity of 
    * >  reliable data in order for the models to perform accurate predictions. 
    * >  When training a machine learning model, machine learning engineers
    * >  need to target and collect a large and representative sample of 
    * >  data. Data from the training set can be as varied as a corpus of 
    * >  text, a collection of images, sensor data, and data collected from 
    * >  individual users of a service. Overfitting is something to watch out
    * >  for when training a machine learning model. Trained models derived 
    * >  from biased or non-evaluated data can result in skewed or undesired 
    * >  predictions. Bias models may result in detrimental outcomes thereby 
    * >  furthering the negative impacts on society or objectives. Algorithmic
    * >  bias is a potential result of data not being fully prepared for 
    * >  training. Machine learning ethics is becoming a field of study and 
    * >  notably be integrated within machine learning engineering teams.
    * 
     * @param model A machine learning model object
     * @param x the training data to feed to the model object, 
     *  it usually be a dataframe object.
     * @param features usually a character vector to gets the training data fields
     * @param args 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
   */
   function feed(model:object, x:any, features:any, args?:object, env?:object): object;
   /**
    * configs of the hidden layer of the ``@``T:enigma.ANN```` model
    * 
    * 
     * @param model The ``@``T:enigma.ANN```` machine learning model object
     * @param size A integer vector for configs the layer size in the hidden layer
     * @param activate Activation function of the hidden layer
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function hidden_layer(model:object, size:any, activate?:any, env?:object): object;
   /**
    * ### configs of the output labels 
    *  
    *  configs of the output labels of the feeded training data 
    *  for machine learning model
    * 
    * 
     * @param model -
     * @param labels The data field name for get the actual label value from the input
     *  training data
     * 
     * + default value Is ``null``.
     * @param args 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function output(model:object, labels?:any, args?:object, env?:object): object;
   /**
    * Do machine learning model training
    * 
    * 
     * @param model A machine learning algorithm model which is generated from 
     *  the ``tensor`` function.
     * @param args the additional arguments to the machine learning model trainer, it could be:
     *  
     *  1. Artificial neural networks model
     *  
     *  + truncate, numeric
     *  + threshold, numeric
     *  + parallel, logical
     *  + softmax, logical
     *  + max.epochs, integer
     *  
     *  2. xgboost
     *  
     *  + loss,Loss: logloss for classification and squareloss for regression
     *  + cost,eval_metric: auc for classification and mse for regression
     *  + gamma, numeric: default 0.0
     *  + lambda, numeric: default 1.0
     *  + learn_rate, eta, numeric: default 0.3
     *  + max_depth, integer: default 7
     *  + num_boost_round, integer, default 10
     *  + max, maximize, logical: default true
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function learn(model:object, args?:object, env?:object): object;
   /**
    * Do data prediction
    * 
    * 
     * @param model A machine learning model which is has been trained via the ``learn`` function.
     *  Or this machine learning algorithm model also could be loaded from the snapshot 
     *  file via the ``readModelFile``.
     * @param data A dataframe object which should contains the same features fields with the 
     *  input training data
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function solve(model:object, data:any, env?:object): any;
   /**
    * ### Split a Dataset into Train and Test Sets
    *  
    *  The train-test split is used to estimate the performance
    *  of machine learning algorithms that are applicable for 
    *  prediction-based Algorithms/Applications. This method is 
    *  a fast and easy procedure to perform such that we can 
    *  compare our own machine learning model results to machine
    *  results. By default, the Test set is split into 30 % of 
    *  actual data and the training set is split into 70% of the 
    *  actual data.
    *  
    *  We need To split a dataset into train And test sets To 
    *  evaluate how well our machine learning model performs. The
    *  train Set Is used To fit the model, And the statistics Of
    *  the train Set are known. The second Set Is called the test
    *  data Set, this Set Is solely used For predictions.
    * 
    * 
     * @param x -
     * @param train_ratio -
     * 
     * + default value Is ``0.8``.
     * @param cross 
     * + default value Is ``0.2``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function trainTestSplit(x:object, train_ratio?:number, cross?:number, env?:object): object;
}
