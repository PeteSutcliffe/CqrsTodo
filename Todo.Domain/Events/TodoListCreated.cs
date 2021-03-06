﻿using System;

namespace Todo.Domain.Events
{
    [Serializable]
    public class TodoListCreated : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
    }
}