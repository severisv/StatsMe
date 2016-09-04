module GameRepository

open Models
open Game
open FSharp.Data
open System

//type GamesFile15 = CsvProvider<"./data/15.csv">
//type GamesFile16 = CsvProvider<"./data/16.csv">
//type GamesFile17 = CsvProvider<"http://www.football-data.co.uk/mmz4281/1617/E0.csv">

type GamesFile15 = CsvProvider<"./data/FR14.csv">
type GamesFile16 = CsvProvider<"./data/FR15.csv">
type GamesFile17 = CsvProvider<"./data/FR16.csv">


let games15 = GamesFile15.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; 
        HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; 
        AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF };
               Odds = {  H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 


let games16 = GamesFile16.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; 
        HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; 
        AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF };
               Odds = {  H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 


let games17 = GamesFile17.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; 
        HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; 
        AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF };
        Odds = { H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 




let loadPlGames = Seq.append games15 games16 |> Seq.append games17 |> Seq.sortByDescending(fun game -> game.Date)


let get league =
    match league with
    | PL -> loadPlGames
    | BL -> loadPlGames
    | FR -> loadPlGames
    | SP -> loadPlGames
