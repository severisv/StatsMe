module Parameters

type Parameters = {
    OddsThreshold : float
}

let print parameters = 
    printf "--------------------------------------------------\n{ OddsThreshold: %f } \n" parameters.OddsThreshold
