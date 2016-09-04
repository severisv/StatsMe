module ParameterRepository

open System
open FSharp.Data
open FSharp.Data.CsvExtensions
open Models
open Odds

type Parameters = CsvProvider<"./data/parameters.csv">


let get (league : League) = 
        let rows = Parameters.GetSample().Rows |> Seq.filter(fun row -> row.League = league.name)

        if (rows |> Seq.length) > 0 then
            rows |> Seq.sortBy(fun row -> -row.Score) 
                |> Seq.take 1 
                |> Seq.map(fun row -> { OddsThreshold = (float row.OddsThreshold); 
                                        PreviousGameCount = row.PreviousGameCount; 
                                        AwayToHomeRatio = (float row.AwayToHomeRatio);
                                        League = (Models.mapLeague row.League);
                                        Score = (float row.Score) })
                |> Seq.head
        else { OddsThreshold = 1.8; PreviousGameCount = 14; AwayToHomeRatio = 0.0;
             League = league; Score = 0.1 }


let save (result : EndResult) =
      let wr = new System.IO.StreamWriter("../../data/parameters.csv", true)
      wr.Write result
      wr.Close()

let add result = 
    if result.TotalSpent > 1.0 then
        let bestCurrentParamSet = get result.Parameters.League
        if result.score > bestCurrentParamSet.Score then
            save result
     
