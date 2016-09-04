module Predictions

open Game
open Odds
open Models
open Games

let getOutcomeDistribution results result totalGames = 
        float(results |> Seq.filter (fun game -> game.Result = result) |> Seq.length ) / totalGames

let createTeamPrediction venue prevGames =
       let totalGames = prevGames |> Seq.length |> float
       let winPercentage = getOutcomeDistribution prevGames Win totalGames
       let drawPercentage = getOutcomeDistribution prevGames Draw totalGames
       let lossPercentage = getOutcomeDistribution prevGames Loss totalGames
       if venue = Home then { Home = winPercentage; Draw = drawPercentage; Away = lossPercentage }
       else { Home = lossPercentage; Draw = drawPercentage; Away = winPercentage }

let getTeamPrediction game allGames parameters venue = 
        
       let prevGamesOnSameVenue = 
            if venue = Home then (getPreviousHomeGames game.Date game.HomeTeam allGames parameters.PreviousGameCount)
            else (getPreviousAwayGames game.Date game.AwayTeam allGames parameters.PreviousGameCount)

       let mainPrediction = prevGamesOnSameVenue |> createTeamPrediction venue

       let prevGamesOnOppositeVenue = 
            if venue = Home then (getPreviousAwayGames game.Date game.HomeTeam allGames parameters.PreviousGameCount)
            else (getPreviousHomeGames game.Date game.AwayTeam allGames parameters.PreviousGameCount)

       let coPrediction = prevGamesOnOppositeVenue |> createTeamPrediction venue

       let mainWeight = 2.0-parameters.AwayToHomeRatio
       { Home = (mainPrediction.Home*mainWeight + coPrediction.Home*parameters.AwayToHomeRatio) / 2.0;  
        Draw = (mainPrediction.Draw*mainWeight + coPrediction.Draw*parameters.AwayToHomeRatio) / 2.0;  
        Away = (mainPrediction.Away*mainWeight + coPrediction.Away*parameters.AwayToHomeRatio) / 2.0 }

    


let createOdds prediction parameters =
        let threshold = parameters.OddsThreshold
        { H = threshold/prediction.Home; U = threshold/prediction.Draw; B = threshold/prediction.Away }

let createPrediction homePrediction awayPrediction parameters =
      let prediction = { Home = (homePrediction.Home + awayPrediction.Home)/2.0;  
                           Draw = (homePrediction.Draw + awayPrediction.Draw)/2.0; 
                           Away = (homePrediction.Away + awayPrediction.Away)/2.0 }   
        
      createOdds prediction parameters