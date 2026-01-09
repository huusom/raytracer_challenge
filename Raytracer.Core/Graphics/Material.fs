module Raytracer.Graphics.Material

open Raytracer.Graphics
open Raytracer.Math
open Raytracer

[<CustomEquality; NoComparison>]
type T =
    { mutable color: Color.T
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

let create color ambient diffuse specular shininess =
    { color = color
      ambient = ambient
      diffuse = diffuse
      specular = specular
      shininess = shininess }

let lightning material (light: Light.T) point eyev normalv =
    let effective_color = material.color * light.intensity
    let lightv = (light.position - point) |> Tuple.normalize
    let ambient = effective_color * material.ambient
    let light_dot_normal = Tuple.dot lightv normalv

    let diffuse, specular =
        if light_dot_normal < 0. then
            Color.black, Color.black
        else
            let diffuse = effective_color * material.diffuse * light_dot_normal
            let reflectv = Tuple.reflect -lightv normalv
            let reflect_dot_eye = Tuple.dot reflectv eyev

            if reflect_dot_eye <= 0. then
                diffuse, Color.black
            else
                let factor = reflect_dot_eye ** material.shininess
                let specular = light.intensity * material.specular * factor
                diffuse, specular

    ambient + diffuse + specular
