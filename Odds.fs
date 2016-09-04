module Odds

open System

type Prediction = {
    Home : double
    Draw : double
    Away : double
}

type Odds = {
    H : float
    U : float
    B : float
}

type Prize = {
    Balance : float
    Spent: float
}