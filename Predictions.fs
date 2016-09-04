module Predictions

open Game
open Odds
open Models

let getOutcomeDistribution results result totalGames = 
        float(results |> Seq.filter (fun game -> game.Result = result) |> Seq.length ) / totalGames

let getTeamPrediction (prevGames:seq<PlayedGame>) venue = 
       let totalGames = prevGames |> Seq.length |> float
       let winPercentage = getOutcomeDistribution prevGames Win totalGames
       let drawPercentage = getOutcomeDistribution prevGames Draw totalGames
       let lossPercentage = getOutcomeDistribution prevGames Loss totalGames
       if venue = Home then { Home = winPercentage; Draw = drawPercentage; Away = lossPercentage }
       else { Home = lossPercentage; Draw = drawPercentage; Away = winPercentage }


let createOdds prediction parameters =
        let threshold = parameters.OddsThreshold
        { H = threshold/prediction.Home; U = threshold/prediction.Draw; B = threshold/prediction.Away }

let createPrediction homePrediction awayPrediction parameters =
      let prediction = { Home = (homePrediction.Home + awayPrediction.Home)/2.0;  
                           Draw = (homePrediction.Draw + awayPrediction.Draw)/2.0; 
                           Away = (homePrediction.Away + awayPrediction.Away)/2.0 }   
        
      createOdds prediction parameters