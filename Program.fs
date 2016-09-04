open System
open FSharp.Data
open Odds
open Game

let getPreviousHomeGames date team games take =
        let home = games |> Seq.filter(fun game -> game.HomeTeam.Name = team.Name && game.Date < date)
        let homegames = if home |> Seq.length > take then home |> Seq.take take else home
        homegames |> Seq.map(fun game -> 
          { Shots = game.HomeTeam.Shots - game.AwayTeam.Shots; 
            ShotsOnTarget = game.HomeTeam.ShotsOnTarget - game.AwayTeam.ShotsOnTarget;
            Result = (Game.getResult game.HomeTeam.Score game.AwayTeam.Score) })
 
let getPreviousAwayGames date team games take =       
        let away = games |> Seq.filter(fun game -> game.AwayTeam.Name = team.Name && game.Date < date)
        let awaygames = if away |> Seq.length > take then away |> Seq.take take else away
        awaygames |> Seq.map(fun game -> 
          { Shots = game.AwayTeam.Shots - game.HomeTeam.Shots; 
            ShotsOnTarget = game.AwayTeam.ShotsOnTarget - game.HomeTeam.ShotsOnTarget; 
            Result = (Game.getResult game.AwayTeam.Score game.HomeTeam.Score) })        


let getOutcomePercentage results result totalGames = 
        float(results |> Seq.filter (fun game -> game.Result = result) |> Seq.length ) / totalGames

let getTeamScore (prevGames:seq<PlayedGame>) = 
       let totalGames = prevGames |> Seq.length |> float
       let outcomes = prevGames |> Seq.map(fun game -> game)
       let winPercentage = getOutcomePercentage outcomes Win totalGames
       let drawPercentage = getOutcomePercentage outcomes Draw totalGames
       let lossPercentage = getOutcomePercentage outcomes Loss totalGames
       { Home = winPercentage; Draw = drawPercentage; Away = lossPercentage }


let createOdds prediction treshold =
        { H = treshold/prediction.Home; U = treshold/prediction.Draw; B = treshold/prediction.Away }
    
let betOnGame amount (prediction:float) (provided:float) (didWin:bool) =
        if provided > prediction then 
            if didWin then (provided * amount), amount
            else -amount, amount
        else 0.0, 0.0

let bet game allGames variable = 
        let homeScore = getTeamScore (getPreviousHomeGames game.Date game.HomeTeam allGames 14)
        let awayScore = getTeamScore (getPreviousAwayGames game.Date game.AwayTeam allGames 14)
       
        let prediction = { Home = (homeScore.Home + awayScore.Home)/2.0;  Draw = (homeScore.Draw + awayScore.Draw)/2.0; Away = (homeScore.Away + awayScore.Away)/2.0 }   
        let odds = createOdds prediction variable
        
        let outcome = Game.getOutcome game
        
        let amount = 100.0

        let homebalance, homeSpent = betOnGame amount odds.H game.Odds.H (outcome = H)
        let drawbalance, drawSpent = betOnGame amount odds.U game.Odds.U (outcome = U)
        let awaybalance, awaySpent = betOnGame amount odds.B game.Odds.B (outcome = B)
        


        let result = { Balance = homebalance + drawbalance + awaybalance; Spent =  homeSpent + drawSpent + awaySpent }
        printf "%s - %s   ->  %f \n" game.HomeTeam.Name game.AwayTeam.Name result.Balance
        result

        
 
 
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
        Odds = {  H = float c.B365H; U = float c.B365D;  B = float c.B365A } } ) 

 
        
[<EntryPoint>]
let main argv =
    let allGames = Seq.append games15 games16 |> Seq.append games17 |> Seq.sortByDescending(fun game -> game.Date)
      
    let sample = 30
   
    for i = 9 to 9 do
        let variable = (float i)/10.0
        let predictions = allGames |> Seq.take (sample) |> Seq.map(fun game -> bet game allGames variable)

        let balance = predictions |> Seq.sumBy (fun res -> res.Balance)
        let spent = predictions |> Seq.sumBy (fun res -> res.Spent)
        printf "%f:  %f - %f   ->  %f \n" variable balance spent ((balance + spent)/ spent)
    
   
    let s = Console.ReadLine()
    0 // return an integer exit code
