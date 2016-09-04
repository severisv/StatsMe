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
    let allGames = GameRepository.get PL
      
    let sample = 500
    
     
    for i = 9 to 9 do
        let parameters = { OddsThreshold = (float i)/10.0 }
        let predictions = allGames |> Seq.take (sample) |> Seq.map(fun game -> bet game allGames parameters)

        let balance = predictions |> Seq.sumBy (fun res -> res.Balance)
        let spent = predictions |> Seq.sumBy (fun res -> res.Spent)
        parameters.print
        printf "%f - %f   ->  %f \n" balance spent ((balance + spent)/ spent)
    
   
    let s = Console.ReadLine()
    0 // return an integer exit code
