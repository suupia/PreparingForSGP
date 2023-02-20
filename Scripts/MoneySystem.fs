namespace Scripts

module MoneySystemModule =
    //ゲームで使う数字の型を定義
    type Amount = int

    //Buyによって減る
    type IResource =
        abstract member Amount: Amount


    type IBuyable =
        abstract member Buy: IResource -> Amount



module MoneyModule =
    open MoneySystemModule

    type Money(init: Amount) =
        let mutable amount = init
        member this.Amount = amount

        interface IResource with
            member this.Amount = amount

        member this.Decrement delta =
            amount <- amount - delta
            amount




module FacilityModule =
    open MoneySystemModule

    type FacilityData = { Name: string; Price: Amount }


    type Facility(facilityData: FacilityData) =

        let mutable ownedNum = 0
        member this.OwnedNumber = ownedNum

        interface IBuyable with
            member this.Buy resource =
                resource.Amount - (facilityData.Price )

        member this.IncrementOwnedNumber increment = ownedNum <- ownedNum + increment




module ThiefModule =
    open MoneySystemModule

    type Id = int
    type Level = int
    type ThiefData = { Id :Id ; Level: Level ; Reward:Level->Amount} 

    let getThiefName id = 
        match id with   
           | 0 -> "No Setting"
           | x -> $"Thief{x}" //後で詳細を決める


    type Thief(thiefData: ThiefData,caughtThiefCollection :CaughtThiefContainer) =

        member this.Id = thiefData.Id

        member this.Reward = thiefData.Level |> thiefData.Reward

        member this.OnCaught = 
            caughtThiefCollection.Add this

    and CaughtThiefContainer() = 

        let mutable thiefs = List.empty : list<Thief>

        member this.Add thief = 
            thiefs <- thief::thiefs

        member this.TotalCaught = 
            thiefs |> List.length

        member this.TotalCaughtOfSpecificType targetThief =
            thiefs |> List.filter (fun thief -> thief.Id = targetThief.Id) 
                   |> List.length

        member this.TotalReward =
            thiefs |> List.map (fun thief -> thief.Reward)
                   |> List.sum

