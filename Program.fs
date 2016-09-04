open System
open Models
open Odds
open Game
open Games
open Predictions
open Betting
 

let count result predictions =
    float(predictions |> Seq.filter(fun res -> res = result) |> Seq.length) / float(predictions |> Seq.length)


        
[<EntryPoint>]
let main argv =

    for league in Models.getLeagues do

        let allGames = GameRepository.get league
      
        let sample = 720
        
        for j = 0 to 5 do
            for i = 7 to 9 do
                for k = 5 to 10 do
                    let parameters = { 
                        League = league; 
                        OddsThreshold = (float i)/4.0;
                        AwayToHomeRatio = float(j)/5.0;
                        PreviousGameCount = k*2;
                        Score = 0.0
                        }
                    let predictions = allGames |> Seq.take (sample) |> Seq.map(fun game -> bet game allGames parameters)

                    let balance = predictions |> Seq.sumBy (fun res -> res.Balance)
                    let spent = predictions |> Seq.sumBy (fun res -> res.Spent)
        
                    let result = { Balance = balance; TotalSpent = spent; Parameters = parameters }
                    result.print
             
                    ParameterRepository.add result                  
          

    
   
    let s = Console.ReadLine()
    0 // return an integer exit code
