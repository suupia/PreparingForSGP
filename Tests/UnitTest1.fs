module Tests

open System
open NUnit.Framework
open FSharpProject.MoneySystem
open MoneySystemModule
open MoneyModule
open FacilityModule
open StoreModule
open ThiefModule


[<TestFixture>]
type PreparationPhaseTest() =

    //Money
    [<Test>]
    member this.Buy_WithPrice100_Decrement100() =
        let money = Money(1000)
        let monitoringCamera = { Name = "äƒéãÉJÉÅÉâ"; Price = 100 }
        let facility1 = Facility(0,monitoringCamera)
        let money = (facility1 :> IBuyable).Buy(money)
        Assert.That(money.Amount, Is.EqualTo(900))
        let money = (facility1 :> IBuyable).Buy(money)
        Assert.That(money.Amount, Is.EqualTo(800))

    //Facility
    [<Test>]
    member this.IncrementOwnedNumber_WithFacility1_IncremetOwnedNumber() =
        let monitoringCamera = { Name = "äƒéãÉJÉÅÉâ"; Price = 100 }
        let facility = Facility(0,monitoringCamera)
        let facility = facility.IncrementOwnedNumber 1
        let afterOwnedNumber = facility.OwnedNumber
        Assert.That(afterOwnedNumber, Is.EqualTo(1))
        let facility = facility.IncrementOwnedNumber 1
        let afterOwnedNumber2 = facility.OwnedNumber
        Assert.That(afterOwnedNumber2, Is.EqualTo(2))
        let facility = facility.IncrementOwnedNumber 1
        let afterOwnedNumber3 = facility.OwnedNumber
        Assert.That(afterOwnedNumber3, Is.EqualTo(3))

    //Store
    [<Test>]
    member this.StoreLevelUp_WithStore_IncrementStoreCost() =
        let money = Money(100000)

        let initCost = 100
        let cost = fun level -> 2 * initCost * level + initCost
        let store = Store(0, cost)
        Assert.That(store.Cost, Is.EqualTo(100))
        let (store,money) = store.LevelUp(money)
        Assert.That(store.Cost, Is.EqualTo(300))
        let (store,money) = store.LevelUp(money)
        Assert.That(store.Cost, Is.EqualTo(500))

    [<Test>]
    member this.StoreLevelUp_WithNotEnoughResource_IncrementLevelCorrectly() =

        let money = Money(400)
        let initCost = 100
        let cost = fun level -> 2 * initCost * level + initCost
        let store = Store(0, cost)
        Assert.That(store.Level, Is.EqualTo(0))
        let (store,money) = store.LevelUp(money)
        Assert.That(store.Level, Is.EqualTo(1))
        let (store,money) = store.LevelUp(money)
        Assert.That(store.Level,Is.EqualTo(2))
        let (store,money) = store.LevelUp(money)
        Assert.That(store.Level,Is.EqualTo(2))


type ObservationPhaseTest() =

    let weakThiefRecord =
        { Id = 1
          Level = 2
          Reward = (fun x -> x * 2) }
        : ThiefRecord

    let normalThiefRecord =
        { Id = 2
          Level = 3
          Reward = (fun x -> x * 6 - 10) }
        : ThiefRecord

    [<Test>]
    member this.Reward_WithVailedFunction_ReturnsReward() =
        let caughtThiefContainer = CaughtThiefContainer(List.empty)

        let thief = Thief(weakThiefRecord)
        Assert.That(thief.Reward, Is.EqualTo(4))

        let thief = Thief(normalThiefRecord)
        Assert.That(thief.Reward, Is.EqualTo(8))

    [<Test>]
    member this.TotalCaught_WithOnCath_IncrementOne() =
        let caughtThiefContainer = CaughtThiefContainer(List.empty)
        let thief1 = Thief(weakThiefRecord)
        let thief2 = Thief(weakThiefRecord)
        let thief3 = Thief(normalThiefRecord)


        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(0))
        let caughtThiefContainer = thief1.OnCaught caughtThiefContainer
        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(1))
        let caughtThiefContainer = thief2.OnCaught caughtThiefContainer
        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(2))
        let caughtThiefContainer = thief3.OnCaught caughtThiefContainer
        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(3))

    [<Test>]
    member this.TotalCaughtOfSpecificType_WithOnCath_IncrementOne() =
        let caughtThiefContainer = CaughtThiefContainer(List.empty)
        let thief1 = Thief(weakThiefRecord)
        let thief2 = Thief(weakThiefRecord)
        let thief3 = Thief(normalThiefRecord)

        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefRecord), Is.EqualTo(0))
        let caughtThiefContainer = thief1.OnCaught caughtThiefContainer
        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefRecord), Is.EqualTo(1))
        let caughtThiefContainer = thief2.OnCaught caughtThiefContainer
        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefRecord), Is.EqualTo(2))
        let caughtThiefContainer = thief3.OnCaught caughtThiefContainer
        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefRecord), Is.EqualTo(2))


    [<Test>]
    member this.TotalReward_WithMixedThiefs_ReturnsCorrectTotalReward() =
        let caughtThiefContainer = CaughtThiefContainer(List.empty)
        let thief1 = Thief(weakThiefRecord)
        let thief2 = Thief(weakThiefRecord)
        let thief3 = Thief(normalThiefRecord)

        let caughtThiefContainer = caughtThiefContainer.Add thief1
        Assert.That(caughtThiefContainer.TotalReward, Is.EqualTo(4))

        let caughtThiefContainer = caughtThiefContainer.Add thief2
        Assert.That(caughtThiefContainer.TotalReward, Is.EqualTo(8))

        let caughtThiefContainer = caughtThiefContainer.Add thief3
        Assert.That(caughtThiefContainer.TotalReward, Is.EqualTo(16))
