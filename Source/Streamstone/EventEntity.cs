﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.Cosmos.Table;

namespace Streamstone
{
    class EventEntity : TableEntity
    {
        public const string RowKeyPrefix = "SS-SE-";

        public EventEntity()
        {
            Properties = EventProperties.None;
        }

        public EventEntity(Partition partition, RecordedEvent @event)
        {
            PartitionKey = partition.PartitionKey;
            RowKey = partition.EventVersionRowKey(@event.Version);
            Properties = @event.Properties;
            Version = @event.Version;   
        }

        public int Version                  { get; set; }
        public EventProperties Properties   { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            Properties = EventProperties.ReadEntity(properties);
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var result = base.WriteEntity(operationContext);
            Properties.WriteTo(result);
            return result;
        }
    }
}