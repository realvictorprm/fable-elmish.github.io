module Sample.Index.View

open Fable.Core
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Global
open Types


type Sample =
  | Tile of SampleReference
  | Placeholder

let tileDocs tile =
  match tile with
  | Tile info ->
      div
        [ ClassName "tile is-parent is-vertical" ]
        [ article
            [ ClassName "tile is-child box" ]
            [ p
                [ ClassName "title" ]
                [ a
                    [ Href (toHash ((info.title) |> (Some >> Samples))) ]
                    [ str info.title ] ]
              p
                [ ClassName "subtitle" ]
                [ str info.description ] ] ]
  | Placeholder ->
      div
        [ ClassName "tile is-parent is-vertical" ]
        [ article
            [ ClassName "tile is-child" ]
            [  ] ]

let tileVertical tileList =
  div
    [ ClassName "tile is-vertical is-6" ]
    (tileList |> List.map tileDocs)

let renderSection sectionInfo =
  let rec divideTiles tiles index (left, right) =
    match tiles with
    | tile::trail ->
        let sectionInfo' =
          match index % 2 with
          | 0 ->
              left @ [ Tile tile], right
          | 1 ->
              left, right @ [ Tile tile]
          | _ -> failwith "Should not happened"
        divideTiles trail (index + 1) sectionInfo'
    | [] ->
        // Ensure we have the same number of tiles in both columns
        // This prevent to have taller tiles
        if (index % 2) <> 0 then
          left, right @ [ Placeholder ]
        else
          left, right

  let (leftSection, rightSection) = divideTiles sectionInfo.samples 0 ([],[])

  div
    [ ]
    [ h1
        [ ClassName "title" ]
        [ str sectionInfo.title ]
      div
        [ ClassName "tile is-ancestor" ]
        [ tileVertical leftSection
          tileVertical rightSection ] 
      br [] ]

let view (model:Model) =
  div [] (model.index |> List.map renderSection)
    