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

let bet game allGames variable = 
        let homePrediction = getTeamPrediction (getPreviousHomeGames game.Date game.HomeTeam allGames 14) Home
        let awayPrediction = getTeamPrediction (getPreviousAwayGames game.Date game.AwayTeam allGames 14) Away
       
        let prediction = { Home = (homePrediction.Home + awayPrediction.Home)/2.0;  Draw = (homePrediction.Draw + awayPrediction.Draw)/2.0; Away = (homePrediction.Away + awayPrediction.Away)/2.0 }   
        let odds = createOdds prediction variable
        
        let outcome = getOutcome game
        
        let amount = 100.0

        let homebalance, homeSpent = betOnGame amount odds.H game.Odds.H (outcome = H)
        let drawbalance, drawSpent = betOnGame amount odds.U game.Odds.U (outcome = U)
        let awaybalance, awaySpent = betOnGame amount odds.B game.Odds.B (outcome = B)
        


        let result = { Balance = homebalance + drawbalance + awaybalance; Spent =  homeSpent + drawSpent + awaySpent }
        printf "%s - %s   ->  %f \n" game.HomeTeam.Name game.AwayTeam.Name result.Balance
        result