module Raytracer.Graphics.Material

open Raytracer.Graphics
open Raytracer.Math

[<CustomEquality; NoComparison>]
type T =
    { mutable color: Color.T
      mutable pattern: Pattern.T option
      mutable ambient: float
      mutable diffuse: float
      mutable specular: float
      mutable shininess: float }

    override this.Equals(obj) =
        match obj with
        | :? T as other ->
            eq this.ambient other.ambient
            && eq this.diffuse other.diffuse
            && eq this.specular other.specular
            && eq this.shininess other.shininess
            && this.color.Equals other.color
        | _ -> false

    override this.GetHashCode() : int =
        hash (this.color, this.ambient, this.diffuse, this.specular, this.shininess)

let create ambient diffuse specular shininess pattern color =
    { color = color
      pattern = pattern
      ambient = ambient
      diffuse = diffuse
      specular = specular
      shininess = shininess }
