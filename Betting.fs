module Betting

open Game
open Games
open Odds
open Predictions

let betOnGame amount (prediction:float) (provided:float) (didWin:bool) =
        if provided > prediction then 
            if didWin then (provided * amount), amount
            else -amount, amount
        else 0.0, 0.0

let bet game allGames parameters = 
        let homePrediction = getTeamPrediction (getPreviousHomeGames game.Date game.HomeTeam allGames 14) Home
        let awayPrediction = getTeamPrediction (getPreviousAwayGames game.Date game.AwayTeam allGames 14) Away
       
        let prediction = createPrediction homePrediction awayPrediction parameters
        
        let outcome = getOutcome game
        
        let amount = 100.0

        let homebalance, homeSpent = betOnGame amount prediction.H game.Odds.H (outcome = H)
        let drawbalance, drawSpent = betOnGame amount prediction.U game.Odds.U (outcome = U)
        let awaybalance, awaySpent = betOnGame amount prediction.B game.Odds.B (outcome = B)
        


        let result = { Balance = homebalance + drawbalance + awaybalance; Spent =  homeSpent + drawSpent + awaySpent }
        printf "%s - %s   ->  %f \n" game.HomeTeam.Name game.AwayTeam.Name result.Balance
        result