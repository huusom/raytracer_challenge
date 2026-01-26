module Raytracer.Scene.Light

open Raytracer.Graphics
open Raytracer.Math
open Raytracer.Geometry

[<Struct>]
type T =
    { position: Tuple.T
      intensity: Color.T }

let create position intensity =
    { position = position
      intensity = intensity }

let pointLightOf position intensity = create position intensity

let colorFrom light (comps: Intersection.Comps.T) shadow =
    let material = comps.object.material

    let color = Shape.colorFrom comps.object comps.point

    let effective_color = color * light.intensity
    let ambient = effective_color * material.ambient

    if shadow then
        ambient
    else
        let lightv = light.position - comps.point |> Tuple.normalize
        let light_dot_normal = Tuple.dot lightv comps.normal

        if light_dot_normal < 0. then
            ambient
        else
            let diffuse = effective_color * material.diffuse * light_dot_normal
            let reflectv = Tuple.reflect -lightv comps.normal
            let reflect_dot_eye = Tuple.dot reflectv comps.eye

            if reflect_dot_eye <= 0. then
                ambient + diffuse
            else
                let factor = reflect_dot_eye ** material.shininess
                let specular = light.intensity * material.specular * factor
                ambient + diffuse + specular
