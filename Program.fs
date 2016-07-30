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


type PlayedGame = {
    Shots : int
    ShotsOnTarget : int
}


type Game = {
    Date : DateTime
    Division : string
    HomeTeam : Team
    AwayTeam : Team
  }

type Result =  H | U | B


let getResult game = 
    if game.HomeTeam.Score > game.AwayTeam.Score then H
    elif game.HomeTeam.Score < game.AwayTeam.Score then B
    else U


let getPreviousGames date team games take =
        let home = games |> Seq.filter(fun game -> game.HomeTeam.Name = team.Name && game.Date < date)
        let homegames = if home |> Seq.length > take then home |> Seq.take take else home
        let homeMapped = homegames |> Seq.map(fun game -> { Shots = game.HomeTeam.Shots - game.AwayTeam.Shots; ShotsOnTarget = game.HomeTeam.ShotsOnTarget - game.AwayTeam.ShotsOnTarget })
        
        let away = games |> Seq.filter(fun game -> game.AwayTeam.Name = team.Name && game.Date < date)
        let awaygames = if away |> Seq.length > take then away |> Seq.take take else away
        let awayMapped = awaygames |> Seq.map(fun game -> { Shots = game.AwayTeam.Shots - game.HomeTeam.Shots; ShotsOnTarget = game.AwayTeam.ShotsOnTarget - game.HomeTeam.ShotsOnTarget })
        
        homeMapped |> Seq.append awayMapped


let getTeamScore prevGames = 
        let count = prevGames |> Seq.length
        let shots = prevGames |> Seq.sumBy(fun prev -> prev.Shots)
        let shotsOnTarget = prevGames |> Seq.sumBy(fun prev -> prev.ShotsOnTarget)
        let result = float(shotsOnTarget) + (float(shots) * 0.25) + 100.0
        if count < 1 then 
            90.0
        elif count < 5 then 
            result * 0.5
        else result

let mutable uavgjort = 0

let predict game allGames variable = 
        let homeScore = 0.45 * (getPreviousGames game.Date game.HomeTeam allGames variable |> getTeamScore)
        let awayScore = 0.3 * (getPreviousGames game.Date game.AwayTeam allGames variable |> getTeamScore)
              
        if Math.Abs(Math.Abs(homeScore) - Math.Abs(awayScore)) < 8.0 then
            uavgjort <- 1 + uavgjort
            U       
        elif homeScore > awayScore then 
            H    
        else 
            B

let predictionHolds game allGames variable = 
        let prediction = predict game allGames variable
        let result = getResult game
        result = prediction



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
      
    let sample = 600
   
    for i = 10 to 35 do
        let correctPredictions = allGames |> Seq.take sample |> Seq.filter(fun game -> predictionHolds game allGames i) |> Seq.length  
        printf "%i: %f \n" i (float(correctPredictions)/float(sample)*100.0)

    
   
//    printf "%f \n" (float(uavgjort)/float(sample))
    let s = Console.ReadLine()
    0 // return an integer exit code
