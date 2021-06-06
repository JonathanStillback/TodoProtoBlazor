using System;
using System.Collections.Generic;

namespace Models
{
    public class Todo
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public TodoStatus Status {get; set;}
        public List<string> Comments {get; set;}
	}

    public enum TodoStatus
    {
        New = 1,
        Started = 2,
        Waiting = 3,
        Finished = 4
    }
}
