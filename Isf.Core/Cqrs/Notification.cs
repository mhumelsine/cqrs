using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class Notification
    {
        public static Notification OK = new Notification();

        private Dictionary<string, List<string>> errorDictionary =
            new Dictionary<string, List<string>>();

        public bool HasErrors { get { return errorDictionary.Count > 0; } }

        public IEnumerable<string> ErrorList
        {
            get
            {
                return errorDictionary.Values
                    .SelectMany(propertyList =>
                        propertyList.Select(error => error));
            }
        }

        public Dictionary<string, List<string>> ErrorDictionary
        {
            get
            {
                //return a copy of the dictionary so it can't be modified
                return errorDictionary
                    .ToDictionary(entry => entry.Key, entry => entry.Value);
            }
        }

        public Notification AddError(string error)
        {
            AddError(error, string.Empty);
            return this;
        }
        public Notification AddError(string error, string property)
        {
            List<string> errorList;

            //if not found create
            if (!errorDictionary.TryGetValue(property, out errorList))
            {
                errorList = new List<string>();
                errorDictionary.Add(property, errorList);
            }

            errorList.Add(error);

            return this;
        }

        public static Notification Join(params Notification[] notifications)
        {
            var notification = new Notification();

            foreach (var item in notifications)
            {
                foreach (var propertyError in item.ErrorDictionary)
                {
                    foreach (var error in propertyError.Value)
                    {
                        notification.AddError(error, propertyError.Key);
                    }
                }
            }

            return notification;
        }
    }
}
