module Raytracer.Presets.Chapter9

open Raytracer
open Raytracer.Geometry.Transformation

let name = "chapter9"
let render (camera : Scene.Camera.T) =
    let radians r = System.Math.PI / r

    let ambient = 0.1
    let diffuse = 0.9
    let specular = 0.9
    let shininess = 200.0

    let floor =
        Graphics.Color.white
        |> Graphics.Material.create ambient diffuse specular shininess
        |> Geometry.Shape.planeOf Geometry.Transformation.identity 

    let middle =
        Graphics.Color.create 0.1 1 0.5
        |> Graphics.Material.create ambient 0.7 0.3 shininess
        |> Geometry.Shape.sphereOf (translationOf -0.5 1 0.5)

    let right =
        Graphics.Color.create 0.5 1 1
        |> Graphics.Material.create ambient 0.7 0.3 shininess
        |> Geometry.Shape.sphereOf (combine [ translationOf 1.5 0.5 -0.5; scalingOf 0.5 0.5 0.5 ])

    let left =
        Graphics.Color.create 1 0.8 0.1
        |> Graphics.Material.create ambient 0.7 0.3 shininess
        |> Geometry.Shape.sphereOf (combine [ translationOf -1.5 0.33 -0.75; scalingOf 0.33 0.33 0.33 ])

    let light =
        Scene.Light.pointLightOf (Math.Tuple.pointOf -10 10 -10) Graphics.Color.white

    let world =
        Scene.World.create [ floor; middle; right; left ] [ light ]

    camera.transform <- viewOf (Math.Tuple.pointOf 0 1.5 -5) (Math.Tuple.pointOf 0 1 0) (Math.Tuple.vectorOf 0 1 0)

    world |> Scene.Camera.render camera

