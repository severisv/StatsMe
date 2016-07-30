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
    Date : DateTime
    Division : string
    HomeTeam : Team
    AwayTeam : Team
  }


type GamesFile = CsvProvider<"./data/15.csv">

let mapRows (rows:seq<GamesFile.Row>) = rows |> Seq.map ( fun c -> { Division = c.Div; Date = DateTime.Parse c.Date; HomeTeam = { Name = c.HomeTeam; Score = c.FTHG; Shots = c.HS; ShotsOnTarget = c.HST; Corners = c.HC; Fouls = c.HF }; AwayTeam = { Name = c.AwayTeam; Score = c.FTAG; Shots = c.AS; ShotsOnTarget = c.AST; Corners = c.AC; Fouls = c.AF }} ) 



let games15 = GamesFile.Load("../../data/15.csv").Rows |> mapRows
let games16 = GamesFile.Load("../../data/16.csv").Rows |> mapRows
 
 
[<EntryPoint>]
let main argv =
    let games = Seq.append games15 games16
   
    for game in games |> Seq.take 5 do
        printf "%s \n" (game.Date.ToShortDateString())
        
    let s = Console.ReadLine()
    0 // return an integer exit code
