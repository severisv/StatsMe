module Games
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


