using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class TemplateControl<T>
    {
        string file = AppDomain.CurrentDomain.BaseDirectory;
        public TemplateControl()
        {
            file += "App_Data/" + typeof(T).Name + ".json";
            EmptyFile();
        }
        void EmptyFile()
        {
            if (!File.Exists(file))
            {
                using (var sw = new StreamWriter(file, false))
                    sw.Write("[]");
            }
        }
        public List<T> Get()
        {
            EmptyFile();
            using (var sr = new StreamReader(file))
                return JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
        }

        public void Add(T obj)
        {
            var list = Get();
            dynamic d = obj;
            d.ID = list.Count;
            list.Add(d);
            using (var sw = new StreamWriter(file, false))
                sw.Write(JsonConvert.SerializeObject(list));
        }

        public void Update(int id, T obj)
        {
            var list = Get();
            list[id] = obj;
            using (var sw = new StreamWriter(file, false))
                sw.Write(JsonConvert.SerializeObject(list));
        }

        public void Delete(int id)
        {
            var list = Get();
            dynamic d = list[id];
            d.Deleted = true;
            list[id] = d;
            using (var sw = new StreamWriter(file, false))
                sw.Write(JsonConvert.SerializeObject(list));
        }

        public T Get(int id)
        {
            return Get()[id];
        }
    }
}