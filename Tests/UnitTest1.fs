module Tests

open System
open NUnit.Framework
open Scripts
open MoneySystemModule
open MoneyModule
open FacilityModule
open ThiefModule


[<TestFixture>]
type PreparationPhaseTest() =

    [<Test>]
    member this.Buy_WithPrice100_Decrement100() =
        let money = Money(1000)
        let monitoringCamera = { Name = "äƒéãÉJÉÅÉâ"; Price = 100 }
        let facility1 = Facility(monitoringCamera)
        let aftermoney = (facility1 :> IBuyable).Buy(money)
        Assert.That(aftermoney, Is.EqualTo(900))

    [<Test>]
    member this.IncrementOwnedNumber_WithFacility1_IncremetOwnedNumber() =
        let monitoringCamera = { Name = "äƒéãÉJÉÅÉâ"; Price = 100 }
        let facility1 = Facility(monitoringCamera)
        facility1.IncrementOwnedNumber 1
        let afterOwnedNumber = facility1.OwnedNumber
        Assert.That(afterOwnedNumber, Is.EqualTo(1))
        facility1.IncrementOwnedNumber 1
        let afterOwnedNumber2 = facility1.OwnedNumber
        Assert.That(afterOwnedNumber2, Is.EqualTo(2))
        facility1.IncrementOwnedNumber 1
        let afterOwnedNumber3 = facility1.OwnedNumber
        Assert.That(afterOwnedNumber3, Is.EqualTo(3))

[<TestFixture>]
type ObservationPhaseTest() =

    let weakThiefData = { Id = 1; Level = 2; Reward = (fun x -> x*2) } : ThiefData
    let normalThiefData = { Id = 2 ; Level = 3; Reward = (fun x -> x*6 - 10) } : ThiefData

    [<Test>]
    member this.Reward_WithVailedFunction_ReturnsReward() =
        let caughtThiefContainer = CaughtThiefContainer()

        let thief = Thief(weakThiefData,caughtThiefContainer)
        let reward = thief.Reward 
        Assert.That(reward, Is.EqualTo(4))

        let thief = Thief(normalThiefData,caughtThiefContainer)
        let reward = thief.Reward 
        Assert.That(reward, Is.EqualTo(8))
    
    [<Test>]
    member this.TotalCaught_WithOnCath_IncrementOne() =
        let caughtThiefContainer = CaughtThiefContainer()
        let thief1 = Thief(weakThiefData,caughtThiefContainer)
        let thief2 = Thief(weakThiefData,caughtThiefContainer)
        let thief3 = Thief(normalThiefData,caughtThiefContainer)


        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(0))
        thief1.OnCaught
        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(1))
        thief2.OnCaught
        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(2))
        thief3.OnCaught
        Assert.That(caughtThiefContainer.TotalCaught, Is.EqualTo(3))

    [<Test>]
    member this.TotalCaughtOfSpecificType_WithOnCath_IncrementOne() =
        let caughtThiefContainer = CaughtThiefContainer()
        let thief1 = Thief(weakThiefData,caughtThiefContainer)
        let thief2 = Thief(weakThiefData,caughtThiefContainer)
        let thief3 = Thief(normalThiefData,caughtThiefContainer)

        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefData), Is.EqualTo(0))
        thief1.OnCaught
        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefData), Is.EqualTo(1))
        thief2.OnCaught
        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefData), Is.EqualTo(2))
        thief3.OnCaught
        Assert.That(caughtThiefContainer.TotalCaughtOfSpecificType(weakThiefData), Is.EqualTo(2))


    [<Test>]
    member this.TotalReward_WithMixedThiefs_ReturnsCorrectTotalReward() =
        let caughtThiefContainer = CaughtThiefContainer()
        let thief1 = Thief(weakThiefData,caughtThiefContainer)
        let thief2 = Thief(weakThiefData,caughtThiefContainer)
        let thief3 = Thief(normalThiefData,caughtThiefContainer)
        
        caughtThiefContainer.Add thief1
        Assert.That(caughtThiefContainer.TotalReward, Is.EqualTo(4))
       
        caughtThiefContainer.Add thief2
        Assert.That(caughtThiefContainer.TotalReward, Is.EqualTo(8))
       
        caughtThiefContainer.Add thief3
        Assert.That(caughtThiefContainer.TotalReward, Is.EqualTo(16))


