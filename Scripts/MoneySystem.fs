namespace FSharpProject.MoneySystem

module MoneySystemModule =
    //ゲームで使う数字の型を定義
    type Amount = int //主にお金に使用
    type Level = int 
    type Cost =  Level -> Amount

    //Buyによって減る
    type IResource =
        abstract member Amount: Amount

    type IBuyable =
        abstract member Buy: IResource -> IResource

    type IIncrement = 
        abstract member IIncrement : int -> int

    type IDecrement = 
        abstract member IDecrement : int -> int



module MoneyModule =
    open MoneySystemModule

    type Money(init: Amount) =
        let  amount = init
        member this.Amount = amount

        interface IResource with
            member this.Amount = amount

     



module FacilityModule =
    open MoneySystemModule
    open MoneyModule

    type FacilityRecord = { Name: string; Price: Amount }

    type Facility(ownedNum, facilityRecord: FacilityRecord) =

        let  ownedNum = ownedNum
        member this.OwnedNumber = ownedNum

        interface IBuyable with
            member this.Buy resource =
               new Money(  resource.Amount - facilityRecord.Price )

        
        member this.Name = facilityRecord.Name
        member this.Price = facilityRecord.Price

        member this.IncrementOwnedNumber increment =
            new Facility( ownedNum + increment,facilityRecord)



module StoreModule =
    open MoneySystemModule
    open MoneyModule

    type Store(level:Level,cost:Cost) =
        let level = level : Level
        let cost = cost 

        member this.Level = level
        member this.LevelUp (resource:IResource) = 
            if resource.Amount >= cost level
             then (new Store(level+1,cost), new Money(resource.Amount-cost level))
             else (new Store(level,cost), new Money(resource.Amount))
        member this.Cost =
            cost  level





module ThiefModule =
    open MoneySystemModule

    type Id = int
    type Level = int
    type ThiefRecord = { Id :Id ; Level: Level ; Reward:Level->Amount} 

    let getThiefName id = 
        match id with   
           | 0 -> "No Setting"
           | x -> $"Thief{x}" //後で詳細を決める


    type Thief(thiefRecord: ThiefRecord) =

        member this.Id = thiefRecord.Id

        member this.Reward = thiefRecord.Level |> thiefRecord.Reward

        member this.OnCaught (caughtThiefCollection:CaughtThiefContainer) = 
            caughtThiefCollection.Add(this)

    and CaughtThiefContainer(thiefs) = 

        let thiefs = thiefs

        member this.Add thief = 
            new CaughtThiefContainer(thief::thiefs )

        member this.TotalCaught = 
            thiefs |> List.length

        member this.TotalCaughtOfSpecificType targetThief =
            thiefs |> List.filter (fun thief -> thief.Id = targetThief.Id) 
                   |> List.length

        member this.TotalReward =
            thiefs |> List.map (fun thief -> thief.Reward)
                   |> List.sum

