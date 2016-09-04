module Odds

open System

type Odds = {
    H : float
    U : float
    B : float
}

type Prize = {
    Balance : float
    Spent: float
}

type Prediction = {
    Home : double
    Draw : double
    Away : double
}


let createOdds prediction treshold =
        { H = treshold/prediction.Home; U = treshold/prediction.Draw; B = treshold/prediction.Away }