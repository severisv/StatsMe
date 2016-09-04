module Models

type Parameters = { OddsThreshold : float }
    with
    member this.print = 
            printf "--------------------------------------------------\n{ OddsThreshold: %f } \n" this.OddsThreshold


type Seasons = S14 | S15 | S16 | S17

type Leagues = PL | BL | FR | SP

