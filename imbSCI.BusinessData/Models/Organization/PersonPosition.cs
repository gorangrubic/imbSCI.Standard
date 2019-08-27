using System;

namespace imbSCI.BusinessData.Models.Organization
{
    /// <summary>
    /// Position of an employee
    /// </summary>
    [Flags]
    public enum PersonPosition
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.none'
        none = 0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.none'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Unknown'
        Unknown = 1,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Unknown'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.GeneralManager'
        GeneralManager = 2,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.GeneralManager'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Owner'
        Owner = 4,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Owner'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Manager'
        Manager = 8,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Manager'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Sales'
        Sales = 16,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Sales'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Procurement'
        Procurement = 32,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Procurement'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Administation'
        Administation = 64,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Administation'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Technical'
        Technical = 128,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Technical'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Production'
        Production = 256,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Production'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Research'
        Research = 512,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Research'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Other'
        Other = 1024,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Other'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Support'
        Support = 2048,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Support'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Engineer'
        Engineer = 4096,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Engineer'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Operator'
        Operator = 8192,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.Operator'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.SalesManager'
        SalesManager = Sales | Manager,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.SalesManager'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.TechnicalSupport'
        TechnicalSupport = Technical | Support,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.TechnicalSupport'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.MachineOperator'
        MachineOperator = Production | Operator,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.MachineOperator'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.ProductionWorker'
        ProductionWorker = Production | Other,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.ProductionWorker'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.ProductionManager'
        ProductionManager = Production | Manager,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.ProductionManager'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.ProductionEngineer'
        ProductionEngineer = Production | Engineer
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PersonPosition.ProductionEngineer'
    }
}