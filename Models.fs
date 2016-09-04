module Models
open Util

type Parameters = { OddsThreshold : float }
    with
    member this.print = 
            printf "--------------------------------------------------\n{ OddsThreshold: %f } \n" this.OddsThreshold


type League = PL | BL | FR | SP
    with member this.name = GetUnionCaseName this
                        

type Season = {
    Year : int
    League : League
}

