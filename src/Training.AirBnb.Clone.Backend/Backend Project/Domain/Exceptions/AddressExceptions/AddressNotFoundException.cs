﻿namespace Backend_Project.Domain.Exceptions.AddressExceptions
{
    public class AddressNotFoundException : Exception
    {
        public AddressNotFoundException(string message) : base(message) { }
    }
}
