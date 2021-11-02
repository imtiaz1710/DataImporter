﻿using System;
using System.Runtime.Serialization;

namespace DataImporter.Excel.Exceptions
{
    [Serializable]
    internal class DuplicateNameException : Exception
    {
        public DuplicateNameException()
        {
        }

        public DuplicateNameException(string message) : base(message)
        {
        }

        public DuplicateNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}