using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Npgg.ApiRouter
{
    public class QueryStringCollection : MultiKeyValueCollection
    {

        public override string ToString()
        {
            if (this.Count == 0)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach(var kvp in this)
            {
                foreach (var value in kvp.Value)
                {
                    stringBuilder.AppendFormat("{0}={1}&", kvp.Key, value);
                }
            }

            if(stringBuilder.Length>1)
                stringBuilder.Length--;

            return stringBuilder.ToString();
        }
    }
    public class HeaderCollection : MultiKeyValueCollection
    {

    }

    public class ResponseHeaderCollection : Dictionary<string,string>
    {
        public void CheckCors()
        {

        }
    }



    public class MultiKeyValueCollection: Dictionary<string, ICollection<string>>
    {
        /**
         * 자신이 가지고 있는 헤더들의 소문자 버전 키값을 생성한다.
         */
        public void CloneToLowercase()
        {
            foreach(var key in this.Keys.ToArray())
            {
                var lowerKey = key.ToLower();

                if (lowerKey != key)
                {
                    this[lowerKey] = this[key];
                }
            }
        }

        public T FirstOrDefault<T>(string key, T value)
        {
            if(this.TryGetFirst<T>(key, out var result) == false)
            {
                result = value;
            }
            return result;
        }

        public void CloneTo(MultiKeyValueCollection target)
        {
            foreach(var kvp in this)
            {
                target.Add(kvp.Key, kvp.Value);
            }
        }

        public void AddValue<T>(string key, T value)
        {
            if (this.TryGetValue(key, out var list) == false)
            {
                list = new List<string>();
                base.Add(key, list);
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            var stringValue = converter.ConvertToString(value);
            list.Add(stringValue);
        }

        public void SetValue<T>(string key, T value)
        {
            if (this.TryGetValue(key, out var list) == false)
            {
                list = new List<string>();
                base.Add(key, list);
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            var stringValue = converter.ConvertToString(value);
            list.Add(stringValue);
        }

        public void AddValue(string key, DateTime value)
        {
            if (this.TryGetValue(key, out var list) == false)
            {
                list = new List<string>();
                base.Add(key, list);
            }

            var stringValue = value.ToString("u");
            list.Add(stringValue);
        }

        public List<T> GetValues<T>(string key)
        {
            if (this.TryGetValue(key, out var list) == false || list.Count == 0)
            {
                return null;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));


            var result = list.Select(value => converter.ConvertFromString(value)).Cast<T>().ToList();

            return result;
        }

        public bool TryGetFirst<T>(string key, out T result)
        {
            if (this.TryGetValue(key, out var list) == false || list.Count == 0)
            {
                result = default(T);
                return false;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));

            result = (T)converter.ConvertFromString(list.First());
            return true;
        }


    }
}
