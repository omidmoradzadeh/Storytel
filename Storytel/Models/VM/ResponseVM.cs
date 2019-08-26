using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.VM
{
    public class ResponseVM
    {
        public string Title { get; }
        public bool HasError { get; } = true;
        public List<string> ErrorItems { get; } = new List<string>();
        public object Data { get; }

        public ResponseVM(string title)
        {
            Title = title;
            HasError = true;
        }

        public ResponseVM(bool hasError, object data)
        {
            Title = hasError ? "Operation has error" : "Operation has succeeded";
            HasError = hasError;
            Data = data;
        }

        public ResponseVM(string title, bool hasError, object data)
        {
            Title = title;
            HasError = hasError;
            Data = data;
        }

        public ResponseVM(string title, bool hasError, List<string> errorItems)
        {
            Title = title;
            HasError = hasError;
            ErrorItems = errorItems;
        }
    }


}
