module Predictions

open Game
open Odds

let getOutcomeDistribution results result totalGames = 
        float(results |> Seq.filter (fun game -> game.Result = result) |> Seq.length ) / totalGames

let getTeamPrediction (prevGames:seq<PlayedGame>) venue = 
       let totalGames = prevGames |> Seq.length |> float
       let winPercentage = getOutcomeDistribution prevGames Win totalGames
       let drawPercentage = getOutcomeDistribution prevGames Draw totalGames
       let lossPercentage = getOutcomeDistribution prevGames Loss totalGames
       if venue = Home then { Home = winPercentage; Draw = drawPercentage; Away = lossPercentage }
       else { Home = lossPercentage; Draw = drawPercentage; Away = winPercentage }