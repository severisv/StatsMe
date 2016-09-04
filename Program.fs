open System
open FSharp.Data
open Parameters
open Odds
open Game
open Games
open Predictions
open Betting
 

let count result predictions =
    float(predictions |> Seq.filter(fun res -> res = result) |> Seq.length) / float(predictions |> Seq.length)

type GamesFile15 = CsvProvider<"./data/15.csv">
type GamesFile16 = CsvProvider<"./data/16.csv">
type GamesFile17 = CsvProvider<"http://www.football-data.co.uk/mmz4281/1617/E0.csv">

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

 
        
[<EntryPoint>]
let main argv =
    let allGames = Seq.append games15 games16 |> Seq.append games17 |> Seq.sortByDescending(fun game -> game.Date)
      
    let sample = 30
    
     
    for i = 9 to 9 do
        let parameters = { OddsThreshold = (float i)/10.0 }
        let predictions = allGames |> Seq.take (sample) |> Seq.map(fun game -> bet game allGames parameters)

        let balance = predictions |> Seq.sumBy (fun res -> res.Balance)
        let spent = predictions |> Seq.sumBy (fun res -> res.Spent)
        Parameters.print parameters
        printf "%f - %f   ->  %f \n" balance spent ((balance + spent)/ spent)
    
   
    let s = Console.ReadLine()
    0 // return an integer exit code
