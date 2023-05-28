require(enigma);

import {gpu} from "enigma";

var x = [0,1,2,3,4,5,6,7,8,9];
var result = gpu.exec(x + 5);

console.log(result);