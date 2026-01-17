module Raytracer.Scene.Light

[<Struct>]
type T =
    { position: Raytracer.Math.Tuple.T
      intensity: Raytracer.Graphics.Color.T }

let create position intensity =
    { position = position
      intensity = intensity }

let pointLightOf position intensity = create position intensity
