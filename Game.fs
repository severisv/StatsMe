module Game

open System
open Odds

type Team = {
    Name : string
    Score : int
    Shots : int
    ShotsOnTarget : int
    Corners : int
    Fouls : int
}

type Result =  Win | Draw | Loss

let getResult goalsFor goalsAgainst =
    if goalsFor > goalsAgainst then Win
    elif goalsFor < goalsAgainst then Loss
    else Draw

type PlayedGame = {
    Shots : int
    ShotsOnTarget : int
    Result : Result
}

type Game = {
    Date : DateTime
    Division : string
    HomeTeam : Team
    AwayTeam : Team
    Odds : Odds
  }

type Outcome =  H | U | B

let getOutcome game = 
    if game.HomeTeam.Score > game.AwayTeam.Score then H
    elif game.HomeTeam.Score < game.AwayTeam.Score then B
    else U


