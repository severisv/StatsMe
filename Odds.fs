module Odds

open System
open Models

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

type EndResult = {
    Parameters: Parameters
    Balance: float
    TotalSpent : float
    } with 
    member this.score = 
            ((this.Balance + this.TotalSpent)/ this.TotalSpent)
    
    member this.print = 
            printf "--------------------------------------------------\n"
            this.Parameters.print
            printf "{ Score: %f  Total spent: %f }\n" this.score this.TotalSpent
    
    override this.ToString() = 
        sprintf "%f,%s" this.score (this.Parameters.ToString())