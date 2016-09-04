module GameRepository

open Models
open Game
open FSharp.Data
open FSharp.Data.CsvExtensions
open System


let mapRows (rows:seq<CsvRow>) = rows |> Seq.map ( fun c -> { Division = c?Div; Date = DateTime.Parse c?Date; 
        HomeTeam = { Name = c?HomeTeam; Score = c?FTHG.AsInteger(); Shots = c?HS.AsInteger(); ShotsOnTarget = c?HST.AsInteger(); Corners = c?HC.AsInteger(); Fouls = c?HF.AsInteger() }; 
        AwayTeam = { Name = c?AwayTeam; Score = c?FTAG.AsInteger(); Shots = c?AS.AsInteger(); ShotsOnTarget = c?AST.AsInteger(); Corners = c?AC.AsInteger(); Fouls = c?AF.AsInteger() };
        Odds = { H = c?B365H.AsFloat(); U = c?B365D.AsFloat();  B = c?B365A.AsFloat() } } )



let getResource season = 
    if season.Year = 17 then
        match season.League with
        | PL -> "http://www.football-data.co.uk/mmz4281/1617/E0.csv"
        | BL -> "http://www.football-data.co.uk/mmz4281/1617/D1.csv"
        | SP -> "http://www.football-data.co.uk/mmz4281/1617/SP1.csv"
        | FR -> "http://www.football-data.co.uk/mmz4281/1617/F1.csv"
    else sprintf "../../data/%s_%i.csv" season.League.name season.Year


let loadGames season = 
    let resource = getResource season
    CsvFile.Load(resource).Cache().Rows |> mapRows
    


let get league = 
    loadGames { Year = 14; League = league } 
        |> Seq.append  (loadGames { Year = 15; League = league } )
        |> Seq.append (loadGames { Year = 16; League = league } )
        |> Seq.append (loadGames { Year = 17; League = league } )
        |> Seq.sortByDescending(fun game -> game.Date)

 
