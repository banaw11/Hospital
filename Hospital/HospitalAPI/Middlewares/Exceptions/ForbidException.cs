﻿using System;

namespace HospitalAPI.Middlewares.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException(string message) : base(message)
        {
        }
    }
}