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

type Game = {
    Date : string
    Division : string
    HomeTeam : Team
    AwayTeam : Team
  }

type GamesFile = CsvProvider<"./data/15.csv">

let gameList = GamesFile.GetSample()

let getGames = gameList.Rows 
            |> Seq.map ( fun c -> { Division = c.Div; Date = c.Date; HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF }} ) 
    

[<EntryPoint>]
let main argv =
    
    let games = getGames
    let count = Seq.length games

    printf "%i" count
    let s = Console.ReadLine()
    0 // return an integer exit code
