module GameRepository

open Models
open Game
open FSharp.Data
open System

type GamesFile14 = CsvProvider<"./data/sample_14.csv">
type GamesFile15 = CsvProvider<"./data/sample_15.csv">
type GamesFile1617 = CsvProvider<"./data/sample_1617.csv">

let mapRows14 (rows:seq<GamesFile14.Row>) = rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; 
        HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; 
        AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF };
        Odds = { H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 


let mapRows15 (rows:seq<GamesFile15.Row>) = rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; 
        HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; 
        AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF };
        Odds = { H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 


let mapRows1617 (rows:seq<GamesFile1617.Row>) = rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; 
        HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; 
        AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF };
        Odds = { H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 


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
    if season.Year = 14 then GamesFile14.Load(resource).Rows |> mapRows14
    else if season.Year = 15 then GamesFile15.Load(resource).Rows |> mapRows15
    else GamesFile1617.Load(resource).Rows |> mapRows1617
    


let get league = 
    loadGames { Year = 14; League = league } 
        |> Seq.append  (loadGames { Year = 15; League = league } )
        |> Seq.append (loadGames { Year = 16; League = league } )
        |> Seq.append (loadGames { Year = 17; League = league } )
        |> Seq.sortByDescending(fun game -> game.Date)

 
