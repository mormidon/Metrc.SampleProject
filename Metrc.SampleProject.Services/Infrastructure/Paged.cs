using System;
using System.Collections.Generic;
using System.Linq;

namespace Metrc.SampleProject.Services.Infrastructure
{
    public sealed class Paged<T>
    {
        public Paged(IEnumerable<T> data, Int64? total = null)
        {
            // This is an attempt to skip the conversion to array if the IEnumerable (data) is already an array
            var dataArray = data as T[] ?? data.ToArray();
            Data = dataArray;
            Total = total ?? dataArray.Length;
        }

        public T[] Data { get; set; }
        public Int64 Total { get; set; }
    }
}
