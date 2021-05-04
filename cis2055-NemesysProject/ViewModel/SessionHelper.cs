using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace cis2055_NemesysProject.ViewModel
{
    public static class SessionHelper
    {
        
        //Sets an object into a Session Variable
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            //object ser = null;
           session.SetString(key, JsonConvert.SerializeObject(value));
        }


        //Gets Session Variable to Object
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
