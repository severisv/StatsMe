module Models
open Util


type League = PL | BL | FR | SP
    with member this.name = GetUnionCaseName this

let mapLeague str =
    match str with
        | "PL" -> PL
        | "BL" -> BL
        | "FR"-> FR
        | "SP" -> SP

type Parameters = { 
        OddsThreshold : float; 
        PreviousGameCount: int;
        AwayToHomeRatio: float;
        League: League;
        Score: float
        }
    with
    member this.print = 
            printf "--------------------------------------------------\n{ League: %s   OddsThreshold: %f  AwayToHomeRatio: %f  Previous games: %i } \n" 
                this.League.name this.OddsThreshold this.AwayToHomeRatio this.PreviousGameCount
    override this.ToString() = 
        sprintf "%f,%i,%f,%s\n" this.OddsThreshold this.PreviousGameCount this.AwayToHomeRatio this.League.name                               

type Season = {
    Year : int
    League : League
}

let getLeagues = [ PL; FR; BL; SP ]


