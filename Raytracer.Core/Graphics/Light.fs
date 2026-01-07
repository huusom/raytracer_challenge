module Raytracer.Graphics.Light

type T =
    { position: Raytracer.Math.Tuple.T
      intensity: Color.T }

let pointLight position intensity =
    { position = position
      intensity = intensity }
