﻿using SpacedRepApp.Share;
using System;
using System.Collections.Generic;

namespace SpacedRepApp.Controllers
{
    public class NoteDto
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Contents { get; set; }
        public bool Revised { get; set; } = false;
        public long CategoryId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}