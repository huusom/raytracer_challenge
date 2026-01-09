[<AutoOpen>]
module Raytracer.Library

let epsilon = 0.00001
let inline eq a b = abs (a - b) < epsilon

