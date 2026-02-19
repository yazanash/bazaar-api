using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public enum TransactionType
    {
        Featured,
        AdUsage
    }
    public enum PostDateFilter
    {
        AnyTime,
        Past24,
        PastWeek,
        PastMonth,
    }
    public enum GenderType
    {
        NotSpecified,
        Male,
        Female,
    }
    public enum CarBodyType
    {
        NotSpecified,
        Sedan,
        Hatchback,
        Station,
        Coupe,
        Convertible,
        SUV,
        OffRoad
    }
    public enum MotorBodyType
    {
        NotSpecified,
        Chrome,
        CarbonFiber,
        Steel,
        Aluminum
    }
    public enum FuelType
    {
        NotSpecified,
        Diesel,
        Gasoline,
        Electric,
        Hybrid
    }
    public enum SellerType
    {
        NotSpecified,
        Owner,
        Broker,
        Agency
    }
    public enum Category
    {
        NotSpecified,
        Passenger,
        Motorcycles,
        Trucks
    }
    public enum Transmission
    {
        NotSpecified,
        Manual,
        Automatic,
        CVT, 
        DualClutch
    }
    public enum MotorTransmission
    {
        NotSpecified,
        Manual,
        Automatic,
        SemiAutomatic
    }
    public enum RegistrationType
    {
        NotSpecified,
        PublicReg,
        PrivateReg
    }
    public enum DriveSystem
    {
        NotSpecified,
        FWD,
        RWD,
        AWD,
        FourWD
    }
    public enum UsageType
    {
        NotSpecified,
        InternalUsage,
        ExternalUsage,
        Personal
    }
    public enum TrucksUsageType
    {
        NotSpecified,
        Personal,        
        HeavyTransport,  
        WaterTanker,  
        Refrigerated,   
        FurnitureMoving, 
        Construction    
    }
    public enum PubStatus
    {
        NotSpecified,
        Accepted,
        Rejected,
        Pending,
        Closed
    }
    public enum TruckBodyType
    {
        NotSpecified,
        Refrigerated,
        Closed,  
        Open,   
        Tanker, 
        Chassis,     
        Tipper, 
        Pickup 
    }
}
