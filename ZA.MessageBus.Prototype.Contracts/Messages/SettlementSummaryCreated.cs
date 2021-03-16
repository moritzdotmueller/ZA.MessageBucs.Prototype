using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZA.MessageBus.Prototype.Contracts.Messages
{
    public class SettlementSummaryCreated : MessageBase
    {
        public string SomeStringProperty { get; init; }
        public decimal SomeDecimalProperty { get; init; }
    }
}
