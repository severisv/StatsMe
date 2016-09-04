module Models
open Util


type League = PL | BL | FR | SP
    with member this.name = GetUnionCaseName this

type Parameters = { OddsThreshold : float; League: League }
    with
    member this.print = 
            printf "--------------------------------------------------\n{ League: %s   OddsThreshold: %f } \n" this.League.name this.OddsThreshold
                                    

type Season = {
    Year : int
    League : League
}

let getLeagues = [ PL; BL; FR; SP ]
