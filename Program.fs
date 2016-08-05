open System
open FSharp.Data

type Team = {
    Name : string
    Score : int
    Shots : int
    ShotsOnTarget : int
    Corners : int
    Fouls : int
}

type Outcome =  Win | Draw | Loss

type PlayedGame = {
    Shots : int
    ShotsOnTarget : int
    Outcome : Outcome
}


type Game = {
    Date : DateTime
    Division : string
    HomeTeam : Team
    AwayTeam : Team
  }

type Result =  H | U | B


type Prediction = {
    Home : double
    Draw : double
    Away : double
}

type Odds = {
    H : double
    U : double
    B : double
}

let getResult game = 
    if game.HomeTeam.Score > game.AwayTeam.Score then H
    elif game.HomeTeam.Score < game.AwayTeam.Score then B
    else U

let getOutcome goalsFor goalsAgainst =
    if goalsFor > goalsAgainst then Win
    elif goalsFor < goalsAgainst then Loss
    else Draw

let getPreviousHomeGames date team games take =
        let home = games |> Seq.filter(fun game -> game.HomeTeam.Name = team.Name && game.Date < date)
        let homegames = if home |> Seq.length > take then home |> Seq.take take else home
        homegames |> Seq.map(fun game -> 
          { Shots = game.HomeTeam.Shots - game.AwayTeam.Shots; 
            ShotsOnTarget = game.HomeTeam.ShotsOnTarget - game.AwayTeam.ShotsOnTarget;
            Outcome = (getOutcome game.HomeTeam.Score game.AwayTeam.Score) })
 
let getPreviousAwayGames date team games take =       
        let away = games |> Seq.filter(fun game -> game.AwayTeam.Name = team.Name && game.Date < date)
        let awaygames = if away |> Seq.length > take then away |> Seq.take take else away
        awaygames |> Seq.map(fun game -> 
          { Shots = game.AwayTeam.Shots - game.HomeTeam.Shots; 
            ShotsOnTarget = game.AwayTeam.ShotsOnTarget - game.HomeTeam.ShotsOnTarget; 
            Outcome = (getOutcome game.AwayTeam.Score game.HomeTeam.Score) })        


let getOutcomePercentage outcomes outcome totalGames = 
        float(outcomes |> Seq.filter (fun game -> game.Outcome = outcome) |> Seq.length ) / totalGames

let getTeamScore (prevGames:seq<PlayedGame>) = 
       let totalGames = prevGames |> Seq.length |> float
       let outcomes = prevGames |> Seq.map(fun game -> game)
       let winPercentage = getOutcomePercentage outcomes Win totalGames
       let drawPercentage = getOutcomePercentage outcomes Draw totalGames
       let lossPercentage = getOutcomePercentage outcomes Loss totalGames
       { Home = winPercentage; Draw = drawPercentage; Away = lossPercentage }


let createOdds prediction =
        { H = 1.0/prediction.Home; U = 1.0/prediction.Draw; B = 1.0/prediction.Away }
    

let bet game allGames variable = 
        let homeScore = getTeamScore (getPreviousHomeGames game.Date game.HomeTeam allGames 14)
        let awayScore = getTeamScore (getPreviousAwayGames game.Date game.AwayTeam allGames 14)
       
        let prediction = { Home = (homeScore.Home + awayScore.Home)/2.0;  Draw = (homeScore.Draw + awayScore.Draw)/2.0; Away = (homeScore.Away + awayScore.Away)/2.0 }   
        let odds = createOdds prediction

        printfn "%f %f %f \n" odds.H odds.U odds.B


        if prediction.Home > prediction.Draw && prediction.Home > prediction.Away then H
        elif prediction.Draw > prediction.Home && prediction.Draw > prediction.Away then U
        else B
 

let predictionHolds game allGames variable = 
        let prediction = bet game allGames variable
        let result = getResult game
        result = prediction


let count result predictions =
    float(predictions |> Seq.filter(fun res -> res = result) |> Seq.length) / float(predictions |> Seq.length)

type GamesFile13 = CsvProvider<"./data/13.csv">
type GamesFile14 = CsvProvider<"./data/14.csv">
type GamesFile15 = CsvProvider<"./data/15.csv">
type GamesFile16 = CsvProvider<"./data/16.csv">

let games13 = GamesFile13.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF }} ) 
let games14 = GamesFile14.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF }} ) 
let games15 = GamesFile15.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF }} ) 
let games16 = GamesFile16.GetSample().Rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF }} ) 
 
        
[<EntryPoint>]
let main argv =
    let allGames = Seq.append games15 games16 |> Seq.append games14  |> Seq.append games13  |> Seq.sortByDescending(fun game -> game.Date)
      
    let sample = 10
   
    for i = 1 to 1 do
        let predictions = allGames |> Seq.take (sample) |> Seq.map(fun game -> bet game allGames "variable")
        let totalHome = predictions |> count H 
        let totalDraw = predictions |> count U 
        let totalAway = predictions |> count B 
        printf "%f %f %f \n" totalHome totalDraw totalAway
    
   
    let s = Console.ReadLine()
    0 // return an integer exit code
