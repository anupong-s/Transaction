using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TransactionModel
{
    public static class ConfigurationManager
    {
        public static Formats Format
        {
            get { return Formats.Instance; }
        }

        public static Pay_In PayIn
        {
            get { return Pay_In.Instance; }
        }

        public static PaymentGateway PaymentGateways
        {
            get { return PaymentGateway.Instance; }
        }

        static Dictionary<string, SettingCache> _cacheDict = new Dictionary<string, SettingCache>();

        class CacheData<TKey, TData>
        {
            internal TKey Key { get; set; }
            internal DateTime CacheDate { get; set; }
            internal TData Data { get; set; }
        }

        class CacheData<T> : CacheData<T, T>
        {
        }

        class SettingCache : CacheData<string>
        {
            internal SettingCache(string key)
            {
                Key = key;
            }

            internal bool NeedUpdate { get { return DateTime.Now.Subtract(CacheDate).TotalMinutes > 10; } }
            internal void Update()
            {
                using (var container = new TransactionModelContainer())
                {
                    string[] keys = Key.Split('.');
                    var cfg = container[keys[0],keys[1]];
                    Data = cfg.Value1;
                }

                CacheDate = DateTime.Now;
            }
        }

        internal static string Value
        {
            get
            {
                var lookupField = GetLookupField();
                SettingCache cache;

                if (_cacheDict.TryGetValue(lookupField, out cache))
                {
                    if (cache.NeedUpdate)
                        cache.Update();

                    return cache.Data;
                }
                else
                {
                    using (var container = new TransactionModelContainer())
                    {
                        string[] keys = lookupField.Split('.');
                        var cfg = container[keys[0], keys[1]];
                        var value = cfg.Value1;

                        cache = new SettingCache(lookupField) { CacheDate = DateTime.Now, Data = value };
                        _cacheDict.Add(lookupField, cache);

                        return value;
                    }
                }
            }
            set
            {
                using (var container = new TransactionModelContainer())
                {
                    string[] keys = GetLookupField().Split('.');
                    var cfg = container[keys[0], keys[1]];
                    cfg.Value1 = value;

                    container.SaveChanges();
                }
            }
        }

        private static string GetLookupField()
        {
            var method = new StackTrace().GetFrames().Select(x => x.GetMethod()).FirstOrDefault(x =>
                x.DeclaringType != typeof(ConfigurationManager));

            var methodName = method.Name;
            var type = method.DeclaringType.Name;

            if (methodName.StartsWith("get_") || methodName.StartsWith("set_"))
            {
                methodName = methodName.Substring(4);
            }

            return type + "." + methodName;
        }

        internal static T GetValue<T>() where T : struct
        {
            return GetValue(default(T));
        }

        internal static T GetValue<T>(T defalutValue) where T : struct
        {
            T result;

            if (TryChange<T>(Value, out result))
                return result;
            else return defalutValue;
        }

        internal static T GetValue<T>(Func<T> defalutValue) where T : struct
        {
            return GetValue<T>(defalutValue());
        }

        internal static Nullable<T> GetNullableValue<T>() where T : struct
        {
            T result;

            if (TryChange<T>(Value, out result))
                return result;
            else return null;
        }

        static bool TryChange<T>(string value, out T result)
        {
            try
            {
                result = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
    }
}
