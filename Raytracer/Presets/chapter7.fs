module Raytracer.Presets.Chapter7

open Raytracer
open Raytracer.Geometry.Transformation

let name = "chapter7"
let render (camera : Scene.Camera.T) =
    let radians r = System.Math.PI / r

    let ambient = 0.1
    let diffuse = 0.9
    let specular = 0.9
    let shininess = 200.0
    let color = Graphics.Color.white

    let floor =
        Graphics.Material.create ambient diffuse 0 shininess color
        |> Geometry.Shape.sphereOf (scalingOf 10 0.01 10)

    let left_wall =
        Graphics.Material.create ambient diffuse specular shininess color
        |> Geometry.Shape.sphereOf (
            combine
                [ translationOf 0 0 5
                  rotationYOf (radians -4.)
                  rotationXOf (radians 2.)
                  scalingOf 10 0.1 10 ]
        )

    let right_wall =
        Graphics.Material.create ambient diffuse specular shininess color
        |> Geometry.Shape.sphereOf (
            combine
                [ translationOf 0 0 5
                  rotationYOf (radians 4.)
                  rotationXOf (radians 2.)
                  scalingOf 10 0.1 10 ]
        )

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
        Scene.World.create [ floor; left_wall; right_wall; middle; right; left ] [ light ]

    camera.transform <- viewOf (Math.Tuple.pointOf 0 1.5 -5) (Math.Tuple.pointOf 0 1 0) (Math.Tuple.vectorOf 0 1 0)

    world |> Scene.Camera.render camera

